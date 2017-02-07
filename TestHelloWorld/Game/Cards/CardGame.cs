using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			public abstract class CardGame : Game {
				protected readonly CardDeck _deck;

				protected CardGame(int numPlayers, CardDeck deck) : base(numPlayers) {
					_deck = deck;
				}

				public void DealCardToPlayer(CardPlayer p) {
					p.ReceiveCard(_deck.DealCard());
				}

				public override void NewGame() {
					base.NewGame();

					_deck.Reset();
					_deck.Shuffle();
				}
			}
		}
	}
}
