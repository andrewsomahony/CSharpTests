using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class InvalidPieceForPromotionException : Exception {
				public InvalidPieceForPromotionException(char p) :
				base("Piece is invalid for promotion! (" + p + ")") {

				}
			}

			public class InvalidPieceForCreationException : Exception {
				public InvalidPieceForCreationException(char p) :
				base("Piece is invalid for creation! (" + p + ")") {

				}
			}

			public partial class ChessGame : BoardGame {
				private char[] validPromotionPieces = new char[] {
					'Q', 'B', 'R', 'N'
				};

				protected bool IsValidPromotionPiece(char newPieceCharacter) {
					return Array.Exists(validPromotionPieces, piece => newPieceCharacter == piece);
				}

				public bool needsPiecePromotionInput {
					get {
						return null != _pieceToBePromoted;
					}
				}

				public void NeedsAPiecePromoted(ChessPiece p) {
					RequireInput();
					_pieceToBePromoted = p;
				}

				private void PromotePiece(ChessPlayer p, ChessPiece oldPiece, ChessPiece newPiece) {
					newPiece.isExtra = true;
					_pieces.Add(newPiece);

					chessBoard.RemovePiece(oldPiece);
					_piecesTakenOrRemoved.Push(oldPiece);

					chessBoard.PlacePiece(newPiece, oldPiece.coordinates);
				}

				public void UnPromotePiece(ChessPiece piece, ChessPiece pawn) {
					chessBoard.RemovePiece(piece);
					_pieces.Remove(piece);

					chessBoard.AddPiece(pawn);
				}

				public void PromotePiece(ChessPlayer p, ChessPiece oldPiece, char newPieceCharacter) {
					if (false == IsValidPromotionPiece(newPieceCharacter)) {
						throw new InvalidPieceForPromotionException(newPieceCharacter);
					}

					PromotePiece(p, oldPiece, CreateNewPiece(newPieceCharacter, p.isWhite));
				}

				public void PromoteCurrentPlayerPiece(char newPieceCharacter) {
					PromotePiece(currentChessPlayer, _pieceToBePromoted, newPieceCharacter);
				}
			}
		}
	}
}
