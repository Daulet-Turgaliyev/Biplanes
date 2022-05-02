using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class WindowsManager: MonoBehaviour {
    private const int MIN_ORDER = 10;
    
    [Inject] private BackButton _backButton;
    [Inject] private DiContainer _diContainer;
    [Inject] private GlobalSettings.Screen _screen;
    
    [SerializeField]
    private Canvas _mainCanvas;
    
    public static bool IsInited { get; private set; }
    public static event Action OnInited;

    private readonly Stack<BaseWindow> _windows = new Stack<BaseWindow>();
    private readonly Dictionary<string, BaseWindow> _cacheWindows = new Dictionary<string, BaseWindow>();
    
    private int _order;

    private Sequence _showTween;
    private Sequence _closeTween;

    private void Awake() {
        _mainCanvas.enabled = false;
        _order = MIN_ORDER;

        SceneManager.sceneLoaded += (scene, mode) => {
            DOTween.Sequence().AppendInterval(2).OnComplete(() => {
                if (scene.buildIndex >= SceneManager.sceneCount)
                {
                    IsInited = true;
                    OnInited?.Invoke();
                    OnInited = null;
                }
            });
        };
    }

    private void Start() {
        _backButton.OnBack += BackButtonCall;
    }

    private void OnDestroy() {
        _backButton.OnBack -= BackButtonCall;
    }

    private void BackButtonCall() {
        if (_windows.Count > 0) {
            var window = _windows.Peek();
            if (window.BackButtonEnabled) {
                window.OnBack();
            }
        }
    }

    private void DisableInterations() {
        _screen.DisableInteractions();
    }

    private void EnableInterations() {
        _screen.EnableInteractions(false);
    }

    private Tween Close(BaseWindow window) {
        _closeTween = DOTween.Sequence();
        if (window == null) return _closeTween;
        
        _closeTween.AppendCallback(DisableInterations);

        window.OnStartHide();
        _closeTween.Append(window.Hide());
        _closeTween.AppendCallback(window.OnHided);

        if (_windows.Count > 0) {
            var newWindow = _windows.Peek();
            if (newWindow.FullBackground) {
                _closeTween.AppendCallback(() => {
                    newWindow.OnStartShow();
                    newWindow.OnShowed();
                    
                    _order -= 2;
                });
            } else {
                _closeTween.AppendCallback(() => {
                    _order -= 2;
                });
                _closeTween.Append(Show(newWindow, true));
            }
        } else {

            _closeTween.AppendCallback(LastClosed);
            
            _closeTween.AppendCallback(() => {
                _order -= 2;
            });
        }
        
        _closeTween.AppendCallback(EnableInterations);

        return _closeTween;
    }

    private void LastClosed() {
        // Закрывать главный канвас?
        //_mainCanvas.enabled = false;
    }

    private void FirstOpened(BaseWindow window) {
        _mainCanvas.enabled = true;
    }

    private void IncOrder(Canvas canvas) {
        canvas.sortingOrder = _order;
        _order++;
    }

    private void IncOrder(BaseWindow window) {
        window.SortingOrder = _order;
        _order++;
    }
    
    private Tween Show(BaseWindow window, bool reopen = false) {
        _showTween = DOTween.Sequence();
        if (window == null) return _showTween;
        
        _showTween.AppendCallback(DisableInterations);
        _showTween.AppendCallback(() => {
            if (reopen) return;
            IncOrder(window);
        });
        
        if (_windows.Count == 0) {
            _showTween.AppendCallback(()=> FirstOpened(window));
            
        } else {
            var was = _windows.Peek();
            if (!was.FullBackground) {
                _showTween.Append(was.Hide());
            }
        }
        
        window.OnStartShow();
        
        _showTween.Append(window.Show());
        _showTween.AppendCallback(window.OnShowed);
        _showTween.AppendCallback(EnableInterations);
        
        return _showTween;
    }

    public Tween CloseLast() {
        if (_windows.Count == 0) return DOTween.Sequence();
        return Close(_windows.Pop());
    }

    // TODO: remove break showing/closing window animation outside, if not need
    public Tween CloseAll(bool competeActiveShowCloseAnimation = true) {
        if (competeActiveShowCloseAnimation) {
            _closeTween.Complete(true);
            _showTween.Complete(true);
        }
        
        var sequence = DOTween.Sequence();
        while (_windows.Count > 0) {
            sequence.Append(CloseLast());
        }

        return sequence;
    }

    public void DestroyAll()
    {
        foreach (var window in _windows)
        {
            Destroy(window.gameObject);
        }
    }
    
    public Tween ShowWindow(BaseWindow window) {
        if (window == null) return DOTween.Sequence();
        
        if (_windows.Count > 0 && window == _windows.Peek()) return DOTween.Sequence();
        var tween = Show(window);
        _windows.Push(window);
        return tween;
    }
    
    public T OpenWindow<T>() where T: BaseWindow {
        T window = GetWindow<T>();
        ShowWindow(window);
        return window;
    }

    public T GetWindow<T>() where T: BaseWindow {
        string windowName = typeof(T).ToString();
        T window;

        if (_cacheWindows.ContainsKey(windowName)) {
            window = (T)_cacheWindows[windowName];
            window.FromResources = false;
            return window;
        }

        T windowPrefab = null;
        var clearWindowName = windowName.Replace("_2", string.Empty);
        if (windowPrefab == null)
        {
            windowPrefab = Resources.Load<T>("Windows/" + clearWindowName);
        }
        Debug.Log(windowPrefab);
        if (windowPrefab == null)
        {
            Debug.LogError($"{typeof(T)} not found in Resources");
            return null;
        }

        return InstantiateWindow(windowPrefab, windowName);
    }

    public T InstantiateWindow<T>(T windowPrefab, string windowName) where T : BaseWindow
    {
        T window = Instantiate(windowPrefab, _mainCanvas.transform, false);
#if UNITY_EDITOR
        if (windowName.Contains("_2")) window.name += "_2";
#endif
        
        _diContainer.Inject(window);
        window.InitializeWindow(this);
        _cacheWindows[windowName] = window;
        window.HideFast();
        window.FromResources = true;
        return window;
    }

    public bool IsOpened<T>() where T : BaseWindow
    {
        return _windows.Any(window => window is T);
    }

    public bool IsOpenedAny() => _windows.Count > 0;
}
