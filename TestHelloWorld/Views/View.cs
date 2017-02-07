using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using Parsers;

	using UserInput;
	using StringParsers;

	using Utils;

	public class ViewReceivedExitCommandException : Exception {
		public ViewReceivedExitCommandException() : base("Exit command received!") {
			
		}
	}

	public class ViewReceivedCloseCommandException : Exception {
		public ViewReceivedCloseCommandException() : base("Close command received!") {
			
		}
	}

	public class InvalidStringToParseException : Exception {
		public InvalidStringToParseException(string s) : base("Invalid string to parse! " + s) {
			
		}
	}

	public abstract class View : IView {
		protected ViewStack _parentViewStack;
		private bool _isAlive = false;

		private const string _backCommand = "back";
		private const string _quitCommand = "quit";

		protected IProgram _program;

		public abstract string title {
			get;
		}

		class Reader {
			public string Read(string prompt) {
				Console.Write(prompt);
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

		private void ParseInputString<T>(string inputString, IList<T> values) {
			string[] splitArray = inputString.Split(',');

			if (splitArray.Length < 1) {
				throw new InvalidStringToParseException(inputString);
			} else {
				foreach (string s in splitArray) {
					T parsedValue;
					try {
						StringParserFactory.Instance().Parse(s, out parsedValue);
						values.Add(parsedValue);
					} catch (InvalidStringFormatException) {
						throw new InvalidStringToParseException(inputString);
					}
				}
			}
		}

		private string GetPromptFromUserInput<T>(GenericUserInput<T> userInput) {
			string prompt = "";

			if ("ChessPromotionInput" == userInput.name) {
				prompt += "Which piece to promote to?";
			} else if ("YesOrNoInput" == userInput.name) {
				prompt += ((YesNoUserInput)(object)userInput).question;
			} else {
				prompt += "Enter " + userInput.name;
			}

			if (true == userInput.isMultiple) {
				prompt += "(Enter multiple values separated by a comma)";
			}

			if (userInput is SpecificValuesUserInput<T>) {
				prompt += " (";

				SpecificValuesUserInput<T> specificValuesCast = (SpecificValuesUserInput<T>)userInput;

				int count = 0;
				foreach (T value in specificValuesCast.specificValues) {
					prompt += value;

					if (count < specificValuesCast.specificValues.Count - 1) {
						prompt += "/";
					}

					count++;
				}

				prompt += ")";
			}

			prompt += ": ";

			return prompt;			
		}

		// IView interface

		async public virtual Task<GenericUserInput<T>> GetUserInputAsync<T>(GenericUserInput<T> userInput) {
			await Task.Delay(0); // This function actually blocks, so we suppress a warning by putting this.

			while (true) {
				try {
					ReadAndParse(userInput, "", true);
					break;
				} catch (InvalidStringToParseException) {
					Console.WriteLine("Invalid input, needs to be of type " + typeof(T));
				}
			}
			return userInput;
		}

		async public Task RunProgramAsync(IProgram program) {
			program.LinkToView(this);

			await program.InitAsync();
			await program.RunAsync();
			await program.CloseAsync();
		}

		async public Task RunProgramAsync() {
			await RunProgramAsync(_program);
		}

		// End IView interface

		protected void ReadAndParse<T>(GenericUserInput<T> userInput, 
		                               string customPrompt = "", bool acceptsCommands = true) {
			string prompt = customPrompt;

			if ("" == prompt) {
				prompt = GetPromptFromUserInput(userInput);
			}

			string value = new Reader().Read(prompt);

			if (true == acceptsCommands) {
				if (true == ValueIsCommand(value, _backCommand)) {
					throw new ViewReceivedCloseCommandException();
				} else if (true == ValueIsCommand(value, _quitCommand)) {
					throw new ViewReceivedExitCommandException();
				}
			}

			List<T> outputValues = new List<T>();

			ParseInputString(value, outputValues);
			userInput.SetValues(outputValues);
		}

		// !!! This function is depreciated!

		protected bool ReadAndParse(string prompt, OldStringParser s, bool canExit = true) {
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

		protected void ReadKey(string prompt) {
			Console.WriteLine(prompt);
			Console.ReadKey();
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

		// Exit and Close are depreciated!

		private void Exit() {
			Close();
			_parentViewStack.Exit();
		}

		public void Close() {
			_isAlive = false;
		}

		public virtual void Init() {
			_isAlive = true;
		}

		public virtual void Stop() {
			
		}



		// All the basic view does is close itself.

		public virtual void Run() {
			Close();
		}

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
