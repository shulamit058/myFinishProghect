using Microsoft.CSharp;
using System;
using System.IO;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication1
{ 
   public   class Conversion
    {
        #region DefineVariable
        ListBox listBoxError;
        ListBoxItem listboxTitle;
        List<Command> words;
        String[] partString = new String[5];
        RichTextBox richtextbox;
        public int[] flags = new int[57];
        int f = 0;
        bool run;
        bool inCompilation = true;
        string m_readNamesPlace = "מקום"+"\n";
        #endregion
        #region Ctor
        public Conversion(ListBox listBox, RichTextBox _richtextbox)
        {
             words = CommandList.CommandList1(); 
            listBoxError = listBox;
           // listboxTitle = listboxitem;
            richtextbox = _richtextbox;
        }
        #endregion
        #region Method
        public void MiniSpace(String StringOfRichtextbox, bool run, MainWindow mainWindow, List<int> flags)
        {
            
                this.run = run;
                String allnewstring = "";
                //מחרוזת שהמשתמש הקליד
                String stringWroteUser = StringOfRichtextbox;
                char[] strToDivideAccording = { '\n', '\r' };
                //וזה במקרה- שהמשתמש מקליד כמה משפטים באותה שורה אז נצטרך לחלק לפי הנקודה שמורה על הסוף חוץ ממקרים מיוחדים כמ בתנאים
                String[] partStringUser = stringWroteUser.Split(strToDivideAccording);//כל שורה=מחרוזת 
                //פנקציה לטפול כל שורה בנפרד 
            try
            {
                allnewstring = allLines(partStringUser); 
            }
            catch
            {
            }
                CodeCompiler codeCompile = new CodeCompiler(richtextbox);
                bool b = false;
           
                
                codeCompile.CodeCompiler1(allnewstring);
           
             if (run)
            {//זה הקימפול של כל הקוד
               b= (codeCompile.CodeCompiler2(allnewstring, listBoxError, listboxTitle, mainWindow));
            }
            //רק אחרי קימפול של הכל ניתן להתחיל לעבור לפי הברייק פוינטים ולהריץ!!!
            //אחרי כל התגום ניתן להרצה בקומפילר
            if (b == true)
            {

                   inCompilation = false;
                    int j = 0;
                    
                    for (; j < flags.Count; j++)
                    {
                        if (flags[j] == 1 && j > MainWindow.IndexOfTheLastBreakPoint)
                            break;
                    }

                    
                    string s1 = "";
                    if (j == flags.Count)
                    {//אם אין שום ברייקפוינט
                        allnewstring = allLines(partStringUser);
                        s1 = allnewstring;
                    }
                    else
                    {//אם יש ברייק פוינט
                        s1 = parcialLines(partStringUser, j + 1, mainWindow);
                       
                    } 
                //צריך לחשוב פה על הרבה דברים אם רץ עם פונקציות צריך לטפל במשתנים המוכרים בלבד... אולי לקמפל ומה שלא מתקמפל עבורו למחוק...ד
                        //פה צריך להוסיף קראיה של המשתנים מקובץ הטקסט אם זה לא נופל...
                    s1 = s1.Substring(0, s1.LastIndexOf("}"));
                    string s = File.ReadAllText(@"..\..\FileToShowVars.txt");
                    char[] c = { '\n', '\r' };
                    String[] partString = s.Split(c);
                    for (int i = 0; i < partString.Length; i++)
                    {
                        if (partString[i] != "")
                        {
                            //problem!!! צריך שיוסיף רק את המשתנים שהוגדרו עד עכשיו!!! קריטי!!!!!
                            if (s1.Contains(partString[i]))
                                s1 += WritToTxtToSeevarsAfterBreakPoint(partString[i]);//לשים לב שכותב גם משתנים מקומיים שלא יכול אח"כ לכתוב אותם לטקסט כי לא מכיר אותם!!!
                        }

                    }
                    s1 += "}"; 

                    //תהילה!!!
                    codeCompile.CodeCompiler1(s1);
                    try
                    {
                     if (run)
                            codeCompile.CodeCompiler3(s1, listBoxError, listboxTitle, mainWindow);
                     mainWindow.GridOfViewingVarsNames.Content = "שם המשתנה" + "\n" + ReadNames();
                     mainWindow.GridOfViewingVarsContents.Content = "ערך המשתנה" + "\n" + ReadContent();
                     mainWindow.GridOfViewingVarsNamesPlace.Content = m_readNamesPlace;
                     }
                    catch 
                    {//  נופל במקרה שלא יכול לקרוא את ערכי כל המשתנים- הם לא מוגדרים לו וגם אם יש בתכנית פונקציות יפול... כי לוקח את שפ הפונקציה בתור משתנה! !!!
                        this.run = run;
                         allnewstring = "";
                        //מחרוזת שהמשתמש הקליד
                         stringWroteUser = StringOfRichtextbox;
                       
                        //וזה במקרה- שהמשתמש מקליד כמה משפטים באותה שורה אז נצטרך לחלק לפי הנקודה שמורה על הסוף חוץ ממקרים מיוחדים כמ בתנאים
                        partStringUser = stringWroteUser.Split(strToDivideAccording);//כל שורה=מחרוזת 
                        //פנקציה לטפול כל שורה בנפרד
                        allnewstring = allLines(partStringUser);
                        codeCompile = new CodeCompiler(richtextbox);
                        //אחרי כל התגום ניתן להרצה בקומפילר
                        codeCompile.CodeCompiler1(allnewstring);
                        if (run)
                            codeCompile.CodeCompiler5(allnewstring, listBoxError, listboxTitle);
                    }

                  MainWindow.IndexOfTheLastBreakPoint = j;  
                        
             }
           
           }

        public bool SendToCompile(string StringOfRichtextbox, bool run, MainWindow mainWindow)
        {
            this.run = run;
            String allnewstring = "";
            //מחרוזת שהמשתמש הקליד
            String stringWroteUser = StringOfRichtextbox;
            char[] strToDivideAccording = { '\n', '\r' };
            //וזה במקרה- שהמשתמש מקליד כמה משפטים באותה שורה אז נצטרך לחלק לפי הנקודה שמורה על הסוף חוץ ממקרים מיוחדים כמ בתנאים
            String[] partStringUser = stringWroteUser.Split(strToDivideAccording);//כל שורה=מחרוזת  
            //פנקציה לטפול כל שורה בנפרד 
            try
            {
                allnewstring = allLines(partStringUser);
            }
            catch { }

            CodeCompiler codeCompile = new CodeCompiler(richtextbox);
            codeCompile.CodeCompiler1(allnewstring);
            return (codeCompile.CodeCompiler2(allnewstring, listBoxError, listboxTitle, mainWindow));
        }

        public String allLines(string[] partStringUser)
        {
            
                //מחרוזת שמכילה את כל התרגום
                String allnewstring = "";
            try
            {
                for (int i = 0; i < partStringUser.Length; i++)
                {
                  //  if (partStringUser[i].Contains("הדפס"))
                   // {
                   // }
                   // else
                    //שולח את השורה הנוכחית ומחרוזת שתכיל את כל התוכנית
                    allnewstring = convertLine(partStringUser[i], allnewstring, i);
                }
            }
            catch { }
            return allnewstring;
        }
        public String parcialLines(string[] partStringUser, int row, MainWindow mainWindow)
        {
            string TillTheBreaPoint = "";
            // לא לשכוח פה להחליט האם להשאיר קוד זה עבור פונקציות או לא 
           // if (!(((partStringUser[0][0]) == 'פ')))//אם אין פונקציה
           // {
                for (int i = 0; i < row; i++)
                {
                    //שולח את השורה הנוכחית ומחרוזת שתכיל את כל התוכנית
                    TillTheBreaPoint = convertLine(partStringUser[i], TillTheBreaPoint, i);
                }
                TillTheBreaPoint = AddSoger(TillTheBreaPoint);

                return TillTheBreaPoint;
           // }//עד כאן טיפול בקוד שאין בתוכו פונקציה!!
          //  else
               // return allLines(partStringUser);


         }
        public string AddSoger(string s)
        {
            //הוספת סוגר במידה ויש סוגר פתוח
            int sogar = 0;
            foreach (char item in s)
            {
                if (item == '{')
                    sogar++;
                else
                    if (item == '}')
                        sogar--;
            }
            if (sogar > 0)
            {
                for (int i = 0; i < sogar; i++)
                {
                    s += "}";
                }
            }
            return s;
        }

        public string convertLine(string source_line,  string allnewstring,int row)
        {
            string  line_now = source_line;
            int index = 0;
            String allnewstringFirst = "", allnewstringTwo = "";
            //צריך לדאוג אם יש לי רק "אחד ב""כדי שבמקרה כזה לא יפול
            int length = line_now.Length;
            while (source_line != "" && index < length)
            {
                if (line_now.Contains('\"'))//אם יש מרכאות
                {
                    int startPrint = line_now.IndexOf('"');
                    // הוא עובר על כל שורה ובודק האם יש בה " אם יש מחלק
                    if (startPrint > 0 && line_now[startPrint - 1].ToString() != "'" && line_now[startPrint + 1].ToString() != "'")// שידע שזה לא string'"'אם  
                    {//את מחורזת ל3 א)מחורזת עד-". ב)מה שנמצא ב-". ג)ההמשך 
                        int endPrint = line_now.IndexOf('"', startPrint + 1);
                        allnewstringFirst = line_now.Substring(0, startPrint);
                        allnewstringTwo = line_now.Substring(startPrint, endPrint - startPrint + 1);
                        if (run)
                        allnewstringTwo=Reverse1(allnewstringTwo);
                     //   Console.WriteLine(allnewstringTwo.ToString());
                        line_now = line_now.Substring(endPrint + 1, line_now.Length - endPrint - 1);
                        index += endPrint + 1;
                        //שולחים לעשות את כל השאר הרווחים
                        try
                        {
                            allnewstringFirst = ToDoMinSpaceAndDivideFunc(allnewstringFirst, row);
                        }
                        catch { }
                        allnewstring = String.Concat(allnewstring, allnewstringFirst, allnewstringTwo);

                    }
                    else
                        //מקדם לאיבר הבא
                        index += startPrint;
                }
                else
                {
                    allnewstring = String.Concat(allnewstring, ToDoMinSpaceAndDivideFunc(line_now, row), "\n");
                    index += length;
                }
            }
            return allnewstring;
        }
        public static string Reverse1(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public String ToDoMinSpaceAndDivideFunc(String strSource,int row)
        {
            strSource = Regex.Replace(strSource, @"\s+", " ");
            strSource = AddSpaceInThisCase1(strSource);
            //צריך לחשוב על שם זה דומה ללמעלה אבל בדיוק מקרים שיש את הסימו פעם אחת ולא פעמים
            strSource = AddSpaceInThisCase2(strSource);
            //מקצצת רוחיים בסוף ובהתחלה
            strSource = strSource.Trim();
            //זיהוי משפט שהוא הגדרה לש מערך או מטריצה
            strSource = DefinitionArrAndMat(strSource);
            //בכל מקרה אחר 
            return DefinitionVariable(strSource, row);
        }
        public string AddSpaceInThisCase1(string strSource)
        {
            // כל זה זה בשביל שהיה רווחים בכל המקרים הללו 
            String[] stringContainsChar = { ")", "(", "++", "--", "==", ";", ",", "]", "[", "}", "{", "<",">",'"'.ToString(), "/" }; 
            for (int i = 0; i < stringContainsChar.Length; i++)
                if (strSource.Contains(stringContainsChar[i]))
                    strSource = strSource.Replace(stringContainsChar[i], " " + stringContainsChar[i] + " ");
            return strSource;
        }
        public string AddSpaceInThisCase2(string strSource)
        {
            int IndexEquals;
            Char[] charContainsChar = { '=', '+', '-' ,'\\'};
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
        public String DefinitionArrAndMat(String strSource)
        {
            //טיפןל בהגדרה של מערך או מטריצה 
            int sameIndex = strSource.IndexOf('=');
            String[] kindvar = { "מספר_שלם", "מחרוזת", "מספר_ממשי", "תו" };
            int place = 0;
            for (int i = 0; i < kindvar.Length; i++)
            {
                if (strSource.Contains(kindvar[i]))
                {
                    place = i;
                    break;
                }
            }
            int p1 = strSource.IndexOf(kindvar[place]);
            int p2 = strSource.LastIndexOf(kindvar[place]);
            bool bracket_contains = strSource.Contains('[');
            bool is_input = strSource.Contains("קלוט");
            if ((bracket_contains)&&!is_input && ((p1 != -1 && p2 != -1 && p1 != p2) || (p1 != -1 && p2 == p1 && strSource.IndexOf("=") < p1)) && sameIndex!=-1)
                return String.Concat(strSource.Substring(0, sameIndex + 1), " new ",
                    strSource.Substring(sameIndex + 1, strSource.Length - sameIndex - 1));
            return strSource;
        }
        int rowOfScanTxt = 0;
        string notToWriteToTxtFile = "";
        public String DefinitionVariable(String strSource,int row)
        {
            //מפרידה לפי רווחים
            String[] StrAccordingPaofit = strSource.Split(' ');
            String allnewstring = "";
            string MyVar = "";
            char index=' ';
            bool flagOfNotToWriteToTxtFile = false;
            string ToTxtFile = "";
            for (int i = 0; i < StrAccordingPaofit.Length; i++)
            {
                if ("פונקציה" == StrAccordingPaofit[i])
                    flagOfNotToWriteToTxtFile = true;
                if (StrAccordingPaofit[i] != "")
                {
                    //מילה מתוך מילים שמורות
                   if (words.Where(w => w.Hebrew.Equals(StrAccordingPaofit[i])).Any())
                    {
                       
                        //בדיקה האם יש כאן קלט
                        if (((words.Where(w => w.Hebrew.Equals(StrAccordingPaofit[i])).First().English).Equals("Convert.ToDouble(Console.ReadLine())")
                            || (words.Where(w => w.Hebrew.Equals(StrAccordingPaofit[i])).First().English).Equals("Convert.ToInt32(Console.ReadLine())")
                            || (words.Where(w => w.Hebrew.Equals(StrAccordingPaofit[i])).First().English).Equals("Convert.ToChar(Console.ReadLine())")
                             || (words.Where(w => w.Hebrew.Equals(StrAccordingPaofit[i])).First().English).Equals("Console.ReadLine()")) && (!inCompilation))
                        {
                            if (row > MainWindow.IndexOfTheLastBreakPoint)//בדיקה האם זה בפעם הראשונה שעוברים על קטע זה שאז צריך באמת לקלוט ולכתוב לקובץ את הערך שנקלט
                            {
                               // MainWindow.IndexOfTheLastBreakPoint = row;//tehila try problem!!!
                                allnewstring += words.Where(w => w.Hebrew.Equals(StrAccordingPaofit[i])).First().English + " ";

                                for (int j = 0; j < allnewstring.Length; j++)//MyVar מכיל את שם המשתנה
                                {
                                    if (allnewstring[j] >= 'א' && allnewstring[j] <= 'ת' && !(allnewstring[j] >= '0' && allnewstring[j] <= '9'))
                                    {
                                        
                                        MyVar = allnewstring.Substring(allnewstring.IndexOf(allnewstring[j]));
                                        if (MyVar.Contains('['))//זיהוי שמשתנה זה הוא מערך
                                        {
                                            index=Convert.ToChar(MyVar.Substring(MyVar.IndexOf('[') + 1, MyVar.IndexOf(']') - MyVar.IndexOf('[') - 1));
                                         }
                                        MyVar = MyVar.Substring(0, MyVar.IndexOf(' '));
                                        if (index >= '0' && index <= '9')
                                            MyVar = MyVar + '[' + index.ToString() + ']';

                                        break;
                                    }
                                }

                            }
                            else
                            {
                                
                                char ch = '"';
                                string fromTxtFile = ReadLineFromTxtFile(rowOfScanTxt);
                                allnewstring = allnewstring.Trim();
                                //קריאה מהקובץ אליו נכתב במהלך הריצה סוג המשתנה
                                string type = fromTxtFile.Substring( fromTxtFile.IndexOf(':')+1);//סוג המשתנה
                                while (fromTxtFile.Substring(0, fromTxtFile.IndexOf(' '))=="0")//לא ברור מאיפה מגיעים האפסים האלו- לא לשכוח שאסור לקלוט 0!!!!
                                {
                                    rowOfScanTxt++;
                                    fromTxtFile = ReadLineFromTxtFile(rowOfScanTxt);
                                }
                                type = type.Trim();
                                fromTxtFile=fromTxtFile.Trim();
                                fromTxtFile = fromTxtFile.Substring(0, fromTxtFile.IndexOf(' '));//תוכן המשתנה
                                switch (type)
                                {
                                    case ("Int32"):
                                        {
                                           allnewstring += fromTxtFile + ";\n";
                                            allnewstring += "Console.WriteLine (" + fromTxtFile + ");";
                                            
                                        }
                                        break;
                                    case ("Double"):
                                        {
                                            allnewstring += fromTxtFile + ";\n";
                                            allnewstring += "Console.WriteLine (" + fromTxtFile + ");";
                                        }
                                        break;
                                    case ("String"):
                                        {
                                            allnewstring += ch + fromTxtFile + ch + ";\n";
                                            allnewstring += "Console.WriteLine (" + ch + fromTxtFile + ch + ");";
                                        }
                                        break;
                                    case ("Char"):
                                        {
                                            allnewstring += "'" + fromTxtFile + "'"+ ";\n";
                                            allnewstring += "Console.WriteLine (" + "'" + fromTxtFile + "'" + ");";
                                        }
                                        break;
                                   
                                }
                                rowOfScanTxt++;
                               
                                

                            }

                        }
                        else
                            allnewstring += words.Where(w => w.Hebrew.Equals(StrAccordingPaofit[i])).First().English + " ";
 
                    }
                    else
                    {
                        //שם של משתנה
                        if (StrAccordingPaofit[i][0] >= 'א' && StrAccordingPaofit[i][0] <= 'ת' && !(StrAccordingPaofit[i][0] >= '0'
                            && StrAccordingPaofit[i][0] <= '9') )
                        {//לכתוב לקובץ הטקסט את שמות כל המשתנים ואח"כ לפני הברייק פוינט לשלוח לקובץ הטקסט את ערכם Console.WriteLine(משתנה.tostring());:
                            
                                allnewstring = allnewstring + " " + StrAccordingPaofit[i] + "a" + " ";
                                ToTxtFile = StrAccordingPaofit[i] + "a" + " ";
                                try
                                {
                                    int open=0, close=0, num=0;
                                    for (int y = 0; y < StrAccordingPaofit.Length; y++)
                                    {
                                        if (StrAccordingPaofit[y] == "[")
                                            open = y;
                                        if (StrAccordingPaofit[y] == "]")
                                            close = y;
                                    }
                                    //if (StrAccordingPaofit[i + 2] == "[" && StrAccordingPaofit[i + 6] == "]" && StrAccordingPaofit[i + 4][0] >= '0' && StrAccordingPaofit[i + 4][0] <= '9')
                                    //    ToTxtFile += '[' + StrAccordingPaofit[i + 4] + ']';
                                    if (close != 0 && open != 0)
                                    {
                                        num = (close - open) / 2 + open;
                                        if (StrAccordingPaofit[open] == "[" && StrAccordingPaofit[close] == "]" && StrAccordingPaofit[num][0] >= '0' && StrAccordingPaofit[num][0] <= '9')
                                            ToTxtFile += '[' + StrAccordingPaofit[num] + ']';
                                    }
                                }
                                catch { }
                                if (!flagOfNotToWriteToTxtFile && ToTxtFile != notToWriteToTxtFile)
                                {
                                    WritToTxtOfVarsFile(ToTxtFile);
                                }
                                else
                                {
                                    notToWriteToTxtFile = ToTxtFile;
                                    flagOfNotToWriteToTxtFile = false;
                                }
                            
                        }
                        else
                            //כל תו אחר 
                            allnewstring = allnewstring + StrAccordingPaofit[i];
                    }

                
                }
            }
            if (MyVar != "")
                allnewstring += WritToTxtOfScanFile(MyVar);
           

            return allnewstring;
        }
       //פונקציה שמוסיפה בריצה את כתיבת תוכן המשתנה לתוך קובץ טקסט
        public string WritToTxtOfScanFile(string MyVar)
        {
           string path = @"..\..\FileForScanning.txt";
           char ch = '"';
           string AddWriteToTxtOfScanFile = "\n File.WriteAllText( @" + ch + path + ch + ", File.ReadAllText( @" + ch + path + ch + ") +" + MyVar + ".ToString() +"+ch+"       type:"+ch+"+   "+MyVar+".GetType().Name  + Environment.NewLine);";
            return AddWriteToTxtOfScanFile;
        }
     //פונקציה שכותבת את שמות כל המשתנים לתוך קובץ טקסט
        public void WritToTxtOfVarsFile(string ToTxtFile)
        {

            if (!(File.ReadAllText(@"..\..\FileToShowVars.txt").Contains(ToTxtFile)))
            {
                 string str = ToTxtFile + Environment.NewLine;
                 File.WriteAllText(@"..\..\FileToShowVars.txt", File.ReadAllText(@"..\..\FileToShowVars.txt") + str);
            }
        }
       //פונקציה שקוראת מקובץ טקסט את תוכן המשתנה עפי שורה
          public string ReadLineFromTxtFile(int i)
        {
            string s="";
            try
            {
                s = File.ReadLines(@"..\..\FileForScanning.txt").Skip(i).Take(1).First();
            }
            catch(Exception ert){}
          return s;
        }
          //פונקציה שמוסיפה בריצה את כתיבת תוכן המשתנים עד הברייק פוינט לתוך קובץ טקסט
          public string WritToTxtToSeevarsAfterBreakPoint(string MyVar)
          {
              string path = @"..\..\VarsForBreakPoint.txt";
              char ch = '"';
              string AddWriteToTxtOfScanFile = "\n File.WriteAllText( @" + ch + path + ch + ", File.ReadAllText( @" + ch + path + ch + ") +" + MyVar + ".ToString() +" + ch + " = " + ch + "+" + ch + MyVar + ch + "+" +ch+"       type:"+  ch + "+   " + MyVar + ".GetType().Name  +Environment.NewLine);";
              return AddWriteToTxtOfScanFile;
          }
          //פונקציה שקוראת מקובץ טקסט את תוכן המשתנים כולם 
          public string ReadFromTxtFile()
          {
             string s = "";
             string s2= File.ReadAllText(@"..\..\VarsForBreakPoint.txt");
             char[] c = { '\n', '\r' };
             String[] partString = s2.Split(c);
             for (int i = 0; i < partString.Length; i++)
             {
                 if (partString[i] != "" && !(partString[i].Contains("System")))
                 {
                    if (IsNumber(partString[i]))
                    {
                         try
                         {
                             s += partString[i];
                             s += "\n";
                         }
                         catch  { }
                     }
                    else//יש פה מחרוזת או תו
                     {
                         s += RollString(partString[i]);
                         s += "\n";
                    }
                 }
              }
             return s;
          }

           public string ReadNames()
          {
              string source = ReadFromTxtFile();
              string target = "";
              char[] c = { '\n', '\r' };
              String[] partString = source.Split(c);
              for (int i = 0; i < partString.Length; i++)
              {
                  if (partString[i] != "")
                  {
                      if (partString[i].Contains("="))
                      {
                          if (partString[i].Contains("type"))
                              partString[i] = partString[i].Substring(partString[i].IndexOf("=") + 1, partString[i].IndexOf("type") - (partString[i].IndexOf("=") + 1));
                          else
                              partString[i] = partString[i].Substring(partString[i].IndexOf("=") + 1);
                          partString[i] = partString[i].Trim();
                      }

                      if (partString[i].Contains("a"))
                      {
                          partString[i] = partString[i].Replace("a", "*");
                      }

                      if (!(target.Contains(partString[i])))//שלא יכתוב פעמיים משתנים
                      { 
                          if (partString[i].Contains("["))
                          {
                              m_readNamesPlace = m_readNamesPlace + partString[i].Substring(partString[i].IndexOf("[")+1, partString[i].IndexOf("]") - partString[i].IndexOf("[")-1) + "\n";
                              partString[i] = partString[i].Substring(0, partString[i].IndexOf("[") - 1);

                          }
                          else
                              m_readNamesPlace = m_readNamesPlace + "\n";

                          target = target + partString[i] + "\n";
                      }
                  }
              }

              return target;
          }

          

          public string ReadContent()
           {
               string source = ReadFromTxtFile();
               string target = "";
               char[] c = { '\n', '\r' };
               String[] partString = source.Split(c);
               for (int i = 0; i < partString.Length; i++)
               {
                   if (partString[i] != "")
                   {
                       if (partString[i].Contains("="))
                       {
                           partString[i] = partString[i].Substring(0,partString[i].IndexOf("="));
                           
                           partString[i] = partString[i].Trim();
                       }
                      
                      
                           target = target + partString[i] + "\n";
                   }
               }

               return target;
           }
         
        
       //פונקציה שהופכת שם של משתנה!
          public string RollString(string s)//דומח = יעלa        type:String
          {
              string s2 = (s.Substring(s.IndexOf("=")+1)).Trim();


              s2 = (s2.Substring(0, s2.IndexOf("t")-1)).Trim();
              

              string s1 = (s.Substring(0,s.IndexOf("="))).Trim();
              string newS="";
              for (int i = 0; i < s1.Length; i++)
              {
                  newS += s1.Substring(s1.Length - i-1, 1);
                  
              }
              return newS + "=" + s2;
          }
       //פונקציה שמחזירה אמת אם המשתנה הוא מספר
          public bool IsNumber(string s)
          {
             
              s = s.Trim();
              if(s.Contains("String"))
                 return false;
              return true;

              
          }
       
        
        #endregion
    }
}
