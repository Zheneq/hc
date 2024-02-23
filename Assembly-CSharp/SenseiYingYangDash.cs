using System.Collections.Generic;
using UnityEngine;

public class SenseiYingYangDash : Ability
{
	[Separator("Custom colors for Ability Bar Icon")]
	public Color m_colorForAllyDash = new Color(0f, 1f, 1f);
	public Color m_colorForEnemyDash = new Color(1f, 0f, 1f);
	[Separator("Targeting Info", "cyan")]
	public bool m_chooseDestination;
	public AbilityAreaShape m_chooseDestShape = AbilityAreaShape.Three_x_Three;
	public bool m_useActorAtSquareBeforeEvadeIfMiss = true;
	[Separator("For Second Dash", "cyan")]
	public int m_secondCastTurns = 1;
	public bool m_secondDashAllowBothTeams;
	[Separator("On Enemy Hit")]
	public int m_damage = 10;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_extraDamageForDiffTeamSecondDash;
	public int m_extraDamageForLowHealth;
	public float m_enemyLowHealthThresh;
	public bool m_reverseHealthThreshForEnemy;
	[Separator("On Ally Hit")]
	public int m_healOnAlly = 10;
	public StandardEffectInfo m_allyHitEffect;
	public int m_extraHealOnAllyForDiffTeamSecondDash;
	public int m_extraHealOnAllyForLowHealth;
	public float m_allyLowHealthThresh;
	public bool m_reverseHealthThreshForAlly;
	[Header("-- whether to heal allies who dashed away")]
	public bool m_healAllyWhoDashedAway;
	[Header("-- Cooldown reduction")]
	public int m_cdrIfNoSecondDash;
	[Header("-- Sequences --")]
	public GameObject m_castOnAllySequencePrefab;
	public GameObject m_castOnEnemySequencePrefab;

