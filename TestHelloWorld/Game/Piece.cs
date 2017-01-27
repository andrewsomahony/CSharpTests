using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		public abstract class Piece {
			private Coordinate _coordinates;
			private int _numMoves;

			// A piece can simply exist in the ether (as a part of the player), without
			// being placed on the board.  This is here to see
			// if the piece is actually on the board.

			private bool _isPlaced;

			protected readonly char _character;

			internal List<Move> _moves;

			public Piece() {
				_moves = new List<Move>();
				_numMoves = 0;
				_isPlaced = false;
			}

			protected Piece(char c) : this() {
				_character = c;
			}

			internal void AddMove(Move m) {
				_moves.Add(m);
			}

			public virtual void Place(Coordinate c) {
				_coordinates = c;
				_numMoves = 0;
				_isPlaced = true;
			}

			public virtual void UnPlace() {
				_isPlaced = false;
			}

			public virtual void Move(Coordinate c) {
				_coordinates = c;
				_numMoves++;
			}

			public virtual void UnMove(Coordinate c) {
				_coordinates = c;
				_numMoves--;
			}

			internal List<Move> moves { 
				get {
					return _moves;
				}
			}

			public Coordinate coordinates {
				get {
					return _coordinates;
				}
			}

			public bool hasMoved { 
				get {
					return 0 != _numMoves;
				}
			}

			public bool isPlaced {
				get {
					return _isPlaced;
				}
			}

			public int numMoves {
				get {
					return _numMoves;
				}
			}

			public virtual char displayCharacter {
				get {
					return _character;
				}
			}

			public char character {
				get {
					return _character;
				}
			}
		}
	}
}
