using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	public abstract class StackMenu : Menu {
		// For a view that puts other views onto the stack.
		protected readonly Dictionary<int, View> _options;

		protected StackMenu(Dictionary<int, View> options) {
			_options = options;
		}

		protected override void Show() {
			base.Show();

			foreach (KeyValuePair<int, View> kv in _options) {
				Console.WriteLine(kv.Key + ". " + kv.Value.title);
			}
		}

		protected override void DoExit() {
			// Not needed
		}

		protected override void ExecuteOption(int index) {
			_parentViewStack.PushView(_options[index]);
		}
	}
}
