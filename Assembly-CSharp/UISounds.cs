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
			if (UISounds.s_instance == this)
			{
				UISounds.s_instance = null;
			}
		}
	}

	public void Play(string eventName)
	{
		if (this.m_enableSounds)
		{
			AudioManager.PostEvent(eventName, null);
		}
	}

	public void Stop(string eventName)
	{
		AudioManager.PostEvent(eventName, AudioManager.EventAction.StopSound, null, null);
	}
}
