using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace BoardGames {
			namespace Chess {
				using ChessFileIO;

				public class NoLoaderForChessFileException : Exception {
					public NoLoaderForChessFileException(string filename) : base("No loader for Chess file! " + filename) { }
				}

				public class ChessFileLoader {
					private readonly List<Type> _chessFileLoaders;

					public ChessFileLoader() {
						_chessFileLoaders = new List<Type>();

						_chessFileLoaders.Add(typeof(PGNChessFile));
					}

					public ChessFile LoadFile(string filename) {
						ChessFile finalFile = null;

						foreach (Type t in _chessFileLoaders) {
							ChessFile f = Activator.CreateInstance(t) as ChessFile;

							if (true == f.CanLoad(filename)) {
								finalFile = f;
								break;
							}
						}

						if (null == finalFile) {
							throw new NoLoaderForChessFileException(filename);
						}

						finalFile.Load(filename);

						return finalFile;
					}
				}
			}
		}
	}
}
