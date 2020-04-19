using System;
using System.Collections.Generic;
using Fabric;
using UnityEngine;

public class AudioWarningCleanup : MonoBehaviour, IEventListener
{
	public string[] m_warningsToIgnore;

	[HideInInspector]
	private bool m_registering;

	private void Start()
	{
		this.m_registering = true;
		if (EventManager.Instance != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioWarningCleanup.Start()).MethodHandle;
			}
			foreach (string eventName in this.m_warningsToIgnore)
			{
				EventManager.Instance.RegisterListener(this, eventName);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_registering = false;
	}

	private void OnDestroy()
	{
		this.m_registering = true;
		if (EventManager.Instance != null)
		{
			foreach (string eventName in this.m_warningsToIgnore)
			{
				EventManager.Instance.UnregisterListener(this, eventName);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioWarningCleanup.OnDestroy()).MethodHandle;
			}
		}
		this.m_registering = false;
	}

	bool IEventListener.IsDestroyed
	{
		get
		{
			return !this.m_registering;
		}
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
