using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
using ConvNetSharp;
using ConvNetSharp.Training;

namespace ANN
{
    public partial class SchunkANN : Form
    {
        private int trainingBatchSize;

        private Net net;
        private AdadeltaTrainer trainer;

        private List<Entry> training;
        private List<Entry> testing;

        // Ширина изображения
        int inputWidth = 18;
        // Высота изображения
        int inputHeight = 27;
        // Число каналов у изображения
        int inputDepth = 1;

        public PictureBox[] Finger = new PictureBox[486];

        public SchunkANN()
        {
            InitializeComponent();
        }

        void RecognizeSchunkBtn_Click(object sender, EventArgs e)
        {
            ResultSchunkPicBox.Image = null;
            ResultsSchunkTxtBox.Text = null;

            try
            {
                Receive("sensor", "127.0.0.1", 4446);

                TestNetwork();

                //showTactileNetworkResult();
            }
            catch (SocketException)
            {
                ResultsSchunkTxtBox.Text = "Ошибка приёма данных!";
            }

            TeachKukaBtn.Enabled = true;
        }

        void RecognizeKukaBtn_Click(object sender, EventArgs e)
        {
            ResultsKukaTxtBox.Text = null;

            try
            {
                Receive("weight", "172.31.1.147", 30000);

                ShowWeightNetworkResult();
            }
            catch (SocketException)
            {
                ResultsKukaTxtBox.Text = "Ошибка приёма данных!";
            }
        }

        void ClearSchunkBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить все изученные объекты из памяти ИНС?",
                                         "Очистка памяти ИНС",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Array.Resize(ref inputTactile, 1);
                inputTactile[0] = new double[486] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                Save(@"Ideal_Input_Tactile.cfg", inputTactile);

                Array.Resize(ref outputTactile, 1);
                outputTactile[0] = new double[1] { 1.0 };
                Save(@"Ideal_Output_Tactile.cfg", outputTactile);

                Array.Resize(ref namesTactile, 1);
                namesTactile[0] = "Пустота";
                SaveNames("tactile");
            }

            tactileNetwork = null;

            RecognizeSchunkBtn.Enabled = false;
        }

        void ClearKukaBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить все изученные объекты из памяти ИНС?",
                                         "Очистка памяти ИНС",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Array.Resize(ref inputWeight, 1);
                inputWeight[0] = new double[2] { 0.07, 1.0 };
                Save(@"Ideal_Input_Weight.cfg", inputWeight);

                Array.Resize(ref outputWeight, 1);
                outputWeight[0] = new double[1] { 1.0 };
                Save(@"Ideal_Output_Weight.cfg", outputWeight);

                Array.Resize(ref namesWeight, 1);
                namesWeight[0] = "Сидр";
                SaveNames("weight");
            }
        }

        void InputsSchunkBtn_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Input_Tactile.cfg");
        }

        private void InputsKukaBtn_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Input_Weight.cfg");
        }

        void OutputsSchunkBtn_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Output_Tactile.cfg");
        }

        void OutputsKukaBtn_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Output_Weight.cfg");
        }

        void NamesSchunkBtn_Click(object sender, EventArgs e)
        {
            Process.Start(@"Names_Tactile.cfg");
        }

        void NamesKukaBtn_Click(object sender, EventArgs e)
        {
            Process.Start(@"Names_Weight.cfg");
        }

        private void TeachSchunkBtn_Click(object sender, EventArgs e)
        {
            net = null;
            PrepareData();
            CreateNetwork();
            CreateTrainer();
            TrainNetwork();
            SaveNetwork();

            /*
            tactileNetwork = null;
            createNetworkForTactile();
            trainTactileNetwork();
            */

            MessageBox.Show("Обучение завершено!",
                "Готово",
                MessageBoxButtons.OK);
            RecognizeSchunkBtn.Enabled = true;
        }

        private void TeachKukaBtn_Click(object sender, EventArgs e)
        {
            weightNetwork = null;

            CreateNetworkForWeight();
            TrainWeightNetwork();

            MessageBox.Show("Обучение завершено!",
                            "Готово",
                            MessageBoxButtons.OK);

            RecognizeKukaBtn.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawFinger();
        }
    }
}