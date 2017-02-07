using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				public class PlayingCardDeck52 : PlayingCardDeck {
					public PlayingCardDeck52() : base(52) {
						for (int i = 0; i < 4; i++) {
							// This explicit cast for the suits works as the index for the enum
							// is also zero-based.					
							PlayingCardSuit suit = (PlayingCardSuit)i;

							// !!! I would write a unit test for this to make sure it's being initialized
							// !!! properly, but I only have one file to work with for this test ;)

							AddCard(new PlayingCard(PlayingCardValue.Two, suit));
							AddCard(new PlayingCard(PlayingCardValue.Three, suit));
							AddCard(new PlayingCard(PlayingCardValue.Four, suit));
							AddCard(new PlayingCard(PlayingCardValue.Five, suit));
							AddCard(new PlayingCard(PlayingCardValue.Six, suit));
							AddCard(new PlayingCard(PlayingCardValue.Seven, suit));
							AddCard(new PlayingCard(PlayingCardValue.Eight, suit));
							AddCard(new PlayingCard(PlayingCardValue.Nine, suit));
							AddCard(new PlayingCard(PlayingCardValue.Ten, suit));
							AddCard(new PlayingCard(PlayingCardValue.Jack, suit));
							AddCard(new PlayingCard(PlayingCardValue.Queen, suit));
							AddCard(new PlayingCard(PlayingCardValue.King, suit));
							AddCard(new PlayingCard(PlayingCardValue.Ace, suit));
						}
					}
				}
			}
		}
	}
}
