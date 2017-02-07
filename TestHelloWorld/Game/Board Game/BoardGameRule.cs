using System;
namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			public abstract class BoardGameRule : Rule {
				public abstract bool Applies(BoardGame game, Player player, Move move, Piece piece);
				public abstract void CheckMove(BoardGame game, Player player, Move move, Piece piece);
				public abstract void AfterMove(BoardGame game, Player player, Move move, Piece piece);

				public abstract void UndoMove(BoardGame game, Player player, Move move, Piece piece);
			}
		}
	}
}
