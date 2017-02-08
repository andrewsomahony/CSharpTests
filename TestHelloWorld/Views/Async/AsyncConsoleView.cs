using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;

	public abstract class AsyncConsoleView : ConsoleView, IAsyncView {
		protected IProgram _program;

		// IAsyncView interface

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

		abstract public Task RunAsync();

		async public virtual Task InitAsync() {
			await Task.Delay(0);
		}

		async public virtual Task StopAsync() {
			await Task.Delay(0);
		}

		public override void Run() {
			RunAsync().Wait();
		}

		public override void Init() {
			InitAsync().Wait();

			base.Init();
		}

		public override void Stop() {
			StopAsync().Wait();
		}

		// End IAsyncView interface
	}
}
