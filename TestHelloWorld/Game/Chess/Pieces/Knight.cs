using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class Knight : ChessPiece {
				public Knight(bool isWhite) : base('N', isWhite) {
					AddMove(new ChessMove(1, 2, false));
					AddMove(new ChessMove(2, 1, false));
					AddMove(new ChessMove(2, -1, false));
					AddMove(new ChessMove(1, -2, false));
					AddMove(new ChessMove(-1, -2, false));
					AddMove(new ChessMove(-2, -1, false));
					AddMove(new ChessMove(-2, 1, false));
					AddMove(new ChessMove(-1, 2, false));
				}
			}
		}
	}
}
