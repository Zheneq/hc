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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityRequest.get_MainTarget()).MethodHandle;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityRequest.CompareTo(object)).MethodHandle;
			}
			if (this.m_ability != null)
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
				if (abilityRequest.m_ability != null)
				{
					if (this.m_ability.RunPriority == abilityRequest.m_ability.RunPriority)
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
						if (!this.m_ability.IsFreeAction())
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
							if (!abilityRequest.m_ability.IsFreeAction())
							{
								return 0;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityRequest.OnSerializeHelper(IBitStream)).MethodHandle;
				}
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityTarget.SerializeAbilityTargetList(this.m_targets, stream);
			}
			if (stream.isReading)
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
				this.m_targets = AbilityTarget.DeSerializeAbilityTargetList(stream);
			}
			stream.Serialize(ref cinematicRequested);
			stream.Serialize(ref b4);
		}
		this.m_actionType = (AbilityData.ActionType)b;
		this.m_caster = GameFlowData.Get().FindActorByActorIndex((int)b3);
		this.m_cinematicRequested = cinematicRequested;
		this.m_resolveState = (AbilityRequest.AbilityResolveState)b4;
		this.m_ability = ((!(this.m_caster == null)) ? this.m_caster.\u000E().GetAbilityOfActionType(this.m_actionType) : null);
	}

	public enum AbilityResolveState
	{
		QUEUED,
		RESOLVING,
		RESOLVED
	}
}
