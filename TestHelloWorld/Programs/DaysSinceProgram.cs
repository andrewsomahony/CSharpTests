using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;

	public class DaysSinceProgramException : Exception {
		public DaysSinceProgramException(string message) : base(message) {
			
		}
	}

	public class DaysSinceProgram : Program {
		private readonly AnyIntValueUserInput _dayInput;
		private readonly AnyIntValueUserInput _monthInput;
		private readonly AnyIntValueUserInput _yearInput;
		private DaysSince _daysSince;

		public DaysSinceProgram() {
			_dayInput = new AnyIntValueUserInput("day");
			_monthInput = new AnyIntValueUserInput("month");
			_yearInput = new AnyIntValueUserInput("year");
		}

		async public override Task RunAsync() {
			await RequiresUserInputAsync(_dayInput);
			await RequiresUserInputAsync(_monthInput);
			await RequiresUserInputAsync(_yearInput);

			try {
				_daysSince = new DaysSince(_dayInput.value, _monthInput.value, _yearInput.value);
			} catch (Exception e) {
				throw new DaysSinceProgramException(e.Message);
			}

			// We can have this line to just give a RunAsync method
			// an await, to suppress a warning.
			//await base.RunAsync();
		}

		public string dateString {
			get {
				return _daysSince.dateString;
			}
		}

		public int daysSinceDate {
			get {
				return _daysSince.daysSince;
			}
		}
	}
}
