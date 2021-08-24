using System; 
using System.IO;
namespace First  
{   
public class Program  
{ 
static int  זוגיa (int [,] מטריצהa ,int  גודלa )
{
int  כמותa =0;
for (int  אa =0; אa < גודלa ; אa ++)
for (int  בa =0; בa < גודלa ; בa ++)
{
if ( מטריצהa [ אa , בa ]%2==0)
 כמותa ++;
}
return    כמותa ;
}
public static void Main() 
{
int [,] מטריצהa =new int [2,2]{{100,95},{87,90}};
int  גודלa =2;
Console.WriteLine ("   איברי   המטריצה   ");
for (int  אינדקס_שורהa =0; אינדקס_שורהa < גודלa ; אינדקס_שורהa ++)
{
for (int  אינדקס_עמודהa =0; אינדקס_עמודהa < גודלa ; אינדקס_עמודהa ++)
{
Console.Write ("   "+ מטריצהa [ אינדקס_שורהa , אינדקס_עמודהa ]);
}
Console.WriteLine ("   ");
}
Console.WriteLine ("   מספר   האיברים   הזוגיים   במטריצה   ");
Console.WriteLine ( זוגיa ( מטריצהa , גודלa ));
  Console.ReadLine();
}
}
}

