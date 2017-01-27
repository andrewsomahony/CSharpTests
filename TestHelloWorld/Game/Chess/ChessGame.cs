using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class InvalidChessMoveException : Exception {
				public InvalidChessMoveException(string message) : base(message) {
					
				}
			}

			public class NoMoveToUndoException : Exception {
				public NoMoveToUndoException() : base("No move to undo!") {
					
				}
			}

			public class NoMoveToGoBackToException : Exception {
				public NoMoveToGoBackToException() : base("No move to go back to!") {
					
				}
			}

			class AmbiguousChessMoveException : Exception {
				public AmbiguousChessMoveException(string move) :
				base("One or more pieces apply to this move! " + move) {
					
				}
			}

			class NoValidPieceForChessMoveException : Exception {
				public NoValidPieceForChessMoveException(string move) :
				base("No valid piece for this move! " + move) {
					
				}
			}

			public partial class ChessGame : BoardGame {
				protected class SavedChessMove {
					private readonly ChessMove _move;
					private readonly ChessPiece _piece;

					public SavedChessMove(ChessMove move, ChessPiece piece) {
						_move = move;
						_piece = piece;
					}

					public ChessMove move {
						get {
							return _move;
						}
					}

					public ChessPiece piece {
						get {
							return _piece;
						}
					}
				}

				protected ChessPiece _pieceToBePromoted;

				protected bool _isAStalemate;

				protected List<string> _moveList;

				protected Stack<ChessPiece> _piecesTakenOrRemoved;
				protected Stack<SavedChessMove> _savedMoves;

				public ChessGame() : base(2) {
					_board = new ChessBoard();

					_moveList = new List<string>();
					_piecesTakenOrRemoved = new Stack<ChessPiece>();
					_savedMoves = new Stack<SavedChessMove>();

					// The pieces are a property of the game

					for (int i = 0; i < 2; i++) {
						bool isWhite = 0 == i % 2 ? true : false;

						_pieces.Add(CreateNewPiece('K', isWhite));
						_pieces.Add(CreateNewPiece('Q', isWhite));

						_pieces.Add(CreateNewPiece('B', isWhite));
						_pieces.Add(CreateNewPiece('B', isWhite));

						_pieces.Add(CreateNewPiece('N', isWhite));
						_pieces.Add(CreateNewPiece('N', isWhite));

						_pieces.Add(CreateNewPiece('R', isWhite));
						_pieces.Add(CreateNewPiece('R', isWhite));

						for (int j = 0; j < 8; j++) {
							_pieces.Add(CreateNewPiece('P', isWhite));
						}
					}

					_rules.Add(new NoPieceInTheWayRule());
					_rules.Add(new CaptureRule());
					_rules.Add(new EnPassantRule());
					_rules.Add(new PawnPromotionRule());
					_rules.Add(new CastlingRule());
				}

				public List<ChessPiece> chessPieces {
					get {
						return _pieces.Cast<ChessPiece>().ToList();
					}
				}

				public ChessBoard chessBoard {
					get {
						return _board as ChessBoard;
					}
				}

				public ChessPlayer whitePlayer {
					get {
						return _players.Cast<ChessPlayer>().ToList().Find(p => true == p.isWhite);
					}
				}

				public ChessPlayer blackPlayer {
					get {
						return _players.Cast<ChessPlayer>().ToList().Find(p => false == p.isWhite);
					}
				}

				public override string name {
					get {
						return "Chess";
					}
				}

				protected void SwapCurrentPlayer() {
					if (null != _currentPlayer) {
						_currentPlayer = opposingPlayer;
					}
				}

				protected override void EndTurn() {
					if (null != _currentPlayer) {
						// We check for stalemate here as
						// if we can swap players (if _currentPlayer isn't null),
						// then we know a move has been made.
						CheckStalemate(opposingPlayer);
					}
					// Swapping and checking for stalemate are not able to be done together,
					// as when we are going backwards, there is no point to check for stalemate,
					// plus, if we are going backwards from a checkmate position, the game will
					// incorrectly classify the game as stalemate.

					SwapCurrentPlayer();
				}

				public override void DoneWithInput() {
					EndTurn();
					_pieceToBePromoted = null;

					base.DoneWithInput();
				}

				public void OfferDraw() {
					RequireInput();
				}

				public ChessPlayer OpposingPlayer(ChessPlayer p) {
					if (true == p.isWhite) {
						return blackPlayer;
					} else {
						return whitePlayer;
					}
				}

				public ChessPlayer opposingPlayer {
					get {
						return OpposingPlayer(currentChessPlayer);
					}
				}

				public ChessPlayer currentChessPlayer {
					get {
						return _currentPlayer as ChessPlayer;
					}
				}

				public override void NewGame() {
					base.NewGame();

					AddPlayer(new ChessPlayer("White", true));
					AddPlayer(new ChessPlayer("Black", false));
				}

				protected virtual void InitializeCurrentPlayer() {
					_currentPlayer = whitePlayer;
				}

				public sealed override void Begin() {
					base.Begin();

					Reset();

					SetupBoard();

					DoneWithInput();
					InitializeCurrentPlayer();
				}

				protected override void Reset() {
					_isAStalemate = false;
					Clear();
				}

				public override void Clear() {
					// Remove all the promoted pieces.
					// We want to clear the game to its default state,
					// as if people are just sitting down to play it
					// and are ready to place the pieces on the board.
					// (But haven't placed them yet)

					_pieces.RemoveAll(p => true == (p as ChessPiece).isExtra);
					_pieces.ForEach(delegate(Piece p) {
						p.UnPlace();
					});

					_moveList.Clear();

					_piecesTakenOrRemoved.Clear();
					_savedMoves.Clear();
					_board.Clear();
				}

				public void TakePiece(ChessPiece piece) {
					chessBoard.TakePiece(piece);
					_piecesTakenOrRemoved.Push(piece);
				}

				public void UnTakePiece(ChessPiece piece) {
					chessBoard.UnTakePiece(piece);
				}

				public ChessPiece PopLatestRemovedPiece() {
					return _piecesTakenOrRemoved.Pop();
				}

				private void MovePiece(ChessPiece piece, ChessMove move) {
					piece.Move(move);
					_savedMoves.Push(new SavedChessMove(move, piece));
				}

				private void AddMoveString(string moveString) {
					_moveList.Add(moveString);
				}

				public void GoBackOneMove() {
					try {
						SavedChessMove savedMove = _savedMoves.Pop();

						// The endgame doesn't swap the current player,
						// as there's no point as the opposing player
						// cannot make a move.

						if (false == noMovePossibleAfterCurrentOne) {
							SwapCurrentPlayer();
						}

						// We have to run the rules backwards here,
						// to properly undo the various stacks

						for (int i = _rules.Count - 1; i >= 0; i--) {
							ChessRule rule = _rules[i] as ChessRule;
							rule.UndoMove(this, currentChessPlayer,
										  savedMove.move, savedMove.piece, latestAlgebraicMove);
						}

						savedMove.piece.UndoMove(savedMove.move);
					} catch (Exception) {
						throw new NoMoveToGoBackToException();
					}
				}

				public void UndoLatestMove() {
					try {
						GoBackOneMove();
						_moveList.Remove(_moveList[_moveList.Count - 1]);
					} catch (NoMoveToGoBackToException) {
						throw new NoMoveToUndoException();
					}
				}

				// The public method is solely for making sure that a move is a move that
				// can be made, or with exceptions we can process.  Used in the rules for
				// check and checkmate.

				public void CheckRulesBeforeMove(ChessPlayer player, ChessMove move, ChessPiece piece,
												 bool checkIfOwnKingIsInCheck) {
					List<ChessRule> rulesThatApply = new List<ChessRule>();

					CheckRulesBeforeMove(player, move, piece, ref rulesThatApply,
										 checkIfOwnKingIsInCheck);
				}

				// This is used by the rules for Checkmate, to see if the King can move away,
				// and if other pieces can block the attacking piece.

				private void CheckRulesBeforeMove(ChessPlayer player, ChessMove move, ChessPiece piece,
												 ref List<ChessRule> rulesThatApply,
												 bool checkIfOwnKingIsInCheck) {
					foreach (ChessRule rule in _rules) {
						if (true == rule.Applies(this, player, move, piece)) {
							rule.CheckMove(this, player, move, piece);
							rulesThatApply.Add(rule);
						}
					}

					// This is a special case.  If a piece moves but the move results in 
					// their king being in check, then it's an invalid move.  The reason it's 
					// separate from the rule array is that we use this function to check
					// for checkmate and check itself, so it will be an infinite loop if
					// we don't specify whether we want to check it or not; as it will be in
					// every rules array.

					if (true == checkIfOwnKingIsInCheck) {
						new OwnKingInCheckRule().CheckMove(this, player, move, piece);
					}
				}

				private void ExecuteRulesAfterMove(List<ChessRule> rulesThatApply,
												  ChessPlayer player, ChessMove move, ChessPiece piece,
												  ref AlgebraicMove finalAlgebraicMove) {
					foreach (ChessRule rule in rulesThatApply) {
						rule.AfterMove(this, player, move, piece, ref finalAlgebraicMove);
					}
				}

				protected void GetPieceAndMoveForAlgebraicMove(AlgebraicMove algebraicMove,
				                                        out ChessPieceToChessMoveDictionary finalPiecesAndMoves) {

					finalPiecesAndMoves =
						new ChessPieceToChessMoveDictionary();

					if (true == algebraicMove.isKingsideCastle ||
						true == algebraicMove.isQueensideCastle) {
						finalPiecesAndMoves[PlayerPiecesOnBoard(currentChessPlayer).Find(p => 'K' == p.character)] = 
						true == algebraicMove.isKingsideCastle ? new ChessMove(2, 0, false)
														 : new ChessMove(-2, 0, false);
					} else {
						// Make sure we can move to this square
						chessBoard.ValidateChessCoordinates(algebraicMove.newCoordinates);

						List<ChessPiece> validPieces =
							PlayerPiecesOnBoard(currentChessPlayer).FindAll(
								piece => algebraicMove.piece == piece.character);

						if ("" != algebraicMove.pieceFile) {
							validPieces = validPieces.FindAll(
								p => algebraicMove.pieceFile == ChessCoordinate.FromCoordinate(p.coordinates).file);
						}

						if ("" != algebraicMove.pieceRank) {
							validPieces = validPieces.FindAll(
								p => Int32.Parse(algebraicMove.pieceRank) ==
									ChessCoordinate.FromCoordinate(p.coordinates).rank);
						}

						GetPiecesFromListThatCanLegallyMoveToCoordinate(algebraicMove.newCoordinates,
																		validPieces,
																		ref finalPiecesAndMoves);
					}					
				}

				public override void MakeMove(string move) {
					if (true == _requiresUserInput) {
						Debug.Assert(false, "Chess requires user input before another move can be made!");
					}

					try {
						AlgebraicMove algebraicMove = new AlgebraicMove(move);

						ChessMove finalMove = null;
						ChessPiece finalPiece = null;
						List<ChessRule> rulesThatApply = new List<ChessRule>();

						ChessPieceToChessMoveDictionary finalPiecesAndMoves;
						GetPieceAndMoveForAlgebraicMove(algebraicMove, out finalPiecesAndMoves);

						if (0 == finalPiecesAndMoves.Count) {
							throw new NoValidPieceForChessMoveException(algebraicMove.ToString());
						}

						// Check all the rules, if they pass,
						// then move the piece, and do the post-rules

						bool hasValidPiece = false;
						Exception mostRecentException = null;
						foreach (KeyValuePair<ChessPiece, ChessMove> kvp in finalPiecesAndMoves) {
							try {
								CheckRulesBeforeMove(currentChessPlayer, kvp.Value, kvp.Key,
													 ref rulesThatApply,
													 true);

								if (true == hasValidPiece) {
									throw new AmbiguousChessMoveException(algebraicMove.ToString());
								}

								finalPiece = kvp.Key;
								finalMove = kvp.Value;

								hasValidPiece = true;
							} catch (AmbiguousChessMoveException e) {
								throw e;
							} catch (Exception e) {
								// Some pieces' moves are illegal moves, while some are legal moves but
								// place the king in check.  We want to make sure that if there is an error,
								// it first tries to see if it was a legal move anyways, and the king is just in
								// check.

								if (null == mostRecentException ||
								    false == mostRecentException is OwnKingIsInCheckException) {
									mostRecentException = e;
								}
							}
						}

						if (false == hasValidPiece) {
							throw mostRecentException;
						} else {
							AlgebraicMove finalAlgebraicMove = algebraicMove; // Copies the struct into the new struct.

							MovePiece(finalPiece, finalMove);

							ExecuteRulesAfterMove(rulesThatApply, currentChessPlayer,
												  finalMove, finalPiece, ref finalAlgebraicMove);

							CheckCheckAndCheckmate(ref finalAlgebraicMove);

							AddMoveString(finalAlgebraicMove.ToString());

							if (true == finalAlgebraicMove.drawOffered &&
							    false == noMovePossibleAfterCurrentOne) {
								OfferDraw();
							}

							if (false == _requiresUserInput &&
								false == noMovePossibleAfterCurrentOne) {
								EndTurn();
							}
						}
					} catch (Exception e) {
						throw new InvalidChessMoveException(e.Message);
					}
				}

				protected override void SetupBoard() {
					// We're guaranteed to only have the default number
					// of copies for each piece when this function is called.

					chessBoard.PlacePiece(DefaultKing(whitePlayer),
										  ChessCoordinate.FromCoordinateString("e1"));
					chessBoard.PlacePiece(DefaultQueen(whitePlayer),
										  ChessCoordinate.FromCoordinateString("d1"));

					List<ChessPiece> whiteBishops = DefaultBishops(whitePlayer);

					chessBoard.PlacePiece(whiteBishops[0],
										  ChessCoordinate.FromCoordinateString("c1"));
					chessBoard.PlacePiece(whiteBishops[1],
										  ChessCoordinate.FromCoordinateString("f1"));

					List<ChessPiece> whiteKnights = DefaultKnights(whitePlayer);

					chessBoard.PlacePiece(whiteKnights[0],
										  ChessCoordinate.FromCoordinateString("b1"));
					chessBoard.PlacePiece(whiteKnights[1],
										  ChessCoordinate.FromCoordinateString("g1"));

					List<ChessPiece> whiteRooks = DefaultRooks(whitePlayer);

					chessBoard.PlacePiece(whiteRooks[0],
										  ChessCoordinate.FromCoordinateString("a1"));
					chessBoard.PlacePiece(whiteRooks[1],
										  ChessCoordinate.FromCoordinateString("h1"));

					List<ChessPiece> whitePawns = DefaultPawns(whitePlayer);

					for (int i = 0; i < 8; i++) {
						chessBoard.PlacePiece(whitePawns[i],
											  ChessCoordinate.FromCoordinateString("" + (char)('a' + i) + "2"));
					}

					chessBoard.PlacePiece(DefaultKing(blackPlayer),
										  ChessCoordinate.FromCoordinateString("e8"));
					chessBoard.PlacePiece(DefaultQueen(blackPlayer),
										  ChessCoordinate.FromCoordinateString("d8"));

					List<ChessPiece> blackBishops = DefaultBishops(blackPlayer);

					chessBoard.PlacePiece(blackBishops[0],
										  ChessCoordinate.FromCoordinateString("c8"));
					chessBoard.PlacePiece(blackBishops[1],
										  ChessCoordinate.FromCoordinateString("f8"));

					List<ChessPiece> blackKnights = DefaultKnights(blackPlayer);

					chessBoard.PlacePiece(blackKnights[0],
										  ChessCoordinate.FromCoordinateString("b8"));
					chessBoard.PlacePiece(blackKnights[1],
										  ChessCoordinate.FromCoordinateString("g8"));

					List<ChessPiece> blackRooks = DefaultRooks(blackPlayer);

					chessBoard.PlacePiece(blackRooks[0],
										  ChessCoordinate.FromCoordinateString("a8"));
					chessBoard.PlacePiece(blackRooks[1],
										  ChessCoordinate.FromCoordinateString("h8"));

					List<ChessPiece> blackPawns = DefaultPawns(blackPlayer);

					for (int i = 0; i < 8; i++) {
						chessBoard.PlacePiece(blackPawns[i],
											  ChessCoordinate.FromCoordinateString("" + (char)('a' + i) + "7"));
					}
				}

				public List<string> moveList {
					get {
						return _moveList;
					}
				}

				public string latestMoveString {
					get {
						if (_moveList.Count > 0) {
							return _moveList[_moveList.Count - 1];
						} else {
							return "";
						}
					}
				}

				private AlgebraicMove latestAlgebraicMove {
					get {
						return new AlgebraicMove(latestMoveString);
					}
				}

				// numChecks, isInCheckmate, latestPieceToMove, and drawOffered
				// need to be virtual because a game that's being replayed
				// might not be on "latestAlgebraicMove", as they're concerned
				// with whatever move has been replayed by the user, and the user
				// can go backwards and forwards.

				public virtual int numChecks {
					get {
						return latestAlgebraicMove.numChecks;
					}
				}

				public virtual bool isInCheckmate {
					get {
						return latestAlgebraicMove.resultsInCheckmate;
					}					
				}

				public virtual bool drawOffered {
					get {
						return latestAlgebraicMove.drawOffered;
					}
				}

				public virtual ChessPiece latestPieceToMove {
					get {
						if ("" == latestMoveString) {
							return null;
						} else {
							return
								chessBoard.GetPieceForChessCoordinate(
									(new AlgebraicMove(latestMoveString)).newCoordinates);
						}
					}
				}

				public virtual bool noMovePossibleAfterCurrentOne {
					get {
						return isAStalemate || isInCheckmate;
					}
				}

				public bool isAStalemate {
					get {
						return _isAStalemate;
					}
				}

				// These serve as overrides, incase this game is loaded
				// from a file that has a pre-set position.

				public virtual bool whiteCannotCastleKingside {
					get {
						return false;
					}
				}

				public virtual bool whiteCannotCastleQueenside {
					get {
						return false;
					}
				}

				public virtual bool blackCannotCastleKingside {
					get {
						return false;
					}
				}

				public virtual bool blackCannotCastleQueenside {
					get {
						return false;
					}
				}

				public string whitePlayerName {
					get {
						return whitePlayer.name;
					}
					set {
						whitePlayer.name = value;
					}
				}

				public string blackPlayerName {
					get {
						return blackPlayer.name;
					}
					set {
						blackPlayer.name = value;
					}
				}
			}
		}
	}
}
