using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Chess {
			namespace ChessFileIO {
				public abstract partial class ChessFile {
					private readonly List<FileChessGame> _loadedGames;

					protected ChessFile() {
						_loadedGames = new List<FileChessGame>();
					}

					public ChessFile(ChessGame game) : this() {

					}

					public abstract bool CanLoad(string filename);

					public abstract void Load(string filename);
					public abstract void Save(string filename);

					protected void AddLoadedGame(FileChessGame g) {
						_loadedGames.Add(g);
					}

					// We never return the actual game, just a copy of it.

					public FileChessGame this[int index] {
						get {
							return loadedGames[index];
						}
					}

					public int numGames {
						get {
							return _loadedGames.Count;
						}
					}

					public List<FileChessGame> loadedGames {
						get {
							return new List<FileChessGame>(_loadedGames);
						}
					}

					public abstract string name {
						get;
					}
				}
			}
		}
	}
}
