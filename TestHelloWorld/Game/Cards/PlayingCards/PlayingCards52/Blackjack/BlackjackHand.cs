using System;
namespace TestHelloWorld {
	namespace Games {
		namespace Cards {
			namespace PlayingCards {
				public enum BlackjackHandResult {
					None,
					Win,
					Lose,
					Bust
				}

				public class BlackjackHand : PlayingCardHand {
					private BlackjackHandResult _result;

					public BlackjackHand() : base(0) {
					}

					public BlackjackHandResult result {
						get {
							return _result;
						}
						set {
							_result = value;
						}
					}
				}
			}
		}
	}
}
