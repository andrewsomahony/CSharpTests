using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public abstract class ChessRule : Rule {
				public ChessRule() {
				}

				public abstract bool Applies(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece);
				public abstract void CheckMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece);
				public abstract void AfterMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
											   ref AlgebraicMove finalMove);

				public virtual void UndoMove(ChessGame game, ChessPlayer player, ChessMove move, ChessPiece piece,
											 AlgebraicMove moveToUndo) {
					
				}

				public override bool Applies(Game game, Player player, Move move, Piece piece) {
					return this.Applies(game as ChessGame, 
					                    player as ChessPlayer, 
					                    move as ChessMove, 
					                    piece as ChessPiece);
				}
				public override void CheckMove(Game game, Player player, Move move, Piece piece) {
					this.CheckMove(game as ChessGame,
								   player as ChessPlayer,
								   move as ChessMove,
								   piece as ChessPiece
								  );
				}
				public override void AfterMove(Game game, Player player, Move move, Piece piece) {
					AlgebraicMove tempAlgebraicMove = new AlgebraicMove();

					this.AfterMove(game as ChessGame,
								   player as ChessPlayer,
								   move as ChessMove,
								   piece as ChessPiece,
								   ref tempAlgebraicMove
								  );					
				}

				public override void UndoMove(Game game, Player player, Move move, Piece piece) {
					this.UndoMove(game as ChessGame, 
					              player as ChessPlayer, 
					              move as ChessMove, 
					              piece as ChessPiece,
								  new AlgebraicMove());
				}
			}
		}
	}
}
