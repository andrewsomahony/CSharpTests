using System;
namespace TestHelloWorld {
	namespace StringParsers {
		class Int32StringParser : StringParser<Int32>, IPrimitiveTypeStringParser {
			public override void ParseString(string inputString) {
				try {
					_value = Int32.Parse(inputString);
				} catch (FormatException) {
					throw new StringParserCannotParseStringException(inputString);
				}
			}
		}

		class Int64StringParser : StringParser<Int64>, IPrimitiveTypeStringParser {
			public override void ParseString(string inputString) {
				try {
					_value = Int64.Parse(inputString);
				} catch (FormatException) {
					throw new StringParserCannotParseStringException(inputString);
				}
			}
		}

		class StringStringParser : StringParser<string>, IPrimitiveTypeStringParser {
			public override void ParseString(string inputString) {
				_value = inputString;
			}
		}

		class CharStringParser : StringParser<char>, IPrimitiveTypeStringParser {
			public override void ParseString(string inputString) {
				_value = inputString[0];
			}
		}		
	}
}
