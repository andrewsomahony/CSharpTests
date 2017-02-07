using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace TestHelloWorld {
	using Utils;
	using UserInput;

	class App {
		static AnyIntValueUserInput _numSettlements;
		static AnyIntValueUserInput _numCities;
		static bool isAlive;

		/*async static Task<GenericUserInput<T>> GetInputAsync<T>(GenericUserInput<T> userInput) {
			await Task.Delay(0); // Suppress the "this doesn't have await" warning

			string s = Console.ReadLine();

			IList<T> list = new List<T>();

			list.Add((T)(object)Int32.Parse(s));

			userInput.SetValues(list);

			return userInput;
		}

		async static Task InitAsync() {
			_numSettlements = new AnyIntValueUserInput();
			_numCities = new AnyIntValueUserInput();

			try {
				await GetInputAsync(_numSettlements);
				await GetInputAsync(_numCities);
			} catch (Exception) {
				isAlive = false;
			}
		}*/

		/*async static Task MainAsync(string[] args) {
			isAlive = true;
			await InitAsync();

			while (isAlive) {
				Console.WriteLine(_numSettlements);
				Console.WriteLine(_numCities);
			}
		}
		*/
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