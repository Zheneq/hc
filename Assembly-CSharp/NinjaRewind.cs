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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "NinjaRewind";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComp == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_syncComp = GetComponent<Ninja_SyncComponent>();
		}
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
		abilityUtil_Targeter_Shape.SetShowArcToShape(false);
		base.Targeter = abilityUtil_Targeter_Shape;
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
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_syncComp.m_rewindHToHp > 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						BoardSquare squareForRewind = m_syncComp.GetSquareForRewind();
						return squareForRewind != null;
					}
					}
				}
			}
		}
		return false;
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			BoardSquare squareForRewind = m_syncComp.GetSquareForRewind();
			if (squareForRewind != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return AbilityTarget.CreateAbilityTargetFromBoardSquare(squareForRewind, caster.GetTravelBoardSquareWorldPosition());
					}
				}
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
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
		if (tooltipSubjectTypes != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self) && m_syncComp != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actorData != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					int rewindHToHp = m_syncComp.m_rewindHToHp;
					int hitPoints = actorData.HitPoints;
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					dictionary[AbilityTooltipSymbol.Damage] = 0;
					dictionary[AbilityTooltipSymbol.Healing] = 0;
					if (hitPoints > rewindHToHp)
					{
						dictionary[AbilityTooltipSymbol.Damage] = hitPoints - rewindHToHp;
					}
					else if (rewindHToHp > hitPoints)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						dictionary[AbilityTooltipSymbol.Healing] = rewindHToHp - hitPoints;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
}
