using System;
namespace TestHelloWorld {
	using Games.BoardGames.Chess;
	using Parsers;

	public class NewGameChessView : ChessView {
		public NewGameChessView() : base(null) {
		}

		public override string title {
			get {
				return "New Chess Game";
			}
		}

		public override void Init() {
			base.Init();

			_game = new ChessGame();
		}

		private void EndAndAskForNewGame() {
			_game.End();
			if (true == Confirm("New game?")) {
				_game.Begin();
			}
		}

		public override void Run() {
			Clear();
			DrawChessBoard();

			DefaultStringParser stringParser = new DefaultStringParser();

			_game.NewGame();

			if (!ReadAndParse("Enter white's name", stringParser)) {
				return;
			}
			_game.whitePlayerName = stringParser.value;

			if (!ReadAndParse("Enter black's name", stringParser)) {
				return;
			}
			_game.blackPlayerName = stringParser.value;

			_game.Begin();

			while (_game.isActive) {
				DrawInterface();

				while (true) {
					if (true == _game.noMovePossibleAfterCurrentOne) {
						EndAndAskForNewGame();
						break;
					} else {
						if (!ReadAndParse(_game.currentPlayer.name + "'s move", stringParser, true)) {
							_game.End();
							break;
						}

						try {
							if ("undo" == stringParser.value) {
								_game.UndoLatestMove();
							} else {
								_game.MakeMove(stringParser.value);

								if (true == _game.needsPiecePromotionInput) {
									while (true) {
										try {
											ReadAndParse("Enter piece to promote to (Q/R/N/B)", stringParser, false);
											_game.PromoteCurrentPlayerPiece(stringParser.value[0]);

											break;
										} catch (InvalidPieceForPromotionException e) {
											Console.WriteLine(e.Message);
										}
									}
									_game.DoneWithInput();
								} else if (true == _game.drawOffered) {
									if (true ==
										Confirm(_game.currentPlayer.name + " has offered a draw.  Accept?")) {
										EndAndAskForNewGame();
										break;
									} else {
										_game.DoneWithInput();
									}
								}
							}

							break;
						} catch (InvalidChessMoveException e) {
							Console.WriteLine(e.Message);
						} catch (NoMoveToUndoException e) {
							Console.WriteLine(e.Message);
						}
					}
				}
			}
			// We close here because when the player decides they're done with the game,
			// then this view needs to close (not prompt for new names), and the player can select
			// whether to start a new one, or replay an old one (or whatever else is on the base ChessView menu).

			Close();
		}
	}
}
