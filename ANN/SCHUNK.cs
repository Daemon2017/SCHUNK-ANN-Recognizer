using System;
using System.Windows.Forms;
using System.Linq;
using AForge.Neuro.Learning;
using AForge.Neuro;
using System.Collections.Generic;
using System.IO;
using ConvNetSharp;
using ConvNetSharp.Layers;
using ConvNetSharp.Training;
using ConvNetSharp.Serialization;

namespace ANN
{
    public partial class SchunkANN
    {
        ActivationNetwork tactileNetwork;

        double[][] InputTactile;
        double[][] OutputTactile;
        string[] NamesTactile;

        double[] listObjectWeightForTactile;

        int tactileNetworkOutputNeurons = 0;

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

                    Console.WriteLine("Loss: {0} Train accuracy: {1}% Test accuracy: {2}%",
                                      loss,
                                      Math.Round(trainAccWindow.Items.Average() * 100.0, 2),
                                      Math.Round(valAccWindow.Items.Average() * 100.0, 2));

                    Console.WriteLine("Example seen: {0} Fwd: {1}ms Bckw: {2}ms",
                                      stepCount,
                                      Math.Round(trainer.ForwardTime.TotalMilliseconds, 2),
                                      Math.Round(trainer.BackwardTime.TotalMilliseconds, 2));
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
            training = LoadFile("TrainOut.txt", "Train.txt");

            // Определяем количество имеющихся примеров для обучения
            trainingBatchSize = training.Count;
        }

