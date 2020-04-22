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

	public AbilityTarget MainTarget
	{
		get
		{
			if (m_targets.Count > 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return m_targets[0];
					}
				}
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
	}

	internal AbilityRequest(IBitStream stream)
	{
		OnSerializeHelper(stream);
	}

	public void RequestCinematic(int animTauntIndex, int tauntUniqueId)
	{
		m_cinematicRequested = animTauntIndex;
	}

	public void CancelCinematic()
	{
		m_cinematicRequested = -1;
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_ability != null)
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
						if (abilityRequest.m_ability != null)
						{
							if (m_ability.RunPriority == abilityRequest.m_ability.RunPriority)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										if (!m_ability.IsFreeAction())
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
											if (!abilityRequest.m_ability.IsFreeAction())
											{
												return 0;
											}
											while (true)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												break;
											}
										}
										return m_ability.IsFreeAction().CompareTo(abilityRequest.m_ability.IsFreeAction());
									}
								}
							}
							return m_ability.RunPriority.CompareTo(abilityRequest.m_ability.RunPriority);
						}
					}
					return 1;
				}
			}
		}
		throw new ArgumentException("Object is not an AbilityRequest");
	}

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
				while (true)
				{
					switch (5)
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityTarget.SerializeAbilityTargetList(m_targets, stream);
			}
			if (stream.isReading)
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
				m_targets = AbilityTarget.DeSerializeAbilityTargetList(stream);
			}
			stream.Serialize(ref value4);
			stream.Serialize(ref value3);
		}
		m_actionType = (AbilityData.ActionType)value;
		m_caster = GameFlowData.Get().FindActorByActorIndex(value2);
		m_cinematicRequested = value4;
		m_resolveState = (AbilityResolveState)value3;
		m_ability = ((!(m_caster == null)) ? m_caster.GetAbilityData().GetAbilityOfActionType(m_actionType) : null);
	}
}
