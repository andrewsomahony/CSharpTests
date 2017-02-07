using System;
using System.Linq;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				class InvalidPawnCaptureException : Exception {
					public InvalidPawnCaptureException() : base("Invalid pawn capture!") {

					}
				}

				class CaptureRule : ChessRule {
					public override bool Applies(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
						Coordinate newCoordinates = move.AddToCoordinate(piece.coordinates);

						ChessPiece occupyingPiece = game.board.GetPieceForCoordinate(newCoordinates) as ChessPiece;

						if (null != occupyingPiece) {
							if (occupyingPiece.isWhite != player.isWhite) {
								return true;
							}
						}
						return false;
					}

					public override void CheckMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
						// We know that there's a piece occupying the final square (thanks to Applies),
						// now we just check that it's a legal capture.

						if ('P' == piece.character) {
							if (0 == move.xDisplacement) {
								throw new InvalidPawnCaptureException();
							}
						}
					}

					public override void AfterMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
												   ref AlgebraicMove finalMove) {

						List<ChessPiece> occupyingPieces =
							game.board.GetPiecesForCoordinate(piece.coordinates).Cast<ChessPiece>().ToList();

						ChessPiece occupyingPiece = occupyingPieces.Find(p => p.isWhite != player.isWhite);

						game.TakePiece(occupyingPiece);

						finalMove.isCapture = true;
					}

					public override void UndoMove(ChessGame game,
												  ChessPlayer player,
												  ChessMove move,
												  ChessPiece piece,
												  AlgebraicMove moveToUndo) {
						// All this has to do is put the original piece back,
						// and then the undo engine knows which piece captured it,
						// and will move it back.

						if (true == moveToUndo.isCapture) {
							game.UnTakePiece(game.PopLatestRemovedPiece());
						}
					}
				}
			}
		}
	}
}
