using Fabric;
using System.Collections.Generic;
using UnityEngine;

public class ChatterManager : MonoBehaviour
{
	private struct SubmittedChatter
	{
		public IChatterData chatter;

		public GameObject source;

		public float calculatedPriority;

		public float timeSubmitted;

		public bool errorPrintCompleted;
	}

	private static ChatterManager s_instance;

	[Tooltip("The amount of time to delay after one chatter finishes playing before new chatter submissions will be accepted.")]
	public float m_chatterCooldownSec;

	[Tooltip("The amount by which to multiply chatter priority when the source is the local player's character.")]
	public float m_localActorPriorityMultiplier = 1.5f;

	[Tooltip("The rate (in seconds) at which the manager decides which of the chatters submitted during that time to play.")]
	public float m_submissionIntervalSec = 0.75f;

	private const float m_currentChatterEndFallbackTime = 20f;

	private bool m_enableChatter = true;

	private List<SubmittedChatter> m_submittedChatter = new List<SubmittedChatter>();

	private IChatterData m_currentlyPlayingChatter;

	private GameObject m_currentlyPlayingChatterTarget;

	private float m_currentCooldownSec;

	private float m_currentIntervalSec;

	private float m_timeSinceLastChatterStart;

	private Dictionary<string, float> m_chatterAvailability = new Dictionary<string, float>();

	private Dictionary<ChatterData.ChatterGroup, float> m_chatterGroupAvailability = new Dictionary<ChatterData.ChatterGroup, float>();

	private ConversationTemplate m_conversation;

	private int m_conversationIndex;

	private const string c_chatterDebugHeader = "<color=orange>Chatter:</color> ";

	private const string c_rejectChatterDebugHeader = "<color=red>Rejecting chatter: </color>";

	public bool EnableChatter
	{
		get
		{
			return m_enableChatter;
		}
		set
		{
			m_enableChatter = value;
		}
	}

