using System;
namespace TestHelloWorld {
	namespace StringParsers {
		class StringParserCannotParseStringException : Exception {
			public StringParserCannotParseStringException(string s) : base("Cannot parse string! " + s) {
				
			}
		}

		abstract class StringParser {
			public abstract void ParseString(string inputString);

			public abstract Type type {
				get;
			}
		}

		abstract class StringParser<T> : StringParser {
			protected T _value;

			public T value {
				get {
					return _value;
				}
			}

			public override Type type {
				get {
					return typeof(T);
				}
			}
		}
	}
}
