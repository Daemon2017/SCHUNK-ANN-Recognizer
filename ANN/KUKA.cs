using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using AForge.Neuro.Learning;
using AForge.Neuro;

namespace ANN
{
    public partial class SchunkANN
    {
        ActivationNetwork weightNetwork;

        double[][] inputWeight;
        double[][] outputWeight;
        string[] namesWeight;

        int weightNetworkOutputNeurons = 0;

        void CreateNetworkForWeight()
        {
            string[] numLines = File.ReadAllLines("Ideal_Output_Tactile.cfg");
            int weightNetworkInputNeurons = numLines.Length + 1;

            string[] genLines = File.ReadAllLines("Ideal_Output_Weight.cfg");
            weightNetworkOutputNeurons = genLines.Length;

            weightNetwork = new ActivationNetwork(new BipolarSigmoidFunction(0.5),
                                                 weightNetworkInputNeurons,
                                                 weightNetworkOutputNeurons);
        }

        void TrainNetworkForWeight()
        {
            inputWeight = LoadFromFile("Ideal_Input_Weight.cfg");

            weightNetwork.Randomize();

            ResilientBackpropagationLearning learning = new ResilientBackpropagationLearning(weightNetwork);
            learning.LearningRate = 0.5;

            outputWeight = LoadFromFile("Ideal_Output_Weight.cfg");

            bool needToStop = false;
            int iteration = 0;
            while (!needToStop)
            {
                double error = learning.RunEpoch(inputWeight, outputWeight);

                if (error == 0)
                {
                    break;
                }
                else if (iteration < 50000)
                {
                    iteration++;
                }
                else
                {
                    needToStop = true;
                }
            }
        }

        void TestNetworkForWeight()
        {
            int nameNumb = 0;
            double[] listObjectWeightForWeight;
            listObjectWeightForWeight = new double[weightNetworkOutputNeurons];
            int objectsRecognited = 0;

            //Тут что-то недоброе - многовато выходов
            Array.Resize(ref sensorSample, listObjectWeightForTactile.Length + 1);
            for (int i = 0; i < listObjectWeightForTactile.Length; i++)
            {
                sensorSample[i + 1] = listObjectWeightForTactile[i];
            }

            foreach (double d in weightNetwork.Compute(sensorSample))
            {
                listObjectWeightForWeight[nameNumb] = d;

                if (d >= 0.5)
                {
                    objectsRecognited++;
                }

                ResultsKukaTxtBox.Text += d.ToString();
                ResultsKukaTxtBox.Text += ": ";
                ResultsKukaTxtBox.Text += d.ToString("0.##");
                ResultsKukaTxtBox.Text += Environment.NewLine;

                nameNumb++;
            }

            double maxObjectWeight = listObjectWeightForWeight.Max();

            LoadNamesFromFile("weight");

            if ((maxObjectWeight >= 0.5) && (objectsRecognited == 1))
            {
                int indexMaxObjectWeight = Array.IndexOf(listObjectWeightForWeight,
                                                         maxObjectWeight);

                DialogResult result = MessageBox.Show("Верно ли определен объект?",
                                                      "Проверка корректности распознавания",
                                                      MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    NetworkForWeightNotRecognited();
                }

                CreateNetworkForWeight();
            }
            else
            {
                NetworkForWeightNotRecognited();
            }
        }

        void NetworkForWeightNotRecognited()
        {
            Namer f = new Namer(namesWeight);
            f.ShowDialog();

            Array.Resize(ref namesWeight, namesWeight.Length + 1);
            int nameCounter = 0;

            for (int j = 0; j < namesWeight.Length; j++)
            {
                if (f.newName != namesWeight[j])
                {
                    nameCounter++;
                }
                else
                {
                    Array.Resize(ref outputWeight, outputWeight.Length + 1);
                    outputWeight[outputWeight.Length - 1] = new double[outputWeight[0].Length];
                    outputWeight[outputWeight.Length - 1] = outputWeight[j];

                    for (int i = 0; i < outputWeight.Length; i++)
                    {
                        Array.Resize(ref outputWeight[i], outputWeight[i].Length + 1);
                        outputWeight[i][outputWeight.Length - 1] = -1;
                    }
                    SaveToFile(@"Ideal_Output_Weight.cfg", outputWeight);
                }
            }

            Array.Resize(ref inputWeight, inputWeight.Length + 1);
            inputWeight[inputWeight.Length - 1] = new double[inputWeight[0].Length];
            inputWeight[inputWeight.Length - 1] = sensorSample;
            SaveToFile(@"Ideal_Input_Weight.cfg", inputWeight);

            if (nameCounter >= namesWeight.Length)
            {
                Array.Resize(ref outputWeight, outputWeight.Length + 1);
                outputWeight[outputWeight.Length - 1] = new double[outputWeight[0].Length];

                for (int i = 0; i < outputWeight.Length; i++)
                {
                    Array.Resize(ref outputWeight[i], outputWeight[i].Length + 1);
                    outputWeight[i][outputWeight.Length - 1] = -1;
                    outputWeight[outputWeight.Length - 1][i] = -1;
                }
                outputWeight[outputWeight.Length - 1][outputWeight[outputWeight.Length - 1].Length - 1] = 1;
                SaveToFile(@"Ideal_Output_Weight.cfg", outputWeight);
            }

            namesWeight[namesWeight.Length - 1] = f.newName;

            SaveNamesToFile("weight");

            weightNetwork = null;

            CreateNetworkForWeight();
            TrainNetworkForWeight();

            MessageBox.Show("Переобучение завершено!",
                            "Готово",
                            MessageBoxButtons.OK);
        }
    }
}