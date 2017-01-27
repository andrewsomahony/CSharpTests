using System;
using TestHelloWorld.Parsers;

namespace TestHelloWorld {
	public class DaysSinceView : View {
		public DaysSinceView() {
		}

		public override string title {
			get {
				return "Days Since Date";
			}
		}

		public override void Run() {
			Show();

			IntStringParser day = new IntStringParser();
			IntStringParser month = new IntStringParser();
			IntStringParser year = new IntStringParser();

			while (true) {
				try {
					if (!ReadAndParse("Enter the day", day)) {
						break;
					}
					if (!ReadAndParse("Enter the month", month)) {
						break;
					}
					if (!ReadAndParse("Enter the year", year)) {
						break;
					}

					DaysSince s = new DaysSince(day.value, month.value, year.value);
					Console.WriteLine(s.daysSince + " days since " + s.dateString);
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}
