using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientResolutionAction : IComparable
{
	private ResolutionActionType m_type;

	private ClientAbilityResults m_abilityResults;

	private ClientEffectResults m_effectResults;

	private ClientMovementResults m_moveResults;

	public ClientResolutionAction()
	{
		m_type = ResolutionActionType.Invalid;
		m_abilityResults = null;
		m_effectResults = null;
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		ClientResolutionAction clientResolutionAction = obj as ClientResolutionAction;
		if (clientResolutionAction != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (ReactsToMovement() != clientResolutionAction.ReactsToMovement())
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return ReactsToMovement().CompareTo(clientResolutionAction.ReactsToMovement());
							}
						}
					}
					if (!ReactsToMovement())
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
						if (!clientResolutionAction.ReactsToMovement())
						{
							return 0;
						}
					}
					float moveCost = m_moveResults.m_triggeringPath.moveCost;
					float moveCost2 = clientResolutionAction.m_moveResults.m_triggeringPath.moveCost;
					if (moveCost != moveCost2)
					{
						return moveCost.CompareTo(moveCost2);
					}
					bool flag = m_moveResults.HasBarrierHitResults();
					bool flag2 = clientResolutionAction.m_moveResults.HasBarrierHitResults();
					if (flag)
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
						if (!flag2)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									return -1;
								}
							}
						}
					}
					if (!flag)
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
						if (flag2)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return 1;
								}
							}
						}
					}
					bool flag3 = m_moveResults.HasGameModeHitResults();
					bool flag4 = clientResolutionAction.m_moveResults.HasGameModeHitResults();
					if (flag3)
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
						if (!flag4)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									return -1;
								}
							}
						}
					}
					if (!flag3)
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
						if (flag4)
						{
							return 1;
						}
					}
					return 0;
				}
				}
			}
		}
		throw new ArgumentException("Object is not a ClientResolutionAction");
	}

	public static ClientResolutionAction ClientResolutionAction_DeSerializeFromStream(ref IBitStream stream)
	{
		ClientResolutionAction clientResolutionAction = new ClientResolutionAction();
		sbyte value = -1;
		stream.Serialize(ref value);
		ResolutionActionType resolutionActionType = (ResolutionActionType)value;
		clientResolutionAction.m_type = (ResolutionActionType)value;
		if (resolutionActionType == ResolutionActionType.AbilityCast)
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
			clientResolutionAction.m_abilityResults = AbilityResultsUtils.DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		else
		{
			if (resolutionActionType != ResolutionActionType.EffectAnimation)
			{
				if (resolutionActionType != ResolutionActionType.EffectPulse)
				{
					if (resolutionActionType != ResolutionActionType.EffectOnMove && resolutionActionType != ResolutionActionType.BarrierOnMove)
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
						if (resolutionActionType != ResolutionActionType.PowerupOnMove)
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
							if (resolutionActionType != ResolutionActionType.GameModeOnMove)
							{
								goto IL_009e;
							}
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
					clientResolutionAction.m_moveResults = AbilityResultsUtils.DeSerializeClientMovementResultsFromStream(ref stream);
					goto IL_009e;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			clientResolutionAction.m_effectResults = AbilityResultsUtils.DeSerializeClientEffectResultsFromStream(ref stream);
		}
		goto IL_009e;
		IL_009e:
		return clientResolutionAction;
	}

	public ActorData GetCaster()
	{
		if (m_abilityResults != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_abilityResults.GetCaster();
				}
			}
		}
		if (m_effectResults != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_effectResults.GetCaster();
				}
			}
		}
		return null;
	}

	public AbilityData.ActionType GetSourceAbilityActionType()
	{
		if (m_abilityResults != null)
		{
			return m_abilityResults.GetSourceActionType();
		}
		if (m_effectResults != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_effectResults.GetSourceActionType();
				}
			}
		}
		return AbilityData.ActionType.INVALID_ACTION;
	}

	public bool IsResolutionActionType(ResolutionActionType testType)
	{
		return m_type == testType;
	}

	public bool HasReactionHitByCaster(ActorData caster)
	{
		if (m_abilityResults != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_abilityResults.HasReactionByCaster(caster);
				}
			}
		}
		if (m_effectResults != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_effectResults.HasReactionByCaster(caster);
				}
			}
		}
		return false;
	}

	public void GetHitResults(out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (m_abilityResults != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					actorHitResList = m_abilityResults.GetActorHitResults();
					posHitResList = m_abilityResults.GetPosHitResults();
					return;
				}
			}
		}
		if (m_effectResults == null)
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
			actorHitResList = m_effectResults.GetActorHitResults();
			posHitResList = m_effectResults.GetPosHitResults();
			return;
		}
	}

	public void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (m_abilityResults != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
					return;
				}
			}
		}
		if (m_effectResults == null)
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
			m_effectResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
			return;
		}
	}

	public void RunStartSequences()
	{
		if (m_abilityResults != null)
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
			m_abilityResults.StartSequences();
		}
		if (m_effectResults == null)
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
			m_effectResults.StartSequences();
			return;
		}
	}

	public void Run_OutsideResolution()
	{
		if (m_abilityResults != null)
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
			m_abilityResults.StartSequences();
		}
		if (m_effectResults != null)
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
			m_effectResults.StartSequences();
		}
		if (m_moveResults == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			m_moveResults.ReactToMovement();
			return;
		}
	}

	public bool CompletedAction()
	{
		if (m_type == ResolutionActionType.AbilityCast)
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
					return m_abilityResults.DoneHitting();
				}
			}
		}
		if (m_type != ResolutionActionType.EffectAnimation)
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
			if (m_type != ResolutionActionType.EffectPulse)
			{
				if (m_type != ResolutionActionType.EffectOnMove)
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
					if (m_type != ResolutionActionType.BarrierOnMove)
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
						if (m_type != ResolutionActionType.PowerupOnMove)
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
							if (m_type != ResolutionActionType.GameModeOnMove)
							{
								Debug.LogError("ClientResolutionAction has unknown type: " + (int)m_type + ".  Assuming it's complete...");
								return true;
							}
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
				}
				return m_moveResults.DoneHitting();
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return m_effectResults.DoneHitting();
	}

	public void ExecuteUnexecutedClientHitsInAction()
	{
		if (m_type == ResolutionActionType.AbilityCast)
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
					m_abilityResults.ExecuteUnexecutedClientHits();
					return;
				}
			}
		}
		if (m_type != ResolutionActionType.EffectAnimation)
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
			if (m_type != ResolutionActionType.EffectPulse)
			{
				if (m_type != ResolutionActionType.EffectOnMove)
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
					if (m_type != ResolutionActionType.BarrierOnMove && m_type != ResolutionActionType.PowerupOnMove)
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
						if (m_type != ResolutionActionType.GameModeOnMove)
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
							break;
						}
					}
				}
				m_moveResults.ExecuteUnexecutedClientHits();
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_effectResults.ExecuteUnexecutedClientHits();
	}

	public bool HasUnexecutedHitOnActor(ActorData actor)
	{
		bool result = false;
		if (m_type == ResolutionActionType.AbilityCast)
		{
			result = m_abilityResults.HasUnexecutedHitOnActor(actor);
		}
		else
		{
			if (m_type != ResolutionActionType.EffectAnimation)
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
				if (m_type != ResolutionActionType.EffectPulse)
				{
					if (m_type != ResolutionActionType.EffectOnMove)
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
						if (m_type != ResolutionActionType.BarrierOnMove && m_type != ResolutionActionType.PowerupOnMove)
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
							if (m_type != ResolutionActionType.GameModeOnMove)
							{
								goto IL_00a3;
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
					result = m_moveResults.HasUnexecutedHitOnActor(actor);
					goto IL_00a3;
				}
			}
			result = m_effectResults.HasUnexecutedHitOnActor(actor);
		}
		goto IL_00a3;
		IL_00a3:
		return result;
	}

	public void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		if (m_type == ResolutionActionType.AbilityCast)
		{
			m_abilityResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
			return;
		}
		if (m_type != ResolutionActionType.EffectAnimation)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_type != ResolutionActionType.EffectPulse)
			{
				if (m_type != ResolutionActionType.EffectOnMove)
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
					if (m_type != ResolutionActionType.BarrierOnMove)
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
						if (m_type != ResolutionActionType.PowerupOnMove)
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
							if (m_type != ResolutionActionType.GameModeOnMove)
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
								break;
							}
						}
					}
				}
				m_moveResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_effectResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
	}

	public static bool DoneHitting(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults)
	{
		bool flag = true;
		bool flag2 = true;
		foreach (ClientActorHitResults value in actorToHitResults.Values)
		{
			if (value.ExecutedHit)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (!value.HasUnexecutedReactionHits())
						{
							goto IL_004b;
						}
						goto IL_0047;
					}
				}
			}
			goto IL_0047;
			IL_0047:
			flag = false;
			break;
			IL_004b:;
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.ValueCollection.Enumerator enumerator2 = positionHitResults.Values.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator2.MoveNext())
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
				ClientPositionHitResults current2 = enumerator2.Current;
				if (!current2.ExecutedHit)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							flag2 = false;
							goto end_IL_0077;
						}
					}
				}
			}
			end_IL_0077:;
		}
		return flag && flag2;
	}

	public static bool HasUnexecutedHitOnActor(ActorData targetActor, Dictionary<ActorData, ClientActorHitResults> actorToHitResults)
	{
		bool result = false;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> current = enumerator.Current;
				ClientActorHitResults value = current.Value;
				if (!value.ExecutedHit)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (current.Key.ActorIndex == targetActor.ActorIndex)
					{
						goto IL_006f;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (!value.HasUnexecutedReactionOnActor(targetActor))
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				goto IL_006f;
				IL_006f:
				return true;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public static void ExecuteUnexecutedHits(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults, ActorData caster)
	{
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> current = enumerator.Current;
				if (!current.Value.ExecutedHit)
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
					ActorData key = current.Key;
					if (ClientAbilityResults.LogMissingSequences)
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
						Log.Warning(ClientAbilityResults.s_clientHitResultHeader + "Executing unexecuted actor hit on " + key.GetDebugName() + " from " + caster.GetDebugName());
					}
					current.Value.ExecuteActorHit(current.Key, caster);
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = positionHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> current2 = enumerator2.Current;
				if (!current2.Value.ExecutedHit)
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
					if (ClientAbilityResults.LogMissingSequences)
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
						Log.Warning(ClientAbilityResults.s_clientHitResultHeader + "Executing unexecuted position hit on " + current2.Key.ToString() + " from " + caster.GetDebugName());
					}
					current2.Value.ExecutePositionHit();
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public static void ExecuteReactionHitsWithExtraFlagsOnActorAux(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> actorToHitResult in actorToHitResults)
		{
			ActorData key = actorToHitResult.Key;
			ClientActorHitResults value = actorToHitResult.Value;
			if (!value.ExecutedHit)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (key == targetActor)
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
					value.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
				}
			}
		}
	}

	public static bool HasReactionHitByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> actorHitResult in actorHitResults)
		{
			if (actorHitResult.Value.HasReactionHitByCaster(caster))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	public static void GetReactionHitResultsByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults, out Dictionary<ActorData, ClientActorHitResults> reactionActorHits, out Dictionary<Vector3, ClientPositionHitResults> reactionPosHits)
	{
		reactionActorHits = null;
		reactionPosHits = null;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> current = enumerator.Current;
				if (current.Value.HasReactionHitByCaster(caster))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							current.Value.GetReactionHitResultsByCaster(caster, out reactionActorHits, out reactionPosHits);
							return;
						}
					}
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		return sequenceSource != null && ContainsSequenceSourceID(sequenceSource.RootID);
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		if (m_type == ResolutionActionType.AbilityCast)
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
					return m_abilityResults.ContainsSequenceSourceID(id);
				}
			}
		}
		if (m_type != ResolutionActionType.EffectAnimation)
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
			if (m_type != ResolutionActionType.EffectPulse)
			{
				if (m_type != ResolutionActionType.EffectOnMove)
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
					if (m_type != ResolutionActionType.BarrierOnMove)
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
						if (m_type != ResolutionActionType.PowerupOnMove)
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
							if (m_type != ResolutionActionType.GameModeOnMove)
							{
								Debug.LogError("ClientResolutionAction has unknown type: " + (int)m_type + ".  Assuming it does not have a given SequenceSource...");
								return false;
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
					}
				}
				return m_moveResults.ContainsSequenceSourceID(id);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return m_effectResults.ContainsSequenceSourceID(id);
	}

	public bool ReactsToMovement()
	{
		int result;
		if (m_type != ResolutionActionType.EffectOnMove && m_type != ResolutionActionType.BarrierOnMove)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_type != ResolutionActionType.PowerupOnMove)
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
				result = ((m_type == ResolutionActionType.GameModeOnMove) ? 1 : 0);
				goto IL_0044;
			}
		}
		result = 1;
		goto IL_0044;
		IL_0044:
		return (byte)result != 0;
	}

	public ActorData GetTriggeringMovementActor()
	{
		if (m_moveResults == null)
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
					return null;
				}
			}
		}
		return m_moveResults.m_triggeringMover;
	}

	public void OnActorMoved_ClientResolutionAction(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (!m_moveResults.TriggerMatchesMovement(mover, curPath))
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
			m_moveResults.ReactToMovement();
			return;
		}
	}

	public void AdjustKnockbackCounts_ClientResolutionAction(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
	{
		if (m_type == ResolutionActionType.AbilityCast)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityResults.AdjustKnockbackCounts_ClientAbilityResults(ref outgoingKnockbacks, ref incomingKnockbacks);
					return;
				}
			}
		}
		if (m_type != ResolutionActionType.EffectAnimation)
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
			if (m_type != ResolutionActionType.EffectPulse)
			{
				return;
			}
		}
		m_effectResults.AdjustKnockbackCounts_ClientEffectResults(ref outgoingKnockbacks, ref incomingKnockbacks);
	}

	public string GetDebugDescription()
	{
		string str = m_type.ToString() + ": ";
		if (m_type == ResolutionActionType.AbilityCast)
		{
			return str + m_abilityResults.GetDebugDescription();
		}
		if (m_type != ResolutionActionType.EffectAnimation)
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
			if (m_type != ResolutionActionType.EffectPulse)
			{
				if (m_type != ResolutionActionType.EffectOnMove)
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
					if (m_type != ResolutionActionType.BarrierOnMove)
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
						if (m_type != ResolutionActionType.PowerupOnMove)
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
							if (m_type != ResolutionActionType.GameModeOnMove)
							{
								return str + "??? (invalid results)";
							}
						}
					}
				}
				return str + m_moveResults.GetDebugDescription();
			}
		}
		return str + m_effectResults.GetDebugDescription();
	}

	public string GetUnexecutedHitsDebugStr(bool logSequenceDataActors = false)
	{
		string text;
		if (m_type == ResolutionActionType.AbilityCast)
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
			text = m_abilityResults.UnexecutedHitsDebugStr();
			if (logSequenceDataActors)
			{
				text = text + "\n" + m_abilityResults.GetSequenceStartDataDebugStr() + "\n";
			}
		}
		else if (m_type == ResolutionActionType.EffectAnimation || m_type == ResolutionActionType.EffectPulse)
		{
			text = m_effectResults.UnexecutedHitsDebugStr();
		}
		else
		{
			if (m_type != ResolutionActionType.EffectOnMove)
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
				if (m_type != ResolutionActionType.BarrierOnMove && m_type != ResolutionActionType.PowerupOnMove)
				{
					if (m_type != ResolutionActionType.GameModeOnMove)
					{
						text = string.Empty;
						goto IL_00bf;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			text = m_moveResults.UnexecutedHitsDebugStr();
		}
		goto IL_00bf;
		IL_00bf:
		return text;
	}

	public static string AssembleUnexecutedHitsDebugStr(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionToHitResults)
	{
		int num = 0;
		string text = string.Empty;
		int num2 = 0;
		string text2 = string.Empty;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> current = enumerator.Current;
				ActorData key = current.Key;
				ClientActorHitResults value = current.Value;
				if (!value.ExecutedHit)
				{
					num++;
					string text3 = text;
					text = text3 + "\n\t\t" + num + ". ActorHit on " + key.GetDebugName();
				}
				else
				{
					num2++;
					string text3 = text2;
					text2 = text3 + "\n\t\t" + num2 + ". ActorHit on " + key.GetDebugName();
				}
			}
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
					goto end_IL_001a;
				}
			}
			end_IL_001a:;
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = positionToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> current2 = enumerator2.Current;
				Vector3 key2 = current2.Key;
				ClientPositionHitResults value2 = current2.Value;
				if (!value2.ExecutedHit)
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
					num++;
					string text3 = text;
					text = text3 + "\n\t\t" + num + ". PositionHit on " + key2.ToString();
				}
				else
				{
					num2++;
					string text3 = text2;
					text2 = text3 + "\n\t\t" + num2 + ". PositionHit on " + key2.ToString();
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		string str = "\n\tUnexecuted hits: " + num + text;
		if (num2 > 0)
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
			str = str + "\n\tExecuted hits: " + num2 + text2;
		}
		return str + "\n";
	}
}
