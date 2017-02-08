using System;
namespace TestHelloWorld {
	using Parsers;

	public class FibonacciView : ConsoleView {
		public FibonacciView() {
		}

		public override string title {
			get {
				return "Fibonacci Numbers";
			}
		}

		public override void Run() {
			Show();

			IntStringParser intParser = new IntStringParser();

			while (true) {
				try {
					if (!ReadAndParse("Enter the number of steps", intParser)) {
						break;
					}

					Fibonacci f = new Fibonacci(intParser.value);
					f.print();
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}
