using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Markup;
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
      Write("background-fill", gp.Chart.Fill);
    }

    private void Write(string strNodeName, Fill fill)
    {
      m_xDoc.WriteStartElement(strNodeName);
      Write("color", fill.Color);
      Write("secondary-color", fill.SecondaryValueGradientColor);
      m_xDoc.WriteElementString("angle", XmlConvert.ToString(fill.Angle));
      m_xDoc.WriteElementString("fill-type", fill.Type.ToString());
      m_xDoc.WriteElementString("scaled", XmlConvert.ToString(fill.IsScaled));
      m_xDoc.WriteElementString("align-h", fill.AlignH.ToString());
      m_xDoc.WriteElementString("align-v", fill.AlignV.ToString());
      m_xDoc.WriteElementString("range-min", XmlConvert.ToString(fill.RangeMin));
      m_xDoc.WriteElementString("range-max", XmlConvert.ToString(fill.RangeMax));
      m_xDoc.WriteElementString("range-default", XmlConvert.ToString(fill.RangeDefault));
      if (fill.Type == FillType.Brush)
      {
        const string BrushNodeName = "brush";
        switch (fill.Brush)
        {
          case SolidBrush sb:
            Write(BrushNodeName, sb);
            break;

          case LinearGradientBrush lgb:
            Write(BrushNodeName, lgb);
            break;

          case TextureBrush tb:
            Write(BrushNodeName, tb);
            break;

          default:
            break;
        }
      }

      m_xDoc.WriteEndElement();
    }

    private void Write(string strNodeName, SolidBrush sb)
    {
      if (sb != null)
      {
        m_xDoc.WriteStartElement(strNodeName);
        m_xDoc.WriteAttributeString("type", "solid-brush");
        Write("color", sb.Color);
        m_xDoc.WriteEndElement();
      }
    }

    private void Write(string strNodeName, LinearGradientBrush lgb)
    {
      if (lgb != null)
      {
        m_xDoc.WriteStartElement(strNodeName);
        m_xDoc.WriteAttributeString("type", "linear-gradient-brush");
        m_xDoc.WriteElementString("gama-correction", XmlConvert.ToString(lgb.GammaCorrection));
        Write("blend", lgb.Blend);
        Write("interpolation-colors", lgb.InterpolationColors);
        Write("linear-colors", lgb.LinearColors);
        Write("rectangle", lgb.Rectangle);
        Write("transform", lgb.Transform);
        m_xDoc.WriteElementString("wrap-mode", lgb.WrapMode.ToString());
        m_xDoc.WriteEndElement();
      }
    }

    private void Write(string strNodeName, TextureBrush tb)
    {
      if (tb != null)
      {
        m_xDoc.WriteStartElement(strNodeName);
        m_xDoc.WriteAttributeString("type", "texture-brush");
        Write("image", tb.Image);
        Write("transform", tb.Transform);
        m_xDoc.WriteElementString("wrap-mode", tb.WrapMode.ToString());
        m_xDoc.WriteEndElement();
      }
    }

    private void Write(string strNodeName, Image img)
    {
      if (img != null)
      {
        using (var ms = new MemoryStream())
        {
          img.Save(ms, img.RawFormat);
          byte[] abyImageData = ms.ToArray();

          m_xDoc.WriteStartElement(strNodeName);
          m_xDoc.WriteAttributeString("image-format", img.RawFormat.ToString());
          m_xDoc.WriteBase64(abyImageData, 0, abyImageData.Length);
          m_xDoc.WriteEndElement();
        }
      }
    }

    private void Write(string strNodeName, Matrix m)
    {
      if (m != null)
      {
        Write(strNodeName, m.Elements, "m");
      }
    }

    private void Write(string strNodeName, RectangleF rectangle)
    {
      m_xDoc.WriteStartElement(strNodeName);
      m_xDoc.WriteAttributeString("x", XmlConvert.ToString(rectangle.X));
      m_xDoc.WriteAttributeString("y", XmlConvert.ToString(rectangle.Y));
      m_xDoc.WriteAttributeString("w", XmlConvert.ToString(rectangle.Width));
      m_xDoc.WriteAttributeString("h", XmlConvert.ToString(rectangle.Height));
      m_xDoc.WriteEndElement();
    }

    private void Write(string strNodeName, Color[] aColors)
    {
      if (aColors != null)
      {
        m_xDoc.WriteStartElement(strNodeName);
        foreach (Color c in aColors)
        {
          Write("color", c);
        }
        m_xDoc.WriteEndElement();
      }
    }

    private void Write(string strNodeName, ColorBlend cb)
    {
      if (cb != null)
      {
        m_xDoc.WriteStartElement(strNodeName);
        Write("colors", cb.Colors);
        Write("positions", cb.Positions, "p");
        m_xDoc.WriteEndElement();
      }
    }

    private void Write(string strNodeName, Blend blend)
    {
      if (blend != null)
      {
        m_xDoc.WriteStartElement(strNodeName);
        Write("factors", blend.Factors, "f");
        Write("positions", blend.Positions, "p");
        m_xDoc.WriteEndElement();
      }
    }

    private void Write(string strNodeName, float[] aValues, string strValueName = "v")
    {
      m_xDoc.WriteStartElement(strNodeName);
      foreach (float f in aValues)
      {
        m_xDoc.WriteElementString(strValueName, XmlConvert.ToString(f));
      }
      m_xDoc.WriteEndElement();
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
      Write("color", Font.FontColor);
      m_xDoc.WriteElementString("family", Font.Family);
      m_xDoc.WriteElementString("bold", XmlConvert.ToString(Font.IsBold));
      m_xDoc.WriteElementString("italic", XmlConvert.ToString(Font.IsItalic));
      m_xDoc.WriteElementString("underline", XmlConvert.ToString(Font.IsUnderline));
      m_xDoc.WriteElementString("angle", XmlConvert.ToString(Font.Angle));
      m_xDoc.WriteElementString("string-alignment", Font.StringAlignment.ToString());
      m_xDoc.WriteElementString("size", XmlConvert.ToString(Font.Size));
      m_xDoc.WriteStartElement("border");
      Write(Font.Border);
      m_xDoc.WriteEndElement();

      Write("fill", Font.Fill);

      m_xDoc.WriteElementString("anti-alias", XmlConvert.ToString(Font.IsAntiAlias));
      m_xDoc.WriteElementString("drop-shadow", XmlConvert.ToString(Font.IsDropShadow));
      Write("drop-shadow-color", Font.DropShadowColor);
      m_xDoc.WriteElementString("drop-shadow-angle", XmlConvert.ToString(Font.DropShadowAngle));
      m_xDoc.WriteElementString("drop-shadow-offset", XmlConvert.ToString(Font.DropShadowOffset));


    }

    private void Write(Border border)
    {
      m_xDoc.WriteElementString("inflate-factor", XmlConvert.ToString(border.InflateFactor));
      Write((LineBase)border);
    }

    private void Write(LineBase lb)
    {
      Write("color", lb.Color);
      m_xDoc.WriteElementString("style", lb.Style.ToString());
      m_xDoc.WriteElementString("dash-on", XmlConvert.ToString(lb.DashOn));
      m_xDoc.WriteElementString("dash-off", XmlConvert.ToString(lb.DashOff));
      m_xDoc.WriteElementString("width", XmlConvert.ToString(lb.Width));
      m_xDoc.WriteElementString("visible", XmlConvert.ToString(lb.IsVisible));
      m_xDoc.WriteElementString("anti-alias", XmlConvert.ToString(lb.IsAntiAlias));

      Write("gradient-fill", lb.GradientFill);
    }

    private void Write(string strNodeName, Color clr)
    {
      m_xDoc.WriteStartElement(strNodeName);
      if (clr.IsNamedColor)
      {
        m_xDoc.WriteAttributeString("name", clr.Name);
      }
      m_xDoc.WriteAttributeString("argb", XmlConvert.ToString(clr.ToArgb()));
      m_xDoc.WriteEndElement();
    }
  }
}
