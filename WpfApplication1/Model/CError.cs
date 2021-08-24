using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApplication1
{
    public delegate void deleget_num_error(object sender, RoutedEventArgs e);
     public class CError
    {
        //הריצ טקסט בוקס שעל המסך
        RichTextBox richtextbox;
        //מכיל את כל רשימת השגיאות 
        Dictionary<String, String> Error;
        //דליגיט לקבלת מספר השורה
        public deleget_num_error my_deleget_num_error { get; set; }
        public static List<Paragraph> Paragraphs = new List<Paragraph>();
        public CError(RichTextBox _richtextbox)
        {
            //השמה של הריצ טקסט בוקס שעל המסך
            richtextbox = _richtextbox;
            Error = new Dictionary<String, String>();//לבנתיים זה מוגדר כרשימה ואח"כ נקשר לבסיס נתונים
            Error.Add("CS0246", "השם של סוג/טיפוס  לא נמצא ");
            Error.Add("CS1002", "מצפה לנקודה -פסיק בסיום הפקודה");
            Error.Add("CS1001", "מצפה לנקודה -פסיק בסיום הפקודה");
            Error.Add("CS0029", "לא יכל להמיר מחורזת למספר וגם לא ההיפך");
            Error.Add("CS1041", "לא ניתן להשתמש במילים שמורות");
            Error.Add("CS0178", " [ הגדרה לא חוקית -מצפה ל ,או ל ");
            Error.Add("CS1525", "הביטוי == לא חוקי ");
            Error.Add("CS0103", "המשתנה לא נמצא בהקשר הנכון או לא הוגדר");
            Error.Add("CS0642", "בסוף משפט תנאי או לולאת כל עוד לא יבוא ; ");
            Error.Add("CS0650", "שגיאה בהגדרת המערך יש להגדיר מערך _סוג");
            Error.Add("CS0270", "שגיאה בהגדרת המערך יש להגדיר מערך _סוג");
            //CS1041: 
            Error.Add("CS0019", "האופרטור * לא יכול להיות מושם במשתנה מסוג שלם");  
        }

        private void ListItemError_Selected(object sender, RoutedEventArgs e)
        {
            if (my_deleget_num_error != null)
                my_deleget_num_error(sender, e);
        }
        public void FuncInCaseError(System.Windows.Controls.ListBox listBoxError, ListBoxItem listboxTitle, CompilerResults results)
        {
            //אובייקט שבונה את המבנה של השגיאות
            StringBuilder sb = new StringBuilder();
            //ניקוי של רשימת הארורים
            listBoxError.Items.Clear();
            //הוספת הכותרת של רשימת שגיאות
            listBoxError.Items.Add(listboxTitle);
            //מעבר על רשימת השגיאות והוספתן לערימת השגיאות
            foreach (CompilerError error in results.Errors)
            {
                //רשימת השגיאות
                ListBoxItem listBoxItem = new ListBoxItem();
                //הוספת שגיאה לרשימת השגיאות
                listBoxError.Items.Add(listBoxItem);
                //מעבר הסמן על השורה שבה השגיאה נמצאת
                listBoxItem.Selected += FuncNumError;
                //??
                listBoxItem.Tag = ((int)(error.Line) -7).ToString();
                //טקסט השגיאה לפני התרגום 
                string error_text = error.ErrorText;
                //בדיקה אם השגיאה מופיעה ברשימות השגיאות שתרגמנו
                if (Error.Keys.Contains(error.ErrorNumber))
                    //הבאת תרגום השגיאה לעברית
                    error_text = Error[error.ErrorNumber];
                //הוספת השגיאה+השורה שבה היא נמצאת
                listBoxItem.Content = error_text + "    " + ((int)(error.Line) - 6).ToString();
                //מבנה השגיאה 
                sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
            }
        }
        // פונקציה המופעלת על כל שגיאה ומראה את מספר השורה המופיעה בטאג
        public void FuncNumError(object sender, RoutedEventArgs e)
        {
            
            //מכיל את רשימת השגיאות 
            ListBoxItem list_item_error = ((ListBoxItem)sender);
            //מקבל את השורה שבה השגיאה נמצאת - בעזרת הטאג
            int num_line_error = (Int16.Parse((list_item_error.Tag).ToString()));
            //מצביע על השורה הראשונה של הריצ טקסט בוקס
            richtextbox.CaretPosition = richtextbox.Document.ContentStart;
            //מצביע על השורה שבה השגיאה נמצאת ביחס לשורה הראשונה 
            richtextbox.CaretPosition = richtextbox.CaretPosition.GetLineStartPosition(num_line_error);//שורה לא עובדת????
            //מעלה פוקוס על הריצ טקסט בוקס 
            richtextbox.Focus();
            Paragraph ColorError = richtextbox.CaretPosition.Paragraph;
            ColorError.Background = Brushes.Yellow;
            Paragraphs.Add(ColorError);
            richtextbox.Focus();
        }
    }
}
