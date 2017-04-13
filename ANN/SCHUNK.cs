﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using ConvNetSharp;
using ConvNetSharp.Layers;
using ConvNetSharp.Training;

namespace ANN
{
    public partial class SchunkANN
    {
        string[] namesTactile;

        double[] listObjectWeightForTactile;

        // Храним последние оценки качества - на обучающей и пробной выборке
        private readonly CircularBuffer<double> trainAccWindow = new CircularBuffer<double>(100);
        private readonly CircularBuffer<double> valAccWindow = new CircularBuffer<double>(100);

        // Храним оценки потерь
        private readonly CircularBuffer<double> wLossWindow = new CircularBuffer<double>(100);
        private readonly CircularBuffer<double> xLossWindow = new CircularBuffer<double>(100);

        private int stepCount;

        double loss = 100;

        private Item PrepareTrainingSample()
        {
            // Выбираем случайный пример
            Random random = new Random();
            var n = random.Next(trainingBatchSize);
            var entry = training[n];

            // Приводим пример к заданному виду
            var x = new Volume(inputWidth,
                               inputHeight,
                               inputDepth,
                               0.0);
            for (var i = 0; i < inputWidth; i++)
            {
                for (var j = 0; j < inputHeight; j++)
                {
                    x.Set(j + i * inputWidth,
                          entry.Input[j + i * inputHeight]);
                }
            }

            // Раздутие делать не будем
            var result = x;

            return new Item
            {
                Input = result,
                Output = entry.Output,
                IsValidation = n % 10 == 0
            };
        }

        private void TrainingStep(Item sample)
        {
            // Оцениваем качество работы до обучения
            if (sample.IsValidation)
            {
                net.Forward(sample.Input);
                var yhat = net.GetPrediction();
                var valAcc = (yhat == sample.Output) ? 1.0 : 0.0;
                valAccWindow.Add(valAcc);
                return;
            }

            // Обучаем
            trainer.Train(sample.Input,
                          sample.Output);

            // Оцениваем потери
            var lossx = trainer.CostLoss;
            xLossWindow.Add(lossx);
            var lossw = trainer.L2DecayLoss;
            wLossWindow.Add(lossw);

            // Оцениваем качество работы после обучения
            var prediction = net.GetPrediction();
            var trainAcc = (prediction == sample.Output) ? 1.0 : 0.0;
            trainAccWindow.Add(trainAcc);

            if (stepCount % 200 == 0)
            {
                if (xLossWindow.Count == xLossWindow.Capacity)
                {
                    var xa = xLossWindow.Items.Average();
                    var xw = wLossWindow.Items.Average();
                    loss = xa + xw;

                    toolStripStatusLabel1.Text = string.Format("Потери: {0}", loss);
                    statusStrip1.Refresh();
                }
            }

            stepCount++;
        }

        private Item PrepareTestSample()
        {
            var entry = testing[0];

            // Приводим пример к заданному виду
            var x = new Volume(inputWidth,
                               inputHeight,
                               inputDepth,
                               0.0);
            for (var i = 0; i < inputWidth; i++)
            {
                for (var j = 0; j < inputHeight; j++)
                {
                    x.Set(j + i * inputWidth,
                          entry.Input[j + i * inputHeight]);
                }
            }

            // Раздутие делать не будем
            var result = x;

            return new Item
            {
                Input = result,
                Output = entry.Output,
                IsValidation = false
            };
        }

        private int TestStep(Item sample)
        {
            net.Forward(sample.Input);
            var yhat = net.GetPrediction();
            return yhat;
        }

        public static List<Entry> LoadFile(string outputFile, string inputFile, int maxItem = -1)
        {
            double[][] InputTactile = LoadData(inputFile);
            List<double[]> inputs = new List<double[]>();
            for (int i = 0; i < InputTactile.Length; i++)
            {
                inputs.Add(InputTactile[i]);
            }

            double[][] OutputTactile = LoadData(outputFile);
            List<double> output = new List<double>();
            for (int i = 0; i < OutputTactile.GetLength(0); i++)
            {
                output.Add(OutputTactile[i][0]);
            }

            if (output.Count == 0 || inputs.Count == 0)
            {
                return new List<Entry>();
            }

            return output.Select((t, i) => new Entry
            {
                Output = t,
                Input = inputs[i]
            }).ToList();
        }

        public static List<Entry> Get(double[] inputData, int maxItem = -1)
        {
            List<double[]> inputs = new List<double[]>();
            inputs.Add(inputData);

            List<double> output = new List<double>();
            output.Add(0);

            return output.Select((t, i) => new Entry
            {
                Output = t,
                Input = inputs[i]
            }).ToList();
        }

        static double[][] LoadData(string fileName)
        {
            string[] genLines = File.ReadAllLines(fileName);

            double[][] temp = null;

            Array.Resize(ref temp, genLines.Length);

            for (int i = 0; i < genLines.Length; i++)
            {
                string[] genTemp = genLines[i].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                Array.Resize(ref genTemp, genTemp.Length);
                Array.Resize(ref temp[i], genTemp.Length);

                for (int j = 0; j < genTemp.Length; j++)
                {
                    if (double.TryParse(genTemp[j], out temp[i][j])) { }
                }
            }

            return temp;
        }

        private void PrepareData()
        {
            // Загружаем наборы данных для обучения и проверки
            training = LoadFile("Ideal_Output_Tactile.cfg", "Ideal_Input_Tactile.cfg");

            // Определяем количество имеющихся примеров для обучения
            trainingBatchSize = training.Count;

            //Загружаем названия объектов
            names = File.ReadAllLines("Names_Tactile.cfg");
        }

        private void CreateNetworkForTactile()
        {
            // Создаем сеть
            net = new Net();

            net.AddLayer(new InputLayer(inputWidth, inputHeight, inputDepth));

            // Ширина, высота и глубина фильтра
            net.AddLayer(new ConvLayer(5, 5, 8)
            {
                // Шаг скольжения свертки
                Stride = 1,
                // Заполнение краев нулями
                Pad = 2
            });
            net.AddLayer(new ReluLayer());

            // Ширина и высота окна уплотнения
            net.AddLayer(new PoolLayer(2, 2)
            {
                // Сдвиг
                Stride = 2
            });

            net.AddLayer(new ConvLayer(5, 5, 16)
            {
                Stride = 1,
                Pad = 2
            });
            net.AddLayer(new ReluLayer());

            net.AddLayer(new PoolLayer(3, 3)
            {
                Stride = 3
            });

            net.AddLayer(new FullyConnLayer(names.Length));
            net.AddLayer(new SoftmaxLayer(names.Length));
        }

        private void TrainNetworkForTactile(double availableLoss)
        {
            trainer = new AdadeltaTrainer(net)
            {
                // Количество обрабатываемых образцов за заход
                BatchSize = 15,
                // Шаг понижения скорости обучения
                L2Decay = 0.001,
            };

            do
            {
                var sample = PrepareTrainingSample();
                TrainingStep(sample);
            } while (loss > availableLoss);
        }

        private void TestNetworkForTactile()
        {
            testing = Get(sensorSample);
            var testSample = PrepareTestSample();
            int currentPrediction = TestStep(testSample);

            ResultsSchunkTxtBox.Text = names[currentPrediction];

            switch(currentPrediction)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }
        }
    }
}