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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace Shenon_Fano
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      public struct node
        {

            // for storing symbol 
          public  string sym;

            // for storing probability or frquency 
          public double pro;
          public  int [] arr;
          public  int top;
       

        }

      

            public MainWindow()
        {
            InitializeComponent();
        }
        List <string> part = new List<string> { };
        double[] p = new double []{ };
        string[] Res = new string[] { };

       


        private double [] Propabilities()
        {

           
          OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Файлы txt|*.txt";
            if (OPF.ShowDialog() ==true)
            {
                MessageBox.Show(OPF.FileName);
            }
            StreamReader objReader = new StreamReader(OPF.FileName);
            string s = "";
             
                s= System.IO.File.ReadAllText("D:\\russian_alphabet.txt", System.Text.Encoding.GetEncoding(1251));
                string [] alphabet= s.Split(new char[] { '\r','\n'}, StringSplitOptions.RemoveEmptyEntries);

          
            objReader.Close();
           
            string text2 = mes.Text;
            double [] array = new double[text2.Length];
            Dictionary<string, int> frequency = new Dictionary<string, int>();
            for (int i = 0; i < alphabet.Length; i++)
            {

                frequency[alphabet[i].ToString()] = 0;
            }

            for (int i = 0; i < text2.Length; i++)
            {
                if (frequency.ContainsKey(text2[i].ToString()))
                {
                    frequency[text2[i].ToString()]++;
                    
                }
            }
            for (int i = 0; i < text2.Length; i++)
            {

                double p = 0;
                p = (double)frequency[text2[i].ToString()] / (double)text2.Length;
                array[i] = p;
                
            }

            return array;
        }

        private int FragmentationArray(string mes)
        {
             p = Propabilities();
            double[] decision = new double[p.Length- 1];
            int k = 1;
            double y= 0;
            double s = 0;
            for (int i=0;i<p.Length-1;i++)
            {
               for(int q=0;q<k;q++)
                {
                    s += p[q];

                }
               
                for (int j = p.Length - 1; j > k-1; j--)
                {
                   y  += p[j];

                }
                decision[k-1] = Math.Abs(s - y);
                k++;
                s = 0;
                y = 0;

            }
            int index = 0;
            for(int i=0;i<decision.Length; i++)
            {
                if (decision[i] == decision.Min())
                {
                    index = i;
                }
            }
            return index;


        }

       

        public void Fano1(int L,int h, List<node> myList)
        {

            int top=0;
            int top1 = 0;
            int i, d, k=0, j;
            double pack1 = 0, pack2 = 0, diff1 = 0, diff2 = 0;
            if ((L + 1) == h || L== h ||L > h)
            {
                if (L == h || L > h)
                    return;
                node ns = myList[h];
                ns.arr[++ns.top] = 0;
                myList[h] = ns; 
                ns= myList[L];
                ns.arr[++ns.top] = 1;
                myList[L] = ns;
                
                return;
            }

            else
            {
                node ns = new node();
                for (i = L; i <= h - 1; i++)
                {
                    ns = myList[i];
                    pack1 = pack1 + ns.pro;
                }
                ns = myList[h];
               /* pack2 = pack2 + ns.pro;
                diff1 = pack1 - pack2;
                if (diff1 < 0)
                    diff1 = diff1 * -1;
                j = 2;
                while (j != h - L + 1)
                {
                    k = h - j;
                    pack1 = pack2 = 0;
                    for (i = L; i <= k; i++)
                    {
                        ns = myList[i];
                        pack1 = pack1 + ns.pro;
                    }
                    for (i = h; i > k; i--)
                    {
                        ns = myList[i];
                        pack2 = pack2 + ns.pro;
                    }
                    diff2 = pack1 - pack2;
                    if (diff2 < 0)
                        diff2 = diff2 * -1;
                    if (diff2 >= diff1)
                        break;
                    diff1 = diff2;
                    j++;
                }*/
                k = FragmentationPart(L, p, h);
                for (i = L; i <= k; i++)
                {
                    ns = myList[i];
                    ns.arr[++ns.top] = 1;
                    myList[i] = ns;
                }
                for (i = k + 1; i < h; i++)
                {
                    ns = myList[i];
                    ns.arr[++ns.top] = 0;
                    myList[i] = ns;
                }


                Fano1(L, k, myList);
                Fano1(k + 1, h, myList);
            }



            /*( if (L < R)
             {
                 // Fano(n + 1, R);
         ..        L = n + 1;
                 goto Point;
             }
             if (L >= R)

             }*/

            // Fano1(L, n);

        }
        
        private int FragmentationPart(int L, double [] p, int R)
        {
            List<double> p1 = new List<double>() { };

            for(int i=L;i<=R;i++)
            {
                p1.Add( p[i]);
            }
            double[] decision = new double[p1.Count- 1];
            int k = 1;
            double y = 0;
            double s = 0;
            for (int i = 0; i < p1.Count - 1; i++)
            {
                for (int q = 0; q < k; q++)
                {
                    s += p1[q];

                }

                for (int j = p1.Count - 1; j > k - 1; j--)
                {
                    y += p1[j];

                }
                decision[k - 1] = Math.Abs(s - y);
                k++;
                s = 0;
                y = 0;

            }
            int index = 0;
            for (int i = 0; i < decision.Length; i++)
            {
                if (decision[i] == decision.Min())
                {
                    index = i;
                }
            }
            return index+L;

        }
            /*private string [] Split(string a, int index)
        {
            string copy = a;
            try
            {
                string b = copy.Substring(0, index);
                string c = copy.Substring(index, copy.Length - index);
                string[] res = new string[2];
                res[0] = b;
                res[1] = c;
                return res;
            }
            catch
            {
                return null;

            }
        }*/

        private void Main()
        {
          
            string message = mes.Text;
            Res = new string[message.Length];
           int index= FragmentationArray(message);
            int L = 0;
            int R = message.Length-1;
            int n =0;
            int k = 0;
            List<node> myList = new List<node>(message.Length);
            int[] arr = new int[message.Length] ;
            int[] arr1 = new int[message.Length];

            for(int i=0;i<myList.Count;i++)
            {
                arr[i] = 0;
                arr1[i] = 0;
            }
       
     
            for (int i = 0; i < message.Length; i++)
            {
                node ns = new node();
                ns.pro = p[i];
                ns.arr=arr;
                myList.Add(ns);

            }
            
            Fano1(0, message.Length - 1, myList);
            for (int i =message.Length - 1; i >= 0; i--)
            {
                Result.Content+= "\n\t" +message[i]+  "\t\t" + myList[i].pro+ "\t";
                arr = myList[i].arr ;
                for (int j = 0; j <= myList[i].top; j++)
                    Result.Content += arr[j].ToString();
            }

        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main();
        }
    }
}
