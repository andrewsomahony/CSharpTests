using System;
namespace TestHelloWorld {
	namespace Parsers {
		public class StringParserInvalidFormatException : Exception {
			string _value;
			public StringParserInvalidFormatException(string value) : base("Invalid format! " + value) {
				_value = value;
			}

			public string value {
				get {
					return _value;
				}
			}
		}

		public abstract class StringParser {
			public abstract void Parse(string s);
			public abstract bool CanParse(string s);
		}

		public class DefaultStringParser : StringParser {
			string _value;

			public override void Parse(string s) {
				_value = s;
			}

			public override bool CanParse(string s) {
				return true;
			}

			public string value {
				get {
					return _value;
				}
			}
		}

		public class CertainCharacterParser : DefaultStringParser {
			private bool _caseSensitive;
			private char[] _characters;
			protected char _value;

			public CertainCharacterParser() : this(false, new char[] {}) {
				
			}

			public CertainCharacterParser(bool caseSensitive, char[] characters) {
				_characters = characters;
				_caseSensitive = caseSensitive;
			}

			public override void Parse(string s) {
				bool found = false;

				if (null != s &&
					s.Length > 0) {
					foreach (char c in _characters) {
						if ((false == _caseSensitive &&
							 Char.ToUpper(c) == Char.ToUpper(s[0])) ||
							c == s[0]) {
							_value = c;
							found = true;
							break;
						}
					}
				}

				if (false == found) {
					throw new StringParserInvalidFormatException(s);
				}
			}

			public bool ValueIsCharacter(char c) {
				return Char.ToUpper(_value) == Char.ToUpper(c);
			}
		}

		public class ConfirmCharacterParser : CertainCharacterParser {
			public ConfirmCharacterParser(bool caseSensitive) : base(caseSensitive, new char[] { 'y', 'n' }) {
				
			}

			public bool isYes {
				get {
					return ValueIsCharacter('y');
				}
			}
		}

		public class IntStringParser : StringParser {
			int _value;

			private int DoParse(string s) {
				return Int32.Parse(s);
			}

			public override void Parse(string s) {
				try {
					_value = DoParse(s);
				} catch (FormatException) {
					throw new StringParserInvalidFormatException(s);
				}
			}

			public override bool CanParse(string s) {
				try {
					DoParse(s);
					return true;
				} catch (Exception) {
					return false;
				}
			}

			public int value {
				get {
					return _value;
				}
			}
		}
	}
}
