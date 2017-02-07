using System;
namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				class InvalidEnPassantAttemptException : Exception {
					public InvalidEnPassantAttemptException() : base("Invalid en-passant attempt!") {

					}
				}

				// Makes sure that pawns only move diagonally if they are capturing something,
				// or if it is an en passant capture.

				class EnPassantRule : ChessRule {
					// finalCoordinates are the coordinates of the piece that's doing the capturing.
					private bool EnPassantIsValid(ChessGame game, ChessPlayer player, Coordinate finalCoordinates) {
						AlgebraicMove latestMove = new AlgebraicMove(game.latestMoveString);
						ChessPiece latestPieceToMove = game.latestPieceToMove;
						ChessCoordinate finalChessCoordinate =
							ChessCoordinate.FromCoordinate(finalCoordinates);

						if ('P' == latestPieceToMove.character &&
							1 == latestPieceToMove.numMoves &&
							latestMove.newCoordinates.file == finalChessCoordinate.file &&
							1 == game.chessBoard.CompareRanks(finalChessCoordinate.rank,
															  latestPieceToMove.chessCoordinates.rank, player.isWhite)) {
							return true;
						} else {
							return false;
						}
					}

					public override bool Applies(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
						Coordinate finalCoordinates = move.AddToCoordinate(piece.coordinates);

						return 'P' == piece.character &&
								0 != move.xDisplacement &&
								null == game.chessBoard.GetPieceForCoordinate(finalCoordinates);
					}

					public override void CheckMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
						if (false == EnPassantIsValid(game, player,
													 move.AddToCoordinate(piece.coordinates))) {
							throw new InvalidEnPassantAttemptException();
						}
					}

					public override void AfterMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
												   ref AlgebraicMove finalMove) {
						// game.latestPieceToMove is still the previous piece that completed the move cycle,
						// which has been verified to be a pawn that moved in a way that it can be captured
						// en passant.

						game.TakePiece(game.latestPieceToMove);
						finalMove.isEnPassant = true;
						finalMove.isCapture = true;
					}

					public override void UndoMove(ChessGame game,
												  ChessPlayer player,
												  ChessMove move,
												  ChessPiece piece,
												  AlgebraicMove moveToUndo) {
						// En Passant is a capture, so the capture rule deals with
						// the undo-ing of the move
					}
				}
			}
		}
	}
}
