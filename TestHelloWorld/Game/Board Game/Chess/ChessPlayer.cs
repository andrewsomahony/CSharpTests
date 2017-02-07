using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public class ChessPlayer : Player {
					private bool _isWhite;

					public ChessPlayer(string name, bool isWhite) : base(name) {
						_isWhite = isWhite;
					}

					public override void Reset() {
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
}
