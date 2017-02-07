using NUnit.Framework;

namespace TestHelloWorld.Test {
	using TestHelloWorld.Games.BoardGames.Chess;

	[TestFixture]
	public class AlgebraicMoveStrings {
		[Test]
		public void TestAlgebraicMoveStrings() {
			string[] stringsToTest = new string[] {
				"c4",
				"dxc4",
				"c8=Q",
				"axb8=B",
				"bxa4e.p.",
				"Be4",
				"Qxd7",
				"Kxg7++",
				"Rg5#",
				"O-O",
				"O-O-O"
			};

			foreach (string testString in stringsToTest) {
				Assert.That(testString == new AlgebraicMove(testString).ToString());
			}
		}
	}
}
