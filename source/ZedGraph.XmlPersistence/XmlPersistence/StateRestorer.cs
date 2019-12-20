using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;

namespace ZedGraph.XmlPersistence
{
  public class StateRestorer
  {
    private XmlNode m_xnState;

    public StateRestorer(XmlNode xnState)
    {
      m_xnState = xnState ?? throw new ArgumentNullException(nameof(xnState));
    }

    public void RestoreTitle(GraphPane gp)
    {
      XmlNode xnTitle = m_xnState.SelectSingleNode("title");
      if (xnTitle != null)
      {
        Restore(xnTitle, gp.Title);
      }
    }

    public void RestoreXAxis(GraphPane gp)
    {
      XmlNode xnAxis = m_xnState.SelectSingleNode("x-axis");
      if (xnAxis != null)
      {
        Restore(xnAxis, gp.XAxis);
      }
    }

    public void RestoreYAxis(GraphPane gp)
    {
      XmlNode xnAxis = m_xnState.SelectSingleNode("y-axis");
      if (xnAxis != null)
      {
        Restore(xnAxis, gp.YAxis);
      }
    }

    public void RestoreY2Axis(GraphPane gp)
    {
      XmlNode xnAxis = m_xnState.SelectSingleNode("y2-axis");
      if (xnAxis != null)
      {
        Restore(xnAxis, gp.Y2Axis);
      }
    }

    public void RestoreBackgroundFill(GraphPane gp)
    {
      XmlNode xnFill = m_xnState.SelectSingleNode("background-fill");
      if (Read(xnFill, out Fill NewFill))
      {
        gp.Chart.Fill = NewFill;
      }
    }

    private void Restore(XmlNode xnAxis, Axis Axis)
    {
      try
      {
        Axis.Scale.Min = ValueOrDefault(xnAxis, "minimum", Axis.Scale.Min);
        Axis.Scale.Max = ValueOrDefault(xnAxis, "maximum", Axis.Scale.Max);
        Axis.Scale.MajorStep = ValueOrDefault(xnAxis, "major-step", Axis.Scale.MajorStep);
        Axis.Scale.MinorStep = ValueOrDefault(xnAxis, "minor-step", Axis.Scale.MinorStep);
        Axis.Scale.MajorUnit = ValueOrDefault(xnAxis, "major-unit", Axis.Scale.MajorUnit);
        Axis.Scale.MinorUnit = ValueOrDefault(xnAxis, "minor-unit", Axis.Scale.MinorUnit);
        Axis.Scale.MinAuto = ValueOrDefault(xnAxis, "minimum-auto", Axis.Scale.MinAuto);
        Axis.Scale.MaxAuto = ValueOrDefault(xnAxis, "maximum-auto", Axis.Scale.MaxAuto);
        Axis.Scale.MajorStepAuto = ValueOrDefault(xnAxis, "major-step-auto", Axis.Scale.MajorStepAuto);
        Axis.Scale.MinorStepAuto = ValueOrDefault(xnAxis, "minor-step-auto", Axis.Scale.MinorStepAuto);

        string strValue = xnAxis.SelectSingleNode("visible/text()")?.Value;
        if (!string.IsNullOrEmpty(strValue))
        {
          Axis.IsVisible = XmlConvert.ToBoolean(strValue);
        }

        Restore(xnAxis, Axis.Title);

      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OverflowException)
      {
        Trace.WriteLine(ex.Message);
      }
    }

    private void Restore(XmlNode xnLabel, GapLabel Label)
    {
      Label.Text = ValueOrDefault(xnLabel, "text", Label.Text);
      Label.IsVisible = ValueOrDefault(xnLabel, "visible", Label.IsVisible);
      Label.Gap = ValueOrDefault(xnLabel, "gap", Label.Gap);
      if (Read(xnLabel.SelectSingleNode("font"), Label.FontSpec, out FontSpec NewFont))
      {
        Label.FontSpec = NewFont;
      }
    }

    private void Restore(XmlNode xnBorder, Border b)
    {
      if (xnBorder != null)
      {
        b.InflateFactor = XmlConvert.ToSingle(xnBorder.SelectSingleNode("inflate-factor").InnerText);
        Restore(xnBorder, (LineBase)b);
      }
    }

    private void Restore(XmlNode xnBorder, LineBase lb)
    {
      lb.Color = ValueOrDefault(xnBorder, "color", lb.Color);
      lb.Style = ValueOrDefault(xnBorder, "style", lb.Style);
      lb.DashOn = ValueOrDefault(xnBorder, "dash-on", lb.DashOn);
      lb.DashOff = ValueOrDefault(xnBorder, "dash-off", lb.DashOff);
      lb.Width = ValueOrDefault(xnBorder, "width", lb.Width);
      lb.IsVisible = ValueOrDefault(xnBorder, "visible", lb.IsVisible);
      lb.IsAntiAlias = ValueOrDefault(xnBorder, "anti-alias", lb.IsAntiAlias);

      if (Read(xnBorder.SelectSingleNode("gradient-fill"), out Fill NewFill))
      {
        lb.GradientFill = NewFill;
      }
    }

