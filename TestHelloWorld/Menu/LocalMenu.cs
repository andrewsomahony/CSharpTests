using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	public delegate void ExecuteLocalOption();

	public class LocalMenuOption {
		private readonly string _name;
		private readonly ExecuteLocalOption _fn;

		public LocalMenuOption(string name, ExecuteLocalOption fn) {
			_name = name;
			_fn = fn;
		}

		public string name {
			get {
				return _name;
			}
		}

		public ExecuteLocalOption fn {
			get {
				return _fn;
			}
		}
	}

	public abstract class LocalMenu : Menu {
		// For a view that just wants options to be handled within itself
		// It can't be readonly as it uses delegates, and we can't use
		// instance methods as delegates within a base constructor,
		// so each subclass has to initialize this itself.
		protected Dictionary<int, LocalMenuOption> _options;

		private string _optionInvokeError;

		protected LocalMenu() {
			_options = new Dictionary<int, LocalMenuOption>();
			_optionInvokeError = "";
		}

		protected string optionInvokeError {
			set {
				_optionInvokeError = value;
			}
		}

		protected override void Show() {
			base.Show();

			foreach (KeyValuePair<int, LocalMenuOption> kv in _options) {
				Console.WriteLine(kv.Key + ". " + kv.Value.name);
			}

			if ("" != _optionInvokeError) {
				Console.WriteLine(_optionInvokeError);
				Console.WriteLine();
			}
		}

		protected void AddOption(int index, string name, ExecuteLocalOption fn) {
			_options.Add(index, new LocalMenuOption(name, fn));
		}

		protected override void ExecuteOption(int index) {
			// If any of the option functions throws an exception,
			// this can catch them and automatically display an error message.

			try {
				optionInvokeError = "";
				_options[index].fn.Invoke();
			} catch (Exception e) {
				optionInvokeError = e.ToString();
			}
		}
	}
}
