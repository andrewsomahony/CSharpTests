using System;
using System.Collections.Generic;

namespace TestHelloWorld {

	class InvalidArabicNumeralException : Exception {
		public InvalidArabicNumeralException(string numeral) : base("Invalid Arabic Numeral! " + numeral) { 
		}
	}

	class InvalidRomanNumeralException : Exception {
		public InvalidRomanNumeralException(string numeral) : base("Invalid Roman Numeral! " + numeral) { 
		}
	}

	class RomanNumeralMapper { 
		private static Dictionary<int, string> _romanNumerals = new Dictionary<int, string>()
		{
			{1, "I"},
			{5, "V"},
			{10, "X"},
			{50, "L"},
			{100, "C"},
			{500, "D"},
			{1000, "M"},
		};

		private static string _romanNumeralBar = "|";

		public static bool charIsRomanNumeral(char c) {
			return 0 != RomanNumeralMapper.romanNumeralToNumber(c.ToString());
		}

		public static string numberToRomanNumeral(int number) {
			return _romanNumerals[number];
		}

		public static bool charIsRomanNumeralBar(char c) {
			return 0 == c.ToString().CompareTo(_romanNumeralBar);
		}

		public static int romanNumeralToNumber(string numeral) {
			foreach (var pair in _romanNumerals) {
				if (0 == numeral.CompareTo(pair.Value)) {
					return pair.Key;
				}
			}
			return 0;
		}

		public static string romanNumeralBar {
			get {
				return _romanNumeralBar;
			}
		}
	}

	class RomanNumeralReader {
		private string _numeralString;
		private int _currentStringPosition;

		public RomanNumeralReader(string ns) {
			numeralString = ns;
		}

		// getNextNumber
		// Starts reading at _currentStringPosition, stops
		// when a new numeral is reached; reads *1000 bars as well.
		// Returns 0 for no number, else the calculated number.

		public int getNextNumber() {
			int returnNumber = 0;
			bool hasNumber = false;
			for (; _currentStringPosition < _numeralString.Length; _currentStringPosition++) {
				if (true == hasNumber) {
					if (true == RomanNumeralMapper.charIsRomanNumeralBar(_numeralString[_currentStringPosition])) {
						returnNumber *= 1000;
					} else {
						break;
					}
				} else {
					returnNumber = RomanNumeralMapper.romanNumeralToNumber(_numeralString[_currentStringPosition].ToString());
					hasNumber = true;
				}
			}

			return returnNumber;
		}

		public string numeralString {
			get {
				return _numeralString;
			}
			set {
				_numeralString = value;
				_currentStringPosition = 0;
			}
		}
	}

	public class Numeral {
		public Numeral() {
		}

		private bool isRomanNumeral(string s) {
			foreach (char c in s.ToUpper()) {
				bool charIsValid = false;
				charIsValid = RomanNumeralMapper.charIsRomanNumeral(c);

				if (!charIsValid) {
					if (true == RomanNumeralMapper.charIsRomanNumeralBar(c)) {
						charIsValid = true;
					} else {
						return false;
					}
				}
			}
			return true;
		}

		private bool isArabicNumeral(string s) {
			try {
				Int32.Parse(s);
				return true;
			} catch (Exception) {
				return false;
			}
		}

		// Number guaranteed to be square of 10, or multiple of 5

		private string mapNumberToRomanNumeral(int number) {
			string mappedNumberString = "";

			int numBars = 0;
			while (number > 1000) {
				number /= 1000;
				numBars++;
			}

			mappedNumberString += RomanNumeralMapper.numberToRomanNumeral(number);
			for (int i = 0; i < numBars; i++) {
				mappedNumberString += RomanNumeralMapper.romanNumeralBar;
			}

			return mappedNumberString;
		}

		public string arabicToRomanNumeral(string s) {
			if (false == isArabicNumeral(s)) {
				throw new InvalidArabicNumeralException(s);
			}

			int scale = 10;
			int number = Int32.Parse(s);

			while (true) {
				if (scale > number) {
					break;
				}
				scale *= 10;
			}

			string romanNumeral = "";

			while (1 != scale) {
				int scaleNumber = (number / (scale / 10)) * (scale / 10);

				if (scaleNumber < scale / 2) {
					if (scaleNumber == (scale / 2) - (scale / 10)) {
						// Subtract
						romanNumeral += mapNumberToRomanNumeral(scale / 10);
						romanNumeral += mapNumberToRomanNumeral(scale / 2);
					} else {
						for (int i = 0; i < scaleNumber; i += scale / 10) {
							romanNumeral += mapNumberToRomanNumeral(scale / 10);
						}
					}
				} else if (scaleNumber > scale / 2) {
					if (scaleNumber == scale - (scale / 10)) {
						// Subtract
						romanNumeral += mapNumberToRomanNumeral(scale / 10);
						romanNumeral += mapNumberToRomanNumeral(scale);
					} else {
						romanNumeral += mapNumberToRomanNumeral(scale / 2);
						for (int i = scale / 2; i < scaleNumber; i += scale / 10) {
							romanNumeral += mapNumberToRomanNumeral(scale / 10);
						}
					}
				} else {
					romanNumeral += mapNumberToRomanNumeral(scale / 2);
				}
				scale /= 10;
				number -= scaleNumber;
			}

			return romanNumeral;
		}

		private List<int> parseRomanNumeral(string s) {
			List<int> returnArray = new List<int>();

			RomanNumeralReader reader = new RomanNumeralReader(s);

			int currentNumber = 0;
			int nextNumber = 0;
			while (true) {
				if (0 == currentNumber) {
					currentNumber = reader.getNextNumber();
					if (0 == currentNumber) {
						break;
					}
				}
				nextNumber = reader.getNextNumber();

				if (currentNumber < nextNumber) {
					returnArray.Add(-currentNumber);
					returnArray.Add(nextNumber);
					currentNumber = 0;
				} else {
					returnArray.Add(currentNumber);
					currentNumber = nextNumber;
				}
			}

			return returnArray;
		}

		public int romanToArabicNumeral(string s) {
			s = s.ToUpper();

			if (false == isRomanNumeral(s)) {
				throw new InvalidRomanNumeralException(s);
			}

			int returnValue = 0;

			List<int> parsedNumbers = parseRomanNumeral(s);

			foreach (int i in parsedNumbers) {
				returnValue += i;
			}

			return returnValue;
		}

		public string getParsedRomanNumeral(string s) {
			s = s.ToUpper();

			if (false == isRomanNumeral(s)) { 
				throw new InvalidRomanNumeralException(s);
			}

			string returnValue = "";

			List<int> parsedNumbers = parseRomanNumeral(s);

			foreach (int i in parsedNumbers) {
				if (i > 0) {
					if (returnValue.Length > 0) {
						returnValue += "+";
					}
				} else {
					returnValue += "-";
				}

				int absNumber = Math.Abs(i);
				if (absNumber > 1000) {
					returnValue += "(";

					string multiply = "";
					while (absNumber > 1000) {
						multiply += "*1000";
						absNumber /= 1000;
					}

					returnValue += absNumber.ToString();
					returnValue += multiply;

					returnValue += ")";
				} else {
					returnValue += absNumber.ToString();
				}
			}

			return returnValue;
		}
	}

}
