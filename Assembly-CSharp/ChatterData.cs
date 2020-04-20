using System;
using UnityEngine;

[Serializable]
public class ChatterData
{
	public ChatterData.Locality m_locality;

	[AudioEvent(false)]
	public string m_audioEvent;

	public string m_audioEventEmote;

	public int m_priority = 1;

	public float m_pctChanceToPlay = 1f;

	public float m_pctRatioVOToEmote = 1f;

	public ChatterData.ChatterGroup m_globalChatterGroup;

	public float m_cooldownTimeSeconds;

	public float m_chatterDelay;

	public bool m_oncePerTurn;

	private bool m_playedThisTurn;

	private string m_audioEventOverride;

	public ChatterData GetCommonData()
	{
		return this;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.Invalid;
	}

	internal static bool ShouldPlayChatter(IChatterData chatter, GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (chatter.GetCommonData().m_oncePerTurn)
		{
			if (chatter.GetCommonData().m_playedThisTurn)
			{
				return false;
			}
		}
		if (eventType != chatter.GetActivateOnEvent())
		{
			return false;
		}
		if (TheatricsManager.Get())
		{
			if (TheatricsManager.Get().IsCinematicPlaying())
			{
				return false;
			}
		}
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num > chatter.GetCommonData().m_pctChanceToPlay)
		{
			return false;
		}
		ChatterData.Locality locality = chatter.GetCommonData().m_locality;
		if (locality != ChatterData.Locality.All)
		{
			GameFlowData gameFlowData = GameFlowData.Get();
			if (gameFlowData == null)
			{
				return false;
			}
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				ActorData component2 = component.gameObject.GetComponent<ActorData>();
				if (component2 == null)
				{
					Log.Error(string.Format("Chatter component {0} is on an object that does not have ActorData; non-All locality isn't allowed. (audio event {1})", component, chatter.GetCommonData().m_audioEvent), new object[0]);
					return false;
				}
				if (locality == ChatterData.Locality.LocalOnly)
				{
					if (activeOwnedActorData != component2)
					{
						return false;
					}
				}
				if (locality == ChatterData.Locality.TeamOnly)
				{
					if (activeOwnedActorData.GetTeam() != component2.GetTeam())
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public void OnTurnTick()
	{
		if (this.m_oncePerTurn)
		{
			this.m_playedThisTurn = false;
			this.m_audioEventOverride = null;
		}
	}

	public void OnPlay()
	{
		if (this.m_oncePerTurn)
		{
			this.m_playedThisTurn = true;
			this.m_audioEventOverride = null;
		}
	}

	public void ClearAudioEventOverride()
	{
		this.m_audioEventOverride = null;
	}

	public void SetAudioEventOverride(string eventOverride)
	{
		this.m_audioEventOverride = eventOverride;
	}

	public string GetAudioEventOverride()
	{
		return this.m_audioEventOverride;
	}

	public enum Locality
	{
		All,
		LocalOnly,
		TeamOnly
	}

	public enum ChatterGroup
	{
		None,
		FrontEndReady
	}
}
