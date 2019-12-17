using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace ZedGraph
{
  /// <summary>
  /// Base class for serializing brushes with <see cref="XmlSerializer"/> 
  /// </summary>
  [XmlInclude(typeof(XmlSolidBrush)), XmlInclude(typeof(XmlLinearGradientBrush))]
  public class XmlBrush
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public XmlBrush()
    {
    }

    protected XmlBrush(Brush b)
    {
    }

    protected virtual Brush GetNativeBrush() { return null; }

    /// <summary>
    /// Operator to convert an <see cref="XmlBrush"/> to a normal brush. 
    /// </summary>
    /// <param name="xb"></param>
    public static implicit operator Brush(XmlBrush xb)
    {
      return xb?.GetNativeBrush();
    }

    /// <summary>
    /// Operator to wrap a <see cref="Brush"/> in an <see cref="XmlBrush"/>
    /// </summary>
    /// <param name="b"></param>
    public static implicit operator XmlBrush(Brush b)
    {
      if (b == null)
      {
        return new XmlBrush(b);
      }

      if (b is SolidBrush sb)
      {
        return new XmlSolidBrush(sb);
      }

      if (b is LinearGradientBrush lgb)
      {
        return new XmlLinearGradientBrush(lgb);
      }

      throw new ArgumentOutOfRangeException(nameof(b), $"Unsupported brush type: {b.GetType()}");
    }
  }
}
