using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZedGraph
{
  /// <summary>
  /// Collection of cursor objects.
  /// </summary>
  public class CursorObjList : List<CursorObj>
  {
    /// <summary>
    /// Draw all visible cursors.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="pane"></param>
    /// <param name="scaleFactor"></param>
    public void Draw(Graphics g, GraphPane pane, float scaleFactor)
    {
      foreach (CursorObj c in this)
      {
        if (c.Line != null && c.Line.IsVisible)
        {
          c.Draw(g, pane, scaleFactor);
        }
      }
    }
  }
}
