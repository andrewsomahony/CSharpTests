using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			class OwnKingIsInCheckException : Exception {
				public OwnKingIsInCheckException() : base("This move places your king in check!") {
					
				}
			}

			class OwnKingInCheckRule : ChessRule {
				private bool CoordinateIntersectsOpposingPieceMove(ChessCoordinate c, ChessPiece p, ChessMove m) {
					Coordinate tempCoordinates = p.coordinates;

					// We start at -1 as capturing the piece counts as intersecting it.

					int stepIndex = -1;
					do {
						if (c == ChessCoordinate.FromCoordinate(tempCoordinates)) {
							return true;
						}
						stepIndex++;
					} while (true == m.TakeStep(stepIndex, ref tempCoordinates));

					return false;
				}

				public override bool Applies(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
					return true;
				}

				public override void CheckMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {

					ChessPiece ourKing = game.DefaultKing(player);

					ChessPieceToChessMoveDictionary piecesAndMovesThatCanAttackKing = 
						new ChessPieceToChessMoveDictionary();

					// So we make the move, and make sure to un-make it later.
					// This just helps us by not throwing in so many strange edge cases
					// that we have to handle in specific exceptions.  We want ANY exception
					// on a move check to be because the move is completely illegal and against
					// the rules.

					piece.Move(move);

					game.GetPiecesFromListThatCanLegallyMoveToCoordinate(ourKing.chessCoordinates,
																game.PlayerPiecesOnBoard(game.OpposingPlayer(player)),
																ref piecesAndMovesThatCanAttackKing);

					try {
						foreach (KeyValuePair<ChessPiece, ChessMove> kvp in piecesAndMovesThatCanAttackKing) {
							ChessPiece p = kvp.Key;
							ChessMove m = kvp.Value;

							try {
								// If we check if the other player's king is in check on this move,
								// it's an infinite recursion.  We don't need to check if the move to
								// the king's square will leave their king in check, as in Chess,
								// pieces don't move to the King's square...only threaten it from a distance.

								game.CheckRulesBeforeMove(game.OpposingPlayer(player),
														  m, p, false);

								// If this piece that can attack the king is on the square that this piece
								// is moving to, then it's a capture and the king is not threatened by this piece.

								if (piece.coordinates != p.coordinates) {
									throw new OwnKingIsInCheckException();
								}
							} catch (PieceInTheWayException e) {
								// If the king tries to move onto a piece occupied by another opposing piece,
								// but is still in check from the piece we're checking in the loop.

								// The exception is thrown because we haven't actually executed the entire move.
								// All we do is set the piece's coordinates to what the move states, but run none of the
								// rules, and their after-effects.  Therefore, the capture rule is never run, so the game
								// still thinks a piece that is to be captured is still there when the rule check happens.

								if (1 == e.pieces.Count &&
								    ourKing == piece &&
									e.pieces[0].coordinates == piece.coordinates) {
									throw new OwnKingIsInCheckException();
								}
							} catch (OwnKingIsInCheckException e) {
								throw e;
							} catch (Exception) {
								// Suppress as the move is illegal, and this piece cannot attack the king legally.
							}
						}
					} catch (OwnKingIsInCheckException e) {
						throw e;
					} finally {
						piece.UndoMove(move);
					}
				}

				public override void AfterMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
											   ref AlgebraicMove finalMove) {
				}				
			}
		}
	}
}
