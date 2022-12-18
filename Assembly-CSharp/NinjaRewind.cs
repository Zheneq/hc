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
}
