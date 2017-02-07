using System;
namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public class Rook : ChessPiece {
					public Rook(bool isWhite) : base('R', isWhite) {
						AddMove(new ChessMove(1, 0, true));
						AddMove(new ChessMove(-1, 0, true));
						AddMove(new ChessMove(0, 1, true));
						AddMove(new ChessMove(0, -1, true));
					}
				}
			}
		}
	}
}
