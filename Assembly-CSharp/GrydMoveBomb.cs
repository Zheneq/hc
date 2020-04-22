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
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (4)
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
			m_abilityName = "Move Bomb";
		}
		m_placeBombAbility = (base.ActorData.GetAbilityData().GetAbilityOfType(typeof(GrydPlaceBomb)) as GrydPlaceBomb);
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeters.Clear();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, m_selectBombsThroughLoS);
		base.Targeters.Add(item);
		AbilityUtil_Targeter_Shape item2 = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, m_moveBombsThroughLoS);
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
