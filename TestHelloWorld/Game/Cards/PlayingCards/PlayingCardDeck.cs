using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				public abstract class PlayingCardDeck : CardDeck {
					public PlayingCardDeck(int numCards) : base(numCards) {

					}

					public override bool isValidDeck {
						get {
							return _deck.Count == maxNumCards;
						}
					}
				}
			}
		}
	}
}
