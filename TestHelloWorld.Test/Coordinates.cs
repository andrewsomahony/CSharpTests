using NUnit.Framework;

namespace TestHelloWorld.Test {
	[TestFixture]
	public class Coordinates {
		[Test]
		public void CheckCoordinateSubtraction() {
			Coordinate c1 = new Coordinate(1, 2);
			Coordinate c2 = new Coordinate(2, 1);

			Coordinate result = c1 - c2;
			Assert.That(-1 == result.x);
			Assert.That(1 == result.y);
		}

		[Test]
		public void CheckCoordinateAddition() {
			Coordinate c1 = new Coordinate(2, 3);
			Coordinate c2 = new Coordinate(4, 5);

			Coordinate result = c1 + c2;
			Assert.That(6 == result.x);
			Assert.That(8 == result.y);			
		}
	}
}
