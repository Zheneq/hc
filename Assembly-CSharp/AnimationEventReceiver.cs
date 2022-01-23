using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationEventReceiver : MonoBehaviour, IAnimationEvents
{
	private ActorData m_actorData;

	private Renderer[] m_renderers;

	private Dictionary<string, string> m_surfaceFoleyEventNames = new Dictionary<string, string>();

	private GameObject m_attachmentsParent;

	private const bool c_muteAudioIfInvisibleToClient = true;

	private ActorData GetActorData()
	{
		if (m_actorData == null)
		{
			string text = string.Empty;
			GameObject gameObject = base.gameObject;
			while (m_actorData == null)
			{
				if (gameObject.transform.parent != null)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "/";
					}
					text += gameObject.name;
					m_actorData = gameObject.GetComponent<ActorData>();
					if (gameObject.GetComponent<UIActorModelData>() != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return null;
							}
						}
					}
					gameObject = gameObject.transform.parent.gameObject;
					continue;
				}
				break;
			}
			if (m_actorData == null)
			{
				Log.Error(base.gameObject.name + " AnimationEventReceiver failed to initialize properly, can't find ActorData in the hierarchy " + text);
			}
		}
		return m_actorData;
	}

	private Renderer[] GetRenderers()
	{
		if (m_renderers == null)
		{
			m_renderers = GetComponentsInChildren<Renderer>(true);
		}
		return m_renderers;
	}

	private void Start()
	{
		m_surfaceFoleyEventNames.Add("SFXTag_RunStepEvent", "fts_sw_surfacetype_run");
		m_surfaceFoleyEventNames.Add("SFXTag_ScuffEvent", "fts_sw_surfacetype_scuff");
		m_surfaceFoleyEventNames.Add("SFXTag_BodyFallEvent", "fts_sw_surfacetype_bodyfall");
		m_surfaceFoleyEventNames.Add("SFXTag_JumpLandEvent", "fts_sw_surfacetype_jumpland");
		m_surfaceFoleyEventNames.Add("SFXTag_HighJumpLandEvent", "ablty/bazookagirl/rocketjump_land");
		
		for (int num = 0; num < transform.childCount; num++)
        {
			Transform child = transform.GetChild(num);
			Rigidbody[] componentsInChildren = child.GetComponentsInChildren<Rigidbody>();
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				m_attachmentsParent = child.gameObject;
				break;
			}
		}
	}

	public void NewEvent(Object eventObject)
	{
		ProcessAnimationEvent(eventObject, base.gameObject);
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
		if (GetActorData() == null || !ShouldPostAudioEvent())
		{
			return;
		}
		while (true)
		{
			GetActorData().PostAnimationAudioEvent(eventName);
			return;
		}
	}

	private bool ShouldPostAudioEvent()
	{
		if (GetActorData() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					int result;
					if (NetworkClient.active)
					{
						if (0 == 0)
						{
							result = (GetActorData().IsActorVisibleToClient() ? 1 : 0);
						}
						else
						{
							result = 1;
						}
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}

	internal void ProcessAnimationEvent(Object eventObject, GameObject sourceObject)
	{
		if (GetActorData() == null)
		{
			return;
		}
		if (eventObject == null)
		{
			if (Application.isEditor)
			{
				string text = (!(GetActorData() == null) && !(GetActorData().GetActorModelData() == null)) ? GetActorData().GetActorModelData().GetCurrentAnimatorStateName() : "NULL";
				string text2 = (!(GetActorData() != null)) ? "UNKNOWN" : GetActorData().DebugNameString();
				Log.Error("Animation event on {0}'s animation state {1} is missing an Object in  the Unity Editor window for an animation event.  Please set the Object field to one of the scripts in the EventObjects folder, or change  the function name to AudioEvent, if the string field has an audio event name.", text2, text);
			}
			return;
		}
        if (ShouldPostAudioEvent() && m_surfaceFoleyEventNames.ContainsKey(eventObject.name))
        {
            AudioManager.PostEvent("sw_surfacetype", AudioManager.EventAction.SetSwitch, "metal");
            AudioManager.PostEvent(m_surfaceFoleyEventNames[eventObject.name], GetActorData().gameObject);
        }
        ActorData actorData = GetActorData();
		TheatricsManager.Get()?.OnAnimationEvent(actorData, eventObject, sourceObject);
		CameraManager.Get()?.OnAnimationEvent(actorData, eventObject);
		
		actorData.OnAnimEvent(eventObject, sourceObject);
        if (m_attachmentsParent != null)
        {
            if (eventObject.name == "VFX_ShowAttachments")
            {
                m_attachmentsParent.SetActive(true);
            }
            else if (eventObject.name == "VFX_HideAttachments")
            {
                m_attachmentsParent.SetActive(false);
            }
        }
    }

	public void ShowGeometryEvent(string eventName)
	{
		bool flag = false;
		Renderer[] renderers = GetRenderers();
		foreach (Renderer renderer in renderers)
		{
			if (eventName == renderer.name)
			{
				renderer.enabled = true;
				flag = true;
			}
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning($"ShowGeometryEvent: attempted to show object name '{eventName}', no such object found");
			return;
		}
	}

	public void HideGeometryEvent(string eventName)
	{
		bool flag = false;
		Renderer[] renderers = GetRenderers();
		foreach (Renderer renderer in renderers)
		{
			if (eventName == renderer.name)
			{
				renderer.enabled = false;
				flag = true;
			}
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning($"HideGeometryEvent: attempted to hide object name '{eventName}', no such object found");
			return;
		}
	}
}
