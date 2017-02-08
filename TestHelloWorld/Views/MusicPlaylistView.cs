using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	using Parsers;

	public class MusicPlaylistView : ConsoleView {
		private MusicPlaylist _playlist;

		public MusicPlaylistView() {
			_playlist = new MusicPlaylist();
		}

		public override string title {
			get {
				return "Music Playlist Maker";
			}
		}

		public override void Init() {
			base.Init();

			_playlist.Load("SongLibrary.xml");
		}

		public override void Run() {
			Show();

			DefaultStringParser stringParser = new DefaultStringParser();
			IntStringParser intParser = new IntStringParser();

			while (true) {
				try {
					if (!ReadAndParse("Enter the starting letter", stringParser)) {
						break;
					}
					if (!ReadAndParse("Enter the number of songs", intParser)) {
						break;
					}

					List<Song> newPlaylist =
						_playlist.makePlaylist(Char.ToUpper(stringParser.value[0]), intParser.value);
					foreach (Song s in newPlaylist) {
						Console.WriteLine(s.name + " - " + s.artistName);
					}
				} catch (CannotMakePlaylistException e) {
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}
