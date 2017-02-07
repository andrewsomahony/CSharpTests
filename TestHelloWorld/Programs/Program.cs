using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;
	using Utils;

	public class Program : IProgram {
		private IView _view;

		public Program() {
		}

		async public Task<GenericUserInput<T>> RequiresUserInputAsync<T>(GenericUserInput<T> userInput) {
			await _view.GetUserInputAsync(userInput);

			return userInput;
		}

		async public virtual Task InitAsync() {
			await Task.Delay(0);
		}

		async public virtual Task RunAsync() {
			await Task.Delay(0);
		}

		async public virtual Task CloseAsync() {
			await Task.Delay(0);
		}

		public void LinkToView(IView v) {
			_view = v;
		}
	}
}
