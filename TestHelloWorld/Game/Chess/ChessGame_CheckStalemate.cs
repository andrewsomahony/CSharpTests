using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public partial class ChessGame {
				// Checks to see if the current player cannot make a move.

				protected void CheckStalemate() {
					CheckStalemate(currentChessPlayer);
				}

				protected void CheckStalemate(ChessPlayer p) {
					// Check every coordinate on the board,
					// and see if we can make a legal move with any of our
					// pieces there.

					// If we can't, then the game is a stalemate.

					bool hasLegalMove = false;
					for (int row = 1; row <= _board.height && false == hasLegalMove; row++) {
						for (int col = 1; col <= _board.width && false == hasLegalMove; col++) {
							ChessPieceToChessMoveDictionary validPiecesAndMoves =
								new ChessPieceToChessMoveDictionary();
							
							GetPiecesFromListThatCanLegallyMoveToCoordinate(ChessCoordinate.FromCoordinate(new Coordinate(col, row)),
							                                         PlayerPiecesOnBoard(p),
																	 ref validPiecesAndMoves);

							foreach (KeyValuePair<ChessPiece, ChessMove> kvp in validPiecesAndMoves) {
								try {
									CheckRulesBeforeMove(p, kvp.Value, kvp.Key, true);
									hasLegalMove = true;
									break;
								} catch (Exception) {
									// If it's an illegal move then we can't move here.
								}
							}
						}
					}

					if (false == hasLegalMove) {
						_isAStalemate = true;
					}
				}
			}
		}
	}
}
