using NUnit.Framework;

namespace TestHelloWorld.Test {
	using TestHelloWorld.Games.Chess;

	[TestFixture]
	public class PawnMoves {
		[Test]
		public void CheckPawnMove() {
			AlgebraicMove pawnMove = new AlgebraicMove("c4");

			Assert.That('P' == pawnMove.piece);
			Assert.That(4 == pawnMove.newCoordinates.rank);
			Assert.That("c" == pawnMove.newCoordinates.file);

			Assert.That("" == pawnMove.pieceFile);
			Assert.That("" == pawnMove.pieceRank);
			Assert.That(false == pawnMove.isCapture);
			Assert.That(false == pawnMove.isEnPassant);
			Assert.That(null == pawnMove.promotionPiece);

			Assert.That(false == pawnMove.isKingsideCastle);
			Assert.That(false == pawnMove.isQueensideCastle);
		}

		[Test]
		public void CheckPawnCaptureMove() {
			AlgebraicMove pawnCaptureMove = new AlgebraicMove("dxc5");

			Assert.That('P' == pawnCaptureMove.piece);
			Assert.That("d" == pawnCaptureMove.pieceFile);
			Assert.That(true == pawnCaptureMove.isCapture);
			Assert.That(5 == pawnCaptureMove.newCoordinates.rank);
			Assert.That("c" == pawnCaptureMove.newCoordinates.file);

			Assert.That("" == pawnCaptureMove.pieceRank);
			Assert.That(false == pawnCaptureMove.isEnPassant);
			Assert.That(null == pawnCaptureMove.promotionPiece);

			Assert.That(false == pawnCaptureMove.isKingsideCastle);
			Assert.That(false == pawnCaptureMove.isQueensideCastle);		
		}

		[Test]
		public void CheckPawnEnPassantMove() {
			AlgebraicMove pawnEnPassantMove = new AlgebraicMove("exf6e.p.");

			Assert.That('P' == pawnEnPassantMove.piece);
			Assert.That("e" == pawnEnPassantMove.pieceFile);
			Assert.That(true == pawnEnPassantMove.isCapture);
			Assert.That(6 == pawnEnPassantMove.newCoordinates.rank);
			Assert.That("f" == pawnEnPassantMove.newCoordinates.file);
			Assert.That(true == pawnEnPassantMove.isEnPassant);

			Assert.That("" == pawnEnPassantMove.pieceRank);
			Assert.That(null == pawnEnPassantMove.promotionPiece);

			Assert.That(false == pawnEnPassantMove.isKingsideCastle);
			Assert.That(false == pawnEnPassantMove.isQueensideCastle);	
		}

		[Test]
		public void CheckPawnPromotionMove() {
			AlgebraicMove pawnPromotionMove = new AlgebraicMove("f8=Q");

			Assert.That('P' == pawnPromotionMove.piece);
			Assert.That(8 == pawnPromotionMove.newCoordinates.rank);
			Assert.That("f" == pawnPromotionMove.newCoordinates.file);
			Assert.That('Q' == pawnPromotionMove.promotionPiece);

			Assert.That("" == pawnPromotionMove.pieceFile);
			Assert.That("" == pawnPromotionMove.pieceRank);
			Assert.That(false == pawnPromotionMove.isCapture);
			Assert.That(false == pawnPromotionMove.isEnPassant);

			Assert.That(false == pawnPromotionMove.isKingsideCastle);
			Assert.That(false == pawnPromotionMove.isQueensideCastle);	
		}

		[Test]
		public void CheckPawnCaptureAndPromotionMove() {
			AlgebraicMove move = new AlgebraicMove("fxd8=R");

			Assert.That('P' == move.piece);
			Assert.That(8 == move.newCoordinates.rank);
			Assert.That("d" == move.newCoordinates.file);
			Assert.That('R' == move.promotionPiece);
			Assert.That(true == move.isCapture);
			Assert.That("f" == move.pieceFile);

			Assert.That("" == move.pieceRank);
			Assert.That(false == move.isEnPassant);

			Assert.That(false == move.isKingsideCastle);
			Assert.That(false == move.isQueensideCastle);
		}
	}
}
