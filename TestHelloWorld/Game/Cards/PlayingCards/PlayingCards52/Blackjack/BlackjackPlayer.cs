using System;
using System.Collections.Generic;
using System.Linq;

namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				class AddingCardToNonexistentHandException : Exception {
					public AddingCardToNonexistentHandException() : base("Hand does not exist!") {
						
					}
				}

				public class BlackjackPlayer : CardPlayer {
					// We want to just represent the base hand in an array
					// of all hands we have.
					protected List<PlayingCardHand> _hands;

					public BlackjackPlayer(string name) : base(name, new PlayingCardHand()) {
						_hands = new List<PlayingCardHand>();

						// Thanks to the wonders of Polymorphism, all
						// we need here is a simple explicit cast.

						_hands.Add((PlayingCardHand)_hand);
					}

					public override void Reset() {
						base.Reset();

						foreach (PlayingCardHand hand in _hands) {
							hand.Clear();
						}
					}

					// Type safety
					// The explicit cast will be caught at runtime
					// if it's not possible.
					public override void ReceiveCard(Card c) {
						this.ReceiveCard((PlayingCard)c, 0);
					}

					public void ReceiveCard(PlayingCard p, int handIndex) {
						try {
							_hands[handIndex].AddCard(p);
						} catch (IndexOutOfRangeException) {
							throw new AddingCardToNonexistentHandException();	
						}
					}

					public IReadOnlyList<PlayingCardHand> hands {
						get {
							return _hands;
						}
					}
				}
			}
		}
	}
}
