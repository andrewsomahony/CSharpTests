using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			public class ChessMove : Move {
				public ChessMove(int xd, int yd, bool isInfinite) :
				base(xd, yd, isInfinite) {
				}

				public static ChessMove FromOldAndNewCoordinates(ChessCoordinate newCoordinates, 
				                                                 ChessCoordinate oldCoordinates) {
					Coordinate c = newCoordinates.ToCoordinate() - oldCoordinates.ToCoordinate();

					return new ChessMove(c.x, c.y, false);
				}

				public ChessCoordinate AddToChessCoordinate(ChessCoordinate c) {
					return ChessCoordinate.FromCoordinate(this.AddToCoordinate(c.ToCoordinate()));
				}
			}
		}
	}
}
