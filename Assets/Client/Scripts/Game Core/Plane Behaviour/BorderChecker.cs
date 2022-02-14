using UnityEngine;

public sealed class BorderChecker : MonoBehaviour
{
    private float _xBorder;
    private float _currentHorizontalPosition;

    private const float BORDER_OFFSET = .5f; 
    
    private void Start()
    {
        var main = Camera.main;
        if (main == null) return;
        _xBorder = main.orthographicSize * main.aspect;
    }

    private void Update()
    {
        CheckBorder();
    }

    private void CheckBorder()
    {
        _currentHorizontalPosition = transform.position.x;
        
        if (IsWithinBorder() == true) return;
        
        Vector3 teleportToNewPosition = _currentHorizontalPosition > 0 ? new Vector2(-_currentHorizontalPosition + BORDER_OFFSET, transform.position.y) : 
                                                                         new Vector2(Mathf.Abs(_currentHorizontalPosition) - BORDER_OFFSET, transform.position.y);
        
        
        transform.position = teleportToNewPosition;
    }

    private bool IsWithinBorder()
    {
        if (_currentHorizontalPosition < _xBorder && _currentHorizontalPosition > -_xBorder)
            return true;

        return false;
    }
}
