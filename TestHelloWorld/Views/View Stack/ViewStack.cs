using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	// The ViewStack handles running views.
	// Views are classes that can initialize and run anything they like, from
	// their own code, to separate programs that can run asynchronously.

	// This class handles the non-async and async views, handles closing and exiting views
	// by catching certain exceptions, and handles presentation in regards to the console
	// clearing and such.

	public class ViewStack {
		private List<IStackView> _views;

		public ViewStack() {
			_views = new List<IStackView>();
		}

		public void PushView(IStackView v) {
			Console.Clear();

			_views.Add(v);
			v.parentViewStack = this;

			bool hasExited = false;
			try {
				v.Init();
				v.Run();
			} catch (ViewReceivedExitCommandException) {
				hasExited = true;
			} catch (ViewReceivedCloseCommandException) {
				// Let the finally block close the view
			} catch (AggregateException e) {
				// These are thrown if this view is async.

				e.Handle((exception) => {
					if (exception is ViewReceivedExitCommandException) {
						hasExited = true;
						return true;
					} else if (exception is ViewReceivedCloseCommandException) {
						return true;
					} else {
						return false;
					}
				});
			} finally {
				if (true == hasExited) {
					Exit();
				} else {
					v.Close();
					PopView();
				}
			}
		}

		private void PopView() {
			if (_views.Count > 1) {
				Console.Clear();
			}

			IStackView v = _views[_views.Count - 1];
			v.Stop();
			_views.Remove(v);
		}

		public void Exit() {
			int limit = _views.Count;
			for (int i = limit - 1; i >= 0; i--) {
				_views[i].Close();
			}
		}
	}
}
