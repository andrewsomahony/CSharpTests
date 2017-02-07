using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		public abstract class Player {
			private string _name;

			public Player(string name) {
				_name = name;
			}

			public abstract void Reset();

			public string name {
				get {
					return _name;
				}
				set {
					_name = value;
				}
			}
		}
	}
}
