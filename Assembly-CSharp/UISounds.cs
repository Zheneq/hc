using UnityEngine;
using UnityEngine.SceneManagement;

public class UISounds : MonoBehaviour
{
	public bool m_enableSounds = true;

	private static UISounds s_instance;

	public static UISounds GetUISounds()
	{
		return s_instance;
	}

	private void Awake()
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					s_instance = this;
					return;
				}
			}
		}
		Log.Warning("Please remove UISounds component from scene: {0}.unity", SceneManager.GetActiveScene().name);
	}

	private void OnDestroy()
	{
		if (!(s_instance != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (s_instance == this)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					s_instance = null;
					return;
				}
			}
			return;
		}
	}

	public void Play(string eventName)
	{
		if (!m_enableSounds)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AudioManager.PostEvent(eventName);
			return;
		}
	}

	public void Stop(string eventName)
	{
		AudioManager.PostEvent(eventName, AudioManager.EventAction.StopSound);
	}
}
