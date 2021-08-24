using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for SafetyExit.xaml
    /// </summary>
    public partial class SafetyExit : Window
    {
        System.Windows.Controls.RichTextBox richTextBox;
        String path;
        private MainWindow mainWindow;
        private Rabbit.Controls.RichTextBoxEx richtxtbox;
        private int p;

        public SafetyExit(Rabbit.Controls.RichTextBoxEx _richtxtbox, string _path, MainWindow _mainWindow)
        {
            // TODO: Complete member initialization
            richTextBox = _richtxtbox;
            path = _path;
            mainWindow = _mainWindow;
            InitializeComponent();
        }

        public SafetyExit(Rabbit.Controls.RichTextBoxEx _richtxtbox, string _path, MainWindow _mainWindow, int _p)
        {
            // TODO: Complete member initialization
            richTextBox = _richtxtbox;
            path = _path;
            mainWindow = _mainWindow;
            p = _p;
            InitializeComponent();
        }
        private void Button_Click_Yes(object sender, RoutedEventArgs e)
        {
            richTextBox.SelectAll();
             if (path != "")
            {
                String all_text = richTextBox.Selection.Text + Environment.NewLine;
                File.WriteAllText(path, all_text);
                this.Close();
                 if(p !=1)
                   mainWindow.Close();
            }
              //  FolderBrowserDialog fbd = new FolderBrowserDialog();
              //  fbd.ShowDialog();
              //  if (fbd.SelectedPath != "")
              //  {
              //  path = fbd.SelectedPath+ @"rivki.hep";
              //// path = @"C:\תיקיה חדשה (2)\rivki.hep";
              //      File.Create(path);
              //      String all_text = richTextBox.Selection.Text + Environment.NewLine;
              //      this.Close();
              //  }
        }
        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Close();
        }

    }
}
