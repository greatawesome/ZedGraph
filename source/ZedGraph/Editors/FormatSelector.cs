using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ZedGraph.Editors
{
  class FormatSelector : UITypeEditor
  {
    private IWindowsFormsEditorService m_EditorService;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      m_EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
      if (m_EditorService != null)
      {
        var lb = new ListBox();
        lb.SelectionMode = SelectionMode.One;
        lb.SelectedValueChanged += lb_SelectedValueChanged;
        bool bFound = false;
        string strValue = value?.ToString() ?? string.Empty;
        foreach (string strFormat in GetAvailableFormats())
        {
          int nIndex = lb.Items.Add(strFormat);
          if (string.Equals(strFormat, strValue))
          {
            lb.SelectedIndex = nIndex;
            bFound = true; 
          }
        }
        if (!bFound && !string.IsNullOrEmpty(strValue))
        {
          lb.SelectedIndex = lb.Items.Add(strValue);
        }

        m_EditorService.DropDownControl(lb);
        if (lb.SelectedItem == null)
        {
          // No selection. Return original value. 
          return value;
        }

        return lb.SelectedItem;
      }

      return base.EditValue(context, provider, value);
    }

    private void lb_SelectedValueChanged(object sender, EventArgs e)
    {
      m_EditorService.CloseDropDown();
    }

    private IEnumerable<string> GetAvailableFormats()
    {
      yield return "HH:mm";
      yield return "HH:mm:ss";
      yield return "HH:mm:ss.fff";
      yield return "hh:mm tt";
      yield return "hh:mm:ss tt";
      yield return "yyyy-MM-dd HH:mm";
      yield return "yyyy-MM-dd hh:mm tt";
      yield return "0.0";
      yield return "0.00";
      yield return "0.000";
      yield return "#,##0";
      yield return "E";
      yield return "g";
    }
  }
}
