using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	public class DaysSinceView : AsyncConsoleView {
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

		async public override Task RunAsync() {
			while (isAlive) {
				Clear();
				Show();

				// Keep running the program over and over again.
				// This means initializing it and such each time.
				try {
					await RunProgramAsync();

					Console.WriteLine(daysSinceProgram.daysSinceDate + " days since " + daysSinceProgram.dateString);

					// The ViewStack and PushView are synchronous, meaning
					// this function will block until every view has exited,
					// so when we return here, we return to exactly where we left off.
					_parentViewStack.PushView(new FibonacciView());
				} catch (DaysSinceProgramException e) {
					Console.WriteLine(e.Message);
				}
			}			
		}
	}
}
