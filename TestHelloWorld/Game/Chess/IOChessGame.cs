using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			using ChessFileIO;

			public class InvalidMoveIndexException : Exception {
				public InvalidMoveIndexException(int index) : base("Invalid move index! " + index) {
					
				}
			}

			public class CannotCreateNewPieceException : Exception {
				public CannotCreateNewPieceException(char piece) : base("Cannot create new piece! " + piece) {
					
				}
			}

			public class UnknownPieceCharacterException : Exception {
				public UnknownPieceCharacterException(char c) : base("Unknown piece character! " + c) {
					
				}
			}

			public class NoMoveToGoForwardToException : Exception {
				public NoMoveToGoForwardToException() : base("No move to go forward to!") {

				}
			}

			public class MoveDoesntMatchEngineMoveException : Exception {
				public MoveDoesntMatchEngineMoveException(string move, string engineMove) 
					: base("Move doesn't match engine move! (" + move + " != " + engineMove + ")") {
					
				}	
			}

			public class CannotReplayChessMoveException : Exception {
				private string _move;
				private int _moveNumber;
				private string _message;

				public CannotReplayChessMoveException(string message, string move, int moveNumber) {
					_message = message;
					_move = move;
					_moveNumber = moveNumber;
				}

				private string FormatMoveAndMoveNumber() {
					string returnString = "";

					returnString += ((_moveNumber + 1) / 2) + ". ";

					if (0 == _moveNumber % 2) {
						returnString += "... ";	
					}

					returnString += _move;

					return returnString;
				}

				public override string ToString() {
					return "" + _message + " (" + FormatMoveAndMoveNumber() + ")";
				}
			}

			public class IOChessGame : ChessGame {
				private ChessFile _chessFile;
				private FileChessGame _currentGame;

				private int _currentMoveIndex;

				public IOChessGame() : base() {

				}

				public override void RequireInput() {
					// If we aren't replaying a game, then
					// we require input, but otherwise, user input
					// is never needed for a replayed game.

					if (false == isReplayingGame) {
						base.RequireInput();
					}
				}

				// Number starts with 0
				public void SetGame(int index) {
					_currentGame = _chessFile[index];

					// Now we run the game and un-run it, to make
					// sure that the move list is ok, and that the game
					// retains a consistent state.

					// The Asserts are there because if there is a problem with
					// the undo engine, it's a programmer error.

					int currentMoveNumber = 1;
					string currentMove = "";
					try {
						// Clear everything up as we're starting fresh.

						Clear();

						NewGame();

						whitePlayerName = _currentGame.whitePlayerName;
						blackPlayerName = _currentGame.blackPlayerName;

						Begin();

						try {
							foreach (string s in _currentGame.moveList) {
								currentMove = s;
								MakeMove(s);

								_currentMoveIndex++;
								currentMoveNumber++;

								// We want to make sure the moves we get are the same
								// as the ones coming in.
								if (new AlgebraicMove(s) != currentExecutedAlgebraicMove) {
									throw new MoveDoesntMatchEngineMoveException(s, currentExecutedMoveString);
								}

							}
						} catch (InvalidMoveIndexException) {
							// We can just suppress this one, as we're always
							// guaranteed a valid move index.
						} catch (MoveDoesntMatchEngineMoveException e) {
							throw e;
						} catch (Exception e) {
							throw new CannotReplayChessMoveException(
								e.Message, currentMove, currentMoveNumber);
						}

						List<string> backupMoveList = new List<string>(_moveList);

						while (_moveList.Count > 0) {
							try {
								_currentMoveIndex--;
								UndoLatestMove();
							} catch (NoMoveToUndoException) {
								break;
							}
						}

						Debug.Assert(0 == _moveList.Count);
						Debug.Assert(0 == _piecesTakenOrRemoved.Count);
						Debug.Assert(0 == _savedMoves.Count);

						_moveList.AddRange(backupMoveList);

						// Reset so the game state is where we started at, except with a valid move list.
						Reset();
					} catch (Exception e) {
						End();
						throw e;
					}
				}

				public void LoadPGN(string filename) {
					ChessFileLoader loader = new ChessFileLoader();

					try {
						_chessFile = loader.LoadFile(filename);
					} catch (Exception e) {
						throw e;
					}

					_currentGame = null;
				}

				public void SavePGN(string filename) {
					
				}

				protected override void InitializeCurrentPlayer() {
					if (true == isReplayingGame) {
						base.InitializeCurrentPlayer();
					} else {
						// The loaded game may start with black making the first move,
						// so we need to figure out how to read that from the current game.
						_currentPlayer = _currentGame.whiteToMove ? whitePlayer : blackPlayer;

						CheckStalemate();
					}
				}

				protected override void SetupBoard() {
					if (true == isReplayingGame &&
					    true == _currentGame.hasBoardSetup) {
						// We are given the pieces in the initial array to work with,
						// so any extra pieces we find in the board setup (promoted pawns),
						// we just make new ones, and will remove them when the game resets,
						// and then this function is called again to create them again, etc. etc.

						for (int rank = 1; rank <= 8; rank++) {
							for (int file = 1; file <= 8; file++) {
								ChessCoordinate currentCoordinate = new ChessCoordinate(rank, file);

								char? piece = _currentGame.GetPieceForCoordinate(
									currentCoordinate.ToCoordinate());

								if (true == piece.HasValue) {
									bool isWhite = _currentGame.PieceIsWhiteForCoordinate(
										currentCoordinate.ToCoordinate());

									ChessPlayer playerForCurrentPiece = true == isWhite ? whitePlayer : blackPlayer;
									char pieceCharacter = piece.Value;

									// If this piece is a piece that can result from a pawn promotion, we just
									// need to see if the pieces that the player starts with have already been placed.

									// If they have, we create a new piece.

									List<ChessPiece> defaultPieces;
									switch (pieceCharacter) {
										case 'Q':
											defaultPieces = new List<ChessPiece>() {
												DefaultQueen(playerForCurrentPiece)
											};
											break;
										case 'N':
											defaultPieces = DefaultKnights(playerForCurrentPiece);
											break;
										case 'B':
											defaultPieces = DefaultBishops(playerForCurrentPiece);
											break;
										case 'R':
											defaultPieces = DefaultRooks(playerForCurrentPiece);
											break;
										case 'K':
											defaultPieces = new List<ChessPiece>() {
												DefaultKing(playerForCurrentPiece)
											};
											break;
										case 'P':
											defaultPieces = DefaultPawns(playerForCurrentPiece);
											break;
										default:
											throw new UnknownPieceCharacterException(pieceCharacter);
									}

									List<ChessPiece> defaultUnplacedPieces = defaultPieces.FindAll(
										p => false == p.isPlaced);

									if (defaultUnplacedPieces.Count > 0) {
										chessBoard.PlacePiece(defaultUnplacedPieces[0], currentCoordinate);
									} else {
										// We cannot clone pawns and kings
										if ('P' == pieceCharacter ||
											'K' == pieceCharacter) {
											throw new CannotCreateNewPieceException(pieceCharacter);
										}
										chessBoard.PlacePiece(CreateNewPiece(pieceCharacter, isWhite),
															  currentCoordinate);
									}
								}
							}
						}
					} else {
						base.SetupBoard();
					}
				}

				private void SetCurrentMoveIndex(int index) {
					if (_currentGame.numMoves > 0) {
						// So we want to allow the index to be == _currentGame.numMoves
						// because we have getters (currentlyExecutedMove) that use the move
						// of currentMoveIndex - 1 for checks.
						if (index < 0 ||
							index > _currentGame.numMoves) {
							throw new InvalidMoveIndexException(index);
						}
						_currentMoveIndex = index;
					}
				}

				private void ReplayMove() {
					// !!! This implementation is temporary.
					// We really don't have to do much here, as the engine has already checked
					// that all the moves we're replaying are valid, so we just need to move
					// the piece specified to the specified coordinate, and check for checkmate and
					// stalemate and such.

					MakeMove(currentMoveString);
					_moveList.RemoveAt(_moveList.Count - 1);

					SetCurrentMoveIndex(currentMoveIndex + 1);
				}

				public void GoForwardOneMove() {
					// When this method is called, all the moves in the array
					// are guaranteed to work with our implementation.

					// Therfore, the only exception that is thrown is if the move
					// string is invalid, meaning there is no move to advance to.

					try {
						ReplayMove();
					} catch (Exception) {
						throw new NoMoveToGoForwardToException();
					}
				}

				public void GoBackwardOneMove() {
					try {
						SetCurrentMoveIndex(currentMoveIndex - 1);
						GoBackOneMove();
					} catch (InvalidMoveIndexException) {
						throw new NoMoveToGoBackToException();
					}
				}

				protected override void Reset() {
					// Reset clears the move list, so we want
					// to just save it and restore it, to prevent code re-use

					List<string> tempMoveList = new List<string>(_moveList);

					base.Reset();

					_moveList.AddRange(tempMoveList);

					SetCurrentMoveIndex(0);
				}

				public int numGamesLoaded {
					get {
						return _chessFile.numGames;
					}
				}

				public bool isReplayingGame {
					get {
						return null != _currentGame;
					}
				}

				public List<FileChessGame> loadedGames {
					get {
						return _chessFile.loadedGames;
					}
				}

				public int currentMoveIndex {
					get {
						return _currentMoveIndex;
					}
				}

				private string currentMoveString {
					get {
						if (_currentGame.numMoves > 0) {
							return _moveList[currentMoveIndex];
						} else {
							return "";
						}
					}
				}

				private string currentExecutedMoveString {
					get {
						if (currentMoveIndex > 0) {
							return _moveList[currentMoveIndex - 1];
						} else {
							return "";
						}
					}
				}

				private AlgebraicMove currentExecutedAlgebraicMove {
					get {
						return new AlgebraicMove(currentExecutedMoveString);
					}
				}

				private AlgebraicMove currentAlgebraicMove {
					get {
						return new AlgebraicMove(currentMoveString);
					}
				}

				public string currentComment {
					get {
						if (true == isReplayingGame) {
							return _currentGame.GetCommentForMoveNumber(currentMoveIndex);
						} else {
							return "";
						}
					}
				}

				public string currentRAV {
					get {
						if (true == isReplayingGame) {
							return _currentGame.GetRAVForMoveNumber(currentMoveIndex);
						} else {
							return "";
						}
					}
				}

				public override bool noMovePossibleAfterCurrentOne {
					get {
						return currentMoveIndex == _currentGame.numMoves - 1 ||
							   currentAlgebraicMove.resultsInCheckmate;
					}
				}

				public override int numChecks {
					get {
						return currentExecutedAlgebraicMove.numChecks;
					}
				}

				public override bool isInCheckmate {
					get {
						return currentExecutedAlgebraicMove.resultsInCheckmate;
					}
				}

				public override bool drawOffered {
					get {
						return currentExecutedAlgebraicMove.drawOffered;
					}
				}

				public override ChessPiece latestPieceToMove {
					get {
						if ("" == currentExecutedMoveString) {
							return null;
						} else {
							return
								chessBoard.GetPieceForChessCoordinate(
									currentExecutedAlgebraicMove.newCoordinates);
						}
					}
				}

				public override bool whiteCannotCastleKingside {
					get {
						if (true == _currentGame.whiteCanCastleKingside.HasValue) {
							return _currentGame.whiteCanCastleKingside.Value;
						} else {
							return base.whiteCannotCastleKingside;
						}
					}
				}

				public override bool whiteCannotCastleQueenside {
					get {
						if (true == _currentGame.whiteCanCastleQueenside.HasValue) {
							return _currentGame.whiteCanCastleQueenside.Value;
						} else {
							return base.whiteCannotCastleQueenside;
						}
					}
				}

				public override bool blackCannotCastleKingside {
					get {
						if (true == _currentGame.blackCanCastleKingside.HasValue) {
							return _currentGame.blackCanCastleKingside.Value;
						} else {
							return base.blackCannotCastleKingside;
						}
					}
				}

				public override bool blackCannotCastleQueenside {
					get {
						if (true == _currentGame.blackCanCastleQueenside.HasValue) {
							return _currentGame.blackCanCastleQueenside.Value;
						} else {
							return base.blackCannotCastleQueenside;
						}
					}
				}
			}
		}
	}
}
