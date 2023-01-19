// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SenseiDash : Ability
{
	[Header("-- Targeting")]
	public bool m_canTargetAlly = true;
	public bool m_canTargetEnemy;
	public AbilityAreaShape m_chooseDestinationShape = AbilityAreaShape.Three_x_Three;
	[Header("-- On Hit")]
	public int m_damageAmount = 20;
	public int m_healAmount = 20;
	public StandardEffectInfo m_effectOnTargetEnemy;
	public StandardEffectInfo m_effectOnTargetAlly;
	[Header("-- If Hitting Targets In Between")]
	public bool m_hitActorsInBetween = true;
	public float m_chargeHitWidth = 1f;
	public float m_radiusAroundStartToHit;
	public float m_radiusAroundEndToHit = 1.5f;
	public bool m_chargeHitPenetrateLos;
	[Header("-- Sequences --")]
	public GameObject m_hitSequencePrefab;

	private StandardEffectInfo m_cachedEffectOnTargetEnemy;
	private StandardEffectInfo m_cachedEffectOnTargetAlly;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		ClearTargeters();
		if (ShouldHitActorsInBetween())
		{
			AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
				this,
				GetRadiusAroundStartToHit(),
				GetRadiusAroundEndToHit(),
				0.5f * GetChargeHitWidth(),
				-1,
				false,
				ChargeHitPenetrateLos());
			targeter.SetAffectedGroups(CanHitEnemy(), CanHitAlly(), false);
			targeter.m_shouldAddTargetDelegate = delegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability)
			{
				SenseiDash senseiDash = ability as SenseiDash;
				return senseiDash == null
				       || senseiDash.CanHitEnemy() && actorToConsider.GetTeam() != caster.GetTeam()
				       || senseiDash.CanHitAlly() && actorToConsider.GetTeam() == caster.GetTeam();
			};
			Targeter = targeter;
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Charge(
				this,
				AbilityAreaShape.SingleSquare,
				false,
				AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
				CanHitEnemy(),
				CanHitAlly());
		}
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnTargetEnemy = m_effectOnTargetEnemy;
		m_cachedEffectOnTargetAlly = m_effectOnTargetAlly;
	}

	public bool CanTargetAlly()
	{
		return m_canTargetAlly;
	}

	public bool CanTargetEnemy()
	{
		return m_canTargetEnemy;
	}

	public bool CanHitAlly()
	{
		return GetHealAmount() > 0 || GetEffectOnTargetAlly().m_applyEffect;
	}

	public bool CanHitEnemy()
	{
		return GetDamageAmount() > 0 || GetEffectOnTargetEnemy().m_applyEffect;
	}

	public AbilityAreaShape GetChooseDestinationShape()
	{
		return m_chooseDestinationShape;
	}

	public int GetDamageAmount()
	{
		return m_damageAmount;
	}

	public int GetHealAmount()
	{
		return m_healAmount;
	}

	public StandardEffectInfo GetEffectOnTargetEnemy()
	{
		return m_cachedEffectOnTargetEnemy ?? m_effectOnTargetEnemy;
	}

	public StandardEffectInfo GetEffectOnTargetAlly()
	{
		return m_cachedEffectOnTargetAlly ?? m_effectOnTargetAlly;
	}

	public bool ShouldHitActorsInBetween()
	{
		return m_hitActorsInBetween;
	}

	public float GetChargeHitWidth()
	{
		return m_chargeHitWidth;
	}

	public float GetRadiusAroundStartToHit()
	{
		return m_radiusAroundStartToHit;
	}

	public float GetRadiusAroundEndToHit()
	{
		return m_radiusAroundEndToHit;
	}

	public bool ChargeHitPenetrateLos()
	{
		return m_chargeHitPenetrateLos;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetEnemy, "EffectOnTargetEnemy", m_effectOnTargetEnemy);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetAlly, "EffectOnTargetAlly", m_effectOnTargetAlly);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealAmount());
		if (GetEffectOnTargetAlly() != null)
		{
			GetEffectOnTargetAlly().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmount();
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
		{
			dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount();
		}
		return dictionary;
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
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
		    || !targetSquare.IsValidForGameplay()
		    || targetSquare == caster.GetCurrentBoardSquare())
		{
			return false;
		}
		
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			GetChooseDestinationShape(),
			target,
			false,
			caster,
			TargeterUtils.GetRelevantTeams(caster, CanTargetAlly(), CanTargetEnemy()),
			null);
			
		bool isTargetValid = false;
		foreach (ActorData actor in actorsInShape)
		{
			bool canTargetActorInDecision = CanTargetActorInDecision(
				caster,
				actor,
				CanTargetEnemy(),
				CanTargetAlly(),
				false,
				ValidateCheckPath.Ignore,
				true,
				false);
			if (canTargetActorInDecision)
			{
				isTargetValid = true;
				break;
			}
		}
		
		bool canDashToTarget = false;
		if (targetSquare != null
		    && targetSquare.IsValidForGameplay()
		    && targetSquare != caster.GetCurrentBoardSquare())
		{
			canDashToTarget = KnockbackUtils.CanBuildStraightLineChargePath(
				caster, targetSquare, caster.GetCurrentBoardSquare(), false, out _);
		}
		
		return canDashToTarget && isTargetValid;
	}
	
#if SERVER
	// added in rogues
	private new List<ActorData> GetHitActors(
		List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(
			caster.GetSquareAtPhaseStart().ToVector3(),
			targetSquare.ToVector3(),
			GetRadiusAroundStartToHit(),
			GetRadiusAroundEndToHit(),
			0.5f * GetChargeHitWidth(),
			ChargeHitPenetrateLos(),
			caster,
			TargeterUtils.GetRelevantTeams(caster, CanHitAlly(), CanHitEnemy()),
			nonActorTargetInfo);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInRadiusOfLine);
		return actorsInRadiusOfLine;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_hitSequencePrefab,
				targets[0].FreePos + new Vector3(0f, 1f, 0f),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetChooseDestinationShape(), targets[0]);
		StandardEffectInfo effectOnTargetAlly = GetEffectOnTargetAlly();
		StandardEffectInfo effectOnTargetEnemy = GetEffectOnTargetEnemy();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in GetHitActors(targets, caster, nonActorTargetInfo))
		{
			if (actorData != caster)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
				if (actorData.GetTeam() == caster.GetTeam())
				{
					if (effectOnTargetAlly.m_applyEffect)
					{
						actorHitResults.AddStandardEffectInfo(effectOnTargetAlly);
					}
					actorHitResults.AddBaseHealing(GetHealAmount());
				}
				else if (actorData.GetTeam() != caster.GetTeam())
				{
					if (effectOnTargetEnemy.m_applyEffect)
					{
						actorHitResults.AddStandardEffectInfo(effectOnTargetEnemy);
					}
					actorHitResults.AddBaseDamage(GetDamageAmount());
				}
				abilityResults.StoreActorHit(actorHitResults);
			}
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}
#endif
}
