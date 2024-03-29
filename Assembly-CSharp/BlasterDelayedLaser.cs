// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class BlasterDelayedLaser : Ability
{
	[Header("-- Laser Data")]
	public bool m_penetrateLineOfSight = true;
	public float m_length = 8f;
	public float m_width = 2f;
	[Header("-- Initial Placement Phase")]
	public AbilityPriority m_placementPhase = AbilityPriority.Prep_Offense;
	[Header("-- Delay Data")]
	public int m_turnsBeforeTriggering = 1;
	public bool m_remoteTriggerMode = true;
	public bool m_remoteTriggerIsFreeAction = true;
	public int m_triggerAnimationIndex = 11;
	public bool m_triggerAimAtBlaster;
	[Header("-- On Hit")]
	public int m_damageAmount = 40;
	public StandardEffectInfo m_effectOnHit;
	public int m_extraDamageToNearEnemy;
	public float m_nearDistance;
	[Header("-- On Cast Hit Effect")]
	public StandardEffectInfo m_onCastEnemyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	[Header("    For Satellite, persistent")]
	public GameObject m_laserGroundSequencePrefab;
	[Header("    For laser firing, with gameplay hits")]
	public GameObject m_laserTriggerSequencePrefab;
	[Header("    For laser firing, only on caster")]
	public GameObject m_laserTriggerOnCasterSequencePrefab;

	private AbilityUtil_Targeter_Laser m_laserTargeter;
	private AbilityMod_BlasterDelayedLaser m_abilityMod;
	private BlasterOvercharge m_overchargeAbility;
	private Blaster_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedEffectOnHit;
	private StandardEffectInfo m_cachedOnCastEnemyHitEffect;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComponent = GetComponent<Blaster_SyncComponent>();
		m_overchargeAbility = GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge;
		SetCachedFields();
		m_laserTargeter = new AbilityUtil_Targeter_BlasterDelayedLaser(this, m_syncComponent, TriggerAimAtBlaster(), GetWidth(), GetLength(), PenetrateLineOfSight());
		Targeter = m_laserTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return m_syncComponent != null && !m_syncComponent.m_canActivateDelayedLaser;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLength();
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnHit = m_abilityMod != null
			? m_abilityMod.m_effectOnHitMod.GetModifiedValue(m_effectOnHit)
			: m_effectOnHit;
		m_cachedOnCastEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_onCastEnemyHitEffectMod.GetModifiedValue(m_onCastEnemyHitEffect)
			: m_onCastEnemyHitEffect;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public float GetLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_lengthMod.GetModifiedValue(m_length)
			: m_length;
	}

	public float GetWidth()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width) 
			: m_width;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetEffectOnHit()
	{
		return m_cachedEffectOnHit ?? m_effectOnHit;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		return m_cachedOnCastEnemyHitEffect ?? m_onCastEnemyHitEffect;
	}

	public bool TriggerAimAtBlaster()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_triggerAimAtBlasterMod.GetModifiedValue(m_triggerAimAtBlaster) 
			: m_triggerAimAtBlaster;
	}

	public int GetExtraDamageToNearEnemy()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageToNearEnemyMod.GetModifiedValue(m_extraDamageToNearEnemy) 
			: m_extraDamageToNearEnemy;
	}

	public float GetNearDistance()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_nearDistanceMod.GetModifiedValue(m_nearDistance) 
			: m_nearDistance;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		ActorData actorData = ActorData;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null
		    && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy)
		    && actorData != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int damage = GetDamageAmount();
			if (m_syncComponent != null
			    // reactor
			    && m_syncComponent.m_overchargeBuffs > 0
			    // rogues
			    // && m_syncComponent.m_overchargeCount > 0
			    && m_overchargeAbility != null
			    && m_overchargeAbility.GetExtraDamageForDelayedLaser() > 0)
			{
				damage += m_overchargeAbility.GetExtraDamageForDelayedLaser();
			}

			Vector3 src = m_syncComponent.m_canActivateDelayedLaser
				? m_syncComponent.m_delayedLaserStartPos
				: actorData.GetFreePos();
			if (GetExtraDamageToNearEnemy() > 0 && GetNearDistance() > 0f)
			{
				float nearDistInSquares = GetNearDistance() * Board.Get().squareSize;
				Vector3 vector = targetActor.GetFreePos() - src;
				vector.y = 0f;
				if (vector.magnitude <= nearDistInSquares)
				{
					damage += GetExtraDamageToNearEnemy();
				}
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterDelayedLaser abilityMod_BlasterDelayedLaser = modAsBase as AbilityMod_BlasterDelayedLaser;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_BlasterDelayedLaser != null
			? abilityMod_BlasterDelayedLaser.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterDelayedLaser != null
			? abilityMod_BlasterDelayedLaser.m_effectOnHitMod.GetModifiedValue(m_effectOnHit)
			: m_effectOnHit, "EffectOnHit", m_effectOnHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterDelayedLaser != null
			? abilityMod_BlasterDelayedLaser.m_onCastEnemyHitEffectMod.GetModifiedValue(m_onCastEnemyHitEffect)
			: m_onCastEnemyHitEffect, "OnCastEnemyHitEffect", m_onCastEnemyHitEffect);
		AddTokenInt(tokens, "ExtraDamageToNearEnemy", string.Empty, abilityMod_BlasterDelayedLaser != null
			? abilityMod_BlasterDelayedLaser.m_extraDamageToNearEnemyMod.GetModifiedValue(m_extraDamageToNearEnemy)
			: m_extraDamageToNearEnemy);
		AddTokenInt(tokens, "MaxTurnsBeforeTrigger", string.Empty, m_turnsBeforeTriggering);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterDelayedLaser))
		{
			m_abilityMod = abilityMod as AbilityMod_BlasterDelayedLaser;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override bool IsFreeAction()
	{
		return m_remoteTriggerMode
		       && m_syncComponent != null
		       && m_syncComponent.m_canActivateDelayedLaser
			? m_remoteTriggerIsFreeAction
			: base.IsFreeAction();
	}

	public override AbilityPriority GetRunPriority()
	{
		return m_remoteTriggerMode
		       && m_syncComponent != null
		       && m_syncComponent.m_canActivateDelayedLaser
		       && GameFlowData.Get().CurrentTurn > m_syncComponent.m_lastPlacementTurn
			? base.GetRunPriority()
			: m_placementPhase;
	}

	public override TargetData[] GetTargetData()
	{
		return m_remoteTriggerMode
		       && m_syncComponent != null
		       && m_syncComponent.m_canActivateDelayedLaser
			? new TargetData[0]
			: base.GetTargetData();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return m_syncComponent != null && m_syncComponent.m_canActivateDelayedLaser
			? ActorModelData.ActionAnimationType.None
			: base.GetActionAnimType();
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return m_syncComponent != null
		       && m_syncComponent.m_canActivateDelayedLaser
		       && animIndex == (int)base.GetActionAnimType();
	}

#if SERVER
	// added in rogues
	public bool CastingToDetonate()
	{
		return m_remoteTriggerMode
		       && m_syncComponent != null
		       && m_syncComponent.m_canActivateDelayedLaser;
	}

	// added in rogues
	public override bool SkipTheatricsAnimationEntry(ActorData caster)
	{
		return CastingToDetonate();
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (!CastingToDetonate() && m_syncComponent != null)
		{
			m_syncComponent.Networkm_lastPlacementTurn = GameFlowData.Get().CurrentTurn;
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (CastingToDetonate())
		{
			SequenceSource sequenceSource = new SequenceSource(null, null, true, additionalData.m_sequenceSource);
			sequenceSource.SetWaitForClientEnable(false);
			return new ServerClientUtils.SequenceStartData(SequenceLookup.Get().GetSimpleHitSequencePrefab(), caster.GetFreePos(), caster.AsArray(), caster, sequenceSource);
		}
		else
		{
			GetHitActors(targets, caster, out Vector3 endPos, null);
			HealLaserSequence.ExtraParams extraParams = new HealLaserSequence.ExtraParams
			{
				endPos = endPos
			};
			return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, caster.GetFreePos(), additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, new Sequence.IExtraSequenceParams[]
			{
				extraParams
			});
		}
	}

	// added in rogues
	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out Vector3 laserEndPos, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 startPos = caster.GetLoSCheckPos();
		Vector3 dir = targets[0].AimDirection;
		List<Team> otherTeams = caster.GetOtherTeams();
		return AreaEffectUtils.GetActorsInLaser(startPos, dir, GetLength(), GetWidth(), caster, otherTeams, PenetrateLineOfSight(), -1, false, true, out laserEndPos, nonActorTargetInfo);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (CastingToDetonate())
		{
			GatherAbilityResults_RemoteDetonate(targets, caster, ref abilityResults);
		}
		else
		{
			GatherAbilityResults_FirstCast(targets, caster, ref abilityResults);
		}
	}

	// added in rogues
	private void GatherAbilityResults_RemoteDetonate(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		BlasterDelayedLaserEffect blasterDelayedLaserEffect = null;
		List<Effect> worldEffectsByCaster = ServerEffectManager.Get().GetWorldEffectsByCaster(caster);
		foreach (Effect effect in worldEffectsByCaster)
		{
			if (effect is BlasterDelayedLaserEffect)
			{
				blasterDelayedLaserEffect = effect as BlasterDelayedLaserEffect;
				break;
			}
		}
		int num = blasterDelayedLaserEffect == null ? 0 : blasterDelayedLaserEffect.m_time.age;
		int overrideValue = GetModdedCooldown() - num;
		MiscHitEventData_OverrideCooldown hitEvent = new MiscHitEventData_OverrideCooldown(caster.GetAbilityData().GetActionTypeOfAbility(this), overrideValue);
		actorHitResults.AddMiscHitEvent(hitEvent);
		abilityResults.StoreActorHit(actorHitResults);
		if (ServerAbilityUtils.CurrentlyGatheringRealResults())
		{
			m_syncComponent.m_lastCinematicRequested = abilityResults.CinematicRequested;
		}
	}

	// added in rogues
	private void GatherAbilityResults_FirstCast(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (GetOnCastEnemyHitEffect().m_applyEffect)
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			foreach (ActorData target in GetHitActors(targets, caster, out Vector3 _, nonActorTargetInfo))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
				actorHitResults.AddStandardEffectInfo(GetOnCastEnemyHitEffect());
				abilityResults.StoreActorHit(actorHitResults);
			}
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
		if (m_remoteTriggerMode)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			MiscHitEventData_OverrideCooldown hitEvent = new MiscHitEventData_OverrideCooldown(caster.GetAbilityData().GetActionTypeOfAbility(this), 0);
			actorHitResults2.AddMiscHitEvent(hitEvent);
			abilityResults.StoreActorHit(actorHitResults2);
		}
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(caster.GetFreePos()));
		int overchargeExtraDamage = m_overchargeAbility != null ? m_overchargeAbility.GetExtraDamageForDelayedLaser() : 0;
		Effect effect = new BlasterDelayedLaserEffect(AsEffectSource(), caster, targets[0], GetLength(), GetWidth(), PenetrateLineOfSight(), TriggerAimAtBlaster(), m_turnsBeforeTriggering, m_remoteTriggerMode, GetDamageAmount(), overchargeExtraDamage, GetExtraDamageToNearEnemy(), GetNearDistance(), GetEffectOnHit(), m_laserGroundSequencePrefab, m_laserTriggerSequencePrefab, m_laserTriggerOnCasterSequencePrefab, m_triggerAnimationIndex, abilityResults.SequenceSource);
		if (ServerAbilityUtils.CurrentlyGatheringRealResults())
		{
			m_syncComponent.m_lastCinematicRequested = -1;
		}
		positionHitResults.AddEffect(effect);
		abilityResults.StorePositionHit(positionHitResults);
	}

	// added in rogues
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BlasterStats.LurkerDroneDamage, results.FinalDamage);
		}
	}
#endif
}
