using System;
namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public class Bishop : ChessPiece {
					public Bishop(bool isWhite) : base('B', isWhite) {
						AddMove(new ChessMove(-1, 1, true));
						AddMove(new ChessMove(1, 1, true));
						AddMove(new ChessMove(1, -1, true));
						AddMove(new ChessMove(-1, -1, true));
					}
				}
			}
		}
	}
}
