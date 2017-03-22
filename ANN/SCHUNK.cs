using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using AForge.Neuro.Learning;
using AForge.Neuro;

namespace ANN
{
    public partial class SchunkANN : Form
    {
        ActivationNetwork tactileNetwork;

        double[][] InputTactile;
        double[][] OutputTactile;
        string[] NamesTactile;

        double[] listObjectWeightForTactile;

        int tactileNetworkOutputNeurons = 0;

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

                createNetworkForTactile();
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

            createNetworkForTactile();
            trainTactileNetwork();

            MessageBox.Show("Переобучение завершено!",
                            "Готово",
                            MessageBoxButtons.OK);
        }
    }
}