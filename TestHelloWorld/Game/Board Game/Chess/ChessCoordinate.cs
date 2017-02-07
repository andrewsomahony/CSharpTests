using System;
using System.Text.RegularExpressions;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public class InvalidChessCoordinateStringException : Exception {
					public InvalidChessCoordinateStringException(string coordinateString) :
					base("Invalid Chess coordinate string! " + coordinateString) {

					}
				}

				public class InvalidChessCoordinateFileException : Exception {
					public InvalidChessCoordinateFileException(string file)
						: base("Invalid Chess Coordinate file! " + file) {

					}
				}

				public struct ChessCoordinate {
					private readonly int _rank; //y coordinate, between 1-8
					private readonly string _file; //x coordinate, between a-z

					public ChessCoordinate(string rank, string file) : this(Int32.Parse(rank), file) {
					}

					public ChessCoordinate(int rank, string file) {
						_rank = rank;
						_file = file.ToLower();

						if (_file[0] < 'a' ||
							_file[0] > 'z') {
							throw new InvalidChessCoordinateFileException(file);
						}
					}

					public ChessCoordinate(int rank, int file) : this(rank, Char.ToString((char)('a' + (file - 1)))) {
					}

					public ChessCoordinate(Coordinate c) : this(c.y, c.x) {

					}

					public static ChessCoordinate FromCoordinate(Coordinate c) {
						return new ChessCoordinate(c.y, Char.ToString((char)('a' + c.x - 1)));
					}

					public Coordinate ToCoordinate() {
						return new Coordinate((int)(_file[0] - 'a') + 1, _rank);
					}

					// Used mostly for placing pieces initially, basically when
					// we know what the default Chess coordinates are for the starting
					// positions.

					public static ChessCoordinate FromCoordinateString(string coordinate) {
						Regex r = new Regex(@"(?<file>[a-z])(?<rank>[0-9])");

						MatchCollection m = r.Matches(coordinate);

						if (m.Count >= 1) {
							GroupCollection g = m[0].Groups;

							return new ChessCoordinate(g["rank"].Value, g["file"].Value);
						} else {
							throw new InvalidChessCoordinateStringException(coordinate);
						}
					}

					public override bool Equals(object obj) {
						if (false == (obj is ChessCoordinate)) {
							return false;
						} else {
							ChessCoordinate c = (ChessCoordinate)obj;
							return (_file == c.file && _rank == c.rank);
						}
					}

					public override int GetHashCode() {
						return (int)(_file[0] - 'a') ^ _rank;
					}

					public static bool operator ==(ChessCoordinate c1, ChessCoordinate c2) {
						return c1.Equals(c2);
					}

					public static bool operator !=(ChessCoordinate c1, ChessCoordinate c2) {
						return !c1.Equals(c2);
					}

					public static ChessCoordinate operator -(ChessCoordinate c1, ChessCoordinate c2) {
						return ChessCoordinate.FromCoordinate(c1.ToCoordinate() - c2.ToCoordinate());
					}

					public static ChessCoordinate operator +(ChessCoordinate c1, ChessCoordinate c2) {
						return ChessCoordinate.FromCoordinate(c1.ToCoordinate() + c2.ToCoordinate());
					}

					public override string ToString() {
						return string.Format("rank={0}, file={1}", rank, file);
					}

					public int rank {
						get {
							return _rank;
						}
					}

					public string file {
						get {
							return _file;
						}
					}
				}
			}
		}
	}
}
