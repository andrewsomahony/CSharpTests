using System;
namespace TestHelloWorld {
	namespace Alphabets {
		public abstract class Alphabet {
			public abstract string alphabet {
				get;
			}

			public bool charIsValid(char c) {
				return -1 != alphabet.IndexOf(c.ToString().ToUpper()[0]);
			}
		}
	}
}
