using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	using Games.BoardGames.Chess;
	using Parsers;

	public class OldGameChessView : ChessView {
		public OldGameChessView(Dictionary<int, IStackView> options) : base(options) {
		}

		public OldGameChessView() : this(null) {
			
		}

		protected IOChessGame ioChessGame {
			get {
				return (IOChessGame)_game;
			}
		}
	}
}
