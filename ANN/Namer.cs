using System;
using System.Windows.Forms;

namespace ANN
{
    public partial class Namer : Form
    {
        public Namer(string[] data)
        {
            InitializeComponent();

            this.data = data;
        }

        string[] data;

        public string newName;
        public bool KnownObject;

        private void Namer_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(data);
        }

        public void button1_Click(object sender, EventArgs e)
        {
            newName = textBox1.Text;
            KnownObject = false;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newName = comboBox1.SelectedItem.ToString();
            KnownObject = true;
            Close();
        }
    }
}
