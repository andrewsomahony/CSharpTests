using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public partial class ChessGame {
					// Used for Checkmate checking
					// "player" is the player that's trying to make the move,
					// so we check the array of opposing pieces.

					public void GetPiecesFromListThatCanBlockAPiecesMove(ChessPlayer player,
																		 ChessPiece pieceToMove, ChessMove move,
																		 IReadOnlyList<ChessPiece> opposingPieces,
																		 bool checkFinalSquare,
													ref ChessPieceToChessMoveDictionary piecesAndMovesThatCanBlock) {

						// Blocking a piece's move includes taking that piece,
						// so we start at -1 to check the initial coordinate as well.

						int stepCount = -1;
						Coordinate tempCoordinates = pieceToMove.coordinates;
						do {

							ChessPieceToChessMoveDictionary piecesAndMovesForSquare =
								new ChessPieceToChessMoveDictionary();

							// We don't need to check what can capture, as if a piece is in the way,
							// then a piece is in the way and the move is already blocked.

							GetPiecesFromListThatCanMoveToCoordinate(ChessCoordinate.FromCoordinate(tempCoordinates),
																	 opposingPieces,
																	 ref piecesAndMovesForSquare);

							foreach (KeyValuePair<ChessPiece, ChessMove> kvp in piecesAndMovesForSquare) {
								try {
									CheckRulesBeforeMove(OpposingPlayer(player),
														 kvp.Value,
														 kvp.Key,
														 true);

									piecesAndMovesThatCanBlock[kvp.Key] = kvp.Value;
								} catch (Exception) {
									// If this move cannot be made, no big deal
								}
							}

							stepCount++;

							if (false == checkFinalSquare &&
								stepCount == move.stepCount - 1) {
								break;
							}

						} while (true == move.TakeStep(stepCount, ref tempCoordinates));
					}

					// These functions are used by the rules for Checkmate checking

					// This one checks if there's any pieces in the way of a move,
					// BUT DOES NOT CHECK the final square.

					public void GetPiecesFromListThatAreIntheWayOfAMove(ChessMove move, ChessPiece piece,
																		IReadOnlyList<ChessPiece> pieces,
																		IList<ChessPiece> piecesInTheWay) {
						Coordinate startCoordinates = piece.coordinates;

						// Each move is actually a number of steps...
						// Think as a kid when you tapped on each square when
						// you moved a piece in any game; each tap = a step

						int stepCount = 0;
						int maxStepCount = move.stepCount;
						Coordinate tempCoordinates = piece.coordinates;

						// Each move is a series of steps.
						// Think when you were a kid, and you tapped on each
						// square when you moved a piece.  One tap = one step

						while (true == move.TakeStep(stepCount, ref tempCoordinates) &&
							   stepCount < maxStepCount - 1) {
							ChessPiece occupyingPiece =
								chessBoard.GetPieceForCoordinate(tempCoordinates) as ChessPiece;

							// Make sure it's in our list of pieces we provided.
							if (null != occupyingPiece &&
								null != pieces.Where(p => p == occupyingPiece)) {
								piecesInTheWay.Add(occupyingPiece);
							}
							stepCount++;
						}
					}

					private void GetPiecesFromListThatCanMoveToCoordinate(ChessCoordinate c,
																		  IReadOnlyList<ChessPiece> pieces,
													  ref ChessPieceToChessMoveDictionary validPiecesAndMoves) {
						foreach (ChessPiece piece in pieces) {
							ChessMove proposedMove = ChessMove.FromOldAndNewCoordinates(c,
															   ChessCoordinate.FromCoordinate(piece.coordinates));

							foreach (ChessMove m in piece.chessMoves) {
								if (true == m.IsLegal(proposedMove)) {
									validPiecesAndMoves[piece] = proposedMove;
								}
							}
						}
					}

					public void GetPiecesFromListThatCanLegallyMoveToCoordinate(ChessCoordinate c,
																				IReadOnlyList<ChessPiece> pieces,
																 ref ChessPieceToChessMoveDictionary validPiecesAndMoves) {
						GetPiecesFromListThatCanMoveToCoordinate(c, pieces, ref validPiecesAndMoves);
					}
				}
			}
		}
	}
}
