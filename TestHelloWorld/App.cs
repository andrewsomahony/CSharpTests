using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace TestHelloWorld {
	using Utils;
	using UserInput;

	class App {
		static Task<string> _moveStringTask;
		static bool _hasInput;
		static string _inputString;

		async static Task<GenericUserInput<T>> GetInputAsync<T>(GenericUserInput<T> userInput) {
			await Task.Delay(0); // Suppress the "this doesn't have await" warning

			string s = Console.ReadLine();

			IList<T> list = new List<T>();

			list.Add((T)(object)Int32.Parse(s));

			userInput.SetValues(list);

			return userInput;
		}

		async static Task<string> GetMoveString() {
			_moveStringTask = new Task<string>(() => { 
				while (!_hasInput) { }
				return _inputString;
			});

			return await _moveStringTask;
		}

		async static Task MainAsync(string[] args) {
			await GetMoveString();
		}


		public static void Main(string[] args) {
			//MainAsync(args).Wait();

			/*AppPromise<int>.CreatePromise((resolve, reject) => {
				resolve.Invoke(1);
			}).promise
				.Then((int i) => Console.WriteLine(i))
				.Catch((exception) => Console.WriteLine(exception))
				.Done();*/

			/*
			CreatePromise<string>(DoInput)
				.Then((inputString) => 
				      Console.WriteLine(inputString))
				.Catch((exception) => Console.WriteLine(exception.Message))
				.Done();*/

			//new ViewStack().PushView(new MainMenu());
			new ViewStack().PushView(new DaysSinceView());
		}
	}
}