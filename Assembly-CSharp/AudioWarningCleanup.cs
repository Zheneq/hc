using Fabric;
using System.Collections.Generic;
using UnityEngine;

public class AudioWarningCleanup : MonoBehaviour, IEventListener
{
	public string[] m_warningsToIgnore;

	[HideInInspector]
	private bool m_registering;

	bool IEventListener.IsDestroyed
	{
		get { return !m_registering; }
	}

	private void Start()
	{
		m_registering = true;
		if (EventManager.Instance != null)
		{
			string[] warningsToIgnore = m_warningsToIgnore;
			foreach (string eventName in warningsToIgnore)
			{
				EventManager.Instance.RegisterListener(this, eventName);
			}
		}
		m_registering = false;
	}

	private void OnDestroy()
	{
		m_registering = true;
		if (EventManager.Instance != null)
		{
			string[] warningsToIgnore = m_warningsToIgnore;
			foreach (string eventName in warningsToIgnore)
			{
				EventManager.Instance.UnregisterListener(this, eventName);
			}
		}
		m_registering = false;
	}

	bool IEventListener.GetEventInfo(GameObject parentGameObject, ref EventInfo eventInfo)
	{
		return false;
	}

	bool IEventListener.GetEventListeners(string eventName, List<EventListener> listeners)
	{
		return false;
	}

	bool IEventListener.GetEventListeners(int eventID, List<EventListener> listeners)
	{
		return false;
	}

	bool IEventListener.IsActive(GameObject parentGameObject)
	{
		return base.gameObject.activeSelf;
	}

	EventStatus IEventListener.Process(Fabric.Event postedEvent)
	{
		return EventStatus.Handled;
	}
}