	public static ChatterManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		if (m_currentCooldownSec > 0f)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_currentCooldownSec -= Time.deltaTime;
		}
		m_currentIntervalSec += Time.deltaTime;
		if (m_currentlyPlayingChatter != null)
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
			m_timeSinceLastChatterStart += Time.deltaTime;
			if (m_timeSinceLastChatterStart > 20f)
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
				ChatterDebugLog($"Current Chatter has been cleared - timeout has been exceeded waiting for an end event for audio event [{m_currentlyPlayingChatter.GetCommonData().m_audioEvent}].");
				m_currentlyPlayingChatter = null;
				m_currentlyPlayingChatterTarget = null;
				if (m_chatterCooldownSec > 0f)
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
					m_currentCooldownSec = m_chatterCooldownSec;
				}
			}
		}
		if (!(m_currentIntervalSec >= m_submissionIntervalSec) || m_currentlyPlayingChatter != null)
		{
			return;
		}
		m_currentIntervalSec %= m_submissionIntervalSec;
		if (m_conversation != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					PlayChatter(new SubmittedChatter
					{
						chatter = new ChatterDataStub(m_conversation.m_lines[m_conversationIndex].m_line),
						source = GameFlowData.Get().activeOwnedActorData.gameObject,
						calculatedPriority = 1000f,
						timeSubmitted = Time.time
					});
					m_conversationIndex++;
					if (m_conversation.m_lines.Length <= m_conversationIndex)
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
						m_conversation = null;
						m_conversationIndex = 0;
					}
					m_submittedChatter.Clear();
					return;
				}
			}
		}
		if (m_submittedChatter.Count <= 0)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			float time = Time.time;
			SubmittedChatter submittedChatter = m_submittedChatter[0];
			float num = time - submittedChatter.timeSubmitted;
			SubmittedChatter submittedChatter2 = m_submittedChatter[0];
			if (num >= submittedChatter2.chatter.GetCommonData().m_chatterDelay)
			{
				m_submittedChatter.Sort((SubmittedChatter a, SubmittedChatter b) => -a.calculatedPriority.CompareTo(b.calculatedPriority));
				PlayChatter(m_submittedChatter[0]);
				m_submittedChatter.Clear();
			}
			return;
		}
	}

	public void SubmitChatter(IChatterData chatter, GameObject source)
	{
		if (m_currentlyPlayingChatter != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ChatterDebugLog("<color=red>Rejecting chatter: </color>" + chatter.GetCommonData().m_audioEvent + " due to existing chatter " + m_currentlyPlayingChatter.GetCommonData().m_audioEvent + " still playing");
					return;
				}
			}
		}
		if (m_currentCooldownSec > 0f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					ChatterDebugLog("<color=red>Rejecting chatter: </color>" + chatter.GetCommonData().m_audioEvent + " due to global chatter cooldown");
					return;
				}
			}
		}
		if (m_chatterAvailability.ContainsKey(chatter.GetCommonData().m_audioEvent))
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
			if (m_chatterAvailability[chatter.GetCommonData().m_audioEvent] > Time.time)
			{
				ChatterDebugLog("<color=red>Rejecting chatter: </color>" + chatter.GetCommonData().m_audioEvent + " due to individual chatter cooldown");
				return;
			}
		}
		if (m_chatterGroupAvailability.ContainsKey(chatter.GetCommonData().m_globalChatterGroup))
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
			if (m_chatterGroupAvailability[chatter.GetCommonData().m_globalChatterGroup] > Time.time)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						ChatterDebugLog("<color=red>Rejecting chatter: </color>" + chatter.GetCommonData().m_audioEvent + " due to group chatter cooldown");
						return;
					}
				}
			}
		}
		ChatterDebugLog("<color=yellow>Submitted chatter: </color>" + chatter.GetCommonData().m_audioEvent);
		float num = chatter.GetCommonData().m_priority;
		if ((bool)GameFlowData.Get())
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
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				if (GameFlowData.Get().activeOwnedActorData == source)
				{
					num *= m_localActorPriorityMultiplier;
				}
			}
		}
		m_submittedChatter.Add(new SubmittedChatter
		{
			chatter = chatter,
			source = source,
			calculatedPriority = num,
			timeSubmitted = Time.time
		});
	}

	public void SubmitConversation(ConversationTemplate conversation)
	{
		m_conversation = conversation;
		m_conversationIndex = 0;
	}

	public void CancelActiveChatter()
	{
		m_submittedChatter.Clear();
		if (m_currentlyPlayingChatter == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			PostChatterEvent(m_currentlyPlayingChatterTarget, m_currentlyPlayingChatter.GetCommonData().m_audioEvent, m_currentlyPlayingChatter, AudioManager.EventAction.StopSound);
			return;
		}
	}

	public void ForceCancelActiveChatter()
	{
		m_currentlyPlayingChatter = null;
	}

	private void PlayChatter(SubmittedChatter submission)
	{
		if (Options_UI.Get().GetChatterEnabled())
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
			if (EnableChatter)
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
				ChatterData commonData = submission.chatter.GetCommonData();
				string audioEventOverride = commonData.GetAudioEventOverride();
				if (string.IsNullOrEmpty(audioEventOverride))
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
					ChatterDebugLog("<color=white>Playing chatter: </color>" + commonData.m_audioEvent + ", priority " + submission.calculatedPriority);
					if (Random.Range(0f, 1f) > commonData.m_pctRatioVOToEmote)
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
						PostChatterEvent(submission.source.gameObject, commonData.m_audioEventEmote, submission.chatter);
					}
					else
					{
						PostChatterEvent(submission.source.gameObject, commonData.m_audioEvent, submission.chatter);
					}
				}
				else
				{
					ChatterDebugLog("<color=white>Playing chatter: </color>" + commonData.m_audioEvent + ", with override: " + audioEventOverride + ", priority " + submission.calculatedPriority);
					PostChatterEvent(submission.source.gameObject, audioEventOverride, submission.chatter);
				}
				if (commonData.m_cooldownTimeSeconds > 0f)
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
					m_chatterAvailability[commonData.m_audioEvent] = Time.time + commonData.m_cooldownTimeSeconds;
				}
				if (commonData.m_globalChatterGroup != 0)
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
					m_chatterGroupAvailability[commonData.m_globalChatterGroup] = Time.time + commonData.m_cooldownTimeSeconds;
				}
				m_timeSinceLastChatterStart = 0f;
				commonData.OnPlay();
				m_currentlyPlayingChatter = submission.chatter;
				m_currentlyPlayingChatterTarget = submission.source.gameObject;
				goto IL_0274;
			}
		}
		if (!submission.errorPrintCompleted)
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
			submission.errorPrintCompleted = true;
			ChatterDebugLog("Wanted to play chatter: " + submission.chatter.GetCommonData().m_audioEvent + " but the UI (state: " + Options_UI.Get().ActiveStateName + ") has chatter disabled");
		}
		goto IL_0274;
		IL_0274:
		using (Dictionary<string, float>.Enumerator enumerator = m_chatterAvailability.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator.MoveNext())
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
					break;
				}
				KeyValuePair<string, float> current = enumerator.Current;
				if (current.Value <= Time.time)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							m_chatterAvailability.Remove(current.Key);
							goto end_IL_0282;
						}
					}
				}
			}
			end_IL_0282:;
		}
		if (m_chatterGroupAvailability.Count <= 0)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			using (Dictionary<ChatterData.ChatterGroup, float>.Enumerator enumerator2 = m_chatterGroupAvailability.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<ChatterData.ChatterGroup, float> current2 = enumerator2.Current;
					if (current2.Value <= Time.time)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								m_chatterGroupAvailability.Remove(current2.Key);
								return;
							}
						}
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
		}
	}

	private OnEventNotify GenerateNotifyCallback(IChatterData chatterHandle)
	{
		return delegate(EventNotificationType type, string eventName, object info, GameObject gameObject)
		{
			OnFabricEventNotify(type, eventName, info, gameObject, chatterHandle);
		};
	}

	private void PostChatterEvent(GameObject target, string eventName, IChatterData chatterHandle, AudioManager.EventAction action = AudioManager.EventAction.PlaySound)
	{
		if (!target)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AudioManager.PostEventNotify(eventName, GenerateNotifyCallback(chatterHandle), target);
					return;
				}
			}
		}
		ActorData component = target.GetComponent<ActorData>();
		if ((bool)component)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					component.PostAudioEvent(eventName, GenerateNotifyCallback(chatterHandle), action);
					return;
				}
			}
		}
		ChatterComponent component2 = target.GetComponent<ChatterComponent>();
		if ((bool)component2 && (bool)UIFrontEnd.GetVisibleCharacters())
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
			if (component2.GetCharacterResourceLink() == UIFrontEnd.GetVisibleCharacters().CharacterResourceLinkInSlot(0))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						string text = UIFrontEnd.GetVisibleCharacters().CharacterResourceLinkInSlot(0).ReplaceAudioEvent(eventName, UICharacterSelectWorldObjects.Get().CharacterVisualInfoInSlot(0));
						if (text != eventName)
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
						}
						AudioManager.PostEventNotify(text, GenerateNotifyCallback(chatterHandle), base.gameObject);
						return;
					}
					}
				}
			}
		}
		Log.Warning("Unknown chatter event source {0} while playing chatter {1}", target, eventName);
	}

	private void OnFabricEventNotify(EventNotificationType type, string eventName, object info, GameObject gameObject, IChatterData chatterHandle)
	{
		if (type != 0)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (chatterHandle != m_currentlyPlayingChatter)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				ChatterDebugLog("Received finished callback from Fabric.");
				m_currentlyPlayingChatter = null;
				m_currentlyPlayingChatterTarget = null;
				if (m_chatterCooldownSec > 0f)
				{
					m_currentCooldownSec = m_chatterCooldownSec;
				}
				return;
			}
		}
	}

	private void ChatterDebugLog(string logStr)
	{
		if (!Application.isEditor || !(ActorDebugUtils.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.Chatter, false))
			{
				Log.Warning(Log.Category.ChatterAudio, "<color=orange>Chatter:</color> " + logStr + " @time " + Time.time);
			}
			return;
		}
	}
}
