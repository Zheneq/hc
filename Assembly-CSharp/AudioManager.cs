using Fabric;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
	public enum EventAction
	{
		PlaySound,
		StopSound,
		PauseSound,
		UnpauseSound,
		SetVolume,
		SetPitch,
		SetSwitch,
		SetParameter,
		SetFadeIn,
		SetFadeOut,
		SetPan,
		AddPreset,
		RemovePreset,
		SetDSPParameter,
		RegisterGameObject,
		ResetDynamicMixer,
		AdvanceSequence,
		ResetSequence,
		SwitchPreset,
		SetTime,
		SetModularSynthParameter,
		StopAll
	}

	public static MixSnapshots mixSnapshotManager;

	public static bool s_deathAudio = true;

	public static bool s_pickupAudio = true;

	private const string c_unsetEventName = "_UnSet_";

	private const string c_warningTextOnUnsetUsed = "<color=white>    * AudioManager trying to post _UnSet_ event</color>";

	public static void SetMixerSnapshotManager(MixSnapshots snapShot)
	{
		mixSnapshotManager = snapShot;
	}

	public static MixSnapshots GetMixerSnapshotManager()
	{
		return mixSnapshotManager;
	}

	public static bool PostEvent(string eventName, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!ShouldSkipAudioEvents())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						EventManager instance = EventManager.Instance;
						int result;
						if (instance == null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							result = 0;
						}
						else
						{
							result = (instance.PostEvent(eventName, parentGameObject) ? 1 : 0);
						}
						return (byte)result != 0;
					}
					}
				}
			}
		}
		else if (Application.isEditor)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>");
		}
		return false;
	}

	public static bool PostEvent(string eventName, EventAction eventAction, object parameter = null, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!ShouldSkipAudioEvents())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						EventManager instance = EventManager.Instance;
						int result;
						if (instance == null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							result = 0;
						}
						else
						{
							result = (instance.PostEvent(eventName, (Fabric.EventAction)eventAction, parameter, parentGameObject) ? 1 : 0);
						}
						return (byte)result != 0;
					}
					}
				}
			}
		}
		else if (Application.isEditor)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>");
		}
		return false;
	}

	public static bool PostEventNotify(string eventName, OnEventNotify notifyCallback, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!ShouldSkipAudioEvents())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						EventManager instance = EventManager.Instance;
						return !(instance == null) && instance.PostEventNotify(eventName, parentGameObject, notifyCallback);
					}
					}
				}
			}
		}
		else if (Application.isEditor)
		{
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>");
		}
		return false;
	}

	public static bool PostEventNotify(string eventName, EventAction eventAction, OnEventNotify notifyCallback, object parameter = null, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
		{
			if (!ShouldSkipAudioEvents())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						EventManager instance = EventManager.Instance;
						int result;
						if (instance == null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							result = 0;
						}
						else
						{
							result = (instance.PostEventNotify(eventName, (Fabric.EventAction)eventAction, parameter, parentGameObject, notifyCallback) ? 1 : 0);
						}
						return (byte)result != 0;
					}
					}
				}
			}
		}
		else if (Application.isEditor)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>");
		}
		return false;
	}

	public static bool ShouldSkipAudioEvents()
	{
		return false;
	}

	public static void EnableMusicAtStartup()
	{
		GameObject gameObject = GameObject.Find("_AUDIO_INITIALIZE");
		List<EventTrigger> list = new List<EventTrigger>();
		gameObject.GetComponentsInChildren(true, list);
		foreach (EventTrigger item in list)
		{
			if (item._eventName == "start_music")
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!item.enabled)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					item.enabled = true;
				}
			}
		}
	}

	public static void EnableAmbianceAtStartup()
	{
		GameObject gameObject = GameObject.Find("_AUDIO_INITIALIZE");
		List<EventTrigger> list = new List<EventTrigger>();
		gameObject.GetComponentsInChildren(true, list);
		using (List<EventTrigger>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EventTrigger current = enumerator.Current;
				if (current._eventName == "start_ambiance" && !current.enabled)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					current.enabled = true;
				}
			}
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
	}

	public static bool StandardizeAudioLinkages_NeedsUpdate(AudioMixerGroup mixerGroup)
	{
		if (mixerGroup == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (GameWideData.Get() == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (mixerGroup.audioMixer == GetMixerSnapshotManager().m_mixer)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public static AudioMixerGroup StandardizeAudioLinkages_Update(AudioMixerGroup mixerGroup)
	{
		if (mixerGroup == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		if (GameWideData.Get() == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return mixerGroup;
				}
			}
		}
		AudioMixerGroup[] array = GetMixerSnapshotManager().m_mixer.FindMatchingGroups(mixerGroup.name);
		AudioMixerGroup audioMixerGroup = null;
		if (array != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			AudioMixerGroup[] array2 = array;
			foreach (AudioMixerGroup audioMixerGroup2 in array2)
			{
				if (audioMixerGroup2.name == mixerGroup.name)
				{
					audioMixerGroup = audioMixerGroup2;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (audioMixerGroup == null)
		{
			audioMixerGroup = mixerGroup;
		}
		return audioMixerGroup;
	}

	public static IEnumerable<T> FindAllSceneObjectsOfType<T>()
	{
		Transform[] array = Object.FindObjectsOfType<Transform>();
		foreach (Transform ts in array)
		{
			if (ts.parent == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				T[] componentsInChildren = ts.GetComponentsInChildren<T>(true);
				int num = 0;
				if (num < componentsInChildren.Length)
				{
					yield return componentsInChildren[num];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				yield break;
			case 0:
				break;
			}
		}
	}

	public static void StandardizeAudioLinkages()
	{
		IEnumerator<Fabric.Component> enumerator = FindAllSceneObjectsOfType<Fabric.Component>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Fabric.Component current = enumerator.Current;
				if (StandardizeAudioLinkages_NeedsUpdate(current._audioMixerGroup))
				{
					current._audioMixerGroup = StandardizeAudioLinkages_Update(current._audioMixerGroup);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_000d;
				}
			}
			end_IL_000d:;
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0059;
					}
				}
			}
			end_IL_0059:;
		}
		IEnumerator<AudioSource> enumerator2 = FindAllSceneObjectsOfType<AudioSource>().GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				AudioSource current2 = enumerator2.Current;
				if (StandardizeAudioLinkages_NeedsUpdate(current2.outputAudioMixerGroup))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					current2.outputAudioMixerGroup = StandardizeAudioLinkages_Update(current2.outputAudioMixerGroup);
				}
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		finally
		{
			if (enumerator2 != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						enumerator2.Dispose();
						goto end_IL_00c5;
					}
				}
			}
			end_IL_00c5:;
		}
	}

	public static void StandardizeAudioLinkages(GameObject root)
	{
		Fabric.Component[] componentsInChildren = root.GetComponentsInChildren<Fabric.Component>();
		foreach (Fabric.Component component in componentsInChildren)
		{
			if (StandardizeAudioLinkages_NeedsUpdate(component._audioMixerGroup))
			{
				component._audioMixerGroup = StandardizeAudioLinkages_Update(component._audioMixerGroup);
			}
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
			AudioSource[] componentsInChildren2 = root.GetComponentsInChildren<AudioSource>();
			foreach (AudioSource audioSource in componentsInChildren2)
			{
				if (StandardizeAudioLinkages_NeedsUpdate(audioSource.outputAudioMixerGroup))
				{
					audioSource.outputAudioMixerGroup = StandardizeAudioLinkages_Update(audioSource.outputAudioMixerGroup);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public static void TransitionToOnRealMixer(this AudioMixerSnapshot snapshot, float transitionTime)
	{
		if (GetMixerSnapshotManager().m_mixer == null)
		{
			snapshot.TransitionTo(transitionTime);
			return;
		}
		AudioMixerSnapshot audioMixerSnapshot = GetMixerSnapshotManager().m_mixer.FindSnapshot(snapshot.name);
		if (audioMixerSnapshot == null)
		{
			snapshot.TransitionTo(transitionTime);
		}
		else
		{
			audioMixerSnapshot.TransitionTo(transitionTime);
		}
	}
}
