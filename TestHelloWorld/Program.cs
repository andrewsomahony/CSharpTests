using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	class MainClass {
		public static void Main(string[] args) {
			new ViewStack().PushView(new MainMenu());
		}
	}
}
