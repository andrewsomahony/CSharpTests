using System;
namespace TestHelloWorld {
	namespace UserInput {
		public class AnyIntValuesUserInput : AnyValuesUserInput<int> {
			public AnyIntValuesUserInput(string name, string description,
			                             uint minNumValues, uint maxNumValues) :
			base(name, description, minNumValues, maxNumValues) {
			}

			public AnyIntValuesUserInput() : this("AnyIntValues", "", 1, 0) {
				
			}
		}

		public class AnyIntValueUserInput : AnyValueUserInput<int> {
			public AnyIntValueUserInput(string name, string description = "") :
			base(name, description) {
				
			}

			public AnyIntValueUserInput() : this("AnyIntValue") {
				
			}
		}

		public class SpecificIntValuesUserInput : SpecificValuesUserInput<int> {
			public SpecificIntValuesUserInput(string name, string description,
			                                  int[] values, uint minNumValues, uint maxNumValues) :
			base(name, description,
				 values, minNumValues, maxNumValues) {
				
			}
		}

		public class SpecificIntValueUserInput : SpecificValueUserInput<int> {
			public SpecificIntValueUserInput(string name, string description, int[] values) :
			base(name, description, values) {
				
			}
		}
	}
}
