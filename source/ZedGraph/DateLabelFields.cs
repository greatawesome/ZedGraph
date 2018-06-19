using System;
using System.Text;

namespace ZedGraph
{
  /// <summary>
  /// Elements of time/date that should be included in a date label. Calculated by
  /// <see cref="DateScale.CalcDateStepSize"/>
  /// </summary>
  [Flags]
  public enum DateLabelFields
  {
    None = 0,
    Year = 1,
    Month = 2,
    Day = 4,
    Hour = 8,
    Minute = 16,
    Second = 32,
    Milliseconds = 64,
  }

  public static class AutoDateLabelField
  {
    public static DateLabelFields SelectFields(double dRange, DateLabelFields Previous = DateLabelFields.None)
    {
      DateLabelFields Result = Previous;
      if (dRange > Scale.Default.RangeYearYear)
      {
        Result |= DateLabelFields.Year;
      }
      else if (dRange > Scale.Default.RangeYearMonth)
      {
        Result |= DateLabelFields.Year | DateLabelFields.Month;
      }
      else if (dRange > Scale.Default.RangeMonthMonth)
      {
        Result |= DateLabelFields.Year | DateLabelFields.Month;
      }
      else if (dRange > Scale.Default.RangeDayDay)
      {
        Result |= DateLabelFields.Day | DateLabelFields.Month;
      }
      else if (dRange > Scale.Default.RangeDayHour)
      {
        Result |= DateLabelFields.Day | DateLabelFields.Month | DateLabelFields.Hour | DateLabelFields.Minute;
      }
      else if (dRange > Scale.Default.RangeHourHour)
      {
        Result |= DateLabelFields.Hour | DateLabelFields.Minute;
      }
      else if (dRange > Scale.Default.RangeHourMinute)
      {
        Result |= DateLabelFields.Hour | DateLabelFields.Minute;
      }
      else if (dRange > Scale.Default.RangeMinuteMinute)
      {
        Result |= DateLabelFields.Hour | DateLabelFields.Minute;
      }
      else if (dRange > Scale.Default.RangeMinuteSecond)
      {
        Result |= DateLabelFields.Minute | DateLabelFields.Second;
      }
      else if (dRange > Scale.Default.RangeSecondSecond)
      {
        Result |= DateLabelFields.Minute | DateLabelFields.Second;
      }
      else
      {
        Result |= DateLabelFields.Second | DateLabelFields.Milliseconds;
      }

      return Result;
    }

    public static string CreateFormatString(DateLabelFields Value)
    {
      string[] astrFormatElements = { "yyyy", "MM", "dd", "HH", "mm", "ss", "fff" };
      string[] astrSeparators = { "-", "-", " ", ":", ":", "." };

      int nStartIndex;
      if (HasFlag(Value, DateLabelFields.Year))
      {
        nStartIndex = 0;
      }
      else if (HasFlag(Value, DateLabelFields.Month))
      {
        nStartIndex = 1;
      }
      else if (HasFlag(Value, DateLabelFields.Day))
      {
        nStartIndex = 2;
      }
      else if (HasFlag(Value, DateLabelFields.Hour))
      {
        nStartIndex = 3;
      }
      else if (HasFlag(Value, DateLabelFields.Minute))
      {
        nStartIndex = 4;
      }
      else if (HasFlag(Value, DateLabelFields.Second))
      {
        nStartIndex = 5;
      }
      else if (HasFlag(Value, DateLabelFields.Milliseconds))
      {
        nStartIndex = 6;
      }
      else
      {
        throw new ArgumentException($"The value {Value} is not supported");
      }

      int nEndIndex;
      if (HasFlag(Value, DateLabelFields.Milliseconds))
      {
        nEndIndex = 6;
      }
      else if (HasFlag(Value, DateLabelFields.Second))
      {
        nEndIndex = 5;
      }
      else if (HasFlag(Value, DateLabelFields.Minute))
      {
        nEndIndex = 4;
      }
      else if (HasFlag(Value, DateLabelFields.Hour))
      {
        nEndIndex = 3;
      }
      else if (HasFlag(Value, DateLabelFields.Day))
      {
        nEndIndex = 2;
      }
      else if (HasFlag(Value, DateLabelFields.Month))
      {
        nEndIndex = 1;
      }
      else if (HasFlag(Value, DateLabelFields.Year))
      {
        nEndIndex = 0;
      }
      else
      {
        throw new ArgumentException($"The value {Value} is not supported");
      }


      var sbFormatString = new StringBuilder();
      for (int iFormatElement = nStartIndex; iFormatElement <= nEndIndex; ++iFormatElement)
      {
        sbFormatString.Append(astrFormatElements[iFormatElement]);
        if (iFormatElement < astrFormatElements.Length && iFormatElement != nEndIndex)
        {
          sbFormatString.Append(astrSeparators[iFormatElement]);
        }
      }

      return sbFormatString.ToString();
    }

    private static bool HasFlag(DateLabelFields Value, DateLabelFields Test)
    {
      uint uValue = (uint)Value;
      uint uTest = (uint)Test;
      return (uValue & uTest) == uTest;
    }
  }
}
