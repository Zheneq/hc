// ROGUES
// SERVER
using System;
using System.Collections.Generic;

public class AbilityRequest : IComparable
{
	public enum AbilityResolveState
	{
		QUEUED,
		RESOLVING,
		RESOLVED
	}

	public Ability m_ability;
	public AbilityData.ActionType m_actionType;
	public ActorData m_caster;
	public List<AbilityTarget> m_targets;
	public int m_cinematicRequested;
	public AbilityResolveState m_resolveState;

	// TODO server-only or rogues-only?
#if SERVER
	// added in rogues
	internal bool m_stillValid; // was public in rogues
	// added in rogues
	internal ServerAbilityUtils.AbilityRunData m_additionalData; // was public in rogues
	// added in rogues
	internal int m_tauntUniqueId; // was public in rogues
#endif

	public AbilityTarget MainTarget
	{
		get
		{
			if (m_targets.Count > 0)
			{
				return m_targets[0];
			}
			return null;
		}
		private set
		{
		}
	}

	public AbilityRequest(Ability ability, AbilityData.ActionType actionType, List<AbilityTarget> targets, ActorData caster)
	{
		m_ability = ability;
		m_actionType = actionType;
		m_caster = caster;
		m_targets = new List<AbilityTarget>(targets);
		m_cinematicRequested = -1;
		m_resolveState = AbilityResolveState.QUEUED;

		// added in rogues
#if SERVER
		m_additionalData = new ServerAbilityUtils.AbilityRunData(this);
		m_stillValid = true;
#endif
	}

	// removed in rogues
	internal AbilityRequest(IBitStream stream)
	{
		OnSerializeHelper(stream);
	}

	public void RequestCinematic(int animTauntIndex, int tauntUniqueId)
	{
		m_cinematicRequested = animTauntIndex;

		// added in rogues
#if SERVER
		m_additionalData.m_abilityResults.CinematicRequested = animTauntIndex;
		m_tauntUniqueId = tauntUniqueId;
#endif
	}

	public void CancelCinematic()
	{
		m_cinematicRequested = -1;

		// added in rogues
#if SERVER
		m_additionalData.m_abilityResults.CinematicRequested = -1;
		m_tauntUniqueId = -1;
#endif
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		AbilityRequest abilityRequest = obj as AbilityRequest;
		if (abilityRequest == null)
		{
			throw new ArgumentException("Object is not an AbilityRequest");
		}
		if (m_ability == null || abilityRequest.m_ability == null)
		{
			return 1;
		}
		if (m_ability.RunPriority != abilityRequest.m_ability.RunPriority)
		{
			return m_ability.RunPriority.CompareTo(abilityRequest.m_ability.RunPriority);
		}
		if (m_ability.IsFreeAction() || abilityRequest.m_ability.IsFreeAction())
		{
			return m_ability.IsFreeAction().CompareTo(abilityRequest.m_ability.IsFreeAction());
		}
		return 0;
	}

	// removed in rogues
	internal void OnSerializeHelper(IBitStream stream)
	{
		sbyte value;
		sbyte value2;
		sbyte value3;
		int value4;
		checked
		{
			value = (sbyte)m_actionType;
			int num;
			if (m_caster == null)
			{
				num = ActorData.s_invalidActorIndex;
			}
			else
			{
				num = m_caster.ActorIndex;
			}
			value2 = (sbyte)num;
			value3 = (sbyte)m_resolveState;
			value4 = m_cinematicRequested;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			if (stream.isWriting)
			{
				AbilityTarget.SerializeAbilityTargetList(m_targets, stream);
			}
			if (stream.isReading)
			{
				m_targets = AbilityTarget.DeSerializeAbilityTargetList(stream);
			}
			stream.Serialize(ref value4);
			stream.Serialize(ref value3);
		}
		m_actionType = (AbilityData.ActionType)value;
		m_caster = GameFlowData.Get().FindActorByActorIndex(value2);
		m_cinematicRequested = value4;
		m_resolveState = (AbilityResolveState)value3;
		m_ability = m_caster?.GetAbilityData().GetAbilityOfActionType(m_actionType);
	}
}
