using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	using Utils;

	namespace Games {
		namespace Cards {
			// If we have a specific number of cards we want in a deck (like 52 for a normal playing
			// card deck), but we try to add too many
			class TooManyCardsInDeckException : Exception {
				public TooManyCardsInDeckException(int numCards, int maxCards)
					: base("Too many cards in the deck! " + numCards + " > " + maxCards) {

				}
			}

			// If we try to deal a card from the deck but there are no more left.
			class NoMoreCardsInDeckException : Exception {
				public NoMoreCardsInDeckException() : base("No more cards left in the deck!") {

				}
			}

			// Abstract base class for a deck of cards.
			// Contains shuffling methods and resetting methods,
			// and manages which cards have been dealt.

			public abstract class CardDeck {
				protected List<Card> _deck;
				protected List<bool> _cardsDealt;

				protected CardDeck(int numCards) {
					// The indexes between these two match up.
					// I felt this was the best way to track what was dealt
					// and what wasn't, as being dealt isn't a property of the
					// card, but actually a property of the deck.

					_deck = new List<Card>(numCards);
					_cardsDealt = new List<bool>(numCards);
				}

				public void AddCard(Card c) {
					if (_deck.Count == maxNumCards) {
						throw new TooManyCardsInDeckException(_deck.Count + 1, maxNumCards);
					}
					_deck.Add(c);
					_cardsDealt.Add(false);
				}

				public Card DealCard() {
					if (0 == numCardsLeft) {
						throw new NoMoreCardsInDeckException();
					}

					_cardsDealt[firstCardNotDealtIndex] = true;
					return _deck[firstCardNotDealtIndex];
				}

				public void Reset() {
					for (int i = 0; i < maxNumCards; i++) {
						_cardsDealt[i] = false;
					}
				}

				public void Shuffle() {
					// We only need to shuffle the cards that haven't been dealt.
					// Therefore, we can just start wherever the first non-dealt card is

					List<Card> tempCardDeck = new List<Card>(numCardsLeft);
					List<int> cardIndexesRemovedFromDeckForShuffling = new List<int>();

					for (int i = 0; i < numCardsLeft; i++) {
						int nextIndex;
						// !!! Infinite loop risk?
						while (true) {
							nextIndex = RandomNumber.Instance().GenerateBetween(firstCardNotDealtIndex, numCardsLeft);

							if (false == cardIndexesRemovedFromDeckForShuffling.Contains(nextIndex)) {
								break;
							}
						}

						tempCardDeck.Add(_deck[nextIndex]);
						cardIndexesRemovedFromDeckForShuffling.Add(nextIndex);
					}

					_deck.RemoveRange(firstCardNotDealtIndex, numCardsLeft);
					_deck.AddRange(tempCardDeck);
				}

				public int maxNumCards {
					get {
						return _deck.Capacity;
					}
				}

				public int numCardsLeft {
					get {
						int count;
						for (count = 0; count < maxNumCards; count++) {
							if (false == _cardsDealt[count]) {
								break;
							}
						}
						return _deck.Count - count;
					}
				}

				public int firstCardNotDealtIndex {
					get {
						return maxNumCards - numCardsLeft;
					}
				}

				// Each deck has its own way to decide if it's valid.
				public abstract bool isValidDeck {
					get;
				}
			}
		}
	}
}
