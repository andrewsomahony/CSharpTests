using System;
namespace TestHelloWorld {
	using Parsers;

	public class ParsedRomanToArabicNumeralView : ConsoleView {
		public ParsedRomanToArabicNumeralView() {
		}

		public override string title {
			get {
				return "Parsed Roman to Arabic Numeral";
			}
		}

		public override void Run() {
			Show();

			DefaultStringParser stringParser = new DefaultStringParser();
			Numeral n = new Numeral();

			while (true) {
				try {
					if (!ReadAndParse("Enter the Roman numeral", stringParser)) {
						break;
					}

					Console.WriteLine(n.getParsedRomanNumeral(stringParser.value) +
					                  " = " + n.romanToArabicNumeral(stringParser.value));
				} catch (InvalidRomanNumeralException) {
					Console.WriteLine("String is not a Roman numeral!");
				}
			}
		}
	}
}
