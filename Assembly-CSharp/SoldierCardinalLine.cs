using System;
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
	public int m_damageAmount = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Cardinal Line";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.ClearTargeters();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(this, this.GetPositionShape(), true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeters.Add(item);
		AbilityUtil_Targeter_SoldierCardinalLines abilityUtil_Targeter_SoldierCardinalLines = new AbilityUtil_Targeter_SoldierCardinalLines(this, this.GetPositionShape(), this.GetLineWidth(), this.PenetrateLos(), this.UseBothCardinalDir(), this.GetAoeDamage() > 0, this.GetAoeShape());
		abilityUtil_Targeter_SoldierCardinalLines.SetUseMultiTargetUpdate(true);
		abilityUtil_Targeter_SoldierCardinalLines.SetAffectedGroups(true, false, false);
		base.Targeters.Add(abilityUtil_Targeter_SoldierCardinalLines);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public override Ability.TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		if (targetIndex == 1)
		{
			return Ability.TargetingParadigm.Direction;
		}
		return base.GetControlpadTargetingParadigm(targetIndex);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			overridePos = AreaEffectUtils.GetCenterOfShape(this.GetPositionShape(), targetsSoFar[0]);
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public unsafe override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
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
		StandardEffectInfo cachedEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		this.m_cachedEnemyEffectOnSubsequentTurns = ((!this.m_abilityMod) ? this.m_enemyEffectOnSubsequentTurns : this.m_abilityMod.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(this.m_enemyEffectOnSubsequentTurns));
	}

	public bool UseBothCardinalDir()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_useBothCardinalDirMod.GetModifiedValue(this.m_useBothCardinalDir);
		}
		else
		{
			result = this.m_useBothCardinalDir;
		}
		return result;
	}

	public AbilityAreaShape GetPositionShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_positionShapeMod.GetModifiedValue(this.m_positionShape);
		}
		else
		{
			result = this.m_positionShape;
		}
		return result;
	}

	public float GetLineWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_lineWidthMod.GetModifiedValue(this.m_lineWidth);
		}
		else
		{
			result = this.m_lineWidth;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLos : this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return (this.m_cachedEnemyHitEffect == null) ? this.m_enemyHitEffect : this.m_cachedEnemyHitEffect;
	}

	public float GetNearCenterDistThreshold()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_nearCenterDistThresholdMod.GetModifiedValue(this.m_nearCenterDistThreshold);
		}
		else
		{
			result = this.m_nearCenterDistThreshold;
		}
		return result;
	}

	public int GetExtraDamageForNearCenterTargets()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageForNearCenterTargets : this.m_abilityMod.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(this.m_extraDamageForNearCenterTargets);
	}

	public AbilityAreaShape GetAoeShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoeShapeMod.GetModifiedValue(this.m_aoeShape);
		}
		else
		{
			result = this.m_aoeShape;
		}
		return result;
	}

	public int GetAoeDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoeDamageMod.GetModifiedValue(this.m_aoeDamage);
		}
		else
		{
			result = this.m_aoeDamage;
		}
		return result;
	}

	public int GetNumSubsequentTurns()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_numSubsequentTurnsMod.GetModifiedValue(this.m_numSubsequentTurns);
		}
		else
		{
			result = this.m_numSubsequentTurns;
		}
		return result;
	}

	public int GetDamageOnSubsequentTurns()
	{
		return (!this.m_abilityMod) ? this.m_damageOnSubsequentTurns : this.m_abilityMod.m_damageOnSubsequentTurnsMod.GetModifiedValue(this.m_damageOnSubsequentTurns);
	}

	public StandardEffectInfo GetEnemyEffectOnSubsequentTurns()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyEffectOnSubsequentTurns != null)
		{
			result = this.m_cachedEnemyEffectOnSubsequentTurns;
		}
		else
		{
			result = this.m_enemyEffectOnSubsequentTurns;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageAmount());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (currentTargeterIndex > 0)
		{
			if (currentTargeterIndex < base.Targeters.Count)
			{
				AbilityUtil_Targeter_SoldierCardinalLines abilityUtil_Targeter_SoldierCardinalLines = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SoldierCardinalLines;
				if (abilityUtil_Targeter_SoldierCardinalLines != null)
				{
					List<AbilityTooltipSubject> tooltipSubjectTypes = abilityUtil_Targeter_SoldierCardinalLines.GetTooltipSubjectTypes(targetActor);
					if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						int num = 0;
						if (abilityUtil_Targeter_SoldierCardinalLines.m_directHitActorToCenterDist.ContainsKey(targetActor))
						{
							num += this.GetDamageAmount();
							if (this.GetExtraDamageForNearCenterTargets() > 0)
							{
								if (abilityUtil_Targeter_SoldierCardinalLines.m_directHitActorToCenterDist[targetActor] <= this.GetNearCenterDistThreshold() * Board.Get().squareSize)
								{
									num += this.GetExtraDamageForNearCenterTargets();
								}
							}
						}
						if (abilityUtil_Targeter_SoldierCardinalLines.m_aoeHitActors.Contains(targetActor))
						{
							num += this.GetAoeDamage();
						}
						dictionary[AbilityTooltipSymbol.Damage] = num;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierCardinalLine abilityMod_SoldierCardinalLine = modAsBase as AbilityMod_SoldierCardinalLine;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SoldierCardinalLine)
		{
			val = abilityMod_SoldierCardinalLine.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SoldierCardinalLine) ? this.m_enemyHitEffect : abilityMod_SoldierCardinalLine.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect), "EnemyHitEffect", this.m_enemyHitEffect, true);
		base.AddTokenInt(tokens, "AoeDamage", string.Empty, (!abilityMod_SoldierCardinalLine) ? this.m_aoeDamage : abilityMod_SoldierCardinalLine.m_aoeDamageMod.GetModifiedValue(this.m_aoeDamage), false);
		string name2 = "ExtraDamageForNearCenterTargets";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SoldierCardinalLine)
		{
			val2 = abilityMod_SoldierCardinalLine.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(this.m_extraDamageForNearCenterTargets);
		}
		else
		{
			val2 = this.m_extraDamageForNearCenterTargets;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "NumSubsequentTurns";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_SoldierCardinalLine)
		{
			val3 = abilityMod_SoldierCardinalLine.m_numSubsequentTurnsMod.GetModifiedValue(this.m_numSubsequentTurns);
		}
		else
		{
			val3 = this.m_numSubsequentTurns;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "DamageOnSubsequentTurns";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_SoldierCardinalLine)
		{
			val4 = abilityMod_SoldierCardinalLine.m_damageOnSubsequentTurnsMod.GetModifiedValue(this.m_damageOnSubsequentTurns);
		}
		else
		{
			val4 = this.m_damageOnSubsequentTurns;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SoldierCardinalLine) ? this.m_enemyEffectOnSubsequentTurns : abilityMod_SoldierCardinalLine.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(this.m_enemyEffectOnSubsequentTurns), "EnemyEffectOnSubsequentTurns", this.m_enemyEffectOnSubsequentTurns, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierCardinalLine))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SoldierCardinalLine);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
