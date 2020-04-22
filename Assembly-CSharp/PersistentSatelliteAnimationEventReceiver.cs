using UnityEngine;

public class PersistentSatelliteAnimationEventReceiver : MonoBehaviour, IAnimationEvents
{
	private PersistentSatellite m_persistentSatellite;

	public void Setup(PersistentSatellite persistentSatellite)
	{
		m_persistentSatellite = persistentSatellite;
	}

	public void NewEvent(Object eventObject)
	{
		if (m_persistentSatellite == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Error(string.Concat(this, " NewEvent called before setup"));
					return;
				}
			}
		}
		if (eventObject == null)
		{
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		m_persistentSatellite.OnAnimationEvent(eventObject);
	}

	public void VFXEvent(Object eventObject)
	{
		NewEvent(eventObject);
	}

	public void GameplayEvent(Object eventObject)
	{
		NewEvent(eventObject);
	}

	public void OnAnimationEvent(GameObject eventObject)
	{
		m_persistentSatellite.OnAnimationEvent(eventObject);
	}

	public void AudioEvent(string eventName)
	{
		if (m_persistentSatellite == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Error(string.Concat(this, " NewEvent called before Start"));
					return;
				}
			}
		}
		AudioManager.PostEvent(eventName, m_persistentSatellite.gameObject);
	}
}
