using System.Collections.Generic;
using UnityEngine;

public class ValkyriePullToLaserCenter : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 5f;
	public float m_laserRangeInSquares = 6.5f;
	public int m_maxTargets = 5;
	public bool m_lengthIgnoreLos = true;
	[Header("-- Damage & effects")]
	public int m_damage = 40;
	public StandardEffectInfo m_effectToEnemies;
	public int m_extraDamageForCenterHits;
	public float m_centerHitWidth = 0.1f;
	[Header("-- Knockback on Cast")]
	public float m_maxKnockbackDist = 3f;
	public KnockbackType m_knockbackType = KnockbackType.PerpendicularPullToAimDir;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ValkyriePullToLaserCenter m_abilityMod;
	private StandardEffectInfo m_cachedEffectToEnemies;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Valkyrie Pull Beam";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_KnockbackLaser(
			this,
			GetLaserWidth(),
			GetLaserRangeInSquares(),
			false,
			m_maxTargets,
			GetMaxKnockbackDist(),
			GetMaxKnockbackDist(),
			m_knockbackType,
			false)
		{
			LengthIgnoreWorldGeo = m_lengthIgnoreLos
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRangeInSquares();
	}

	private void SetCachedFields()
	{
		m_cachedEffectToEnemies = m_abilityMod != null
			? m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies)
			: m_effectToEnemies;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectToEnemies, "EffectToEnemies", m_effectToEnemies);
		AddTokenInt(tokens, "ExtraDamageForCenterHits", string.Empty, m_extraDamageForCenterHits);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int damage = GetDamage();
		int extraDamageIfKnockedInPlace = GetExtraDamageIfKnockedInPlace();
		if (extraDamageIfKnockedInPlace != 0 && !targetActor.GetActorStatus().IsMovementDebuffImmune())
		{
			foreach (AbilityUtil_Targeter.ActorTarget target in Targeter.GetActorsInRange())
			{
				if (target.m_actor != targetActor)
				{
					continue;
				}
				if (target.m_subjectTypes.Contains(AbilityTooltipSubject.HighHP))
				{
					damage += extraDamageIfKnockedInPlace;
				}
				break;
			}
		}
		int extraDamageForCenterHits = GetExtraDamageForCenterHits();
		AbilityUtil_Targeter_KnockbackLaser targeter = Targeter as AbilityUtil_Targeter_KnockbackLaser;
		if (extraDamageForCenterHits > 0 && targeter != null)
		{
			if (AreaEffectUtils.IsSquareInBoxByActorRadius(
				    targetActor.GetCurrentBoardSquare(),
				    ActorData.GetLoSCheckPos(),
				    targeter.GetLastLaserEndPos(),
				    GetCenterHitWidth()))
			{
				damage += extraDamageForCenterHits;
			}
		}
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		return dictionary;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetLaserRangeInSquares()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeInSquaresMod.GetModifiedValue(m_laserRangeInSquares)
			: m_laserRangeInSquares;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public bool LengthIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lengthIgnoreLosMod.GetModifiedValue(m_lengthIgnoreLos)
			: m_lengthIgnoreLos;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int GetExtraDamageIfKnockedInPlace()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfKnockedInPlaceMod.GetModifiedValue(0)
			: 0;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		return m_cachedEffectToEnemies ?? m_effectToEnemies;
	}

	public int GetExtraDamageForCenterHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForCenterHitsMod.GetModifiedValue(m_extraDamageForCenterHits)
			: m_extraDamageForCenterHits;
	}

	public float GetCenterHitWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_centerHitWidthMod.GetModifiedValue(m_centerHitWidth)
			: m_centerHitWidth;
	}

	public float GetMaxKnockbackDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist)
			: m_maxKnockbackDist;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType)
			: m_knockbackType;
	}

	public bool ShouldSkipDamageReductionOnNextTurnStab()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_nextTurnStabSkipsDamageReduction.GetModifiedValue(false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyriePullToLaserCenter))
		{
			m_abilityMod = abilityMod as AbilityMod_ValkyriePullToLaserCenter;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
