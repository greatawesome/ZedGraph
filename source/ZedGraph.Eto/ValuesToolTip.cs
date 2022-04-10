namespace ZedGraph
{
    using System;
    using System.Drawing;
    using Eto.Forms;

    public class ValuesToolTip : IValuesToolTip
    {
        #region Fields

        /// <summary>
        /// The last caption that was set.
        /// </summary>
        private string lastCaption;

        /// <summary>
        /// The last point a tool tip caption was set at.
        /// </summary>
        private Point lastPoint;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesToolTip"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="activeCallback">The active callback.</param>
        /// <param name="setToolTipCallback">The set tool tip callback.</param>
        /// <exception cref="System.ArgumentNullException">
        /// control
        /// or
        /// activeCallback
        /// or
        /// setToolTipCallback
        /// </exception>
        public ValuesToolTip(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            this.Control = control;
        }

        #endregion

        #region Properties and Indexers

        /// <summary>
        /// Gets the control that this tool tip instance handles.
        /// </summary>
        /// <value>
        /// The control that this tool tip instance handles.
        /// </value>
        public Control Control { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Disables the tool tip.
        /// </summary>
        public void Disable()
        {
            this.Control.ToolTip = "";
        }

        /// <summary>
        /// Enables the tool tip.
        /// </summary>
        public void Enable()
        {
            //Does nothing. In Eto, setting the tooltip will enable it. Since all calls to this method are preceded by Set calls, we should be fine.
            return;
        }

        /// <summary>
        /// Sets the specified caption.
        /// </summary>
        /// <param name="caption">The caption.</param>
        public void Set(string caption)
        {
            this.Set(caption, this.lastPoint);
        }

        /// <summary>
        /// Sets the caption for the tool tip at the specified point.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="point">The point.</param>
        public void Set(string caption, Point point)
        {
            if (point != this.lastPoint || caption != this.lastCaption)
            {
                this.Control.ToolTip = caption;
                this.lastPoint = point;
                this.lastCaption = caption;
            }
        }

        /// <summary>
        /// Creates a <see cref="ValuesToolTip"/> for the specified control,
        /// using the supplied tooltip to display values.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="toolTip">The tool tip.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">toolTip</exception>
        public static ValuesToolTip Create(Control control)
        {
            return new ValuesToolTip(control);
        }

        #endregion
    }
}
