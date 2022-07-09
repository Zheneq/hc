// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class BlasterStretchingCone : Ability
{
	public enum DamageChangeMode
	{
		IncreaseFromMin,
		IncreaseFromMax
	}

	[Header("-- Cone Limits")]
	public float m_minLength;
	public float m_maxLength;
	public float m_minAngle;
	public float m_maxAngle;
	public AreaEffectUtils.StretchConeStyle m_stretchStyle = AreaEffectUtils.StretchConeStyle.DistanceSquared;
	public float m_coneBackwardOffset;
	public bool m_penetrateLineOfSight;
	[Header("-- On Hit")]
	public int m_damageAmountNormal;
	// added in rogues
	// public int m_damageAmountOvercharged;
	public int m_extraDamageForSingleHit;
	public bool m_removeOverchargeEffectOnCast;
	
	// removed in rogues
	[Header("-- Damage scaling by distance from enemy")]
	public float m_extraDamagePerSquareDistanceFromEnemy;
	// end removed in rogues
	
	[Header("-- Damage Change by Angle and distance")]
	public DamageChangeMode m_angleDamageChangeMode;
	public int m_anglesPerDamageChange;
	public DamageChangeMode m_distDamageChangeMode = DamageChangeMode.IncreaseFromMax;
	public float m_distPerDamageChange;
	public int m_maxDamageChange;
	[Header("-- Effects On Hit")]
	public StandardEffectInfo m_normalEnemyEffect;
	public StandardEffectInfo m_overchargedEnemyEffect;
	public StandardEffectInfo m_singleEnemyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_overchargedCastSequencePrefab;

	private AbilityMod_BlasterStretchingCone m_abilityMod;
	private BlasterOvercharge m_overchargeAbility;
	private BlasterDashAndBlast m_dashAndBlastAbility;
	private Blaster_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedNormalEnemyEffect;
	private StandardEffectInfo m_cachedOverchargedEnemyEffect;
	private StandardEffectInfo m_cachedSingleEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Stretching Cone";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Blaster_SyncComponent>();
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_overchargeAbility = component.GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge;
			m_dashAndBlastAbility = component.GetAbilityOfType(typeof(BlasterDashAndBlast)) as BlasterDashAndBlast;
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_StretchCone(this, GetMinLength(), GetMaxLength(), GetMinAngle(), GetMaxAngle(), m_stretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxLength();
	}

	private void SetCachedFields()
	{
		m_cachedNormalEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_normalEnemyEffectMod.GetModifiedValue(m_normalEnemyEffect)
			: m_normalEnemyEffect;
		m_cachedOverchargedEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_overchargedEnemyEffectMod.GetModifiedValue(m_overchargedEnemyEffect)
			: m_overchargedEnemyEffect;
		m_cachedSingleEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(m_singleEnemyHitEffect)
			: m_singleEnemyHitEffect;
	}

	public float GetMinLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_minLengthMod.GetModifiedValue(m_minLength) 
			: m_minLength;
	}

	public float GetMaxLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxLengthMod.GetModifiedValue(m_maxLength) 
			: m_maxLength;
	}

	public float GetMinAngle()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_minAngleMod.GetModifiedValue(m_minAngle) 
			: m_minAngle;
	}

	public float GetMaxAngle()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxAngleMod.GetModifiedValue(m_maxAngle) 
			: m_maxAngle;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset) 
			: m_coneBackwardOffset;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight) 
			: m_penetrateLineOfSight;
	}

	public int GetDamageAmountNormal()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal)
			: m_damageAmountNormal;
	}

	// added in rogues
	// public int GetDamageAmountOvercharged()
	// {
	// 	return m_abilityMod ? m_abilityMod.m_damageAmountOverchargedMod.GetModifiedValue(m_damageAmountOvercharged) : m_damageAmountOvercharged;
	// }

	public int GetExtraDamageForSingleHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit) 
			: m_extraDamageForSingleHit;
	}

	// removed in rogues
	public float GetExtraDamagePerSquareDistanceFromEnemy()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamagePerSquareDistanceFromEnemyMod.GetModifiedValue(m_extraDamagePerSquareDistanceFromEnemy) 
			: m_extraDamagePerSquareDistanceFromEnemy;
	}

	public int GetAnglesPerDamageChange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_anglesPerDamageChangeMod.GetModifiedValue(m_anglesPerDamageChange) 
			: m_anglesPerDamageChange;
	}

	public float GetDistPerDamageChange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_distPerDamageChangeMod.GetModifiedValue(m_distPerDamageChange) 
			: m_distPerDamageChange;
	}

	public int GetMaxDamageChange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxDamageChangeMod.GetModifiedValue(m_maxDamageChange) 
			: m_maxDamageChange;
	}

	public StandardEffectInfo GetNormalEnemyEffect()
	{
		return m_cachedNormalEnemyEffect ?? m_normalEnemyEffect;
	}

	public StandardEffectInfo GetOverchargedEnemyEffect()
	{
		return m_cachedOverchargedEnemyEffect ?? m_overchargedEnemyEffect;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return m_cachedSingleEnemyHitEffect ?? m_singleEnemyHitEffect;
	}

	private bool AmOvercharged(ActorData caster)
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Blaster_SyncComponent>();
		}
		// reactor
		return m_syncComp.m_overchargeBuffs > 0;
		// rogues
		// return m_syncComp.m_overchargeCount > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		return m_syncComp != null
		       // reactor
		       && m_syncComp.m_overchargeBuffs > 1
		       // rogues
		       // && m_syncComp.m_overchargeCount > 1
		       && m_overchargeAbility != null
		       && m_overchargeAbility.GetExtraDamageForMultiCast() > 0
			? m_overchargeAbility.GetExtraDamageForMultiCast()
			: 0;
	}

	public int GetCurrentModdedDamage()
	{
		if (AmOvercharged(ActorData))
		{
			// reactor
			return GetDamageAmountNormal() + m_overchargeAbility.GetExtraDamage() + GetMultiStackOverchargeDamage();
			// rogues
			// return GetDamageAmountOvercharged() + GetMultiStackOverchargeDamage();
		}
		return GetDamageAmountNormal();
	}

	public int GetExtraDamageFromAngle(float angleNow)
	{
		if (GetAnglesPerDamageChange() > 0)
		{
			int num = 0;
			if (m_angleDamageChangeMode == DamageChangeMode.IncreaseFromMin)
			{
				num = Mathf.Max((int)(angleNow - GetMinAngle()), 0);
			}
			else
			{
				num = Mathf.Max((int)(GetMaxAngle() - angleNow), 0);
			}
			int num2 = num / GetAnglesPerDamageChange();
			if (GetMaxDamageChange() > 0)
			{
				num2 = Mathf.Clamp(num2, 0, GetMaxDamageChange());
			}
			return num2;
		}
		return 0;
	}

	public int GetExtraDamageFromRadius(float radiusInSquares)
	{
		if (GetDistPerDamageChange() > 0.1f)
		{
			float num = 0f;
			if (m_distDamageChangeMode == DamageChangeMode.IncreaseFromMin)
			{
				num = Mathf.Max(radiusInSquares - GetMinLength(), 0f);
			}
			else
			{
				num = Mathf.Max(GetMaxLength() - radiusInSquares, 0f);
			}
			int num2 = Mathf.RoundToInt(num / GetDistPerDamageChange());
			if (GetMaxDamageChange() > 0)
			{
				num2 = Mathf.Clamp(num2, 0, GetMaxDamageChange());
			}
			return num2;
		}
		return 0;
	}

	// removed in rogues
	public int GetExtraDamageForEnemy(ActorData caster, ActorData target)
	{
		if (GetExtraDamagePerSquareDistanceFromEnemy() > 0f)
		{
			float num = VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), target.GetFreePos()) - 1.4f;
			return Mathf.RoundToInt(GetExtraDamagePerSquareDistanceFromEnemy() * num);
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterStretchingCone abilityMod_BlasterStretchingCone = modAsBase as AbilityMod_BlasterStretchingCone;
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal)
			: m_damageAmountNormal);
		// reactor
		AddTokenFloat(tokens, "ExtraDamagePerSquareDistanceFromEnemy", string.Empty, m_extraDamagePerSquareDistanceFromEnemy);
		// rogues
		// AddTokenInt(tokens, "DamageAmountOvercharged", string.Empty, abilityMod_BlasterStretchingCone != null
		// 	? abilityMod_BlasterStretchingCone.m_damageAmountOverchargedMod.GetModifiedValue(m_damageAmountOvercharged)
		// 	: m_damageAmountOvercharged);
		AddTokenInt(tokens, "ExtraDamageForSingleHit", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit);
		AddTokenInt(tokens, "AnglesPerDamageChange", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_anglesPerDamageChangeMod.GetModifiedValue(m_anglesPerDamageChange)
			: m_anglesPerDamageChange);
		AddTokenInt(tokens, "MaxDamageChange", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_maxDamageChangeMod.GetModifiedValue(m_maxDamageChange)
			: m_maxDamageChange);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_normalEnemyEffectMod.GetModifiedValue(m_normalEnemyEffect)
			: m_normalEnemyEffect, "NormalEnemyEffect", m_normalEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_overchargedEnemyEffectMod.GetModifiedValue(m_overchargedEnemyEffect)
			: m_overchargedEnemyEffect, "OverchargedEnemyEffect", m_overchargedEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_singleEnemyHitEffectMod.GetModifiedValue(m_singleEnemyHitEffect)
			: m_singleEnemyHitEffect, "SingleEnemyHitEffect", m_singleEnemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetCurrentModdedDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = Targeter as AbilityUtil_Targeter_StretchCone;
			int baseDamage = 0;
			if (abilityUtil_Targeter_StretchCone != null)
			{
				baseDamage += GetExtraDamageFromAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				baseDamage += GetExtraDamageFromRadius(abilityUtil_Targeter_StretchCone.LastConeRadiusInSquares);
				
				// removed in rogues
				baseDamage += GetExtraDamageForEnemy(ActorData, targetActor);
				// end removed in rogues
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int damage = GetCurrentModdedDamage() + baseDamage;
				if (visibleActorsCountByTooltipSubject == 1)
				{
					damage += GetExtraDamageForSingleHit();
				}
				dictionary[AbilityTooltipSymbol.Damage] = damage;
			}
		}
		return dictionary;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = GetMinLength() * Board.Get().squareSize;
		max = GetMaxLength() * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BlasterStretchingCone))
		{
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BlasterStretchingCone;
		Setup();
		if (m_dashAndBlastAbility != null && m_dashAndBlastAbility.m_useConeParamsFromPrimary)
		{
			m_dashAndBlastAbility.OnPrimaryAttackModChange();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
		if (m_dashAndBlastAbility != null && m_dashAndBlastAbility.m_useConeParamsFromPrimary)
		{
			m_dashAndBlastAbility.OnPrimaryAttackModChange();
		}
	}

#if SERVER
	// added in rogues
	public GameObject GetCurrentCastSequence(ActorData caster)
	{
		if (AmOvercharged(caster))
		{
			return m_overchargedCastSequencePrefab;
		}
		return m_castSequencePrefab;
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 freePos = targets[0].FreePos;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		Vector3 aimDirection = targets[0].AimDirection;
		float minLength = GetMinLength();
		float maxLength = GetMaxLength();
		float minAngle = GetMinAngle();
		float maxAngle = GetMaxAngle();
		AreaEffectUtils.GatherStretchConeDimensions(freePos, loSCheckPos, minLength, maxLength, minAngle, maxAngle, m_stretchStyle, out var lengthInSquares, out var angleInDegrees);
		BlasterStretchConeSequence.ExtraParams extraParams = new BlasterStretchConeSequence.ExtraParams();
		extraParams.angleInDegrees = angleInDegrees;
		extraParams.lengthInSquares = lengthInSquares;
		extraParams.forwardAngle = VectorUtils.HorizontalAngle_Deg(aimDirection);
		return new ServerClientUtils.SequenceStartData(GetCurrentCastSequence(caster), caster.GetCurrentBoardSquare(), additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, extraParams.ToArray());
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> list = FindHitActors(targets, caster, nonActorTargetInfo, out var angleNow, out var radiusInSquares);
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		foreach (ActorData target in list)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, loSCheckPos));
			int num = GetCurrentModdedDamage() + GetExtraDamageFromAngle(angleNow) + GetExtraDamageFromRadius(radiusInSquares);
			if (list.Count == 1)
			{
				num += GetExtraDamageForSingleHit();
			}
			actorHitResults.SetBaseDamage(num);
			if (AmOvercharged(caster))
			{
				actorHitResults.AddStandardEffectInfo(GetOverchargedEnemyEffect());
				if (m_overchargeAbility != null)
				{
					// custom
					actorHitResults.AddStandardEffectInfo(m_overchargeAbility.GetExtraEffectOnOtherAbilities());
					// rogues
					// actorHitResults.AddStandardEffectInfo(m_overchargeAbility.GetExtraEffectForStretchingCone());
				}
			}
			else
			{
				actorHitResults.AddStandardEffectInfo(GetNormalEnemyEffect());
			}
			if (list.Count == 1)
			{
				actorHitResults.AddStandardEffectInfo(GetSingleEnemyHitEffect());
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (m_removeOverchargeEffectOnCast && AmOvercharged(caster))
		{
			Effect effect = ServerEffectManager.Get().GetEffect(caster, typeof(BlasterOverchargeEffect));
			if (effect != null)
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, loSCheckPos));
				actorHitResults2.AddEffectForRemoval(effect, ServerEffectManager.Get().GetActorEffects(caster));
				abilityResults.StoreActorHit(actorHitResults2);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo, out float angleNow, out float radiusInSquares)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 freePos = targets[0].FreePos;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		float minLength = GetMinLength();
		float maxLength = GetMaxLength();
		float minAngle = GetMinAngle();
		float maxAngle = GetMaxAngle();
		AreaEffectUtils.GatherStretchConeDimensions(freePos, loSCheckPos, minLength, maxLength, minAngle, maxAngle, m_stretchStyle, out var num, out var num2);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(loSCheckPos, coneCenterAngleDegrees, num2, num, GetConeBackwardOffset(), PenetrateLineOfSight(), caster, caster.GetOtherTeams(), nonActorTargetInfo);
		angleNow = num2;
		radiusInSquares = num;
		return actorsInCone;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0 && (AmOvercharged(caster) || m_syncComp.m_lastOverchargeTurn == GameFlowData.Get().CurrentTurn))
		{
			int damageAmountNormal = GetDamageAmountNormal();
			int addAmount = results.BaseDamage - damageAmountNormal;
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BlasterStats.DamageAddedFromOvercharge, addAmount);
		}
	}
#endif
}
