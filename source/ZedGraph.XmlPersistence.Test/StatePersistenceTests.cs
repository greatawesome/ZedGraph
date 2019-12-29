using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Xml;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZedGraph.XmlPersistence.Test.Properties;

namespace ZedGraph.XmlPersistence.Test
{

  [TestClass]
  public class StatePersistenceTests
  {
    private static readonly Random m_RNG = new Random();

    [TestMethod]
    public void PersistTitle()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Make sure no values have their defaults. 
      var Title = Template.Title;
      Title.Gap = 5.0f;
      Title.IsVisible = !Title.IsVisible;

      var Font = Title.FontSpec;
      CreateFontSpec(Font);

      // Persist the object
      var Persisted = Write(x => x.WriteTitle(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreTitle(Target);

      // Check fidelity. 
      Target.Title.Should().BeEquivalentTo(Template.Title, options => options.Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestDefaultBackgroundFill()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Persist the object
      var Persisted = Write(x => x.WriteBackgroundFill(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreBackgroundFill(Target);

      // Check fidelity. 
      Target.Chart.Fill.Should().BeEquivalentTo(Template.Chart.Fill, options => options.Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestSolidBackgroundFill()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");
      Template.Chart.Fill = new Fill(Color.Purple);

      // Persist the object
      var Persisted = Write(x => x.WriteBackgroundFill(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreBackgroundFill(Target);

      // Check fidelity. 
      Target.Chart.Fill.Should().BeEquivalentTo(Template.Chart.Fill, options => options.Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestGradientBackgroundFill()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");
      Template.Chart.Fill = new Fill(Color.AliceBlue, Color.Azure, 11f);

      // Persist the object
      var Persisted = Write(x => x.WriteBackgroundFill(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreBackgroundFill(Target);

      // Check fidelity. 
      Target.Chart.Fill.Should().BeEquivalentTo(Template.Chart.Fill, options => options.Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestTextureBackgroundFill()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");
      Template.Chart.Fill = new Fill(Resources.TrafficCone, WrapMode.Clamp);

      // Persist the object
      var Persisted = Write(x => x.WriteBackgroundFill(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreBackgroundFill(Target);

      // Check fidelity. 
      Target.Chart.Fill.Should().BeEquivalentTo(Template.Chart.Fill, options => options.Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestXAxisDefault()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Persist the object
      var Persisted = Write(x => x.WriteXAxis(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreXAxis(Target);

      // Check fidelity. 
      Check(Target.XAxis, Template.XAxis);

    }

    [TestMethod]
    public void TestXAxisPersistence()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      FillAxisValues(Template.XAxis);

      // Persist the object
      var Persisted = Write(x => x.WriteXAxis(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreXAxis(Target);

      // Check fidelity. 
      Check(Target.XAxis, Template.XAxis);

    }

    [TestMethod]
    public void TestYAxisDefault()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Persist the object
      var Persisted = Write(x => x.WriteYAxis(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreYAxis(Target);

      // Check fidelity. 
      Check(Target.YAxis, Template.YAxis);
    }

    [TestMethod]
    public void TestYAxisPersistence()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      FillAxisValues(Template.YAxis);

      // Persist the object
      var Persisted = Write(x => x.WriteYAxis(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreYAxis(Target);

      // Check fidelity. 
      Check(Target.YAxis, Template.YAxis);
    }

    [TestMethod]
    public void TestYAxisLog()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      FillAxisValues(Template.YAxis);
      Template.YAxis.Type = AxisType.Log;

      // Persist the object
      var Persisted = Write(x => x.WriteYAxis(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreYAxis(Target);

      // Check fidelity. 
      Check(Target.YAxis, Template.YAxis);
    }

    [TestMethod]
    public void TestY2AxisDefault()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Persist the object
      var Persisted = Write(x => x.WriteY2Axis(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreY2Axis(Target);

      // Check fidelity. 
      Check(Target.Y2Axis, Template.Y2Axis);
    }

    [TestMethod]
    public void TestY2AxisPersistence()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      FillAxisValues(Template.Y2Axis);
      Template.Y2Axis.Title.Text= "Y2 Axis";

      // Persist the object
      var Persisted = Write(x => x.WriteY2Axis(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreY2Axis(Target);

      // Check fidelity. 
      Check(Target.Y2Axis, Template.Y2Axis);
    }

    [TestMethod]
    public void TestMultipleYAxes()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");
      Template.YAxisList.Add("Another Y Axis");

      FillAxisValues(Template.YAxis);
      FillAxisValues(Template.YAxisList[1]);

      // Persist the object
      var Persisted = Write(x => x.WriteYAxisList(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreYAxisCollection(Target);

      // Check fidelity. 
      Target.YAxisList.Should().BeEquivalentTo(Template.YAxisList, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestMultipleY2Axes()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");
      Template.Y2AxisList.Add("Another Y Axis");

      FillAxisValues(Template.Y2Axis);
      FillAxisValues(Template.Y2AxisList[1]);

      // Persist the object
      var Persisted = Write(x => x.WriteY2AxisList(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreY2AxisCollection(Target);

      // Check fidelity. 
      Target.Y2AxisList.Should().BeEquivalentTo(Template.Y2AxisList, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestDefaultChartPersistence()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Persist the object
      var Persisted = Write(x => x.WriteChart(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreChart(Target);

      // Check fidelity. 
      Target.Chart.Should().BeEquivalentTo(Template.Chart, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestChartPersistence()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");
      Chart chrt = Template.Chart;

      chrt.Rect = new RectangleF(40, 40, 100, 100);
      chrt.Fill = new Fill(new Color[] { Color.Red, Color.Green, Color.Blue }, 12.43f);

      var Border = chrt.Border;
      Border.InflateFactor *= 2;
      Border.Color = Color.Tan;
      Border.Style = DashStyle.Dot;
      Border.DashOff *= 2;
      Border.DashOn *= 3;
      Border.Width *= 3;
      Border.IsVisible = !Border.IsVisible;
      Border.IsAntiAlias = !Border.IsAntiAlias;
      Border.GradientFill = new Fill(Color.OldLace, Color.AliceBlue, 12f);

      // Persist the object
      var Persisted = Write(x => x.WriteChart(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreChart(Target);

      // Check fidelity. 
      Target.Chart.Should().BeEquivalentTo(Template.Chart, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestNoCursors()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Persist the object
      var Persisted = Write(x => x.WriteCursors(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreCursors(Target);

      // Check fidelity. 
      Target.Cursors.Should().BeEquivalentTo(Template.Cursors, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestCursors()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      Template.Cursors.Add(new CursorObj(0.12, CursorOrientation.Vertical) { CoordinateUnit = CoordType.ChartFraction, Line = new LineBase(Color.AliceBlue), Name = "My cursor" });
      Template.Cursors.Add(new CursorObj(12.3, CursorOrientation.Horizontal) { CoordinateUnit = CoordType.AxisXY2Scale, Line = new LineBase(Color.Tomato), Name = "Another cursor" });

      // Persist the object
      var Persisted = Write(x => x.WriteCursors(Template));
      Dump(Persisted);

      // Restore it. 
      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreCursors(Target);

      // Check fidelity. 
      Target.Cursors.Should().BeEquivalentTo(Template.Cursors, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }


    #region Helpers
    private XmlDocument Write(Action<StateWriter> fn)
    {
      using (var ms = new MemoryStream())
      using (var xw = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true }))
      {
        var sw = new StateWriter(xw);
        xw.WriteStartDocument();
        xw.WriteStartElement("state");
        fn(sw);
        xw.WriteEndElement();
        xw.WriteEndDocument();
        xw.Flush();

        var xd = new XmlDocument();
        ms.Seek(0, SeekOrigin.Begin);
        xd.Load(ms);
        return xd;
      }
    }

    private void Check(Axis Test, Axis Original)
    {
      // Ignore cyclic references because scale points back to axis. 
      // Ignore object handles because brushes have an NativeBrush property that points to a GDI+ brush handle which
      // is created for each brush. 
      Test.Should().BeEquivalentTo(Original, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }


    private void Dump(XmlDocument xd)
    {
      using (var ms = new MemoryStream())
      using (var xw = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true }))
      {
        xd.Save(xw);
        xw.Flush();

        string strContent = UTF8Encoding.UTF8.GetString(ms.ToArray());
        Debug.WriteLine(strContent);
      }
    }

    private bool IsObjectHandle(IMemberInfo mi)
    {
      return mi.SelectedMemberPath.EndsWith(".NativeBrush");
    }
    #endregion

    #region Setup values. 
    private static void CreateFontSpec(FontSpec Font)
    {
      Font.Angle = m_RNG.Next(2, 50);
      Font.FontColor = Color.Beige;
      Font.Family = "Comic Sans";
      Font.IsBold = !Font.IsBold;
      Font.IsItalic = !Font.IsItalic;
      Font.IsUnderline = !Font.IsUnderline;
      Font.IsAntiAlias = !Font.IsAntiAlias;
      Font.IsDropShadow = !Font.IsDropShadow;
      Font.StringAlignment = StringAlignment.Near;
      Font.Size *= m_RNG.Next(2, 5);
      Font.DropShadowColor = Color.Blue;
      Font.DropShadowOffset = m_RNG.Next(10, 50);
      Font.DropShadowAngle = 11;
      Font.Fill = new Fill(Color.AliceBlue, Color.Azure, 11f);

      var Border = Font.Border;
      Border.InflateFactor *= 2;
      Border.Color = Color.Tan;
      Border.Style = DashStyle.Dot;
      Border.DashOff *= 2;
      Border.DashOn *= 3;
      Border.Width *= 3;
      Border.IsVisible = !Border.IsVisible;
      Border.IsAntiAlias = !Border.IsAntiAlias;

      Border.GradientFill = new Fill(Color.OldLace, Color.AliceBlue, 12f);
    }

    private static void FillAxisValues(Axis Axis)
    {
      Axis.Scale.Min = m_RNG.Next(10, 40);
      Axis.Scale.Max = m_RNG.Next(100, 200);
      Axis.Scale.MajorStep = 20;
      Axis.Scale.MinorStep = 5;
      Axis.Scale.Exponent = 2;
      Axis.Scale.BaseTic = 15;
      Axis.Scale.MajorUnit = DateUnit.Minute;
      Axis.Scale.MinorUnit = DateUnit.Second;
      Axis.Scale.MinAuto = !Axis.Scale.MinAuto;
      Axis.Scale.MaxAuto = !Axis.Scale.MaxAuto;
      Axis.Scale.MajorStepAuto = !Axis.Scale.MajorStepAuto;
      Axis.Scale.MinorStepAuto = !Axis.Scale.MinorStepAuto;
      Axis.Scale.FormatAuto = !Axis.Scale.FormatAuto;
      Axis.Scale.Format = "0.00";
      Axis.Scale.Mag = 10;
      Axis.Scale.MagAuto = !Axis.Scale.MagAuto;
      Axis.Scale.MinGrace = 2;
      Axis.Scale.MaxGrace = 1.3;
      Axis.Scale.Align = AlignP.Outside;
      Axis.Scale.AlignH = AlignH.Left;
      CreateFontSpec(Axis.Scale.FontSpec);
      Axis.Scale.LabelGap = 4.2f;
      Axis.Scale.IsLabelsInside = !Axis.Scale.IsLabelsInside;
      Axis.Scale.IsSkipFirstLabel = !Axis.Scale.IsSkipFirstLabel;
      Axis.Scale.IsSkipLastLabel = !Axis.Scale.IsSkipLastLabel;
      Axis.Scale.IsSkipCrossLabel = !Axis.Scale.IsSkipCrossLabel;
      Axis.Scale.IsReverse = !Axis.Scale.IsReverse;
      Axis.Scale.IsUseTenPower = !Axis.Scale.IsUseTenPower;
      Axis.Scale.IsPreventLabelOverlap = !Axis.Scale.IsPreventLabelOverlap;
      Axis.Scale.IsVisible = !Axis.Scale.IsVisible;

      Axis.Cross = 1.2;
      Axis.CrossAuto = false;
      Axis.MinSpace = 4;
      Axis.Color = Color.Bisque;


      {
        var mg = Axis.MajorGrid;
        mg.Color = Color.BlanchedAlmond;
        mg.DashOff = 3;
        mg.DashOn = 7;
        mg.IsVisible = !mg.IsVisible;
        mg.IsZeroLine = !mg.IsZeroLine;
        mg.PenWidth = 8;
      }

      {
        var mg = Axis.MinorGrid;
        mg.Color = Color.AliceBlue;
        mg.DashOff = 4;
        mg.DashOn = 6;
        mg.IsVisible = !mg.IsVisible;
        mg.PenWidth = 10;
      }

      {
        var mt = Axis.MajorTic;
        mt.Color = Color.PaleGoldenrod;
        mt.IsBetweenLabels = !mt.IsBetweenLabels;
        mt.IsCrossInside = !mt.IsCrossInside;
        mt.IsCrossOutside = !mt.IsCrossOutside;
        mt.IsInside = !mt.IsInside;
        mt.IsOpposite = !mt.IsOpposite;
        mt.IsOutside = !mt.IsOutside;
        mt.PenWidth = 11;
        mt.Size = 3;
      }

      {
        var mt = Axis.MinorTic;
        mt.Color = Color.IndianRed;
        mt.IsCrossInside = !mt.IsCrossInside;
        mt.IsCrossOutside = !mt.IsCrossOutside;
        mt.IsInside = !mt.IsInside;
        mt.IsOpposite = !mt.IsOpposite;
        mt.IsOutside = !mt.IsOutside;
        mt.PenWidth = 2.32f;
        mt.Size = 3.2f;
      }

      Axis.IsVisible = !Axis.IsVisible;
      Axis.IsAxisSegmentVisible = !Axis.IsAxisSegmentVisible;
      Axis.AxisGap = 8;

      var t = Axis.Title;
      CreateFontSpec(t.FontSpec);
      t.Gap = 47;
      t.IsOmitMag = !t.IsOmitMag;
      t.IsTitleAtCross = !t.IsTitleAtCross;
      t.IsVisible = !t.IsVisible;
    }
    #endregion


  }
}
