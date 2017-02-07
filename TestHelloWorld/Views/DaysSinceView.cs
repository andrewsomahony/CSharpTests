using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	public class DaysSinceView : View {
		public DaysSinceView() {
			_program = new DaysSinceProgram();
		}

		private DaysSinceProgram daysSinceProgram {
			get {
				return (DaysSinceProgram)_program;
			}
		}

		public override string title {
			get {
				return "Days Since Date";
			}
		}

		async public Task RunAsync() {
			Show();

			while (isAlive) {
				// Keep running the program over and over again.
				// This means initializing it and such each time.
				await RunProgramAsync();

				Console.WriteLine(daysSinceProgram.daysSinceDate + " days since " + daysSinceProgram.dateString);
			}			
		}

		public override void Run() {
			RunAsync().Wait();
		}
	}
}
