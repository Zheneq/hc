using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnnouncerSounds : MonoBehaviour, IGameEventListener
{
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

	public GameObject m_announcerDefaultAudioPrefab;

	private GameObject m_announcerAudioInstance;

	public bool m_enableSounds = true;

	public List<AnnouncerEvent> m_eventsToNotPlay;

	public float m_delayedAnnouncementDelay = 0.5f;

	private float m_delayedAnnouncementTimer = -1f;

	private AnnouncerEvent m_delayedAnnouncement = AnnouncerEvent.Invalid;

	[Separator("For Loot Matrix VO", true)]
	public GameObject m_lootVoAutioPrefab;

	[Separator("Onboarding VO", true)]
	public GameObject m_onboardingVoAudioPrefab;

	[Header("-- Use to specify character names in audio events, in case they are different from CharacterType, for exmple, RobotAnimal -> pup")]
	public List<CharTypeToAudioCharName> m_charTypeToAudioCharNameOverride = new List<CharTypeToAudioCharName>();

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
		return s_instance;
	}

	private void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		else
		{
			Log.Warning("Please remove AnnouncerSounds component from scene: {0}.unity", SceneManager.GetActiveScene().name);
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.PostCharacterDeath);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.CharacterRespawn);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		IEnumerator enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CharacterType key = (CharacterType)enumerator.Current;
				m_cachedCharTypeToName[key] = key.ToString().ToLower();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_00cc;
					}
				}
			}
			end_IL_00cc:;
		}
		using (List<CharTypeToAudioCharName>.Enumerator enumerator2 = m_charTypeToAudioCharNameOverride.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				CharTypeToAudioCharName current = enumerator2.Current;
				if (!string.IsNullOrEmpty(current.m_audioCharName))
				{
					m_cachedCharTypeToName[current.m_charType] = current.m_audioCharName.ToLower();
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void Start()
	{
		m_announcerAudioInstance = UnityEngine.Object.Instantiate(m_announcerDefaultAudioPrefab);
		UnityEngine.Object.DontDestroyOnLoad(m_announcerAudioInstance);
	}

	public void InstantiateLootVOPrefabIfNeeded()
	{
		if (!(m_lootVoAudioInstance == null))
		{
			return;
		}
		while (true)
		{
			if (m_lootVoAutioPrefab != null)
			{
				m_lootVoAudioInstance = UnityEngine.Object.Instantiate(m_lootVoAutioPrefab);
				UnityEngine.Object.DontDestroyOnLoad(m_lootVoAudioInstance);
				AudioManager.StandardizeAudioLinkages(m_lootVoAudioInstance);
			}
			return;
		}
	}

	public void InstantiateOnboardingVOPrefabIfNeeded()
	{
		if (!(m_onboardingVoAudioInstance == null))
		{
			return;
		}
		while (true)
		{
			if (m_onboardingVoAudioPrefab != null)
			{
				while (true)
				{
					m_onboardingVoAudioInstance = UnityEngine.Object.Instantiate(m_onboardingVoAudioPrefab);
					UnityEngine.Object.DontDestroyOnLoad(m_onboardingVoAudioInstance);
					AudioManager.StandardizeAudioLinkages(m_onboardingVoAudioInstance);
					return;
				}
			}
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(s_instance != null) || !(s_instance == this))
		{
			return;
		}
		while (true)
		{
			s_instance = null;
			return;
		}
	}

	private void Update()
	{
		if (m_delayedAnnouncement == AnnouncerEvent.Invalid)
		{
			return;
		}
		while (true)
		{
			if (!(m_delayedAnnouncementTimer > 0f))
			{
				return;
			}
			while (true)
			{
				if (m_delayedAnnouncementTimer <= Time.time)
				{
					while (true)
					{
						PlayDelayedAnnouncement();
						return;
					}
				}
				return;
			}
		}
	}

	private void PlayDelayedAnnouncement()
	{
		if (ObjectivePoints.Get() != null)
		{
			if (ObjectivePoints.Get().m_matchState != ObjectivePoints.MatchState.MatchEnd)
			{
				if (AppState.GetCurrent() != AppState_InGameEnding.Get())
				{
					PlayAnnouncementByEnum(m_delayedAnnouncement);
				}
			}
		}
		m_delayedAnnouncementTimer = -1f;
		m_delayedAnnouncement = AnnouncerEvent.Invalid;
	}

	public void PlayAnnouncementByStr(string eventName)
	{
		if (!m_enableSounds)
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(eventName);
			if (c_debugLoggingOn)
			{
				while (true)
				{
					Debug.Log(new StringBuilder().Append("Playing announcement ").Append(eventName).Append(".").ToString());
					return;
				}
			}
			return;
		}
	}

	public void PlayAnnouncementByEnum(AnnouncerEvent eventEnum)
	{
		if (!m_enableSounds)
		{
			return;
		}
		if (m_eventsToNotPlay != null)
		{
			if (m_eventsToNotPlay.Contains(eventEnum))
			{
				return;
			}
		}
		string audioEventOfAnnouncerEvent = GetAudioEventOfAnnouncerEvent(eventEnum);
		AudioManager.PostEvent(audioEventOfAnnouncerEvent);
		if (!c_debugLoggingOn)
		{
			return;
		}
		while (true)
		{
			Debug.Log(new StringBuilder().Append("Playing announcement enum ").Append(eventEnum.ToString()).Append(" with event string ").Append(audioEventOfAnnouncerEvent).Append(".").ToString());
			return;
		}
	}

	public void StopAnnouncementByStr(string eventName)
	{
		AudioManager.PostEvent(eventName, AudioManager.EventAction.StopSound);
	}

	public void StopAnnouncementByEnum(AnnouncerEvent eventEnum)
	{
		string audioEventOfAnnouncerEvent = GetAudioEventOfAnnouncerEvent(eventEnum);
		AudioManager.PostEvent(audioEventOfAnnouncerEvent, AudioManager.EventAction.StopSound);
	}

	public void PlayLootVOForCharacter(CharacterType charType)
	{
		if (charType == CharacterType.None || !(m_lootVoAudioInstance != null))
		{
			return;
		}
		while (true)
		{
			if (m_cachedCharTypeToName.ContainsKey(charType))
			{
				while (true)
				{
					string eventName = new StringBuilder().Append("vo/").Append((object)m_cachedCharTypeToName[charType]).Append("/loot_matrix_drop").ToString();
					AudioManager.PostEvent(eventName);
					return;
				}
			}
			return;
		}
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		switch (eventType)
		{
		case GameEventManager.EventType.PostCharacterDeath:
			while (true)
			{
				GameEventManager.CharacterDeathEventArgs characterDeathEventArgs = args as GameEventManager.CharacterDeathEventArgs;
				if (characterDeathEventArgs != null && characterDeathEventArgs.deadCharacter == GameFlowData.Get().activeOwnedActorData)
				{
					while (true)
					{
						PlayAnnouncementByEnum(AnnouncerEvent.Death);
						return;
					}
				}
				return;
			}
		case GameEventManager.EventType.CharacterRespawn:
		{
			GameEventManager.CharacterRespawnEventArgs characterRespawnEventArgs = args as GameEventManager.CharacterRespawnEventArgs;
			if (characterRespawnEventArgs == null)
			{
				break;
			}
			while (true)
			{
				if (characterRespawnEventArgs.respawningCharacter == GameFlowData.Get().activeOwnedActorData)
				{
					while (true)
					{
						PlayAnnouncementByEnum(AnnouncerEvent.Respawn);
						return;
					}
				}
				return;
			}
		}
		case GameEventManager.EventType.TurnTick:
			if (!(ObjectivePoints.Get() != null))
			{
				break;
			}
			while (true)
			{
				if (!(GameFlowData.Get() != null))
				{
					return;
				}
				while (true)
				{
					if (GameFlowData.Get().CurrentTurn == ObjectivePoints.Get().m_timeLimitTurns)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								m_delayedAnnouncementTimer = Time.time + m_delayedAnnouncementDelay;
								m_delayedAnnouncement = AnnouncerEvent.SuddenDeath;
								return;
							}
						}
					}
					int num = ObjectivePoints.Get().m_timeLimitTurns - GameFlowData.Get().CurrentTurn;
					if (num > 5)
					{
						return;
					}
					m_delayedAnnouncementTimer = Time.time + m_delayedAnnouncementDelay;
					if (num == 5)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								m_delayedAnnouncement = AnnouncerEvent.TurnsRemaining_05;
								return;
							}
						}
					}
					if (num == 4)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								m_delayedAnnouncement = AnnouncerEvent.TurnsRemaining_04;
								return;
							}
						}
					}
					if (num == 3)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								m_delayedAnnouncement = AnnouncerEvent.TurnsRemaining_03;
								return;
							}
						}
					}
					switch (num)
					{
					default:
						return;
					case 2:
						m_delayedAnnouncement = AnnouncerEvent.TurnsRemaining_02;
						return;
					case 1:
						break;
					}
					while (true)
					{
						m_delayedAnnouncement = AnnouncerEvent.TurnsRemaining_01;
						return;
					}
				}
			}
		}
	}

	public string GetAudioEventOfAnnouncerEvent(AnnouncerEvent announcerEvent)
	{
		if (announcerEvent == AnnouncerEvent.Solo)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_audioEventSolo;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.CoOp)
		{
			return m_audioEventCoOp;
		}
		if (announcerEvent == AnnouncerEvent.Pvp)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_audioEventPvp;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Practice)
		{
			return m_audioEventPractice;
		}
		if (announcerEvent == AnnouncerEvent.Ranked)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_audioEventRanked;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Quick)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventQuick;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Custom)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventCustom;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_10)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_10;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_09)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_09;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_08)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_08;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_07)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_07;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_06)
		{
			return m_audioEventCountdown_06;
		}
		if (announcerEvent == AnnouncerEvent.Countdown_05)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_05;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_04)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_04;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_03)
		{
			return m_audioEventCountdown_03;
		}
		if (announcerEvent == AnnouncerEvent.Countdown_02)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_02;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Countdown_01)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventCountdown_01;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.MovementPhase)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_audioEventMovementPhase;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.PrepPhase)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_audioEventPrepPhase;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.DashPhase)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_audioEventDashPhase;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.BlastPhase)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_audioEventBlastPhase;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.Death)
		{
			return m_audioEventDeath;
		}
		if (announcerEvent == AnnouncerEvent.Respawn)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_audioEventRespawn;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.SuddenDeath)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_audioEventSuddenDeath;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.TurnsRemaining_05)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_audioEventTurnsRemaining_05;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.TurnsRemaining_04)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_audioEventTurnsRemaining_04;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.TurnsRemaining_03)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_audioEventTurnsRemaining_03;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.TurnsRemaining_02)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_audioEventTurnsRemaining_02;
				}
			}
		}
		if (announcerEvent == AnnouncerEvent.TurnsRemaining_01)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_audioEventTurnsRemaining_01;
				}
			}
		}
		switch (announcerEvent)
		{
		case AnnouncerEvent.Victory:
			while (true)
			{
				return m_audioEventVictory;
			}
		case AnnouncerEvent.Defeat:
			return m_audioEventDefeat;
		default:
			Debug.LogError(new StringBuilder().Append("Failed to find audio event str for event enum ").Append(announcerEvent.ToString()).Append(".").ToString());
			return string.Empty;
		}
	}

	public void PlayCountdownAnnouncementIfAppropriate(float previousTimeRemaining, float currentTimeRemaining)
	{
		if (previousTimeRemaining > 10f)
		{
			if (currentTimeRemaining <= 10f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						PlayAnnouncementByEnum(AnnouncerEvent.Countdown_10);
						return;
					}
				}
			}
		}
		if (previousTimeRemaining > 9f)
		{
			if (currentTimeRemaining <= 9f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						PlayAnnouncementByEnum(AnnouncerEvent.Countdown_09);
						return;
					}
				}
			}
		}
		if (previousTimeRemaining > 8f && currentTimeRemaining <= 8f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					PlayAnnouncementByEnum(AnnouncerEvent.Countdown_08);
					return;
				}
			}
		}
		if (previousTimeRemaining > 7f && currentTimeRemaining <= 7f)
		{
			PlayAnnouncementByEnum(AnnouncerEvent.Countdown_07);
			return;
		}
		if (previousTimeRemaining > 6f)
		{
			if (currentTimeRemaining <= 6f)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						PlayAnnouncementByEnum(AnnouncerEvent.Countdown_06);
						return;
					}
				}
			}
		}
		if (previousTimeRemaining > 5f && currentTimeRemaining <= 5f)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					PlayAnnouncementByEnum(AnnouncerEvent.Countdown_05);
					return;
				}
			}
		}
		if (previousTimeRemaining > 4f && currentTimeRemaining <= 4f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					PlayAnnouncementByEnum(AnnouncerEvent.Countdown_04);
					return;
				}
			}
		}
		if (previousTimeRemaining > 3f)
		{
			if (currentTimeRemaining <= 3f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						PlayAnnouncementByEnum(AnnouncerEvent.Countdown_03);
						return;
					}
				}
			}
		}
		if (previousTimeRemaining > 2f)
		{
			if (currentTimeRemaining <= 2f)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						PlayAnnouncementByEnum(AnnouncerEvent.Countdown_02);
						return;
					}
				}
			}
		}
		if (!(previousTimeRemaining > 1f))
		{
			return;
		}
		while (true)
		{
			if (currentTimeRemaining <= 1f)
			{
				PlayAnnouncementByEnum(AnnouncerEvent.Countdown_01);
			}
			return;
		}
	}
}
