using System;
using System.Collections.Generic;
using Fabric;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
	public static MixSnapshots mixSnapshotManager;

	public static bool s_deathAudio = true;

	public static bool s_pickupAudio = true;

	private const string c_unsetEventName = "_UnSet_";

	private const string c_warningTextOnUnsetUsed = "<color=white>    * AudioManager trying to post _UnSet_ event</color>";

	public static void SetMixerSnapshotManager(MixSnapshots snapShot)
	{
		AudioManager.mixSnapshotManager = snapShot;
	}

	public static MixSnapshots GetMixerSnapshotManager()
	{
		return AudioManager.mixSnapshotManager;
	}

	public static bool PostEvent(string eventName, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.PostEvent(string, GameObject)).MethodHandle;
			}
			if (!AudioManager.ShouldSkipAudioEvents())
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
				EventManager instance = EventManager.Instance;
				bool result;
				if (instance == null)
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
					result = false;
				}
				else
				{
					result = instance.PostEvent(eventName, parentGameObject);
				}
				return result;
			}
		}
		else if (Application.isEditor)
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
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>", new object[0]);
		}
		return false;
	}

	public static bool PostEvent(string eventName, AudioManager.EventAction eventAction, object parameter = null, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.PostEvent(string, AudioManager.EventAction, object, GameObject)).MethodHandle;
			}
			if (!AudioManager.ShouldSkipAudioEvents())
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
				EventManager instance = EventManager.Instance;
				bool result;
				if (instance == null)
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
					result = false;
				}
				else
				{
					result = instance.PostEvent(eventName, (Fabric.EventAction)eventAction, parameter, parentGameObject);
				}
				return result;
			}
		}
		else if (Application.isEditor)
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
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>", new object[0]);
		}
		return false;
	}

	public static bool PostEventNotify(string eventName, OnEventNotify notifyCallback, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.PostEventNotify(string, OnEventNotify, GameObject)).MethodHandle;
			}
			if (!AudioManager.ShouldSkipAudioEvents())
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
				EventManager instance = EventManager.Instance;
				return !(instance == null) && instance.PostEventNotify(eventName, parentGameObject, notifyCallback);
			}
		}
		else if (Application.isEditor)
		{
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>", new object[0]);
		}
		return false;
	}

	public static bool PostEventNotify(string eventName, AudioManager.EventAction eventAction, OnEventNotify notifyCallback, object parameter = null, GameObject parentGameObject = null)
	{
		if (eventName != "_UnSet_")
		{
			if (!AudioManager.ShouldSkipAudioEvents())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.PostEventNotify(string, AudioManager.EventAction, OnEventNotify, object, GameObject)).MethodHandle;
				}
				EventManager instance = EventManager.Instance;
				bool result;
				if (instance == null)
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
					result = false;
				}
				else
				{
					result = instance.PostEventNotify(eventName, (Fabric.EventAction)eventAction, parameter, parentGameObject, notifyCallback);
				}
				return result;
			}
		}
		else if (Application.isEditor)
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
			Log.Warning("<color=white>    * AudioManager trying to post _UnSet_ event</color>", new object[0]);
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
		gameObject.GetComponentsInChildren<EventTrigger>(true, list);
		foreach (EventTrigger eventTrigger in list)
		{
			if (eventTrigger._eventName == "start_music")
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.EnableMusicAtStartup()).MethodHandle;
				}
				if (!eventTrigger.enabled)
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
					eventTrigger.enabled = true;
				}
			}
		}
	}

	public static void EnableAmbianceAtStartup()
	{
		GameObject gameObject = GameObject.Find("_AUDIO_INITIALIZE");
		List<EventTrigger> list = new List<EventTrigger>();
		gameObject.GetComponentsInChildren<EventTrigger>(true, list);
		using (List<EventTrigger>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EventTrigger eventTrigger = enumerator.Current;
				if (eventTrigger._eventName == "start_ambiance" && !eventTrigger.enabled)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.EnableAmbianceAtStartup()).MethodHandle;
					}
					eventTrigger.enabled = true;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public static bool StandardizeAudioLinkages_NeedsUpdate(AudioMixerGroup mixerGroup)
	{
		if (mixerGroup == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.StandardizeAudioLinkages_NeedsUpdate(AudioMixerGroup)).MethodHandle;
			}
			return false;
		}
		if (GameWideData.Get() == null)
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
			return false;
		}
		if (mixerGroup.audioMixer == AudioManager.GetMixerSnapshotManager().m_mixer)
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
			return false;
		}
		return true;
	}

	public static AudioMixerGroup StandardizeAudioLinkages_Update(AudioMixerGroup mixerGroup)
	{
		if (mixerGroup == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.StandardizeAudioLinkages_Update(AudioMixerGroup)).MethodHandle;
			}
			return null;
		}
		if (GameWideData.Get() == null)
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
			return mixerGroup;
		}
		AudioMixerGroup[] array = AudioManager.GetMixerSnapshotManager().m_mixer.FindMatchingGroups(mixerGroup.name);
		AudioMixerGroup audioMixerGroup = null;
		if (array != null)
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
			foreach (AudioMixerGroup audioMixerGroup2 in array)
			{
				if (audioMixerGroup2.name == mixerGroup.name)
				{
					audioMixerGroup = audioMixerGroup2;
				}
			}
			for (;;)
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
		foreach (Transform ts in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (ts.parent == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.<FindAllSceneObjectsOfType>c__Iterator0.MoveNext()).MethodHandle;
				}
				foreach (T t in ts.GetComponentsInChildren<T>(true))
				{
					yield return t;
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				for (;;)
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
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		yield break;
	}

	public static void StandardizeAudioLinkages()
	{
		IEnumerator<Fabric.Component> enumerator = AudioManager.FindAllSceneObjectsOfType<Fabric.Component>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Fabric.Component component = enumerator.Current;
				if (AudioManager.StandardizeAudioLinkages_NeedsUpdate(component._audioMixerGroup))
				{
					component._audioMixerGroup = AudioManager.StandardizeAudioLinkages_Update(component._audioMixerGroup);
				}
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.StandardizeAudioLinkages()).MethodHandle;
			}
		}
		finally
		{
			if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
		IEnumerator<AudioSource> enumerator2 = AudioManager.FindAllSceneObjectsOfType<AudioSource>().GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				AudioSource audioSource = enumerator2.Current;
				if (AudioManager.StandardizeAudioLinkages_NeedsUpdate(audioSource.outputAudioMixerGroup))
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
					audioSource.outputAudioMixerGroup = AudioManager.StandardizeAudioLinkages_Update(audioSource.outputAudioMixerGroup);
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			if (enumerator2 != null)
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
				enumerator2.Dispose();
			}
		}
	}

	public static void StandardizeAudioLinkages(GameObject root)
	{
		foreach (Fabric.Component component in root.GetComponentsInChildren<Fabric.Component>())
		{
			if (AudioManager.StandardizeAudioLinkages_NeedsUpdate(component._audioMixerGroup))
			{
				component._audioMixerGroup = AudioManager.StandardizeAudioLinkages_Update(component._audioMixerGroup);
			}
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(AudioManager.StandardizeAudioLinkages(GameObject)).MethodHandle;
		}
		foreach (AudioSource audioSource in root.GetComponentsInChildren<AudioSource>())
		{
			if (AudioManager.StandardizeAudioLinkages_NeedsUpdate(audioSource.outputAudioMixerGroup))
			{
				audioSource.outputAudioMixerGroup = AudioManager.StandardizeAudioLinkages_Update(audioSource.outputAudioMixerGroup);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public static void TransitionToOnRealMixer(this AudioMixerSnapshot snapshot, float transitionTime)
	{
		if (AudioManager.GetMixerSnapshotManager().m_mixer == null)
		{
			snapshot.TransitionTo(transitionTime);
			return;
		}
		AudioMixerSnapshot audioMixerSnapshot = AudioManager.GetMixerSnapshotManager().m_mixer.FindSnapshot(snapshot.name);
		if (audioMixerSnapshot == null)
		{
			snapshot.TransitionTo(transitionTime);
			return;
		}
		audioMixerSnapshot.TransitionTo(transitionTime);
	}

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
}
