using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace UserInput {
		public class TooManyUserInputValuesException : Exception {
			public TooManyUserInputValuesException(int numValues, uint maxNumValues)
				: base("Too many values for user input! (" + numValues + " > " + maxNumValues + ")") {

			}
		}

		public class TooFewUserInputValuesException : Exception {
			public TooFewUserInputValuesException(int numValues, uint minNumValues)
				: base("Too many values for user input! (" + numValues + " < " + minNumValues + ")") {

			}
		}

		abstract public partial class UserInputBase {
			protected class UserInputStorage<T> {
				List<T> _values;
				uint _minNumValues;
				uint _maxNumValues; // If 0, then the number of values is unlimited.

				public UserInputStorage() : this(1, 0) {
					
				}

				public UserInputStorage(uint maxNumValues) : this(1, maxNumValues) {
					
				}

				public UserInputStorage(uint minNumValues, uint maxNumValues) {
					// We don't really need to allocate to capacity,
					// as we check if the count goes over the max by using
					// our stored variable.

					_values = new List<T>();
					_minNumValues = minNumValues;
					_maxNumValues = maxNumValues;
				}

				public void SetValues(IList<T> values) {
					if (true == hasCapacity &&
						values.Count > _maxNumValues) {
						throw new TooManyUserInputValuesException(values.Count, _maxNumValues);
					} else if (values.Count < _minNumValues) {
						throw new TooFewUserInputValuesException(values.Count, _minNumValues);
					} else {
						_values.Clear();
						_values.AddRange(values);
					}
				}

				public bool allowsMultipleValues {
					get {
						return 0 == _maxNumValues || _maxNumValues > 1;
					}
				}

				private bool hasCapacity {
					get {
						return _maxNumValues > 0;
					}
				}

				public IReadOnlyList<T> values {
					get {
						return _values;
					}
				}
			}
		}
	}
}
