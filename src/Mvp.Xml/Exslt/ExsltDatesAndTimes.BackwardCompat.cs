using System;

namespace Mvp.Xml.Exslt;

public partial class ExsltDatesAndTimes
{
    public string dateTime() => DateTime();

    public string dateTime(string s) => DateTime(s);

    public bool leapYear() => LeapYear();

    public bool leapYear(string d) => LeapYear(d);

    public double monthInYear() => MonthInYear();

    public double monthInYear(string d) => MonthInYear(d);

    public double weekInYear(DateTime d) => WeekInYear(d);

    public double weekInYear() => WeekInYear();

    public double weekInYear(string d) => WeekInYear(d);

    public double weekInMonth(DateTime d) => WeekInMonth(d);

    public double weekInMonth() => WeekInMonth();

    public double weekInMonth(string d) => WeekInMonth(d);

    public double dayInYear() => DayInYear();

    public double dayInYear(string d) => DayInYear(d);

    public double dayInWeek() => DayInWeek();

    public double dayInWeek(string d) => DayInWeek(d);

    public double dayInMonth() => DayInMonth();

    public double dayInMonth(string d) => DayInMonth(d);

    public double dayOfWeekInMonth(int day) => DayOfWeekInMonth(day);

    public double dayOfWeekInMonth() => DayOfWeekInMonth();

    public double dayOfWeekInMonth(string day) => DayOfWeekInMonth(day);

    public double hourInDay() => HourInDay();

    public double hourInDay(string d) => HourInDay(d);

    public double minuteInHour() => MinuteInHour();

    public double minuteInHour(string d) => MinuteInHour(d);

    public double secondInMinute() => SecondInMinute();

    public double secondInMinute(string d) => SecondInMinute(d);

    public string dayName(int dow) => DayName(dow);

    public string dayName() => DayName();

    public string dayName(string dow) => DayName(dow);

    public string dayAbbreviation(int dow) => DayAbbreviation(dow);

    public string dayAbbreviation() => DayAbbreviation();

    public string dayAbbreviation(string dow) => DayAbbreviation(dow);

    public string monthName(int month) => MonthName(month);

    public string monthName() => MonthName();

    public string monthName(string month) => MonthName(month);

    public string monthAbbreviation(int month) => MonthAbbreviation(month);

    public string monthAbbreviation() => MonthAbbreviation();

    public string monthAbbreviation(string d) => MonthAbbreviation(d);

    public string formatDate(string d, string format) => FormatDate(d, format);

    public string parseDate(string d, string format) => ParseDate(d, format);

    public string addDuration(string duration1, string duration2) => AddDuration(duration1, duration2);
}