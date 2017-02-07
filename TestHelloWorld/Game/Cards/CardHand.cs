using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			public abstract class CardHand {
				protected List<Card> _hand;
				protected List<bool> _cardIsHidden;

				protected CardHand(int numCards) {
					if (numCards > 0) {
						_cardIsHidden = new List<bool>(numCards);
						_hand = new List<Card>(numCards);
					} else {
						_hand = new List<Card>();
						_cardIsHidden = new List<bool>();
					}
				}

				public virtual void AddCard(Card c, bool isHidden) {
					_hand.Add(c);
					_cardIsHidden.Add(isHidden);					
				}

				public virtual void AddCard(Card c) {
					AddCard(c, false);
				}

				public void ShowCard(int index) {
					_cardIsHidden[index] = false;
				}

				public void HideCard(int index) {
					_cardIsHidden[index] = true;
				}

				public void ShowAllCards() {
					for (int i = 0; i < _cardIsHidden.Count; i++) {
						ShowCard(i);
					}
				}

				public void Clear() {
					_hand.Clear();
				}

				public int Count() {
					return _hand.Count;
				}

				public virtual Card this[int index] {
					get {
						return _hand[index];
					}
				}
			}
		}
	}
}
