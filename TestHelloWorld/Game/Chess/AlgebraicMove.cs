using System;
using System.Text.RegularExpressions;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class InvalidAlgebraicMoveStringException : Exception {
				public InvalidAlgebraicMoveStringException(string moveString) 
					: base("Invalid algebraic move! " + moveString) {
					
				}
			}

			public struct AlgebraicMove {
				private bool _isKingsideCastle;
				private bool _isQueensideCastle;

				private char? _piece;

				private bool _isEnPassant;

				private string _pieceFile;
				private string _pieceRank;

				private bool _isCapture;

				private ChessCoordinate? _newCoordinates;

				private char? _promotionPiece;

				private bool _drawOffered;

				// These are set by the check/checkmate rules
				private int _numChecks;
				private bool _resultsInCheckmate;

				public AlgebraicMove(string moveString) {
					_isKingsideCastle = false;
					_isQueensideCastle = false;

					_drawOffered = false;

					_numChecks = 0;

					_resultsInCheckmate = false;

					_piece = null;

					_isCapture = false;
					_isEnPassant = false;

					_newCoordinates = null;

					_pieceFile = "";
					_pieceRank = "";

					_promotionPiece = null;

					if (moveString.Length > 0) {
						Regex kingCastleRegex = new Regex(@"^O-O$");
						Regex queenCastleRegex = new Regex(@"^O-O-O$");

						// We allow all pieces for promotion, as we
						// want the promotion rule to verify which pieces are and aren't allowed,
						// not the regex.

						// There is a special case where the rank of the pawn needs to be specified:
						// that's if there is a moved pawn in front of another pawn that has not moved yet.

						// Both pawns can advance to the square ahead of the front pawn, so there needs to be
						// a disambiguation made.  It is an illegal move for the back pawn to do this, but that's
						// handled in the rules.  The engine needs to know WHICH piece is moving before it decides
						// whether it can move.

						// !!! Do we need the piece_rank for the pawn?
						Regex pawnMoveAndPossibleCaptureRegex =
							new Regex(@"^((?<piece_file>[a-wy-z])(?<is_capture>x)?)?(?<new_file>[a-wy-z])(?<new_rank>[0-9])(?<is_en_passant>e.p.)?(=(?<promoted_piece>[KPQBNR]))?(?<checks>[\+]+)?(?<has_draw_offer>=)?(?<is_checkmate>#)?$");
						Regex nonPawnMoveAndPossibleCaptureRegex =
							new Regex(@"^(?<piece>[KQBNR])(?<piece_file>[a-wy-z])?(?<piece_rank>[0-9])?(?<is_capture>x)?(?<new_file>[a-wy-z])(?<new_rank>[0-9])(?<checks>[\+]+)?(?<has_draw_offer>=)?(?<is_checkmate>#)?$");

						MatchCollection kingCastle = kingCastleRegex.Matches(moveString);

						// We don't set much for the castling, as we have no idea
						// of the layout of the board, how big it is, etc., so we can't
						// tell what the corner coordinates are.  We just let the Game engine do that.

						if (kingCastle.Count >= 1) {
							_isKingsideCastle = true;
							_piece = 'K';
						} else {
							MatchCollection queenCastle = queenCastleRegex.Matches(moveString);
							if (queenCastle.Count >= 1) {
								_isQueensideCastle = true;
								_piece = 'K';
							} else {
								MatchCollection pawn = pawnMoveAndPossibleCaptureRegex.Matches(moveString);
								if (pawn.Count >= 1) {
									GroupCollection groups = pawn[0].Groups;

									_piece = 'P';

									_pieceFile = groups["piece_file"].Value;
									_pieceRank = groups["piece_rank"].Value;

									_isCapture = groups["is_capture"].Success || groups["is_en_passant"].Success ||
									                                 groups["piece_file"].Success;

									_newCoordinates = new ChessCoordinate(groups["new_rank"].Value,
																			  groups["new_file"].Value);

									_isEnPassant = groups["is_en_passant"].Success;

									if (true == groups["promoted_piece"].Success) {
										_promotionPiece = groups["promoted_piece"].Value[0];
									}

									if (true == groups["checks"].Success) {
										_numChecks = groups["checks"].Length;
									}

									_drawOffered = groups["has_draw_offer"].Success;

									_resultsInCheckmate = groups["is_checkmate"].Success;

								} else {
									MatchCollection nonPawn = nonPawnMoveAndPossibleCaptureRegex.Matches(moveString);
									if (nonPawn.Count >= 1) {
										GroupCollection groups = nonPawn[0].Groups;

										_piece = groups["piece"].Value[0];

										_pieceFile = groups["piece_file"].Value;
										_pieceRank = groups["piece_rank"].Value;

										_isCapture = groups["is_capture"].Success;

										_newCoordinates = new ChessCoordinate(groups["new_rank"].Value,
																			  groups["new_file"].Value);

										if (true == groups["checks"].Success) {
											_numChecks = groups["checks"].Length;
										}

										_drawOffered = groups["has_draw_offer"].Success;

										_resultsInCheckmate = groups["is_checkmate"].Success;								
									} else {
										throw new InvalidAlgebraicMoveStringException(moveString);
									}
								}
							}
						}
					}
				}

				public override string ToString() {
					if (true == _isKingsideCastle) {
						return "O-O";
					} else if (true == _isQueensideCastle) {
						return "O-O-O";
					} else {
						string returnString = "";

						if ('P' == _piece) {
							if ("" != _pieceRank) {
								returnString += _pieceRank;
							}

							if ("" != _pieceFile) {
								returnString += _pieceFile;
							}

							if (true == _isCapture ||
							    true == _isEnPassant) {
								returnString += "x";
							}

							returnString += _newCoordinates.Value.file;
							returnString += _newCoordinates.Value.rank.ToString();

							if (true == _promotionPiece.HasValue) {
								returnString += "=" + promotionPiece.Value;
							}

							if (true == _isEnPassant) {
								returnString += "e.p.";
							}
						} else {
							returnString += _piece;
							if ("" != _pieceFile) {
								returnString += _pieceFile;
							}
							if ("" != _pieceRank) {
								returnString += _pieceRank;
							}
							if (true == _isCapture) {
								returnString += "x";
							}
							returnString += _newCoordinates.Value.file;
							returnString += _newCoordinates.Value.rank.ToString();
						}

						if (true == _resultsInCheckmate) {
							returnString += "#";
						} else {
							for (int i = 0; i < numChecks; i++) {
								returnString += "+";
							}
							if (true == _drawOffered) {
								returnString += "=";
							}
						}

						return returnString;
					}
				}

				public void Check() {
					_numChecks++;
				}

				public void ClearChecks() {
					_numChecks = 0;
				}

				public override bool Equals(object obj) {
					if (false == (obj is AlgebraicMove)) {
						return false;
					} else {
						// We check everything but En-Passant,
						// as that's an optional notation in the move string.
						// We put it in because it looks cool and shows that
						// we actually know the rule, which is the least-known
						// and understood (and newest) rule in Chess.

						AlgebraicMove move = (AlgebraicMove)obj;

						return _isKingsideCastle == move.isKingsideCastle &&
							   _isQueensideCastle == move.isQueensideCastle &&
							   _drawOffered == move.drawOffered &&
							   _numChecks == move.numChecks &&
							   _resultsInCheckmate == move.resultsInCheckmate &&
							   _piece == move.piece &&
							   _isCapture == move.isCapture &&
							   newCoordinates == move.newCoordinates &&
							   _pieceFile == move.pieceFile &&
							   _pieceRank == move.pieceRank &&
							   _promotionPiece == move.promotionPiece;					
					}
				}

				public override int GetHashCode() {
					if (null == _piece ||
					    false == _newCoordinates.HasValue) {
						return -1;
					} else {
						return (int)_piece ^ newCoordinates.GetHashCode();
					}
				}

				public static bool operator ==(AlgebraicMove a1, AlgebraicMove a2) {
					return a1.Equals(a2);
				}

				public static bool operator !=(AlgebraicMove a1, AlgebraicMove a2) {
					return !a1.Equals(a2);
				}

				public int numChecks {
					get {
						return _numChecks;
					}
				}

				public bool hasCheck {
					get {
						return numChecks > 0;
					}
				}

				public bool isKingsideCastle {
					get {
						return _isKingsideCastle;
					}
					set {
						_isKingsideCastle = value;
					}
				}

				public bool isQueensideCastle {
					get {
						return _isQueensideCastle;
					}
					set {
						_isQueensideCastle = value;
					}
				}

				public char? piece {
					get {
						return _piece;
					}
				}

				public bool isEnPassant {
					get {
						return _isEnPassant;
					}
					set {
						_isEnPassant = value;
					}
				}

				public string pieceFile {
					get {
						return _pieceFile;
					}
				}

				public string pieceRank {
					get {
						return _pieceRank;
					}
				}

				public bool isCapture {
					get {
						return _isCapture;
					}
					set {
						_isCapture = value;
					}
				}

				public bool isPawnCapture {
					get {
						return "" != pieceFile && 
							'P' == piece; 
					}
				}

				public bool resultsInCheckmate {
					get {
						return _resultsInCheckmate;
					}
					set {
						_resultsInCheckmate = value;
					}
				}

				public bool drawOffered {
					get {
						return _drawOffered;
					}
					set {
						_drawOffered = value;
					}
				}

				public ChessCoordinate newCoordinates {
					get {
						// !!! Should I do some sort of check here?
						if (false == _newCoordinates.HasValue) {
							return new ChessCoordinate();
						} else {
							return _newCoordinates.Value;
						}
					}
					set {
						_newCoordinates = value;
					}
				}

				public char? promotionPiece {
					get {
						return _promotionPiece;
					}
					set {
						_promotionPiece = value;
					}
				}

				public bool hasPromotion {
					get {
						return null != promotionPiece;
					}
				}
			}
		}
	}

}
