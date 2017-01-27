using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	public class MainMenu : StackMenu {
		public MainMenu() : base(new Dictionary<int, View>() {
			{1, new DaysSinceView()},
			{2, new RomanNumeralMenu()},
			{3, new MusicPlaylistView()},
			{4, new FibonacciView()},
			{5, new ChessView()}
		}) {
		}

		public override string title {
			get {
				return "Main Menu";
			}
		}
	}
}
