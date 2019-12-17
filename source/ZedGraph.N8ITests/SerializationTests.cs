using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Text;
using System.Diagnostics;
using FluentAssertions.Equivalency;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ZedGraph.N8ITests
{
  [TestClass]
  public class SerializationTests
  {
    /// <summary>
    /// Test that we can serialize and deserialize a default <see cref="FontSpec"/> object. 
    /// </summary>
    [TestMethod]
    public void TestSerializeDefaultFontSpec()
    {
      var Original = new FontSpec();
      var xs = new XmlSerializer(Original.GetType());
      using (var ms = new MemoryStream())
      {
        using (var xw = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true }))
        {
          xs.Serialize(xw, Original);
        }

        ms.Seek(0, SeekOrigin.Begin);
        using (var xr = XmlReader.Create(ms))
        {
          var Copy = xs.Deserialize(xr);

          Copy.Should().BeEquivalentTo(Original, options => options.Excluding(x => IsNativeBrush(x)));
        }
      }
    }

    /// <summary>
    /// Test that we can serialize and deserialize a more complex <see cref="FontSpec"/> object. 
    /// </summary>
    [TestMethod]
    public void TestSerializeComplexFontSpec()
    {
      var Original = new FontSpec();
      Original.Angle = 12.3f;
      Original.Border.Color = Color.FromArgb(120, 10, 20, 40);
      Original.Border.GradientFill.Brush = new SolidBrush(Color.Orange);
      Original.Border.GradientFill.Color = Color.Purple;
      Original.Border.GradientFill.IsVisible = true;
      Original.Border.IsAntiAlias = true;
      Original.DropShadowColor = Color.Aqua;
      Original.Family = "Arial";
      Original.Fill.Brush = new LinearGradientBrush(new Point(10,10), new Point(100,100), Color.Green, Color.Yellow);
      

      var xs = new XmlSerializer(Original.GetType());
      using (var ms = new MemoryStream())
      {
        using (var xw = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true }))
        {
          xs.Serialize(xw, Original);
        }

        WriteXml(ms);

        ms.Seek(0, SeekOrigin.Begin);
        using (var xr = XmlReader.Create(ms))
        {
          var Copy = xs.Deserialize(xr);

          Copy.Should().BeEquivalentTo(Original, options => options.Excluding(x => IsNativeBrush(x)));
        }
      }
    }

    [TestMethod]
    public void TestLinearScale()
    {
      var Axis = new XAxis("My X Axis");
      var Original = Axis.Scale;
      var xs = new XmlSerializer(Original.GetType());
      using (var ms = new MemoryStream())
      {
        using (var xw = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true }))
        {
          xs.Serialize(xw, Original);
        }

        ms.Seek(0, SeekOrigin.Begin);
        using (var xr = XmlReader.Create(ms))
        {
          var Copy = xs.Deserialize(xr);

          Copy.Should().BeEquivalentTo(Original, options => options.Excluding(x => IsNativeBrush(x)));
        }
      }

    }

    /// <summary>
    /// Test that we can serialize and deserialize a default <see cref="XAxis"/> object. 
    /// </summary>
    [TestMethod]
    public void TestSerializeDefaultXAxis()
    {
      var Original = new XAxis("My X Axis");
      Original.Scale.Max = 100; 

      var xs = new XmlSerializer(Original.GetType());
      using (var ms = new MemoryStream())
      {
        using (var xw = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true }))
        {
          xs.Serialize(xw, Original);
        }

        WriteXml(ms);

        ms.Seek(0, SeekOrigin.Begin);
        using (var xr = XmlReader.Create(ms))
        {
          var Copy = xs.Deserialize(xr);

          // Exclude fields because the scale references the axis. 
          // Exclude NativeBrush properties since that is a handle to the brush
          // that will be different for each brush. 
          Copy.Should().BeEquivalentTo(Original, options => options.ExcludingFields().Excluding(x => IsNativeBrush(x)));
        }
      }

    }

    private void WriteXml(MemoryStream ms)
    {
      string strXml = ASCIIEncoding.UTF8.GetString(ms.ToArray());
      Debug.WriteLine(strXml);
    }

    static bool IsNativeBrush(IMemberInfo mi)
    {
      return mi.SelectedMemberPath.EndsWith(".NativeBrush");
    }
  }
}
