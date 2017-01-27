using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			class PiecesInTheWayException : Exception {
				readonly List<ChessPiece> _pieces;
				public PiecesInTheWayException(List<ChessPiece> pieces) : base("Pieces are in the way!") {
					_pieces = pieces;
				}

				public List<ChessPiece> pieces {
					get {
						return _pieces;
					}
				}
			}

			class PieceInTheWayException : PiecesInTheWayException {
				public PieceInTheWayException(ChessPiece piece) : base(new List<ChessPiece>() {piece}) {
					
				}
			}

			// Checks if any pieces are in the way.
			// Has exceptions for the Knight hopping over a piece,
			// as well as if an opposing color piece is on the final square

			// The pawn rules will further check that, if the piece is a pawn,
			// and there is an opposing color piece on the final square, that
			// the pawn is trying to capture that piece, as opposed to just move
			// on top of it.

			class NoPieceInTheWayRule : ChessRule {
				public override bool Applies(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
					return true;
				}

				public override void CheckMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
					Coordinate startCoordinates = piece.coordinates;

					List<ChessPiece> piecesInTheWay = new List<ChessPiece>();

					// Get all the pieces in the way, EXCEPT for the final square.
					game.GetPiecesFromListThatAreIntheWayOfAMove(move, piece, game.chessPieces, ref piecesInTheWay);

					if (piecesInTheWay.Count > 0 &&
						'N' != piece.character) {
						throw new PieceInTheWayException(piecesInTheWay[0]);
					} else {
						ChessPiece occupyingPiece = 
							game.chessBoard.GetPieceForCoordinate(move.AddToCoordinate(piece.coordinates)) as ChessPiece;
						if (null != occupyingPiece) {
							if (occupyingPiece.isWhite == player.isWhite) {
								throw new PieceInTheWayException(occupyingPiece);
							}
						}
					}
				}

				public override void AfterMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
											   ref AlgebraicMove finalMove) { 
				}
			}
		}
	}
}
