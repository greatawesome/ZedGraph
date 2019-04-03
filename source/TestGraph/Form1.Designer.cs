namespace TestGraph
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.zgGraph = new ZedGraph.ZedGraphControl();
      this.SuspendLayout();
      // 
      // zgGraph
      // 
      this.zgGraph.Dock = System.Windows.Forms.DockStyle.Fill;
      this.zgGraph.Location = new System.Drawing.Point(0, 0);
      this.zgGraph.Name = "zgGraph";
      this.zgGraph.ScrollGrace = 0D;
      this.zgGraph.ScrollMaxX = 0D;
      this.zgGraph.ScrollMaxY = 0D;
      this.zgGraph.ScrollMaxY2 = 0D;
      this.zgGraph.ScrollMinX = 0D;
      this.zgGraph.ScrollMinY = 0D;
      this.zgGraph.ScrollMinY2 = 0D;
      this.zgGraph.SelectAppendModifierKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.None)));
      this.zgGraph.Size = new System.Drawing.Size(1249, 668);
      this.zgGraph.TabIndex = 0;
      this.zgGraph.UseExtendedPrintDialog = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1249, 668);
      this.Controls.Add(this.zgGraph);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private ZedGraph.ZedGraphControl zgGraph;
  }
}

