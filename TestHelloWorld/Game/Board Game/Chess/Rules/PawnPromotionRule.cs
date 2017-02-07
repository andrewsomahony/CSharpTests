using System;
using System.Diagnostics;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				class PawnPromotionRule : ChessRule {
					public override bool Applies(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
						return 'P' == piece.character;
					}

					public override void CheckMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {

					}

					public override void AfterMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
												   ref AlgebraicMove finalMove) {
						bool needsPromotion = false;
						if (true == player.isWhite) {
							needsPromotion = 8 == ChessCoordinate.FromCoordinate(piece.coordinates).rank;
						} else {
							needsPromotion = 1 == ChessCoordinate.FromCoordinate(piece.coordinates).rank;
						}

						if (true == needsPromotion) {
							if (true == finalMove.promotionPiece.HasValue) {
								game.PromotePiece(player, piece, finalMove.promotionPiece.Value);
							} else {
								game.NeedsAPiecePromoted(piece);
							}
						} else {
							finalMove.promotionPiece = null;
						}
					}

					public override void UndoMove(ChessGame game,
												  ChessPlayer player,
												  ChessMove move,
												  ChessPiece piece,
												  AlgebraicMove moveToUndo) {
						// All this needs to do is put the original pawn back,
						// and then the undo engine will figure out that it's there,
						// and move it back to where it was.

						if (true == moveToUndo.hasPromotion) {
							ChessPiece pawn = game.PopLatestRemovedPiece();

							// Asserts are for bugs in the code.
							Debug.Assert(pawn != piece);

							game.UnPromotePiece(game.chessBoard.GetPieceForChessCoordinate(moveToUndo.newCoordinates),
												game.PopLatestRemovedPiece());
						}
					}
				}
			}
		}
	}
}
