using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace ZedGraph
{
  /// <summary>
  /// Orientation of the cursor line.
  /// </summary>
  public enum CursorOrientation
  {
    /// <summary>
    /// Cursor is horizontal and parallel the the x axis. 
    /// </summary>
    Horizontal,

    /// <summary>
    /// Cursor is vertical and parallel to the y-axis. 
    /// </summary>
    Vertical,
  }

  /// <summary>
  /// Base object for cursor. 
  /// </summary>
  public class CursorObj 
  {
    /// <summary>
    /// protected field that maintains the attributes of the line using an
    /// instance of the <see cref="LineBase" /> class.
    /// </summary>
    protected LineBase _line;

    /// <summary>
    /// A tag object for the user. This can be used to store additional information 
    /// associated with the <see cref="CursorObj"/>. ZedGraph does not use this value
    /// for any purposes. 
    /// </summary>
    /// <remarks>
    /// If you are going to Serialize ZedGraph data then any
    /// type you store in <see cref="Tag"/> must be a serializable
    /// type (otherwise you'll get an exception)
    /// </remarks>
    public object Tag { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dPosition">Position of the cursor</param>
    /// <param name="o">Orientation of the cursor.</param>
    public CursorObj(double dPosition, CursorOrientation o)
    {
      Location = dPosition;
      Orientation = o;
      _line = new LineBase(Color.LightSkyBlue);
      _line.Style = DashStyle.Dash;
    }

    #region Properties

    /// <summary>
    /// A <see cref="LineBase" /> class that contains the attributes for drawing this
    /// <see cref="LineObj" />.
    /// </summary>
    public LineBase Line
    {
      get { return _line; }
      set { _line = value; }
    }

    /// <summary>
    /// Get/set the location of the cursor. 
    /// </summary>
    public double Location { get; set; }

    /// <summary>
    /// Get/set the orientation of the cursor. 
    /// </summary>
    public CursorOrientation Orientation { get; set; }

    /// <summary>
    /// Stores the coordinate system to be used for defining the
    /// object position.  
    /// </summary>
    public CoordType CoordinateUnit { get; set; } = CoordType.AxisXYScale;


    #endregion

    /// <summary>
    /// Render this object
    /// </summary>
    /// <param name="g">A graphic device object to render into</param>
    /// <param name="pane">The parent or owner if this object</param>
    /// <param name="scaleFactor">The scaling factor to be used for rendering
    /// objects. This is calculated and passed down by the parent <see cref="GraphPane"/></param>
    public void Draw(Graphics g, GraphPane pane, float scaleFactor)
    {

      CalculatePosition(pane, out PointF ptStart, out PointF ptEnd);

      using (Pen p = _line.GetPen(pane, scaleFactor))
      {
        RectangleF rcChart = pane.Chart.Rect;
        ptStart.X = Clamp(ptStart.X, rcChart.Left, rcChart.Right);
        ptStart.Y = Clamp(ptStart.Y, rcChart.Top, rcChart.Bottom);
        ptEnd.X = Clamp(ptEnd.X, rcChart.Left, rcChart.Right);
        ptEnd.Y = Clamp(ptEnd.Y, rcChart.Top, rcChart.Bottom);

        g.DrawLine(p, ptStart, ptEnd);
      }
    }

    private float Clamp(float x, float left, float right)
    {
      float fMin = Math.Min(left, right);
      if (x < fMin)
        return fMin;

      float fMax = Math.Max(left, right);
      if (x > fMax)
        return fMax;

      return x;
    }

    /// <summary>
    /// Determine if the specified screen point lies inside the bounding box of this
    /// <see cref="CursorObj"/>.
    /// </summary>
    /// <remarks>The bounding box is calculated assuming a distance
    /// of <see cref="GraphPane.Default.NearestTol"/> pixels around the arrow segment.
    /// </remarks>
    /// <param name="pt">The screen point, in pixels</param>
    /// <param name="pane">
    /// A reference to the <see cref="PaneBase"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <returns>true if the point lies in the bounding box, false otherwise</returns>
    public bool PointInBox(PointF pt, PaneBase pane, float scaleFactor)
    {
      PointF ptStart;
      PointF ptEnd;

      CalculatePosition(pane, out ptStart, out ptEnd);

      if (Orientation == CursorOrientation.Horizontal)
      {
        return (ptStart.Y - GraphPane.Default.NearestTol) < pt.Y
          && (ptStart.Y + GraphPane.Default.NearestTol) > pt.Y;
      }
      else
      {
        return (ptStart.X - GraphPane.Default.NearestTol) < pt.X
          && (ptStart.X + GraphPane.Default.NearestTol) > pt.X;
      }
    }

    private void CalculatePosition(PaneBase pane, out PointF ptStart, out PointF ptEnd)
    {
      if (Orientation == CursorOrientation.Horizontal)
      {
        ptStart = pane.TransformCoord(double.MinValue, Location, CoordinateUnit);
        ptEnd = pane.TransformCoord(double.MaxValue, Location, CoordinateUnit);
      }
      else
      {
        ptStart = pane.TransformCoord(Location, double.MinValue, CoordinateUnit);
        ptEnd = pane.TransformCoord(Location, double.MaxValue, CoordinateUnit);
      }
    }
  }
}
