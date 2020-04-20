using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiYingYangDash : Ability
{
	[Separator("Custom colors for Ability Bar Icon", true)]
	public Color m_colorForAllyDash = new Color(0f, 1f, 1f);

	public Color m_colorForEnemyDash = new Color(1f, 0f, 1f);

	[Separator("Targeting Info", "cyan")]
	public bool m_chooseDestination;

	public AbilityAreaShape m_chooseDestShape = AbilityAreaShape.Three_x_Three;

	public bool m_useActorAtSquareBeforeEvadeIfMiss = true;

	[Separator("For Second Dash", "cyan")]
	public int m_secondCastTurns = 1;

	public bool m_secondDashAllowBothTeams;

	[Separator("On Enemy Hit", true)]
	public int m_damage = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	public int m_extraDamageForDiffTeamSecondDash;

	public int m_extraDamageForLowHealth;

	public float m_enemyLowHealthThresh;

	public bool m_reverseHealthThreshForEnemy;

	[Separator("On Ally Hit", true)]
	public int m_healOnAlly = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "SenseiYingYangDash";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_syncComp = base.GetComponent<Sensei_SyncComponent>();
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true);
		abilityUtil_Targeter_Charge.m_affectCasterDelegate = new AbilityUtil_Targeter_Shape.IsAffectingCasterDelegate(this.IncludeCasterInTargeter);
		if (this.ChooseDestinaton())
		{
			base.Targeters.Add(abilityUtil_Targeter_Charge);
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge2 = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false, false);
			abilityUtil_Targeter_Charge2.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_Charge2);
		}
		else
		{
			base.Targeter = abilityUtil_Targeter_Charge;
		}
	}

	private bool IncludeCasterInTargeter(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		StandardEffectInfo moddedEffectForSelf = base.GetModdedEffectForSelf();
		if (moddedEffectForSelf != null)
		{
			if (moddedEffectForSelf.m_applyEffect)
			{
				return true;
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public bool ChooseDestinaton()
	{
		bool chooseDestination = this.m_chooseDestination;
		bool result;
		if (chooseDestination)
		{
			result = (this.m_targetData.Length > 1);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (this.ChooseDestinaton())
		{
			result = 2;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		this.m_cachedEnemyHitEffect = ((!this.m_abilityMod) ? this.m_enemyHitEffect : this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect));
		StandardEffectInfo cachedAllyHitEffect;
		if (this.m_abilityMod)
		{
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
	}

	public AbilityAreaShape GetChooseDestShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_chooseDestShapeMod.GetModifiedValue(this.m_chooseDestShape);
		}
		else
		{
			result = this.m_chooseDestShape;
		}
		return result;
	}

	public int GetSecondCastTurns()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_secondCastTurnsMod.GetModifiedValue(this.m_secondCastTurns);
		}
		else
		{
			result = this.m_secondCastTurns;
		}
		return result;
	}

	public bool SecondDashAllowBothTeams()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_secondDashAllowBothTeamsMod.GetModifiedValue(this.m_secondDashAllowBothTeams);
		}
		else
		{
			result = this.m_secondDashAllowBothTeams;
		}
		return result;
	}

	public int GetDamage()
	{
		return (!this.m_abilityMod) ? this.m_damage : this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
		{
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public int GetExtraDamageForDiffTeamSecondDash()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageForDiffTeamSecondDashMod.GetModifiedValue(this.m_extraDamageForDiffTeamSecondDash);
		}
		else
		{
			result = this.m_extraDamageForDiffTeamSecondDash;
		}
		return result;
	}

	public int GetExtraDamageForLowHealth()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageForLowHealth : this.m_abilityMod.m_extraDamageForLowHealthMod.GetModifiedValue(this.m_extraDamageForLowHealth);
	}

	public float GetEnemyLowHealthThresh()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_enemyLowHealthThreshMod.GetModifiedValue(this.m_enemyLowHealthThresh);
		}
		else
		{
			result = this.m_enemyLowHealthThresh;
		}
		return result;
	}

	public bool ReverseHealthThreshForEnemy()
	{
		return (!this.m_abilityMod) ? this.m_reverseHealthThreshForEnemy : this.m_abilityMod.m_reverseHealthThreshForEnemyMod.GetModifiedValue(this.m_reverseHealthThreshForEnemy);
	}

	public int GetHealOnAlly()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healOnAllyMod.GetModifiedValue(this.m_healOnAlly);
		}
		else
		{
			result = this.m_healOnAlly;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
		{
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public int GetExtraHealOnAllyForDiffTeamSecondDash()
	{
		return (!this.m_abilityMod) ? this.m_extraHealOnAllyForDiffTeamSecondDash : this.m_abilityMod.m_extraHealOnAllyForDiffTeamSecondDashMod.GetModifiedValue(this.m_extraHealOnAllyForDiffTeamSecondDash);
	}

	public int GetExtraHealOnAllyForLowHealth()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraHealOnAllyForLowHealthMod.GetModifiedValue(this.m_extraHealOnAllyForLowHealth);
		}
		else
		{
			result = this.m_extraHealOnAllyForLowHealth;
		}
		return result;
	}

	public float GetAllyLowHealthThresh()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_allyLowHealthThreshMod.GetModifiedValue(this.m_allyLowHealthThresh);
		}
		else
		{
			result = this.m_allyLowHealthThresh;
		}
		return result;
	}

	public bool ReverseHealthThreshForAlly()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_reverseHealthThreshForAllyMod.GetModifiedValue(this.m_reverseHealthThreshForAlly);
		}
		else
		{
			result = this.m_reverseHealthThreshForAlly;
		}
		return result;
	}

	public int GetCdrIfNoSecondDash()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cdrIfNoSecondDashMod.GetModifiedValue(this.m_cdrIfNoSecondDash);
		}
		else
		{
			result = this.m_cdrIfNoSecondDash;
		}
		return result;
	}

	public int GetCurrentAllyHeal(ActorData allyActor, ActorData caster)
	{
		int num = this.GetHealOnAlly();
		if (allyActor != null)
		{
			bool flag = allyActor.GetHitPointShareOfMax() < this.GetAllyLowHealthThresh();
			if (this.ReverseHealthThreshForAlly())
			{
				flag = (allyActor.GetHitPointShareOfMax() > this.GetAllyLowHealthThresh());
			}
			if (this.GetExtraHealOnAllyForLowHealth() > 0 && this.GetAllyLowHealthThresh() > 0f)
			{
				if (flag)
				{
					num += this.GetExtraHealOnAllyForLowHealth();
				}
			}
			if (this.ShouldApplyBonusForDiffTeam(allyActor, caster) && this.GetExtraHealOnAllyForDiffTeamSecondDash() > 0)
			{
				num += this.GetExtraHealOnAllyForDiffTeamSecondDash();
			}
		}
		return num;
	}

	public int GetCurrentDamage(ActorData enemyActor, ActorData caster)
	{
		int num = this.GetDamage();
		if (enemyActor != null)
		{
			bool flag = enemyActor.GetHitPointShareOfMax() < this.GetEnemyLowHealthThresh();
			if (this.ReverseHealthThreshForEnemy())
			{
				flag = (enemyActor.GetHitPointShareOfMax() > this.GetEnemyLowHealthThresh());
			}
			if (this.GetExtraDamageForLowHealth() > 0)
			{
				if (this.GetEnemyLowHealthThresh() > 0f)
				{
					if (flag)
					{
						num += this.GetExtraDamageForLowHealth();
					}
				}
			}
			if (this.ShouldApplyBonusForDiffTeam(enemyActor, caster) && this.GetExtraDamageForDiffTeamSecondDash() > 0)
			{
				num += this.GetExtraDamageForDiffTeamSecondDash();
			}
		}
		return num;
	}

	public bool CanTargetEnemy()
	{
		if (this.m_syncComp != null)
		{
			bool flag = this.IsForSecondDash();
			int result;
			if (flag)
			{
				if (!this.SecondDashAllowBothTeams())
				{
					if (flag)
					{
						result = (this.m_syncComp.m_syncLastYingYangDashedToAlly ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return result != 0;
				}
			}
			result = 1;
			return result != 0;
		}
		return true;
	}

	public bool CanTargetAlly()
	{
		if (this.m_syncComp != null)
		{
			bool flag = this.IsForSecondDash();
			int result;
			if (flag)
			{
				if (!this.SecondDashAllowBothTeams())
				{
					if (flag)
					{
						result = ((!this.m_syncComp.m_syncLastYingYangDashedToAlly) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return result != 0;
				}
			}
			result = 1;
			return result != 0;
		}
		return true;
	}

	private bool IsForSecondDash()
	{
		return this.m_syncComp != null && (int)this.m_syncComp.m_syncTurnsForSecondYingYangDash > 0;
	}

	private bool ShouldApplyBonusForDiffTeam(ActorData targetActor, ActorData caster)
	{
		if (this.IsForSecondDash())
		{
			bool flag = targetActor.GetTeam() == caster.GetTeam();
			return this.m_syncComp.m_syncLastYingYangDashedToAlly != flag;
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_damage);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.m_healOnAlly);
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0)
		{
			results.m_healing = this.GetCurrentAllyHeal(targetActor, actorData);
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = this.GetCurrentDamage(targetActor, actorData);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "SecondCastTurns", string.Empty, this.m_secondCastTurns, false);
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
		base.AddTokenInt(tokens, "ExtraDamageForDiffTeamSecondDash", string.Empty, this.m_extraDamageForDiffTeamSecondDash, false);
		base.AddTokenInt(tokens, "ExtraDamageForLowHealth", string.Empty, this.m_extraDamageForLowHealth, false);
		base.AddTokenInt(tokens, "HealOnAlly", string.Empty, this.m_healOnAlly, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
		base.AddTokenInt(tokens, "ExtraHealOnAllyForDiffTeamSecondDash", string.Empty, this.m_extraHealOnAllyForDiffTeamSecondDash, false);
		base.AddTokenInt(tokens, "ExtraHealOnAllyForLowHealth", string.Empty, this.m_extraHealOnAllyForLowHealth, false);
		base.AddTokenInt(tokens, "CdrIfNoSecondDash", string.Empty, this.m_cdrIfNoSecondDash, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiYingYangDash))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SenseiYingYangDash);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, this.CanTargetEnemy(), this.CanTargetAlly(), false, Ability.ValidateCheckPath.CanBuildPath, true, false, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		bool flag2 = false;
		if (targetIndex == 0)
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe.OccupantActor != null)
				{
					if (base.CanTargetActorInDecision(caster, boardSquareSafe.OccupantActor, this.CanTargetEnemy(), this.CanTargetAlly(), false, Ability.ValidateCheckPath.CanBuildPath, true, false, false))
					{
						flag = true;
						flag2 = true;
					}
				}
			}
		}
		else
		{
			flag = true;
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquareSafe3 = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe3 != null && boardSquareSafe3.IsBaselineHeight())
			{
				if (boardSquareSafe3 != boardSquareSafe2)
				{
					if (boardSquareSafe3 != caster.GetCurrentBoardSquare())
					{
						if (AreaEffectUtils.IsSquareInShape(boardSquareSafe3, this.GetChooseDestShape(), target.FreePos, boardSquareSafe2, false, caster))
						{
							int num;
							flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe3, boardSquareSafe2, false, out num);
						}
					}
				}
			}
		}
		bool result;
		if (flag2)
		{
			result = flag;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override bool UseCustomAbilityIconColor()
	{
		bool result;
		if (this.m_syncComp != null)
		{
			result = ((int)this.m_syncComp.m_syncTurnsForSecondYingYangDash > 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (this.m_syncComp != null)
		{
			if ((int)this.m_syncComp.m_syncTurnsForSecondYingYangDash > 0)
			{
				bool flag = this.CanTargetAlly();
				bool flag2 = this.CanTargetEnemy();
				if (flag)
				{
					return this.m_colorForAllyDash;
				}
				if (flag2)
				{
					return this.m_colorForEnemyDash;
				}
			}
		}
		return Color.white;
	}
}
