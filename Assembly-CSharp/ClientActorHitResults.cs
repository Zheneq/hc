using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClientActorHitResults
{
	private bool m_hasDamage;

	private bool m_hasHealing;

	private bool m_hasTechPointGain;

	private bool m_hasTechPointLoss;

	private bool m_hasTechPointGainOnCaster;

	private bool m_hasKnockback;

	private ActorData m_knockbackSourceActor;

	private int m_finalDamage;

	private int m_finalHealing;

	private int m_finalTechPointsGain;

	private int m_finalTechPointsLoss;

	private int m_finalTechPointGainOnCaster;

	private bool m_damageBoosted;

	private bool m_damageReduced;

	private bool m_isPartOfHealOverTime;

	private bool m_updateCasterLastKnownPos;

	private bool m_updateTargetLastKnownPos;

	private bool m_triggerCasterVisOnHitVisualOnly;

	private bool m_updateEffectHolderLastKnownPos;

	private ActorData m_effectHolderActor;

	private bool m_updateOtherLastKnownPos;

	private List<ActorData> m_otherActorsToUpdateVisibility;

	private bool m_targetInCoverWrtDamage;

	private Vector3 m_damageHitOrigin;

	private bool m_canBeReactedTo;

	private bool m_isCharacterSpecificAbility;

	private List<ClientEffectStartData> m_effectsToStart;

	private List<int> m_effectsToRemove;

	private List<ClientBarrierStartData> m_barriersToAdd;

	private List<int> m_barriersToRemove;

	private List<ServerClientUtils.SequenceEndData> m_sequencesToEnd;

	private List<ClientReactionResults> m_reactions;

	private List<int> m_powerupsToRemove;

	private List<ClientPowerupStealData> m_powerupsToSteal;

	private List<ClientMovementResults> m_directPowerupHits;

	private List<ClientGameModeEvent> m_gameModeEvents;

	private List<int> m_overconIds;

	public bool ExecutedHit
	{
		get;
		private set;
	}

	public bool IsMovementHit
	{
		get;
		set;
	}

	public bool HasKnockback
	{
		get
		{
			return m_hasKnockback;
		}
		private set
		{
		}
	}

	public ActorData KnockbackSourceActor
	{
		get
		{
			return m_knockbackSourceActor;
		}
		private set
		{
		}
	}

	public ClientActorHitResults(ref IBitStream stream)
	{
		byte bitField1 = 0;
		stream.Serialize(ref bitField1);
		ServerClientUtils.GetBoolsFromBitfield(
			bitField1,
			out m_hasDamage,
			out m_hasHealing,
			out m_hasTechPointGain,
			out m_hasTechPointLoss,
			out m_hasTechPointGainOnCaster,
			out m_hasKnockback,
			out m_targetInCoverWrtDamage,
			out m_canBeReactedTo);

		byte bitField2 = 0;
		stream.Serialize(ref bitField2);
		ServerClientUtils.GetBoolsFromBitfield(
			bitField2,
			out m_damageBoosted,
			out m_damageReduced,
			out m_updateCasterLastKnownPos,
			out m_updateTargetLastKnownPos,
			out m_updateEffectHolderLastKnownPos,
			out m_updateOtherLastKnownPos,
			out m_isPartOfHealOverTime,
			out m_triggerCasterVisOnHitVisualOnly);

		if (m_hasDamage)
		{
			short finalDamage = 0;
			stream.Serialize(ref finalDamage);
			m_finalDamage = finalDamage;
		}
		if (m_hasHealing)
		{
			short finalHealing = 0;
			stream.Serialize(ref finalHealing);
			m_finalHealing = finalHealing;
		}
		if (m_hasTechPointGain)
		{
			short finalTechPointsGain = 0;
			stream.Serialize(ref finalTechPointsGain);
			m_finalTechPointsGain = finalTechPointsGain;
		}
		if (m_hasTechPointLoss)
		{
			short finalTechPointsLoss = 0;
			stream.Serialize(ref finalTechPointsLoss);
			m_finalTechPointsLoss = finalTechPointsLoss;
		}
		if (m_hasTechPointGainOnCaster)
		{
			short finalTechPointGainOnCaster = 0;
			stream.Serialize(ref finalTechPointGainOnCaster);
			m_finalTechPointGainOnCaster = finalTechPointGainOnCaster;
		}
		if (m_hasKnockback)
		{
			short actorIndex = (short)ActorData.s_invalidActorIndex;
			stream.Serialize(ref actorIndex);
			if (actorIndex != ActorData.s_invalidActorIndex)
			{
				m_knockbackSourceActor = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			}
			else
			{
				m_knockbackSourceActor = null;
			}
		}
		if (m_hasDamage && m_targetInCoverWrtDamage || m_hasKnockback)
		{
			float damageHitOriginX = 0f;
			float damageHitOriginZ = 0f;
			stream.Serialize(ref damageHitOriginX);
			stream.Serialize(ref damageHitOriginZ);
			m_damageHitOrigin.x = damageHitOriginX;
			m_damageHitOrigin.y = 0f;
			m_damageHitOrigin.z = damageHitOriginZ;
		}
		if (m_updateEffectHolderLastKnownPos)
		{
			short effectHolderActor = (short)ActorData.s_invalidActorIndex;
			stream.Serialize(ref effectHolderActor);
			if (effectHolderActor != ActorData.s_invalidActorIndex)
			{
				m_effectHolderActor = GameFlowData.Get().FindActorByActorIndex(effectHolderActor);
			}
			else
			{
				m_effectHolderActor = null;
			}
		}
		if (m_updateOtherLastKnownPos)
		{
			byte otherActorsToUpdateVisibilityNum = 0;
			stream.Serialize(ref otherActorsToUpdateVisibilityNum);
			m_otherActorsToUpdateVisibility = new List<ActorData>(otherActorsToUpdateVisibilityNum);
			for (int i = 0; i < otherActorsToUpdateVisibilityNum; i++)
			{
				short actorIndex = (short)ActorData.s_invalidActorIndex;
				stream.Serialize(ref actorIndex);
				if (actorIndex != ActorData.s_invalidActorIndex)
				{
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
					if (actorData != null)
					{
						m_otherActorsToUpdateVisibility.Add(actorData);
					}
				}
			}
		}

		bool hasEffectsToStart = false;
		bool hasEffectsToRemove = false;
		bool hasBarriersToAdd = false;
		bool hasBarriersToRemove = false;
		bool hasSequencesToEnd = false;
		bool hasReactions = false;
		bool hasPowerupsToRemove = false;
		bool hasPowerupsToSteal = false;
		bool hasDirectPowerupHits = false;
		bool hasGameModeEvents = false;
		bool isCharacterSpecificAbility = false;
		bool hasOverconIds = false;
		byte bitField3 = 0;
		byte bitField4 = 0;
		stream.Serialize(ref bitField3);
		stream.Serialize(ref bitField4);
		ServerClientUtils.GetBoolsFromBitfield(
			bitField3,
			out hasEffectsToStart,
			out hasEffectsToRemove,
			out hasBarriersToRemove,
			out hasSequencesToEnd,
			out hasReactions,
			out hasPowerupsToRemove,
			out hasPowerupsToSteal,
			out hasDirectPowerupHits);
		ServerClientUtils.GetBoolsFromBitfield(
			bitField4,
			out hasGameModeEvents,
			out isCharacterSpecificAbility,
			out hasBarriersToAdd,
			out hasOverconIds);

		m_effectsToStart = hasEffectsToStart
			? AbilityResultsUtils.DeSerializeEffectsToStartFromStream(ref stream)
			: new List<ClientEffectStartData>();
		m_effectsToRemove = hasEffectsToRemove
			? AbilityResultsUtils.DeSerializeEffectsForRemovalFromStream(ref stream)
			: new List<int>();
		m_barriersToAdd = hasBarriersToAdd
			? AbilityResultsUtils.DeSerializeBarriersToStartFromStream(ref stream)
			: new List<ClientBarrierStartData>();
		m_barriersToRemove = hasBarriersToRemove
			? AbilityResultsUtils.DeSerializeBarriersForRemovalFromStream(ref stream)
			: new List<int>();
		m_sequencesToEnd = hasSequencesToEnd
			? AbilityResultsUtils.DeSerializeSequenceEndDataListFromStream(ref stream)
			: new List<ServerClientUtils.SequenceEndData>();
		m_reactions = hasReactions
			? AbilityResultsUtils.DeSerializeClientReactionResultsFromStream(ref stream)
			: new List<ClientReactionResults>();
		m_powerupsToRemove = hasPowerupsToRemove
			? AbilityResultsUtils.DeSerializePowerupsToRemoveFromStream(ref stream)
			: new List<int>();
		m_powerupsToSteal = hasPowerupsToSteal
			? AbilityResultsUtils.DeSerializePowerupsToStealFromStream(ref stream)
			: new List<ClientPowerupStealData>();
		m_directPowerupHits = hasDirectPowerupHits
			? AbilityResultsUtils.DeSerializeClientMovementResultsListFromStream(ref stream)
			: new List<ClientMovementResults>();
		m_gameModeEvents = hasGameModeEvents
			? AbilityResultsUtils.DeSerializeClientGameModeEventListFromStream(ref stream)
			: new List<ClientGameModeEvent>();
		m_overconIds = hasOverconIds
			? AbilityResultsUtils.DeSerializeClientOverconListFromStream(ref stream)
			: new List<int>();
		m_isCharacterSpecificAbility = isCharacterSpecificAbility;

		IsMovementHit = false;
		ExecutedHit = false;
	}

	public bool HasUnexecutedReactionOnActor(ActorData actor)
	{
		bool flag = false;
		for (int i = 0; i < m_reactions.Count; i++)
		{
			if (flag)
			{
				break;
			}
			flag = m_reactions[i].HasUnexecutedReactionOnActor(actor);
		}
		return flag;
	}

	public bool HasUnexecutedReactionHits()
	{
		bool flag = false;
		for (int i = 0; i < m_reactions.Count; i++)
		{
			if (!flag)
			{
				flag = !m_reactions[i].ReactionHitsDone();
				continue;
			}
			break;
		}
		return flag;
	}

	public bool HasReactionHitByCaster(ActorData caster)
	{
		for (int i = 0; i < m_reactions.Count; i++)
		{
			if (!(m_reactions[i].GetCaster() == caster))
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> actorHits, out Dictionary<Vector3, ClientPositionHitResults> posHits)
	{
		actorHits = null;
		posHits = null;
		for (int i = 0; i < m_reactions.Count; i++)
		{
			if (!(m_reactions[i].GetCaster() == caster))
			{
				continue;
			}
			while (true)
			{
				actorHits = m_reactions[i].GetActorHitResults();
				posHits = m_reactions[i].GetPosHitResults();
				return;
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		for (int i = 0; i < m_reactions.Count; i++)
		{
			ClientReactionResults clientReactionResults = m_reactions[i];
			byte extraFlags = clientReactionResults.GetExtraFlags();
			if (clientReactionResults.PlayedReaction())
			{
				continue;
			}
			int num;
			if ((extraFlags & 1) != 0)
			{
				if (hasDamage)
				{
					num = (clientReactionResults.HasUnexecutedReactionOnActor(targetActor) ? 1 : 0);
					goto IL_0063;
				}
			}
			num = 0;
			goto IL_0063;
			IL_0063:
			int num2;
			if (num == 0)
			{
				if ((extraFlags & 2) != 0)
				{
					if (hasDamage)
					{
						num2 = (clientReactionResults.HasUnexecutedReactionOnActor(caster) ? 1 : 0);
						goto IL_009c;
					}
				}
				num2 = 0;
			}
			else
			{
				num2 = 1;
			}
			goto IL_009c;
			IL_009c:
			int num3;
			if (num2 == 0)
			{
				if ((extraFlags & 4) != 0)
				{
					if (hasDamage)
					{
						num3 = ((clientReactionResults.GetCaster() == targetActor) ? 1 : 0);
						goto IL_00d0;
					}
				}
				num3 = 0;
			}
			else
			{
				num3 = 1;
			}
			goto IL_00d0;
			IL_00d0:
			if (num3 != 0)
			{
				if (ClientAbilityResults.DebugTraceOn)
				{
					Log.Warning(new StringBuilder().Append(ClientAbilityResults.s_clientHitResultHeader).Append(clientReactionResults.GetDebugDescription()).Append(" executing reaction hit on first damaging hit").ToString());
				}
				clientReactionResults.PlayReaction();
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

	public void ExecuteActorHit(ActorData target, ActorData caster)
	{
		if (ExecutedHit)
		{
			return;
		}
		if (ClientAbilityResults.DebugTraceOn)
		{
			Debug.LogWarning(new StringBuilder().Append(ClientAbilityResults.s_executeActorHitHeader).Append(" Target: ").Append(target.DebugNameString()).Append(" Caster: ").Append(caster.DebugNameString()).ToString());
		}
		bool flag = ClientResolutionManager.Get().IsInResolutionState();
		if (m_triggerCasterVisOnHitVisualOnly)
		{
			if (!m_updateCasterLastKnownPos)
			{
				caster.TriggerVisibilityForHit(IsMovementHit, false);
			}
		}
		if (m_updateCasterLastKnownPos)
		{
			caster.TriggerVisibilityForHit(IsMovementHit);
		}
		if (m_updateTargetLastKnownPos)
		{
			target.TriggerVisibilityForHit(IsMovementHit);
		}
		if (m_updateEffectHolderLastKnownPos)
		{
			if (m_effectHolderActor != null)
			{
				m_effectHolderActor.TriggerVisibilityForHit(IsMovementHit);
			}
		}
		if (m_updateOtherLastKnownPos)
		{
			if (m_otherActorsToUpdateVisibility != null)
			{
				for (int i = 0; i < m_otherActorsToUpdateVisibility.Count; i++)
				{
					m_otherActorsToUpdateVisibility[i].TriggerVisibilityForHit(IsMovementHit);
				}
			}
		}
		if (m_hasDamage)
		{
			if (flag)
			{
				target.ClientUnresolvedDamage += m_finalDamage;
				CaptureTheFlag.OnActorDamaged_Client(target, m_finalDamage);
			}
			object obj;
			if (m_targetInCoverWrtDamage)
			{
				obj = "|C";
			}
			else
			{
				obj = "|N";
			}
			string str = (string)obj;
			BuffIconToDisplay icon = BuffIconToDisplay.None;
			if (m_damageBoosted)
			{
				icon = BuffIconToDisplay.BoostedDamage;
			}
			else if (m_damageReduced)
			{
				icon = BuffIconToDisplay.ReducedDamage;
			}
			target.AddCombatText(new StringBuilder().Append(m_finalDamage).Append(str).ToString(), string.Empty, CombatTextCategory.Damage, icon);
			if (m_targetInCoverWrtDamage)
			{
				target.OnHitWhileInCover(m_damageHitOrigin, caster);
			}
			if (target.GetActorBehavior() != null)
			{
				target.GetActorBehavior().Client_RecordDamageFromActor(caster);
			}
			GameEventManager.ActorHitHealthChangeArgs args = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Damage, m_finalDamage, target, caster, m_isCharacterSpecificAbility);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorDamaged_Client, args);
		}
		if (m_hasHealing)
		{
			if (flag)
			{
				target.ClientUnresolvedHealing += m_finalHealing;
				if (m_isPartOfHealOverTime)
				{
					target.ClientAppliedHoTThisTurn += m_finalHealing;
				}
			}
			target.AddCombatText(m_finalHealing.ToString(), string.Empty, CombatTextCategory.Healing, BuffIconToDisplay.None);
			if (target.GetActorBehavior() != null)
			{
				target.GetActorBehavior().Client_RecordHealingFromActor(caster);
			}
			GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = new GameEventManager.CharacterHealBuffArgs();
			characterHealBuffArgs.targetCharacter = target;
			characterHealBuffArgs.casterActor = caster;
			characterHealBuffArgs.healed = true;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterHealedOrBuffed, characterHealBuffArgs);
			GameEventManager.ActorHitHealthChangeArgs args2 = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing, m_finalHealing, target, caster, m_isCharacterSpecificAbility);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorHealed_Client, args2);
		}
		if (m_hasTechPointGain)
		{
			if (flag)
			{
				target.ClientUnresolvedTechPointGain += m_finalTechPointsGain;
			}
			target.AddCombatText(m_finalTechPointsGain.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
		}
		if (m_hasTechPointLoss)
		{
			if (flag)
			{
				target.ClientUnresolvedTechPointLoss += m_finalTechPointsLoss;
			}
			target.AddCombatText(m_finalTechPointsLoss.ToString(), string.Empty, CombatTextCategory.TP_Damage, BuffIconToDisplay.None);
		}
		if (m_hasTechPointGainOnCaster)
		{
			if (flag)
			{
				caster.ClientUnresolvedTechPointGain += m_finalTechPointGainOnCaster;
			}
			caster.AddCombatText(m_finalTechPointGainOnCaster.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
		}
		if (m_hasKnockback)
		{
			ClientKnockbackManager.Get().OnKnockbackHit(m_knockbackSourceActor, target);
			if (caster != target)
			{
				if (target.GetActorStatus() != null && target.GetActorStatus().IsKnockbackImmune())
				{
					target.OnKnockbackWhileUnstoppable(m_damageHitOrigin, caster);
				}
			}
		}
		int num = 0;
		using (List<ClientEffectStartData>.Enumerator enumerator = m_effectsToStart.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientEffectStartData current = enumerator.Current;
				num += current.m_absorb;
				ClientEffectBarrierManager.Get().ExecuteEffectStart(current);
			}
		}
		if (num > 0)
		{
			target.AddCombatText(num.ToString(), string.Empty, CombatTextCategory.Absorb, BuffIconToDisplay.None);
			GameEventManager.ActorHitHealthChangeArgs args3 = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb, num, target, caster, m_isCharacterSpecificAbility);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorGainedAbsorb_Client, args3);
		}
		foreach (int item in m_effectsToRemove)
		{
			ClientEffectBarrierManager.Get().EndEffect(item);
		}
		foreach (ClientBarrierStartData item2 in m_barriersToAdd)
		{
			ClientEffectBarrierManager.Get().ExecuteBarrierStart(item2);
		}
		using (List<int>.Enumerator enumerator4 = m_barriersToRemove.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				int current4 = enumerator4.Current;
				ClientEffectBarrierManager.Get().EndBarrier(current4);
			}
		}
		using (List<ServerClientUtils.SequenceEndData>.Enumerator enumerator5 = m_sequencesToEnd.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				ServerClientUtils.SequenceEndData current5 = enumerator5.Current;
				current5.EndClientSequences();
			}
		}
		using (List<ClientReactionResults>.Enumerator enumerator6 = m_reactions.GetEnumerator())
		{
			while (enumerator6.MoveNext())
			{
				ClientReactionResults current6 = enumerator6.Current;
				current6.PlayReaction();
			}
		}
		using (List<int>.Enumerator enumerator7 = m_powerupsToRemove.GetEnumerator())
		{
			while (enumerator7.MoveNext())
			{
				int current7 = enumerator7.Current;
				PowerUp powerUpOfGuid = PowerUpManager.Get().GetPowerUpOfGuid(current7);
				if (powerUpOfGuid != null)
				{
					powerUpOfGuid.Client_OnPickedUp(target.ActorIndex);
				}
			}
		}
		using (List<ClientPowerupStealData>.Enumerator enumerator8 = m_powerupsToSteal.GetEnumerator())
		{
			while (enumerator8.MoveNext())
			{
				ClientPowerupStealData current8 = enumerator8.Current;
				current8.m_powerupResults.RunResults();
				PowerUp powerUpOfGuid2 = PowerUpManager.Get().GetPowerUpOfGuid(current8.m_powerupGuid);
				if (powerUpOfGuid2 != null)
				{
					powerUpOfGuid2.Client_OnSteal(target.ActorIndex);
				}
			}
		}
		using (List<ClientMovementResults>.Enumerator enumerator9 = m_directPowerupHits.GetEnumerator())
		{
			while (enumerator9.MoveNext())
			{
				ClientMovementResults current9 = enumerator9.Current;
				current9.ReactToMovement();
			}
		}
		using (List<ClientGameModeEvent>.Enumerator enumerator10 = m_gameModeEvents.GetEnumerator())
		{
			while (enumerator10.MoveNext())
			{
				ClientGameModeEvent current10 = enumerator10.Current;
				current10.ExecuteClientGameModeEvent();
			}
		}
		using (List<int>.Enumerator enumerator11 = m_overconIds.GetEnumerator())
		{
			while (enumerator11.MoveNext())
			{
				int current11 = enumerator11.Current;
				if (UIOverconData.Get() != null)
				{
					UIOverconData.Get().UseOvercon(current11, caster.ActorIndex, true);
				}
			}
		}
		ExecutedHit = true;
		ClientResolutionManager.Get().UpdateLastEventTime();
		ClientResolutionManager.Get().OnHitExecutedOnActor(target, caster, m_hasDamage, m_hasHealing, m_canBeReactedTo);
	}

	public void ShowDamage(ActorData target)
	{
		string empty = string.Empty;
		target.ShowDamage(empty);
	}

	public int GetNumEffectsToStart()
	{
		return (m_effectsToStart != null) ? m_effectsToStart.Count : 0;
	}

	public void SwapEffectsToStart(ClientActorHitResults other)
	{
		List<ClientEffectStartData> effectsToStart = m_effectsToStart;
		m_effectsToStart = other.m_effectsToStart;
		other.m_effectsToStart = effectsToStart;
	}
}
