﻿// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// TODO SENSEI response effect instantly wears off
public class SenseiAppendStatus : Ability
{
	public enum TargetingMode
	{
		ActorSquare,
		Laser
	}

	[Separator("Targeting")]
	public TargetingMode m_targetingMode;
	[Header("    (( Targeting: If using ActorSquare mode ))")]
	public bool m_canTargetAlly = true;
	public bool m_canTargetEnemy = true;
	public bool m_canTagetSelf;
	public bool m_targetingIgnoreLos;
	[Header("-- Whether to check barriers for enemy targeting")]
	public bool m_checkBarrierForLosIfTargetEnemy = true;
	[Header("    (( Targeting: If using Laser mode ))")]
	public LaserTargetingInfo m_laserInfo;
	[Separator("On Cast Hit Stuff")]
	public int m_energyToAllyTargetOnCast;
	public StandardActorEffectData m_enemyCastHitEffectData;
	public StandardActorEffectData m_allyCastHitEffectData;
	[Separator("For Append Effect")]
	public bool m_endEffectIfAppendedStatus = true;
	public AbilityPriority m_earliestPriorityToConsider;
	public bool m_delayEffectApply = true;
	public bool m_requireDamageToTransfer = true;
	[Header("-- Effect to append --")]
	public StandardEffectInfo m_effectAddedOnEnemyAttack;
	public StandardEffectInfo m_effectAddedOnAllyAttack;
	[Space(10f)]
	public int m_energyGainOnAllyAppendHit;
	[Header("-- Sequences --")]
	public GameObject m_castOnEnemySequencePrefab;
	public GameObject m_castOnAllySequencePrefab;
	public GameObject m_statusApplyOnAllySequencePrefab;
	public GameObject m_statusApplyOnEnemySequencePrefab;

