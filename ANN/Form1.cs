using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;

namespace ANN
{
    public partial class SchunkANN : Form
    {
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
                receive("sensor", "127.0.0.1", 4446);

                showTactileNetworkResult();
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
                receive("weight", "172.31.1.147", 30000);

                showWeightNetworkResult();
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
                Array.Resize(ref InputTactile, 1);
                InputTactile[0] = new double[486] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                save(@"Ideal_Input_Tactile.cfg", InputTactile);

                Array.Resize(ref OutputTactile, 1);
                OutputTactile[0] = new double[1] { 1.0 };
                save(@"Ideal_Output_Tactile.cfg", OutputTactile);

                Array.Resize(ref NamesTactile, 1);
                NamesTactile[0] = "Пустота";
                saveNames("tactile");
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
                Array.Resize(ref InputWeight, 1);
                InputWeight[0] = new double[2] { 0.07, 1.0 };
                save(@"Ideal_Input_Weight.cfg", InputWeight);

                Array.Resize(ref OutputWeight, 1);
                OutputWeight[0] = new double[1] { 1.0 };
                save(@"Ideal_Output_Weight.cfg", OutputWeight);

                Array.Resize(ref NamesWeight, 1);
                NamesWeight[0] = "Сидр";
                saveNames("weight");
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

        int time = 5;
        void timer1_Tick(object sender, EventArgs e)
        {
            time--;
            RecognizeSchunkBtn.Text = time.ToString();

            if (time == 0)
            {
                RecognizeSchunkBtn.Text = "Распознать";
                RecognizeSchunkBtn.Enabled = true;
                timer1.Stop();
                time = 5;
            }
        }

        private void TeachSchunkBtn_Click(object sender, EventArgs e)
        {
            tactileNetwork = null;

            createNetworkForTactile();
            trainTactileNetwork();

            MessageBox.Show("Обучение завершено!",
                            "Готово",
                            MessageBoxButtons.OK);

            RecognizeSchunkBtn.Enabled = true;
        }

        private void TeachKukaBtn_Click(object sender, EventArgs e)
        {
            weightNetwork = null;

            createNetworkForWeight();
            trainWeightNetwork();

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