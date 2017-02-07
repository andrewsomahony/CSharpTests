using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace UserInput {
		public class YesNoUserInput : SpecificValueUserInput<char> {
			private string _question;

			public YesNoUserInput(string question) 
				: base("YesOrNo", "Yes or no user input", new char[] { 'Y', 'N' }) {
				_question = question;
			}

			public string question {
				get {
					return _question;
				}
			}

			public bool yes {
				get {
					return 'Y' == values[0];
				}
			}
		}
	}
}
