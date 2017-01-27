using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		public abstract class BoardGame : Game {
			protected Board _board;

			public Player _currentPlayer = null;

			internal List<Piece> _pieces;

			public BoardGame(int maxNumPlayers) : base(maxNumPlayers) {
				_pieces = new List<Piece>();
			}

			protected abstract void SetupBoard();

			public abstract void MakeMove(string move);

			public override void Begin() {
				base.Begin();
				_currentPlayer = null;
			}

			protected virtual void EndTurn() {
				
			}

			public Board board {
				get {
					return _board;
				}
			}

			public Player currentPlayer {
				get {
					return _currentPlayer;
				}
			}
		}
	}
}
