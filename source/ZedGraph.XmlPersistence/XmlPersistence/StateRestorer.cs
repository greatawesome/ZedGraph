using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Restore(xnTitle, gp.XAxis);
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
      if (xnFill != null)
      {
        var NewFill = Read(xnFill, gp.Chart.Fill);
        if (NewFill != null)
        {
          gp.Chart.Fill = NewFill;
        }
      }
    }

    private Fill Read(XmlNode xnFill, Fill fill)
    {
      XmlNode xnValue;
      Color clr1 = Color.White;
      Color clr2 = Color.FromArgb(255, 255, 210);
      float fAngle = -45f;

      if (xnFill != null)
      {
        xnValue = xnFill.SelectSingleNode("color-1/text()");
        if (xnValue != null)
        {
          clr1 = Color.FromArgb(XmlConvert.ToInt32(xnValue.Value));
        }

        xnValue = xnFill.SelectSingleNode("color-2/text()");
        if (xnValue != null)
        {
          clr2 = Color.FromArgb(XmlConvert.ToInt32(xnValue.Value));
        }

        xnValue = xnFill.SelectSingleNode("angle/text()");
        if (xnValue != null)
        {
          fAngle = (float)XmlConvert.ToDouble(xnValue.Value);
        }
      }
      return new Fill(clr1, clr2, fAngle);
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
      }
    }

    private bool ValueOrDefault(XmlNode xnAxis, string strNodeName, bool bDefault)
    {
      try
      {
        return XmlConvert.ToBoolean(xnAxis.SelectSingleNode(strNodeName).InnerText);
      }
      catch
      {
        return bDefault;
      }
    }

    private DateUnit ValueOrDefault(XmlNode xnAxis, string strNodeName, DateUnit Default)
    {
      try
      {
        return (DateUnit)Enum.Parse(typeof(DateUnit), xnAxis.SelectSingleNode(strNodeName).InnerText); 
      }
      catch
      {
        return Default;
      }
    }

    private double ValueOrDefault(XmlNode xnAxis, string strNodeName, double dDefault)
    {
      try
      {
        return XmlConvert.ToDouble(xnAxis.SelectSingleNode(strNodeName).InnerText);
      }
      catch
      {
        return dDefault;
      }
    }

    private void Restore(XmlNode xnLabel, GapLabel Label)
    {
      XmlNode xnValue;
      FontSpec NewFont;

      xnValue = xnLabel.SelectSingleNode("text/text()");
      if (xnValue != null)
        Label.Text = xnValue.Value;

      xnValue = xnLabel.SelectSingleNode("visible/text()");
      if (xnValue != null)
        Label.IsVisible = XmlConvert.ToBoolean(xnValue.Value);

      xnValue = xnLabel.SelectSingleNode("font");
      if (xnValue != null)
      {
        NewFont = Read(xnValue, Label.FontSpec);
        if (NewFont != null)
        {
          Label.FontSpec = NewFont;
        }
      }
    }

    private FontSpec Read(XmlNode xnFont, FontSpec Template)
    {
      XmlNode xnValue;
      FontSpec NewFont;

      NewFont = Template.Clone();
      xnValue = xnFont.SelectSingleNode("color/text()");
      if (xnValue != null)
        NewFont.FontColor = Color.FromArgb(XmlConvert.ToInt32(xnValue.Value));

      xnValue = xnFont.SelectSingleNode("family/text()");
      if (xnValue != null)
        NewFont.Family = xnValue.Value;

      xnValue = xnFont.SelectSingleNode("bold/text()");
      if (xnValue != null)
        NewFont.IsBold = XmlConvert.ToBoolean(xnValue.Value);

      xnValue = xnFont.SelectSingleNode("italic/text()");
      if (xnValue != null)
        NewFont.IsItalic = XmlConvert.ToBoolean(xnValue.Value);

      xnValue = xnFont.SelectSingleNode("size/text()");
      if (xnValue != null)
        NewFont.Size = (float)XmlConvert.ToDouble(xnValue.Value);

      return NewFont;
    }
  }
}
