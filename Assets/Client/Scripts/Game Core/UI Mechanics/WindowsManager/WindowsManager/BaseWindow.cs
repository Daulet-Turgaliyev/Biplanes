using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
public abstract class BaseWindow: MonoBehaviour
{
	[Inject] protected readonly DiContainer _diContainer;
	
	[SerializeField] protected Button _backButton;

	public bool BackButtonEnabled = true;
	public bool FullBackground;
	public bool UseShadow = true;
	public bool BannerEnabled = true;
	
	[NonSerialized]
	public bool FromResources;

	protected WindowsManager _windowsManager;
	protected Canvas _canvas;

	private bool _isInjected;

	public int SortingOrder {
		get => _canvas.sortingOrder;
		set => _canvas.sortingOrder = value;
	}

	public void InitializeWindow(WindowsManager manager) {
		_canvas = GetComponent<Canvas>();
		_windowsManager = manager;
		if (_backButton) _backButton.onClick.AddListener(OnBack);

		if (!_isInjected)
		{
			var componentsInChildren = GetComponentsInChildren<IAutoInjectable>(true);
			for (var i = 0; i < componentsInChildren.Length; i++)
			{
				_diContainer.Inject(componentsInChildren[i]);
			}

			_isInjected = true;
		}

	}

	public virtual void OnBack() { _windowsManager.CloseLast(); }

	public virtual void HideFast() { _canvas.enabled = false; }

	public virtual void ShowFast() { _canvas.enabled = true; }

	public virtual Tween Hide() { return DOTween.Sequence().AppendCallback(() => _canvas.enabled = false); }

	public virtual Tween Show() { return DOTween.Sequence().AppendCallback(() => _canvas.enabled = true); }

	public virtual void OnHided() { }
	public virtual void OnShowed() { }
	
	public virtual void OnStartHide() { }
	public virtual void OnStartShow() { }
}
