using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				// If we try to initialize a PlayingCard object with an invalid suit
				public class InvalidPlayingCardSuitException : Exception {
					public InvalidPlayingCardSuitException(PlayingCardSuit suit)
						: base("Invalid playing card suit! " + suit) {

					}
				}

				// If we try to initialize a PlayingCard object with an invalid value
				public class InvalidPlayingCardValueException : Exception {
					public InvalidPlayingCardValueException(PlayingCardValue value)
						: base("Invalid playing card value! " + value) {

					}
				}

				// Enum for possible values of a normal playing card
				public enum PlayingCardValue {
					// Code can assume these to be in sequential order,
					// but the starting index is unknown and should not be assumed.
					Two,
					Three,
					Four,
					Five,
					Six,
					Seven,
					Eight,
					Nine,
					Ten,
					Jack,
					Queen,
					King,
					Ace
				}

				// Enum for the possible suits of a normal playing card
				public enum PlayingCardSuit {
					// =0 to show that explicit casts for this enum 
					// can work with for loops starting with 0
					Spade = 0,
					Heart,
					Diamond,
					Club
				}

				// Class to represent a normal playing card
				// from a 52 card deck.

				public class PlayingCard : Card {
					private PlayingCardValue _value;
					private PlayingCardSuit _suit;

					public PlayingCard(PlayingCardValue value, PlayingCardSuit suit) {
						if (value < PlayingCardValue.Two ||
							value > PlayingCardValue.Ace) {
							throw new InvalidPlayingCardValueException(value);
						}

						if (suit < PlayingCardSuit.Spade ||
							suit > PlayingCardSuit.Club) {
							throw new InvalidPlayingCardSuitException(suit);
						}
						_value = value;
						_suit = suit;
					}

					public PlayingCardValue value {
						get {
							return _value;
						}
					}

					public PlayingCardSuit suit {
						get {
							return _suit;
						}
					}

					public override string ToString() {
						string returnValue = "";

						string valueString;

						if (_value >= PlayingCardValue.Two &&
							_value <= PlayingCardValue.Ten) {
							valueString = (2 + (_value - PlayingCardValue.Two)).ToString();
						} else if (PlayingCardValue.Jack == _value) {
							valueString = "J";
						} else if (PlayingCardValue.Queen == _value) {
							valueString = "Q";
						} else if (PlayingCardValue.King == _value) {
							valueString = "K";
						} else {
							// Guaranteed to be an ace, otherwise the constructor
							// would have thrown an exception.
							valueString = "A";
						}

						string suitString;

						if (PlayingCardSuit.Spade == _suit) {
							suitString = "S";
						} else if (PlayingCardSuit.Heart == _suit) {
							suitString = "H";
						} else if (PlayingCardSuit.Diamond == _suit) {
							suitString = "D";
						} else {
							// Guaranteed to be a club, otherwise the constructor
							// would have thrown an exception.
							suitString = "C";
						}

						returnValue += valueString;
						returnValue += "(" + suitString + ")";

						return returnValue;
					}
				}
			}
		}
	}
}
