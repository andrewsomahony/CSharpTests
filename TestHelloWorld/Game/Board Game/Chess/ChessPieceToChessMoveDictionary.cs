using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				// Wrapper class for Dictionary<ChessPiece, ChessMove>, as
				// that's pretty painful to type all the time.

				public class ChessPieceToChessMoveDictionary : Dictionary<ChessPiece, ChessMove> {
					public ChessPieceToChessMoveDictionary() {
					}

					public ChessPieceToChessMoveDictionary(Dictionary<ChessPiece, ChessMove> d) : base(d) {

					}

					public ChessPieceToChessMoveDictionary(ChessPieceToChessMoveDictionary d) : base(d) {

					}
				}
			}
		}
	}
}
