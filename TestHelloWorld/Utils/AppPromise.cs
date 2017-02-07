using System;

using RSG;

namespace TestHelloWorld {
	namespace Utils {
		public class AppPromise<T> {
			private Promise<T> _promise;

			public AppPromise(Promise<T> promise) {
				_promise = promise;
			}

			public static AppPromise<T> CreatePromise(Action<Action<T>, Action<Exception>> action) {
				return new AppPromise<T>(new Promise<T>((resolve, reject)
														=> action.Invoke(resolve, reject)));
			}

			public Promise<T> promise {
				get {
					return _promise;
				}
			}
		}
	}
}
