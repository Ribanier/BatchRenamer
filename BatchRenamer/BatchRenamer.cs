using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatchRenamer
{
    public partial class BatchRenamer : Form
    {
        public BatchRenamer()
        {
            InitializeComponent();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) listBox1.Items.Add(file);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "start from";
            textBox1.Enabled = false;
            numericUpDown1.Enabled = true;
            numericUpDown1.Value = 1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "letter name";
            textBox1.Enabled = false;
            numericUpDown1.Enabled = true;
            numericUpDown1.Value = 5;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            numericUpDown1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog(this);
            if (openFileDialog.FileName != "")
            { foreach (string file in openFileDialog.FileNames) listBox1.Items.Add(file); }

        }

        private void button2_Click(object sender, EventArgs e)
        {


            listBox1.Items.Clear();
        }
        public string RandomName(int wordLenght)
        {
            Random rnd = new Random();
            string word = "";
            string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            for (int i = 0; i < wordLenght; i++)
            {
                word += letters[rnd.Next(letters.Length)];
            }
            return word;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count > 0)
                {
                    progressBar1.Maximum = listBox1.Items.Count;
                    if (radioButton1.Checked)
                    {
                        foreach (string file in listBox1.Items)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            int numericalValue = decimal.ToInt32(numericUpDown1.Value);
                            string word = "";
                            if (numericalValue > 0)
                            {
                                word = RandomName(numericalValue);
                                while (File.Exists(fileInfo.DirectoryName + "/" + word + fileInfo.Extension))
                                { word = RandomName(numericalValue); }
                            }


                            string newName = fileInfo.DirectoryName + "/" + word + fileInfo.Extension;
                            System.IO.File.Move(file, newName);
                            listBox2.Items.Add(newName);
                            listBox2.TopIndex = listBox2.Items.Count - 1;
                            Application.DoEvents();
                            listBox2.Refresh();

                            progressBar1.Value++;
                        }
                        progressBar1.Value = 0;
                        MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(listBox2.Items.Cast<object>().ToArray());
                        listBox2.Items.Clear();

                    }
                    else if (radioButton2.Checked)
                    {
                        foreach (string file in listBox1.Items)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            int numericalValue = decimal.ToInt32(numericUpDown1.Value);
                            while (File.Exists(fileInfo.DirectoryName + "/" + numericalValue + fileInfo.Extension))
                                numericalValue++;

                            string newName = fileInfo.DirectoryName + "/" + numericalValue++ + fileInfo.Extension;
                            System.IO.File.Move(file, newName);
                            listBox2.Items.Add(newName);
                            listBox2.TopIndex = listBox2.Items.Count - 1;
                            Application.DoEvents();
                            listBox2.Refresh();

                            progressBar1.Value++;
                        }
                        progressBar1.Value = 0;
                        MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(listBox2.Items.Cast<object>().ToArray());
                        listBox2.Items.Clear();
                    }
                    else
                    {

                    }
                }
            }
            catch(Exception error ){
                MessageBox.Show(error.ToString(),"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
