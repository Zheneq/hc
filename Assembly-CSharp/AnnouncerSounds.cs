using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnnouncerSounds : MonoBehaviour, IGameEventListener
{
	public GameObject m_announcerDefaultAudioPrefab;

	private GameObject m_announcerAudioInstance;

	public bool m_enableSounds = true;

	public List<AnnouncerSounds.AnnouncerEvent> m_eventsToNotPlay;

	public float m_delayedAnnouncementDelay = 0.5f;

	private float m_delayedAnnouncementTimer = -1f;

	private AnnouncerSounds.AnnouncerEvent m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.Invalid;

	[Separator("For Loot Matrix VO", true)]
	public GameObject m_lootVoAutioPrefab;

	[Separator("Onboarding VO", true)]
	public GameObject m_onboardingVoAudioPrefab;

	[Header("-- Use to specify character names in audio events, in case they are different from CharacterType, for exmple, RobotAnimal -> pup")]
	public List<AnnouncerSounds.CharTypeToAudioCharName> m_charTypeToAudioCharNameOverride = new List<AnnouncerSounds.CharTypeToAudioCharName>();

	private GameObject m_lootVoAudioInstance;

	private GameObject m_onboardingVoAudioInstance;

	private Dictionary<CharacterType, string> m_cachedCharTypeToName = new Dictionary<CharacterType, string>();

	private static AnnouncerSounds s_instance;

	private bool c_debugLoggingOn;

	[AudioEvent(false)]
	public string m_audioEventSolo;

	[AudioEvent(false)]
	public string m_audioEventCoOp;

	[AudioEvent(false)]
	public string m_audioEventPvp;

	[AudioEvent(false)]
	public string m_audioEventPractice;

	[AudioEvent(false)]
	public string m_audioEventRanked;

	[AudioEvent(false)]
	public string m_audioEventQuick;

	[AudioEvent(false)]
	public string m_audioEventCustom;

	[AudioEvent(false)]
	public string m_audioEventMovementPhase;

	[AudioEvent(false)]
	public string m_audioEventPrepPhase;

	[AudioEvent(false)]
	public string m_audioEventDashPhase;

	[AudioEvent(false)]
	public string m_audioEventBlastPhase;

	[AudioEvent(false)]
	public string m_audioEventDeath;

	[AudioEvent(false)]
	public string m_audioEventRespawn;

	[AudioEvent(false)]
	public string m_audioEventSuddenDeath;

	[AudioEvent(false)]
	public string m_audioEventTurnsRemaining_05;

	[AudioEvent(false)]
	public string m_audioEventTurnsRemaining_04;

	[AudioEvent(false)]
	public string m_audioEventTurnsRemaining_03;

	[AudioEvent(false)]
	public string m_audioEventTurnsRemaining_02;

	[AudioEvent(false)]
	public string m_audioEventTurnsRemaining_01;

	[AudioEvent(false)]
	public string m_audioEventCountdown_10;

	[AudioEvent(false)]
	public string m_audioEventCountdown_09;

	[AudioEvent(false)]
	public string m_audioEventCountdown_08;

	[AudioEvent(false)]
	public string m_audioEventCountdown_07;

	[AudioEvent(false)]
	public string m_audioEventCountdown_06;

	[AudioEvent(false)]
	public string m_audioEventCountdown_05;

	[AudioEvent(false)]
	public string m_audioEventCountdown_04;

	[AudioEvent(false)]
	public string m_audioEventCountdown_03;

	[AudioEvent(false)]
	public string m_audioEventCountdown_02;

	[AudioEvent(false)]
	public string m_audioEventCountdown_01;

	[AudioEvent(false)]
	public string m_audioEventVictory;

	[AudioEvent(false)]
	public string m_audioEventDefeat;

	public static AnnouncerSounds GetAnnouncerSounds()
	{
		return AnnouncerSounds.s_instance;
	}

	private void Awake()
	{
		if (AnnouncerSounds.s_instance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.Awake()).MethodHandle;
			}
			AnnouncerSounds.s_instance = this;
		}
		else
		{
			Log.Warning("Please remove AnnouncerSounds component from scene: {0}.unity", new object[]
			{
				SceneManager.GetActiveScene().name
			});
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.PostCharacterDeath);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.CharacterRespawn);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		IEnumerator enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				CharacterType key = (CharacterType)obj;
				this.m_cachedCharTypeToName[key] = key.ToString().ToLower();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
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
				disposable.Dispose();
			}
		}
		using (List<AnnouncerSounds.CharTypeToAudioCharName>.Enumerator enumerator2 = this.m_charTypeToAudioCharNameOverride.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				AnnouncerSounds.CharTypeToAudioCharName charTypeToAudioCharName = enumerator2.Current;
				if (!string.IsNullOrEmpty(charTypeToAudioCharName.m_audioCharName))
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
					this.m_cachedCharTypeToName[charTypeToAudioCharName.m_charType] = charTypeToAudioCharName.m_audioCharName.ToLower();
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
	}

	private void Start()
	{
		this.m_announcerAudioInstance = UnityEngine.Object.Instantiate<GameObject>(this.m_announcerDefaultAudioPrefab);
		UnityEngine.Object.DontDestroyOnLoad(this.m_announcerAudioInstance);
	}

	public void InstantiateLootVOPrefabIfNeeded()
	{
		if (this.m_lootVoAudioInstance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.InstantiateLootVOPrefabIfNeeded()).MethodHandle;
			}
			if (this.m_lootVoAutioPrefab != null)
			{
				this.m_lootVoAudioInstance = UnityEngine.Object.Instantiate<GameObject>(this.m_lootVoAutioPrefab);
				UnityEngine.Object.DontDestroyOnLoad(this.m_lootVoAudioInstance);
				AudioManager.StandardizeAudioLinkages(this.m_lootVoAudioInstance);
			}
		}
	}

	public void InstantiateOnboardingVOPrefabIfNeeded()
	{
		if (this.m_onboardingVoAudioInstance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.InstantiateOnboardingVOPrefabIfNeeded()).MethodHandle;
			}
			if (this.m_onboardingVoAudioPrefab != null)
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
				this.m_onboardingVoAudioInstance = UnityEngine.Object.Instantiate<GameObject>(this.m_onboardingVoAudioPrefab);
				UnityEngine.Object.DontDestroyOnLoad(this.m_onboardingVoAudioInstance);
				AudioManager.StandardizeAudioLinkages(this.m_onboardingVoAudioInstance);
			}
		}
	}

	private void OnDestroy()
	{
		if (AnnouncerSounds.s_instance != null && AnnouncerSounds.s_instance == this)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.OnDestroy()).MethodHandle;
			}
			AnnouncerSounds.s_instance = null;
		}
	}

	private void Update()
	{
		if (this.m_delayedAnnouncement != AnnouncerSounds.AnnouncerEvent.Invalid)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.Update()).MethodHandle;
			}
			if (this.m_delayedAnnouncementTimer > 0f)
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
				if (this.m_delayedAnnouncementTimer <= Time.time)
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
					this.PlayDelayedAnnouncement();
				}
			}
		}
	}

	private void PlayDelayedAnnouncement()
	{
		if (ObjectivePoints.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.PlayDelayedAnnouncement()).MethodHandle;
			}
			if (ObjectivePoints.Get().m_matchState != ObjectivePoints.MatchState.MatchEnd)
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
				if (AppState.GetCurrent() != AppState_InGameEnding.Get())
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
					this.PlayAnnouncementByEnum(this.m_delayedAnnouncement);
				}
			}
		}
		this.m_delayedAnnouncementTimer = -1f;
		this.m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.Invalid;
	}

	public void PlayAnnouncementByStr(string eventName)
	{
		if (this.m_enableSounds)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.PlayAnnouncementByStr(string)).MethodHandle;
			}
			AudioManager.PostEvent(eventName, null);
			if (this.c_debugLoggingOn)
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
				Debug.Log("Playing announcement " + eventName + ".");
			}
		}
	}

	public void PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent eventEnum)
	{
		if (this.m_enableSounds)
		{
			if (this.m_eventsToNotPlay != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent)).MethodHandle;
				}
				if (this.m_eventsToNotPlay.Contains(eventEnum))
				{
					return;
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
			string audioEventOfAnnouncerEvent = this.GetAudioEventOfAnnouncerEvent(eventEnum);
			AudioManager.PostEvent(audioEventOfAnnouncerEvent, null);
			if (this.c_debugLoggingOn)
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
				Debug.Log(string.Concat(new string[]
				{
					"Playing announcement enum ",
					eventEnum.ToString(),
					" with event string ",
					audioEventOfAnnouncerEvent,
					"."
				}));
			}
		}
	}

	public void StopAnnouncementByStr(string eventName)
	{
		AudioManager.PostEvent(eventName, AudioManager.EventAction.StopSound, null, null);
	}

	public void StopAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent eventEnum)
	{
		string audioEventOfAnnouncerEvent = this.GetAudioEventOfAnnouncerEvent(eventEnum);
		AudioManager.PostEvent(audioEventOfAnnouncerEvent, AudioManager.EventAction.StopSound, null, null);
	}

	public void PlayLootVOForCharacter(CharacterType charType)
	{
		if (charType != CharacterType.None && this.m_lootVoAudioInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.PlayLootVOForCharacter(CharacterType)).MethodHandle;
			}
			if (this.m_cachedCharTypeToName.ContainsKey(charType))
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
				string eventName = "vo/" + this.m_cachedCharTypeToName[charType] + "/loot_matrix_drop";
				AudioManager.PostEvent(eventName, null);
			}
		}
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.PostCharacterDeath)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.IGameEventListener.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			GameEventManager.CharacterDeathEventArgs characterDeathEventArgs = args as GameEventManager.CharacterDeathEventArgs;
			if (characterDeathEventArgs != null && characterDeathEventArgs.deadCharacter == GameFlowData.Get().activeOwnedActorData)
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
				this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Death);
			}
		}
		else if (eventType == GameEventManager.EventType.CharacterRespawn)
		{
			GameEventManager.CharacterRespawnEventArgs characterRespawnEventArgs = args as GameEventManager.CharacterRespawnEventArgs;
			if (characterRespawnEventArgs != null)
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
				if (characterRespawnEventArgs.respawningCharacter == GameFlowData.Get().activeOwnedActorData)
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
					this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Respawn);
				}
			}
		}
		else if (eventType == GameEventManager.EventType.TurnTick && ObjectivePoints.Get() != null)
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
			if (GameFlowData.Get() != null)
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
				if (GameFlowData.Get().CurrentTurn == ObjectivePoints.Get().m_timeLimitTurns)
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
					this.m_delayedAnnouncementTimer = Time.time + this.m_delayedAnnouncementDelay;
					this.m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.SuddenDeath;
				}
				else
				{
					int num = ObjectivePoints.Get().m_timeLimitTurns - GameFlowData.Get().CurrentTurn;
					if (num <= 5)
					{
						this.m_delayedAnnouncementTimer = Time.time + this.m_delayedAnnouncementDelay;
						if (num == 5)
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
							this.m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.TurnsRemaining_05;
						}
						else if (num == 4)
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
							this.m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.TurnsRemaining_04;
						}
						else if (num == 3)
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
							this.m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.TurnsRemaining_03;
						}
						else if (num == 2)
						{
							this.m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.TurnsRemaining_02;
						}
						else if (num == 1)
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
							this.m_delayedAnnouncement = AnnouncerSounds.AnnouncerEvent.TurnsRemaining_01;
						}
					}
				}
			}
		}
	}

	public string GetAudioEventOfAnnouncerEvent(AnnouncerSounds.AnnouncerEvent announcerEvent)
	{
		string result;
		if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Solo)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.GetAudioEventOfAnnouncerEvent(AnnouncerSounds.AnnouncerEvent)).MethodHandle;
			}
			result = this.m_audioEventSolo;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.CoOp)
		{
			result = this.m_audioEventCoOp;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Pvp)
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
			result = this.m_audioEventPvp;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Practice)
		{
			result = this.m_audioEventPractice;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Ranked)
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
			result = this.m_audioEventRanked;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Quick)
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
			result = this.m_audioEventQuick;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Custom)
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
			result = this.m_audioEventCustom;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_10)
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
			result = this.m_audioEventCountdown_10;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_09)
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
			result = this.m_audioEventCountdown_09;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_08)
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
			result = this.m_audioEventCountdown_08;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_07)
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
			result = this.m_audioEventCountdown_07;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_06)
		{
			result = this.m_audioEventCountdown_06;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_05)
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
			result = this.m_audioEventCountdown_05;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_04)
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
			result = this.m_audioEventCountdown_04;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_03)
		{
			result = this.m_audioEventCountdown_03;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_02)
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
			result = this.m_audioEventCountdown_02;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Countdown_01)
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
			result = this.m_audioEventCountdown_01;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.MovementPhase)
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
			result = this.m_audioEventMovementPhase;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.PrepPhase)
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
			result = this.m_audioEventPrepPhase;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.DashPhase)
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
			result = this.m_audioEventDashPhase;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.BlastPhase)
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
			result = this.m_audioEventBlastPhase;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Death)
		{
			result = this.m_audioEventDeath;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Respawn)
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
			result = this.m_audioEventRespawn;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.SuddenDeath)
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
			result = this.m_audioEventSuddenDeath;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.TurnsRemaining_05)
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
			result = this.m_audioEventTurnsRemaining_05;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.TurnsRemaining_04)
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
			result = this.m_audioEventTurnsRemaining_04;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.TurnsRemaining_03)
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
			result = this.m_audioEventTurnsRemaining_03;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.TurnsRemaining_02)
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
			result = this.m_audioEventTurnsRemaining_02;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.TurnsRemaining_01)
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
			result = this.m_audioEventTurnsRemaining_01;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Victory)
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
			result = this.m_audioEventVictory;
		}
		else if (announcerEvent == AnnouncerSounds.AnnouncerEvent.Defeat)
		{
			result = this.m_audioEventDefeat;
		}
		else
		{
			Debug.LogError("Failed to find audio event str for event enum " + announcerEvent.ToString() + ".");
			result = string.Empty;
		}
		return result;
	}

	public void PlayCountdownAnnouncementIfAppropriate(float previousTimeRemaining, float currentTimeRemaining)
	{
		if (previousTimeRemaining > 10f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnnouncerSounds.PlayCountdownAnnouncementIfAppropriate(float, float)).MethodHandle;
			}
			if (currentTimeRemaining <= 10f)
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
				this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_10);
				return;
			}
		}
		if (previousTimeRemaining > 9f)
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
			if (currentTimeRemaining <= 9f)
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
				this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_09);
				return;
			}
		}
		if (previousTimeRemaining > 8f && currentTimeRemaining <= 8f)
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
			this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_08);
		}
		else if (previousTimeRemaining > 7f && currentTimeRemaining <= 7f)
		{
			this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_07);
		}
		else
		{
			if (previousTimeRemaining > 6f)
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
				if (currentTimeRemaining <= 6f)
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
					this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_06);
					return;
				}
			}
			if (previousTimeRemaining > 5f && currentTimeRemaining <= 5f)
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
				this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_05);
			}
			else if (previousTimeRemaining > 4f && currentTimeRemaining <= 4f)
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
				this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_04);
			}
			else
			{
				if (previousTimeRemaining > 3f)
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
					if (currentTimeRemaining <= 3f)
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
						this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_03);
						return;
					}
				}
				if (previousTimeRemaining > 2f)
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
					if (currentTimeRemaining <= 2f)
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
						this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_02);
						return;
					}
				}
				if (previousTimeRemaining > 1f)
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
					if (currentTimeRemaining <= 1f)
					{
						this.PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Countdown_01);
					}
				}
			}
		}
	}

	[Serializable]
	public class CharTypeToAudioCharName
	{
		public CharacterType m_charType;

		public string m_audioCharName = string.Empty;
	}

	public enum AnnouncerEvent
	{
		Solo,
		CoOp,
		Pvp,
		Practice,
		Ranked,
		Quick,
		Custom,
		Countdown_10,
		Countdown_09,
		Countdown_08,
		Countdown_07,
		Countdown_06,
		Countdown_05,
		Countdown_04,
		Countdown_03,
		Countdown_02,
		Countdown_01,
		MovementPhase,
		PrepPhase,
		DashPhase,
		BlastPhase,
		Death,
		Respawn,
		SuddenDeath,
		TurnsRemaining_05,
		TurnsRemaining_04,
		TurnsRemaining_03,
		TurnsRemaining_02,
		TurnsRemaining_01,
		Victory,
		Defeat,
		Invalid
	}
}
