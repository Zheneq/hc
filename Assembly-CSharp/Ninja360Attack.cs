using System.Collections.Generic;
using UnityEngine;

public class Ninja360Attack : Ability
{
	public enum TargetingMode
	{
		Shape,
		Cone,
		Laser
	}

	[Separator("Targeting")]
	public TargetingMode m_targetingMode = TargetingMode.Laser;
	public bool m_penetrateLineOfSight;
	[Header("  (( if using Laser ))")]
	public LaserTargetingInfo m_laserInfo;
	[Header("  (( if using Cone ))")]
	public ConeTargetingInfo m_coneInfo;
	public float m_innerConeAngle;
	[Header("  (( if using Shape ))")]
	public AbilityAreaShape m_targeterShape = AbilityAreaShape.Three_x_Three;
	[Separator("On Hit")]
	public int m_damageAmount = 15;
	[Header("-- Damage for Inner Area Hit --")]
	public int m_innerAreaDamage = 30;
	[Space(10f)]
	public StandardEffectInfo m_enemyHitEffect;
	public bool m_useDifferentEffectForInnerCone;
	public StandardEffectInfo m_innerConeEnemyHitEffect;
	[Header("-- Energy Gain on Marked Target --")]
	public int m_energyGainOnMarkedHit;
	public int m_selfHealOnMarkedHit;
	[Separator("[Deathmark] Effect", "magenta")]
	public bool m_applyDeathmarkEffect = true;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private const int c_innerConeIdentifier = 1;
	private Ninja_SyncComponent m_syncComp;
	private AbilityMod_Ninja360Attack m_abilityMod;
	private LaserTargetingInfo m_cachedLaserInfo;
	private ConeTargetingInfo m_cachedConeInfo;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedInnerConeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ninja Basic Attack";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Ninja_SyncComponent>();
		}
		if (m_targetingMode == TargetingMode.Laser)
		{
			LaserTargetingInfo laserInfo = GetLaserInfo();
			Targeter = new AbilityUtil_Targeter_Laser(
				this,
				laserInfo.width,
				laserInfo.range,
				PenetrateLineOfSight(),
				laserInfo.maxTargets);
		}
		else if (m_targetingMode == TargetingMode.Cone)
		{
			ConeTargetingInfo coneInfo = GetConeInfo();
			float radiusInSquares = coneInfo.m_radiusInSquares;
			List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>();
			list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneInfo.m_widthAngleDeg, radiusInSquares));
			if (GetInnerConeAngle() > 0f)
			{
				list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(GetInnerConeAngle(), radiusInSquares));
			}
			AbilityUtil_Targeter_MultipleCones abilityUtil_Targeter_MultipleCones = new AbilityUtil_Targeter_MultipleCones(
				this,
				list,
				coneInfo.m_backwardsOffset,
				PenetrateLineOfSight(),
				true,
				true, 
				false,
				GetSelfHealOnMarkedHit() > 0);
			if (GetSelfHealOnMarkedHit() > 0)
			{
				abilityUtil_Targeter_MultipleCones.m_affectCasterDelegate = IncludeCasterForTargeter;
			}
			Targeter = abilityUtil_Targeter_MultipleCones;
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Shape(this, GetTargeterShape(), PenetrateLineOfSight());
		}
	}

	private bool IncludeCasterForTargeter(ActorData caster, List<ActorData> addedSoFar)
	{
		if (GetSelfHealOnMarkedHit() <= 0)
		{
			return false;
		}
		foreach (ActorData actor in addedSoFar)
		{
			if (IsActorMarked(actor))
			{
				return true;
			}
		}
		return false;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nPlease edit [Deathmark] info on Ninja sync component.\n<color=cyan>-- For Art --</color>\nOn Sequence, for HitActorGroupOnAnimEventSequence components, use:\n" + 1 + " for Inner cone group identifier\n";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		switch (m_targetingMode)
		{
			case TargetingMode.Laser:
				return GetLaserInfo().range;
			case TargetingMode.Cone:
				return GetConeInfo().m_radiusInSquares;
			default:
				return 0f;
		}
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedConeInfo = m_abilityMod != null
			? m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo)
			: m_coneInfo;
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedInnerConeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_innerConeEnemyHitEffectMod.GetModifiedValue(m_innerConeEnemyHitEffect)
			: m_innerConeEnemyHitEffect;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return m_cachedConeInfo ?? m_coneInfo;
	}

	public float GetInnerConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerConeAngleMod.GetModifiedValue(m_innerConeAngle)
			: m_innerConeAngle;
	}

	public AbilityAreaShape GetTargeterShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targeterShapeMod.GetModifiedValue(m_targeterShape)
			: m_targeterShape;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetInnerAreaDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerAreaDamageMod.GetModifiedValue(m_innerAreaDamage)
			: m_innerAreaDamage;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public bool UseDifferentEffectForInnerCone()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useDifferentEffectForInnerConeMod.GetModifiedValue(m_useDifferentEffectForInnerCone)
			: m_useDifferentEffectForInnerCone;
	}

	public StandardEffectInfo GetInnerConeEnemyHitEffect()
	{
		return m_cachedInnerConeEnemyHitEffect ?? m_innerConeEnemyHitEffect;
	}

	public int GetEnergyGainOnMarkedHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainOnMarkedHitMod.GetModifiedValue(m_energyGainOnMarkedHit)
			: m_energyGainOnMarkedHit;
	}

	public int GetSelfHealOnMarkedHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealOnMarkedHitMod.GetModifiedValue(m_selfHealOnMarkedHit)
			: m_selfHealOnMarkedHit;
	}

	public bool ApplyDeathmarkEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(m_applyDeathmarkEffect)
			: m_applyDeathmarkEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSelfHealOnMarkedHit());
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (m_targetingMode != TargetingMode.Cone)
		{
			return false;
		}
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int damage = GetDamageAmount();
			if (GetInnerAreaDamage() > 0 && GetInnerConeAngle() > 0f)
			{
				ActorData actorData = ActorData;
				Vector3 lastUpdateAimDir = Targeter.LastUpdateAimDir;
				float coneForwardAngle = VectorUtils.HorizontalAngle_Deg(lastUpdateAimDir);
				if (IsActorInInnerCone(targetActor, actorData, coneForwardAngle))
				{
					damage = GetInnerAreaDamage();
				}
			}
			results.m_damage = damage;
			return true;
		}
		if (targetActor == ActorData && GetSelfHealOnMarkedHit() > 0)
		{
			int num = 0;
			List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
			for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
			{
				ActorData targetActor2 = visibleActorsInRangeByTooltipSubject[i];
				if (IsActorMarked(targetActor2))
				{
					num += GetSelfHealOnMarkedHit();
				}
			}
			results.m_healing = num;
			return true;
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyGainOnMarkedHit() <= 0)
		{
			return 0;
		}
		int num = 0;
		foreach (ActorData targetActor in Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy))
		{
			if (IsActorMarked(targetActor))
			{
				num += GetEnergyGainOnMarkedHit();
			}
		}
		return num;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage
		       && m_syncComp != null
		       && m_syncComp.m_deathmarkOnTriggerDamage > 0
		       && IsActorMarked(targetActor)
			? "\n+ " + AbilityUtils.CalculateDamageForTargeter(
				ActorData, targetActor, this, m_syncComp.m_deathmarkOnTriggerDamage, false)
			: null;
	}

	public bool IsActorInInnerCone(ActorData targetActor, ActorData caster, float coneForwardAngle)
	{
		if (m_targetingMode == TargetingMode.Cone && GetInnerConeAngle() > 0f)
		{
			ConeTargetingInfo coneInfo = GetConeInfo();
			return AreaEffectUtils.IsSquareInConeByActorRadius(
				targetActor.GetCurrentBoardSquare(),
				caster.GetFreePos(),
				coneForwardAngle,
				GetInnerConeAngle(),
				coneInfo.m_radiusInSquares,
				coneInfo.m_backwardsOffset,
				PenetrateLineOfSight(),
				caster);
		}
		return false;
	}

	public bool IsActorMarked(ActorData targetActor)
	{
		return m_syncComp != null && m_syncComp.ActorHasDeathmark(targetActor);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "InnerAreaDamage", string.Empty, m_innerAreaDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_innerConeEnemyHitEffect, "InnerConeEnemyHitEffect", m_innerConeEnemyHitEffect);
		AddTokenInt(tokens, "EnergyGainOnMarkedHit", string.Empty, m_energyGainOnMarkedHit);
		AddTokenInt(tokens, "SelfHealOnMarkedHit", string.Empty, m_selfHealOnMarkedHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_Ninja360Attack))
		{
			m_abilityMod = abilityMod as AbilityMod_Ninja360Attack;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
