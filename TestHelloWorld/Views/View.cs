using System;
using System.Runtime.InteropServices;

namespace TestHelloWorld {
	using Parsers;
	public abstract class View {
		protected ViewStack _parentViewStack;
		protected bool _isAlive = false;

		private const string _backCommand = "back";
		private const string _quitCommand = "quit";

		public abstract string title {
			get;
		}

		class Reader {
			public string Read(string prompt) {
				Console.Write(prompt + ": ");
				return Console.ReadLine();
			}
		}

		class ConfirmInputNotYesOrNoException : Exception {
			public ConfirmInputNotYesOrNoException(string input) 
				: base("Input needs to be y or n! (" + input + ")") {
				
			}
		}

		private bool ValueIsCommand(string v, string command) {
			return v == "[" + command + "]";
		}

		protected bool ReadAndParse(string prompt, StringParser s, bool canExit = true) {
			Reader r = new Reader();

			// We loop here because this function handles parser errors, such as an invalid integer
			// if the string parser is looking for an integer value.

			while (true) {
				string value = r.Read(prompt);

				DefaultStringParser stringParser = new DefaultStringParser();
				if (true == stringParser.CanParse(value) &&
					true == canExit) {
					stringParser.Parse(value);

					if (true == ValueIsCommand(stringParser.value, _backCommand)) {
						Close();
						return false;
					} else if (true == ValueIsCommand(stringParser.value, _quitCommand)) {
						Exit();
						return false;
					}
				}

				try {
					s.Parse(value);
					return true;
				} catch (StringParserInvalidFormatException e) {
					Console.WriteLine(e.Message);
				} catch (Exception e) {
					throw e;
				}
			}
		}

		protected bool Confirm(string prompt) {
			while (true) {
				try {
					ConfirmCharacterParser confirmCharacterParser = new ConfirmCharacterParser(false);

					ReadAndParse(prompt + " (y/n)", confirmCharacterParser, false);

					return confirmCharacterParser.isYes;
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			}
		}

		protected virtual void Show() {
			if (title.Length > 0) {
				Console.WriteLine(title);

				for (int i = 0; i < title.Length; i++) {
					Console.Write("-");
				}
				Console.Write("\n");
			}
		}

		private void Exit() {
			_isAlive = false;
			_parentViewStack.Exit();
		}

		public virtual void Init() {
			_isAlive = true;
		}

		public virtual void Stop() {
			
		}

		public void Close() {
			_isAlive = false;
		}

		public abstract void Run();

		// View methods, for console manipulation.

		[DllImport("libc")]
		private static extern int system(string exec);

		protected void ResizeScreen(int width, int height) {
			string resizeString = "resize -s " + height + " " + width + " > /dev/null";
			system(resizeString);
		}

		protected void Clear() {
			Console.Clear();
		}

		protected void SetCursorPosition(int x, int y) {
			Console.SetCursorPosition(x, y);
		}

		protected int cursorX {
			get {
				return Console.CursorLeft;
			}
		}

		protected int cursorY {
			get {
				return Console.CursorTop;
			}
		}

		public bool isAlive {
			get {
				return _isAlive;
			}
		}

		public ViewStack parentViewStack {
			get {
				return _parentViewStack;
			}
			set {
				_parentViewStack = value;
			}
		}
	}
}
