using System;
namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public class Queen : ChessPiece {
					public Queen(bool isWhite) : base('Q', isWhite) {
						AddMove(new ChessMove(0, 1, true));
						AddMove(new ChessMove(1, 1, true));
						AddMove(new ChessMove(1, 0, true));
						AddMove(new ChessMove(1, -1, true));
						AddMove(new ChessMove(0, -1, true));
						AddMove(new ChessMove(-1, -1, true));
						AddMove(new ChessMove(-1, 0, true));
						AddMove(new ChessMove(-1, 1, true));
					}
				}
			}
		}
	}
}
