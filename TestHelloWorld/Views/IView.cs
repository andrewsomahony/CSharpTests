using System;
using System.Threading.Tasks;

namespace TestHelloWorld {
	using UserInput;
	using Utils;

	public interface IView {
		void Init();
		void Stop();

		void Run();
		void Close();

		string title {
			get;
		}
	}
}
