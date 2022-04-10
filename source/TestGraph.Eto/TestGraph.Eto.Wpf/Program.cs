using Eto;
using Eto.Forms;
using System;

namespace TestGraph.Eto.Wpf
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Platforms.Wpf).Run(new MainForm());
        }
    }
}