    private bool Read(XmlNode xnFont, FontSpec Template, out FontSpec NewFont)
    {
      if (xnFont == null)
      {
        NewFont = null;
        return false;
      }

      NewFont = Template.Clone();
      NewFont.FontColor = ValueOrDefault(xnFont, "color", NewFont.FontColor);
      NewFont.Family = ValueOrDefault(xnFont, "family", NewFont.Family);
      NewFont.IsBold = ValueOrDefault(xnFont, "bold", NewFont.IsBold);
      NewFont.IsItalic = ValueOrDefault(xnFont, "italic", NewFont.IsItalic);
      NewFont.IsUnderline = ValueOrDefault(xnFont, "underline", NewFont.IsUnderline);
      NewFont.IsAntiAlias = ValueOrDefault(xnFont, "anti-alias", NewFont.IsAntiAlias);
      NewFont.Angle = ValueOrDefault(xnFont, "angle", NewFont.Angle);
      NewFont.StringAlignment = ValueOrDefault(xnFont, "string-alignment", NewFont.StringAlignment);
      Restore(xnFont.SelectSingleNode("border"), NewFont.Border);

      if (Read(xnFont.SelectSingleNode("fill"), out Fill NewFill))
      {
        NewFont.Fill = NewFill;
      }
      NewFont.Size = ValueOrDefault(xnFont, "size", NewFont.Size);

      NewFont.IsDropShadow = ValueOrDefault(xnFont, "drop-shadow", NewFont.IsDropShadow);
      NewFont.DropShadowColor = ValueOrDefault(xnFont, "drop-shadow-color", NewFont.DropShadowColor);
      NewFont.DropShadowAngle = ValueOrDefault(xnFont, "drop-shadow-angle", NewFont.DropShadowAngle);
      NewFont.DropShadowOffset = ValueOrDefault(xnFont, "drop-shadow-offset", NewFont.DropShadowOffset);


      return true;
    }

    private bool Read(XmlNode xnFill, out Fill fill)
    {
      if (xnFill != null)
      {
        if (Read(xnFill.SelectSingleNode("color-1"), out Color clr1)
          && Read(xnFill.SelectSingleNode("color-2"), out Color clr2)
          && Read(xnFill.SelectSingleNode("angle"), out float fAngle))
        {
          fill = new Fill(clr1, clr2, fAngle);
          return true;
        }
        else if (Read(xnFill.SelectSingleNode("color"), out Color clr)
          && Read(xnFill.SelectSingleNode("secondary-color"), out Color clrSecondary)
          && Read(xnFill.SelectSingleNode("angle"), out float fFillAngle)
          && Read(xnFill.SelectSingleNode("fill-type"), out FillType FillType)
          && Read(xnFill.SelectSingleNode("align-h"), out AlignH HorzAlign)
          && Read(xnFill.SelectSingleNode("align-v"), out AlignV VerticalAlign)
          && Read(xnFill.SelectSingleNode("range-min"), out double dRangeMin)
          && Read(xnFill.SelectSingleNode("range-max"), out double dRangeMax)
          && Read(xnFill.SelectSingleNode("range-default"), out double dRangeDefault)
          && Read(xnFill.SelectSingleNode("scaled"), out bool bIsScaled))
        {
          fill = new Fill(clr, clrSecondary, fFillAngle);
          fill.Color = clr;
          fill.SecondaryValueGradientColor = clrSecondary;
          fill.IsScaled = bIsScaled;
          fill.AlignH = HorzAlign;
          fill.AlignV = VerticalAlign;
          fill.RangeMin = dRangeMin;
          fill.RangeMax = dRangeMax;
          fill.RangeDefault = dRangeDefault;

          if (Read(xnFill.SelectSingleNode("brush"), out Brush LoadedBrush))
          {
            fill.Brush = LoadedBrush;
            return true; 
          }
        }
      }
      fill = null;
      return false;
    }

