using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace TestGraph
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();

      var dtStart = new DateTime(2019, 4, 2, 13, 23, 0);
      var dtEnd = new DateTime(2019, 4, 2, 14, 36, 0);
      zgGraph.GraphPane.XAxis.Type = AxisType.Date;

      var xscale = zgGraph.GraphPane.XAxis.Scale;
      xscale.Min = XDate.DateTimeToXLDate(dtStart);
      xscale.Max = XDate.DateTimeToXLDate(dtEnd);
      xscale.MajorStep = 1;
      xscale.MajorUnit = DateUnit.Hour;
      xscale.MinorStep = 30;
      xscale.MinorUnit = DateUnit.Minute;

      var xAxis = zgGraph.GraphPane.XAxis;
      xAxis.MajorGrid.IsVisible = true;
      xAxis.MajorGrid.Color = Color.Blue;
      xAxis.MinorGrid.IsVisible = true;
      xAxis.MinorGrid.Color = Color.AliceBlue;


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
      points.Add(XDate.DateTimeToXLDate(dtEnd -  TimeSpan.FromMinutes(10)), 1);
      points.Add(XDate.DateTimeToXLDate(dtEnd), 0);
      zgGraph.GraphPane.AddCurve("Fish", points, Color.Black);

      zgGraph.AxisChange();
      zgGraph.Invalidate();

    }
  }
}
