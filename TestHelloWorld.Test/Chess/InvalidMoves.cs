using System;
using NUnit.Framework;

namespace TestHelloWorld.Test {
	using TestHelloWorld.Games.Chess;

	[TestFixture]
	public class InvalidMoves {
		[Test]
		public void CheckInvalidMoves() {
			try {
				AlgebraicMove move = new AlgebraicMove("uhiadsihuda");
				// Suppress the "variable not used" warning
				Assert.Fail(move.ToString());
			} catch (InvalidAlgebraicMoveStringException) {
				Assert.Pass();
			}

			try {
				AlgebraicMove move = new AlgebraicMove("1233");
				// Suppress the "variable not used" warning
				Assert.Fail(move.ToString());
			} catch (InvalidAlgebraicMoveStringException) {
				Assert.Pass();
			}

			try {
				AlgebraicMove move = new AlgebraicMove("c100");
				// Suppress the "variable not used" warning
				Assert.Fail(move.ToString());
			} catch (InvalidAlgebraicMoveStringException) {
				Assert.Pass();
			}
		}
	}
}
