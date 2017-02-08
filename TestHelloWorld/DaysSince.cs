using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

namespace TestHelloWorld
{
	class DaysInYear { 
		public int this[int year] {
			get {
				if (0 == (year % 4)) {
					if (0 != (year % 100)) {
						return 366;
					}
				}
				return 365;
			}
		}

		public bool isLeapYear(int year) {
			return 366 == this[year];
		}
	}

	class DaysInMonth {
		public static int daysInMonth(int month, bool isLeapYear) {
			if (2 == month) {
				if (true == isLeapYear) {
					return 29;
				}
				else {
					return 28;
				}
			}
			else {
				if (1 == month ||
					3 == month ||
					5 == month ||
					7 == month ||
					8 == month ||
					10 == month ||
					12 == month) {
					return 31;
				} else {
					return 30;
				}
			}
		}
	}

	public abstract class AfterCurrentException : Exception {
		public AfterCurrentException(string message) : base(message) {
		}
	}

	public class YearAfterCurrentYearException : AfterCurrentException { 
		public YearAfterCurrentYearException() : base("Year is after the current year!") {
		}
	}

	public class MonthAfterCurrentMonthException : AfterCurrentException { 
		public MonthAfterCurrentMonthException() : base("Month is after the current month!") {
		}
	}

	public class InvalidDayException : Exception {
		public InvalidDayException(int day) : base("Invalid day! " + day) {
			
		}
	}

	public class InvalidDateStringException : Exception {
		public InvalidDateStringException(string dateString) : base("Invalid date string! " + dateString) { 
		}
	}

	public class DaysSince
	{
		private int _daysSince;
		private int _day;
		private int _month;
		private int _year;

		public DaysSince(string ds) {
			Regex r = new Regex(@"(\d{1,2})\/(\d{1,2})\/(\d{4})");

			MatchCollection matchCollection = r.Matches(ds);

			try {
				int day = 0;
				int month = 0;
				int year = 0;

				GroupCollection group = matchCollection[0].Groups;

				day = Int32.Parse(group[1].Value);
				month = Int32.Parse(group[2].Value);
				year = Int32.Parse(group[3].Value);

				setDate(day, month, year);
			} catch (Exception) {
				throw new InvalidDateStringException(ds);
			}
		}

		public DaysSince(int day, int month, int year) {
			setDate(day, month, year);
		}

		private void setDate(int day, int month, int year) {
			_day = day;
			_month = month;
			_year = year;
			calculateDaysSince();
		}

		private void calculateDaysSince() {
			var currentDate = DateTime.Now;

			int currentYear = currentDate.Year;
			int currentMonth = currentDate.Month;
			int currentDay = currentDate.Day;

			if (currentYear < _year) {
				throw new YearAfterCurrentYearException();
			} else if (currentYear == _year) {
				if (currentMonth < _month) { 
					throw new MonthAfterCurrentMonthException();
				}
			}

			_daysSince = 0;
			DaysInYear diy = new DaysInYear();

			if (_day > DaysInMonth.daysInMonth(_month, diy.isLeapYear(_year))) {
				throw new InvalidDayException(_day);
			}

			if (currentYear != _year) {
				for (int i = _year + 1; i < currentYear; i++) {
					_daysSince += diy[i];
				}

				for (int i = _month + 1; i <= 12; i++) {
					_daysSince += DaysInMonth.daysInMonth(i, diy.isLeapYear(_year));
				}

				for (int i = 1; i < currentMonth; i++) {
					_daysSince += DaysInMonth.daysInMonth(i, diy.isLeapYear(currentYear));
				}

				_daysSince += DaysInMonth.daysInMonth(_month, diy.isLeapYear(_year)) - _day;
				_daysSince += currentDay;
			} else {
				if (currentMonth != _month) {
					for (int i = _month + 1; i < currentMonth; i++) {
						_daysSince += DaysInMonth.daysInMonth(i, diy.isLeapYear(_year));
					}

					_daysSince += DaysInMonth.daysInMonth(_month, diy.isLeapYear(_year)) - _day;
					_daysSince += currentDay;
				} else {
					_daysSince = currentDay - _day;
				}
			}
		}

		public int daysSince { 
			get {
				return _daysSince;
			}
		}

		public string dateString { 
			get {
				return _day + "/" + _month + "/" + _year;
			}
		}
	}
}
