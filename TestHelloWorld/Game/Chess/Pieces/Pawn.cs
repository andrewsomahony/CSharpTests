using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class Pawn : ChessPiece {
				public Pawn(bool isWhite) : base('P', isWhite) {
					// So, the Chess (and our) coordinate system
					// does deal in positive and negative numbers.

					// White forwards moves are always positive,
					// while black forwards moves are always negative.

					// The move engine just makes sure that the amount
					// and direction of movement fits the piece being moved,
					// and also is valid for the board (not moving off the board),
					// while the rules engine determines if the move is within the rules of
					// the specific game, in this case, Chess.

					if (true == isWhite) {
						AddMove(new ChessMove(0, 1, false));
						AddMove(new ChessMove(0, 2, false));

						// Capture moves
						AddMove(new ChessMove(-1, 1, false));
						AddMove(new ChessMove(1, 1, false));
					} else {
						AddMove(new ChessMove(0, -1, false));
						AddMove(new ChessMove(0, -2, false));

						// Capture moves
						AddMove(new ChessMove(1, -1, false));
						AddMove(new ChessMove(-1, -1, false));
					}
				}
			}
		}
	}
}
