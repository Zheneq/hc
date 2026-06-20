// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class NinjaRewind : Ability
{
	[Header("-- What to set on Rewind --")]
	public bool m_setHealthIfGaining = true;
	public bool m_setHealthIfLosing = true;
	public bool m_setCooldowns = true;
	[Header("-- Whether can queue movement evade")]
	public bool m_canQueueMoveAfterEvade = true;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private Ninja_SyncComponent m_syncComp;
	
#if SERVER
	// added in rogues
	private Passive_Ninja m_passive;
	// added in rogues
	private AbilityData m_abilityData;
	// added in rogues
	private AbilityData.ActionType m_myActionType;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "NinjaRewind";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Ninja_SyncComponent>();
		}
#if SERVER
		// added in rogues
		if (m_passive == null)
		{
			PassiveData component = GetComponent<PassiveData>();
			if (component != null)
			{
				m_passive = component.GetPassiveOfType(typeof(Passive_Ninja)) as Passive_Ninja;
			}
		}
		if (m_abilityData == null)
		{
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_abilityData != null)
		{
			m_myActionType = m_abilityData.GetActionTypeOfAbility(this);
		}
#endif
		AbilityUtil_Targeter_Shape targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Always);
		targeter.SetShowArcToShape(false);
		Targeter = targeter;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return m_canQueueMoveAfterEvade;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComp != null
		       && m_syncComp.m_rewindHToHp > 0
		       && m_syncComp.GetSquareForRewind() != null;
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		if (m_syncComp != null)
		{
			BoardSquare squareForRewind = m_syncComp.GetSquareForRewind();
			if (squareForRewind != null)
			{
				return AbilityTarget.CreateAbilityTargetFromBoardSquare(squareForRewind, caster.GetFreePos());
			}
		}
		return base.CreateAbilityTargetForSimpleAction(caster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		ActorData actorData = ActorData;
		if (tooltipSubjectTypes != null
		    && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self)
		    && m_syncComp != null
		    && actorData != null)
		{
			int rewindHToHp = m_syncComp.m_rewindHToHp;
			int hitPoints = actorData.HitPoints;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>
			{
				[AbilityTooltipSymbol.Damage] = 0,
				[AbilityTooltipSymbol.Healing] = 0
			};
			if (hitPoints > rewindHToHp)
			{
				dictionary[AbilityTooltipSymbol.Damage] = hitPoints - rewindHToHp;
			}
			else if (rewindHToHp > hitPoints)
			{
				dictionary[AbilityTooltipSymbol.Healing] = rewindHToHp - hitPoints;
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
	
#if SERVER
	// added in rogues
	public override BoardSquare GetModifiedMoveStartSquare(ActorData caster, List<AbilityTarget> targets)
	{
		if (m_syncComp != null)
		{
			BoardSquare squareForRewind = m_syncComp.GetSquareForRewind();
			if (squareForRewind != null)
			{
				return squareForRewind;
			}
		}
		return base.GetModifiedMoveStartSquare(caster, targets);
	}

	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return true;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp != null)
		{
			BoardSquare squareForRewind = m_syncComp.GetSquareForRewind();
			if (squareForRewind != null)
			{
				ServerEvadeUtils.ChargeSegment[] array = new ServerEvadeUtils.ChargeSegment[2];
				array[0] = new ServerEvadeUtils.ChargeSegment
				{
					m_pos = caster.GetCurrentBoardSquare(),
					m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
					m_end = BoardSquarePathInfo.ChargeEndType.Impact
				};
				array[1] = new ServerEvadeUtils.ChargeSegment
				{
					m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
					m_pos = squareForRewind
				};
				float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(array));
				array[0].m_segmentMovementSpeed = segmentMovementSpeed;
				array[1].m_segmentMovementSpeed = segmentMovementSpeed;
				return array;
			}
		}
		Debug.LogError("Ninja rewind failed to find destination square");
		return base.GetChargePath(targets, caster, additionalData);
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				caster.GetSquareAtPhaseStart(),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()))
		{
			CanBeReactedTo = false
		};
		if (m_syncComp != null && m_syncComp.m_rewindHToHp > 0)
		{
			int hitPoints = caster.HitPoints;
			int rewindHToHp = m_syncComp.m_rewindHToHp;
			if (hitPoints > rewindHToHp && m_setHealthIfLosing)
			{
				actorHitResults.AddBaseDamage(hitPoints - rewindHToHp);
			}
			if (hitPoints < rewindHToHp && m_setHealthIfGaining)
			{
				actorHitResults.AddBaseHealing(rewindHToHp - hitPoints);
			}
			if (m_setCooldowns && m_passive != null)
			{
				for (int i = 1; i < 4; i++)
				{
					AbilityData.ActionType actionType = (AbilityData.ActionType)i;
					if (actionType == m_myActionType)
					{
						continue;
					}
					int remainingCooldownForRewind = m_passive.GetRemainingCooldownForRewind(i);
					Ability abilityOfActionType = m_abilityData.GetAbilityOfActionType(actionType);
					if (abilityOfActionType == null || abilityOfActionType.GetModdedCooldown() <= 0)
					{
						continue;
					}
					int cooldownRemaining = m_abilityData.GetCooldownRemaining(actionType);
					if ((cooldownRemaining > 0 && cooldownRemaining != remainingCooldownForRewind + 1)
					    || (cooldownRemaining == 0 && remainingCooldownForRewind > 0))
					{
						actorHitResults.AddMiscHitEvent(new MiscHitEventData_OverrideCooldown(actionType, remainingCooldownForRewind));
					}
				}
			}
		}
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
