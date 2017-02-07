using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				using Utils;

				// Base class for Blackjack.
				// Handles hand values using an internal class.

				class Blackjack : PlayingCardGame52 {
					private const int _bestNonBustHandValue = 21;
					private const string _defaultDealerName = "Dealer";

					class BlackjackHandAnalyzer {
						// This class computes all possible values for a player's hand,
						// as aces can count for 1 or 11.
						private List<int> _possibleHandValues;
						private PlayingCardHand _hand;

						public BlackjackHandAnalyzer(BlackjackPlayer player, int handIndex) {
							_hand = player.hands[handIndex];
							_possibleHandValues = new List<int>();

							CreateNewCardValue(0, 0);
						}

						// We store all possible card values in an array,
						// and this function creates a new one using the recursive
						// function, SumCardValueAtIndex

						private void CreateNewCardValue(int startValue, int startIndex) {
							_possibleHandValues.Add(SumCardValueAtIndex(startValue, startIndex));
						}

						// Recursive function to iterate through the player's hand and sum up
						// the card values.  If it encounters an Ace, it counts it as 1, but also
						// starts a NEW recursion for a new value, using 11, as an Ace can either be
						// 1 or 11.

						private int SumCardValueAtIndex(int currentValue, int index) {
							if (index == _hand.Count()) {
								return currentValue;
							} else {
								PlayingCard p = _hand[index];
								int value = 0;

								if (PlayingCardValue.Ace == p.value) {
									value = 1;
									CreateNewCardValue(currentValue + 11, index + 1);
								} else {
									if (p.value >= PlayingCardValue.Two &&
										p.value <= PlayingCardValue.Ten) {
										value = 2 + (p.value - PlayingCardValue.Two);
									} else {
										// Guaranteed to be a face card
										value = 10;
									}
								}

								return SumCardValueAtIndex(currentValue + value, index + 1);
							}
						}

						// Whatever the player's highest value is.
						// First check if it's a non-bust value, then,
						// if it isn't, just pick the first bust value as
						// it doesn't really matter.

						// Blackjack's best value for a player is their highest
						// non-bust value.

						public int bestValue {
							get {
								int maxNonBustValue = 0;
								foreach (int value in _possibleHandValues) {
									if (value <= _bestNonBustHandValue &&
										value > maxNonBustValue) {
										maxNonBustValue = value;
									}
								}

								if (0 == maxNonBustValue) {
									// Oops, it's a bust.
									// If there isn't a non-bust value, it doesn't matter,
									// we can return whichever one we want.

									return _possibleHandValues[0];
								}

								return maxNonBustValue;
							}
						}

						public bool isBust {
							get {
								return bestValue > _bestNonBustHandValue;
							}
						}

						public IReadOnlyList<int> possibleHandValues {
							get {
								return _possibleHandValues;
							}
						}
					}

					public Blackjack() : this(0) {

					}

					public Blackjack(int numPlayers) : base(numPlayers) {

					}

					protected override void Reset() {
					}

					public override void Clear() {
						foreach (BlackjackPlayer p in _players) {
							p.Reset();
						}
					}

					public void AddPlayer(BlackjackPlayer b) {
						base.AddPlayer(b);
					}

					// This is to simulate a game, for the purposes of
					// testing results.

					public void SimulateGame() {
						NewGame();

						foreach (Player player in _players) {
							int playerCardCount = RandomNumber.Instance().GenerateBetween(2, 6);

							for (int j = 0; j < playerCardCount; j++) {
								DealCardToPlayer(player as BlackjackPlayer);
							}
						}
					}

					public int BestHandResultForPlayer(BlackjackPlayer player) {
						return new BlackjackHandAnalyzer(player, 0).bestValue;
					}

					// This determines the result for each player.

					// If the dealer has bust, the player automatically wins no matter what.
					// Otherwise, if the player's hand is under 21 and beats the dealer,
					// or the player's hand is under 21 and composed of 5 cards, they win!

					// If the player doesn't beat the dealer's hand, it's a loss
					// If the player is over 21, then they're bust.

					public BlackjackHandResult ResultForPlayer(BlackjackPlayer player) {
						/*BlackjackHandAnalyzer dealerHands = new BlackjackHandAnalyzer(dealer, 0);

						if (player == dealer) {
							if (true == dealerHands.isBust) {
								return BlackjackHandResult.Bust;
							} else {
								// The dealer's performance is based on the win-lose performance of
								// the other players.
								return BlackjackHandResult.None;
							}
						} else {
							if (true == dealerHands.isBust) {
								return BlackjackHandResult.Win;
							} else {
								if (BestHandResultForPlayer(player) > _bestNonBustHandValue) {
									return BlackjackHandResult.Bust;
								} else {
									if (5 == player.hands[0].Count() ||
										BestHandResultForPlayer(player) > dealerHands.bestValue) {
										return BlackjackHandResult.Win;
									} else {
										return BlackjackHandResult.Lose;
									}
								}
							}
						}*/
						throw new NotImplementedException();
					}

					public override string name {
						get {
							return "Blackjack";
						}
					}
				}
			}
		}
	}
}
