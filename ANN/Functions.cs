using System;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace ANN
{
    public partial class Form1 : Form
    {
        double[] sensorSample;

        void save(string fileName, double[][] variable)
        {
            using (StreamWriter gg = new StreamWriter(fileName))
            {
                for (int i = 0; i < variable.Length; i++)
                {
                    for (int j = 0; j < variable[i].Length; j++)
                    {
                        gg.Write(variable[i][j] + ";");
                    }
                    gg.Write("\r\n");
                }
                gg.Close();
            }
        }

        void saveNames(string type)
        {
            string WayToFile = "0";
            int Length = 0;
            string[] ArrayToWrite = { };

            if (type == "tactile")
            {
                WayToFile = @"Names_Tactile.cfg";
                Length = NamesTactile.Length;
                ArrayToWrite = NamesTactile;
            }
            else if (type == "weight")
            {
                WayToFile = @"Names_Weight.cfg";
                Length = NamesWeight.Length;
                ArrayToWrite = NamesWeight;
            }

            using (StreamWriter gg = new StreamWriter(WayToFile))
            {
                for (int i = 0; i < Length; i++)
                {
                    gg.Write(ArrayToWrite[i]);
                    gg.Write("\r\n");
                }
                gg.Close();
            }
        }

        void loadNames(string type)
        {
            if (type == "tactile")
            {
                string[] kitLines = File.ReadAllLines("Names_Tactile.cfg");

                Array.Resize(ref NamesTactile, kitLines.Length);

                for (int i = 0; i < kitLines.Length; i++)
                {
                    NamesTactile[i] = kitLines[i];
                }
            }
            else if (type == "weight")
            {
                string[] kitLines = File.ReadAllLines("Names_Weight.cfg");

                Array.Resize(ref NamesWeight, kitLines.Length);

                for (int i = 0; i < kitLines.Length; i++)
                {
                    NamesWeight[i] = kitLines[i];
                }
            }
        }

        void DrawFinger(int PictureNumber, int StartCoordX, int StartCoordY, int i_end)
        {
            int SizeX = 10,
                SizeY = 10;

            for (int i = 0; i < i_end; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Finger[PictureNumber] = new PictureBox();
                    Finger[PictureNumber].Location = new Point(StartCoordX + j * SizeX + j,
                                                               StartCoordY + i * SizeY + i);
                    Finger[PictureNumber].Size = new Size(SizeX, SizeY);
                    Finger[PictureNumber].BackColor = Color.FromArgb(0, 0, 0); ;
                    Finger[PictureNumber].Visible = true;

                    Controls.Add(Finger[PictureNumber]);

                    PictureNumber++;
                }
            }
        }

        void UpdateFinger(int i_start, int i_end)
        {
            for (int i = i_start; i < i_end; i++)
            {
                double ColorNormalization = sensorSample[i] * 255 / 4000;

                Finger[i].BackColor = Color.FromArgb(0, Convert.ToInt32(ColorNormalization), 0); ;

                Finger[i].Invalidate();
                Finger[i].Update();
            }
        }

        void receive(string type, string ip, int port)
        {
            TcpClient client = new TcpClient(ip,
                                             port);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead,
                                          0,
                                          client.ReceiveBufferSize);
            string input_data = Encoding.ASCII.GetString(bytesToRead,
                                                         0,
                                                         bytesRead);

            if (type == "sensor")
            {
                button1.Enabled = false;
                timer1.Start();
            }

            string output_data;
            string[] massive = { "" };

            if (type != "weight")
            {
                output_data = input_data.Split('[', ']')[1];
                massive = output_data.Split(',');
            }

            switch (type)
            {
                case "sensor":
                    sensorSample = new double[486 + 7];

                    for (int i = 0; i < 486; i++)
                    {
                        sensorSample[i] = double.Parse(massive[i]);
                    }

                    UpdateFinger(0, 84);
                    UpdateFinger(84, 162);
                    UpdateFinger(162, 246);
                    UpdateFinger(246, 324);
                    UpdateFinger(324, 408);
                    UpdateFinger(408, 486);

                    for (int i = 0; i < sensorSample.Length; i++)
                    {
                        sensorSample[i] = sensorSample[i] / 4000.0;
                    }
                    break;

                case "angles":
                    double[] anglesSample = new double[7];

                    for (int i = 0; i < 7; i++)
                    {
                        anglesSample[i] = double.Parse(massive[i], CultureInfo.InvariantCulture);
                    }

                    for (int i = 0; i < anglesSample.Length; i++)
                    {
                        sensorSample[486 + i] = anglesSample[i] / 92.0;
                    }
                    break;

                case "weight":
                    sensorSample = new double[1];

                    sensorSample[0] = double.Parse(input_data, CultureInfo.InvariantCulture);
                    sensorSample[0] = sensorSample[0] / 7;
                    break;
            }
        }

        double[][] load(string fileName)
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
    }
}