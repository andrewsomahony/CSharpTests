using System;
namespace TestHelloWorld {
	// Allows Views to be used with the ViewStack
	public interface IStackView : IView {
		ViewStack parentViewStack {
			get; set;
		}

		void SendCloseCommand();
		void SendExitCommand();
	}
}