    private bool Read(XmlNode xnValue, out Color clrResult)
    {
      try
      {
        if (xnValue != null)
        {
          var xaName = xnValue.Attributes["name"];
          var xaARGB = xnValue.Attributes["argb"];
          string strColorValue = xaARGB?.Value ?? xnValue.InnerText;
          int nARGB = XmlConvert.ToInt32(strColorValue);
          clrResult = Color.FromArgb(nARGB);

          string strNamedColor = xaName?.Value;
          if (!string.IsNullOrEmpty(strNamedColor))
          {
            Color clrNamed = Color.FromName(strNamedColor);
            clrResult = clrResult.A != 255 ? Color.FromArgb(clrResult.A, clrNamed) : clrNamed;
          }
          return true;
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }

      clrResult = Color.Empty;
      return false;
    }

    private bool Read(XmlNode xnValue, out Brush brResult)
    {
      try
      {
        if (xnValue != null)
        {
          string strBrushType = xnValue.Attributes["type"]?.Value;
          switch (strBrushType)
          {
            case "solid-brush":
              brResult = ReadSolidBrush(xnValue);
              return true;

            case "linear-gradient-brush":
              brResult = ReadLinearGradientBrush(xnValue);
              return true;

            case "texture-brush":
              brResult= ReadTextureBrush(xnValue);
              return true; 

            default:
              break;
          }
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
      brResult = null;
      return false;
    }

    private Brush ReadSolidBrush(XmlNode xnValue)
    {
      Read(xnValue.SelectSingleNode("color"), out Color clr);
      return new SolidBrush(clr);
    }

    private Brush ReadLinearGradientBrush(XmlNode xnBrush)
    {
      Blend blend = ReadBlend(xnBrush.SelectSingleNode("blend"));
      ColorBlend ColorBlend = ReadColorBlend(xnBrush.SelectSingleNode("interpolation-colors"));
      Color[] LinearColors = ReadColorArray(xnBrush.SelectSingleNode("linear-colors"));
      RectangleF rcBlend = ReadRectangle(xnBrush.SelectSingleNode("rectangle"));
      Matrix mxTransform = ReadMatrix(xnBrush.SelectSingleNode("transform"));
      WrapMode wrap = ValueOrDefault(xnBrush, "wrap-mode", WrapMode.Tile);
      bool bGamaCorrection = ValueOrDefault(xnBrush, "gamma-correction", true);

      var NewBrush = new LinearGradientBrush(rcBlend, LinearColors[0], LinearColors[1], 0f);
      NewBrush.GammaCorrection = bGamaCorrection;
      if (blend != null)
      {
        NewBrush.Blend = blend;
      }
      NewBrush.WrapMode = wrap;
      if (mxTransform != null)
      {
        NewBrush.Transform = mxTransform;
      }
      if (ColorBlend != null)
      {
        NewBrush.InterpolationColors = ColorBlend;
      }

      return NewBrush;
    }

    private Brush ReadTextureBrush(XmlNode xnBrush)
    {
      var Image = ReadImage(xnBrush.SelectSingleNode("image"));
      Matrix mxTransform = ReadMatrix(xnBrush.SelectSingleNode("transform"));
      WrapMode wrap = ValueOrDefault(xnBrush, "wrap-mode", WrapMode.Tile);

      var NewBrush = new TextureBrush(Image, wrap);
      if (mxTransform != null)
      {
        NewBrush.Transform = mxTransform;
      }
      return NewBrush;
    }

    private Image ReadImage(XmlNode xnImage)
    {
      if (xnImage == null)
      {
        return null;
      }

      byte[] abyImageContent = Convert.FromBase64String(xnImage.InnerText);
      using (var ms = new MemoryStream(abyImageContent))
      {
        return Image.FromStream(ms);
      }
    }

    private Matrix ReadMatrix(XmlNode xnMatrix)
    {
      if (xnMatrix == null)
      {
        return null;
      }

      float[] aValues = ReadFloatArray(xnMatrix, "m");
      if (aValues.Length != 6)
      {
        throw new ArgumentException("Node doesn't contain data for a matrix", nameof(xnMatrix));
      }

      return new Matrix(aValues[0], aValues[1], aValues[2], aValues[3], aValues[4], aValues[5]);
    }

    private RectangleF ReadRectangle(XmlNode xnRectangle)
    {
      if (xnRectangle == null)
        throw new ArgumentNullException(nameof(xnRectangle));

      float fX, fY, fWidth, fHeight;
      fX = XmlConvert.ToSingle(xnRectangle.Attributes["x"].InnerText);
      fY = XmlConvert.ToSingle(xnRectangle.Attributes["y"].InnerText);
      fWidth = XmlConvert.ToSingle(xnRectangle.Attributes["w"].InnerText);
      fHeight = XmlConvert.ToSingle(xnRectangle.Attributes["h"].InnerText);

      return new RectangleF(fX, fY, fWidth, fHeight);
    }

    private Color[] ReadColorArray(XmlNode xnColors)
    {
      if (xnColors == null)
      {
        return null;
      }

      var Colors = new List<Color>();
      foreach (XmlNode xnColor in xnColors.SelectNodes("color"))
      {
        if (Read(xnColor, out Color NewColor))
        {
          Colors.Add(NewColor);
        }
      }
      return Colors.ToArray();
    }

    private ColorBlend ReadColorBlend(XmlNode xnColorBlend)
    {
      if (xnColorBlend == null)
      {
        return null;
      }

      XmlNode xnColors = xnColorBlend.SelectSingleNode("colors");
      XmlNode xnPositions = xnColorBlend.SelectSingleNode("positions");

      if (xnColors == null)
      {
        throw new ArgumentException("Missing color values", nameof(xnColorBlend));
      }
      if (xnPositions == null)
      {
        throw new ArgumentException("Missing position values", nameof(xnColorBlend));
      }

      Color[] aColors = ReadColorArray(xnColors);
      float[] aPositions = ReadFloatArray(xnPositions, "p");

      if (aColors.Length != aPositions.Length)
      {
        throw new ArgumentException("Expecting the same number of colors and positions", nameof(xnColorBlend));
      }
      var Blend = new ColorBlend();
      Blend.Colors = aColors;
      Blend.Positions = aPositions;

      return Blend;
    }

    private Blend ReadBlend(XmlNode xnBlend)
    {
      if (xnBlend == null)
      {
        return null;
      }

      float [] aFactors = ReadFloatArray(xnBlend.SelectSingleNode("factors"), "f");
      float [] aPositions = ReadFloatArray(xnBlend.SelectSingleNode("positions"), "p");
      if (aFactors.Length != aPositions.Length)
      {
        throw new ArgumentException("Expecting the same number of factors and positions", nameof(xnBlend));
      }

      var Blend = new Blend();
      Blend.Factors = aFactors;
      Blend.Positions = aPositions;

      return Blend;
    }

    private float[] ReadFloatArray(XmlNode xnCollectionNode, string strChildNodeName)
    {
      var Result = new List<float>();
      foreach (XmlNode xnChild in xnCollectionNode.SelectNodes(strChildNodeName))
      {
        string strValue = xnChild.InnerText;
        float fValue = XmlConvert.ToSingle(strValue);
        Result.Add(fValue);
      }
      return Result.ToArray();
    }

    private bool Read(XmlNode xnNode, out float fValue)
    {
      try
      {
        var strValue = xnNode?.InnerText;
        if (!string.IsNullOrEmpty(strValue))
        {
          fValue = XmlConvert.ToSingle(strValue);
          return true;
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
      fValue = float.NaN;
      return false;
    }

    private bool Read(XmlNode xnNode, out double dValue)
    {
      try
      {
        var strValue = xnNode?.InnerText;
        if (!string.IsNullOrEmpty(strValue))
        {
          dValue = XmlConvert.ToDouble(strValue);
          return true;
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
      dValue = double.NaN;
      return false;
    }

    private bool Read(XmlNode xnNode, out bool bValue)
    {
      try
      {
        var strValue = xnNode?.InnerText;
        if (!string.IsNullOrEmpty(strValue))
        {
          bValue = XmlConvert.ToBoolean(strValue);
          return true;
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
      bValue = default(bool);
      return false;
    }

    private bool Read<T>(XmlNode xnNode, out T Value) where T : Enum
    {
      try
      {
        var strValue = xnNode?.InnerText;
        if (!string.IsNullOrEmpty(strValue))
        {
          Value = (T)Enum.Parse(typeof(T), strValue);
          return true;
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
      Value = default(T);
      return false;
    }



    private string ValueOrDefault(XmlNode xnNode, string strNodeName, string strDefault)
    {
      return xnNode.SelectSingleNode(strNodeName)?.InnerText ?? strDefault;
    }

    private float ValueOrDefault(XmlNode xnNode, string strNodeName, float fDefault)
    {
      return Read(xnNode.SelectSingleNode(strNodeName), out float fValue) ? fValue : fDefault;
    }

    private double ValueOrDefault(XmlNode xnNode, string strNodeName, double dDefault)
    {
      try
      {
        var strValue = xnNode?.SelectSingleNode(strNodeName)?.InnerText;
        return string.IsNullOrEmpty(strValue) ? dDefault : XmlConvert.ToDouble(strValue);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
        return dDefault;
      }
    }

    private bool ValueOrDefault(XmlNode xnNode, string strNodeName, bool bDefault)
    {
      return Read(xnNode.SelectSingleNode(strNodeName), out bool bValue) ? bValue : bDefault;
    }

    private T ValueOrDefault<T>(XmlNode xnNode, string strNodeName, T Default) where T : Enum
    {
      return Read(xnNode.SelectSingleNode(strNodeName), out T Value) ? Value : Default;
    }


    private Color ValueOrDefault(XmlNode xnNode, string strNodeName, Color Default)
    {
      return Read(xnNode.SelectSingleNode(strNodeName), out Color clrValue) ? clrValue : Default;
    }



  }
}
