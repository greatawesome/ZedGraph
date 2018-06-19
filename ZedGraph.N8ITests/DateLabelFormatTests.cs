using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZedGraph
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestSecondRange()
    {
      TestCorrectFields(new DateTime(2018, 6, 1, 12, 0, 0), new DateTime(2018, 6, 1, 12, 0, 30), DateLabelFields.Second | DateLabelFields.Milliseconds);
    }

    [TestMethod]
    public void TestMinuteRange()
    {
      TestCorrectFields(new DateTime(2018, 6, 1, 12, 0, 0), new DateTime(2018, 6, 1, 12, 1, 0), DateLabelFields.Minute | DateLabelFields.Second);
    }

    [TestMethod]
    public void TestHourRange()
    {
      TestCorrectFields(new DateTime(2018, 6, 1, 12, 0, 0), new DateTime(2018, 6, 1, 13, 1, 0), DateLabelFields.Minute | DateLabelFields.Hour);
    }

    [TestMethod]
    public void TestDayRange()
    {
      TestCorrectFields(new DateTime(2018, 6, 1, 12, 0, 0), new DateTime(2018, 6, 4, 13, 1, 0), DateLabelFields.Minute | DateLabelFields.Hour | DateLabelFields.Month | DateLabelFields.Day);
    }

    [TestMethod]
    public void TestMonthRange()
    {
      TestCorrectFields(new DateTime(2018, 6, 1, 12, 0, 0), new DateTime(2018, 7, 4, 13, 1, 0), DateLabelFields.Month | DateLabelFields.Day);
    }

    [TestMethod]
    public void TestYearRange()
    {
      TestCorrectFields(new DateTime(2018, 6, 1, 12, 0, 0), new DateTime(2028, 7, 4, 13, 1, 0), DateLabelFields.Year);
    }

    private void TestCorrectFields(DateTime dtStart, DateTime dtEnd, DateLabelFields ExpectedFields)
    {
      double dRange = Math.Abs(XDate.DateTimeToXLDate(dtStart) - XDate.DateTimeToXLDate(dtEnd));
      DateLabelFields FieldsToInclude = AutoDateLabelField.SelectFields(dRange);
      Assert.AreEqual(ExpectedFields, FieldsToInclude);
    }

    [TestMethod]
    public void TestMilliseconds()
    {
      TestCorrectFormat(DateLabelFields.Milliseconds, "fff");
    }

    [TestMethod]
    public void TestSeconds()
    {
      TestCorrectFormat(DateLabelFields.Second, "ss");
    }

    [TestMethod]
    public void TestMinutes()
    {
      TestCorrectFormat(DateLabelFields.Minute, "mm");
    }

    [TestMethod]
    public void TestHours()
    {
      TestCorrectFormat(DateLabelFields.Hour, "HH");
    }

    [TestMethod]
    public void TestDay()
    {
      TestCorrectFormat(DateLabelFields.Day, "dd");
    }

    [TestMethod]
    public void TestMonth()
    {
      TestCorrectFormat(DateLabelFields.Month, "MM");
    }

    [TestMethod]
    public void TestYear()
    {
      TestCorrectFormat(DateLabelFields.Year, "yyyy");
    }

    [TestMethod]
    public void TestMiddleRange()
    {
      TestCorrectFormat(DateLabelFields.Month | DateLabelFields.Minute, "MM-dd HH:mm");
    }

    [TestMethod]
    public void TestAllRange()
    {
      TestCorrectFormat(DateLabelFields.Year | DateLabelFields.Milliseconds, "yyyy-MM-dd HH:mm:ss.fff");
    }

    [TestMethod]
    public void TestEndRange()
    {
      TestCorrectFormat(DateLabelFields.Hour | DateLabelFields.Milliseconds, "HH:mm:ss.fff");
    }

    [TestMethod]
    public void TestStartRange()
    {
      TestCorrectFormat(DateLabelFields.Year | DateLabelFields.Day, "yyyy-MM-dd");
    }

    private void TestCorrectFormat(DateLabelFields Fields, string strExpectedFormat)
    {
      string strActualFormat = AutoDateLabelField.CreateFormatString(Fields);
      Assert.AreEqual(strExpectedFormat, strActualFormat);
    }
  }
}
