using System;
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
		if (this.m_actorData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.GetActorData()).MethodHandle;
			}
			string text = string.Empty;
			GameObject gameObject = base.gameObject;
			while (this.m_actorData == null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(gameObject.transform.parent != null))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_D9;
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "/";
					}
					text += gameObject.name;
					this.m_actorData = gameObject.GetComponent<ActorData>();
					if (gameObject.GetComponent<UIActorModelData>() != null)
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
						return null;
					}
					gameObject = gameObject.transform.parent.gameObject;
				}
			}
			IL_D9:
			if (this.m_actorData == null)
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
				Log.Error(base.gameObject.name + " AnimationEventReceiver failed to initialize properly, can't find ActorData in the hierarchy " + text, new object[0]);
			}
		}
		return this.m_actorData;
	}

	private Renderer[] GetRenderers()
	{
		if (this.m_renderers == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.GetRenderers()).MethodHandle;
			}
			this.m_renderers = base.GetComponentsInChildren<Renderer>(true);
		}
		return this.m_renderers;
	}

	private void Start()
	{
		this.m_surfaceFoleyEventNames.Add("SFXTag_RunStepEvent", "fts_sw_surfacetype_run");
		this.m_surfaceFoleyEventNames.Add("SFXTag_ScuffEvent", "fts_sw_surfacetype_scuff");
		this.m_surfaceFoleyEventNames.Add("SFXTag_BodyFallEvent", "fts_sw_surfacetype_bodyfall");
		this.m_surfaceFoleyEventNames.Add("SFXTag_JumpLandEvent", "fts_sw_surfacetype_jumpland");
		this.m_surfaceFoleyEventNames.Add("SFXTag_HighJumpLandEvent", "ablty/bazookagirl/rocketjump_land");
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			Rigidbody[] componentsInChildren = child.GetComponentsInChildren<Rigidbody>();
			if (componentsInChildren != null && componentsInChildren.Length > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.Start()).MethodHandle;
				}
				this.m_attachmentsParent = child.gameObject;
				break;
			}
		}
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

	public void AudioEvent(string eventName)
	{
		if (this.GetActorData() == null)
		{
			return;
		}
		if (this.ShouldPostAudioEvent())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.AudioEvent(string)).MethodHandle;
			}
			this.GetActorData().PostAnimationAudioEvent(eventName);
		}
	}

	private bool ShouldPostAudioEvent()
	{
		if (this.GetActorData() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.ShouldPostAudioEvent()).MethodHandle;
			}
			bool result;
			if (NetworkClient.active)
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
				if (!false)
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
					result = this.GetActorData().IsVisibleToClient();
				}
				else
				{
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	internal void ProcessAnimationEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (this.GetActorData() == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.ProcessAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			return;
		}
		if (eventObject == null)
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
			if (Application.isEditor)
			{
				string text = (!(this.GetActorData() == null) && !(this.GetActorData().GetActorModelData() == null)) ? this.GetActorData().GetActorModelData().GetCurrentAnimatorStateName() : "NULL";
				string text2 = (!(this.GetActorData() != null)) ? "UNKNOWN" : this.GetActorData().GetDebugName();
				Log.Error("Animation event on {0}'s animation state {1} is missing an Object in  the Unity Editor window for an animation event.  Please set the Object field to one of the scripts in the EventObjects folder, or change  the function name to AudioEvent, if the string field has an audio event name.", new object[]
				{
					text2,
					text
				});
			}
			return;
		}
		if (this.ShouldPostAudioEvent())
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
			if (this.m_surfaceFoleyEventNames.ContainsKey(eventObject.name))
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
				AudioManager.PostEvent("sw_surfacetype", AudioManager.EventAction.SetSwitch, "metal", null);
				AudioManager.PostEvent(this.m_surfaceFoleyEventNames[eventObject.name], this.GetActorData().gameObject);
			}
		}
		ActorData actorData = this.GetActorData();
		if (TheatricsManager.Get() != null)
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
			TheatricsManager.Get().OnAnimationEvent(actorData, eventObject, sourceObject);
		}
		if (CameraManager.Get() != null)
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
			CameraManager.Get().OnAnimationEvent(actorData, eventObject);
		}
		actorData.OnAnimEvent(eventObject, sourceObject);
		if (this.m_attachmentsParent != null)
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
			if (eventObject.name == "VFX_ShowAttachments")
			{
				this.m_attachmentsParent.SetActive(true);
			}
			else if (eventObject.name == "VFX_HideAttachments")
			{
				this.m_attachmentsParent.SetActive(false);
			}
		}
	}

	public void ShowGeometryEvent(string eventName)
	{
		bool flag = false;
		foreach (Renderer renderer in this.GetRenderers())
		{
			if (eventName == renderer.name)
			{
				renderer.enabled = true;
				flag = true;
			}
		}
		if (!flag)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.ShowGeometryEvent(string)).MethodHandle;
			}
			Debug.LogWarning(string.Format("ShowGeometryEvent: attempted to show object name '{0}', no such object found", eventName));
		}
	}

	public void HideGeometryEvent(string eventName)
	{
		bool flag = false;
		foreach (Renderer renderer in this.GetRenderers())
		{
			if (eventName == renderer.name)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AnimationEventReceiver.HideGeometryEvent(string)).MethodHandle;
				}
				renderer.enabled = false;
				flag = true;
			}
		}
		if (!flag)
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
			Debug.LogWarning(string.Format("HideGeometryEvent: attempted to hide object name '{0}', no such object found", eventName));
		}
	}
}
