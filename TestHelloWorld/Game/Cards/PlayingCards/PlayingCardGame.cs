using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				public abstract class PlayingCardGame : CardGame {
					protected PlayingCardGame(int numPlayers, PlayingCardDeck deck) : base(numPlayers, deck) {

					}
				}
			}
		}
	}
}
