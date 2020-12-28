using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
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
      WriteYAxisList(gp);
      WriteY2AxisList(gp);
      WriteChart(gp);
      WriteCursors(gp);
    }

    public void WriteGraphPane(GraphPane gp)
    {
      m_xDoc.WriteStartElement("pane");

      m_xDoc.WriteElementString("ignore-initial", XmlConvert.ToString(gp.IsIgnoreInitial));
      m_xDoc.WriteElementString("bounded-ranges", XmlConvert.ToString(gp.IsBoundedRanges));
      m_xDoc.WriteElementString("ignore-missing", XmlConvert.ToString(gp.IsIgnoreMissing));
      m_xDoc.WriteElementString("align-grids", XmlConvert.ToString(gp.IsAlignGrids));
      m_xDoc.WriteElementString("line-type", gp.LineType.ToString());
      m_xDoc.WriteElementString("base-dimension", XmlConvert.ToString(gp.BaseDimension));
      m_xDoc.WriteElementString("title-gap", XmlConvert.ToString(gp.TitleGap));
      m_xDoc.WriteElementString("scale-font", XmlConvert.ToString(gp.IsFontsScaled));
      m_xDoc.WriteElementString("scale-pen-width", XmlConvert.ToString(gp.IsPenWidthScaled));

      Write("rectangle", gp.Rect);
      Write("border", gp.Border);
      Write("fill", gp.Fill);
      Write("margin", gp.Margin);

      // Legend, Title are written separately. 

      m_xDoc.WriteEndElement();
    }

    public void WriteLegend(GraphPane gp)
    {
      m_xDoc.WriteStartElement("legend");

      var lgd = gp.Legend;
      Write("font", lgd.FontSpec);
      Write("border", lgd.Border);
      Write("fill", lgd.Fill);
      Write("location", lgd.Location);

      m_xDoc.WriteElementString("visible", XmlConvert.ToString(lgd.IsVisible));
      m_xDoc.WriteElementString("h-stack", XmlConvert.ToString(lgd.IsHStack));
      m_xDoc.WriteElementString("position", lgd.Position.ToString());
      m_xDoc.WriteElementString("reverse", XmlConvert.ToString(lgd.IsReverse));
      m_xDoc.WriteElementString("gap", XmlConvert.ToString(lgd.Gap));
      m_xDoc.WriteElementString("show-symbols", XmlConvert.ToString(lgd.IsShowLegendSymbols));

      m_xDoc.WriteEndElement();
    }

    public void WriteCursors(GraphPane gp)
    {
      m_xDoc.WriteStartElement("cursors");
      foreach (CursorObj co in gp.Cursors)
      {
        Write(co);
      }
      m_xDoc.WriteEndElement();
    }

    public void WriteChart(GraphPane gp)
    {
      m_xDoc.WriteStartElement("chart");
      WriteChart(gp.Chart);
      m_xDoc.WriteEndElement();
    }

    public void WriteTitle(GraphPane gp)
    {
      m_xDoc.WriteStartElement("title");
      Write(gp.Title);
      m_xDoc.WriteEndElement();
    }

    public void WriteXAxis(GraphPane gp)
    {
      Write("x-axis", gp.XAxis);
    }

    public void WriteYAxis(GraphPane gp)
    {
      Write("y-axis", gp.YAxis);
    }

    public void WriteYAxisList(GraphPane gp)
    {
      WriteAxis("y-axis-list", gp.YAxisList);
    }

    public void WriteY2AxisList(GraphPane gp)
    {
      WriteAxis("y2-axis-list", gp.Y2AxisList);
    }

    public void WriteY2Axis(GraphPane gp)
    {
      Write("y2-axis", gp.Y2Axis);
    }

    public void WriteBackgroundFill(GraphPane gp)
    {
      Write("background-fill", gp.Chart.Fill);
    }

    public void WriteChart(Chart chrt)
    {
      Write("rectangle", chrt.Rect);
      m_xDoc.WriteElementString("auto-rectangle", XmlConvert.ToString(chrt.IsRectAuto));
      Write("fill", chrt.Fill);
      Write("border", chrt.Border);
    }

    private void Write(CursorObj co)
    {
      m_xDoc.WriteStartElement("cursor");

      m_xDoc.WriteElementString("name", co.Name);
      m_xDoc.WriteElementString("location", XmlConvert.ToString(co.Location));
      m_xDoc.WriteElementString("orientation", co.Orientation.ToString());
      m_xDoc.WriteElementString("coordinate-unit", co.CoordinateUnit.ToString());

      m_xDoc.WriteStartElement("line");
      Write(co.Line);
      m_xDoc.WriteEndElement();

      m_xDoc.WriteEndElement();
    }

    private void Write(string strNodeName, Margin m)
    {
      m_xDoc.WriteStartElement(strNodeName);

      m_xDoc.WriteElementString("left", XmlConvert.ToString(m.Left));
      m_xDoc.WriteElementString("right", XmlConvert.ToString(m.Right));
      m_xDoc.WriteElementString("top", XmlConvert.ToString(m.Top));
      m_xDoc.WriteElementString("bottom", XmlConvert.ToString(m.Bottom));


      m_xDoc.WriteEndElement();
    }

    private void Write(string strNodeName, Location l)
    {
      m_xDoc.WriteStartElement(strNodeName);

      m_xDoc.WriteElementString("align-h", l.AlignH.ToString());
      m_xDoc.WriteElementString("align-v", l.AlignV.ToString());
      m_xDoc.WriteElementString("coordinate-frame", l.CoordinateFrame.ToString());
      m_xDoc.WriteElementString("x", XmlConvert.ToString(l.X));
      m_xDoc.WriteElementString("y", XmlConvert.ToString(l.Y));
      m_xDoc.WriteElementString("width", XmlConvert.ToString(l.Width));
      m_xDoc.WriteElementString("height", XmlConvert.ToString(l.Height));

      m_xDoc.WriteEndElement();
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
      if (fill.Brush != null)
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

    private void Write(string strNodeName, float[] aValues, string strValueName = "v")
    {
      m_xDoc.WriteStartElement(strNodeName);
      foreach (float f in aValues)
      {
        m_xDoc.WriteElementString(strValueName, XmlConvert.ToString(f));
      }
      m_xDoc.WriteEndElement();
    }

    private void Write(string strNodeName, string[] astrValues, string strValueName = "t")
    {
      m_xDoc.WriteStartElement(strNodeName);

      foreach (string strValue in astrValues)
      {
        m_xDoc.WriteElementString(strValueName, strValue);
      }

      m_xDoc.WriteEndElement();
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

    private void WriteAxis(string strNodeName, IEnumerable<Axis> Axes)
    {
      m_xDoc.WriteStartElement(strNodeName);

      foreach (var ax in Axes)
      {
        Write("axis", ax);
      }

      m_xDoc.WriteEndElement();
    }

    private void Write(string strNodeName, Axis Axis)
    {
      m_xDoc.WriteStartElement(strNodeName);
      m_xDoc.WriteAttributeString("type", Axis.Type.ToString());

      m_xDoc.WriteElementString("visible", XmlConvert.ToString(Axis.IsVisible));
      m_xDoc.WriteElementString("axis-segment-visible", XmlConvert.ToString(Axis.IsAxisSegmentVisible));
      m_xDoc.WriteElementString("cross", XmlConvert.ToString(Axis.Cross));
      m_xDoc.WriteElementString("cross-auto", XmlConvert.ToString(Axis.CrossAuto));
      m_xDoc.WriteElementString("min-space", XmlConvert.ToString(Axis.MinSpace));
      m_xDoc.WriteElementString("axis-gap", XmlConvert.ToString(Axis.AxisGap));
      Write("color", Axis.Color);
      WriteElement("major-tic", Axis.MajorTic, Write);
      WriteElement("minor-tic", Axis.MinorTic, Write);
      WriteElement("major-grid", Axis.MajorGrid, Write);
      WriteElement("minor-grid", Axis.MinorGrid, Write);

      m_xDoc.WriteStartElement("scale");
      Write(Axis.Scale);
      m_xDoc.WriteEndElement();

      m_xDoc.WriteStartElement("title");
      Write(Axis.Title);
      m_xDoc.WriteEndElement();

      m_xDoc.WriteEndElement();
    }

    private void WriteElement<T>(string strNodeName, T value, Action<T> fnWriter)
    {
      m_xDoc.WriteStartElement(strNodeName);
      fnWriter(value);
      m_xDoc.WriteEndElement();
    }

    private void Write(MajorTic value)
    {
      m_xDoc.WriteElementString("between-labels", XmlConvert.ToString(value.IsBetweenLabels));
      Write((MinorTic)value);
    }

    private void Write(MinorTic value)
    {
      Write("color", value.Color);
      m_xDoc.WriteElementString("size", XmlConvert.ToString(value.Size));
      m_xDoc.WriteElementString("outside", XmlConvert.ToString(value.IsOutside));
      m_xDoc.WriteElementString("inside", XmlConvert.ToString(value.IsInside));
      m_xDoc.WriteElementString("opposite", XmlConvert.ToString(value.IsOpposite));
      m_xDoc.WriteElementString("cross-outside", XmlConvert.ToString(value.IsCrossOutside));
      m_xDoc.WriteElementString("cross-inside", XmlConvert.ToString(value.IsCrossInside));
      m_xDoc.WriteElementString("pen-width", XmlConvert.ToString(value.PenWidth));
    }

    private void Write(MajorGrid value)
    {
      m_xDoc.WriteElementString("zero-line", XmlConvert.ToString(value.IsZeroLine));
      Write((MinorGrid)value);
    }

    private void Write(MinorGrid value)
    {
      m_xDoc.WriteElementString("visible", XmlConvert.ToString(value.IsVisible));
      m_xDoc.WriteElementString("dash-on", XmlConvert.ToString(value.DashOn));
      m_xDoc.WriteElementString("dash-off", XmlConvert.ToString(value.DashOff));
      m_xDoc.WriteElementString("pen-width", XmlConvert.ToString(value.PenWidth));
      Write("color", value.Color);
    }

    private void Write(Scale Scale)
    {
      m_xDoc.WriteElementString("minimum", XmlConvert.ToString(Scale.Min));
      m_xDoc.WriteElementString("maximum", XmlConvert.ToString(Scale.Max));
      m_xDoc.WriteElementString("major-step", XmlConvert.ToString(Scale.MajorStep));
      m_xDoc.WriteElementString("minor-step", XmlConvert.ToString(Scale.MinorStep));
      m_xDoc.WriteElementString("major-unit", Scale.MajorUnit.ToString());
      m_xDoc.WriteElementString("minor-unit", Scale.MinorUnit.ToString());
      m_xDoc.WriteElementString("minimum-auto", XmlConvert.ToString(Scale.MinAuto));
      m_xDoc.WriteElementString("maximum-auto", XmlConvert.ToString(Scale.MaxAuto));
      m_xDoc.WriteElementString("major-step-auto", XmlConvert.ToString(Scale.MajorStepAuto));
      m_xDoc.WriteElementString("minor-step-auto", XmlConvert.ToString(Scale.MinorStepAuto));

      m_xDoc.WriteElementString("exponent", XmlConvert.ToString(Scale.Exponent));
      m_xDoc.WriteElementString("base-tic", XmlConvert.ToString(Scale.BaseTic));
      m_xDoc.WriteElementString("format-auto", XmlConvert.ToString(Scale.FormatAuto));
      if (Scale.Format != null)
      {
        m_xDoc.WriteElementString("format", Scale.Format);
      }

      m_xDoc.WriteElementString("magnitude-multiplier", XmlConvert.ToString(Scale.Mag));
      m_xDoc.WriteElementString("magnitude-multiplier-auto", XmlConvert.ToString(Scale.MagAuto));
      m_xDoc.WriteElementString("min-grace", XmlConvert.ToString(Scale.MinGrace));
      m_xDoc.WriteElementString("max-grace", XmlConvert.ToString(Scale.MaxGrace));
      m_xDoc.WriteElementString("tic-alignment", Scale.Align.ToString());
      m_xDoc.WriteElementString("tic-label-alignment", Scale.AlignH.ToString());
      m_xDoc.WriteElementString("label-gap", XmlConvert.ToString(Scale.LabelGap));
      m_xDoc.WriteElementString("labels-inside", XmlConvert.ToString(Scale.IsLabelsInside));
      m_xDoc.WriteElementString("skip-first-label", XmlConvert.ToString(Scale.IsSkipFirstLabel));
      m_xDoc.WriteElementString("skip-last-label", XmlConvert.ToString(Scale.IsSkipLastLabel));
      m_xDoc.WriteElementString("skip-cross-label", XmlConvert.ToString(Scale.IsSkipCrossLabel));
      m_xDoc.WriteElementString("is-reverse", XmlConvert.ToString(Scale.IsReverse));
      m_xDoc.WriteElementString("use-ten-power", XmlConvert.ToString(Scale.IsUseTenPower));
      m_xDoc.WriteElementString("prevent-label-overlap", XmlConvert.ToString(Scale.IsPreventLabelOverlap));
      m_xDoc.WriteElementString("scale-visible", XmlConvert.ToString(Scale.IsVisible));

      Write("scale-font", Scale.FontSpec);

      if (Scale.TextLabels != null)
      {
        Write("text-labels", Scale.TextLabels);
      }

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
      Write("font", Label.FontSpec);
    }

    private void Write(string strNodeName, FontSpec Font)
    {
      m_xDoc.WriteStartElement(strNodeName);
      Write("color", Font.FontColor);
      m_xDoc.WriteElementString("family", Font.Family);
      m_xDoc.WriteElementString("bold", XmlConvert.ToString(Font.IsBold));
      m_xDoc.WriteElementString("italic", XmlConvert.ToString(Font.IsItalic));
      m_xDoc.WriteElementString("underline", XmlConvert.ToString(Font.IsUnderline));
      m_xDoc.WriteElementString("angle", XmlConvert.ToString(Font.Angle));
      m_xDoc.WriteElementString("string-alignment", Font.StringAlignment.ToString());
      m_xDoc.WriteElementString("size", XmlConvert.ToString(Font.Size));
      Write("border", Font.Border);
      Write("fill", Font.Fill);

      m_xDoc.WriteElementString("anti-alias", XmlConvert.ToString(Font.IsAntiAlias));
      m_xDoc.WriteElementString("drop-shadow", XmlConvert.ToString(Font.IsDropShadow));
      Write("drop-shadow-color", Font.DropShadowColor);
      m_xDoc.WriteElementString("drop-shadow-angle", XmlConvert.ToString(Font.DropShadowAngle));
      m_xDoc.WriteElementString("drop-shadow-offset", XmlConvert.ToString(Font.DropShadowOffset));

      m_xDoc.WriteEndElement();
    }

    private void Write(string strNodeName, Border border)
    {
      m_xDoc.WriteStartElement(strNodeName);
      m_xDoc.WriteElementString("inflate-factor", XmlConvert.ToString(border.InflateFactor));
      Write((LineBase)border);
      m_xDoc.WriteEndElement();
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
