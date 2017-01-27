using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			// The board keeps track of what pieces are taken or not taken,
			// with the special case of pieces that are promoted.  If a piece
			// is not on the board, and its taken bool is set, then it's taken.

			public class ChessBoard : Board {
				public ChessBoard() : base(8, 8) {
					_pieces = new List<Piece>(32);
				}

				public List<ChessPiece> chessPieces {
					get {
						return _pieces.Cast<ChessPiece>().ToList();
					}
				}

				public List<ChessPiece> PlayerPieces(ChessPlayer p) {
					return _pieces.FindAll(piece => (piece as ChessPiece).isWhite == p.isWhite)
						          .Cast<ChessPiece>().ToList();
				}

				public void ValidateChessCoordinates(ChessCoordinate c) {
					base.ValidateCoordinates(c.ToCoordinate());
				}

				public ChessPiece GetPieceForChessCoordinate(ChessCoordinate c) {
					return base.GetPieceForCoordinate(c.ToCoordinate()) as ChessPiece;
				}

				public void PlacePiece(ChessPiece p, ChessCoordinate c) {
					base.PlacePiece(p, c.ToCoordinate());
				}

				public void MovePiece(ChessPiece p, ChessCoordinate c) {
					base.MovePiece(p, c.ToCoordinate());
				}

				public void TakePiece(ChessPiece p) {
					p.Take();
					RemovePiece(p);
				}

				public void UnTakePiece(ChessPiece p) {
					p.UnTake();
					AddPiece(p);
				}

				// Checks if p1 is behind p2
				// This is based on p1 and relative to the respective
				// player's view.

				// This is currently only used for en passant checking,
				// to make sure the capturing piece is "in front" of the pawn
				// it's trying to pass.

				public int ComparePieceRanks(ChessPiece p1, ChessPiece p2) {
					return CompareRanks(p1.chessCoordinates.rank, p2.chessCoordinates.rank, p1.isWhite);
				}

				public int CompareRanks(int rank1, int rank2, bool rank1IsWhite) {
					if (true == rank1IsWhite) {
						return rank1 - rank2;
					} else {
						return rank2 - rank1;
					}					
				}
			}
		}
	}
}
