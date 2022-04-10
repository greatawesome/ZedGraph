using Eto;
using Eto.Forms;
using System;

namespace TestGraph.Eto.Gtk
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Platforms.Gtk).Run(new MainForm());
        }
    }
}
