using System.Collections.Generic;
using UnityEngine;

public class SoldierCardinalLine : Ability
{
	[Header("-- Targeting (shape for position targeter, line width for strafe hit area --")]
	public bool m_useBothCardinalDir;
	public AbilityAreaShape m_positionShape = AbilityAreaShape.Two_x_Two;
	public float m_lineWidth = 2f;
	public bool m_penetrateLos = true;
	[Header("-- On Hit Stuff --")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Extra Damage for near center")]
	public float m_nearCenterDistThreshold;
	public int m_extraDamageForNearCenterTargets;
	[Header("-- AoE around targets --")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Three_x_Three;
	public int m_aoeDamage;
	[Header("-- Subsequent Turn Hits --")]
	public int m_numSubsequentTurns;
	public int m_damageOnSubsequentTurns;
	public StandardEffectInfo m_enemyEffectOnSubsequentTurns;
	[Header("-- Sequences --")]
	public GameObject m_projectileSequencePrefab;
	
	private AbilityMod_SoldierCardinalLine m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedEnemyEffectOnSubsequentTurns;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cardinal Line";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		ClearTargeters();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(
			this,
			GetPositionShape(),
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Never);
		Targeters.Add(item);
		AbilityUtil_Targeter_SoldierCardinalLines abilityUtil_Targeter_SoldierCardinalLines = new AbilityUtil_Targeter_SoldierCardinalLines(
			this,
			GetPositionShape(),
			GetLineWidth(),
			PenetrateLos(),
			UseBothCardinalDir(),
			GetAoeDamage() > 0,
			GetAoeShape());
		abilityUtil_Targeter_SoldierCardinalLines.SetUseMultiTargetUpdate(true);
		abilityUtil_Targeter_SoldierCardinalLines.SetAffectedGroups(true, false, false);
		Targeters.Add(abilityUtil_Targeter_SoldierCardinalLines);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		if (targetIndex == 1)
		{
			return TargetingParadigm.Direction;
		}
		return base.GetControlpadTargetingParadigm(targetIndex);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			overridePos = AreaEffectUtils.GetCenterOfShape(GetPositionShape(), targetsSoFar[0]);
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = 1f;
			max = 1f;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedEnemyEffectOnSubsequentTurns = m_abilityMod != null
			? m_abilityMod.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(m_enemyEffectOnSubsequentTurns)
			: m_enemyEffectOnSubsequentTurns;
	}

	public bool UseBothCardinalDir()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useBothCardinalDirMod.GetModifiedValue(m_useBothCardinalDir)
			: m_useBothCardinalDir;
	}

	public AbilityAreaShape GetPositionShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_positionShapeMod.GetModifiedValue(m_positionShape)
			: m_positionShape;
	}

	public float GetLineWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lineWidthMod.GetModifiedValue(m_lineWidth)
			: m_lineWidth;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public float GetNearCenterDistThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_nearCenterDistThresholdMod.GetModifiedValue(m_nearCenterDistThreshold)
			: m_nearCenterDistThreshold;
	}

	public int GetExtraDamageForNearCenterTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(m_extraDamageForNearCenterTargets)
			: m_extraDamageForNearCenterTargets;
	}

	public AbilityAreaShape GetAoeShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape)
			: m_aoeShape;
	}

	public int GetAoeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage;
	}

	public int GetNumSubsequentTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numSubsequentTurnsMod.GetModifiedValue(m_numSubsequentTurns)
			: m_numSubsequentTurns;
	}

	public int GetDamageOnSubsequentTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageOnSubsequentTurnsMod.GetModifiedValue(m_damageOnSubsequentTurns)
			: m_damageOnSubsequentTurns;
	}

	public StandardEffectInfo GetEnemyEffectOnSubsequentTurns()
	{
		return m_cachedEnemyEffectOnSubsequentTurns ?? m_enemyEffectOnSubsequentTurns;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		AbilityUtil_Targeter_SoldierCardinalLines targeter = Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SoldierCardinalLines;
		if (currentTargeterIndex > 0
		    && currentTargeterIndex < Targeters.Count
		    && targeter != null)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				int damage = 0;
				if (targeter.m_directHitActorToCenterDist.ContainsKey(targetActor))
				{
					damage += GetDamageAmount();
					if (GetExtraDamageForNearCenterTargets() > 0
					    && targeter.m_directHitActorToCenterDist[targetActor] <= GetNearCenterDistThreshold() * Board.Get().squareSize)
					{
						damage += GetExtraDamageForNearCenterTargets();
					}
				}
				if (targeter.m_aoeHitActors.Contains(targetActor))
				{
					damage += GetAoeDamage();
				}
				dictionary[AbilityTooltipSymbol.Damage] = damage;
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierCardinalLine abilityMod_SoldierCardinalLine = modAsBase as AbilityMod_SoldierCardinalLine;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage);
		AddTokenInt(tokens, "ExtraDamageForNearCenterTargets", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(m_extraDamageForNearCenterTargets)
			: m_extraDamageForNearCenterTargets);
		AddTokenInt(tokens, "NumSubsequentTurns", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_numSubsequentTurnsMod.GetModifiedValue(m_numSubsequentTurns)
			: m_numSubsequentTurns);
		AddTokenInt(tokens, "DamageOnSubsequentTurns", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_damageOnSubsequentTurnsMod.GetModifiedValue(m_damageOnSubsequentTurns)
			: m_damageOnSubsequentTurns);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(m_enemyEffectOnSubsequentTurns)
			: m_enemyEffectOnSubsequentTurns, "EnemyEffectOnSubsequentTurns", m_enemyEffectOnSubsequentTurns);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierCardinalLine))
		{
			m_abilityMod = abilityMod as AbilityMod_SoldierCardinalLine;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
