using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	using Games.Chess;
	using Parsers;

	public class SelectOldGameChessView : OldGameChessView {
		public SelectOldGameChessView(IOChessGame _game) : base(_game.loadedGames.Select((game, index) => 
		                                                                   			new { Item = game, Index = index })
															   .ToDictionary(x => x.Index + 1,
		                                          	x => new ReplayOldGameChessView(_game, x.Index) as View)) {
		}

		public override string title {
			get {
				return "Select Game";
			}
		}
	}
}
