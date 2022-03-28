using System.Collections.Generic;
using UnityEngine;


public class WindowsManager: MonoBehaviour
{

	[SerializeField]
	private List<BaseWindow> _windows;

	[SerializeField] 
	private Transform _canvasTransform;
	
	public GameObject OpenWindow(GameEntity gameEntity)
	{
		int windowIndex = (int)gameEntity;
		
		var newWindow = Instantiate(_windows[windowIndex].gameObject, parent: _canvasTransform);
		return newWindow;
	}
}

public enum GameEntity
{
	Plane,
	Pilot
}