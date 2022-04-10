using Eto;
using Eto.Forms;
using System;

namespace TestGraph.Eto.Mac
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Platforms.Mac64).Run(new MainForm());
        }
    }
}
