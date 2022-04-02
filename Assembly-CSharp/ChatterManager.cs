using System;
using System.Collections.Generic;
using Fabric;
using UnityEngine;

public class ChatterManager : MonoBehaviour
{
	private static ChatterManager s_instance;

	[Tooltip("The amount of time to delay after one chatter finishes playing before new chatter submissions will be accepted.")]
	public float m_chatterCooldownSec;

	[Tooltip("The amount by which to multiply chatter priority when the source is the local player's character.")]
	public float m_localActorPriorityMultiplier = 1.5f;

	[Tooltip("The rate (in seconds) at which the manager decides which of the chatters submitted during that time to play.")]
	public float m_submissionIntervalSec = 0.75f;

	private const float m_currentChatterEndFallbackTime = 20f;

	private bool m_enableChatter = true;

	private List<ChatterManager.SubmittedChatter> m_submittedChatter = new List<ChatterManager.SubmittedChatter>();

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

	public static ChatterManager Get()
	{
		return ChatterManager.s_instance;
	}

	public bool EnableChatter
	{
		get
		{
			return this.m_enableChatter;
		}
		set
		{
			this.m_enableChatter = value;
		}
	}

	private void Awake()
	{
		ChatterManager.s_instance = this;
	}

	private void OnDestroy()
	{
		ChatterManager.s_instance = null;
	}

	private void Update()
	{
		if (this.m_currentCooldownSec > 0f)
		{
			this.m_currentCooldownSec -= Time.deltaTime;
		}
		this.m_currentIntervalSec += Time.deltaTime;
		if (this.m_currentlyPlayingChatter != null)
		{
			this.m_timeSinceLastChatterStart += Time.deltaTime;
			if (this.m_timeSinceLastChatterStart > 20f)
			{
				this.ChatterDebugLog(string.Format("Current Chatter has been cleared - timeout has been exceeded waiting for an end event for audio event [{0}].", this.m_currentlyPlayingChatter.GetCommonData().m_audioEvent));
				this.m_currentlyPlayingChatter = null;
				this.m_currentlyPlayingChatterTarget = null;
				if (this.m_chatterCooldownSec > 0f)
				{
					this.m_currentCooldownSec = this.m_chatterCooldownSec;
				}
			}
		}
		if (this.m_currentIntervalSec >= this.m_submissionIntervalSec && this.m_currentlyPlayingChatter == null)
		{
			this.m_currentIntervalSec %= this.m_submissionIntervalSec;
			if (this.m_conversation != null)
			{
				this.PlayChatter(new ChatterManager.SubmittedChatter
				{
					chatter = new ChatterDataStub(this.m_conversation.m_lines[this.m_conversationIndex].m_line),
					source = GameFlowData.Get().activeOwnedActorData.gameObject,
					calculatedPriority = 1000f,
					timeSubmitted = Time.time
				});
				this.m_conversationIndex++;
				if (this.m_conversation.m_lines.Length <= this.m_conversationIndex)
				{
					this.m_conversation = null;
					this.m_conversationIndex = 0;
				}
				this.m_submittedChatter.Clear();
			}
			else if (this.m_submittedChatter.Count > 0)
			{
				if (Time.time - this.m_submittedChatter[0].timeSubmitted >= this.m_submittedChatter[0].chatter.GetCommonData().m_chatterDelay)
				{
					this.m_submittedChatter.Sort((ChatterManager.SubmittedChatter a, ChatterManager.SubmittedChatter b) => -a.calculatedPriority.CompareTo(b.calculatedPriority));
					this.PlayChatter(this.m_submittedChatter[0]);
					this.m_submittedChatter.Clear();
				}
			}
		}
	}

