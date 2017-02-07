using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				public abstract class PlayingCardGame52 : PlayingCardGame {
					public PlayingCardGame52(int numPlayers) : base(numPlayers, new PlayingCardDeck52()) {
					}
				}
			}
		}
	}
}
