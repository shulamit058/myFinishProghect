using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WpfApplication1
{
   public class CodeCompiler
    {
        //מכיל את כל הקוד הכתוב בריצ טקסט בוקס 
        static public String code = "";
        //הריצ טקסט בוקס שעל המסך
        System.Windows.Controls.RichTextBox richtextbox;
        public CodeCompiler(System.Windows.Controls.RichTextBox _richtextbox)
        {
            //השמה של הריצ טקסט בוקס שעל המסך
            richtextbox = _richtextbox;
        }
        public void CodeCompiler1(String allnewstring )
        {
            allnewstring = allnewstring.Remove(allnewstring.LastIndexOf('}'));
            allnewstring = string.Concat(allnewstring, "  Console.ReadLine();"+ "\n" + "}");
            
            
            code = @"using System; " + "\n" +@"using System.IO;"+"\n"+
                     @"namespace First  " + "\n" +
                      @"{   " + "\n" +
                     @"public class Program  " + "\n" +
                      @"{ " + "\n" 
                    +allnewstring + "\n"+
                     @"}" + "\n" +
                      @"}";
             }
        public bool CodeCompiler2(String allnewstring, System.Windows.Controls.ListBox listBoxError, ListBoxItem listboxTitle,MainWindow mainWindow)
        {
   
            //מכיל את הפרטים של קובץ ההרצה של הקומפילר 
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //אובייקט שמאתחל את הפרמטרים של הפרווידר   
            CompilerParameters parameters = new CompilerParameters();
            //מוסיף ייחווס לקובץ מערכת
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            //מעדכן את הקומפילר שהקובץ אכן מוכן לריצה 
            parameters.GenerateExecutable = true;
            //אובייקט שממיר את הקובץ לאסמבלי
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
           
            //ניקוי כל השגיאות
            listBoxError.Items.Clear();
            //כותרת - של שגיאות 
            listBoxError.Items.Add(listboxTitle);
            //מעבר על רשימת הארור של אובייקט ריזלט ותרגומן לעברית 
            if (results.Errors.HasErrors)
            {
                CError e = new CError(richtextbox);
                e.FuncInCaseError(listBoxError, listboxTitle, results);
                return false;
            }
            else
            {
                return true;
                ////MainWindow m = new MainWindow();
                //mainWindow.Continue_running();
                ////continue_running.Visibility = Visibility.Visible;
                //Prosses p = new Prosses();
                //p.FuncToDefineWindowOfConsole(results, parameters);
            }
}
        public void CodeCompiler3(String allnewstring, System.Windows.Controls.ListBox listBoxError, ListBoxItem listboxTitle, MainWindow mainWindow)
        {

            //מכיל את הפרטים של קובץ ההרצה של הקומפילר 
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //אובייקט שמאתחל את הפרמטרים של הפרווידר   
            CompilerParameters parameters = new CompilerParameters();
            //מוסיף ייחווס לקובץ מערכת
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            //מעדכן את הקומפילר שהקובץ אכן מוכן לריצה 
            parameters.GenerateExecutable = true;
            //אובייקט שממיר את הקובץ לאסמבלי
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

            //ניקוי כל השגיאות
            listBoxError.Items.Clear();
            
                
                //MainWindow m = new MainWindow();
                mainWindow.Continue_running();
                //continue_running.Visibility = Visibility.Visible;
                Prosses p = new Prosses();
                p.FuncToDefineWindowOfConsole(results, parameters);
            }
      //public void CodeCompiler4(String allnewstring, System.Windows.Controls.ListBox listBoxError, ListBoxItem listboxTitle, MainWindow mainWindow)
      //  {

      //      //מכיל את הפרטים של קובץ ההרצה של הקומפילר 
      //      CSharpCodeProvider provider = new CSharpCodeProvider();
      //      //אובייקט שמאתחל את הפרמטרים של הפרווידר   
      //      CompilerParameters parameters = new CompilerParameters();
      //      //מוסיף ייחווס לקובץ מערכת
      //      parameters.ReferencedAssemblies.Add("System.Drawing.dll");
      //      //מעדכן את הקומפילר שהקובץ אכן מוכן לריצה 
      //      parameters.GenerateExecutable = true;
      //      //אובייקט שממיר את הקובץ לאסמבלי
      //      CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

      //      //ניקוי כל השגיאות
      //      listBoxError.Items.Clear();
      //      //כותרת - של שגיאות 
      //      listBoxError.Items.Add(listboxTitle);
      //      //מעבר על רשימת הארור של אובייקט ריזלט ותרגומן לעברית 
      //      if (results.Errors.HasErrors)
      //      {
      //          CError e = new CError(richtextbox);
      //          e.FuncInCaseError(listBoxError, listboxTitle, results);
      //      }
      //      else
      //      {
      //          //MainWindow m = new MainWindow();
      //          mainWindow.Continue_running();
      //          //continue_running.Visibility = Visibility.Visible;
      //          Prosses p = new Prosses();
      //          p.FuncToDefineWindowOfConsole(results, parameters);
      //      }
      //  }
       //source
           public void CodeCompiler5(String allnewstring, System.Windows.Controls.ListBox listBoxError, ListBoxItem listboxTitle)
        {
   
            //מכיל את הפרטים של קובץ ההרצה של הקומפילר 
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //אובייקט שמאתחל את הפרמטרים של הפרווידר   
            CompilerParameters parameters = new CompilerParameters();
            //מוסיף ייחווס לקובץ מערכת
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            //מעדכן את הקומפילר שהקובץ אכן מוכן לריצה 
            parameters.GenerateExecutable = true;
            //אובייקט שממיר את הקובץ לאסמבלי 
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            //ניקוי כל השגיאות
            listBoxError.Items.Clear();
            //כותרת - של שגיאות 
            listBoxError.Items.Add(listboxTitle);
            //מעבר על רשימת הארור של אובייקט ריזלט ותרגומן לעברית 
            if (results.Errors.HasErrors)
            {
                CError e = new CError(richtextbox);
                e.FuncInCaseError(listBoxError, listboxTitle, results);
            }
            else
            {
                Prosses p = new Prosses();
                p.FuncToDefineWindowOfConsole(results, parameters);
            }
}
        }
       
        }
    


