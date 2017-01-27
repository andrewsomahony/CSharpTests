using System;
using System.Collections.Generic;
using TestHelloWorld.Parsers;

namespace TestHelloWorld {
	public abstract class Menu : View {
		protected string prompt = "Enter selection";

		protected abstract void ExecuteOption(int index);
		protected abstract void DoExit();

		public override void Run() {
			Show();

			IntStringParser intParser = new IntStringParser();
			while (true) {
				try {
					if (ReadAndParse(prompt, intParser)) {
						ExecuteOption(intParser.value);
					} else {
						DoExit();
					}
					break;
				} catch (Exception e) {
					Console.WriteLine("Invalid entry.  Try again " + e.Message);
				}
			}
		}
	}
}
