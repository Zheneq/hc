using System;
using UnityEngine;

public class TempSatelliteAnimationEventReceiver : MonoBehaviour, IAnimationEvents
{
	private TempSatelliteAnimationEventReceiver.IOwner m_owner;

	public void Setup(TempSatelliteAnimationEventReceiver.IOwner owner)
	{
		this.m_owner = owner;
	}

	public void NewEvent(UnityEngine.Object eventObject)
	{
		if (this.m_owner == null)
		{
			Log.Error(this + " NewEvent called before setup", new object[0]);
			return;
		}
		if (eventObject == null)
		{
			return;
		}
		this.m_owner.OnAnimationEvent(eventObject);
	}

	public void VFXEvent(UnityEngine.Object eventObject)
	{
		this.NewEvent(eventObject);
	}

	public void GameplayEvent(UnityEngine.Object eventObject)
	{
		this.NewEvent(eventObject);
	}

	public void AudioEvent(string eventName)
	{
		if (this.m_owner == null)
		{
			Log.Error(this + " NewEvent called before Start", new object[0]);
			return;
		}
		AudioManager.PostEvent(eventName, this.m_owner.GetGameObject());
	}

	public interface IOwner
	{
		void OnAnimationEvent(UnityEngine.Object eventObject);

		GameObject GetGameObject();
	}
}
