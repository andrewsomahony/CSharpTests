using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public partial class ChessGame {
				public List<ChessPiece> PlayerPieces(ChessPlayer p) {
					return chessPieces.FindAll(piece => piece.isWhite == p.isWhite);
				}

				public List<ChessPiece> PlayerPiecesOnBoard(ChessPlayer p) {
					return PlayerPieces(p).Intersect(chessBoard.PlayerPieces(p)).ToList();
				}

				public List<ChessPiece> PlayerPiecesThatAreTaken(ChessPlayer p) {
					return PlayerPieces(p).Except(PlayerPiecesOnBoard(p)).ToList();
				}

				public ChessPiece DefaultKing(ChessPlayer p) {
					return PlayerPieces(p).Find(piece => 'K' == piece.character &&
					                            false == piece.isExtra);
				}

				public ChessPiece DefaultQueen(ChessPlayer p) {
					return PlayerPieces(p).Find(piece => 'Q' == piece.character &&
					                            false == piece.isExtra);
				}

				public List<ChessPiece> DefaultBishops(ChessPlayer p) {
					return PlayerPieces(p).FindAll(piece => 'B' == piece.character &&
					                               false == piece.isExtra);
				}

				public List<ChessPiece> DefaultKnights(ChessPlayer p) {
					return PlayerPieces(p).FindAll(piece => 'N' == piece.character &&
					                               false == piece.isExtra);
				}

				public List<ChessPiece> DefaultRooks(ChessPlayer p) {
					return PlayerPieces(p).FindAll(piece => 'R' == piece.character &&
					                               false == piece.isExtra);
				}

				public List<ChessPiece> DefaultPawns(ChessPlayer p) {
					return PlayerPieces(p).FindAll(piece => 'P' == piece.character &&
					                               false == piece.isExtra);
				}

				protected ChessPiece CreateNewPiece(char newPieceCharacter, bool isWhite) {
					switch (newPieceCharacter) {
						case 'Q':
							return new Queen(isWhite);
						case 'N':
							return new Knight(isWhite);
						case 'R':
							return new Rook(isWhite);
						case 'B':
							return new Bishop(isWhite);
						case 'P':
							return new Pawn(isWhite);
						case 'K':
							return new King(isWhite);
						default:
							throw new InvalidPieceForCreationException(newPieceCharacter);
					}
				}
			}
		}
	}
}
