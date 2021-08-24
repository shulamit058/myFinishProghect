using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WpfApplication1.HelpPages;
using WpfApplication1.Views.HelpPages;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
            //Process p = new Process();
            //p.StartInfo.FileName = "WPfApllication1.exe";
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //string outPut = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();
        }
        public void DefineVariable(object sender, RoutedEventArgs e)
        {
            WDefineVariable w = new WDefineVariable();
            w.Show();
        }
        public void DefineArr(object sender, RoutedEventArgs e)
        {
            WDefineArr w = new WDefineArr();
            w.Show();
        }
        public void DefineMat(object sender, RoutedEventArgs e)
        {
            WDefineMat w = new WDefineMat();
            w.Show();
        }
        public void ConditionStatment(object sender, RoutedEventArgs e)
        {
            WConditionStatment w = new WConditionStatment();
            w.Show();
        }
        public void DefineFor(object sender, RoutedEventArgs e)
        {
            WDefineFor w = new WDefineFor();
            w.Show();
        }

        public void InputOutput(object sender, RoutedEventArgs e)
        {
            WInputOutput w = new WInputOutput();
            w.Show();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void define_function(object sender, RoutedEventArgs e)
        {
            WFunction w = new WFunction();
            w.Show();
        }
   
    }
}
