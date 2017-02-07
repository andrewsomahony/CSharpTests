using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			public abstract class CardPlayer : Player {
				protected CardHand _hand;

				protected CardPlayer(string name, CardHand hand) : base(name) {
					_hand = hand;
				}

				public virtual void ReceiveCard(Card p) {
					_hand.AddCard(p);
				}

				public override void Reset() {
					_hand.Clear();
				}
			}
		}
	}
}
