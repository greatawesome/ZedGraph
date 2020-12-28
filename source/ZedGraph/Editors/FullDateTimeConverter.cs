using System;
using System.ComponentModel;
using System.Globalization;

namespace ZedGraph.Editors
{
  /// <summary>
  /// Custom <see cref="DateTime"/> type converter that always displays
  /// seconds and millisconds components of time. 
  /// </summary>
  class FullDateTimeConverter : DateTimeConverter
  {
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == typeof(string) && value is DateTime dt)
      {
        if (dt == DateTime.MinValue)
        {
          return string.Empty;
        }

        if (culture == null)
        {
          culture = CultureInfo.CurrentCulture;
        }

        DateTimeFormatInfo formatInfo = null;
        formatInfo = (DateTimeFormatInfo)culture.GetFormat(typeof(DateTimeFormatInfo));

        if (culture == CultureInfo.InvariantCulture)
        {
          return dt.ToString(culture);
        }


        string format = formatInfo.ShortDatePattern + " HH::mm:ss.fff";
        return dt.ToString(format, culture);
      }

      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
