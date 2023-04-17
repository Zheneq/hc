using System.Collections.Generic;
using UnityEngine;

public class ThiefOnTheRun : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiStep = true;
	public float m_minDistanceBetweenSteps = 2f;
	public float m_minDistanceBetweenAnySteps = -1f;
	public float m_maxDistanceBetweenSteps = 10f;
	[Header("-- Dash Hit Size")]
	public float m_dashRadius = 1f;
	public bool m_dashPenetrateLineOfSight;
	[Header("-- Hit Damage and Effect")]
	public int m_damageAmount;
	public int m_subsequentDamage;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Hid On Self")]
	public StandardEffectInfo m_effectOnSelfThroughSmokeField;
	public int m_cooldownReductionIfNoEnemy;
	public AbilityData.ActionType m_cooldownReductionOnAbility = AbilityData.ActionType.ABILITY_3;
	[Header("-- Spoil Powerup Spawn")]
	public SpoilsSpawnData m_spoilSpawnInfo;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private int m_numChargePiviots = 1;
	private AbilityMod_ThiefOnTheRun m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedEffectOnSelfThroughSmokeField;
	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "On the Run";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_numChargePiviots = m_targeterMultiStep
			? Mathf.Max(GetNumTargets(), 1)
			: 1;
		float dashRadius = GetDashRadius();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			Targeter = new AbilityUtil_Targeter_ChargeAoE(
				this,
				dashRadius,
				dashRadius,
				dashRadius,
				-1,
				false,
				DashPenetrateLineOfSight());
		}
		else
		{
			ClearTargeters();
			int expectedNumberOfTargeters = GetExpectedNumberOfTargeters();
			for (int i = 0; i < expectedNumberOfTargeters; i++)
			{
				AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
					this,
					dashRadius,
					dashRadius,
					dashRadius,
					-1,
					false,
					DashPenetrateLineOfSight());
				if (i < expectedNumberOfTargeters - 1)
				{
					targeter.UseEndPosAsDamageOriginIfOverlap = true;
				}
				Targeters.Add(targeter);
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_numChargePiviots;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return (GetMaxDistanceBetweenSteps() - 0.5f) * m_numChargePiviots;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedEffectOnSelfThroughSmokeField = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(m_effectOnSelfThroughSmokeField)
			: m_effectOnSelfThroughSmokeField;
		m_cachedSpoilSpawnInfo = m_abilityMod != null
			? m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(m_spoilSpawnInfo)
			: m_spoilSpawnInfo;
	}

	public float GetMinDistanceBetweenSteps()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistanceBetweenStepsMod.GetModifiedValue(m_minDistanceBetweenSteps)
			: m_minDistanceBetweenSteps;
	}

	public float GetMinDistanceBetweenAnySteps()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistanceBetweenAnyStepsMod.GetModifiedValue(m_minDistanceBetweenAnySteps)
			: m_minDistanceBetweenAnySteps;
	}

	public float GetMaxDistanceBetweenSteps()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDistanceBetweenStepsMod.GetModifiedValue(m_maxDistanceBetweenSteps)
			: m_maxDistanceBetweenSteps;
	}

	public float GetDashRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRadiusMod.GetModifiedValue(m_dashRadius)
			: m_dashRadius;
	}

	public bool DashPenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(m_dashPenetrateLineOfSight)
			: m_dashPenetrateLineOfSight;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetSubsequentDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subsequentDamageMod.GetModifiedValue(m_subsequentDamage)
			: m_subsequentDamage;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public StandardEffectInfo GetEffectOnSelfThroughSmokeField()
	{
		return m_cachedEffectOnSelfThroughSmokeField ?? m_effectOnSelfThroughSmokeField;
	}

	public int GetCooldownReductionIfNoEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(m_cooldownReductionIfNoEnemy)
			: m_cooldownReductionIfNoEnemy;
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		return m_cachedSpoilSpawnInfo ?? m_spoilSpawnInfo;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (ActorData != null && ActorData.GetCurrentBoardSquare() != null)
		{
			for (int i = 0; i <= currentTargeterIndex && i < Targeters.Count; i++)
			{
				BoardSquare startSquare = i > 0
					? Board.Get().GetSquare(Targeters[i - 1].LastUpdatingGridPos)
					: ActorData.GetCurrentBoardSquare();
				int subsequentAmount = GetSubsequentDamage();
				if (targetActor.GetCurrentBoardSquare() == startSquare)
				{
					subsequentAmount = 0;
				}
				AddNameplateValueForOverlap(
					ref symbolToValue,
					Targeters[i],
					targetActor,
					currentTargeterIndex,
					GetDamageAmount(),
					subsequentAmount);
			}
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefOnTheRun abilityMod_ThiefOnTheRun = modAsBase as AbilityMod_ThiefOnTheRun;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "SubsequentDamage", string.Empty, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_subsequentDamageMod.GetModifiedValue(m_subsequentDamage)
			: m_subsequentDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(m_effectOnSelfThroughSmokeField)
			: m_effectOnSelfThroughSmokeField, "EffectOnSelfThroughSmokeField", m_effectOnSelfThroughSmokeField);
		AddTokenInt(tokens, "CooldownReductionIfNoEnemy", string.Empty, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(m_cooldownReductionIfNoEnemy)
			: m_cooldownReductionIfNoEnemy);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null || !targetSquare.IsValidForGameplay())
		{
			return false;
		}
		if (GetExpectedNumberOfTargeters() < 2)
		{
			return KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
		}
		BoardSquare startSquare = targetIndex == 0
			? caster.GetCurrentBoardSquare()
			: Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
		bool canCharge = KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare, startSquare, false) != null;
		float stepDist = Vector3.Distance(startSquare.ToVector3(), targetSquare.ToVector3());
		bool isValidStepDist =
			stepDist >= GetMinDistanceBetweenSteps() * Board.Get().squareSize
			&& stepDist <= GetMaxDistanceBetweenSteps() * Board.Get().squareSize;
		if (canCharge
		    && isValidStepDist
		    && GetMinDistanceBetweenAnySteps() > 0f)
		{
			for (int i = 0; i < targetIndex; i++)
			{
				BoardSquare visitedSquare = Board.Get().GetSquare(currentTargets[i].GridPos);
				float distToVisitedSquare = Vector3.Distance(visitedSquare.ToVector3(), targetSquare.ToVector3());
				isValidStepDist &= distToVisitedSquare >= GetMinDistanceBetweenAnySteps() * Board.Get().squareSize;
			}
		}

		return canCharge && isValidStepDist;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefOnTheRun))
		{
			m_abilityMod = abilityMod as AbilityMod_ThiefOnTheRun;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
