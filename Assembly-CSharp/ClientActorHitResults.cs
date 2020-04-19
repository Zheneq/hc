using System;
using System.Collections.Generic;
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

	public unsafe ClientActorHitResults(ref IBitStream stream)
	{
		byte bitField = 0;
		stream.Serialize(ref bitField);
		ServerClientUtils.GetBoolsFromBitfield(bitField, out this.m_hasDamage, out this.m_hasHealing, out this.m_hasTechPointGain, out this.m_hasTechPointLoss, out this.m_hasTechPointGainOnCaster, out this.m_hasKnockback, out this.m_targetInCoverWrtDamage, out this.m_canBeReactedTo);
		byte bitField2 = 0;
		stream.Serialize(ref bitField2);
		ServerClientUtils.GetBoolsFromBitfield(bitField2, out this.m_damageBoosted, out this.m_damageReduced, out this.m_updateCasterLastKnownPos, out this.m_updateTargetLastKnownPos, out this.m_updateEffectHolderLastKnownPos, out this.m_updateOtherLastKnownPos, out this.m_isPartOfHealOverTime, out this.m_triggerCasterVisOnHitVisualOnly);
		short finalDamage = 0;
		short finalHealing = 0;
		short finalTechPointsGain = 0;
		short finalTechPointsLoss = 0;
		short finalTechPointGainOnCaster = 0;
		if (this.m_hasDamage)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActorHitResults..ctor(IBitStream*)).MethodHandle;
			}
			stream.Serialize(ref finalDamage);
			this.m_finalDamage = (int)finalDamage;
		}
		if (this.m_hasHealing)
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
			stream.Serialize(ref finalHealing);
			this.m_finalHealing = (int)finalHealing;
		}
		if (this.m_hasTechPointGain)
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
			stream.Serialize(ref finalTechPointsGain);
			this.m_finalTechPointsGain = (int)finalTechPointsGain;
		}
		if (this.m_hasTechPointLoss)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			stream.Serialize(ref finalTechPointsLoss);
			this.m_finalTechPointsLoss = (int)finalTechPointsLoss;
		}
		if (this.m_hasTechPointGainOnCaster)
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
			stream.Serialize(ref finalTechPointGainOnCaster);
			this.m_finalTechPointGainOnCaster = (int)finalTechPointGainOnCaster;
		}
		if (this.m_hasKnockback)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			short num = (short)ActorData.s_invalidActorIndex;
			stream.Serialize(ref num);
			if ((int)num != ActorData.s_invalidActorIndex)
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
				this.m_knockbackSourceActor = GameFlowData.Get().FindActorByActorIndex((int)num);
			}
			else
			{
				this.m_knockbackSourceActor = null;
			}
		}
		bool flag;
		if (this.m_hasDamage)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_targetInCoverWrtDamage)
			{
				flag = true;
				goto IL_1CB;
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
		flag = this.m_hasKnockback;
		IL_1CB:
		bool flag2 = flag;
		if (flag2)
		{
			float x = 0f;
			float z = 0f;
			stream.Serialize(ref x);
			stream.Serialize(ref z);
			this.m_damageHitOrigin.x = x;
			this.m_damageHitOrigin.y = 0f;
			this.m_damageHitOrigin.z = z;
		}
		if (this.m_updateEffectHolderLastKnownPos)
		{
			short num2 = (short)ActorData.s_invalidActorIndex;
			stream.Serialize(ref num2);
			if ((int)num2 != ActorData.s_invalidActorIndex)
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
				this.m_effectHolderActor = GameFlowData.Get().FindActorByActorIndex((int)num2);
			}
			else
			{
				this.m_effectHolderActor = null;
			}
		}
		if (this.m_updateOtherLastKnownPos)
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
			byte b = 0;
			stream.Serialize(ref b);
			this.m_otherActorsToUpdateVisibility = new List<ActorData>((int)b);
			for (int i = 0; i < (int)b; i++)
			{
				short num3 = (short)ActorData.s_invalidActorIndex;
				stream.Serialize(ref num3);
				if ((int)num3 != ActorData.s_invalidActorIndex)
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
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex((int)num3);
					if (actorData != null)
					{
						this.m_otherActorsToUpdateVisibility.Add(actorData);
					}
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		bool flag12 = false;
		bool isCharacterSpecificAbility = false;
		bool flag13 = false;
		byte bitField3 = 0;
		byte bitField4 = 0;
		stream.Serialize(ref bitField3);
		stream.Serialize(ref bitField4);
		ServerClientUtils.GetBoolsFromBitfield(bitField3, out flag3, out flag4, out flag6, out flag7, out flag8, out flag9, out flag10, out flag11);
		ServerClientUtils.GetBoolsFromBitfield(bitField4, out flag12, out isCharacterSpecificAbility, out flag5, out flag13);
		List<ClientEffectStartData> effectsToStart;
		if (flag3)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			effectsToStart = AbilityResultsUtils.DeSerializeEffectsToStartFromStream(ref stream);
		}
		else
		{
			effectsToStart = new List<ClientEffectStartData>();
		}
		this.m_effectsToStart = effectsToStart;
		this.m_effectsToRemove = ((!flag4) ? new List<int>() : AbilityResultsUtils.DeSerializeEffectsForRemovalFromStream(ref stream));
		List<ClientBarrierStartData> barriersToAdd;
		if (flag5)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			barriersToAdd = AbilityResultsUtils.DeSerializeBarriersToStartFromStream(ref stream);
		}
		else
		{
			barriersToAdd = new List<ClientBarrierStartData>();
		}
		this.m_barriersToAdd = barriersToAdd;
		List<int> barriersToRemove;
		if (flag6)
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
			barriersToRemove = AbilityResultsUtils.DeSerializeBarriersForRemovalFromStream(ref stream);
		}
		else
		{
			barriersToRemove = new List<int>();
		}
		this.m_barriersToRemove = barriersToRemove;
		List<ServerClientUtils.SequenceEndData> sequencesToEnd;
		if (flag7)
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
			sequencesToEnd = AbilityResultsUtils.DeSerializeSequenceEndDataListFromStream(ref stream);
		}
		else
		{
			sequencesToEnd = new List<ServerClientUtils.SequenceEndData>();
		}
		this.m_sequencesToEnd = sequencesToEnd;
		List<ClientReactionResults> reactions;
		if (flag8)
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
			reactions = AbilityResultsUtils.DeSerializeClientReactionResultsFromStream(ref stream);
		}
		else
		{
			reactions = new List<ClientReactionResults>();
		}
		this.m_reactions = reactions;
		this.m_powerupsToRemove = ((!flag9) ? new List<int>() : AbilityResultsUtils.DeSerializePowerupsToRemoveFromStream(ref stream));
		List<ClientPowerupStealData> powerupsToSteal;
		if (flag10)
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
			powerupsToSteal = AbilityResultsUtils.DeSerializePowerupsToStealFromStream(ref stream);
		}
		else
		{
			powerupsToSteal = new List<ClientPowerupStealData>();
		}
		this.m_powerupsToSteal = powerupsToSteal;
		List<ClientMovementResults> directPowerupHits;
		if (flag11)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			directPowerupHits = AbilityResultsUtils.DeSerializeClientMovementResultsListFromStream(ref stream);
		}
		else
		{
			directPowerupHits = new List<ClientMovementResults>();
		}
		this.m_directPowerupHits = directPowerupHits;
		List<ClientGameModeEvent> gameModeEvents;
		if (flag12)
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
			gameModeEvents = AbilityResultsUtils.DeSerializeClientGameModeEventListFromStream(ref stream);
		}
		else
		{
			gameModeEvents = new List<ClientGameModeEvent>();
		}
		this.m_gameModeEvents = gameModeEvents;
		this.m_overconIds = ((!flag13) ? new List<int>() : AbilityResultsUtils.DeSerializeClientOverconListFromStream(ref stream));
		this.m_isCharacterSpecificAbility = isCharacterSpecificAbility;
		this.IsMovementHit = false;
		this.ExecutedHit = false;
	}

	public bool ExecutedHit { get; private set; }

	public bool IsMovementHit { get; set; }

	public bool HasKnockback
	{
		get
		{
			return this.m_hasKnockback;
		}
		private set
		{
		}
	}

	public ActorData KnockbackSourceActor
	{
		get
		{
			return this.m_knockbackSourceActor;
		}
		private set
		{
		}
	}

	public bool HasUnexecutedReactionOnActor(ActorData actor)
	{
		bool flag = false;
		int num = 0;
		while (num < this.m_reactions.Count && !flag)
		{
			flag = this.m_reactions[num].HasUnexecutedReactionOnActor(actor);
			num++;
		}
		return flag;
	}

	public bool HasUnexecutedReactionHits()
	{
		bool flag = false;
		int i = 0;
		while (i < this.m_reactions.Count)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActorHitResults.HasUnexecutedReactionHits()).MethodHandle;
			}
			if (flag)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					return flag;
				}
			}
			else
			{
				flag = !this.m_reactions[i].ReactionHitsDone();
				i++;
			}
		}
		return flag;
	}

	public bool HasReactionHitByCaster(ActorData caster)
	{
		for (int i = 0; i < this.m_reactions.Count; i++)
		{
			if (this.m_reactions[i].GetCaster() == caster)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActorHitResults.HasReactionHitByCaster(ActorData)).MethodHandle;
				}
				return true;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		return false;
	}

	public unsafe void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> actorHits, out Dictionary<Vector3, ClientPositionHitResults> posHits)
	{
		actorHits = null;
		posHits = null;
		for (int i = 0; i < this.m_reactions.Count; i++)
		{
			if (this.m_reactions[i].GetCaster() == caster)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActorHitResults.GetReactionHitResultsByCaster(ActorData, Dictionary<ActorData, ClientActorHitResults>*, Dictionary<Vector3, ClientPositionHitResults>*)).MethodHandle;
				}
				actorHits = this.m_reactions[i].GetActorHitResults();
				posHits = this.m_reactions[i].GetPosHitResults();
				return;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		for (int i = 0; i < this.m_reactions.Count; i++)
		{
			ClientReactionResults clientReactionResults = this.m_reactions[i];
			byte extraFlags = clientReactionResults.GetExtraFlags();
			if (!clientReactionResults.PlayedReaction())
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActorHitResults.ExecuteReactionHitsWithExtraFlagsOnActor(ActorData, ActorData, bool, bool)).MethodHandle;
				}
				if ((extraFlags & 1) == 0)
				{
					goto IL_62;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!hasDamage)
				{
					goto IL_62;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = clientReactionResults.HasUnexecutedReactionOnActor(targetActor);
				IL_63:
				bool flag2;
				if (!flag)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if ((extraFlags & 2) == 0)
					{
						goto IL_98;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!hasDamage)
					{
						goto IL_98;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = clientReactionResults.HasUnexecutedReactionOnActor(caster);
					goto IL_9C;
					IL_98:
					flag2 = false;
				}
				else
				{
					flag2 = true;
				}
				IL_9C:
				bool flag3;
				if (!flag2)
				{
					if ((extraFlags & 4) == 0)
					{
						goto IL_CC;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!hasDamage)
					{
						goto IL_CC;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					flag3 = (clientReactionResults.GetCaster() == targetActor);
					goto IL_D0;
					IL_CC:
					flag3 = false;
				}
				else
				{
					flag3 = true;
				}
				IL_D0:
				bool flag4 = flag3;
				if (flag4)
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
					if (ClientAbilityResults.\u001D)
					{
						Log.Warning(ClientAbilityResults.s_clientHitResultHeader + clientReactionResults.GetDebugDescription() + " executing reaction hit on first damaging hit", new object[0]);
					}
					clientReactionResults.PlayReaction();
					goto IL_111;
				}
				goto IL_111;
				IL_62:
				flag = false;
				goto IL_63;
			}
			IL_111:;
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void ExecuteActorHit(ActorData target, ActorData caster)
	{
		if (this.ExecutedHit)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActorHitResults.ExecuteActorHit(ActorData, ActorData)).MethodHandle;
			}
			return;
		}
		if (ClientAbilityResults.\u001D)
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
			Debug.LogWarning(string.Concat(new string[]
			{
				ClientAbilityResults.s_executeActorHitHeader,
				" Target: ",
				target.\u0018(),
				" Caster: ",
				caster.\u0018()
			}));
		}
		bool flag = ClientResolutionManager.Get().IsInResolutionState();
		if (this.m_triggerCasterVisOnHitVisualOnly)
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
			if (!this.m_updateCasterLastKnownPos)
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
				caster.TriggerVisibilityForHit(this.IsMovementHit, false);
			}
		}
		if (this.m_updateCasterLastKnownPos)
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
			caster.TriggerVisibilityForHit(this.IsMovementHit, true);
		}
		if (this.m_updateTargetLastKnownPos)
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
			target.TriggerVisibilityForHit(this.IsMovementHit, true);
		}
		if (this.m_updateEffectHolderLastKnownPos)
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
			if (this.m_effectHolderActor != null)
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
				this.m_effectHolderActor.TriggerVisibilityForHit(this.IsMovementHit, true);
			}
		}
		if (this.m_updateOtherLastKnownPos)
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
			if (this.m_otherActorsToUpdateVisibility != null)
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
				for (int i = 0; i < this.m_otherActorsToUpdateVisibility.Count; i++)
				{
					this.m_otherActorsToUpdateVisibility[i].TriggerVisibilityForHit(this.IsMovementHit, true);
				}
			}
		}
		if (this.m_hasDamage)
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
			if (flag)
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
				target.ClientUnresolvedDamage += this.m_finalDamage;
				CaptureTheFlag.OnActorDamaged_Client(target, this.m_finalDamage);
			}
			string text;
			if (this.m_targetInCoverWrtDamage)
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
				text = "|C";
			}
			else
			{
				text = "|N";
			}
			string str = text;
			BuffIconToDisplay icon = BuffIconToDisplay.None;
			if (this.m_damageBoosted)
			{
				icon = BuffIconToDisplay.BoostedDamage;
			}
			else if (this.m_damageReduced)
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
				icon = BuffIconToDisplay.ReducedDamage;
			}
			target.AddCombatText(this.m_finalDamage.ToString() + str, string.Empty, CombatTextCategory.Damage, icon);
			if (this.m_targetInCoverWrtDamage)
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
				target.OnHitWhileInCover(this.m_damageHitOrigin, caster);
			}
			if (target.\u000E() != null)
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
				target.\u000E().Client_RecordDamageFromActor(caster);
			}
			GameEventManager.ActorHitHealthChangeArgs args = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Damage, this.m_finalDamage, target, caster, this.m_isCharacterSpecificAbility);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorDamaged_Client, args);
		}
		if (this.m_hasHealing)
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
			if (flag)
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
				target.ClientUnresolvedHealing += this.m_finalHealing;
				if (this.m_isPartOfHealOverTime)
				{
					target.ClientAppliedHoTThisTurn += this.m_finalHealing;
				}
			}
			target.AddCombatText(this.m_finalHealing.ToString(), string.Empty, CombatTextCategory.Healing, BuffIconToDisplay.None);
			if (target.\u000E() != null)
			{
				target.\u000E().Client_RecordHealingFromActor(caster);
			}
			GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = new GameEventManager.CharacterHealBuffArgs();
			characterHealBuffArgs.targetCharacter = target;
			characterHealBuffArgs.casterActor = caster;
			characterHealBuffArgs.healed = true;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterHealedOrBuffed, characterHealBuffArgs);
			GameEventManager.ActorHitHealthChangeArgs args2 = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing, this.m_finalHealing, target, caster, this.m_isCharacterSpecificAbility);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorHealed_Client, args2);
		}
		if (this.m_hasTechPointGain)
		{
			if (flag)
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
				target.ClientUnresolvedTechPointGain += this.m_finalTechPointsGain;
			}
			target.AddCombatText(this.m_finalTechPointsGain.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
		}
		if (this.m_hasTechPointLoss)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag)
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
				target.ClientUnresolvedTechPointLoss += this.m_finalTechPointsLoss;
			}
			target.AddCombatText(this.m_finalTechPointsLoss.ToString(), string.Empty, CombatTextCategory.TP_Damage, BuffIconToDisplay.None);
		}
		if (this.m_hasTechPointGainOnCaster)
		{
			if (flag)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				caster.ClientUnresolvedTechPointGain += this.m_finalTechPointGainOnCaster;
			}
			caster.AddCombatText(this.m_finalTechPointGainOnCaster.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
		}
		if (this.m_hasKnockback)
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
			ClientKnockbackManager.Get().OnKnockbackHit(this.m_knockbackSourceActor, target);
			if (caster != target)
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
				if (target.\u000E() != null && target.\u000E().IsKnockbackImmune(true))
				{
					target.OnKnockbackWhileUnstoppable(this.m_damageHitOrigin, caster);
				}
			}
		}
		int num = 0;
		using (List<ClientEffectStartData>.Enumerator enumerator = this.m_effectsToStart.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientEffectStartData clientEffectStartData = enumerator.Current;
				num += clientEffectStartData.m_absorb;
				ClientEffectBarrierManager.Get().ExecuteEffectStart(clientEffectStartData);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (num > 0)
		{
			target.AddCombatText(num.ToString(), string.Empty, CombatTextCategory.Absorb, BuffIconToDisplay.None);
			GameEventManager.ActorHitHealthChangeArgs args3 = new GameEventManager.ActorHitHealthChangeArgs(GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb, num, target, caster, this.m_isCharacterSpecificAbility);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorGainedAbsorb_Client, args3);
		}
		foreach (int effectGuid in this.m_effectsToRemove)
		{
			ClientEffectBarrierManager.Get().EndEffect(effectGuid);
		}
		foreach (ClientBarrierStartData barrierData in this.m_barriersToAdd)
		{
			ClientEffectBarrierManager.Get().ExecuteBarrierStart(barrierData);
		}
		using (List<int>.Enumerator enumerator4 = this.m_barriersToRemove.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				int barrierGuid = enumerator4.Current;
				ClientEffectBarrierManager.Get().EndBarrier(barrierGuid);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		using (List<ServerClientUtils.SequenceEndData>.Enumerator enumerator5 = this.m_sequencesToEnd.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				ServerClientUtils.SequenceEndData sequenceEndData = enumerator5.Current;
				sequenceEndData.EndClientSequences();
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		using (List<ClientReactionResults>.Enumerator enumerator6 = this.m_reactions.GetEnumerator())
		{
			while (enumerator6.MoveNext())
			{
				ClientReactionResults clientReactionResults = enumerator6.Current;
				clientReactionResults.PlayReaction();
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		using (List<int>.Enumerator enumerator7 = this.m_powerupsToRemove.GetEnumerator())
		{
			while (enumerator7.MoveNext())
			{
				int guid = enumerator7.Current;
				PowerUp powerUpOfGuid = PowerUpManager.Get().GetPowerUpOfGuid(guid);
				if (powerUpOfGuid != null)
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
					powerUpOfGuid.Client_OnPickedUp(target.ActorIndex);
				}
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
		using (List<ClientPowerupStealData>.Enumerator enumerator8 = this.m_powerupsToSteal.GetEnumerator())
		{
			while (enumerator8.MoveNext())
			{
				ClientPowerupStealData clientPowerupStealData = enumerator8.Current;
				clientPowerupStealData.m_powerupResults.RunResults();
				PowerUp powerUpOfGuid2 = PowerUpManager.Get().GetPowerUpOfGuid(clientPowerupStealData.m_powerupGuid);
				if (powerUpOfGuid2 != null)
				{
					powerUpOfGuid2.Client_OnSteal(target.ActorIndex);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		using (List<ClientMovementResults>.Enumerator enumerator9 = this.m_directPowerupHits.GetEnumerator())
		{
			while (enumerator9.MoveNext())
			{
				ClientMovementResults clientMovementResults = enumerator9.Current;
				clientMovementResults.ReactToMovement();
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		using (List<ClientGameModeEvent>.Enumerator enumerator10 = this.m_gameModeEvents.GetEnumerator())
		{
			while (enumerator10.MoveNext())
			{
				ClientGameModeEvent clientGameModeEvent = enumerator10.Current;
				clientGameModeEvent.ExecuteClientGameModeEvent();
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
		using (List<int>.Enumerator enumerator11 = this.m_overconIds.GetEnumerator())
		{
			while (enumerator11.MoveNext())
			{
				int overconId = enumerator11.Current;
				if (UIOverconData.Get() != null)
				{
					UIOverconData.Get().UseOvercon(overconId, caster.ActorIndex, true);
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.ExecutedHit = true;
		ClientResolutionManager.Get().UpdateLastEventTime();
		ClientResolutionManager.Get().OnHitExecutedOnActor(target, caster, this.m_hasDamage, this.m_hasHealing, this.m_canBeReactedTo);
	}

	public void ShowDamage(ActorData target)
	{
		string empty = string.Empty;
		target.ShowDamage(empty);
	}

	public int GetNumEffectsToStart()
	{
		return (this.m_effectsToStart == null) ? 0 : this.m_effectsToStart.Count;
	}

	public void SwapEffectsToStart(ClientActorHitResults other)
	{
		List<ClientEffectStartData> effectsToStart = this.m_effectsToStart;
		this.m_effectsToStart = other.m_effectsToStart;
		other.m_effectsToStart = effectsToStart;
	}
}
