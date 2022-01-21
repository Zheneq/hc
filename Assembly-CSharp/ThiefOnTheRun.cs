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
		int numChargePiviots;
		if (m_targeterMultiStep)
		{
			numChargePiviots = Mathf.Max(GetNumTargets(), 1);
		}
		else
		{
			numChargePiviots = 1;
		}
		m_numChargePiviots = numChargePiviots;
		float dashRadius = GetDashRadius();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, dashRadius, dashRadius, dashRadius, -1, false, DashPenetrateLineOfSight());
			return;
		}
		ClearTargeters();
		int expectedNumberOfTargeters = GetExpectedNumberOfTargeters();
		for (int i = 0; i < expectedNumberOfTargeters; i++)
		{
			AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, dashRadius, dashRadius, dashRadius, -1, false, DashPenetrateLineOfSight());
			if (i < expectedNumberOfTargeters - 1)
			{
				abilityUtil_Targeter_ChargeAoE.UseEndPosAsDamageOriginIfOverlap = true;
			}
			base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
			base.Targeters[i].SetUseMultiTargetUpdate(true);
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
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
		return (GetMaxDistanceBetweenSteps() - 0.5f) * (float)m_numChargePiviots;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		m_cachedEffectOnSelfThroughSmokeField = ((!m_abilityMod) ? m_effectOnSelfThroughSmokeField : m_abilityMod.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(m_effectOnSelfThroughSmokeField));
		m_cachedSpoilSpawnInfo = ((!m_abilityMod) ? m_spoilSpawnInfo : m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(m_spoilSpawnInfo));
	}

	public float GetMinDistanceBetweenSteps()
	{
		return (!m_abilityMod) ? m_minDistanceBetweenSteps : m_abilityMod.m_minDistanceBetweenStepsMod.GetModifiedValue(m_minDistanceBetweenSteps);
	}

	public float GetMinDistanceBetweenAnySteps()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minDistanceBetweenAnyStepsMod.GetModifiedValue(m_minDistanceBetweenAnySteps);
		}
		else
		{
			result = m_minDistanceBetweenAnySteps;
		}
		return result;
	}

	public float GetMaxDistanceBetweenSteps()
	{
		return (!m_abilityMod) ? m_maxDistanceBetweenSteps : m_abilityMod.m_maxDistanceBetweenStepsMod.GetModifiedValue(m_maxDistanceBetweenSteps);
	}

	public float GetDashRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashRadiusMod.GetModifiedValue(m_dashRadius);
		}
		else
		{
			result = m_dashRadius;
		}
		return result;
	}

	public bool DashPenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(m_dashPenetrateLineOfSight);
		}
		else
		{
			result = m_dashPenetrateLineOfSight;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public int GetSubsequentDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_subsequentDamageMod.GetModifiedValue(m_subsequentDamage);
		}
		else
		{
			result = m_subsequentDamage;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfThroughSmokeField()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelfThroughSmokeField != null)
		{
			result = m_cachedEffectOnSelfThroughSmokeField;
		}
		else
		{
			result = m_effectOnSelfThroughSmokeField;
		}
		return result;
	}

	public int GetCooldownReductionIfNoEnemy()
	{
		return (!m_abilityMod) ? m_cooldownReductionIfNoEnemy : m_abilityMod.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(m_cooldownReductionIfNoEnemy);
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		SpoilsSpawnData result;
		if (m_cachedSpoilSpawnInfo != null)
		{
			result = m_cachedSpoilSpawnInfo;
		}
		else
		{
			result = m_spoilSpawnInfo;
		}
		return result;
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
		ActorData actorData = base.ActorData;
		if (actorData != null)
		{
			if (actorData.GetCurrentBoardSquare() != null)
			{
				for (int i = 0; i <= currentTargeterIndex; i++)
				{
					if (i >= base.Targeters.Count)
					{
						break;
					}
					BoardSquare y = (i <= 0) ? actorData.GetCurrentBoardSquare() : Board.Get().GetSquare(base.Targeters[i - 1].LastUpdatingGridPos);
					int subsequentAmount = GetSubsequentDamage();
					if (targetActor.GetCurrentBoardSquare() == y)
					{
						subsequentAmount = 0;
					}
					Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeters[i], targetActor, currentTargeterIndex, GetDamageAmount(), subsequentAmount);
				}
			}
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefOnTheRun abilityMod_ThiefOnTheRun = modAsBase as AbilityMod_ThiefOnTheRun;
		AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_ThiefOnTheRun) ? m_damageAmount : abilityMod_ThiefOnTheRun.m_damageAmountMod.GetModifiedValue(m_damageAmount));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ThiefOnTheRun)
		{
			val = abilityMod_ThiefOnTheRun.m_subsequentDamageMod.GetModifiedValue(m_subsequentDamage);
		}
		else
		{
			val = m_subsequentDamage;
		}
		AddTokenInt(tokens, "SubsequentDamage", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ThiefOnTheRun)
		{
			effectInfo = abilityMod_ThiefOnTheRun.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ThiefOnTheRun) ? m_effectOnSelfThroughSmokeField : abilityMod_ThiefOnTheRun.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(m_effectOnSelfThroughSmokeField), "EffectOnSelfThroughSmokeField", m_effectOnSelfThroughSmokeField);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ThiefOnTheRun)
		{
			val2 = abilityMod_ThiefOnTheRun.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(m_cooldownReductionIfNoEnemy);
		}
		else
		{
			val2 = m_cooldownReductionIfNoEnemy;
		}
		AddTokenInt(tokens, "CooldownReductionIfNoEnemy", empty2, val2);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe == null || !boardSquareSafe.IsValidForGameplay())
		{
			return false;
		}
		bool flag = true;
		if (GetExpectedNumberOfTargeters() < 2)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null;
				}
			}
		}
		BoardSquare boardSquare;
		if (targetIndex == 0)
		{
			boardSquare = caster.GetCurrentBoardSquare();
		}
		else
		{
			boardSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
		}
		bool flag2 = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, boardSquare, false) != null;
		float squareSize = Board.Get().squareSize;
		float num = Vector3.Distance(boardSquare.ToVector3(), boardSquareSafe.ToVector3());
		int num2;
		if (num >= GetMinDistanceBetweenSteps() * squareSize)
		{
			num2 = ((num <= GetMaxDistanceBetweenSteps() * squareSize) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag3 = (byte)num2 != 0;
		if (flag2)
		{
			if (flag3 && GetMinDistanceBetweenAnySteps() > 0f)
			{
				for (int i = 0; i < targetIndex; i++)
				{
					BoardSquare boardSquareSafe2 = Board.Get().GetSquare(currentTargets[i].GridPos);
					flag3 &= (Vector3.Distance(boardSquareSafe2.ToVector3(), boardSquareSafe.ToVector3()) >= GetMinDistanceBetweenAnySteps() * squareSize);
				}
			}
		}
		int result;
		if (flag2)
		{
			result = (flag3 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ThiefOnTheRun))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ThiefOnTheRun);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
