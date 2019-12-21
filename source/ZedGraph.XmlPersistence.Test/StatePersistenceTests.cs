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

    private void Check(XAxis Test, XAxis Original)
    {
      // Ignore cyclic references because scale points back to axis. 
      // Ignore object handles because brushes have an NativeBrush property that points to a GDI+ brush handle which
      // is created for each brush. 
      Test.Should().BeEquivalentTo(Original, options => options.IgnoringCyclicReferences().Excluding(x => IsObjectHandle(x)));
    }

    [TestMethod]
    public void TestXAxisPersistence()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");
      var XAxis = Template.XAxis;

      XAxis.Scale.Min = 10; 
      XAxis.Scale.Max = 100;
      XAxis.Scale.MajorStep = 20;
      XAxis.Scale.MinorStep = 5;
      XAxis.Scale.Exponent = 2;
      XAxis.Scale.BaseTic = 15;
      XAxis.Scale.MajorUnit = DateUnit.Minute;
      XAxis.Scale.MinorUnit = DateUnit.Second;
      XAxis.Scale.MinAuto = ! XAxis.Scale.MinAuto;
      XAxis.Scale.MaxAuto = ! XAxis.Scale.MaxAuto;
      XAxis.Scale.MajorStepAuto = ! XAxis.Scale.MajorStepAuto;
      XAxis.Scale.MinorStepAuto = ! XAxis.Scale.MinorStepAuto;
      XAxis.Scale.FormatAuto = ! XAxis.Scale.FormatAuto;
      XAxis.Scale.Format = "0.00";
      XAxis.Scale.Mag = 10;
      XAxis.Scale.MagAuto = !XAxis.Scale.MagAuto;
      XAxis.Scale.MinGrace = 2;
      XAxis.Scale.MaxGrace = 1.3;
      XAxis.Scale.Align = AlignP.Outside;
      XAxis.Scale.AlignH = AlignH.Left;
      CreateFontSpec(XAxis.Scale.FontSpec);
      XAxis.Scale.LabelGap = 4.2f;
      XAxis.Scale.IsLabelsInside = !XAxis.Scale.IsLabelsInside;
      XAxis.Scale.IsSkipFirstLabel = !XAxis.Scale.IsSkipFirstLabel;
      XAxis.Scale.IsSkipLastLabel = !XAxis.Scale.IsSkipLastLabel;
      XAxis.Scale.IsSkipCrossLabel = !XAxis.Scale.IsSkipCrossLabel;
      XAxis.Scale.IsReverse = !XAxis.Scale.IsReverse;
      XAxis.Scale.IsUseTenPower = !XAxis.Scale.IsUseTenPower;
      XAxis.Scale.IsPreventLabelOverlap = !XAxis.Scale.IsPreventLabelOverlap;
      XAxis.Scale.IsVisible = !XAxis.Scale.IsVisible;

      XAxis.Cross = 1.2;
      XAxis.CrossAuto = false;
      XAxis.MinSpace = 4;
      XAxis.Color = Color.Bisque;


      {
        var mg = XAxis.MajorGrid;
        mg.Color = Color.BlanchedAlmond;
        mg.DashOff = 3;
        mg.DashOn = 7;
        mg.IsVisible = !mg.IsVisible;
        mg.IsZeroLine = !mg.IsZeroLine;
        mg.PenWidth = 8;
      }

      {
        var mg = XAxis.MinorGrid;
        mg.Color = Color.AliceBlue;
        mg.DashOff = 4;
        mg.DashOn = 6;
        mg.IsVisible = !mg.IsVisible;
        mg.PenWidth = 10;
      }

      {
        var mt = XAxis.MajorTic;
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
        var mt = XAxis.MinorTic;
        mt.Color = Color.IndianRed;
        mt.IsCrossInside = !mt.IsCrossInside;
        mt.IsCrossOutside = !mt.IsCrossOutside;
        mt.IsInside = !mt.IsInside;
        mt.IsOpposite = !mt.IsOpposite;
        mt.IsOutside = !mt.IsOutside;
        mt.PenWidth = 2.32f;
        mt.Size = 3.2f;
      }

      XAxis.IsVisible = !XAxis.IsVisible;
      XAxis.IsAxisSegmentVisible = !XAxis.IsAxisSegmentVisible;
      XAxis.AxisGap = 8;

      var t = XAxis.Title;
      CreateFontSpec(t.FontSpec);
      t.Gap = 47;
      t.IsOmitMag = !t.IsOmitMag;
      t.IsTitleAtCross = !t.IsTitleAtCross;
      t.IsVisible = !t.IsVisible;


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

  }
}
