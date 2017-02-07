using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;

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

		async public override Task InitAsync() {
			await base.InitAsync();

			await RequiresUserInputAsync(_dayInput);
			await RequiresUserInputAsync(_monthInput);
			await RequiresUserInputAsync(_yearInput);
		}

		async public override Task RunAsync() {
			_daysSince = new DaysSince(_dayInput.value, _monthInput.value, _yearInput.value);

			await base.RunAsync();
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
