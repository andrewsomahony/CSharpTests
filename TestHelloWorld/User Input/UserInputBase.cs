using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace UserInput {
		abstract public partial class UserInputBase {
			private string _name;
			private string _description;

			public UserInputBase(string name, string description) {
				_name = name;
				_description = description;
			}

			public string name {
				get {
					return _name;
				}
			}

			public string description {
				get {
					return _description;
				}
			}

			abstract public Type type {
				get;
			}
		}

		abstract public class GenericUserInput<T> : UserInputBase {
			private UserInputStorage<T> _inputStorage;

			protected GenericUserInput(string name, string description, UserInputStorage<T> inputStorage)
				: base(name, description) {
				_inputStorage = inputStorage;
			}

			public virtual void SetValues(IList<T> values) {
				_inputStorage.SetValues(values);
			}

			public virtual void SetValue(T value) {
				SetValues(new List<T>() { value });
			}

			public IReadOnlyList<T> values {
				get {
					return _inputStorage.values;
				}
			}

			public override Type type {
				get {
					return typeof(T);
				}
			}

			public bool isMultiple {
				get {
					return _inputStorage.allowsMultipleValues;
				}
			}
		}

		abstract public class GenericArrayUserInput<T> : GenericUserInput<T> {
			protected GenericArrayUserInput(string name, string description, 
			                                uint minNumValues, uint maxNumValues)
				: base(name, description, new UserInputStorage<T>(minNumValues, maxNumValues)) {
				
			}

			protected GenericArrayUserInput(string name, string description) :
			base(name, description, new UserInputStorage<T>()) {
				
			}

			public override string ToString() {
				string returnString = "[";

				foreach (T value in values) {
					returnString += value.ToString();
				}

				returnString += "]";

				return returnString;
			}
		}
	}
}
