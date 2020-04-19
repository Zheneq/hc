using System;
using System.Collections.Generic;
using UnityEngine;

public class GrydMoveBomb : Ability
{
	[Header("-- Enemy direct hit")]
	public bool m_explodeThisTurnOnDirectHit;

	[Header("-- Targeting")]
	public int m_moveRange = 4;

	public bool m_selectBombsThroughLoS = true;

	public bool m_moveBombsThroughLoS;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private GrydPlaceBomb m_placeBombAbility;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydMoveBomb.Start()).MethodHandle;
			}
			this.m_abilityName = "Move Bomb";
		}
		this.m_placeBombAbility = (base.ActorData.\u000E().GetAbilityOfType(typeof(GrydPlaceBomb)) as GrydPlaceBomb);
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeters.Clear();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, this.m_selectBombsThroughLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeters.Add(item);
		AbilityUtil_Targeter_Shape item2 = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, this.m_moveBombsThroughLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeters.Add(item2);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return true;
	}
}
