using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 52)]
public class GameEvent : ScriptableObject
{
	private List<GameEventListener> listeners = new List<GameEventListener>();
	public void Invoke()
	{
		foreach (var listener in listeners)
		{
			listener.OnEventRaised();
		}
	}

	public void RegisterListener(GameEventListener listener) => listeners.Add(listener);
	public void UnRegisterListener(GameEventListener listener) => listeners.Remove(listener);
}
