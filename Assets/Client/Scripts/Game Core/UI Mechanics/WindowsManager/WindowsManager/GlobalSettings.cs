using Zenject;


namespace GlobalSettings {
	public class Screen {
		[Inject] private BackButton _backButton;
		
		private int _count;


		public void EnableInteractions(bool force) {
			if (_count > 0) _count--;
			if (force) _count = 0;
			if (_count == 0) {
				_backButton.Enable();
			}
		}

		public void DisableInteractions() {
			if (_count == 0) {
				_backButton.Disable();
			}
			_count++;
		}
	}
}

