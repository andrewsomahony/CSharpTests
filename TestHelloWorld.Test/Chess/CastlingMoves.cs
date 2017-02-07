using NUnit.Framework;

namespace TestHelloWorld.Test {
	using TestHelloWorld.Games.BoardGames.Chess;

	[TestFixture]
	public class CastlingMoves {
		[Test]
		public void CheckKingsideCastleMove() {
			AlgebraicMove move = new AlgebraicMove("O-O");

			Assert.That('K' == move.piece);
			Assert.That(true == move.isKingsideCastle);
		}

		[Test]
		public void CheckQueensideCastleMove() {
			AlgebraicMove move = new AlgebraicMove("O-O-O");

			Assert.That('K' == move.piece);
			Assert.That(true == move.isQueensideCastle);			
		}
	}
}
