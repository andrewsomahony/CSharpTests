using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	public class ViewStack {
		private List<View> _views;

		public ViewStack() {
			_views = new List<View>();
		}

		public void PushView(View v) {
			Console.Clear();

			_views.Add(v);

			v.parentViewStack = this;

			try {
				v.Init();
				v.Run();
			} catch (ViewReceivedExitCommandException) {
				Exit();
			} catch (ViewReceivedCloseCommandException) {

			} catch (AggregateException e) {
				Console.WriteLine(e.InnerExceptions.Count);
				Console.WriteLine(e.GetBaseException());
				foreach (Exception innerException in e.InnerExceptions) {
					Console.WriteLine(e);
				}
				// These are thrown if this view is async.
				// Just ignore them.
			} catch (Exception) {
				
			} finally {
				v.Close();
				v.Stop();
			}

			PopView();
		}

		private void PopView() {
			if (_views.Count > 1) {
				Console.Clear();
			}

			View v = _views[_views.Count - 1];
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