	private AbilityMod_SenseiAppendStatus m_abilityMod;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardActorEffectData m_cachedEnemyCastHitEffectData;
	private StandardActorEffectData m_cachedAllyCastHitEffectData;
	private StandardEffectInfo m_cachedEffectAddedOnEnemyAttack;
	private StandardEffectInfo m_cachedEffectAddedOnAllyAttack;

#if SERVER
	// added in rogues
	private Passive_Sensei m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiAppendStatus";
		}
		Setup();
	}

	private void Setup()
	{
#if SERVER
		// added in rogues
		m_passive = GetPassiveOfType(typeof(Passive_Sensei)) as Passive_Sensei;
#endif
		
		SetCachedFields();
		if (m_targetingMode == TargetingMode.ActorSquare)
		{
			Targeter = new AbilityUtil_Targeter_Shape(
				this,
				AbilityAreaShape.SingleSquare,
				true,
				AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
				CanTargetAlly(),
				CanTargetEnemy(),
				CanTagetSelf()
					? AbilityUtil_Targeter.AffectsActor.Possible
					: AbilityUtil_Targeter.AffectsActor.Never);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserInfo());
		}
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\n" + SetupNoteVarName("Cast On Enemy Sequence Prefab") +
		       "\nFor initial cast, it targeting Enemy\n\n" + SetupNoteVarName("Cast On Ally Sequence Prefab") +
		       "\nFor initial casst, if targeting Ally ...\n\n" + SetupNoteVarName("Status Apply On Ally Sequence Prefab") +
		       "\nFor impact on target that actually adds buff/debuff\n\n" + SetupNoteVarName("Status Apply On Enemy Sequence Prefab") +
		       "\nFor impact on target that actually adds buff/debuff\n\n";
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedEnemyCastHitEffectData = m_abilityMod != null
			? m_abilityMod.m_enemyCastHitEffectDataMod.GetModifiedValue(m_enemyCastHitEffectData)
			: m_enemyCastHitEffectData;
		m_cachedAllyCastHitEffectData = m_abilityMod != null
			? m_abilityMod.m_allyCastHitEffectDataMod.GetModifiedValue(m_allyCastHitEffectData)
			: m_allyCastHitEffectData;
		m_cachedEffectAddedOnEnemyAttack = m_abilityMod != null
			? m_abilityMod.m_effectAddedOnEnemyAttackMod.GetModifiedValue(m_effectAddedOnEnemyAttack)
			: m_effectAddedOnEnemyAttack;
		m_cachedEffectAddedOnAllyAttack = m_abilityMod != null
			? m_abilityMod.m_effectAddedOnAllyAttackMod.GetModifiedValue(m_effectAddedOnAllyAttack)
			: m_effectAddedOnAllyAttack;
	}

	public bool CanTargetAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTargetAllyMod.GetModifiedValue(m_canTargetAlly)
			: m_canTargetAlly;
	}

	public bool CanTargetEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTargetEnemyMod.GetModifiedValue(m_canTargetEnemy)
			: m_canTargetEnemy;
	}

	public bool CanTagetSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTagetSelfMod.GetModifiedValue(m_canTagetSelf)
			: m_canTagetSelf;
	}

	public bool TargetingIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(m_targetingIgnoreLos)
			: m_targetingIgnoreLos;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardActorEffectData GetEnemyCastHitEffectData()
	{
		return m_cachedEnemyCastHitEffectData ?? m_enemyCastHitEffectData;
	}

	public StandardActorEffectData GetAllyCastHitEffectData()
	{
		return m_cachedAllyCastHitEffectData ?? m_allyCastHitEffectData;
	}

	public int GetEnergyToAllyTargetOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyToAllyTargetOnCastMod.GetModifiedValue(m_energyToAllyTargetOnCast)
			: m_energyToAllyTargetOnCast;
	}

	public bool EndEffectIfAppendedStatus()
	{
		return m_abilityMod != null
			? m_abilityMod.m_endEffectIfAppendedStatusMod.GetModifiedValue(m_endEffectIfAppendedStatus)
			: m_endEffectIfAppendedStatus;
	}

	public StandardEffectInfo GetEffectAddedOnEnemyAttack()
	{
		return m_cachedEffectAddedOnEnemyAttack ?? m_effectAddedOnEnemyAttack;
	}

	public StandardEffectInfo GetEffectAddedOnAllyAttack()
	{
		return m_cachedEffectAddedOnAllyAttack ?? m_effectAddedOnAllyAttack;
	}

	public int GetEnergyGainOnAllyAppendHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainOnAllyAppendHitMod.GetModifiedValue(m_energyGainOnAllyAppendHit)
			: m_energyGainOnAllyAppendHit;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportEnergy(ref number, AbilityTooltipSubject.Ally, GetEnergyToAllyTargetOnCast());
		return number;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (m_targetingMode != TargetingMode.ActorSquare)
		{
			return true;
		}
		return CanTargetActorInDecision(
			caster,
			target.GetCurrentBestActorTarget(),
			CanTargetEnemy(),
			CanTargetAlly(),
			CanTagetSelf(),
			ValidateCheckPath.Ignore,
			!TargetingIgnoreLos(),
			true);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_targetingMode != TargetingMode.ActorSquare)
		{
			return true;
		}
		return HasTargetableActorsInDecision(
			caster,
			CanTargetEnemy(),
			CanTargetAlly(),
			CanTagetSelf(),
			ValidateCheckPath.Ignore,
			!TargetingIgnoreLos(),
			true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_enemyCastHitEffectData.AddTooltipTokens(tokens, "EnemyCastHitEffectData");
		m_allyCastHitEffectData.AddTooltipTokens(tokens, "AllyCastHitEffectData");
		AddTokenInt(tokens, "EnergyToAllyTargetOnCast", string.Empty, m_energyToAllyTargetOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectAddedOnEnemyAttack, "EffectAddedOnEnemyAttack", m_effectAddedOnEnemyAttack);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectAddedOnAllyAttack, "EffectAddedOnAllyAttack", m_effectAddedOnAllyAttack);
		AddTokenInt(tokens, "EnergyGainOnAllyAppendHit", string.Empty, m_energyGainOnAllyAppendHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiAppendStatus))
		{
			m_abilityMod = abilityMod as AbilityMod_SenseiAppendStatus;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> hitActors = GetHitActors(targets, caster, null, out Vector3 endPos);
		if (m_targetingMode == TargetingMode.ActorSquare)
		{
			endPos = Board.Get().GetSquare(targets[0].GridPos).ToVector3();
		}
		GameObject prefab = m_castOnEnemySequencePrefab;
		if (hitActors.Count > 0 && hitActors[0].GetTeam() == caster.GetTeam())
		{
			prefab = m_castOnAllySequencePrefab;
		}
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				prefab,
				endPos,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in GetHitActors(targets, caster, nonActorTargetInfo, out _))
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
			bool isAlly = actorData.GetTeam() == caster.GetTeam();
			StandardActorEffectData data = isAlly ? GetAllyCastHitEffectData() : GetEnemyCastHitEffectData();
			SenseiAppendStatusEffect effect = new SenseiAppendStatusEffect(
				AsEffectSource(),
				actorData.GetCurrentBoardSquare(),
				actorData,
				caster,
				data,
				m_passive,
				GetEffectAddedOnAllyAttack(),
				GetEffectAddedOnEnemyAttack(),
				GetEnergyGainOnAllyAppendHit(),
				EndEffectIfAppendedStatus(),
				m_earliestPriorityToConsider,
				m_delayEffectApply,
				m_requireDamageToTransfer,
				m_statusApplyOnAllySequencePrefab,
				m_statusApplyOnEnemySequencePrefab);
			actorHitResults.AddEffect(effect);
			if (isAlly)
			{
				actorHitResults.SetTechPointGain(GetEnergyToAllyTargetOnCast());
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo, out Vector3 endPos)
	{
		if (m_targetingMode == TargetingMode.ActorSquare)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			endPos = square.ToVector3();
			ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(square, CanTargetEnemy(), CanTargetAlly(), caster);
			List<ActorData> list = new List<ActorData>();
			if (m_checkBarrierForLosIfTargetEnemy
			    && !TargetingIgnoreLos()
			    && targetableActorOnSquare != null
			    && targetableActorOnSquare.GetTeam() != caster.GetTeam())
			{
				BarrierManager.Get().GetAbilityLineEndpoint(
					caster,
					caster.GetFreePos(),
					targetableActorOnSquare.GetFreePos(),
					out bool flag,
					out Vector3 vector,
					nonActorTargetInfo);
				if (!flag)
				{
					list.Add(targetableActorOnSquare);
				}
				else
				{
					vector.y = endPos.y;
					endPos = vector;
				}
			}
			else
			{
				list.Add(targetableActorOnSquare);
			}
			return list;
		}
		else
		{
			LaserTargetingInfo laserInfo = GetLaserInfo();
			return AreaEffectUtils.GetActorsInLaser(
				caster.GetLoSCheckPos(),
				targets[0].AimDirection,
				laserInfo.range,
				laserInfo.width,
				caster,
				laserInfo.GetAffectedTeams(caster),
				laserInfo.penetrateLos,
				laserInfo.maxTargets,
				false,
				true,
				out endPos,
				nonActorTargetInfo);
		}
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.IsReaction)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SenseiStats.NumBuffsPlusDebuffsFromAppendStatus);
		}
	}
#endif
}
