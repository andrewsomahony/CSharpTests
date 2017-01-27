using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class NoPieceAtCoordinateException : Exception {
				public NoPieceAtCoordinateException(Coordinate c) 
					: base("There is no piece at this coordinate! " + c) {
					
				}
			}

			public partial class FileChessGame {
				private readonly List<string> _moveList;
				private readonly List<CommentWithMoveNumberEntry> _commentsWithMoveNumbers;
				private readonly List<RAVWithMoveNumberEntry> _ravsWithMoveNumbers;
				private readonly char?[,] _boardSetup;
				private bool _hasBoardSetup = false;

				public FileChessGame() {
					_moveList = new List<string>();
					_commentsWithMoveNumbers = new List<CommentWithMoveNumberEntry>();
					_ravsWithMoveNumbers = new List<RAVWithMoveNumberEntry>();
					_boardSetup = new char?[8, 8];
				}

				public FileChessGame(ChessGame g) : this() {
					
				}

				// y goes from the bottom of the board to the top
				// and x goes from the left of the board to the right.

				// Lowercase means it's for black, while uppercase is for white.

				public void PlacePiece(char piece, int x, int y) {
					_boardSetup[x, y] = piece;
					_hasBoardSetup = true;
				}

				private char? PieceForCoordinate(Coordinate c) {
					return _boardSetup[c.x - 1, c.y - 1];
				}

				public bool PieceIsWhiteForCoordinate(Coordinate c) {
					char? piece = PieceForCoordinate(c);

					if (true == piece.HasValue) {
						return Char.ToUpper(piece.Value) == piece.Value;
					} else {
						throw new NoPieceAtCoordinateException(c);
					}
				}

				public char? GetPieceForCoordinate(Coordinate c) {
					char? returnValue = PieceForCoordinate(c);

					// We don't want the outside world distinguishing between lower and upper
					// for black and white.  That's purely for this class, and the outside world
					// cna use PieceIsWhiteForCoordinate to figure out what color the piece is.

					if (true == returnValue.HasValue) {
						return Char.ToUpper(returnValue.Value);
					} else {
						return returnValue;
					}
				}

				public void AddMove(string move) {
					_moveList.Add(move);
				}

				public void PushComment(string comment) {
					AddComment(comment, _moveList.Count);
				}

				public void PushRAV(string rav) {
					AddRAV(rav, _moveList.Count);
				}

				public void AddComment(string comment, int moveNumber) {
					_commentsWithMoveNumbers.Add(new CommentWithMoveNumberEntry(comment, moveNumber));
				}

				public void AddRAV(string comment, int moveNumber) {
					_ravsWithMoveNumbers.Add(new RAVWithMoveNumberEntry(comment, moveNumber));
				}

				public string GetCommentForMoveNumber(int moveNumber) {
					foreach (CommentWithMoveNumberEntry c in _commentsWithMoveNumbers) {
						if (moveNumber == c.moveNumber) {
							return c.comment;
						}
					}
					return "";
				}

				public string GetRAVForMoveNumber(int moveNumber) {
					foreach (RAVWithMoveNumberEntry r in _ravsWithMoveNumbers) {
						if (moveNumber == r.moveNumber) {
							return r.rav;
						}
					}
					return "";					
				}

				public List<string> moveList {
					get {
						return _moveList;
					}
				} 

				public int numMoves {
					get {
						return moveList.Count;
					}
				}

				// Capital because lowercase "event" is a C#
				// reserved word.

				public string Event {
					get; set;
				}

				public string site {
					get; set;
				}

				public string whitePlayerName {
					get; set;
				}

				public string blackPlayerName {
					get; set;
				}

				public string result {
					get; set;
				}

				public bool whiteToMove {
					get; set;
				}

				public bool? whiteCanCastleKingside {
					get; set;
				}

				public bool? whiteCanCastleQueenside {
					get; set;
				}

				public bool? blackCanCastleKingside {
					get; set;
				}

				public bool? blackCanCastleQueenside {
					get; set;
				}

				public string date {
					get {
						string returnValue = "";

						if (null != day) {
							returnValue += day + "/";
						}
						if (null != month) {
							returnValue += month + "/";
						}
						if (null != year) {
							returnValue += year;
						}

						return returnValue;
					}
				}

				public int? day {
					get; set;
				}

				public int? month {
					get; set;
				}

				public int? year {
					get; set;
				}

				public bool hasBoardSetup {
					get {
						return _hasBoardSetup;
					}
				}
			}
		}
	}
}
