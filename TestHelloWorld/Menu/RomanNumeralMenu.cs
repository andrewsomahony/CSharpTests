using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	public class RomanNumeralMenu : StackMenu {
		public RomanNumeralMenu() : base(new Dictionary<int, View>() {
			{1, new ArabicToRomanNumeralView()},
			{2, new RomanToArabicNumeralView()},
			{3, new ParsedRomanToArabicNumeralView()}
		}) {
		}

		public override string title {
			get {
				return "Roman Numerals";
			}
		}
	}
}
