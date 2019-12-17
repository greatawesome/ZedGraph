using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace ZedGraph.XmlPersistence
{
  public class StateWriter
  {
    /// <summary>
    /// The destination for graph state. 
    /// </summary>
    private readonly XmlWriter m_xDoc;

    public StateWriter(XmlWriter xDoc)
    {
      m_xDoc = xDoc ?? throw new ArgumentNullException(nameof(xDoc));
    }

    public void WriteState(GraphPane gp)
    {
      WriteTitle(gp);
      WriteXAxis(gp);
      WriteYAxis(gp);
      WriteY2Axis(gp);
      WriteBackgroundFill(gp);
    }

    public void WriteTitle(GraphPane gp)
    {
      m_xDoc.WriteStartElement("title");
      Write(gp.Title);
      m_xDoc.WriteEndElement();
    }

    public void WriteXAxis(GraphPane gp)
    {
      m_xDoc.WriteStartElement("x-axis");
      Write(gp.XAxis);
      m_xDoc.WriteEndElement();
    }

    public void WriteYAxis(GraphPane gp)
    {
      m_xDoc.WriteStartElement("y-axis");
      Write(gp.YAxis);
      m_xDoc.WriteEndElement();
    }

    public void WriteY2Axis(GraphPane gp)
    {
      m_xDoc.WriteStartElement("y2-axis");
      Write(gp.Y2Axis);
      m_xDoc.WriteEndElement();
    }

    public void WriteBackgroundFill(GraphPane gp)
    {
      m_xDoc.WriteStartElement("background-fill");
      Write(gp.Chart.Fill);
      m_xDoc.WriteEndElement();
    }

    private void Write(Fill fill)
    {
      var brFill = fill.Brush as LinearGradientBrush;
      Color clr1, clr2;
      if (brFill != null && brFill.InterpolationColors.Colors.Length == 2)
      {
        clr1 = brFill.InterpolationColors.Colors[0];
        clr2 = brFill.InterpolationColors.Colors[1];
      }
      else
      {
        clr1 = fill.Color;
        clr2 = fill.SecondaryValueGradientColor;
      }
      float fAngle = fill.Angle;

      m_xDoc.WriteElementString("color-1", XmlConvert.ToString(clr1.ToArgb()));
      m_xDoc.WriteElementString("color-2", XmlConvert.ToString(clr2.ToArgb()));
      m_xDoc.WriteElementString("angle", XmlConvert.ToString(fAngle));
    }

    private void Write(Axis Axis)
    {
      m_xDoc.WriteElementString("minimum", XmlConvert.ToString(Axis.Scale.Min));
      m_xDoc.WriteElementString("maximum", XmlConvert.ToString(Axis.Scale.Max));
      m_xDoc.WriteElementString("major-step", XmlConvert.ToString(Axis.Scale.MajorStep));
      m_xDoc.WriteElementString("minor-step", XmlConvert.ToString(Axis.Scale.MinorStep));
      m_xDoc.WriteElementString("major-unit", Axis.Scale.MajorUnit.ToString());
      m_xDoc.WriteElementString("minor-unit", Axis.Scale.MinorUnit.ToString());
      m_xDoc.WriteElementString("minimum-auto", XmlConvert.ToString(Axis.Scale.MinAuto));
      m_xDoc.WriteElementString("maximum-auto", XmlConvert.ToString(Axis.Scale.MaxAuto));
      m_xDoc.WriteElementString("major-step-auto", XmlConvert.ToString(Axis.Scale.MajorStepAuto));
      m_xDoc.WriteElementString("minor-step-auto", XmlConvert.ToString(Axis.Scale.MinorStepAuto));
      m_xDoc.WriteElementString("visible", XmlConvert.ToString(Axis.IsVisible));
      Write(Axis.Title);
    }

    private void Write(AxisLabel Label)
    {
      m_xDoc.WriteElementString("omit-mag", XmlConvert.ToString(Label.IsOmitMag));
      m_xDoc.WriteElementString("title-at-cross", XmlConvert.ToString(Label.IsTitleAtCross));
      Write((GapLabel)Label);
    }

    private void Write(GapLabel Label)
    {
      m_xDoc.WriteElementString("text", Label.Text);
      m_xDoc.WriteElementString("visible", XmlConvert.ToString(Label.IsVisible));
      m_xDoc.WriteElementString("gap", XmlConvert.ToString(Label.Gap));
      m_xDoc.WriteStartElement("font");
      Write(Label.FontSpec);
      m_xDoc.WriteEndElement();
    }

    private void Write(FontSpec Font)
    {
      m_xDoc.WriteElementString("color", XmlConvert.ToString(Font.FontColor.ToArgb()));
      m_xDoc.WriteElementString("family", Font.Family);
      m_xDoc.WriteElementString("bold", XmlConvert.ToString(Font.IsBold));
      m_xDoc.WriteElementString("italic", XmlConvert.ToString(Font.IsItalic));
      m_xDoc.WriteElementString("size", XmlConvert.ToString(Font.Size));
    }
  }
}
