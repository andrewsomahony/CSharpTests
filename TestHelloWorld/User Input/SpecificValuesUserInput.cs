using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace UserInput {
		public class ValueNotInSpecificValuesException<T> : Exception {
			public ValueNotInSpecificValuesException(T value) : base("Value not in specific values! " + value) {
				
			}
		}

		public abstract class SpecificValuesUserInput<T> : GenericArrayUserInput<T> {
			private List<T> _specificValues;

			public SpecificValuesUserInput(string name, string description, 
			                               T[] values, uint minNumValues, uint maxNumValues)
				: base(name, description, minNumValues, maxNumValues) {
				_specificValues = new List<T>(values);
			}

			public SpecificValuesUserInput(string name, string description, T[] values) 
				: this(name, description, values, 1, 0) {
			}

			public override void SetValue(T value) {
				if (false == _specificValues.Contains(value)) {
					throw new ValueNotInSpecificValuesException<T>(value);
				}
				base.SetValue(value);
			}

			public override void SetValues(IList<T> values) {
				foreach (T value in values) {
					if (false == _specificValues.Contains(value)) {
						throw new ValueNotInSpecificValuesException<T>(value);
					}
				}
				base.SetValues(values);
			}

			public IReadOnlyList<T> specificValues {
				get {
					return _specificValues;
				}
			}
		}

		public abstract class SpecificValueUserInput<T> : SpecificValuesUserInput<T> {
			public SpecificValueUserInput(string name, string description, T[] values) 
				: base(name, description, values, 1, 1) {
				
			}

			public T value {
				get {
					return values[0];
				}
			}
		}
	}
}
