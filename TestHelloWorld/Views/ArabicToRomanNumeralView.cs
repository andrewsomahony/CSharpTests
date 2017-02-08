using System;

namespace TestHelloWorld {
	using Parsers;

	public class ArabicToRomanNumeralView : ConsoleView {
		public ArabicToRomanNumeralView() {
		}

		public override string title {
			get {
				return "Arabic to Roman Numeral";
			}
		}

		public override void Run() {
			Show();

			DefaultStringParser stringParser = new DefaultStringParser();
			Numeral n = new Numeral();

			while (true) {
				try {
					if (!ReadAndParse("Enter the Arabic numeral", stringParser)) {
						break;
					}

					Console.WriteLine(n.arabicToRomanNumeral(stringParser.value));
				} catch (InvalidArabicNumeralException) {
					Console.WriteLine("String is not an Arabic numeral!");
				}
			}
		}
	}
}
