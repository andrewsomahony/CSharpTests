using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class ChessPlayer : Player {
				private bool _isWhite;

				public ChessPlayer(string name, bool isWhite) : base(name) {
					_isWhite = isWhite;
				}

				public bool isWhite { 
					get {
						return _isWhite;
					}
				}
			}
		}
	}
}
