using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;

namespace WpfApplication1
{
    class Prosses
    {
        String path;
       // public static bool flag = false;
        public static Process p = new Process();
        public void FuncToDefineWindowOfConsole(CompilerResults results, CompilerParameters parameters)
        {
            //double number;
            //Console.Write("Enter a double: ");
            //number = Convert.ToDouble(Console.ReadLine());
            //Console.WriteLine("You entered " + number.ToString());
            //Console.ReadLine();
           
            try
            {
            //הצגת תוצאות של קובץ ההרצה
            Assembly assembly = results.CompiledAssembly;
            //השמת מרחב שמות + הקלאסס של התוכנית
            Type program = assembly.GetType("First.Program");
            MethodInfo main = program.GetMethod("Main");
            main.Invoke(null, null);//מה זה עושה? ולמה נופל כשמגדירים סטרינג?
            }
            catch { }
            //חלון הפלט של התוכנית
            path = parameters.OutputAssembly;
            AppDomain ad = AppDomain.CurrentDomain;
            string bin_debug = ad.BaseDirectory;// הנתיב הרצוי של החלון בתוך הפרוייקט בתוך תיקיית הפרוייקט בתוך בין בתוך דיבאג
            string name = path.Substring(path.Length - 12, 12);//שם של החלון
            //העתקה מהחלון הדיפולטי למיקום הרצוי
            File.Copy(path, bin_debug + name);
            //פונ להגדרת פתיחת החלון
            DefineToOpenConsole(name);
        }
        private void DefineToOpenConsole(string name)
        {
            ////הגדרות להצגת החלון של הקומפילר
            //Process p = new Process();
            ////השמת שם לחלון
            //p.StartInfo.FileName = name;
            ////הגדרות לשם פתיחת החלון
            ////ללא איפשור לעשות רידירקט 
            //p.StartInfo.RedirectStandardOutput = false;
            ////ללא שימוש במעטפת ההרצה 
            //p.StartInfo.UseShellExecute = false;
            ////התחלת ריצת החלון במסך
            //p.Start();
            ////המתנה ליציאה
            //p.WaitForExit();


            ProcessStartInfo prs = new ProcessStartInfo();
            prs.FileName = name;
            p.StartInfo = prs;

            ThreadStart ths = new ThreadStart(() => p.Start());
            Thread th = new Thread(ths);
            th.Start();
        }

    }
}