	private AbilityMod_SenseiYingYangDash m_abilityMod;
	private Sensei_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiYingYangDash";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComp = GetComponent<Sensei_SyncComponent>();
		AbilityUtil_Targeter_Charge targeter = new AbilityUtil_Targeter_Charge(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
			true,
			true)
			{
				m_affectCasterDelegate = IncludeCasterInTargeter
			};
		if (ChooseDestinaton())
		{
			Targeters.Add(targeter);
			AbilityUtil_Targeter_Charge targeter2 = new AbilityUtil_Targeter_Charge(
				this,
				AbilityAreaShape.SingleSquare,
				true,
				AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
				false);
			targeter2.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter2);
		}
		else
		{
			Targeter = targeter;
		}
	}

	private bool IncludeCasterInTargeter(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		return moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public bool ChooseDestinaton()
	{
		return m_chooseDestination && m_targetData.Length > 1;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return ChooseDestinaton() ? 2 : 1;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	public AbilityAreaShape GetChooseDestShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chooseDestShapeMod.GetModifiedValue(m_chooseDestShape)
			: m_chooseDestShape;
	}

	public int GetSecondCastTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondCastTurnsMod.GetModifiedValue(m_secondCastTurns)
			: m_secondCastTurns;
	}

	public bool SecondDashAllowBothTeams()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondDashAllowBothTeamsMod.GetModifiedValue(m_secondDashAllowBothTeams)
			: m_secondDashAllowBothTeams;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetExtraDamageForDiffTeamSecondDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForDiffTeamSecondDashMod.GetModifiedValue(m_extraDamageForDiffTeamSecondDash)
			: m_extraDamageForDiffTeamSecondDash;
	}

	public int GetExtraDamageForLowHealth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForLowHealthMod.GetModifiedValue(m_extraDamageForLowHealth)
			: m_extraDamageForLowHealth;
	}

	public float GetEnemyLowHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyLowHealthThreshMod.GetModifiedValue(m_enemyLowHealthThresh)
			: m_enemyLowHealthThresh;
	}

	public bool ReverseHealthThreshForEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reverseHealthThreshForEnemyMod.GetModifiedValue(m_reverseHealthThreshForEnemy)
			: m_reverseHealthThreshForEnemy;
	}

	public int GetHealOnAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnAllyMod.GetModifiedValue(m_healOnAlly)
			: m_healOnAlly;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public int GetExtraHealOnAllyForDiffTeamSecondDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealOnAllyForDiffTeamSecondDashMod.GetModifiedValue(m_extraHealOnAllyForDiffTeamSecondDash)
			: m_extraHealOnAllyForDiffTeamSecondDash;
	}

	public int GetExtraHealOnAllyForLowHealth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealOnAllyForLowHealthMod.GetModifiedValue(m_extraHealOnAllyForLowHealth)
			: m_extraHealOnAllyForLowHealth;
	}

	public float GetAllyLowHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyLowHealthThreshMod.GetModifiedValue(m_allyLowHealthThresh)
			: m_allyLowHealthThresh;
	}

	public bool ReverseHealthThreshForAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reverseHealthThreshForAllyMod.GetModifiedValue(m_reverseHealthThreshForAlly)
			: m_reverseHealthThreshForAlly;
	}

	public int GetCdrIfNoSecondDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfNoSecondDashMod.GetModifiedValue(m_cdrIfNoSecondDash)
			: m_cdrIfNoSecondDash;
	}

	public int GetCurrentAllyHeal(ActorData allyActor, ActorData caster)
	{
		int num = GetHealOnAlly();
		if (allyActor == null)
		{
			return num;
		}
		bool isAllyLowHealth = allyActor.GetHitPointPercent() < GetAllyLowHealthThresh();
		if (ReverseHealthThreshForAlly())
		{
			isAllyLowHealth = allyActor.GetHitPointPercent() > GetAllyLowHealthThresh();
		}
		if (GetExtraHealOnAllyForLowHealth() > 0 && GetAllyLowHealthThresh() > 0f && isAllyLowHealth)
		{
			num += GetExtraHealOnAllyForLowHealth();
		}
		if (ShouldApplyBonusForDiffTeam(allyActor, caster) && GetExtraHealOnAllyForDiffTeamSecondDash() > 0)
		{
			num += GetExtraHealOnAllyForDiffTeamSecondDash();
		}
		return num;
	}

	public int GetCurrentDamage(ActorData enemyActor, ActorData caster)
	{
		int num = GetDamage();
		if (enemyActor == null)
		{
			return num;
		}
		bool isEnemyLowHealth = enemyActor.GetHitPointPercent() < GetEnemyLowHealthThresh();
		if (ReverseHealthThreshForEnemy())
		{
			isEnemyLowHealth = enemyActor.GetHitPointPercent() > GetEnemyLowHealthThresh();
		}
		if (GetExtraDamageForLowHealth() > 0 && GetEnemyLowHealthThresh() > 0f && isEnemyLowHealth)
		{
			num += GetExtraDamageForLowHealth();
		}
		if (ShouldApplyBonusForDiffTeam(enemyActor, caster) && GetExtraDamageForDiffTeamSecondDash() > 0)
		{
			num += GetExtraDamageForDiffTeamSecondDash();
		}
		return num;
	}

	public bool CanTargetEnemy()
	{
		return m_syncComp == null
		       || !IsForSecondDash()
		       || SecondDashAllowBothTeams()
		       || m_syncComp.m_syncLastYingYangDashedToAlly;
	}

	public bool CanTargetAlly()
	{
		return m_syncComp == null
		       || !IsForSecondDash()
		       || SecondDashAllowBothTeams()
		       || !m_syncComp.m_syncLastYingYangDashedToAlly;
	}

	private bool IsForSecondDash()
	{
		return m_syncComp != null && m_syncComp.m_syncTurnsForSecondYingYangDash > 0;
	}

	private bool ShouldApplyBonusForDiffTeam(ActorData targetActor, ActorData caster)
	{
		if (IsForSecondDash())
		{
			bool isAlly = targetActor.GetTeam() == caster.GetTeam();
			return m_syncComp.m_syncLastYingYangDashedToAlly != isAlly;
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damage);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, m_healOnAlly);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0)
		{
			results.m_healing = GetCurrentAllyHeal(targetActor, ActorData);
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = GetCurrentDamage(targetActor, ActorData);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "SecondCastTurns", string.Empty, m_secondCastTurns);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "ExtraDamageForDiffTeamSecondDash", string.Empty, m_extraDamageForDiffTeamSecondDash);
		AddTokenInt(tokens, "ExtraDamageForLowHealth", string.Empty, m_extraDamageForLowHealth);
		AddTokenInt(tokens, "HealOnAlly", string.Empty, m_healOnAlly);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "ExtraHealOnAllyForDiffTeamSecondDash", string.Empty, m_extraHealOnAllyForDiffTeamSecondDash);
		AddTokenInt(tokens, "ExtraHealOnAllyForLowHealth", string.Empty, m_extraHealOnAllyForLowHealth);
		AddTokenInt(tokens, "CdrIfNoSecondDash", string.Empty, m_cdrIfNoSecondDash);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiYingYangDash))
		{
			m_abilityMod = abilityMod as AbilityMod_SenseiYingYangDash;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(
			caster,
			CanTargetEnemy(),
			CanTargetAlly(),
			false,
			ValidateCheckPath.CanBuildPath,
			true,
			false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool isTargetValid = false;
		bool isDashValid = false;
		if (targetIndex == 0)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			if (targetSquare != null && targetSquare.OccupantActor != null)
			{
				bool canTargetActorInDecision = CanTargetActorInDecision(
					caster,
					targetSquare.OccupantActor,
					CanTargetEnemy(),
					CanTargetAlly(),
					false,
					ValidateCheckPath.CanBuildPath,
					true,
					false);
				if (canTargetActorInDecision)
				{
					isTargetValid = true;
					isDashValid = true;
				}
			}
		}
		else
		{
			isTargetValid = true;
			BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			if (targetSquare != null
			    && targetSquare.IsValidForGameplay()
			    && targetSquare != prevTargetSquare
			    && targetSquare != caster.GetCurrentBoardSquare())
			{
				bool isSquareInShape = AreaEffectUtils.IsSquareInShape(
					targetSquare, GetChooseDestShape(), target.FreePos, prevTargetSquare, false, caster);
				if (isSquareInShape)
				{
					int foo;
					isDashValid = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, prevTargetSquare, false, out foo);
				}
			}
		}

		return isDashValid && isTargetValid;
	}

	public override bool UseCustomAbilityIconColor()
	{
		return m_syncComp != null && m_syncComp.m_syncTurnsForSecondYingYangDash > 0;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (m_syncComp != null && m_syncComp.m_syncTurnsForSecondYingYangDash > 0)
		{
			if (CanTargetAlly())
			{
				return m_colorForAllyDash;
			}
			if (CanTargetEnemy())
			{
				return m_colorForEnemyDash;
			}
		}
		return Color.white;
	}
}
