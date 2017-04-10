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

        double[][] InputWeight;
        double[][] OutputWeight;
        string[] NamesWeight;

        int weightNetworkOutputNeurons = 0;

        void createNetworkForWeight()
        {
            string[] numLines = File.ReadAllLines("Ideal_Output_Tactile.cfg");
            int weightNetworkInputNeurons = numLines.Length + 1;

            string[] genLines = File.ReadAllLines("Ideal_Output_Weight.cfg");
            weightNetworkOutputNeurons = genLines.Length;

            weightNetwork = new ActivationNetwork(new BipolarSigmoidFunction(0.5),
                                                 weightNetworkInputNeurons,
                                                 weightNetworkOutputNeurons);
        }

        void trainWeightNetwork()
        {
            InputWeight = load("Ideal_Input_Weight.cfg");

            weightNetwork.Randomize();

            ResilientBackpropagationLearning learning = new ResilientBackpropagationLearning(weightNetwork);
            learning.LearningRate = 0.5;

            OutputWeight = load("Ideal_Output_Weight.cfg");

            bool needToStop = false;
            int iteration = 0;
            while (!needToStop)
            {
                double error = learning.RunEpoch(InputWeight, OutputWeight);

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

        void showWeightNetworkResult()
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

            loadNames("weight");

            if ((maxObjectWeight >= 0.5) && (objectsRecognited == 1))
            {
                int indexMaxObjectWeight = Array.IndexOf(listObjectWeightForWeight,
                                                         maxObjectWeight);

                DialogResult result = MessageBox.Show("Верно ли определен объект?",
                                                      "Проверка корректности распознавания",
                                                      MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    weightNetworkNotRecognited();
                }

                createNetworkForWeight();
            }
            else
            {
                weightNetworkNotRecognited();
            }
        }

        void weightNetworkNotRecognited()
        {
            Namer f = new Namer(NamesWeight);
            f.ShowDialog();

            Array.Resize(ref NamesWeight, NamesWeight.Length + 1);
            int nameCounter = 0;

            for (int j = 0; j < NamesWeight.Length; j++)
            {
                if (f.newName != NamesWeight[j])
                {
                    nameCounter++;
                }
                else
                {
                    Array.Resize(ref OutputWeight, OutputWeight.Length + 1);
                    OutputWeight[OutputWeight.Length - 1] = new double[OutputWeight[0].Length];
                    OutputWeight[OutputWeight.Length - 1] = OutputWeight[j];

                    for (int i = 0; i < OutputWeight.Length; i++)
                    {
                        Array.Resize(ref OutputWeight[i], OutputWeight[i].Length + 1);
                        OutputWeight[i][OutputWeight.Length - 1] = -1;
                    }
                    save(@"Ideal_Output_Weight.cfg", OutputWeight);
                }
            }

            Array.Resize(ref InputWeight, InputWeight.Length + 1);
            InputWeight[InputWeight.Length - 1] = new double[InputWeight[0].Length];
            InputWeight[InputWeight.Length - 1] = sensorSample;
            save(@"Ideal_Input_Weight.cfg", InputWeight);

            if (nameCounter >= NamesWeight.Length)
            {
                Array.Resize(ref OutputWeight, OutputWeight.Length + 1);
                OutputWeight[OutputWeight.Length - 1] = new double[OutputWeight[0].Length];

                for (int i = 0; i < OutputWeight.Length; i++)
                {
                    Array.Resize(ref OutputWeight[i], OutputWeight[i].Length + 1);
                    OutputWeight[i][OutputWeight.Length - 1] = -1;
                    OutputWeight[OutputWeight.Length - 1][i] = -1;
                }
                OutputWeight[OutputWeight.Length - 1][OutputWeight[OutputWeight.Length - 1].Length - 1] = 1;
                save(@"Ideal_Output_Weight.cfg", OutputWeight);
            }

            NamesWeight[NamesWeight.Length - 1] = f.newName;

            saveNames("weight");

            weightNetwork = null;

            createNetworkForWeight();
            trainWeightNetwork();

            MessageBox.Show("Переобучение завершено!",
                            "Готово",
                            MessageBoxButtons.OK);
        }
    }
}