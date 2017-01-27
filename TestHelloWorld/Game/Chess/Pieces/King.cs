using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class King : ChessPiece {
				public King(bool isWhite) : base('K', isWhite) {
					AddMove(new ChessMove(0, 1, false));
					AddMove(new ChessMove(1, 1, false));
					AddMove(new ChessMove(1, 0, false));
					AddMove(new ChessMove(1, -1, false));
					AddMove(new ChessMove(0, -1, false));
					AddMove(new ChessMove(-1, -1, false));
					AddMove(new ChessMove(-1, 0, false));
					AddMove(new ChessMove(-1, 1, false));

					// Castling
					AddMove(new ChessMove(-2, 0, false));
					AddMove(new ChessMove(2, 0, false));
				}

				internal List<ChessMove> movesToAvoidCheck {
					get {
						return chessMoves.FindAll(m => Math.Abs(m.xDisplacement) <= 1 &&
										   Math.Abs(m.yDisplacement) <= 1);
					}
				}
			}
		}
	}
}
