using System;
namespace TestHelloWorld {
	public struct Coordinate {
		private readonly int _x;
		private readonly int _y;

		public Coordinate(int x, int y) {
			_x = x;
			_y = y;
		}

		public override string ToString() {
			return string.Format("x={0}, y={1}", x, y);
		}

		public override bool Equals(object obj) {
			if (false == (obj is Coordinate)) {
				return false;
			} else {
				Coordinate c = (Coordinate)obj;
				return (_x == c.x && _y == c.y);
			}
		}

		public override int GetHashCode() {
			return _x ^ _y;
		}

		public static bool operator ==(Coordinate c1, Coordinate c2) {
			return c1.Equals(c2);
		}

		public static bool operator !=(Coordinate c1, Coordinate c2) {
			return !c1.Equals(c2);
		}

		public static Coordinate operator -(Coordinate c1, Coordinate c2) {
			return new Coordinate(c1.x - c2.x, c1.y - c2.y);
		}

		public static Coordinate operator +(Coordinate c1, Coordinate c2) {
			return new Coordinate(c1.x + c2.x, c1.y + c2.y);
		}

		public int x { 
			get {
				return _x;
			}
		}

		public int y { 
			get {
				return _y;
			}
		}
	}
}
