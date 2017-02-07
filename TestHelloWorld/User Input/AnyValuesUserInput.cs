using System;
namespace TestHelloWorld {
	namespace UserInput {
		// Abstract because we want to encourage instantiation for the specific input.

		public abstract class AnyValuesUserInput<T> : GenericArrayUserInput<T> {
			public AnyValuesUserInput(string name, string description, 
			                          uint maxNumValues, uint minNumValues)
				: base(name, description, maxNumValues, minNumValues) {
			}

			public AnyValuesUserInput(string name, string description)
				: this(name, description, 1, 0) {

			}
		}

		public abstract class AnyValueUserInput<T> : AnyValuesUserInput<T> {
			public AnyValueUserInput(string name, string description)
				: base(name, description, 1, 1) {

			}

			public T value {
				get {
					return values[0];
				}
			}
		}
	}
}
