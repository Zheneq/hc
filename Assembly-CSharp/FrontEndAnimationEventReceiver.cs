using System;
using System.Collections.Generic;
using UnityEngine;

public class FrontEndAnimationEventReceiver : MonoBehaviour, IAnimationEvents
{
	private Dictionary<string, string> m_surfaceFoleyEventNames = new Dictionary<string, string>();

	private void Start()
	{
		m_surfaceFoleyEventNames.Add("SFXTag_RunStepEvent", "fts_sw_surfacetype_run");
		m_surfaceFoleyEventNames.Add("SFXTag_ScuffEvent", "fts_sw_surfacetype_scuff");
		m_surfaceFoleyEventNames.Add("SFXTag_BodyFallEvent", "fts_sw_surfacetype_bodyfall");
		m_surfaceFoleyEventNames.Add("SFXTag_JumpLandEvent", "fts_sw_surfacetype_jumpland");
		m_surfaceFoleyEventNames.Add("SFXTag_HighJumpLandEvent", "ablty/bazookagirl/rocketjump_land");
	}

	public void NewEvent(UnityEngine.Object eventObject)
	{
		ProcessAnimationEvent(eventObject, base.gameObject);
	}

	public void VFXEvent(UnityEngine.Object eventObject)
	{
		NewEvent(eventObject);
	}

	public void GameplayEvent(UnityEngine.Object eventObject)
	{
		NewEvent(eventObject);
	}

	private void ProcessAnimationEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (eventObject == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (Application.isEditor)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								Log.Error("Animation event on {0} is missing an Object in  the Unity Editor window for an animation event.  Please set the Object field to one of the scripts in the EventObjects folder, or change  the function name to AudioEvent, if the string field has an audio event name.", base.gameObject.name);
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (!m_surfaceFoleyEventNames.ContainsKey(eventObject.name))
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent("sw_surfacetype", AudioManager.EventAction.SetSwitch, "metal");
			AudioManager.PostEvent(m_surfaceFoleyEventNames[eventObject.name], base.gameObject);
			return;
		}
	}

	public void AudioEvent(string eventName)
	{
		if (eventName == null)
		{
			return;
		}
		while (true)
		{
			if (!eventName.StartsWith("fol/"))
			{
				while (true)
				{
					AudioManager.PostEvent(eventName, base.gameObject);
					return;
				}
			}
			return;
		}
	}

	public void SubtitleEvent(string eventName)
	{
		if (!UITutorialPanel.Get())
		{
			return;
		}
		while (true)
		{
			string[] array = eventName.Split(',');
			if (array.Length == 3)
			{
				while (true)
				{
					string subtitleText = array[0];
					float result = 0f;
					float.TryParse(array[1], out result);
					CharacterType characterType = (CharacterType)Enum.Parse(typeof(CharacterType), array[2]);
					UITutorialPanel.Get().QueueDialogue(subtitleText, null, result, characterType);
					return;
				}
			}
			return;
		}
	}
}
