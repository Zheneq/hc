using System;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndAnimationEventReceiver : MonoBehaviour, IAnimationEvents
{
	private Dictionary<string, string> m_surfaceFoleyEventNames = new Dictionary<string, string>();

	private void Start()
	{
		this.m_surfaceFoleyEventNames.Add("SFXTag_RunStepEvent", "fts_sw_surfacetype_run");
		this.m_surfaceFoleyEventNames.Add("SFXTag_ScuffEvent", "fts_sw_surfacetype_scuff");
		this.m_surfaceFoleyEventNames.Add("SFXTag_BodyFallEvent", "fts_sw_surfacetype_bodyfall");
		this.m_surfaceFoleyEventNames.Add("SFXTag_JumpLandEvent", "fts_sw_surfacetype_jumpland");
		this.m_surfaceFoleyEventNames.Add("SFXTag_HighJumpLandEvent", "ablty/bazookagirl/rocketjump_land");
	}

	public void NewEvent(UnityEngine.Object eventObject)
	{
		this.ProcessAnimationEvent(eventObject, base.gameObject);
	}

	public void VFXEvent(UnityEngine.Object eventObject)
	{
		this.NewEvent(eventObject);
	}

	public void GameplayEvent(UnityEngine.Object eventObject)
	{
		this.NewEvent(eventObject);
	}

	private void ProcessAnimationEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (eventObject == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndAnimationEventReceiver.ProcessAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			if (Application.isEditor)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Error("Animation event on {0} is missing an Object in  the Unity Editor window for an animation event.  Please set the Object field to one of the scripts in the EventObjects folder, or change  the function name to AudioEvent, if the string field has an audio event name.", new object[]
				{
					base.gameObject.name
				});
			}
			return;
		}
		if (this.m_surfaceFoleyEventNames.ContainsKey(eventObject.name))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			AudioManager.PostEvent("sw_surfacetype", AudioManager.EventAction.SetSwitch, "metal", null);
			AudioManager.PostEvent(this.m_surfaceFoleyEventNames[eventObject.name], base.gameObject);
		}
	}

	public void AudioEvent(string eventName)
	{
		if (eventName != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndAnimationEventReceiver.AudioEvent(string)).MethodHandle;
			}
			if (!eventName.StartsWith("fol/"))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				AudioManager.PostEvent(eventName, base.gameObject);
			}
		}
	}

	public void SubtitleEvent(string eventName)
	{
		if (UITutorialPanel.Get())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndAnimationEventReceiver.SubtitleEvent(string)).MethodHandle;
			}
			string[] array = eventName.Split(new char[]
			{
				','
			});
			if (array.Length == 3)
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
				string subtitleText = array[0];
				float timeToDisplay = 0f;
				float.TryParse(array[1], out timeToDisplay);
				CharacterType characterType = (CharacterType)Enum.Parse(typeof(CharacterType), array[2]);
				UITutorialPanel.Get().QueueDialogue(subtitleText, null, timeToDisplay, characterType);
			}
		}
	}
}
