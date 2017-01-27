using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace TestHelloWorld {
	using Alphabets;
	class SongNameAlphabet : Alphabet { 
		public override string alphabet {
			get {
				return "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			}
		}
	}

	public class Artist {
		private string _name;
		private List<Song> _songs;

		public Artist(string name) {
			_name = name;
			_songs = new List<Song>();
		}

		public void addSong(Song s) {
			_songs.Add(s);
		}

		public List<Song> songs {
			get {
				return _songs;
			}
		}

		public string name { 
			get {
				return _name;
			}
		}

		public int numSongs {
			get {
				return _songs.Count;
			}
		}
	}

	class InvalidSongNameException : Exception {
		private string _songName;
		public InvalidSongNameException(string name) : base(name) {
			_songName = name;
		}

		public string songName { 
			get {
				return _songName;
			}
		}
	}

	public class Song {
		private string _name;
		private string _id;
		private int _durationMs;

		private readonly Artist _artist;

		public Song(string name, string id, int durationMs, Artist artist) {
			_name = name;
			_id = id;
			_durationMs = durationMs;
			_artist = artist;
		}

		public string name {
			get {
				return _name;
			}
		}

		public string artistName {
			get {
				return _artist.name;
			}
		}

		public string id {
			get {
				return _id;
			}
		}

		public int durationMs {
			get {
				return _durationMs;
			}
		}

		public char firstCharacterInName {
			get {
				SongNameAlphabet sa = new SongNameAlphabet();
				foreach (char c in _name) {
					if (true == sa.charIsValid(c)) {
						return c;
					}
				}
				throw new InvalidSongNameException(_name);
			}
		}

		public char lastCharacterInName { 
			get { 
				SongNameAlphabet sa = new SongNameAlphabet();
				for (int i = _name.Length - 1; i >= 0; i--) {
					if (true == sa.charIsValid(_name[i])) {
						return _name[i];
					}
				}
				throw new InvalidSongNameException(_name);
			}
		}
	}

	public class InvalidMusicPlaylistFileException : Exception {
		public InvalidMusicPlaylistFileException(string filename, string reason = "")
			: base("Invalid music playlist! " + filename + " (" + reason + ")") {
			
		}
	}

	public class CannotMakePlaylistException : Exception {
		public CannotMakePlaylistException(char startingLetter, char badLetter, int length)
			: base("Cannot make playlist with starting letter " + startingLetter + " and length " + length
			       + "; cannot find a song for letter " + badLetter) {

		}
	}

	public class MusicPlaylist {
		private List<Artist> _artists;

		private Hashtable _songsByStartLetter;
		private Hashtable _songsByEndLetter;

		private static Random rng = new Random();

		public MusicPlaylist() {

		}

		public void Load(string filename) {
			_artists = new List<Artist>();

			_songsByEndLetter = new Hashtable();
			_songsByStartLetter = new Hashtable();

			SongNameAlphabet sa = new SongNameAlphabet();
			foreach (char c in sa.alphabet) {
				_songsByEndLetter[c] = new List<Song>();
				_songsByStartLetter[c] = new List<Song>();
			}

			XmlTextReader reader = new XmlTextReader(filename);
			reader.WhitespaceHandling = WhitespaceHandling.None;

			Artist currentArtist = null;

			try {
				while (!reader.EOF &&
					   reader.Read()) {
					if (XmlNodeType.Element == reader.NodeType) {
						if ("Library" == reader.Name &&
							false == reader.IsStartElement()) {
							break;
						} else {
							if ("Artist" == reader.Name &&
								true == reader.IsStartElement()) {
								currentArtist = new Artist(reader.GetAttribute("name"));
								_artists.Add(currentArtist);
							} else if ("Song" == reader.Name &&
									   true == reader.IsStartElement()) {
								Song s = new Song(reader.GetAttribute("name"),
												  reader.GetAttribute("id"),
												  Int32.Parse(reader.GetAttribute("duration")),
												  currentArtist);

								currentArtist.addSong(s);
								hashSong(s);
							}
						}
					}
				}
			} catch (InvalidSongNameException e) {
				throw new InvalidMusicPlaylistFileException(filename, e.songName);
			} catch (Exception) {
				throw new InvalidMusicPlaylistFileException(filename);
			}			
		}

		class CannotFindSongForLetterException : Exception {
			private char _letter;
			public CannotFindSongForLetterException(char letter) : base("Cannot find song for letter " + letter) {
				_letter = letter;
			}

			public char letter {
				get {
					return _letter;
				}
			}
		}

		public List<Song> makePlaylist(char startingLetter, int numSongs) {
			List<Song> playlist = new List<Song>(numSongs);

			try {
				addSong(playlist, startingLetter, numSongs);
			} catch (CannotFindSongForLetterException e) {
				throw new CannotMakePlaylistException(startingLetter, e.letter, numSongs);
			}

			return playlist;
		}

		private void addSong(List<Song> playlist, char startingLetter, int songsLeft) {
			if (songsLeft > 0) {
				char startingLetterUppercase = Char.ToUpper(startingLetter);

				List<Song> availableSongs = _songsByStartLetter[startingLetterUppercase] as List<Song>;

				int numTries = availableSongs.Count;
				while (0 != numTries) {
					Song testSong = availableSongs[rng.Next(0, availableSongs.Count)];

					if (null == playlist.Find(element => element.id == testSong.id)) {
						playlist.Add(testSong);
						try {
							addSong(playlist, Char.ToUpper(testSong.lastCharacterInName), songsLeft - 1);
							break;
						} catch (CannotFindSongForLetterException) {
							playlist.RemoveAt(playlist.Count - 1);
						}
					}

					numTries--;
				}

				if (0 == numTries) {
					throw new CannotFindSongForLetterException(startingLetterUppercase);
				}
			}
		}

		private void hashSong(Song s) {
			char startLetter = Char.ToUpper(s.firstCharacterInName);
			char endLetter = Char.ToUpper(s.lastCharacterInName);

			List<Song> startArray = _songsByStartLetter[startLetter] as List<Song>;
			List<Song> endArray = _songsByEndLetter[endLetter] as List<Song>;

			startArray.Add(s);
			endArray.Add(s);
		}
	}
}
