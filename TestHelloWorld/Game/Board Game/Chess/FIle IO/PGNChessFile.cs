using System;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				namespace ChessFileIO {
					using ilf.pgn;
					using PGN = ilf.pgn.Data;

					public class InvalidPGNPieceException : Exception {
						public InvalidPGNPieceException(PGN.Piece p) : base("Invalid PGN piece! " + p.ToString()) {

						}
					}

					public class PGNChessFile : ChessFile {
						public override string name {
							get {
								return "PGN";
							}
						}

						public override bool CanLoad(string filename) {
							try {
								LoadGameDatabase(filename);
								return true;
							} catch (Exception) {
								return false;
							}
						}

						private PGN.Database LoadGameDatabase(string filename) {
							PgnReader reader = new PgnReader();
							return reader.ReadFromFile(filename);
						}

						private char ConvertBoardSetupPieceToChar(PGN.Piece piece) {
							char returnValue;

							switch (piece.PieceType) {
								case PGN.PieceType.Pawn:
									returnValue = 'p';
									break;
								case PGN.PieceType.Knight:
									returnValue = 'n';
									break;
								case PGN.PieceType.Bishop:
									returnValue = 'b';
									break;
								case PGN.PieceType.Rook:
									returnValue = 'r';
									break;
								case PGN.PieceType.Queen:
									returnValue = 'q';
									break;
								case PGN.PieceType.King:
									returnValue = 'k';
									break;
								default:
									throw new InvalidPGNPieceException(piece);
							}

							if (PGN.Color.White == piece.Color) {
								return Char.ToUpper(returnValue);
							}

							return returnValue;
						}

						private FileChessGame LoadGame(PGN.Game game) {
							FileChessGame returnGame = new FileChessGame();

							returnGame.Event = game.Event;

							returnGame.day = game.Day;
							returnGame.month = game.Month;
							returnGame.year = game.Year;

							returnGame.whitePlayerName = game.WhitePlayer;
							returnGame.blackPlayerName = game.BlackPlayer;

							returnGame.result = game.Result.ToString();

							if (null != game.BoardSetup &&
								game.BoardSetup.ToString().Length > 0) {
								for (int x = 0; x < 8; x++) {
									for (int y = 0; y < 8; y++) {
										if (null != game.BoardSetup[x, y]) {
											// The coordinates are inverted here, so y-coordinate 0 is actually
											// the black side of the board, proceeding downwards.

											returnGame.PlacePiece(
												ConvertBoardSetupPieceToChar(game.BoardSetup[x, y]),
																  x, 7 - y);
										}
									}
								}

								returnGame.whiteToMove = game.BoardSetup.IsWhiteMove;

								// Explicitly set these if they are here, otherwise leave them alone,
								// as they have no influence if they aren't specified in the file.

								returnGame.whiteCanCastleKingside = game.BoardSetup.CanWhiteCastleKingSide;
								returnGame.whiteCanCastleQueenside = game.BoardSetup.CanWhiteCastleQueenSide;

								returnGame.blackCanCastleKingside = game.BoardSetup.CanBlackCastleKingSide;
								returnGame.blackCanCastleQueenside = game.BoardSetup.CanBlackCastleQueenSide;

								// !!! TODO: Handle en-passant target square
							} else {
								returnGame.whiteToMove = true;
							}

							foreach (PGN.MoveTextEntry m in game.MoveText) {
								switch (m.Type) {
									case PGN.MoveTextEntryType.MovePair:
										PGN.MovePairEntry pair = (PGN.MovePairEntry)m;
										returnGame.AddMove(pair.White.ToString());
										returnGame.AddMove(pair.Black.ToString());
										break;
									case PGN.MoveTextEntryType.SingleMove:
										PGN.HalfMoveEntry half = (PGN.HalfMoveEntry)m;
										returnGame.AddMove(half.Move.ToString());
										break;
									case PGN.MoveTextEntryType.RecursiveAnnotationVariation:
										PGN.RAVEntry entry = (PGN.RAVEntry)m;
										returnGame.PushRAV(entry.MoveText.ToString());
										break;
									case PGN.MoveTextEntryType.Comment:
										returnGame.PushComment(m.ToString());
										break;
									default:
										break;
								}
							}

							return returnGame;
						}

						public override void Load(string filename) {
							PGN.Database db = LoadGameDatabase(filename);

							foreach (PGN.Game g in db.Games) {
								AddLoadedGame(LoadGame(g));
							}
						}

						public override void Save(string filename) {

						}
					}
				}
			}
		}
	}
}
