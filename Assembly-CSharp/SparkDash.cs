using System;
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
		this.SetupTargeter();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (this.ChooseDestinaton())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			return 2;
		}
		return 1;
	}

	private void SetupTargeter()
	{
		AbilityData component = base.GetComponent<AbilityData>();
		this.m_damageBeamAbility = (component.GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack);
		this.m_healBeamAbility = (component.GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam);
		if (this.m_beamSyncComp == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.SetupTargeter()).MethodHandle;
			}
			this.m_beamSyncComp = base.GetComponent<SparkBeamTrackerComponent>();
		}
		this.SetCachedFields();
		base.ClearTargeters();
		AbilityUtil_Targeter abilityUtil_Targeter;
		if (this.ShouldHitActorsInBetween())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0.5f * this.GetChargeWidth(), -1, false, this.m_chargeHitPenetrateLos);
			abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(true, true, false);
			if (this.ShouldHitActorsInBetween())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				abilityUtil_Targeter_ChargeAoE.m_shouldAddTargetDelegate = new AbilityUtil_Targeter_ChargeAoE.ShouldAddActorDelegate(this.TargeterAddActorInbetweenDelegate);
			}
			abilityUtil_Targeter = abilityUtil_Targeter_ChargeAoE;
		}
		else
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_Charge(this, this.m_targetShape, this.m_targetShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true)
			{
				m_affectCasterDelegate = new AbilityUtil_Targeter_Shape.IsAffectingCasterDelegate(this.TargeterAffectsCaster)
			};
		}
		if (this.ChooseDestinaton())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			base.Targeters.Add(abilityUtil_Targeter);
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false, false);
			abilityUtil_Targeter_Charge.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_Charge);
		}
		else
		{
			base.Targeter = abilityUtil_Targeter;
		}
		base.ResetNameplateTargetingNumbers();
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
		bool flag = this.GetHealOnSelfForAllyHit() > 0;
		bool flag2 = this.GetHealOnSelfForEnemyHit() > 0;
		if (!flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.TargeterAffectsCaster(ActorData, List<ActorData>, bool)).MethodHandle;
			}
			if (!flag2)
			{
				return result;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (caster != null && actorsSoFar != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int i = 0; i < actorsSoFar.Count; i++)
			{
				if (flag)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorsSoFar[i].GetTeam() == caster.GetTeam())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						result = true;
						break;
					}
				}
				if (flag2)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorsSoFar[i].GetTeam() != caster.GetTeam())
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	private bool TargeterAddActorInbetweenDelegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability)
	{
		bool result = false;
		SparkDash sparkDash = ability as SparkDash;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityTarget.GridPos);
		if (sparkDash != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.TargeterAddActorInbetweenDelegate(ActorData, AbilityTarget, List<ActorData>, ActorData, Ability)).MethodHandle;
			}
			if (boardSquareSafe != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actorToConsider.GetCurrentBoardSquare() == boardSquareSafe)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
				}
				else
				{
					if (sparkDash.GetInBetweenEnemyEffect().m_applyEffect)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorToConsider.GetTeam() != caster.GetTeam())
						{
							result = true;
						}
					}
					if (sparkDash.GetInBetweenAllyEffect().m_applyEffect)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorToConsider.GetTeam() == caster.GetTeam())
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							result = true;
						}
					}
				}
				return result;
			}
		}
		result = true;
		return result;
	}

	public bool ChooseDestinaton()
	{
		bool flag;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.ChooseDestinaton()).MethodHandle;
			}
			flag = this.m_abilityMod.m_chooseDestinationMod.GetModifiedValue(this.m_chooseDestination);
		}
		else
		{
			flag = this.m_chooseDestination;
		}
		bool flag2 = flag;
		bool result;
		if (flag2)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (this.m_targetData.Length > 1);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public AbilityAreaShape GetChooseDestShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.GetChooseDestShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_chooseDestShapeMod.GetModifiedValue(this.m_chooseDestinationShape);
		}
		else
		{
			result = this.m_chooseDestinationShape;
		}
		return result;
	}

	public bool ShouldChaseTarget()
	{
		return (!this.m_abilityMod) ? this.m_chaseTargetActor : this.m_abilityMod.m_chaseTargetActorMod.GetModifiedValue(this.m_chaseTargetActor);
	}

	public int GetDamage()
	{
		if (this.m_damageBeamAbility != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.GetDamage()).MethodHandle;
			}
			return this.m_damageBeamAbility.GetInitialDamage();
		}
		return 0;
	}

	public int GetHealOnAlly()
	{
		if (this.m_healBeamAbility != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.GetHealOnAlly()).MethodHandle;
			}
			return this.m_healBeamAbility.GetHealingOnAttach();
		}
		return 0;
	}

	public int GetHealOnSelfForAllyHit()
	{
		if (this.m_healBeamAbility != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.GetHealOnSelfForAllyHit()).MethodHandle;
			}
			return this.m_healBeamAbility.GetHealOnSelfPerTurn();
		}
		return 0;
	}

	public int GetHealOnSelfForEnemyHit()
	{
		if (this.m_damageBeamAbility != null)
		{
			return this.m_damageBeamAbility.GetHealOnCasterPerTurn();
		}
		return 0;
	}

	public bool ShouldHitActorsInBetween()
	{
		bool flag;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.ShouldHitActorsInBetween()).MethodHandle;
			}
			flag = this.m_abilityMod.m_hitActorsInBetweenMod.GetModifiedValue(this.m_hitActorsInBetween);
		}
		else
		{
			flag = this.m_hitActorsInBetween;
		}
		bool flag2 = flag;
		bool flag3;
		if (!this.GetInBetweenEnemyEffect().m_applyEffect)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			flag3 = this.GetInBetweenAllyEffect().m_applyEffect;
		}
		else
		{
			flag3 = true;
		}
		bool flag4 = flag3;
		float chargeWidth = this.GetChargeWidth();
		bool result;
		if (flag2 && flag4)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (chargeWidth > 0f);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public float GetChargeWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.GetChargeWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_chargeHitWidthMod.GetModifiedValue(this.m_chargeHitWidth);
		}
		else
		{
			result = this.m_chargeHitWidth;
		}
		return result;
	}

	public StandardEffectInfo GetTargetEnemyEffect()
	{
		return this.m_cachedTargetEnemyEffect;
	}

	public StandardEffectInfo GetTargetAllyEffect()
	{
		return this.m_cachedTargetAllyEffect;
	}

	public StandardEffectInfo GetInBetweenEnemyEffect()
	{
		return this.m_cachedInBetweenEnemyEffect;
	}

	public StandardEffectInfo GetInBetweenAllyEffect()
	{
		return this.m_cachedInBetweenAllyEffect;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetEnemyEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.SetCachedFields()).MethodHandle;
			}
			cachedTargetEnemyEffect = this.m_abilityMod.m_effectOnEnemyMod.GetModifiedValue(this.m_effectOnTargetEnemy);
		}
		else
		{
			cachedTargetEnemyEffect = this.m_effectOnTargetEnemy;
		}
		this.m_cachedTargetEnemyEffect = cachedTargetEnemyEffect;
		StandardEffectInfo cachedTargetAllyEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedTargetAllyEffect = this.m_abilityMod.m_effectOnAllyMod.GetModifiedValue(this.m_effectOnTargetAlly);
		}
		else
		{
			cachedTargetAllyEffect = this.m_effectOnTargetAlly;
		}
		this.m_cachedTargetAllyEffect = cachedTargetAllyEffect;
		this.m_cachedInBetweenEnemyEffect = ((!this.m_abilityMod) ? this.m_effectOnEnemyInBetween : this.m_abilityMod.m_effectOnEnemyInBetweenMod.GetModifiedValue(this.m_effectOnEnemyInBetween));
		StandardEffectInfo cachedInBetweenAllyEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedInBetweenAllyEffect = this.m_abilityMod.m_effectOnAllyInBetweenMod.GetModifiedValue(this.m_effectOnAllyInBetween);
		}
		else
		{
			cachedInBetweenAllyEffect = this.m_effectOnAllyInBetween;
		}
		this.m_cachedInBetweenAllyEffect = cachedInBetweenAllyEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamage());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetHealOnAlly());
		if (this.GetTargetAllyEffect() != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.CalculateNameplateTargetingNumbers()).MethodHandle;
			}
			this.GetTargetAllyEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		}
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
			bool flag = boardSquareSafe && boardSquareSafe == targetActor.GetCurrentBoardSquare();
			int age = 0;
			if (this.m_beamSyncComp != null)
			{
				age = this.m_beamSyncComp.GetTetherAgeOnActor(targetActor.ActorIndex);
			}
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
				int num = this.GetDamage();
				if (this.m_damageBeamAbility != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num += this.m_damageBeamAbility.GetBonusDamageFromTetherAge(age);
				}
				Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
				AbilityTooltipSymbol key = AbilityTooltipSymbol.Damage;
				int value;
				if (flag)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					value = num;
				}
				else
				{
					value = 0;
				}
				dictionary2[key] = value;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				int healOnAlly = this.GetHealOnAlly();
				bool flag2 = this.m_beamSyncComp.IsActorIndexTracked(targetActor.ActorIndex);
				Dictionary<AbilityTooltipSymbol, int> dictionary3 = dictionary;
				AbilityTooltipSymbol key2 = AbilityTooltipSymbol.Healing;
				int value2;
				if (flag)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!flag2)
					{
						value2 = healOnAlly;
						goto IL_14F;
					}
				}
				value2 = 0;
				IL_14F:
				dictionary3[key2] = value2;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				int value3 = 0;
				if (visibleActorsCountByTooltipSubject > 0)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					value3 = this.GetHealOnSelfForAllyHit();
				}
				else if (visibleActorsCountByTooltipSubject2 > 0)
				{
					value3 = this.GetHealOnSelfForEnemyHit();
				}
				dictionary[AbilityTooltipSymbol.Healing] = value3;
			}
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		Ability.TargetingParadigm targetingParadigm = base.GetTargetingParadigm(0);
		if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
		{
			if (targetingParadigm != Ability.TargetingParadigm.Position)
			{
				return result;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.CustomCanCastValidation(ActorData)).MethodHandle;
			}
		}
		result = false;
		SparkBeamTrackerComponent component = caster.GetComponent<SparkBeamTrackerComponent>();
		List<ActorData> list = null;
		if (this.m_canTargetAny)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			List<ActorData> actorsVisibleToActor;
			if (NetworkServer.active)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster, true);
			}
			else
			{
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData, true);
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData targetActor = enumerator.Current;
					if (base.CanTargetActorInDecision(caster, targetActor, true, true, false, Ability.ValidateCheckPath.CanBuildPath, true, false, false))
					{
						return true;
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		bool flag2 = false;
		if (targetIndex == 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkDash.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			List<Team> list = new List<Team>();
			list.Add(caster.GetOpposingTeam());
			list.Add(caster.GetTeam());
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_targetShape, target, this.m_targetShapePenetratesLoS, caster, list, null);
			SparkBeamTrackerComponent component = caster.GetComponent<SparkBeamTrackerComponent>();
			using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (base.CanTargetActorInDecision(caster, actorData, true, true, false, Ability.ValidateCheckPath.CanBuildPath, true, false, false))
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_canTargetAny)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!component.IsTrackingActor(actorData.ActorIndex))
							{
								continue;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						flag = true;
						flag2 = true;
						goto IL_E5;
					}
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_E5:;
		}
		else
		{
			flag = true;
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe2 != null && boardSquareSafe2.IsBaselineHeight())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquareSafe2 != boardSquareSafe && boardSquareSafe2 != caster.GetCurrentBoardSquare())
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (AreaEffectUtils.IsSquareInShape(boardSquareSafe2, this.GetChooseDestShape(), target.FreePos, boardSquareSafe, false, caster))
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						int num;
						flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe2, boardSquareSafe, false, out num);
					}
				}
			}
		}
		bool result;
		if (flag2)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			result = flag;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkDash))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SparkDash);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
