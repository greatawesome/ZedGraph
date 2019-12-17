using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace ZedGraph
{
  /// <summary>
  /// A helper class to facilitate <see cref="XmlSerializer"/> serialization
  /// of <see cref="LinearGradientBrush"/>. 
  /// </summary>
  public class XmlLinearGradientBrush : XmlBrush
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public XmlLinearGradientBrush()
    {
      IsNullBrush = true; 
    }

    /// <summary>
    /// Construct for a specific brush. 
    /// </summary>
    /// <param name="b"></param>
    public XmlLinearGradientBrush(LinearGradientBrush b) : base(b)
    {
      IsNullBrush = b == null;
      if (!IsNullBrush)
      {
        Blend = b.Blend;
        GammaCorrection = b.GammaCorrection;
        StartColor = b.LinearColors[0];
        EndColor = b.LinearColors[1];
        Rectangle = b.Rectangle;
        Transform = b.Transform;
        WrapMode = b.WrapMode;

        if (Blend == null)
        {
          InterpolationColors = b.InterpolationColors;
        }
      }
    }

    protected override Brush GetNativeBrush()
    {
      if (IsNullBrush)
      {
        return null;
      }

      LinearGradientBrush NewBrush = new LinearGradientBrush(Rectangle, StartColor, EndColor, 0F)
      {
        GammaCorrection = GammaCorrection,
        Transform = Transform,
        WrapMode = WrapMode
      };

      if (Blend != null)
      {
        NewBrush.Blend = Blend;
      }

      if (InterpolationColors != null)
      {
        NewBrush.InterpolationColors = InterpolationColors;
      }

      return NewBrush;
    }

    public bool IsNullBrush { get; set; }

    public Blend Blend { get; set; }

    public bool GammaCorrection { get; set; }

    public ColorBlend InterpolationColors { get; set; }

    [XmlElement(Type = typeof(XmlColor))]
    public Color StartColor { get; set; }

    [XmlElement(Type = typeof(XmlColor))]
    public Color EndColor { get; set; }

    public RectangleF Rectangle { get; set; }

    public Matrix Transform { get; set; }

    public WrapMode WrapMode { get; set; }
  }
}
