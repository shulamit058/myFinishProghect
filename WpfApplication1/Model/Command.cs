using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication1
{
    public class Command
    {
        public int Id { get; set; }
        public String Hebrew { get; set; }
        public String English { get; set; }
        public SolidColorBrush Color { get; set; }
        public Command() { }
    }
    public class CommandList
    {
        private static List<Command> commandList1;
        public static List<Command> CommandList1()
        {
            if (commandList1 == null)
            {
                commandList1 = new List<Command>();
                commandList1.Add(new Command() { Id = 1, Hebrew = "מספר_שלם", English = "int", Color = Brushes.OrangeRed });
                commandList1.Add(new Command() { Id = 2, Hebrew = "מספר_ממשי", English = "double", Color = Brushes.OrangeRed });
                commandList1.Add(new Command() { Id = 3, Hebrew = "תו", English = "char", Color = Brushes.OrangeRed });
                commandList1.Add(new Command() { Id = 4, Hebrew = "מחרוזת", English = "string", Color = Brushes.OrangeRed });
                commandList1.Add(new Command() { Id = 5, Hebrew = "אם", English = "if", Color = Brushes.Orange });
                commandList1.Add(new Command() { Id = 6, Hebrew = "אחרת", English = "else", Color = Brushes.Orange });
                commandList1.Add(new Command() { Id = 7, Hebrew = "הדפס", English = "Console.WriteLine", Color = Brushes.Aqua });
                commandList1.Add(new Command() { Id = 7, Hebrew = "הדפס_ללא_ירידת_שורה", English = "Console.Write", Color = Brushes.Aqua });
                commandList1.Add(new Command() { Id = 8, Hebrew = "קלוט_מספר_ממשי", English = "Convert.ToDouble(Console.ReadLine())", Color = Brushes.Orange });
                commandList1.Add(new Command() { Id = 9, Hebrew = "קלוט_מספר_שלם", English = "Convert.ToInt32(Console.ReadLine())", Color = Brushes.Orange });
                commandList1.Add(new Command() { Id = 10, Hebrew = "קלוט_תו", English = "Convert.ToChar(Console.ReadLine())", Color = Brushes.Orange });
                commandList1.Add(new Command() { Id = 11, Hebrew = "קלוט_מחרוזת", English = "Console.ReadLine()", Color = Brushes.Orange });
                commandList1.Add(new Command() { Id = 12, Hebrew = "המשך", English = "continue", Color = Brushes.Red });
                commandList1.Add(new Command() { Id = 13, Hebrew = "שבור", English = "break", Color = Brushes.Red });
                commandList1.Add(new Command() { Id = 14, Hebrew = "כל_עוד", English = "while", Color = Brushes.OrangeRed });
                commandList1.Add(new Command() { Id = 15, Hebrew = "בעבור", English = "for", Color = Brushes.DarkBlue });
                commandList1.Add(new Command() { Id = 16, Hebrew = "התוכנית_הראשית", English = "public static void Main()", Color = Brushes.Blue });
                commandList1.Add(new Command() { Id = 17, Hebrew = "new", English = "new", Color = Brushes.Red });
                commandList1.Add(new Command() { Id = 18, Hebrew = "להחזיר", English = "return  ", Color = Brushes.GreenYellow });
                commandList1.Add(new Command() { Id = 19, Hebrew = "פונקציה", English = "static", Color = Brushes.GreenYellow });
                commandList1.Add(new Command() { Id = 20, Hebrew = "ריק", English = "null", Color = Brushes.HotPink });
             }
            return commandList1;
        }
    }
}

               