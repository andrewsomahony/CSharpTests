using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			// The cannot castle kingside/queenside exceptions are thrown
			// if the game overrides the castling with the boolean values
			// of "whiteCannotCastleKingside" etc.

			// These are set when a game is loaded from a file, otherwise they
			// are meaningless and always false.

			class PlayerCannotCastleKingsideException : Exception {
				public PlayerCannotCastleKingsideException() : base("Kingside castling is not possible!") {
					
				}
			}

			class PlayerCannotCastleQueensideException : Exception {
				public PlayerCannotCastleQueensideException() : base("Queenside castling is not possible!") {

				}
			}

			class NoRookToCastleWithException : Exception {
				public NoRookToCastleWithException() : base("No valid rook to castle with!") {
					
				}
			}

			class RookHasAlreadyMovedException : Exception {
				public RookHasAlreadyMovedException() : base("Rook has already moved!") {
					
				}
			}

			class KingHasAlreadyMovedException : Exception {
				public KingHasAlreadyMovedException() : base("King has already moved!") {
					
				}
			}

			class KingCannotCastleOutOfCheckException : Exception {
				public KingCannotCastleOutOfCheckException() : base("King cannot castle out of check!") {
					
				}
			}

			class KingCannotCastleThroughCheckException : Exception {
				public KingCannotCastleThroughCheckException() : base("King cannot castle through check!") {
					
				}
			}

			class PieceInTheWayOfKingCastleException : Exception {
				public PieceInTheWayOfKingCastleException() : base("A piece is in the way!") {
					
				}
			}

			class CastlingRule : ChessRule {
				// The King in Chess can castle to either the kingside or queenside,
				// so we need to figure out which one that is.

				private ChessCoordinate GetChessCoordinatesForCastlingRook(ChessGame game, ChessPlayer player,
																		   ChessMove castlingMove) {
					string file = castlingMove.xDisplacement < 0 ? "a"
									  : ((char)('a' + (game.chessBoard.width - 1))).ToString();

					int rank = true == player.isWhite ? 1 : game.chessBoard.height;

					return new ChessCoordinate(rank, file);
				}

				private ChessPiece GetRookToCastleWith(ChessGame game, ChessPlayer player, ChessMove move) {
					return game.chessBoard.GetPieceForChessCoordinate(
						GetChessCoordinatesForCastlingRook(game, player, move));					
				}

				public override bool Applies(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
					return 'K' == piece.character &&
						    2 == Math.Abs(move.xDisplacement);
				}

				public override void CheckMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece) {
					// We have some overrides that are set if the game is loaded from a file or something.
					// Check them first.

					ChessCoordinate rookCoordinate = GetChessCoordinatesForCastlingRook(game, player, move);
					bool isQueenside = "a" == rookCoordinate.file ? true : false;
					if (true == player.isWhite) {
						if (true == isQueenside) {
							if (true == game.whiteCannotCastleQueenside) {
								throw new PlayerCannotCastleQueensideException();
							}
						} else {
							if (true == game.whiteCannotCastleKingside) {
								throw new PlayerCannotCastleKingsideException();
							}
						}
					} else {
						if (true == isQueenside) {
							if (true == game.blackCannotCastleQueenside) {
								throw new PlayerCannotCastleQueensideException();
							}
						} else {
							if (true == game.blackCannotCastleKingside) {
								throw new PlayerCannotCastleKingsideException();
							}
						}						
					}

					// The king cannot castle when he is in check, and also
					// cannot castle THROUGH check.

					// Also, the king cannot have moved, and the rook cannot have moved as well.

					if (true == piece.hasMoved) {
						throw new KingHasAlreadyMovedException();
					}

					// Get which rook we're gonna castle with.

					ChessPiece rook = GetRookToCastleWith(game, player, move);

					if (null == rook ||
						'R' != rook.character ||
						player.isWhite != rook.isWhite) {
						throw new NoRookToCastleWithException();
					}

					if (true == rook.hasMoved) {
						throw new RookHasAlreadyMovedException();
					}

					// The King cannot castle out of check

					AlgebraicMove latestMove = new AlgebraicMove(game.latestMoveString);

					if (true == latestMove.hasCheck) {
						throw new KingCannotCastleOutOfCheckException();
					}

					// Check to make sure the king isn't in check between the start
					// and finish square.  The rest of the rules will take care of the final square.

					ChessMove tempMove = new ChessMove(Math.Sign(move.xDisplacement), move.yDisplacement, false);

					try {
						game.CheckRulesBeforeMove(player, tempMove, piece, true);
					} catch (OwnKingIsInCheckException) {
						throw new KingCannotCastleThroughCheckException();
					}

					// Now make sure that no pieces occupy 
					// the two squares that the king will move to.

					Coordinate tempCoordinates = piece.coordinates;
					int stepIndex = 0;

					while (true == move.TakeStep(stepIndex, ref tempCoordinates)) {
						if (null != game.board.GetPieceForCoordinate(tempCoordinates)) {
							throw new PieceInTheWayOfKingCastleException();
						}
						stepIndex++;
					}
				}

				public override void AfterMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
											   ref AlgebraicMove finalMove) {
					ChessPiece rook = GetRookToCastleWith(game, player, move);

					Coordinate newRookCoordinates = new Coordinate(piece.coordinates.x - Math.Sign(move.xDisplacement),
																   piece.coordinates.y);

					if (-1 == Math.Sign(move.xDisplacement)) {
						finalMove.isQueensideCastle = true;
					} else {
						finalMove.isKingsideCastle = true;
					}

					rook.Move(newRookCoordinates);
				}	

				public override void UndoMove(ChessGame game, 
				                              ChessPlayer player, 
				                              ChessMove move, 
				                              ChessPiece piece, 
				                              AlgebraicMove moveToUndo) {
					if (true == moveToUndo.isKingsideCastle ||
						true == moveToUndo.isQueensideCastle) {
						// All we have to do is un-move the rook we castled with,
						// and the undo engine will move the king back.

						Coordinate rookCoordinates = new Coordinate(piece.coordinates.x - Math.Sign(move.xDisplacement),
																	   piece.coordinates.y);

						ChessPiece rook = game.chessBoard.GetPieceForChessCoordinate(
							ChessCoordinate.FromCoordinate(rookCoordinates));

						rook.UnMove(GetChessCoordinatesForCastlingRook(game, player, move).ToCoordinate());
					}
				}
			}
		}
	}
}