	public void SubmitChatter(IChatterData chatter, GameObject source)
	{
		if (this.m_currentlyPlayingChatter != null)
		{
			this.ChatterDebugLog(string.Concat(new string[]
			{
				"<color=red>Rejecting chatter: </color>",
				chatter.GetCommonData().m_audioEvent,
				" due to existing chatter ",
				this.m_currentlyPlayingChatter.GetCommonData().m_audioEvent,
				" still playing"
			}));
			return;
		}
		if (this.m_currentCooldownSec > 0f)
		{
			this.ChatterDebugLog("<color=red>Rejecting chatter: </color>" + chatter.GetCommonData().m_audioEvent + " due to global chatter cooldown");
			return;
		}
		if (this.m_chatterAvailability.ContainsKey(chatter.GetCommonData().m_audioEvent))
		{
			if (this.m_chatterAvailability[chatter.GetCommonData().m_audioEvent] > Time.time)
			{
				this.ChatterDebugLog("<color=red>Rejecting chatter: </color>" + chatter.GetCommonData().m_audioEvent + " due to individual chatter cooldown");
				return;
			}
		}
		if (this.m_chatterGroupAvailability.ContainsKey(chatter.GetCommonData().m_globalChatterGroup))
		{
			if (this.m_chatterGroupAvailability[chatter.GetCommonData().m_globalChatterGroup] > Time.time)
			{
				this.ChatterDebugLog("<color=red>Rejecting chatter: </color>" + chatter.GetCommonData().m_audioEvent + " due to group chatter cooldown");
				return;
			}
		}
		this.ChatterDebugLog("<color=yellow>Submitted chatter: </color>" + chatter.GetCommonData().m_audioEvent);
		float num = (float)chatter.GetCommonData().m_priority;
		if (GameFlowData.Get())
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (GameFlowData.Get().activeOwnedActorData == source)
				{
					num *= this.m_localActorPriorityMultiplier;
				}
			}
		}
		this.m_submittedChatter.Add(new ChatterManager.SubmittedChatter
		{
			chatter = chatter,
			source = source,
			calculatedPriority = num,
			timeSubmitted = Time.time
		});
	}

	public void SubmitConversation(ConversationTemplate conversation)
	{
		this.m_conversation = conversation;
		this.m_conversationIndex = 0;
	}

	public void CancelActiveChatter()
	{
		this.m_submittedChatter.Clear();
		if (this.m_currentlyPlayingChatter != null)
		{
			this.PostChatterEvent(this.m_currentlyPlayingChatterTarget, this.m_currentlyPlayingChatter.GetCommonData().m_audioEvent, this.m_currentlyPlayingChatter, AudioManager.EventAction.StopSound);
		}
	}

	public void ForceCancelActiveChatter()
	{
		this.m_currentlyPlayingChatter = null;
	}

	private void PlayChatter(ChatterManager.SubmittedChatter submission)
	{
		// TODO HACK
#if SERVER
		return; // log fix
#endif

		if (Options_UI.Get().GetChatterEnabled())
		{
			if (this.EnableChatter)
			{
				ChatterData commonData = submission.chatter.GetCommonData();
				string audioEventOverride = commonData.GetAudioEventOverride();
				if (string.IsNullOrEmpty(audioEventOverride))
				{
					this.ChatterDebugLog(string.Concat(new object[]
					{
						"<color=white>Playing chatter: </color>",
						commonData.m_audioEvent,
						", priority ",
						submission.calculatedPriority
					}));
					if (UnityEngine.Random.Range(0f, 1f) > commonData.m_pctRatioVOToEmote)
					{
						this.PostChatterEvent(submission.source.gameObject, commonData.m_audioEventEmote, submission.chatter, AudioManager.EventAction.PlaySound);
					}
					else
					{
						this.PostChatterEvent(submission.source.gameObject, commonData.m_audioEvent, submission.chatter, AudioManager.EventAction.PlaySound);
					}
				}
				else
				{
					this.ChatterDebugLog(string.Concat(new object[]
					{
						"<color=white>Playing chatter: </color>",
						commonData.m_audioEvent,
						", with override: ",
						audioEventOverride,
						", priority ",
						submission.calculatedPriority
					}));
					this.PostChatterEvent(submission.source.gameObject, audioEventOverride, submission.chatter, AudioManager.EventAction.PlaySound);
				}
				if (commonData.m_cooldownTimeSeconds > 0f)
				{
					this.m_chatterAvailability[commonData.m_audioEvent] = Time.time + commonData.m_cooldownTimeSeconds;
				}
				if (commonData.m_globalChatterGroup != ChatterData.ChatterGroup.None)
				{
					this.m_chatterGroupAvailability[commonData.m_globalChatterGroup] = Time.time + commonData.m_cooldownTimeSeconds;
				}
				this.m_timeSinceLastChatterStart = 0f;
				commonData.OnPlay();
				this.m_currentlyPlayingChatter = submission.chatter;
				this.m_currentlyPlayingChatterTarget = submission.source.gameObject;
				goto IL_274;
			}
		}
		if (!submission.errorPrintCompleted)
		{
			submission.errorPrintCompleted = true;
			this.ChatterDebugLog(string.Concat(new string[]
			{
				"Wanted to play chatter: ",
				submission.chatter.GetCommonData().m_audioEvent,
				" but the UI (state: ",
				Options_UI.Get().ActiveStateName,
				") has chatter disabled"
			}));
		}
		IL_274:
		using (Dictionary<string, float>.Enumerator enumerator = this.m_chatterAvailability.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, float> keyValuePair = enumerator.Current;
				if (keyValuePair.Value <= Time.time)
				{
					this.m_chatterAvailability.Remove(keyValuePair.Key);
					goto IL_2E4;
				}
			}
		}
		IL_2E4:
		if (this.m_chatterGroupAvailability.Count > 0)
		{
			using (Dictionary<ChatterData.ChatterGroup, float>.Enumerator enumerator2 = this.m_chatterGroupAvailability.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<ChatterData.ChatterGroup, float> keyValuePair2 = enumerator2.Current;
					if (keyValuePair2.Value <= Time.time)
					{
						this.m_chatterGroupAvailability.Remove(keyValuePair2.Key);
						return;
					}
				}
			}
		}
	}

	private OnEventNotify GenerateNotifyCallback(IChatterData chatterHandle)
	{
		return delegate(EventNotificationType type, string eventName, object info, GameObject gameObject)
		{
			this.OnFabricEventNotify(type, eventName, info, gameObject, chatterHandle);
		};
	}

	private void PostChatterEvent(GameObject target, string eventName, IChatterData chatterHandle, AudioManager.EventAction action = AudioManager.EventAction.PlaySound)
	{
		if (!target)
		{
			AudioManager.PostEventNotify(eventName, this.GenerateNotifyCallback(chatterHandle), target);
			return;
		}
		ActorData component = target.GetComponent<ActorData>();
		if (component)
		{
			component.PostAudioEvent(eventName, this.GenerateNotifyCallback(chatterHandle), action);
			return;
		}
		ChatterComponent component2 = target.GetComponent<ChatterComponent>();
		if (component2 && UIFrontEnd.GetVisibleCharacters())
		{
			if (component2.GetCharacterResourceLink() == UIFrontEnd.GetVisibleCharacters().CharacterResourceLinkInSlot(0))
			{
				string text = UIFrontEnd.GetVisibleCharacters().CharacterResourceLinkInSlot(0).ReplaceAudioEvent(eventName, UICharacterSelectWorldObjects.Get().CharacterVisualInfoInSlot(0));
				if (text != eventName)
				{
				}
				AudioManager.PostEventNotify(text, this.GenerateNotifyCallback(chatterHandle), base.gameObject);
				return;
			}
		}
		Log.Warning("Unknown chatter event source {0} while playing chatter {1}", new object[]
		{
			target,
			eventName
		});
	}

	private void OnFabricEventNotify(EventNotificationType type, string eventName, object info, GameObject gameObject, IChatterData chatterHandle)
	{
		if (type == EventNotificationType.OnFinished)
		{
			if (chatterHandle == this.m_currentlyPlayingChatter)
			{
				this.ChatterDebugLog("Received finished callback from Fabric.");
				this.m_currentlyPlayingChatter = null;
				this.m_currentlyPlayingChatterTarget = null;
				if (this.m_chatterCooldownSec > 0f)
				{
					this.m_currentCooldownSec = this.m_chatterCooldownSec;
				}
			}
		}
	}

	private void ChatterDebugLog(string logStr)
	{
		if (Application.isEditor && ActorDebugUtils.Get() != null)
		{
			if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.Chatter, false))
			{
				Log.Warning(Log.Category.ChatterAudio, string.Concat(new object[]
				{
					"<color=orange>Chatter:</color> ",
					logStr,
					" @time ",
					Time.time
				}), new object[0]);
			}
		}
	}

	private struct SubmittedChatter
	{
		public IChatterData chatter;

		public GameObject source;

		public float calculatedPriority;

		public float timeSubmitted;

		public bool errorPrintCompleted;
	}
}
