using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for WindowOpen.xaml
    /// </summary>
    public partial class WindowOpen : Window
    {
        private System.Windows.Controls.TextBox nameProject;

        private Rabbit.Controls.RichTextBoxEx richtxtbox;
        private MainWindow mainWindow;

        public WindowOpen()
        {
            InitializeComponent();
        }



        public WindowOpen(System.Windows.Controls.TextBox nameProject, Rabbit.Controls.RichTextBoxEx richtxtbox, MainWindow mainWindow)
        {

            InitializeComponent();
            this.nameProject = nameProject;
            this.richtxtbox = richtxtbox;
            this.mainWindow = mainWindow;
        }

        private void Location_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            FilePath.Text = fbd.SelectedPath;
        }
        private void New_Location_Click1()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            FilePath.Text = fbd.SelectedPath;
        }

        private void New_Location_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            fbd.SelectedPath = fbd.SelectedPath.Replace('\\', '/');
            FilePath.Text = fbd.SelectedPath;

        }
      
        private void save_project(object sender, RoutedEventArgs e)
        {
            mainWindow.GridOfBreakPoint.Visibility = Visibility.Visible;
            String path = FilePath.Text + "\\" + txtFileName.Text + ".hep";
            try
            {
                if (path != "")
                {
                    File.Create(path);
                    this.Close();

                    MainWindow.path = path;
                    MainWindow.name = txtFileName.Text;
                    nameProject.Text = MainWindow.name;
                    //חדש פרוייקט -מרוקנים את הריצ טקס בוקס
                    richtxtbox.SelectAll();
                    richtxtbox.Selection.Text = "";
                    //richtxtbox.Selection.Text += "\t";
                    richtxtbox.Selection.Text += "התוכנית_הראשית";
                    richtxtbox.Selection.Text += "\n{\n\n\n\n}";
                    mainWindow.ColorOfReservedAllWords();
                    richtxtbox.Focus();
                }
                else
                    System.Windows.MessageBox.Show("נא בחר נתיב");
            }
            catch { System.Windows.MessageBox.Show("נא בחר נתיב"); }

        }
    }
}