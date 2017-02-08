using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;
	using Utils;

	public interface IProgram {
		Task InitAsync();
		Task RunAsync();
		Task CloseAsync();

		void LinkToView(IAsyncView view);

		Task<GenericUserInput<T>> RequiresUserInputAsync<T>(GenericUserInput<T> userInput);
	}
}
