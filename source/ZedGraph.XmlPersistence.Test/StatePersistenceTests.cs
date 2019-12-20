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

namespace ZedGraph.XmlPersistence.Test
{
  [TestClass]
  public class StatePersistenceTests
  {
    [TestMethod]
    public void PersistTitle()
    {
      GraphPane Template = new GraphPane(new RectangleF(10, 12, 100, 150), "My graph", "X Title", "Y Title");

      // Make sure no values have their defaults. 
      var Title = Template.Title;
      Title.Gap = 5.0f;
      Title.IsVisible = !Title.IsVisible;

      var Font = Title.FontSpec;
      Font.Angle = 11f;
      Font.FontColor = Color.Beige;
      Font.Family = "Comic Sans";
      Font.IsBold = !Font.IsBold;
      Font.IsItalic = !Font.IsItalic;
      Font.IsUnderline = !Font.IsUnderline;
      Font.IsAntiAlias = !Font.IsAntiAlias;
      Font.IsDropShadow = !Font.IsDropShadow;
      Font.StringAlignment = StringAlignment.Near;
      Font.Size *= 2; 
      Font.DropShadowColor = Color.Blue;
      Font.DropShadowOffset = 22;
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

      var Persisted = Write(x => x.WriteTitle(Template));

      Dump(Persisted);

      var Target = new GraphPane();
      var Restorer = new StateRestorer(Persisted.DocumentElement);
      Restorer.RestoreTitle(Target);

      Target.Title.Should().BeEquivalentTo(Template.Title, options=> options.Excluding(x => IsObjectHandle(x)));
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
  }
}
