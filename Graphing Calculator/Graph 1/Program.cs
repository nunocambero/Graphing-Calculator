using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph_1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainApp());

            Console.Write("Enter the desired function to graph: ");
            string function = Console.ReadLine();

            Console.Write("Enter the desired range for the x-axis: ");
            int range = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the desired step value for x: ");
            int step = Convert.ToInt32(Console.ReadLine());

            List<int> rangeList = new List<int>();
            for (int i = 0; i < range; i += step)
            {
                rangeList.Add(i);
            }


        }
    }
}
