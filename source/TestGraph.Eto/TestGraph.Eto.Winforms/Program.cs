using Eto;
using Eto.Forms;
using System;

namespace TestGraph.Eto.WinForms
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Platforms.WinForms).Run(new MainForm());
        }
    }
}
