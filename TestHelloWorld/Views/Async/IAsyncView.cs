using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;
	using Utils;

	public interface IAsyncView : IView, IStackView {
		Task<GenericUserInput<T>> GetUserInputAsync<T>(GenericUserInput<T> userInput);
		Task RunProgramAsync(IProgram program);

		Task InitAsync();
		Task StopAsync();
		Task RunAsync();
	}
}
