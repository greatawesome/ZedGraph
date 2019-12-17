using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZedGraph
{
  public class XmlScale
  {
    public XmlScale()
    {
    }

    public XmlScale(Scale s)
    {
      Min = s.Min;
    }

    public virtual double Min { get; set; }
  }
}
