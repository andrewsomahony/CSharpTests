using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				public class PlayingCardHand : CardHand {
					public PlayingCardHand() : base(0) {
						
					}

					public PlayingCardHand(int numCards) : base(numCards) {
					}

					public override void AddCard(Card c, bool isHidden) {
						// Explicit cast, will be caught at runtime if incorrect.

						base.AddCard((PlayingCard)c, isHidden);
					}

					public override void AddCard(Card c) {
						AddCard(c, false);
					}

					new public PlayingCard this[int index] {
						get {
							return (PlayingCard)_hand[index];
						}
					}
				}
			}
		}
	}
}
