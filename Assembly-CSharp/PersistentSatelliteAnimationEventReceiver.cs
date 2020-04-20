using System;
using UnityEngine;

public class PersistentSatelliteAnimationEventReceiver : MonoBehaviour, IAnimationEvents
{
	private PersistentSatellite m_persistentSatellite;

	public void Setup(PersistentSatellite persistentSatellite)
	{
		this.m_persistentSatellite = persistentSatellite;
	}

	public void NewEvent(UnityEngine.Object eventObject)
	{
		if (this.m_persistentSatellite == null)
		{
			Log.Error(this + " NewEvent called before setup", new object[0]);
			return;
		}
		if (eventObject == null)
		{
			return;
		}
		this.m_persistentSatellite.OnAnimationEvent(eventObject);
	}

	public void VFXEvent(UnityEngine.Object eventObject)
	{
		this.NewEvent(eventObject);
	}

	public void GameplayEvent(UnityEngine.Object eventObject)
	{
		this.NewEvent(eventObject);
	}

	public void OnAnimationEvent(GameObject eventObject)
	{
		this.m_persistentSatellite.OnAnimationEvent(eventObject);
	}

	public void AudioEvent(string eventName)
	{
		if (this.m_persistentSatellite == null)
		{
			Log.Error(this + " NewEvent called before Start", new object[0]);
			return;
		}
		AudioManager.PostEvent(eventName, this.m_persistentSatellite.gameObject);
	}
}
