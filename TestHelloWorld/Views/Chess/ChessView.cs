using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	using Parsers;
	using Games.BoardGames.Chess;

	public class InvalidScrollIndexException : Exception {
		public InvalidScrollIndexException(int index) : base("Invalid scroll index! " + index) {
			
		}
	}

	public class ChessView : StackMenu {
		protected ChessGame _game;

		// This is a local menu, so we need a way to
		// exit it when the menu says so, using the
		// special "DoExit" function.

		protected int _chessBoardBlockWidth;
		protected int _chessBoardBlockHeight;

		protected int _boardX;
		protected int _boardY;

		private int _currentMoveViewScrollIndex;
		private bool _hasScrolled;

		public ChessView(Dictionary<int, View> options) : base(options) {
			_chessBoardBlockWidth = 5;
			_chessBoardBlockHeight = 3;

			_boardX = 2;
			_boardY = 0;			
		}

		public ChessView() : this(new Dictionary<int, View>() {
			{1, new NewGameChessView()},
			{2, new PromptForOldGameChessView()}
		}) {

		}

		public override string title {
			get {
				return "Chess";
			}
		}

		private int boardWidth {
			get {
				if (null == _game) {
					return 8;
				} else {
					return _game.board.width;
				}
			}
		}

		private int boardHeight {
			get {
				if (null == _game) {
					return 8;
				} else {
					return _game.board.height;
				}
			}
		}

		private int boardDisplayWidth {
			get {
				return boardWidth * _chessBoardBlockWidth;
			}
		}

		private int boardDisplayHeight {
			get {
				return boardHeight * _chessBoardBlockHeight;
			}
		}

		protected int numMovesVisible {
			get {
				return (boardDisplayHeight / 2) * 2;
			}
		}

		public override void Init() {
			base.Init();
			ResizeScreen(100, 50);
		}

		public override void Stop() {
			// When the user decides they're done, we want to clear the game we're playing.
			if (null != _game) {
				_game.Clear();
			}
			base.Stop();
		}

		// This allows us to map the display to any potential locale,
		// with whatever characters they use.

		private char GetPieceDisplayCharacter(ChessPiece p) {
			return p.isWhite ? Char.ToUpper(p.character) : Char.ToLower(p.character);
		}

		protected void ScrollToMoveIndex(int index) {
			if (index < 0 || (index > 0 && index > _game.moveList.Count - numMovesVisible)) {
				throw new InvalidScrollIndexException(index);
			}
			    
			_hasScrolled = true;
			_currentMoveViewScrollIndex = index;
		}

		protected void DrawChessBoard() {
			int boardX = _boardX;
			int boardY = _boardY;

			char WHITE = '\u2588';
			char BLACK = (char)32;
			int BLOCK_WIDTH = _chessBoardBlockWidth;
			int BLOCK_HEIGHT = _chessBoardBlockHeight;

			for (int i = 0; i < boardHeight; i++) {
				SetCursorPosition(boardX, boardY + i * BLOCK_HEIGHT);
				for (int u = 0; u < BLOCK_HEIGHT; u++) {
					SetCursorPosition(boardX, boardY + i * BLOCK_HEIGHT + u);
					for (int j = 0; j < boardWidth; j++) {
						int c = (i + j) & 1;
						for (int k = 0; k < BLOCK_WIDTH; k++) {
							if (c == 1) {
								Console.Write(BLACK);
							} else {
								Console.Write(WHITE);
							}
						}
					}
				}
			}

			for (int i = 8; i >= 1; i--) {
				SetCursorPosition(boardX - 2, boardY + ((8 - i) * BLOCK_HEIGHT) + (BLOCK_HEIGHT / 2));
				Console.Write(i);
			}

			for (int i = 0; i < 8; i++) {
				SetCursorPosition(boardX + (i * BLOCK_WIDTH) + (BLOCK_WIDTH / 2), boardY + boardDisplayHeight);
				Console.Write((char)('a' + i));
			}

			if (null != _game) {
				foreach (ChessPiece piece in _game.chessBoard.chessPieces) {
					SetCursorPosition(boardX + (piece.coordinates.x - 1) * BLOCK_WIDTH + (BLOCK_WIDTH / 2),
									  boardY + (boardHeight - piece.coordinates.y) * BLOCK_HEIGHT
									  + (BLOCK_HEIGHT / 2));
					Console.Write(GetPieceDisplayCharacter(piece));
				}
			}

			SetCursorPosition(0, boardY + boardDisplayHeight + 2);		
		}

		protected virtual string FormatMoveListString(string moveString, int index) {
			return moveString;
		}

		protected virtual void DrawExtraStatus() {
			
		}

		protected virtual void DrawInterface() {
			Clear();

			DrawChessBoard();

			// Draw the taken pieces on the right side of the board,
			// in a square-like format.

			int boardX = _boardX;
			int boardY = _boardY;

			int numRowsForTakenPieces = 4;

			int startRow;

			int currentRow;
			int currentColumn;

			int rowCount;
			List<ChessPiece> takenPieceArray;

			for (int i = 0; i < 2; i++) {
				if (0 == i) {
					takenPieceArray = _game.PlayerPiecesThatAreTaken(_game.blackPlayer);
					SetCursorPosition(boardX + boardDisplayWidth + 1, boardY);
				} else {
					takenPieceArray = _game.PlayerPiecesThatAreTaken(_game.whitePlayer);
					SetCursorPosition(boardX + boardDisplayWidth + 1, 
					                  boardY + boardDisplayHeight - (numRowsForTakenPieces * 2) + 1);
				}

				startRow = cursorY;

				currentRow = cursorY;
				currentColumn = cursorX;

				rowCount = 0;

				foreach (ChessPiece piece in takenPieceArray) {
					Console.Write(GetPieceDisplayCharacter(piece));
					rowCount++;

					if (rowCount == numRowsForTakenPieces) {
						rowCount = 0;

						currentColumn += 2;
						currentRow = startRow;
					} else {
						currentRow += 2;
					}
					SetCursorPosition(currentColumn, currentRow);
				}
			}

			// Draw the move list after the taken pieces.
			// Draw in the 1. e4 e5 format; with each player's move
			// on the same line per sequence.

			int movesX = boardX + boardDisplayWidth + 1 + ((16 / numRowsForTakenPieces) * 2) + 1;
			int currentY = boardY;

			// Each line gets two moves, but each move line occupies two console lines.


			SetCursorPosition(movesX,
							  currentY);

			int startIndex = true == _hasScrolled ?
				_currentMoveViewScrollIndex :
				_game.moveList.Count > numMovesVisible ? _game.moveList.Count - numMovesVisible : 0;
			// We never want to start on an odd number, as we want to only draw
			// complete two-move sequences, except for the last move drawn.

			if (1 == (startIndex % 2)) {
				startIndex++;
			}
			int max = Math.Min(startIndex + numMovesVisible, _game.moveList.Count);

			for (int i = startIndex; i < max; i++) {
				if (0 == (i % 2)) {
					Console.Write("" + ((i / 2) + 1) + ". ");
				}

				Console.Write(FormatMoveListString(_game.moveList[i], i));

				Console.Write(" ");

				if (1 == (i % 2)) {
					currentY += 2;
					SetCursorPosition(movesX, currentY);
				}
			}

			int statusTextX = 0;
			int statusTextY = boardY + boardDisplayHeight + 2;

			// Position the cursor to accept moves
			SetCursorPosition(statusTextX, statusTextY);

			// Draw any sort of status

			if (_game.numChecks > 0) {
				switch (_game.numChecks) {
					case 2:
						Console.Write("Double ");
						break;
					default:
						break;
				}
				Console.WriteLine("Check!");

				SetCursorPosition(statusTextX, statusTextY + 2);
			} else if (true == _game.isInCheckmate) {
				Console.WriteLine("Checkmate!  " + _game.currentPlayer.name + " wins!");

				SetCursorPosition(statusTextX, statusTextY + 2);
			} else if (true == _game.isAStalemate) {
				Console.WriteLine("Stalemate!");

				SetCursorPosition(statusTextX, statusTextY + 2);
			}

			DrawExtraStatus();
			SetCursorPosition(statusTextX, cursorY + 1);
		}

		public override void Run() {
			Clear();
			DrawChessBoard();

			base.Run();
		}
	}
}
