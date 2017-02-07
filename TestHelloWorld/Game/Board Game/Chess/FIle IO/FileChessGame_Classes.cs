using System;
namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				public partial class FileChessGame {
					class CommentWithMoveNumberEntry {
						private string _comment;
						private int _moveNumber;

						public CommentWithMoveNumberEntry(string comment, int moveNumber) {
							_comment = comment;
							_moveNumber = moveNumber;
						}

						public string comment {
							get {
								return _comment;
							}
						}

						public int moveNumber {
							get {
								return _moveNumber;
							}
						}
					}

					class RAVWithMoveNumberEntry {
						private string _rav;
						private int _moveNumber;

						public RAVWithMoveNumberEntry(string rav, int moveNumber) {
							_rav = rav;
							_moveNumber = moveNumber;
						}

						public string rav {
							get {
								return _rav;
							}
						}

						public int moveNumber {
							get {
								return _moveNumber;
							}
						}
					}
				}
			}
		}
	}
}
