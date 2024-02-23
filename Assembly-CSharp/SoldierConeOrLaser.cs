using System.Collections.Generic;
using UnityEngine;

public class SoldierConeOrLaser : Ability
{
	public enum LastUsedModeFlag
	{
		None,
		Cone,
		Laser
	}

	[Separator("Targeting")]
	public float m_coneDistThreshold = 4f;
	[Header("  Targeting: For Cone")]
	public ConeTargetingInfo m_coneInfo;
	[Header("  Targeting: For Laser")]
	public LaserTargetingInfo m_laserInfo;
	[Separator("On Hit")]
	public int m_coneDamage = 10;
	public StandardEffectInfo m_coneEnemyHitEffect;
	[Space(10f)]
	public int m_laserDamage = 20;
	public StandardEffectInfo m_laserEnemyHitEffect;
	[Separator("Extra Damage")]
	public int m_extraDamageForAlternating;
	[Space(5f)]
	public float m_closeDistThreshold = -1f;
	public int m_extraDamageForNearTarget;
	public int m_extraDamageForFromCover;
	[Space(5f)]
	public int m_extraDamageToEvaders;
	[Separator("Extra Energy (per target hit)")]
	public int m_extraEnergyForCone;
	public int m_extraEnergyForLaser;
	[Separator("Animation Indices")]
	public int m_onCastLaserAnimIndex = 1;
	public int m_onCastConeAnimIndex = 6;
	[Separator("Sequences")]
	public GameObject m_coneSequencePrefab;
	public GameObject m_laserSequencePrefab;
	public AbilityMod_SoldierConeOrLaser m_abilityMod;

