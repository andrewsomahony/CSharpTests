using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		public abstract class CoordinateOutOfRangeException : Exception {
			public CoordinateOutOfRangeException(int coordinate, int max, string name) 
				: base(name + " is out of range; " + coordinate + " (1, " + max + ")") {
			}
		}

		public class XCoordinateOutOfRangeException : CoordinateOutOfRangeException {
			public XCoordinateOutOfRangeException(int x, int max) : base(x, max, "X") {
				
			}
		}

		public class YCoordinateOutOfRangeException : CoordinateOutOfRangeException {
			public YCoordinateOutOfRangeException(int y, int max) : base(y, max, "Y") {

			}
		}

		public abstract class Board {
			private readonly int _width;
			private readonly int _height;

			protected List<Piece> _pieces;

			public Board(int width, int height) {
				_width = width;
				_height = height;
			}

			public virtual void Clear() {
				_pieces.Clear();
			}

			public void AddPiece(Piece p) {
				_pieces.Add(p);
			}

			public void RemovePiece(Piece p) {
				_pieces.Remove(p);
			}

			// !!! Too much coordinate checking here :-/

			public void ValidateCoordinates(Coordinate c) {
				if (c.x < 1 ||
					 c.x > _width) {
					throw new XCoordinateOutOfRangeException(c.x, _width);
				}
				if (c.y < 1 ||
					c.y > _height) {
					throw new YCoordinateOutOfRangeException(c.y, _height);
				}
			}

			public void PlacePiece(Piece p, Coordinate c) {
				ValidateCoordinates(c);

				p.Place(c);
				AddPiece(p);
			}

			public void MovePiece(Piece p, Coordinate c) {
				ValidateCoordinates(c);

				p.Move(c);
			}

			public Piece GetPieceForCoordinate(Coordinate c) {
				List<Piece> piecesForCoordinate = GetPiecesForCoordinate(c);
				if (piecesForCoordinate.Count > 0) {
					return piecesForCoordinate[0];
				} else {
					return null;
				}
			}

			public List<Piece> GetPiecesForCoordinate(Coordinate c) {
				List<Piece> piecesForCoordinate = new List<Piece>();

				foreach (Piece p in _pieces) {
					if (p.coordinates == c) {
						piecesForCoordinate.Add(p);
					}
				}
				return piecesForCoordinate;
			}

			public int width {
				get {
					return _width;
				}
			}

			public int height {
				get {
					return _height;
				}
			}
		}
	}
}