        private void CreateNetwork()
        {
            // Создаем сеть
            net = new Net();

            net.AddLayer(new InputLayer(inputWidth, inputHeight, inputDepth));

            net.AddLayer(new ConvLayer(5, 5, 8)
            {
                Stride = 1,
                Pad = 2
            });
            net.AddLayer(new ReluLayer());
            net.AddLayer(new PoolLayer(2, 2)
            {
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

            net.AddLayer(new FullyConnLayer(7));
            net.AddLayer(new SoftmaxLayer(7));
        }

        private void CreateTrainer()
        {
            trainer = new AdadeltaTrainer(net)
            {
                // Количество обрабатываемых образцов за заход
                BatchSize = 15,
                // Шаг понижения скорости обучения
                L2Decay = 0.001,
            };
        }

        private void TrainNetwork()
        {
            Console.WriteLine("Convolutional neural network learning...[Press any key to stop]");
            do
            {
                var sample = PrepareTrainingSample();
                TrainingStep(sample);
            } while (loss > 0.02);
        }

        private void SaveNetwork()
        {
            var json = net.ToJSON();
            System.IO.File.WriteAllText(@"WriteLines.json", json);
        }

        private void TestNetwork()
        {
            testing = Get(sensorSample);
            var testSample = PrepareTestSample();
            int currentPrediction = TestStep(testSample);
        }

        void createNetworkForTactile()
        {
            double[][] genLines = load("Ideal_Output_Tactile.cfg");
            tactileNetworkOutputNeurons = genLines[0].Length;

            tactileNetwork = new ActivationNetwork(new BipolarSigmoidFunction(0.25),
                                                    486,
                                                    250,
                                                    125,
                                                    tactileNetworkOutputNeurons);
        }

        void trainTactileNetwork()
        {
            InputTactile = load("Ideal_Input_Tactile.cfg");
            InputWeight = load("Ideal_Input_Weight.cfg");

            tactileNetwork.Randomize();

            ResilientBackpropagationLearning learning = new ResilientBackpropagationLearning(tactileNetwork);
            learning.LearningRate = 0.5;

            OutputTactile = load("Ideal_Output_Tactile.cfg");
            OutputWeight = load("Ideal_Output_Weight.cfg");

            bool needToStop = false;
            int iteration = 0;
            while (!needToStop)
            {
                double error = learning.RunEpoch(InputTactile, OutputTactile);

                if (error == 0)
                {
                    break;
                }
                else if (iteration < 1000)
                {
                    iteration++;
                }
                else
                {
                    needToStop = true;
                }
            }
        }

        void showTactileNetworkResult()
        {
            int nameNumb = 0;
            listObjectWeightForTactile = new double[tactileNetworkOutputNeurons];
            int objectsRecognited = 0;

            loadNames("tactile");

            foreach (double d in tactileNetwork.Compute(sensorSample))
            {
                listObjectWeightForTactile[nameNumb] = d;

                if (d >= 0.5)
                {
                    objectsRecognited++;
                }

                ResultsSchunkTxtBox.Text += NamesTactile[nameNumb].ToString();
                ResultsSchunkTxtBox.Text += ": ";
                ResultsSchunkTxtBox.Text += d.ToString("0.##");
                ResultsSchunkTxtBox.Text += Environment.NewLine;

                nameNumb++;
            }

            double maxObjectWeight = listObjectWeightForTactile.Max();

            if ((maxObjectWeight >= 0.5) && (objectsRecognited == 1))
            {
                int indexMaxObjectWeight = Array.IndexOf(listObjectWeightForTactile,
                                                         maxObjectWeight);

                switch (indexMaxObjectWeight)
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

                DialogResult result = MessageBox.Show("Верно ли определен объект?",
                                                      "Проверка корректности распознавания",
                                                      MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    tactileNetworkNotRecognited();
                }
            }
            else
            {
                tactileNetworkNotRecognited();
            }
        }

        void tactileNetworkNotRecognited()
        {
            Namer GetName = new Namer(NamesTactile);
            GetName.ShowDialog();

            if (GetName.KnownObject == false)
            {
                Array.Resize(ref NamesTactile, NamesTactile.Length + 1);
            }

            int nameCounter = 0;
            bool stop = false;

            for (int j = 0; j < NamesTactile.Length; j++)
            {
                if (GetName.newName != NamesTactile[j])
                {
                    nameCounter++;
                }
                else if (GetName.newName == NamesTactile[j] && stop == false)
                {
                    stop = true;

                    Array.Resize(ref OutputTactile, OutputTactile.Length + 1);
                    OutputTactile[OutputTactile.Length - 1] = new double[OutputTactile[0].Length];
                    OutputTactile[OutputTactile.Length - 1] = OutputTactile[j];

                    if (GetName.KnownObject == false)
                    {
                        for (int i = 0; i < OutputTactile.Length; i++)
                        {
                            Array.Resize(ref OutputTactile[i], OutputTactile[i].Length + 1);
                            OutputTactile[i][OutputTactile.Length - 1] = -1;
                        }
                    }
                    save(@"Ideal_Output_Tactile.cfg", OutputTactile);
                }
            }

            Array.Resize(ref InputTactile, InputTactile.Length + 1);
            InputTactile[InputTactile.Length - 1] = new double[InputTactile[0].Length];
            InputTactile[InputTactile.Length - 1] = sensorSample;
            save(@"Ideal_Input_Tactile.cfg", InputTactile);

            for (int i = 0; i < InputWeight.Length; i++)
            {
                Array.Resize(ref InputWeight[i], InputWeight[i].Length + 1);
                InputWeight[i][InputWeight.Length] = -1;
            }
            save(@"Ideal_Input_Weight.cfg", InputWeight);

            Array.Resize(ref listObjectWeightForTactile, listObjectWeightForTactile.Length + 1);
            listObjectWeightForTactile[listObjectWeightForTactile.Length - 1] = 1;

            if (nameCounter >= NamesTactile.Length)
            {
                Array.Resize(ref OutputTactile, OutputTactile.Length + 1);
                OutputTactile[OutputTactile.Length - 1] = new double[OutputTactile[0].Length];

                for (int i = 0; i < OutputTactile.Length; i++)
                {
                    Array.Resize(ref OutputTactile[i], OutputTactile[i].Length + 1);
                    OutputTactile[i][OutputTactile[0].Length - 1] = -1;
                    OutputTactile[OutputTactile.Length - 1][i] = -1;
                }
                OutputTactile[OutputTactile.Length - 1][OutputTactile[OutputTactile.Length - 1].Length - 1] = 1;
                save(@"Ideal_Output_Tactile.cfg", OutputTactile);
            }

            if (GetName.KnownObject == false)
            {
                NamesTactile[NamesTactile.Length - 1] = GetName.newName;
            }

            saveNames("tactile");

            tactileNetwork = null;
            RecognizeSchunkBtn.Enabled = false;
        }
    }
}