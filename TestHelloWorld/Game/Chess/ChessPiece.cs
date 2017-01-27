using System;
using System.Linq;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class ChessPiece : Piece {
				private bool _isTaken;
				private readonly bool _isWhite;
				private bool _isExtra; // For promoted pieces

				protected ChessPiece(char c, bool isWhite, bool isExtra) : base(c) {
					_isWhite = isWhite;
					_isExtra = isExtra;
				}

				protected ChessPiece(char c, bool isWhite) 
					: this(c, isWhite, false) {
				}

				public void Take() {
					_isTaken = true;
				}

				public void UnTake() {
					_isTaken = false;
				}

				internal void Move(ChessMove m) {
					base.Move(m.AddToCoordinate(coordinates));
				}

				internal void UndoMove(ChessMove m) {
					base.UnMove(
						new ChessMove(-m.xDisplacement, -m.yDisplacement, false).AddToCoordinate(coordinates));
				}

				public override void Place(Coordinate c) {
					base.Place(c);
					_isTaken = false;
				}

				public bool isWhite {
					get {
						return _isWhite;
					}
				}

				public bool isTaken {
					get {
						return _isTaken;
					}
				}

				public bool isExtra {
					get {
						return _isExtra;
					}
					set {
						_isExtra = value;
					}
				}

				internal List<ChessMove> chessMoves {
					get {
						return _moves.Cast<ChessMove>().ToList();
					}
				}

				public ChessCoordinate chessCoordinates {
					get {
						return ChessCoordinate.FromCoordinate(coordinates);
					}
				}
			}
		}
	}
}
