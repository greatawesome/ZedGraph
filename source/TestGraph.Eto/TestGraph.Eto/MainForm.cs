using Eto.Drawing;
using Eto.Forms;
using System;
using ZedGraph;

namespace TestGraph.Eto
{
    public partial class MainForm : Form
    {
        private ZedGraphControl zgGraph;

        public MainForm()
        {
            Title = "ZedGraph Eto Test";
            MinimumSize = new Size(200, 200);

            zgGraph = new ZedGraphControl();
            zgGraph.Size = new Size(500, 375);

            //TEST - see if we can get zooming to work
            zgGraph.IsEnableHCursorMove = false;
            zgGraph.IsEnableVCursorMove = false;

            Content = zgGraph;
    //        {
    //            Padding = 10,
    //            Items =
    //            {
    //                zgGraph
				//	// add more controls here
				//}
    //        };

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

            // create menu
            Menu = new MenuBar
            {
                ApplicationItems =
                {
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };

#if false
      var dtStart = new DateTime(2019, 4, 2, 13, 23, 0);
      var dtEnd = new DateTime(2019, 4, 2, 14, 36, 0);
#else
            var dtStart = new DateTime(2017, 3, 19, 13, 23, 0);
            var dtEnd = new DateTime(2017, 4, 8, 14, 36, 0);
#endif
            zgGraph.GraphPane.XAxis.Type = AxisType.Date;

            zgGraph.IsShowPointValues = true;
            zgGraph.IsShowCursorValues = false;

            var xscale = zgGraph.GraphPane.XAxis.Scale;
            xscale.Min = XDate.DateTimeToXLDate(dtStart);
            xscale.Max = XDate.DateTimeToXLDate(dtEnd);
            xscale.MajorStep = 1;
            xscale.MajorUnit = DateUnit.Hour;
            xscale.MinorStep = 30;
            xscale.MinorUnit = DateUnit.Minute;

            var xAxis = zgGraph.GraphPane.XAxis;
            xAxis.MajorGrid.IsVisible = true;
            xAxis.MajorGrid.Color = System.Drawing.Color.Blue;
            xAxis.MinorGrid.IsVisible = true;
            xAxis.MinorGrid.Color = System.Drawing.Color.AliceBlue;


            bool bAuto = true;
            xscale.MinAuto = bAuto;
            xscale.MaxAuto = bAuto;
            xscale.MajorStepAuto = bAuto;
            xscale.MinorStepAuto = bAuto;

            var yscale = zgGraph.GraphPane.YAxis.Scale;
            yscale.MajorStepAuto = false;
            yscale.MajorStep = 1;
            yscale.Min = 0;
            yscale.Max = 10;

            var points = new PointPairList();
            points.Add(XDate.DateTimeToXLDate(dtStart), 2);
            points.Add(XDate.DateTimeToXLDate(dtStart + TimeSpan.FromMinutes(10)), 3);
            points.Add(XDate.DateTimeToXLDate(dtEnd - TimeSpan.FromMinutes(10)), 1);
            points.Add(XDate.DateTimeToXLDate(dtEnd), 0);
            zgGraph.GraphPane.AddCurve("Fish", points, System.Drawing.Color.Black);

#if false
      zgGraph.GraphPane.Cursors.Add(new CursorObj(XDate.DateTimeToXLDate(dtStart + TimeSpan.FromMinutes(5)), CursorOrientation.Vertical));
      zgGraph.GraphPane.Cursors.Add(new CursorObj(XDate.DateTimeToXLDate(dtEnd - TimeSpan.FromMinutes(1)), CursorOrientation.Vertical));

      zgGraph.GraphPane.Cursors.Add(new CursorObj(1.1, CursorOrientation.Horizontal));
      zgGraph.GraphPane.Cursors.Add(new CursorObj(8.2, CursorOrientation.Horizontal));
#endif

            zgGraph.GraphPane.Cursors.Add(new CursorObj(0.1, CursorOrientation.Vertical) { CoordinateUnit = CoordType.ChartFraction });
            zgGraph.GraphPane.Cursors.Add(new CursorObj(0.8, CursorOrientation.Vertical) { CoordinateUnit = CoordType.ChartFraction });

            zgGraph.AxisChange();
            zgGraph.Invalidate();

        }
    }
}
