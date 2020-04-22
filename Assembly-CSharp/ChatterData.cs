using System;
using UnityEngine;

[Serializable]
public class ChatterData
{
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

	public Locality m_locality;

	[AudioEvent(false)]
	public string m_audioEvent;

	public string m_audioEventEmote;

	public int m_priority = 1;

	public float m_pctChanceToPlay = 1f;

	public float m_pctRatioVOToEmote = 1f;

	public ChatterGroup m_globalChatterGroup;

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
			while (true)
			{
				switch (2)
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
			if (chatter.GetCommonData().m_playedThisTurn)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		if (eventType != chatter.GetActivateOnEvent())
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
		if ((bool)TheatricsManager.Get())
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
			if (TheatricsManager.Get().IsCinematicPlaying())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num > chatter.GetCommonData().m_pctChanceToPlay)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		Locality locality = chatter.GetCommonData().m_locality;
		if (locality != 0)
		{
			GameFlowData gameFlowData = GameFlowData.Get();
			if (gameFlowData == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					return false;
				}
			}
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				ActorData component2 = component.gameObject.GetComponent<ActorData>();
				if (component2 == null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						Log.Error($"Chatter component {component} is on an object that does not have ActorData; non-All locality isn't allowed. (audio event {chatter.GetCommonData().m_audioEvent})");
						return false;
					}
				}
				if (locality == Locality.LocalOnly)
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
					if (activeOwnedActorData != component2)
					{
						return false;
					}
				}
				if (locality == Locality.TeamOnly)
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
		if (m_oncePerTurn)
		{
			m_playedThisTurn = false;
			m_audioEventOverride = null;
		}
	}

	public void OnPlay()
	{
		if (!m_oncePerTurn)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_playedThisTurn = true;
			m_audioEventOverride = null;
			return;
		}
	}

	public void ClearAudioEventOverride()
	{
		m_audioEventOverride = null;
	}

	public void SetAudioEventOverride(string eventOverride)
	{
		m_audioEventOverride = eventOverride;
	}

	public string GetAudioEventOverride()
	{
		return m_audioEventOverride;
	}
}
