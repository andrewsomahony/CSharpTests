using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;
	using Utils;

	public interface IView {
		Task<GenericUserInput<T>> GetUserInputAsync<T>(GenericUserInput<T> userInput);
		Task RunProgramAsync(IProgram program);
	}
}
