using System;
using System.Linq;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace StringParsers {
		public class InvalidStringFormatException : Exception {
			public InvalidStringFormatException(string s) : base("Invalid string format! " + s) {
				
			}
		}

		public class StringParserFactory {
			private List<Type> _stringParserTypes;

			private static StringParserFactory singleton = null;

			public StringParserFactory() {
				_stringParserTypes = new List<Type>();

				// Initialize all the primitive type parsers we have.
				// They can be anywhere, but they have to implement IPrimitiveTypeStringParser,
				// and derive from the StringParser<T> class, where T is a CLI primitive type.

				foreach (Type mytype in 
				         System.Reflection.Assembly.
				         GetExecutingAssembly().GetTypes().
				         Where(mytype => mytype.GetInterfaces().Contains(typeof(IPrimitiveTypeStringParser)))) {
					StringParser stringParser = (Activator.CreateInstance(mytype) as StringParser);
					if (false == stringParser.type.IsPrimitive &&
					    typeof(string) != stringParser.type) {
						throw new Exception("Invalid primitive string parser!");
					}
					AddStringParserType(mytype);
				}
			}

			public void AddStringParserType(Type t) {
				_stringParserTypes.Add(t);
			}

			public void Parse<T>(string inputString, out T outputValue) {
				foreach (Type t in _stringParserTypes) {
					try {
						// We try to cast to the type parser in the array.
						// If it fails, we know that this specific parser (represented by Type t)
						// cannot handle the generic type T, so we suppress the exception and move on.

						StringParser<T> parser
						= (StringParser<T>)(object)Activator.CreateInstance(t);

						parser.ParseString(inputString);
						outputValue = parser.value;

						return;
					} catch (InvalidCastException) {

					} catch (StringParserCannotParseStringException) {
						// We've found the parser that works for type T,
						// but the string is in a format that it cannot parse.
						break;
					}
				}

				// If we cannot find the parser that works for the type we want (T),
				// then the string is in an invalid format.

				throw new InvalidStringFormatException(inputString);
			}

			public static StringParserFactory Instance() {
				if (null == singleton) {
					singleton = new StringParserFactory();
				}
				return singleton;
			}
		}
	}
}
