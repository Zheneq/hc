using System;
using System.Collections.Generic;

public class AbilityRequest : IComparable
{
	public Ability m_ability;

	public AbilityData.ActionType m_actionType;

	public ActorData m_caster;

	public List<AbilityTarget> m_targets;

	public int m_cinematicRequested;

	public AbilityRequest.AbilityResolveState m_resolveState;

	public AbilityRequest(Ability ability, AbilityData.ActionType actionType, List<AbilityTarget> targets, ActorData caster)
	{
		this.m_ability = ability;
		this.m_actionType = actionType;
		this.m_caster = caster;
		this.m_targets = new List<AbilityTarget>(targets);
		this.m_cinematicRequested = -1;
		this.m_resolveState = AbilityRequest.AbilityResolveState.QUEUED;
	}

	internal AbilityRequest(IBitStream stream)
	{
		this.OnSerializeHelper(stream);
	}

	public void RequestCinematic(int animTauntIndex, int tauntUniqueId)
	{
		this.m_cinematicRequested = animTauntIndex;
	}

	public void CancelCinematic()
	{
		this.m_cinematicRequested = -1;
	}

	public AbilityTarget MainTarget
	{
		get
		{
			if (this.m_targets.Count > 0)
			{
				return this.m_targets[0];
			}
			return null;
		}
		private set
		{
		}
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		AbilityRequest abilityRequest = obj as AbilityRequest;
		if (abilityRequest != null)
		{
			if (this.m_ability != null)
			{
				if (abilityRequest.m_ability != null)
				{
					if (this.m_ability.RunPriority == abilityRequest.m_ability.RunPriority)
					{
						if (!this.m_ability.IsFreeAction())
						{
							if (!abilityRequest.m_ability.IsFreeAction())
							{
								return 0;
							}
						}
						return this.m_ability.IsFreeAction().CompareTo(abilityRequest.m_ability.IsFreeAction());
					}
					return this.m_ability.RunPriority.CompareTo(abilityRequest.m_ability.RunPriority);
				}
			}
			return 1;
		}
		throw new ArgumentException("Object is not an AbilityRequest");
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		sbyte b;
		sbyte b3;
		sbyte b4;
		int cinematicRequested;
		checked
		{
			b = (sbyte)this.m_actionType;
			sbyte b2;
			if (this.m_caster == null)
			{
				b2 = (sbyte)ActorData.s_invalidActorIndex;
			}
			else
			{
				b2 = (sbyte)this.m_caster.ActorIndex;
			}
			b3 = b2;
			b4 = (sbyte)this.m_resolveState;
			cinematicRequested = this.m_cinematicRequested;
			stream.Serialize(ref b);
			stream.Serialize(ref b3);
			if (stream.isWriting)
			{
				AbilityTarget.SerializeAbilityTargetList(this.m_targets, stream);
			}
			if (stream.isReading)
			{
				this.m_targets = AbilityTarget.DeSerializeAbilityTargetList(stream);
			}
			stream.Serialize(ref cinematicRequested);
			stream.Serialize(ref b4);
		}
		this.m_actionType = (AbilityData.ActionType)b;
		this.m_caster = GameFlowData.Get().FindActorByActorIndex((int)b3);
		this.m_cinematicRequested = cinematicRequested;
		this.m_resolveState = (AbilityRequest.AbilityResolveState)b4;
		this.m_ability = ((!(this.m_caster == null)) ? this.m_caster.GetAbilityData().GetAbilityOfActionType(this.m_actionType) : null);
	}

	public enum AbilityResolveState
	{
		QUEUED,
		RESOLVING,
		RESOLVED
	}
}
