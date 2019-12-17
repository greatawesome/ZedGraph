using System;
using System.Drawing;
using System.Xml.Serialization;

namespace ZedGraph
{
  /// <summary>
  /// A helper class to facilitate serialization of colors. 
  /// Based on: https://stackoverflow.com/questions/3280362/most-elegant-xml-serialization-of-color-structure/4322461
  /// Decorate any public <see cref="Color"/> properties using 
  /// <code>[XmlElement(Type=typeof(XmlColor))]</code>
  /// </summary>
  public class XmlColor
  {
    private Color m_Color = System.Drawing.Color.Black;

    public XmlColor() { }
    public XmlColor(Color c) { m_Color = c; }


    public Color ToColor()
    {
      return m_Color;
    }

    public void FromColor(Color c)
    {
      m_Color = c;
    }

    public static implicit operator Color(XmlColor x)
    {
      return x.ToColor();
    }

    public static implicit operator XmlColor(Color c)
    {
      return new XmlColor(c);
    }

    [XmlAttribute]
    public string Color
    {
      get { return ColorTranslator.ToHtml(m_Color); }
      set
      {
        try
        {
          if (Alpha == 0xFF) // preserve named color value if possible
            m_Color = ColorTranslator.FromHtml(value);
          else
            m_Color = System.Drawing.Color.FromArgb(Alpha, ColorTranslator.FromHtml(value));
        }
        catch (Exception)
        {
          m_Color = System.Drawing.Color.Black;
        }
      }
    }

    [XmlAttribute]
    public byte Alpha
    {
      get { return m_Color.A; }
      set
      {
        if (value != m_Color.A) // avoid hammering named color if no alpha change
          m_Color = System.Drawing.Color.FromArgb(value, m_Color);
      }
    }
  }
}
