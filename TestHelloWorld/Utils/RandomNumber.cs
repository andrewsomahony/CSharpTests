using System;

namespace TestHelloWorld {
	namespace Utils {
		// Utility class to generate random numbers
		// Thread-safe

		public class RandomNumber {
			static RandomNumber singleton;

			private Object _randomNumberGeneratorLock;
			private Random _randomNumberGenerator;

			public RandomNumber() {
				_randomNumberGenerator = new Random();
				_randomNumberGeneratorLock = new Object();
			}

			public int Generate() {
				return GenerateBetween(int.MinValue, int.MaxValue);
			}

			public int GenerateBetween(int start, int end) {
				lock (_randomNumberGeneratorLock) {
					return _randomNumberGenerator.Next(start, end);
				}
			}

			public static RandomNumber Instance() {
				if (null == singleton) {
					singleton = new RandomNumber();
				}
				return singleton;
			}
		}
	}
}
