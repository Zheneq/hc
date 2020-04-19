using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISounds : MonoBehaviour
{
	public bool m_enableSounds = true;

	private static UISounds s_instance;

	public static UISounds GetUISounds()
	{
		return UISounds.s_instance;
	}

	private void Awake()
	{
		if (UISounds.s_instance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISounds.Awake()).MethodHandle;
			}
			UISounds.s_instance = this;
		}
		else
		{
			Log.Warning("Please remove UISounds component from scene: {0}.unity", new object[]
			{
				SceneManager.GetActiveScene().name
			});
		}
	}

	private void OnDestroy()
	{
		if (UISounds.s_instance != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISounds.OnDestroy()).MethodHandle;
			}
			if (UISounds.s_instance == this)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				UISounds.s_instance = null;
			}
		}
	}

	public void Play(string eventName)
	{
		if (this.m_enableSounds)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISounds.Play(string)).MethodHandle;
			}
			AudioManager.PostEvent(eventName, null);
		}
	}

	public void Stop(string eventName)
	{
		AudioManager.PostEvent(eventName, AudioManager.EventAction.StopSound, null, null);
	}
}
