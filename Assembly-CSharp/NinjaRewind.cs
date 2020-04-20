using System;
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
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaRewind.Start()).MethodHandle;
			}
			this.m_abilityName = "NinjaRewind";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaRewind.Setup()).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Ninja_SyncComponent>();
		}
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		abilityUtil_Targeter_Shape.SetShowArcToShape(false);
		base.Targeter = abilityUtil_Targeter_Shape;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return this.m_canQueueMoveAfterEvade;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComp != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaRewind.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			if (this.m_syncComp.m_rewindHToHp > 0)
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
				BoardSquare squareForRewind = this.m_syncComp.GetSquareForRewind();
				return squareForRewind != null;
			}
		}
		return false;
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaRewind.CreateAbilityTargetForSimpleAction(ActorData)).MethodHandle;
			}
			BoardSquare squareForRewind = this.m_syncComp.GetSquareForRewind();
			if (squareForRewind != null)
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
				return AbilityTarget.CreateAbilityTargetFromBoardSquare(squareForRewind, caster.GetTravelBoardSquareWorldPosition());
			}
		}
		return base.CreateAbilityTargetForSimpleAction(caster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaRewind.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self) && this.m_syncComp != null)
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
				if (actorData != null)
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
					int rewindHToHp = (int)this.m_syncComp.m_rewindHToHp;
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
						for (;;)
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
