﻿using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	using Games.BoardGames.Chess;
	using Parsers;

	public class PromptForOldGameChessView : OldGameChessView {
		public PromptForOldGameChessView(Dictionary<int, IStackView> options) : base(options) {
		}

		public PromptForOldGameChessView() : this(null) {
			
		}

		public override string title {
			get {
				return "Replay Old Game";
			}
		}

		public override void Init() {
			base.Init();

			_game = new IOChessGame();
		}

		public override void Run() {
			DefaultStringParser stringParser = new DefaultStringParser();

			Clear();
			DrawChessBoard();

			if (!ReadAndParse("Enter file path", stringParser)) {
				return;
			}

			try {
				ioChessGame.LoadPGN(stringParser.value);

				if (ioChessGame.numGamesLoaded > 1) {
					_parentViewStack.PushView(new SelectOldGameChessView(ioChessGame));
				} else {
					_parentViewStack.PushView(new ReplayOldGameChessView(ioChessGame, 0));
				}
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		}
	}
}
