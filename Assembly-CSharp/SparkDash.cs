using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkDash : Ability
{
	[Header("-- Targeting")]
	public bool m_canTargetAny;

	private AbilityAreaShape m_targetShape;

	private bool m_targetShapePenetratesLoS;

	[Space(5f)]
	public bool m_chooseDestination = true;

	public AbilityAreaShape m_chooseDestinationShape = AbilityAreaShape.Three_x_Three;

	[Header("-- On Hit")]
	public bool m_applyTetherEffectToTarget;

	public StandardEffectInfo m_effectOnTargetEnemy;

	public StandardEffectInfo m_effectOnTargetAlly;

	[Space(5f)]
	public bool m_chaseTargetActor;

	[Header("-- whether to heal allies who dashed away")]
	public bool m_healAllyWhoDashedAway;

	[Header("-- If Hitting Targets In Between")]
	public bool m_hitActorsInBetween;

	public float m_chargeHitWidth = 1f;

	public bool m_chargeHitPenetrateLos;

	public StandardEffectInfo m_effectOnEnemyInBetween;

	public StandardEffectInfo m_effectOnAllyInBetween;

	[Header("-- Dash Sequences")]
	public GameObject m_dashToEnemySequence;

	public GameObject m_dashToFriendlySequence;

	private AbilityMod_SparkDash m_abilityMod;

	private SparkBeamTrackerComponent m_beamSyncComp;

	private SparkBasicAttack m_damageBeamAbility;

	private SparkHealingBeam m_healBeamAbility;

	private StandardEffectInfo m_cachedTargetEnemyEffect;

	private StandardEffectInfo m_cachedTargetAllyEffect;

	private StandardEffectInfo m_cachedInBetweenEnemyEffect;

	private StandardEffectInfo m_cachedInBetweenAllyEffect;

	private void Start()
	{
		SetupTargeter();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (ChooseDestinaton())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 2;
				}
			}
		}
		return 1;
	}

	private void SetupTargeter()
	{
		AbilityData component = GetComponent<AbilityData>();
		m_damageBeamAbility = (component.GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack);
		m_healBeamAbility = (component.GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam);
		if (m_beamSyncComp == null)
		{
			m_beamSyncComp = GetComponent<SparkBeamTrackerComponent>();
		}
		SetCachedFields();
		ClearTargeters();
		AbilityUtil_Targeter abilityUtil_Targeter = null;
		if (ShouldHitActorsInBetween())
		{
			AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0.5f * GetChargeWidth(), -1, false, m_chargeHitPenetrateLos);
			abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(true, true, false);
			if (ShouldHitActorsInBetween())
			{
				abilityUtil_Targeter_ChargeAoE.m_shouldAddTargetDelegate = TargeterAddActorInbetweenDelegate;
			}
			abilityUtil_Targeter = abilityUtil_Targeter_ChargeAoE;
		}
		else
		{
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, m_targetShape, m_targetShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true);
			abilityUtil_Targeter_Charge.m_affectCasterDelegate = TargeterAffectsCaster;
			abilityUtil_Targeter = abilityUtil_Targeter_Charge;
		}
		if (ChooseDestinaton())
		{
			base.Targeters.Add(abilityUtil_Targeter);
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge2 = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false);
			abilityUtil_Targeter_Charge2.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_Charge2);
		}
		else
		{
			base.Targeter = abilityUtil_Targeter;
		}
		ResetNameplateTargetingNumbers();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private bool TargeterAffectsCaster(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		bool result = false;
		bool flag = GetHealOnSelfForAllyHit() > 0;
		bool flag2 = GetHealOnSelfForEnemyHit() > 0;
		if (!flag)
		{
			if (!flag2)
			{
				goto IL_00d5;
			}
		}
		if (caster != null && actorsSoFar != null)
		{
			for (int i = 0; i < actorsSoFar.Count; i++)
			{
				if (flag)
				{
					if (actorsSoFar[i].GetTeam() == caster.GetTeam())
					{
						result = true;
						break;
					}
				}
				if (!flag2)
				{
					continue;
				}
				if (actorsSoFar[i].GetTeam() != caster.GetTeam())
				{
					result = true;
					break;
				}
			}
		}
		goto IL_00d5;
		IL_00d5:
		return result;
	}

	private bool TargeterAddActorInbetweenDelegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability)
	{
		bool result = false;
		SparkDash sparkDash = ability as SparkDash;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(abilityTarget.GridPos);
		if (sparkDash != null)
		{
			if (boardSquareSafe != null)
			{
				if (actorToConsider.GetCurrentBoardSquare() == boardSquareSafe)
				{
					result = true;
				}
				else
				{
					if (sparkDash.GetInBetweenEnemyEffect().m_applyEffect)
					{
						if (actorToConsider.GetTeam() != caster.GetTeam())
						{
							result = true;
						}
					}
					if (sparkDash.GetInBetweenAllyEffect().m_applyEffect)
					{
						if (actorToConsider.GetTeam() == caster.GetTeam())
						{
							result = true;
						}
					}
				}
				goto IL_00e0;
			}
		}
		result = true;
		goto IL_00e0;
		IL_00e0:
		return result;
	}

	public bool ChooseDestinaton()
	{
		bool num;
		if ((bool)m_abilityMod)
		{
			num = m_abilityMod.m_chooseDestinationMod.GetModifiedValue(m_chooseDestination);
		}
		else
		{
			num = m_chooseDestination;
		}
		int result;
		if (num)
		{
			result = ((m_targetData.Length > 1) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public AbilityAreaShape GetChooseDestShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_chooseDestShapeMod.GetModifiedValue(m_chooseDestinationShape);
		}
		else
		{
			result = m_chooseDestinationShape;
		}
		return result;
	}

	public bool ShouldChaseTarget()
	{
		return (!m_abilityMod) ? m_chaseTargetActor : m_abilityMod.m_chaseTargetActorMod.GetModifiedValue(m_chaseTargetActor);
	}

	public int GetDamage()
	{
		if (m_damageBeamAbility != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_damageBeamAbility.GetInitialDamage();
				}
			}
		}
		return 0;
	}

	public int GetHealOnAlly()
	{
		if (m_healBeamAbility != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_healBeamAbility.GetHealingOnAttach();
				}
			}
		}
		return 0;
	}

	public int GetHealOnSelfForAllyHit()
	{
		if (m_healBeamAbility != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_healBeamAbility.GetHealOnSelfPerTurn();
				}
			}
		}
		return 0;
	}

	public int GetHealOnSelfForEnemyHit()
	{
		if (m_damageBeamAbility != null)
		{
			return m_damageBeamAbility.GetHealOnCasterPerTurn();
		}
		return 0;
	}

	public bool ShouldHitActorsInBetween()
	{
		bool num;
		if ((bool)m_abilityMod)
		{
			num = m_abilityMod.m_hitActorsInBetweenMod.GetModifiedValue(m_hitActorsInBetween);
		}
		else
		{
			num = m_hitActorsInBetween;
		}
		bool flag = num;
		int num2;
		if (!GetInBetweenEnemyEffect().m_applyEffect)
		{
			num2 = (GetInBetweenAllyEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			num2 = 1;
		}
		bool flag2 = (byte)num2 != 0;
		float chargeWidth = GetChargeWidth();
		int result;
		if (flag && flag2)
		{
			result = ((chargeWidth > 0f) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public float GetChargeWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_chargeHitWidthMod.GetModifiedValue(m_chargeHitWidth);
		}
		else
		{
			result = m_chargeHitWidth;
		}
		return result;
	}

	public StandardEffectInfo GetTargetEnemyEffect()
	{
		return m_cachedTargetEnemyEffect;
	}

	public StandardEffectInfo GetTargetAllyEffect()
	{
		return m_cachedTargetAllyEffect;
	}

	public StandardEffectInfo GetInBetweenEnemyEffect()
	{
		return m_cachedInBetweenEnemyEffect;
	}

	public StandardEffectInfo GetInBetweenAllyEffect()
	{
		return m_cachedInBetweenAllyEffect;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetEnemyEffect;
		if ((bool)m_abilityMod)
		{
			cachedTargetEnemyEffect = m_abilityMod.m_effectOnEnemyMod.GetModifiedValue(m_effectOnTargetEnemy);
		}
		else
		{
			cachedTargetEnemyEffect = m_effectOnTargetEnemy;
		}
		m_cachedTargetEnemyEffect = cachedTargetEnemyEffect;
		StandardEffectInfo cachedTargetAllyEffect;
		if ((bool)m_abilityMod)
		{
			cachedTargetAllyEffect = m_abilityMod.m_effectOnAllyMod.GetModifiedValue(m_effectOnTargetAlly);
		}
		else
		{
			cachedTargetAllyEffect = m_effectOnTargetAlly;
		}
		m_cachedTargetAllyEffect = cachedTargetAllyEffect;
		m_cachedInBetweenEnemyEffect = ((!m_abilityMod) ? m_effectOnEnemyInBetween : m_abilityMod.m_effectOnEnemyInBetweenMod.GetModifiedValue(m_effectOnEnemyInBetween));
		StandardEffectInfo cachedInBetweenAllyEffect;
		if ((bool)m_abilityMod)
		{
			cachedInBetweenAllyEffect = m_abilityMod.m_effectOnAllyInBetweenMod.GetModifiedValue(m_effectOnAllyInBetween);
		}
		else
		{
			cachedInBetweenAllyEffect = m_effectOnAllyInBetween;
		}
		m_cachedInBetweenAllyEffect = cachedInBetweenAllyEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealOnAlly());
		if (GetTargetAllyEffect() != null)
		{
			GetTargetAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		Dictionary<AbilityTooltipSymbol, int> dictionary3;
		int value2;
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			BoardSquare boardSquareSafe = Board.Get().GetSquare(base.Targeter.LastUpdatingGridPos);
			bool flag = (bool)boardSquareSafe && boardSquareSafe == targetActor.GetCurrentBoardSquare();
			int age = 0;
			if (m_beamSyncComp != null)
			{
				age = m_beamSyncComp.GetTetherAgeOnActor(targetActor.ActorIndex);
			}
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int num = GetDamage();
				if (m_damageBeamAbility != null)
				{
					num += m_damageBeamAbility.GetBonusDamageFromTetherAge(age);
				}
				Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
				int value;
				if (flag)
				{
					value = num;
				}
				else
				{
					value = 0;
				}
				dictionary2[AbilityTooltipSymbol.Damage] = value;
			}
			else
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
				{
					int healOnAlly = GetHealOnAlly();
					bool flag2 = m_beamSyncComp.IsActorIndexTracked(targetActor.ActorIndex);
					dictionary3 = dictionary;
					if (flag)
					{
						if (!flag2)
						{
							value2 = healOnAlly;
							goto IL_014f;
						}
					}
					value2 = 0;
					goto IL_014f;
				}
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
				{
					int value3 = 0;
					if (visibleActorsCountByTooltipSubject > 0)
					{
						value3 = GetHealOnSelfForAllyHit();
					}
					else if (visibleActorsCountByTooltipSubject2 > 0)
					{
						value3 = GetHealOnSelfForEnemyHit();
					}
					dictionary[AbilityTooltipSymbol.Healing] = value3;
				}
			}
		}
		goto IL_019b;
		IL_019b:
		return dictionary;
		IL_014f:
		dictionary3[AbilityTooltipSymbol.Healing] = value2;
		goto IL_019b;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		TargetingParadigm targetingParadigm = GetTargetingParadigm(0);
		if (targetingParadigm != TargetingParadigm.BoardSquare)
		{
			if (targetingParadigm != TargetingParadigm.Position)
			{
				goto IL_0108;
			}
		}
		result = false;
		SparkBeamTrackerComponent component = caster.GetComponent<SparkBeamTrackerComponent>();
		List<ActorData> list = null;
		if (m_canTargetAny)
		{
			List<ActorData> actorsVisibleToActor;
			if (NetworkServer.active)
			{
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster);
			}
			else
			{
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
			}
			list = actorsVisibleToActor;
			list.Remove(caster);
		}
		else if (component.BeamIsActive())
		{
			list = component.GetBeamActors();
		}
		if (list != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							if (CanTargetActorInDecision(caster, current, true, true, false, ValidateCheckPath.CanBuildPath, true, false))
							{
								return true;
							}
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		goto IL_0108;
		IL_0108:
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		bool flag2 = false;
		if (targetIndex == 0)
		{
			List<Team> list = new List<Team>();
			list.Add(caster.GetEnemyTeam());
			list.Add(caster.GetTeam());
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_targetShape, target, m_targetShapePenetratesLoS, caster, list, null);
			SparkBeamTrackerComponent component = caster.GetComponent<SparkBeamTrackerComponent>();
			using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
			{
				while (true)
				{
					if (!enumerator.MoveNext())
					{
						break;
					}
					ActorData current = enumerator.Current;
					if (CanTargetActorInDecision(caster, current, true, true, false, ValidateCheckPath.CanBuildPath, true, false))
					{
						if (!m_canTargetAny)
						{
							if (!component.IsTrackingActor(current.ActorIndex))
							{
								continue;
							}
						}
						flag = true;
						flag2 = true;
						break;
					}
				}
			}
		}
		else
		{
			flag = true;
			BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquareSafe2 = Board.Get().GetSquare(target.GridPos);
			if (boardSquareSafe2 != null && boardSquareSafe2.IsBaselineHeight())
			{
				if (boardSquareSafe2 != boardSquareSafe && boardSquareSafe2 != caster.GetCurrentBoardSquare())
				{
					if (AreaEffectUtils.IsSquareInShape(boardSquareSafe2, GetChooseDestShape(), target.FreePos, boardSquareSafe, false, caster))
					{
						flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe2, boardSquareSafe, false, out int _);
					}
				}
			}
		}
		int result;
		if (flag2)
		{
			result = (flag ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkDash))
		{
			m_abilityMod = (abilityMod as AbilityMod_SparkDash);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