	private Soldier_SyncComponent m_syncComp;
	private AbilityData m_abilityData;
	private SoldierStimPack m_stimAbility;
	private ConeTargetingInfo m_cachedConeInfo;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedConeEnemyHitEffect;
	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Soldier Cone Or Laser";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Soldier_SyncComponent>();
		}
		if (m_abilityData == null)
		{
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_abilityData != null && m_stimAbility == null)
		{
			m_stimAbility = m_abilityData.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack;
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_ConeOrLaser(this, GetConeInfo(), GetLaserInfo(), m_coneDistThreshold);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		m_cachedConeInfo = m_abilityMod != null
			? m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo)
			: m_coneInfo;
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedConeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect)
			: m_coneEnemyHitEffect;
		m_cachedLaserEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect)
			: m_laserEnemyHitEffect;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return m_cachedConeInfo ?? m_coneInfo;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public int GetConeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage)
			: m_coneDamage;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return m_cachedConeEnemyHitEffect ?? m_coneEnemyHitEffect;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage)
			: m_laserDamage;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		return m_cachedLaserEnemyHitEffect ?? m_laserEnemyHitEffect;
	}

	public int GetExtraDamageForAlternating()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForAlternatingMod.GetModifiedValue(m_extraDamageForAlternating)
			: m_extraDamageForAlternating;
	}

	public float GetCloseDistThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_closeDistThresholdMod.GetModifiedValue(m_closeDistThreshold)
			: m_closeDistThreshold;
	}

	public int GetExtraDamageForNearTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForNearTargetMod.GetModifiedValue(m_extraDamageForNearTarget)
			: m_extraDamageForNearTarget;
	}

	public int GetExtraDamageForFromCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForFromCoverMod.GetModifiedValue(m_extraDamageForFromCover)
			: m_extraDamageForFromCover;
	}

	public int GetExtraDamageToEvaders()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageToEvadersMod.GetModifiedValue(m_extraDamageToEvaders)
			: m_extraDamageToEvaders;
	}

	public int GetExtraEnergyForCone()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyForConeMod.GetModifiedValue(m_extraEnergyForCone)
			: m_extraEnergyForCone;
	}

	public int GetExtraEnergyForLaser()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyForLaserMod.GetModifiedValue(m_extraEnergyForLaser)
			: m_extraEnergyForLaser;
	}

	public bool ShouldUseExtraDamageForNearTarget(ActorData target, ActorData caster)
	{
		if (GetExtraDamageForNearTarget() > 0)
		{
			Vector3 vector = target.GetFreePos() - caster.GetFreePos();
			vector.y = 0f;
			return vector.magnitude < GetCloseDistThreshold() * Board.Get().squareSize;
		}
		return false;
	}

	public bool HasConeDamageMod()
	{
		return m_abilityMod != null && m_abilityMod.m_coneDamageMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	public bool HasLaserDamageMod()
	{
		return m_abilityMod != null && m_abilityMod.m_laserDamageMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	public bool HasNearDistThresholdMod()
	{
		return m_abilityMod != null && m_abilityMod.m_closeDistThresholdMod.operation != AbilityModPropertyFloat.ModOp.Ignore;
	}

	public bool HasExtraDamageForNearTargetMod()
	{
		return m_abilityMod != null && m_abilityMod.m_extraDamageForNearTargetMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	public bool HasConeEnergyMod()
	{
		return m_abilityMod != null && m_abilityMod.m_extraEnergyForConeMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	public bool HasLaserEnergyMod()
	{
		return m_abilityMod != null && m_abilityMod.m_extraEnergyForLaserMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetConeDamage());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetLaserDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int damage = 0;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				damage = GetConeDamage();
				if (GetExtraDamageForAlternating() > 0
				    && m_syncComp != null
				    && m_syncComp.m_lastPrimaryUsedMode == 2)
				{
					damage += GetExtraDamageForAlternating();
				}
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				damage = GetLaserDamage();
				if (GetExtraDamageForAlternating() > 0
				    && m_syncComp != null
				    && m_syncComp.m_lastPrimaryUsedMode == 1)
				{
					damage += GetExtraDamageForAlternating();
				}
			}
			if (ActorData != null)
			{
				if (ShouldUseExtraDamageForNearTarget(targetActor, ActorData))
				{
					damage += GetExtraDamageForNearTarget();
				}
				if (GetExtraDamageForFromCover() > 0 && ActorData.GetActorCover().HasAnyCover())
				{
					damage += GetExtraDamageForFromCover();
				}
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetExtraEnergyForCone() > 0 || GetExtraEnergyForLaser() > 0)
		{
			AbilityUtil_Targeter_ConeOrLaser targeter = Targeter as AbilityUtil_Targeter_ConeOrLaser;
			if (targeter != null)
			{
				int enemies = targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (targeter.m_updatingWithCone)
				{
					return enemies * GetExtraEnergyForCone();
				}
				else
				{
					return enemies * GetExtraEnergyForLaser();
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "ConeDamage", string.Empty, m_coneDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_coneEnemyHitEffect, "ConeEnemyHitEffect");
		AddTokenInt(tokens, "LaserDamage", string.Empty, m_laserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserEnemyHitEffect, "LaserEnemyHitEffect");
		AddTokenInt(tokens, "ExtraDamageForAlternating", string.Empty, m_extraDamageForAlternating);
		AddTokenInt(tokens, "ExtraDamageForNearTarget", string.Empty, m_extraDamageForNearTarget);
		AddTokenInt(tokens, "ExtraDamageForFromCover", string.Empty, m_extraDamageForFromCover);
		AddTokenInt(tokens, "ExtraDamageToEvaders", string.Empty, m_extraDamageToEvaders);
		AddTokenInt(tokens, "ExtraEnergyForCone", string.Empty, m_extraEnergyForCone);
		AddTokenInt(tokens, "ExtraEnergyForLaser", string.Empty, m_extraEnergyForLaser);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = m_coneDistThreshold - 0.1f;
		max = m_coneDistThreshold + 0.1f;
		return true;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		return m_abilityData != null
		       && m_stimAbility != null
		       && m_stimAbility.BasicAttackIgnoreCover()
		       && m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
	}

	public override bool ForceReduceCoverEffectiveness(ActorData targetActor)
	{
		return m_abilityData != null
		       && m_stimAbility != null
		       && m_stimAbility.BasicAttackReduceCoverEffectiveness()
		       && m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
	}

	private bool ShouldUseCone(Vector3 freePos, ActorData caster)
	{
		Vector3 vector = freePos - caster.GetFreePos();
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude <= m_coneDistThreshold;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return animIndex == m_onCastConeAnimIndex || animIndex == m_onCastLaserAnimIndex;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (targets != null && caster != null)
		{
			int animIndex = ShouldUseCone(targets[0].FreePos, caster) ? m_onCastConeAnimIndex : m_onCastLaserAnimIndex;
			return (ActorModelData.ActionAnimationType)animIndex;
		}
		return base.GetActionAnimType();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierConeOrLaser))
		{
			m_abilityMod = abilityMod as AbilityMod_SoldierConeOrLaser;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
