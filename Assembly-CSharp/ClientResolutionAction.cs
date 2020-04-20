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
		this.m_type = ResolutionActionType.Invalid;
		this.m_abilityResults = null;
		this.m_effectResults = null;
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		ClientResolutionAction clientResolutionAction = obj as ClientResolutionAction;
		if (clientResolutionAction == null)
		{
			throw new ArgumentException("Object is not a ClientResolutionAction");
		}
		if (this.ReactsToMovement() != clientResolutionAction.ReactsToMovement())
		{
			return this.ReactsToMovement().CompareTo(clientResolutionAction.ReactsToMovement());
		}
		if (!this.ReactsToMovement())
		{
			if (!clientResolutionAction.ReactsToMovement())
			{
				return 0;
			}
		}
		float moveCost = this.m_moveResults.m_triggeringPath.moveCost;
		float moveCost2 = clientResolutionAction.m_moveResults.m_triggeringPath.moveCost;
		if (moveCost != moveCost2)
		{
			return moveCost.CompareTo(moveCost2);
		}
		bool flag = this.m_moveResults.HasBarrierHitResults();
		bool flag2 = clientResolutionAction.m_moveResults.HasBarrierHitResults();
		if (flag)
		{
			if (!flag2)
			{
				return -1;
			}
		}
		if (!flag)
		{
			if (flag2)
			{
				return 1;
			}
		}
		bool flag3 = this.m_moveResults.HasGameModeHitResults();
		bool flag4 = clientResolutionAction.m_moveResults.HasGameModeHitResults();
		if (flag3)
		{
			if (!flag4)
			{
				return -1;
			}
		}
		if (!flag3)
		{
			if (flag4)
			{
				return 1;
			}
		}
		return 0;
	}

	public unsafe static ClientResolutionAction ClientResolutionAction_DeSerializeFromStream(ref IBitStream stream)
	{
		ClientResolutionAction clientResolutionAction = new ClientResolutionAction();
		sbyte b = -1;
		stream.Serialize(ref b);
		ResolutionActionType resolutionActionType = (ResolutionActionType)b;
		clientResolutionAction.m_type = (ResolutionActionType)b;
		if (resolutionActionType == ResolutionActionType.AbilityCast)
		{
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
						if (resolutionActionType != ResolutionActionType.PowerupOnMove)
						{
							if (resolutionActionType != ResolutionActionType.GameModeOnMove)
							{
								return clientResolutionAction;
							}
						}
					}
					clientResolutionAction.m_moveResults = AbilityResultsUtils.DeSerializeClientMovementResultsFromStream(ref stream);
					return clientResolutionAction;
				}
			}
			clientResolutionAction.m_effectResults = AbilityResultsUtils.DeSerializeClientEffectResultsFromStream(ref stream);
		}
		return clientResolutionAction;
	}

	public ActorData GetCaster()
	{
		if (this.m_abilityResults != null)
		{
			return this.m_abilityResults.GetCaster();
		}
		if (this.m_effectResults != null)
		{
			return this.m_effectResults.GetCaster();
		}
		return null;
	}

	public AbilityData.ActionType GetSourceAbilityActionType()
	{
		if (this.m_abilityResults != null)
		{
			return this.m_abilityResults.GetSourceActionType();
		}
		if (this.m_effectResults != null)
		{
			return this.m_effectResults.GetSourceActionType();
		}
		return AbilityData.ActionType.INVALID_ACTION;
	}

	public bool IsResolutionActionType(ResolutionActionType testType)
	{
		return this.m_type == testType;
	}

	public bool HasReactionHitByCaster(ActorData caster)
	{
		if (this.m_abilityResults != null)
		{
			return this.m_abilityResults.HasReactionByCaster(caster);
		}
		if (this.m_effectResults != null)
		{
			return this.m_effectResults.HasReactionByCaster(caster);
		}
		return false;
	}

	public unsafe void GetHitResults(out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (this.m_abilityResults != null)
		{
			actorHitResList = this.m_abilityResults.GetActorHitResults();
			posHitResList = this.m_abilityResults.GetPosHitResults();
		}
		else if (this.m_effectResults != null)
		{
			actorHitResList = this.m_effectResults.GetActorHitResults();
			posHitResList = this.m_effectResults.GetPosHitResults();
		}
	}

	public unsafe void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (this.m_abilityResults != null)
		{
			this.m_abilityResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
		}
		else if (this.m_effectResults != null)
		{
			this.m_effectResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
		}
	}

	public void RunStartSequences()
	{
		if (this.m_abilityResults != null)
		{
			this.m_abilityResults.StartSequences();
		}
		if (this.m_effectResults != null)
		{
			this.m_effectResults.StartSequences();
		}
	}

	public void Run_OutsideResolution()
	{
		if (this.m_abilityResults != null)
		{
			this.m_abilityResults.StartSequences();
		}
		if (this.m_effectResults != null)
		{
			this.m_effectResults.StartSequences();
		}
		if (this.m_moveResults != null)
		{
			this.m_moveResults.ReactToMovement();
		}
	}

	public bool CompletedAction()
	{
		bool result;
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			result = this.m_abilityResults.DoneHitting();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
			{
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
					{
						if (this.m_type != ResolutionActionType.BarrierOnMove)
						{
							if (this.m_type != ResolutionActionType.PowerupOnMove)
							{
								if (this.m_type != ResolutionActionType.GameModeOnMove)
								{
									Debug.LogError("ClientResolutionAction has unknown type: " + (int)this.m_type + ".  Assuming it's complete...");
									return true;
								}
							}
						}
					}
					return this.m_moveResults.DoneHitting();
				}
			}
			result = this.m_effectResults.DoneHitting();
		}
		return result;
	}

	public void ExecuteUnexecutedClientHitsInAction()
	{
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			this.m_abilityResults.ExecuteUnexecutedClientHits();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
			{
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
					{
						if (this.m_type != ResolutionActionType.BarrierOnMove && this.m_type != ResolutionActionType.PowerupOnMove)
						{
							if (this.m_type != ResolutionActionType.GameModeOnMove)
							{
								return;
							}
						}
					}
					this.m_moveResults.ExecuteUnexecutedClientHits();
					return;
				}
			}
			this.m_effectResults.ExecuteUnexecutedClientHits();
		}
	}

	public bool HasUnexecutedHitOnActor(ActorData actor)
	{
		bool result = false;
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			result = this.m_abilityResults.HasUnexecutedHitOnActor(actor);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
			{
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
					{
						if (this.m_type != ResolutionActionType.BarrierOnMove && this.m_type != ResolutionActionType.PowerupOnMove)
						{
							if (this.m_type != ResolutionActionType.GameModeOnMove)
							{
								return result;
							}
						}
					}
					return this.m_moveResults.HasUnexecutedHitOnActor(actor);
				}
			}
			result = this.m_effectResults.HasUnexecutedHitOnActor(actor);
		}
		return result;
	}

	public void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			this.m_abilityResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
			{
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
					{
						if (this.m_type != ResolutionActionType.BarrierOnMove)
						{
							if (this.m_type != ResolutionActionType.PowerupOnMove)
							{
								if (this.m_type != ResolutionActionType.GameModeOnMove)
								{
									return;
								}
							}
						}
					}
					this.m_moveResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
					return;
				}
			}
			this.m_effectResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
	}

	public static bool DoneHitting(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults)
	{
		bool flag = true;
		bool flag2 = true;
		foreach (ClientActorHitResults clientActorHitResults in actorToHitResults.Values)
		{
			if (clientActorHitResults.ExecutedHit)
			{
				if (!clientActorHitResults.HasUnexecutedReactionHits())
				{
					continue;
				}
			}
			flag = false;
			break;
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.ValueCollection.Enumerator enumerator2 = positionHitResults.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ClientPositionHitResults clientPositionHitResults = enumerator2.Current;
				if (!clientPositionHitResults.ExecutedHit)
				{
					flag2 = false;
					goto IL_C0;
				}
			}
		}
		IL_C0:
		return flag && flag2;
	}

	public static bool HasUnexecutedHitOnActor(ActorData targetActor, Dictionary<ActorData, ClientActorHitResults> actorToHitResults)
	{
		bool result = false;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				ClientActorHitResults value = keyValuePair.Value;
				if (!value.ExecutedHit)
				{
					if (keyValuePair.Key.ActorIndex == targetActor.ActorIndex)
					{
						goto IL_6F;
					}
				}
				if (!value.HasUnexecutedReactionOnActor(targetActor))
				{
					continue;
				}
				IL_6F:
				return true;
			}
		}
		return result;
	}

	public static void ExecuteUnexecutedHits(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults, ActorData caster)
	{
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				if (!keyValuePair.Value.ExecutedHit)
				{
					ActorData key = keyValuePair.Key;
					if (ClientAbilityResults.LogMissingSequences)
					{
						Log.Warning(string.Concat(new string[]
						{
							ClientAbilityResults.s_clientHitResultHeader,
							"Executing unexecuted actor hit on ",
							key.GetDebugName(),
							" from ",
							caster.GetDebugName()
						}), new object[0]);
					}
					keyValuePair.Value.ExecuteActorHit(keyValuePair.Key, caster);
				}
			}
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = positionHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> keyValuePair2 = enumerator2.Current;
				if (!keyValuePair2.Value.ExecutedHit)
				{
					if (ClientAbilityResults.LogMissingSequences)
					{
						Log.Warning(string.Concat(new string[]
						{
							ClientAbilityResults.s_clientHitResultHeader,
							"Executing unexecuted position hit on ",
							keyValuePair2.Key.ToString(),
							" from ",
							caster.GetDebugName()
						}), new object[0]);
					}
					keyValuePair2.Value.ExecutePositionHit();
				}
			}
		}
	}

	public static void ExecuteReactionHitsWithExtraFlagsOnActorAux(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> keyValuePair in actorToHitResults)
		{
			ActorData key = keyValuePair.Key;
			ClientActorHitResults value = keyValuePair.Value;
			if (!value.ExecutedHit)
			{
				if (key == targetActor)
				{
					value.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
				}
			}
		}
	}

	public static bool HasReactionHitByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> keyValuePair in actorHitResults)
		{
			if (keyValuePair.Value.HasReactionHitByCaster(caster))
			{
				return true;
			}
		}
		return false;
	}

	public unsafe static void GetReactionHitResultsByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults, out Dictionary<ActorData, ClientActorHitResults> reactionActorHits, out Dictionary<Vector3, ClientPositionHitResults> reactionPosHits)
	{
		reactionActorHits = null;
		reactionPosHits = null;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				if (keyValuePair.Value.HasReactionHitByCaster(caster))
				{
					keyValuePair.Value.GetReactionHitResultsByCaster(caster, out reactionActorHits, out reactionPosHits);
					return;
				}
			}
		}
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		return sequenceSource != null && this.ContainsSequenceSourceID(sequenceSource.RootID);
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		bool result;
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			result = this.m_abilityResults.ContainsSequenceSourceID(id);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
			{
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
					{
						if (this.m_type != ResolutionActionType.BarrierOnMove)
						{
							if (this.m_type != ResolutionActionType.PowerupOnMove)
							{
								if (this.m_type != ResolutionActionType.GameModeOnMove)
								{
									Debug.LogError("ClientResolutionAction has unknown type: " + (int)this.m_type + ".  Assuming it does not have a given SequenceSource...");
									return false;
								}
							}
						}
					}
					return this.m_moveResults.ContainsSequenceSourceID(id);
				}
			}
			result = this.m_effectResults.ContainsSequenceSourceID(id);
		}
		return result;
	}

	public bool ReactsToMovement()
	{
		if (this.m_type != ResolutionActionType.EffectOnMove && this.m_type != ResolutionActionType.BarrierOnMove)
		{
			if (this.m_type != ResolutionActionType.PowerupOnMove)
			{
				return this.m_type == ResolutionActionType.GameModeOnMove;
			}
		}
		return true;
	}

	public ActorData GetTriggeringMovementActor()
	{
		ActorData result;
		if (this.m_moveResults == null)
		{
			result = null;
		}
		else
		{
			result = this.m_moveResults.m_triggeringMover;
		}
		return result;
	}

	public void OnActorMoved_ClientResolutionAction(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (this.m_moveResults.TriggerMatchesMovement(mover, curPath))
		{
			this.m_moveResults.ReactToMovement();
		}
	}

	public unsafe void AdjustKnockbackCounts_ClientResolutionAction(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
	{
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			this.m_abilityResults.AdjustKnockbackCounts_ClientAbilityResults(ref outgoingKnockbacks, ref incomingKnockbacks);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
			{
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					return;
				}
			}
			this.m_effectResults.AdjustKnockbackCounts_ClientEffectResults(ref outgoingKnockbacks, ref incomingKnockbacks);
		}
	}

	public string GetDebugDescription()
	{
		string text = this.m_type.ToString() + ": ";
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			text += this.m_abilityResults.GetDebugDescription();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
			{
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
					{
						if (this.m_type != ResolutionActionType.BarrierOnMove)
						{
							if (this.m_type != ResolutionActionType.PowerupOnMove)
							{
								if (this.m_type != ResolutionActionType.GameModeOnMove)
								{
									return text + "??? (invalid results)";
								}
							}
						}
					}
					return text + this.m_moveResults.GetDebugDescription();
				}
			}
			text += this.m_effectResults.GetDebugDescription();
		}
		return text;
	}

	public string GetUnexecutedHitsDebugStr(bool logSequenceDataActors = false)
	{
		string text;
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			text = this.m_abilityResults.UnexecutedHitsDebugStr();
			if (logSequenceDataActors)
			{
				text = text + "\n" + this.m_abilityResults.GetSequenceStartDataDebugStr() + "\n";
			}
		}
		else if (this.m_type == ResolutionActionType.EffectAnimation || this.m_type == ResolutionActionType.EffectPulse)
		{
			text = this.m_effectResults.UnexecutedHitsDebugStr();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectOnMove)
			{
				if (this.m_type != ResolutionActionType.BarrierOnMove && this.m_type != ResolutionActionType.PowerupOnMove)
				{
					if (this.m_type != ResolutionActionType.GameModeOnMove)
					{
						return string.Empty;
					}
				}
			}
			text = this.m_moveResults.UnexecutedHitsDebugStr();
		}
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
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				ActorData key = keyValuePair.Key;
				ClientActorHitResults value = keyValuePair.Value;
				if (!value.ExecutedHit)
				{
					num++;
					string text3 = text;
					text = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num,
						". ActorHit on ",
						key.GetDebugName()
					});
				}
				else
				{
					num2++;
					string text3 = text2;
					text2 = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num2,
						". ActorHit on ",
						key.GetDebugName()
					});
				}
			}
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = positionToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> keyValuePair2 = enumerator2.Current;
				Vector3 key2 = keyValuePair2.Key;
				ClientPositionHitResults value2 = keyValuePair2.Value;
				if (!value2.ExecutedHit)
				{
					num++;
					string text3 = text;
					text = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num,
						". PositionHit on ",
						key2.ToString()
					});
				}
				else
				{
					num2++;
					string text3 = text2;
					text2 = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num2,
						". PositionHit on ",
						key2.ToString()
					});
				}
			}
		}
		string str = "\n\tUnexecuted hits: " + num.ToString() + text;
		if (num2 > 0)
		{
			str = str + "\n\tExecuted hits: " + num2.ToString() + text2;
		}
		return str + "\n";
	}
}
