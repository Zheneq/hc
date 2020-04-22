using UnityEngine;

public class TempSatelliteAnimationEventReceiver : MonoBehaviour, IAnimationEvents
{
	public interface IOwner
	{
		void OnAnimationEvent(Object eventObject);

		GameObject GetGameObject();
	}

	private IOwner m_owner;

	public void Setup(IOwner owner)
	{
		m_owner = owner;
	}

	public void NewEvent(Object eventObject)
	{
		if (m_owner == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Log.Error(string.Concat(this, " NewEvent called before setup"));
					return;
				}
			}
		}
		if (!(eventObject == null))
		{
			m_owner.OnAnimationEvent(eventObject);
		}
	}

	public void VFXEvent(Object eventObject)
	{
		NewEvent(eventObject);
	}

	public void GameplayEvent(Object eventObject)
	{
		NewEvent(eventObject);
	}

	public void AudioEvent(string eventName)
	{
		if (m_owner == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Log.Error(string.Concat(this, " NewEvent called before Start"));
					return;
				}
			}
		}
		AudioManager.PostEvent(eventName, m_owner.GetGameObject());
	}
}
