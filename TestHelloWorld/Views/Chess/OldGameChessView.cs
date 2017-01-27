using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	using Games.Chess;
	using Parsers;

	public class OldGameChessView : ChessView {
		public OldGameChessView(Dictionary<int, View> options) : base(options) {
		}

		public OldGameChessView() : this(null) {
			
		}

		protected IOChessGame ioChessGame {
			get {
				return _game as IOChessGame;
			}
		}

		public override void Init() {
			base.Init();
		}
	}
}
