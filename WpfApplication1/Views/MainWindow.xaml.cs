using Microsoft.Win32;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;

namespace WpfApplication1
{
     
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           WindowState = WindowState.Maximized;
            InitializeComponent();
            InitRichTextBoxIntellisenseTrigger();
            InitRichTextBoxSource();
            richtxtbox.Focus();
            DataContext = this;
           
           }

                
         
     Conversion conversion;
        public static string path = "";
        public static string name = " ";
        public static bool activate_color = true; 
        public List<String> ContentAssistSource
        {
            get { return (List<String>)GetValue(ContentAssistSourceProperty); }
            set { SetValue(ContentAssistSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentAssisteSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistSourceProperty =
            DependencyProperty.Register("ContentAssistSource", typeof(List<String>), typeof(MainWindow), new UIPropertyMetadata(new List<string>()));


        public List<char> ContentAssistTriggers
        {
            get { return (List<char>)GetValue(ContentAssistTriggersProperty); }
            set { SetValue(ContentAssistTriggersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentAssistTriggers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistTriggersProperty =
            DependencyProperty.Register("ContentAssistTriggers", typeof(List<char>), typeof(MainWindow), new UIPropertyMetadata(new List<char>()));

      
        private void InitRichTextBoxIntellisenseTrigger()
        {
            ContentAssistTriggers.Add('ב');
            ContentAssistTriggers.Add('כ');
            ContentAssistTriggers.Add('מ');
            ContentAssistTriggers.Add('ת');
            ContentAssistTriggers.Add('ה');
            ContentAssistTriggers.Add('ל');
            ContentAssistTriggers.Add('ק');
            ContentAssistTriggers.Add('פ');
            ContentAssistTriggers.Add('א');
        }

        private void InitRichTextBoxSource()
        {
            List<Command> words = CommandList.CommandList1();
            words.ForEach(x => ContentAssistSource.Add(x.Hebrew));

        }
        private void Compiler_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.IndexOfTheLastBreakPoint == -1)//איפוס הקבצים אם זה הרצה ראשונה
            {
                 File.WriteAllText(@"..\..\FileForScanning.txt", "");//איפוס הקובץ ששומר על הקלטים
                 File.WriteAllText(@"..\..\FileToShowVars.txt", "");
                 File.WriteAllText(@"..\..\VarsForBreakPoint.txt", "");
            }
            listBoxError.Items.Clear();
            //הוספת הכותרת של רשימת שגיאות
            //listBoxError.Items.Add(TitleError);
            CError.Paragraphs.ForEach(x => x.Background = Brushes.White);
            //יצירת אובייקט של מחלקת ההמרה 
            conversion = new Conversion(listBoxError, richtxtbox);
            //סימון כל הטקסט של הריצ טקסט בוקס
            richtxtbox.SelectAll();
            //שליחה לפונקציה הראשית של תחילת ההמרה 
            try
            {
                //continue_running.Visibility = Visibility.Visible;

               // GetBreakPoints(GridOfBreakPoint, null);
                conversion.MiniSpace(richtxtbox.Selection.Text, true,this,flags);
               
            }
            catch (Exception er)
            {
            }
        }
        public void Continue_running()
        {
            continue_running.Visibility = Visibility.Visible;
        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Views.Open open = new Views.Open();
            open.Show();

        }
       
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void Print_Program(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
            printDialog.ShowDialog();
        }
        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            richtxtbox.SelectAll();
        }
        private void undoToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            richtxtbox.Undo();
        }
        private void RedoToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            richtxtbox.Redo();
        }

        public string AddSpaceInThisCase1(string strSource)
        {
            // כל זה זה בשביל שהיה רווחים בכל המקרים הללו 
            String[] stringContainsChar = { ")", "(", "++", "--", "==", ";", ",", "]", "[", "}", "{", '"'.ToString()};
            for (int i = 0; i < stringContainsChar.Length; i++)
                if (strSource.Contains(stringContainsChar[i]))
                    //פנ שמחליפה במחורזת בכל מקום שיש את מחורת 1 מחליפה בשניה
                    strSource = strSource.Replace(stringContainsChar[i], " " + stringContainsChar[i] + " ");
            return strSource;
        }
        private string AddSpaceInThisCase2(string strSource)
        {
            int IndexEquals;
            Char[] charContainsChar = { '=', '+', '-' ,'/'};
            for (int i = 0; i < charContainsChar.Length; i++)
            {
                if (strSource.Contains(charContainsChar[i]))
                {
                    IndexEquals = strSource.IndexOf(charContainsChar[i]);
                    if ((IndexEquals == 0 && strSource.Length > IndexEquals + 1 && strSource[IndexEquals + 1] != charContainsChar[i])
                        || (IndexEquals > 0 && strSource[IndexEquals - 1] != charContainsChar[i] && ((strSource.Length > IndexEquals + 1)
                    && strSource[IndexEquals + 1] != charContainsChar[i]) || strSource.Length - 1 <= IndexEquals))
                        strSource = strSource.Replace(charContainsChar[i].ToString(), " " + charContainsChar[i].ToString() + " ");
                }
            }
            return strSource;
        }
        private void richtxtbox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
                activate_color = true;
                
            ColorOfReservedWords();
        }

        public void ColorOfReservedWords()
        {
            try
            {
                if (activate_color)
                {
                    InlineCollection all_run_now = richtxtbox.CaretPosition.Paragraph.Inlines;
                    List<Inline> leaInline = all_run_now.ToList<Inline>();
                    Run end = richtxtbox.CaretPosition.Paragraph.Inlines.LastInline as Run;
                    Run start = richtxtbox.CaretPosition.Paragraph.Inlines.FirstInline as Run;
                    List<Run> runleaListOld = vgg(leaInline);
                    List<Command> words;
                    words = CommandList.CommandList1();
                    int count = runleaListOld.Count;
                    List<Run> runleaListNew = new List<Run>();
                    for (int j = 0; j < runleaListOld.Count; j++)
                    {
                        Run f = runleaListOld[j];
                        if (runleaListOld[j] != null)
                        {
                            if (runleaListOld[j].Text.Contains(' '))
                            {
                                runleaListOld[j].Text = AddSpaceInThisCase1(runleaListOld[j].Text);
                                runleaListOld[j].Text = AddSpaceInThisCase2(runleaListOld[j].Text);
                                runleaListOld[j].Text = runleaListOld[j].Text;

                                String[] str = runleaListOld[j].Text.Split(' ');//כל שורה=מחרוזת  


                                bool flag_ = false;
                                for (int i = 0; i < str.Length; i++)
                                {
                                    if (str[i] != "")
                                    {
                                        f = new Run();
                                        string text = str[i];
                                        //  f.Text = text.Length < 3 ? "  "+" "+" "+ text    : text; 
                                        f.Text = text;
                                        if (words.Where(w => w.Hebrew.Equals(str[i])).Any())
                                        {
                                            //שהיה לכל אחד לפי מה שמגדור טבלה
                                            f.Foreground = words.Where(w => w.Hebrew.Equals(str[i])).First().Color;
                                            flag_ = true;
                                        }
                                        else

                                            f.Foreground = Brushes.Black;
                                        if ((f.Foreground == Brushes.Black) && (runleaListNew.Count > 0) && (runleaListNew.Last().Foreground == Brushes.Black))
                                        {


                                            Run last = runleaListNew.Last();
                                            runleaListNew.Remove(runleaListNew.Last());
                                            if (last.Text == "   " || last.Text == " ")
                                                f.Text = last.Text + f.Text;
                                            else
                                                f.Text = last.Text + f.Text;
                                        }
                                        runleaListNew.Add(f);


                                        //if (runleaListOld[j].Text.Contains(' '))
                                        if (flag_ || runleaListOld[j].Text.Contains(' '))
                                        {
                                            f = new Run();
                                            f.Text = " " + ' ' + " ";
                                            f.Foreground = Brushes.Black;
                                            runleaListNew.Add(f);
                                            flag_ = false;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                runleaListNew.Add(runleaListOld[j]);
                                if (words.Where(w => w.Hebrew.Equals(runleaListOld[j].Text)).Any())
                                {
                                    f = new Run();
                                    f.Text = " " + ' ' + " ";
                                    f.Foreground = Brushes.Black;
                                    runleaListNew.Add(f);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < runleaListOld.Count; i++)
                    {

                        richtxtbox.CaretPosition.Paragraph.Inlines.Remove(runleaListOld[i]);
                    }
                    for (int i = 0; i < runleaListNew.Count; i++)
                    {
                        richtxtbox.CaretPosition.Paragraph.Inlines.Add(runleaListNew[i]);

                        richtxtbox.CaretPosition = richtxtbox.CaretPosition.GetPositionAtOffset
                            (runleaListNew[i].Text.Length + 2, LogicalDirection.Backward);
                    }
                }
            }
            catch (Exception)
            {


            }
          
        }
        private List<Run> vgg(List<Inline> leaaaaaaaaaaaaaaaa)
        {
            List<Run> runlea = new List<Run>();
            Run start_run = richtxtbox.CaretPosition.Paragraph.Inlines.FirstInline as Run;
            for (int i = 0; i < leaaaaaaaaaaaaaaaa.Count; i++)
            {
                runlea.Add(start_run);
                if (start_run != null)
                    start_run = start_run.NextInline as Run;
            }
            return runlea;


        }

        //private void condition_Selected(object sender, RoutedEventArgs e)
        //{
        //    String stringWroteUser = richtxtbox.Selection.Text;
        //    richtxtbox.Selection.Text = stringWroteUser + conditionTB.Text;
        //    ColorOfReservedWords();
        //    activate_color = false;
        //}

        private void English_Click(object sender, RoutedEventArgs e)
        {
            if (!Only_Compiler_Click1(null, null))
                return;
            English_Code.Visibility = Visibility.Collapsed;
            Visual_Studio_Code.Visibility = Visibility.Visible;
            Hebrew_Code.Visibility = Visibility.Visible;
            conversion = new Conversion(listBoxError, richtxtbox);
            //סימון כל הטקסט של הריצ טקסט בוקס
            richtxtbox.SelectAll();

           // GetBreakPoints(GridOfBreakPoint, null);
            //שליחה לפונקציה הראשית של תחילת ההמרה 

            conversion.MiniSpace(richtxtbox.Selection.Text, false,this,flags);

            richtxtbox.SelectAll();
            all_Text = richtxtbox.Selection.Text;
            scroll_hebrew.Visibility = Visibility.Collapsed;
            scroll_english.Visibility = Visibility.Visible;
            richtxtbox.Visibility = Visibility.Collapsed;
            inEnglish.Visibility = Visibility.Visible;
            inEnglish.SelectAll();

            inEnglish.Selection.Text = CodeCompiler.code;
        }

        private void UndoCommit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String stringWroteUser = richtxtbox.Selection.Text;
                if (stringWroteUser != "")
                {
                    if (richtxtbox.Selection.Text.Contains('\n') != true)
                    {
                        int Index = stringWroteUser.IndexOf(@"//");
                        richtxtbox.Selection.Text = String.Concat(richtxtbox.Selection.Text.Substring(0, Index),
                            richtxtbox.Selection.Text.Substring(Index + 2, richtxtbox.Selection.Text.Length - Index - 2));
                        ColorOfReservedWords();
                        richtxtbox.Focus();
                    }
                    else
                    {
                        int IndexStart = stringWroteUser.IndexOf(@"/*");
                        richtxtbox.Selection.Text = String.Concat(richtxtbox.Selection.Text.Substring(0, IndexStart),
                           richtxtbox.Selection.Text.Substring(IndexStart + 2, richtxtbox.Selection.Text.Length - IndexStart - 2));
                        int IndexEnd = stringWroteUser.LastIndexOf(@"*/");
                        richtxtbox.Selection.Text = String.Concat(richtxtbox.Selection.Text.Substring(0, IndexEnd - 2));
                        ColorOfReservedAllWords();
                        richtxtbox.Focus();
                    }
                }
            }
            catch (Exception)
            {
                
                
            }
          

          
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (richtxtbox.Selection.Text.Contains('\n') != true)
                {

                    String stringWroteUser = @"//" + richtxtbox.Selection.Text;
                    richtxtbox.Selection.Text = stringWroteUser;
                }
                else
                {
                    String stringWroteUser = @"/*" + richtxtbox.Selection.Text;
                    richtxtbox.Selection.Text = stringWroteUser + @"*/";
                }

            }
            catch (Exception)
            {
                
                 
            }


           
        }

        static int Close_Window1 = 0;

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            Close_Window1 = 1;
            nameProject.Text = "";
            listBoxError.Items.Clear();
            //הוספת הכותרת של רשימת שגיאות

            CError.Paragraphs.ForEach(x => x.Background = Brushes.White);
            Hebrew_Code.Visibility = Visibility.Hidden;
            Visual_Studio_Code.Visibility = Visibility.Hidden;
           // English_Code.Visibility = Visibility.Collapsed;
            scroll_hebrew.Visibility = Visibility.Collapsed;
            scroll_english.Visibility = Visibility.Collapsed;
            richtxtbox.Visibility = Visibility.Collapsed;
            inEnglish.Visibility = Visibility.Collapsed;
          
            //SafetyExit s = new SafetyExit(richtxtbox,path,this,1);
            //s.Show();
 
        }
        //private void Close_All_Window(object sender, RoutedEventArgs e)
        //{
        //    SafetyExit se = new SafetyExit(richtxtbox, path,this);
        //    se.Show();
        //    this.Close();
        //}
        private void Save_Program(object sender, RoutedEventArgs e)
        {
            richtxtbox.SelectAll();
            if (path != "")
            {
                String all_text = richtxtbox.Selection.Text + Environment.NewLine;
                File.WriteAllText(path, all_text);
            }
           
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetDataObject(richtxtbox.Selection.Text);
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetDataObject(richtxtbox.Selection.Text);
            richtxtbox.Selection.Text = "";
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richtxtbox.Selection.Text = "";
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.IDataObject dataObject = System.Windows.Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(System.Windows.Forms.DataFormats.Text))
                richtxtbox.Selection.Text = (String)dataObject.GetData(System.Windows.Forms.DataFormats.Text);
        }

        private void MenuItem_Open_Project(object sender, RoutedEventArgs e)
        {
            //if (Close_Window1 == 1)
            //{
                //richtxtbox.Selection.Text = "";
            Hebrew_Code.Visibility=Visibility.Collapsed;
            Visual_Studio_Code.Visibility=Visibility.Collapsed;
            English_Code.Visibility = Visibility.Visible;
                scroll_hebrew.Visibility = Visibility.Visible;
                richtxtbox.Visibility = Visibility.Visible;
            //    listBoxError.Items.Clear();
            //}
            //הוספת הכותרת של רשימת שגיאות

            CError.Paragraphs.ForEach(x => x.Background = Brushes.White);
 
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Hebrew Programmin (*.hep)|*.hep";
            openFileDialog.FilterIndex = 1;
            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog.ShowDialog();
            // GeneralClass.FilePath = openFileDialog.FileName;
            openFileDialog.FileName.Replace('\\', '/');
            //הנתיב לא אמור להיות קבוע אלא הניתוב שנבחר ע"י המשתמש
            //openFileDialog.ValidateNames
            path = openFileDialog.FileName;
            int Index = path.LastIndexOf("\\");
            nameProject.Text = path.Substring(Index + 1, path.Length - Index - 1); 
            name = path; 
            richtxtbox.Visibility = Visibility.Visible;
            nameProject.Visibility = Visibility.Visible;
            listBoxError.Visibility = Visibility.Visible;
            inEnglish.Visibility = Visibility.Hidden;
            scroll_english.Visibility = Visibility.Hidden;
          
            if (path != "")
            {
                // Open the file to read from. 
                if (File.ReadAllText(path) != "")
                {
                    string readText = File.ReadAllText(path);
                    richtxtbox.SelectAll();
                    richtxtbox.Selection.Text = readText;
                    ColorOfReservedAllWords();


                }
                else
                {

                    richtxtbox.SelectAll();
                    richtxtbox.Selection.Text = "התוכנית הראשית";
                    richtxtbox.LineDown();
                    richtxtbox.Selection.Text = "{";
                    richtxtbox.LineDown();
                    richtxtbox.Selection.Text = "}";
                    richtxtbox.LineDown();
                }
            }
            
        }
     



        private List<Paragraph> vggParagraph(List<Block> leaaaaaaaaaaaaaaaa)
        {
            List<Paragraph> runlea = new List<Paragraph>();

            for (int i = 0; i < leaaaaaaaaaaaaaaaa.Count; i++)
            {
                runlea.Add(leaaaaaaaaaaaaaaaa[i] as Paragraph);
            }
            return runlea;


        }

        private void MenuItem_New_Window(object sender, RoutedEventArgs e)
        {
            listBoxError.Items.Clear();
            //הוספת הכותרת של רשימת שגיאות
            CError.Paragraphs.ForEach(x => x.Background = Brushes.White);
            WindowOpen windowOpen = new WindowOpen(nameProject, richtxtbox,this);
            richtxtbox.Visibility = Visibility.Visible;
            nameProject.Visibility = Visibility.Visible;
            listBoxError.Visibility = Visibility.Visible;
            windowOpen.Show();
            ////English_Code.Visibility = Visibility.Collapsed;
            ////Visual_Studio_Code.Visibility = Visibility.Visible;
            ////Hebrew_Code.Visibility = Visibility.Visible;
            ////conversion = new Conversion(listBoxError, richtxtbox);
            //////סימון כל הטקסט של הריצ טקסט בוקס
            ////richtxtbox.SelectAll();
            //////שליחה לפונקציה הראשית של תחילת ההמרה 
            ////conversion.MiniSpace(richtxtbox.Selection.Text, false);

            ////richtxtbox.SelectAll();
            ////all_Text = richtxtbox.Selection.Text;
            ////scroll_hebrew.Visibility = Visibility.Collapsed;
            ////scroll_english.Visibility = Visibility.Visible;
            ////richtxtbox.Visibility = Visibility.Collapsed;
            ////inEnglish.Visibility = Visibility.Visible;
            ////inEnglish.SelectAll();

            ////inEnglish.Selection.Text = CodeCompiler.code;



            ////Hebrew_Code.Visibility = Visibility.Hidden;
            ////Visual_Studio_Code.Visibility = Visibility.Hidden;
            ////English_Code.Visibility = Visibility.Visible;
            ////scroll_hebrew.Visibility = Visibility.Visible;
            ////scroll_english.Visibility = Visibility.Collapsed;
            ////richtxtbox.Visibility = Visibility.Visible;
            ////inEnglish.Visibility = Visibility.Collapsed;
            ////richtxtbox.SelectAll();
            ////richtxtbox.Selection.Text = all_Text;
            ////ColorOfReservedAllWords(); 
           
        }




          private void definition_var_Selected(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + varTB.Text;
            ColorOfReservedWords();
            activate_color = false;
        }

        private void definition_arr_Selected(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + arrTB.Text;
            ColorOfReservedWords();
            activate_color = false;
        }
        private void definition_mat_Selected(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + matTB.Text;
            ColorOfReservedWords();
            activate_color = false;
        }
        private void loop_for_Selected(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            
            string s = "מספר_שלם   אינדקס   =   0   ;";
            string s1="בעבור   (   אינדקס   =   0   ;   אינדקס<3   ;   אינדקס   ++   )   ";
            loopForTB.Text = s  + s1; ;
            richtxtbox.Selection.Text = stringWroteUser + loopForTB.Text;
            ColorOfReservedWords();
            activate_color = false;
        }
        private void loop_while_Selected(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + loopWhileTB.Text;
            ColorOfReservedWords();
            activate_color = false;
        }

        private void define_input(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + input.Text;
            ColorOfReservedWords();
            activate_color = false;
        }
        private void define_output(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + output.Text;
            ColorOfReservedWords();
            activate_color = false;
        }
        private void define_function(object sender, RoutedEventArgs e)
        {
            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + function.Text;
            ColorOfReservedWords();
            activate_color = false;
        }

        private void condition_Selected(object sender, RoutedEventArgs e)
        {

            String stringWroteUser = richtxtbox.Selection.Text;
            richtxtbox.Selection.Text = stringWroteUser + conditionTB.Text;
            ColorOfReservedWords();
            activate_color = false;

        }

        private void Exec_Visual_Studio_Click(object sender, RoutedEventArgs e)
        {
            inEnglish.SelectAll();
            string createText = inEnglish.Selection.Text + Environment.NewLine;
           // File.WriteAllText(@"C:\Users\תהילה\Desktop\ConsoleApplication2\ConsoleApplication2\Program.cs", createText);
            // System.Diagnostics.Process.Start(@"C:\Users\תהילה\Desktop\ConsoleApplication2\ConsoleApplication2.sln");
            File.WriteAllText(@"..\ConsoleApplication1\ConsoleApplication1\Program.cs", createText);
           System.Diagnostics.Process.Start(@"..\ConsoleApplication1\ConsoleApplication1.sln");
        

        }
        String all_Text = null;
        private void Return_Hebrew_Click(object sender, RoutedEventArgs e)
        {
            Hebrew_Code.Visibility = Visibility.Hidden;
            Visual_Studio_Code.Visibility = Visibility.Hidden;
            English_Code.Visibility = Visibility.Visible;
            scroll_hebrew.Visibility = Visibility.Visible;
            scroll_english.Visibility = Visibility.Collapsed;
            richtxtbox.Visibility = Visibility.Visible;
            inEnglish.Visibility = Visibility.Collapsed;
            richtxtbox.SelectAll();
            richtxtbox.Selection.Text = all_Text;
            ColorOfReservedAllWords();
        }

        public void ColorOfReservedAllWords()
        {
            TextPointer place = richtxtbox.Document.ContentStart;
            richtxtbox.CaretPosition = place;
            InlineCollection all_run_now = richtxtbox.CaretPosition.Paragraph.Inlines;
            bool try1 = true;
            while (try1)
            {
                try
                {
                    ColorOfReservedWords();
                    richtxtbox.CaretPosition = richtxtbox.CaretPosition.GetPositionAtOffset
                  (4, LogicalDirection.Backward);

                }
                catch (Exception)
                {
                    try1 = false;

                }
            }
        }
         private void Close_All_Window(object sender, RoutedEventArgs e)
        {
         
            SafetyExit se = new SafetyExit(richtxtbox, path, this);
            se.Show();
        }
       
         public static List<int> flags = new List<int>();
        public static int IndexOfTheLastBreakPoint = -1;
         //public static int[] flags = new int[57];
         private void button1_Click(object sender, RoutedEventArgs e)
         {
             IndexOfTheLastBreakPoint = -1;
             string s1 = @"../../Pictures\BreakPoint.png";
             string s2 = @"../../Pictures\HiddenBreakPoint.png";
             string s3 = "";
             System.Windows.Controls.Button b = sender as System.Windows.Controls.Button;

             var brush1 = new ImageBrush();
             brush1.ImageSource = new BitmapImage(new Uri(s1, UriKind.Relative));
             var brush2 = new ImageBrush();
             brush2.ImageSource = new BitmapImage(new Uri(s2, UriKind.Relative));

             string s = b.Name.Substring(10, b.Name.Length - 10);
             try
             {
                 int index = int.Parse(s);
                 
                 if (flags[index ] == 0)
                 {
                     //בדיקה האם מותר לשים פה ברייק פוינט- רק אם יש שם תוכן
                     //richtxtbox.SelectAll();
                     //s3 = richtxtbox.Selection.Text;
                     s3 = GetText(richtxtbox);
                     char[] strToDivideAccording = { '\n', '\r' };
                     String[] partStringUser = s3.Split(strToDivideAccording);//כל שורה=מחרוזת 
                     if ((partStringUser[index ]) != "")
                     {
                         flags[index ] = 1;
                         b.Background = brush1;
                     }
                 }
                 else
                 {
                     flags[index ] = 0;
                     b.Background = brush2;
                 }
             }
             catch(Exception exx)
             {
             }

          }
         public void ReFlag(int row)
         {
             flags[row] = 0;
         }
         public static int MoneRows = 0;
        
         //private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
         //{
         //    GridOfBreakPoint.
         //        string s3 = GetText(richtxtbox);
         //        char[] strToDivideAccording = { '\n', '\r' };
         //        String[] partStringUser = s3.Split(strToDivideAccording);//כל שורה=מחרוזת

         //       if (partStringUser.Length > MoneRows)
         //    {
         //        try
         //        {
         //            string s1 = @"../../Pictures\BreakPoint.png";
         //            string s2 = @"../../Pictures\HiddenBreakPoint.png";
         //            System.Windows.Controls.Button b = sender as System.Windows.Controls.Button;

         //            var brush1 = new ImageBrush();
         //            brush1.ImageSource = new BitmapImage(new Uri(s1, UriKind.Relative));
         //            var brush2 = new ImageBrush();
         //            brush2.ImageSource = new BitmapImage(new Uri(s2, UriKind.Relative));
         //            for (int i = MoneRows; i < partStringUser.Length; i++)
         //            {
         //              RowDefinition rd = new RowDefinition();
         //              rd.Height = 8; new GridLength(3);
         //            GridOfBreakPoint.RowDefinitions.Add(rd);

         //            Button b = new Button();
         //            b.Background = brush2;
         //            b.Click += button1_Click;
         //            b.Name = "BreakPoint" + i;
         //            b.Height = 17;
         //            b.Width = 17;
         //                 Width="18" Height="18"
         //            b.BorderBrush = Brushes.Silver;
         //            Grid.SetRow(b, i);
         //            GridOfBreakPoint.Children.Add(b);
         //            flags.Add(0);
         //            }
                     
         //            MoneRows = GridOfBreakPoint.RowDefinitions.Count;
         //        }
         //        catch (Exception eeeee)
         //        {
         //        }
                 

         //    }
            
            

         //}
         private void next(object sender, RoutedEventArgs e)
         {
             File.WriteAllText(@"..\..\VarsForBreakPoint.txt", "");
             //NextFlag = true;
             //Prosses.flag = true;
             try
             {
                 Prosses.p.Kill();
                 Prosses.p.Close();
             }
             catch { }
            // Prosses.flag = true;
             Compiler_Click(null, null);

         }
        public static string GetText( RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
        
        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            string s1 = @"../../Pictures\BreakPoint.png";
            string s2 = @"../../Pictures\HiddenBreakPoint.png";
            
            var brush1 = new ImageBrush();
            brush1.ImageSource = new BitmapImage(new Uri(s1, UriKind.Relative));
            var brush2 = new ImageBrush();
            brush2.ImageSource = new BitmapImage(new Uri(s2, UriKind.Relative));

            double x = scroll_hebrew.ExtentHeight;
            int y =(int)(x/(18.5));
           
            for (int i = GridOfBreakPoint.RowDefinitions.Count; i < y; i++)
            {
                try
                {


                    RowDefinition rd = new RowDefinition();
                   // rd.Height = new GridLength(18);
                    GridOfBreakPoint.RowDefinitions.Add(rd);
                   

                    Button b = new Button();
                    b.Background = brush2;
                    b.Click += button1_Click;
                    b.Name = "BreakPoint" +i;
                    b.Height = 18.5;
                    b.Width = 18.5;
                    b.BorderBrush = Brushes.Silver;
                    Grid.SetRow(b, i);
                    GridOfBreakPoint.Children.Add(b); 
                    
                    flags.Add(0);

                    
                }
                catch (Exception eeeee)
                {
                }


            }
            string s3 = "";
            s3 = GetText(richtxtbox);
             char[] strToDivideAccording = { '\n', '\r' };
             String[] partStringUser = s3.Split(strToDivideAccording);//כל שורה=מחרוזת 
                        
            for (int i = 0; i < flags.Count; i++)
            {
                 try
                {
                    
                    if (flags[i] == 1)
                    {
                         if ((partStringUser[i]) == "")
                        {
                            flags[i] = 0;
                            
                            Button b = new Button();
                            b.Background = brush2;
                            b.Click += button1_Click;
                            b.Name = "BreakPoint" + i;
                            b.Height = 18;
                            b.Width = 18;
                            b.BorderBrush = Brushes.Silver;
                            Grid.SetRow(b, i);
                            GridOfBreakPoint.Children.Add(b);
 
           
                        }
                    }
                   
                }
                catch
                {
                }

                
            }
        
            
        }
        private void Only_Compiler_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.IndexOfTheLastBreakPoint == -1)//איפוס הקבצים אם זה הרצה ראשונה
            {
                File.WriteAllText(@"..\..\FileForScanning.txt", "");//איפוס הקובץ ששומר על הקלטים
                File.WriteAllText(@"..\..\FileToShowVars.txt", "");
                File.WriteAllText(@"..\..\VarsForBreakPoint.txt", "");
            }
            listBoxError.Items.Clear();
            //הוספת הכותרת של רשימת שגיאות
            //listBoxError.Items.Add(TitleError);
            CError.Paragraphs.ForEach(x => x.Background = Brushes.White);
            //יצירת אובייקט של מחלקת ההמרה 
            conversion = new Conversion(listBoxError, richtxtbox);
            //סימון כל הטקסט של הריצ טקסט בוקס
            richtxtbox.SelectAll();//איך מבטלים זאת מיד אח"כ????
            
            //שליחה לפונקציה הראשית של תחילת ההמרה 
            try
            {
                //continue_running.Visibility = Visibility.Visible;

                // GetBreakPoints(GridOfBreakPoint, null);
                bool b= conversion.SendToCompile(richtxtbox.Selection.Text, true, this);
                
            }
            catch (Exception er)
            {
                
            }
        }

        private bool Only_Compiler_Click1(object sender, RoutedEventArgs e)
        {
            if (MainWindow.IndexOfTheLastBreakPoint == -1)//איפוס הקבצים אם זה הרצה ראשונה
            {
                File.WriteAllText(@"..\..\FileForScanning.txt", "");//איפוס הקובץ ששומר על הקלטים
                File.WriteAllText(@"..\..\FileToShowVars.txt", "");
                File.WriteAllText(@"..\..\VarsForBreakPoint.txt", "");
            }
            listBoxError.Items.Clear();
            //הוספת הכותרת של רשימת שגיאות
            //listBoxError.Items.Add(TitleError);
            CError.Paragraphs.ForEach(x => x.Background = Brushes.White);
            //יצירת אובייקט של מחלקת ההמרה 
            conversion = new Conversion(listBoxError, richtxtbox);
            //סימון כל הטקסט של הריצ טקסט בוקס
            richtxtbox.SelectAll();
            //שליחה לפונקציה הראשית של תחילת ההמרה 
            try
            {
                //continue_running.Visibility = Visibility.Visible;

                // GetBreakPoints(GridOfBreakPoint, null);
              return  conversion.SendToCompile(richtxtbox.Selection.Text, true,this);
            }
            catch (Exception er)
            {
                return false;
            }
        }
        //public void LableOfViewingVars12()
        //{
        //    LableOfViewingVars.Content = "344";
        //}
       
        //public static int HouManyBreakPoints()
        //{
        //    int f = 0;
        //    foreach (int item in flags)
        //    {
        //        if (item == 1)
        //            f++;
        //    }
        //     return f;
        //}

        //private void GetBreakPoints(object sender, RoutedEventArgs e)
        //{
        //     //  זה לא עובד!!!!!!!!!!!!1צריך לעבור על הגריד, ומי שיש בו את בראש1 צריך לשים אצלו בליסט 1!!
        
        //    string s1 = @"../../Pictures\BreakPoint.png";
        //    string s2 = @"../../Pictures\HiddenBreakPoint.png";
        //    var brush1 = new ImageBrush();
        //    brush1.ImageSource = new BitmapImage(new Uri(s1, UriKind.Relative));
        //    var brush2 = new ImageBrush();
        //    brush2.ImageSource = new BitmapImage(new Uri(s2, UriKind.Relative));
          
        //    Grid g = sender as Grid;
        //    for (int i = 0; i < g.Children.Count; i++)
        //    {
        //        Button b = new Button();
        //        b = g.Children[i] as Button;
        //        if (b.Background == brush1)
        //            flags[i] = 1;
                
        //    }

            
          
        //}


    }
}





