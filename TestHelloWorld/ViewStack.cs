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

			v.Init();
			while (v.isAlive) {
				v.Run();
			}
			PopView();
		}

		private void PopView() {
			Console.Clear();

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
