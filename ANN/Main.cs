using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
using ConvNetSharp;
using ConvNetSharp.Training;
using ConvNetSharp.Serialization;
using System.IO;

namespace ANN
{
    public partial class SchunkANN : Form
    {
        private int trainingBatchSize;

        private Net net;
        private AdadeltaTrainer trainer;

        private List<Entry> training;
        private List<Entry> testing;

        string[] names;

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

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawFinger();
        }

        private void trainingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            net = null;

            PrepareData();
            CreateNetworkForTactile();
            TrainNetworkForTactile(0.02);

            testingToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;

            MessageBox.Show("Обучение завершено!",
                            "Готово",
                            MessageBoxButtons.OK);
        }

        private void testingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultSchunkPicBox.Image = null;
            ResultsSchunkTxtBox.Text = null;

            try
            {
                ReceiveTCP("sensor", "127.0.0.1", 4446);

                TestNetworkForTactile();
            }
            catch (SocketException)
            {
                ResultsSchunkTxtBox.Text = "Ошибка приёма данных!";
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            net = null;
            var json_temp = File.ReadAllLines("NetworkStructure.json");
            string json = string.Join("", json_temp);
            net = SerializationExtensions.FromJSON(json);

            testingToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var json = net.ToJSON();
            File.WriteAllText(@"NetworkStructure.json", json);
        }

        private void showInputsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Input_Tactile.cfg");
        }

        private void showOutputsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Output_Tactile.cfg");
        }

        private void showNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Names_Tactile.cfg");
        }

        private void trainingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            weightNetwork = null;

            CreateNetworkForWeight();
            TrainNetworkForWeight();

            testingToolStripMenuItem1.Enabled = true;

            MessageBox.Show("Обучение завершено!",
                            "Готово",
                            MessageBoxButtons.OK);
        }

        private void testingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResultsKukaTxtBox.Text = null;

            try
            {
                ReceiveTCP("weight", "172.31.1.147", 30000);

                TestNetworkForWeight();
            }
            catch (SocketException)
            {
                ResultsKukaTxtBox.Text = "Ошибка приёма данных!";
            }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void inputsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Input_Weight.cfg");
        }

        private void outputsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Ideal_Output_Weight.cfg");
        }

        private void namesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Names_Weight.cfg");
        }
    }
}