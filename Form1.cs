using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Выход из программы по клавише ESC
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }
        public string CompressionInfo { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            string inputText = textBox1.Text;
            string inputFilename = "input.txt";
            string outputFilename = "output.txt";

            if (string.IsNullOrEmpty(inputText))
            {
                MessageBox.Show("Входная строка пуста.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            File.WriteAllText(inputFilename, inputText); // Запись входной строки в файл
            File.WriteAllText(outputFilename, string.Empty); // Очистка содержимого выходного файла перед записью новых данных
            Huffman.BuildHuffmanTree(inputFilename, outputFilename); // Сжатие строки
            string compressedText = File.ReadAllText(outputFilename); // Чтение закодированной строки из файла
            int compressedSize = compressedText.Length; // Получение длины закодированной строки
            Form2 form2 = new Form2();
            form2.CompressionInfo = $"Закодированная строка:\n{compressedText}\n\nРазмер файла до сжатия: {inputText.Length * 8} бит\nРазмер файла после сжатия: {compressedSize} бит\nСтепень сжатия: {Math.Round((double)inputText.Length * 8 / compressedSize, 2)}";
            form2.ShowDialog();
            //MessageBox.Show($"Закодированная строка:\n{compressedText}\n\nРазмер файла до сжатия: {inputText.Length * 8} бит\nРазмер файла после сжатия: {compressedSize} бит\nСтепень сжатия: {Math.Round((double)inputText.Length * 8 / compressedSize, 2)}", "Информация о сжатии", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Создаем и отображаем вторую форму
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
