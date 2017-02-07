using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public partial class ChessGame {
					// Checks if the current player has won the game.

					private void CheckCheckAndCheckmate(ref AlgebraicMove finalMove) {
						CheckCheckAndCheckmate(currentChessPlayer, ref finalMove);
					}

					private void CheckCheckAndCheckmate(ChessPlayer p, ref AlgebraicMove finalMove) {
						ChessPlayer op = OpposingPlayer(p);

						King king = DefaultKing(op) as King;

						ChessPieceToChessMoveDictionary piecesThatCanAttackKing =
							new ChessPieceToChessMoveDictionary();

						finalMove.resultsInCheckmate = false;
						finalMove.ClearChecks();

						// The only pieces that capture differently are pawns, so we know
						// the square we're capturing to is occupied (it has the king on it),
						// so we don't need to distinguish.  Plus, the rules will handle that.

						GetPiecesFromListThatCanLegallyMoveToCoordinate(king.chessCoordinates,
																	PlayerPiecesOnBoard(p),
																	ref piecesThatCanAttackKing);

						if (piecesThatCanAttackKing.Count > 0) {
							// Check to see if any of the opposing pieces are in the way of
							// the lanes to capture the king.

							ChessPieceToChessMoveDictionary tempPiecesThatCanAttackKing =
								new ChessPieceToChessMoveDictionary(piecesThatCanAttackKing);

							piecesThatCanAttackKing.Clear();

							foreach (KeyValuePair<ChessPiece, ChessMove> kvp in tempPiecesThatCanAttackKing) {
								try {
									CheckRulesBeforeMove(p, kvp.Value, kvp.Key,
														 false);
									finalMove.Check();
									piecesThatCanAttackKing[kvp.Key] = kvp.Value;
								} catch (Exception) {
									// This move cannot be made, so we just suppress the
									// exception
								}
							}

							// Now check to see if this is not just check, but checkmate.

							if (piecesThatCanAttackKing.Count > 0) {
								// Now we check if other pieces can block the lanes to the King

								tempPiecesThatCanAttackKing =
									new ChessPieceToChessMoveDictionary(piecesThatCanAttackKing);

								piecesThatCanAttackKing = new ChessPieceToChessMoveDictionary();

								foreach (KeyValuePair<ChessPiece, ChessMove> kvp in tempPiecesThatCanAttackKing) {
									ChessPieceToChessMoveDictionary piecesThatCanBlock =
										new ChessPieceToChessMoveDictionary();

									// All except the king, as it can't block itself

									GetPiecesFromListThatCanBlockAPiecesMove(p,
																			 kvp.Key,
																			 kvp.Value,
																			 PlayerPiecesOnBoard(op)
																			 .FindAll(piece => 'K' != piece.character),
																			 false,
																			 ref piecesThatCanBlock);

									// Check if these pieces can legally block this piece from attacking the king.
									// If the move is illegal, we suppress the exception, and the move is unblockable
									// by that piece.

									bool moveCanBeBlocked = false;
									foreach (KeyValuePair<ChessPiece, ChessMove> kvp2 in piecesThatCanBlock) {
										try {
											CheckRulesBeforeMove(op, kvp2.Value, kvp2.Key, true);
											moveCanBeBlocked = true;
										} catch (Exception) {
											// Suppress
										}
									}

									if (false == moveCanBeBlocked) {
										piecesThatCanAttackKing[kvp.Key] = kvp.Value;
									}
								}

								if (piecesThatCanAttackKing.Count > 0) {
									// Now we check if the king can move out of the way
									// or capture the respective piece threatening it.

									bool kingCanMakeAMove = false;
									foreach (ChessMove m in king.movesToAvoidCheck) {
										try {
											chessBoard.ValidateChessCoordinates(
												ChessCoordinate.FromCoordinate(m.AddToCoordinate(king.coordinates)));

											CheckRulesBeforeMove(op, m, king, true);

											kingCanMakeAMove = true;
										} catch (Exception) {
											// The king cannot make this move for whatever reason
											// the OwnKingInCheckRule will throw an exception if this
											// move puts the king into check.
										}
									}

									if (false == kingCanMakeAMove) {
										finalMove.resultsInCheckmate = true;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
