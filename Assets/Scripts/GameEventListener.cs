using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { }
public class GameEventListener : MonoBehaviour
{
	[SerializeField]
	private GameEvent gameEvent;

	[SerializeField]
	private GameObjectEvent responseEvent;

	private void OnEnable()
	{
		gameEvent.RegisterListener(this);
	}
	private void OnDisable()
	{
		gameEvent.UnRegisterListener(this);
	}
	public void OnEventRaised()
	{
		//foreach (var response in responseEvents)
		//{
		//	response.Invoke();
		//}
		responseEvent.Invoke(gameObject);


	}
}
