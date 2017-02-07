using NUnit.Framework;

namespace TestHelloWorld.Test {
	using TestHelloWorld.Games.BoardGames.Chess;

	[TestFixture]
	public class NonPawnMoves {
		[Test]
		public void CheckNonPawnMove() {
			AlgebraicMove move = new AlgebraicMove("Qc6");

			Assert.That('Q' == move.piece);
			Assert.That(6 == move.newCoordinates.rank);
			Assert.That("c" == move.newCoordinates.file);

			Assert.That("" == move.pieceFile);
			Assert.That("" == move.pieceRank);
			Assert.That(false == move.isCapture);
			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);
		}

		[Test]
		public void CheckNonPawnWithFileMove() {
			AlgebraicMove move = new AlgebraicMove("Nba5");

			Assert.That('N' == move.piece);
			Assert.That(5 == move.newCoordinates.rank);
			Assert.That("a" == move.newCoordinates.file);
			Assert.That("b" == move.pieceFile);

			Assert.That("" == move.pieceRank);
			Assert.That(false == move.isCapture);
			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);
		}

		[Test]
		public void CheckNonPawnWithRankMove() {
			AlgebraicMove move = new AlgebraicMove("R3c2");

			Assert.That('R' == move.piece);
			Assert.That(2 == move.newCoordinates.rank);
			Assert.That("c" == move.newCoordinates.file);
			Assert.That("3" == move.pieceRank);

			Assert.That("" == move.pieceFile);
			Assert.That(false == move.isCapture);
			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);			
		}

		[Test]
		public void CheckNonPawnWithRankAndFileMove() {
			AlgebraicMove move = new AlgebraicMove("Bd5e4");

			Assert.That('B' == move.piece);
			Assert.That(4 == move.newCoordinates.rank);
			Assert.That("e" == move.newCoordinates.file);
			Assert.That("5" == move.pieceRank);
			Assert.That("d" == move.pieceFile);

			Assert.That(false == move.isCapture);
			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);			
		}

		[Test]
		public void CheckNonPawnCaptureMove() {
			AlgebraicMove move = new AlgebraicMove("Rxa1");

			Assert.That('R' == move.piece);
			Assert.That(1 == move.newCoordinates.rank);
			Assert.That("a" == move.newCoordinates.file);
			Assert.That(true == move.isCapture);

			Assert.That("" == move.pieceFile);
			Assert.That("" == move.pieceRank);
			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);		
		}

		[Test]
		public void CheckNonPawnCaptureWithFileMove() {
			AlgebraicMove move = new AlgebraicMove("Nbxf7");

			Assert.That('N' == move.piece);
			Assert.That(7 == move.newCoordinates.rank);
			Assert.That("f" == move.newCoordinates.file);
			Assert.That("b" == move.pieceFile);
			Assert.That(true == move.isCapture);

			Assert.That("" == move.pieceRank);
			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);
		}

		[Test]
		public void CheckNonPawnCaptureWithRankMove() {
			AlgebraicMove move = new AlgebraicMove("R3xc2");

			Assert.That('R' == move.piece);
			Assert.That(2 == move.newCoordinates.rank);
			Assert.That("c" == move.newCoordinates.file);
			Assert.That("3" == move.pieceRank);
			Assert.That(true == move.isCapture);

			Assert.That("" == move.pieceFile);
			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);
		}

		[Test]
		public void CheckNonPawnCaptureWithRankAndFileMove() {
			AlgebraicMove move = new AlgebraicMove("Kh5xg6");

			Assert.That('K' == move.piece);
			Assert.That(6 == move.newCoordinates.rank);
			Assert.That("g" == move.newCoordinates.file);
			Assert.That("5" == move.pieceRank);
			Assert.That("h" == move.pieceFile);
			Assert.That(true == move.isCapture);

			Assert.That(false == move.isEnPassant);
			Assert.That(null == move.promotionPiece);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);
		}
	}
}
