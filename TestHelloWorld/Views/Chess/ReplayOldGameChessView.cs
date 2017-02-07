using System;
namespace TestHelloWorld {
	using Games.BoardGames.Chess;
	using Parsers;

	public class ReplayOldGameChessView : OldGameChessView {
		private readonly int _gameIndex;

		public ReplayOldGameChessView(IOChessGame game, int gameIndex) : base(null) {
			_game = game;
			_gameIndex = gameIndex;
		}

		private FileChessGame fileGame {
			get {
				return ioChessGame.loadedGames[_gameIndex];
			}
		}

		public override string title {
			get {
				return fileGame.Event;
			}
		}

		protected override string FormatMoveListString(string moveString, int index) {
			string returnString = base.FormatMoveListString(moveString, index);

			if (ioChessGame.currentMoveIndex == index) {
				return "[" + returnString + "]";
			} else {
				return returnString;
			}
		}

		protected override void DrawExtraStatus() {
			if ("" != ioChessGame.currentComment) {
				Console.WriteLine(ioChessGame.currentComment);
			}
		}

		public override void Run() {
			ioChessGame.SetGame(_gameIndex);

			_game.Begin();

			Clear();

			while (_game.isActive) {
				// numMovesVisible is always an even number
				if (ioChessGame.currentMoveIndex >= numMovesVisible) {
					if (0 == ioChessGame.currentMoveIndex % 2) {
						ScrollToMoveIndex(ioChessGame.currentMoveIndex - numMovesVisible + 1);
					}
				} else {
					ScrollToMoveIndex(0);
				}

				DrawInterface();

				while (true) {
					CertainCharacterParser certainCharacterParser = new CertainCharacterParser(false,
																							   new char[] { 'f', 'b' });

					try {
						if (!ReadAndParse("Enter command (f/b)", certainCharacterParser, true)) {
							_game.End();
							break;
						}

						if (true == certainCharacterParser.ValueIsCharacter('F')) {
							ioChessGame.GoForwardOneMove();
						} else if (true == certainCharacterParser.ValueIsCharacter('B')) {
							ioChessGame.GoBackwardOneMove();
						}
						break;
					} catch (Exception e) {
						Console.WriteLine(e.Message);
					}
				}
			}
		}
	}
}
