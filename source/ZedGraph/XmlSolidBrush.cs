using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;

namespace ZedGraph
{
  /// <summary>
  /// A helper class to facilitate <see cref="XmlSerializer"/> serialization
  /// of brushes. 
  /// </summary>
  public class XmlSolidBrush : XmlBrush
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public XmlSolidBrush()
    {
    }

    /// <summary>
    /// Construct for a specific brush. 
    /// </summary>
    /// <param name="b"></param>
    public XmlSolidBrush(SolidBrush b) : base(b)
    {
      IsNullBrush = b == null;
      if (b != null)
      {
        Color = b.Color;
      }
    }

    protected override Brush GetNativeBrush()
    {
      return IsNullBrush ? null : new SolidBrush(Color);
    }

    public bool IsNullBrush { get; set; }

    [XmlElement(Type = typeof(XmlColor))]
    public Color Color { get; set; }

  }
}
