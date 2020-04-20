using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonkBuffCharge_Dash : Ability
{
	[Space(10f)]
	public AbilityAreaShape m_damageEnemiesShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public bool m_damageAoePenetratesLoS;

	public int m_damage = 0x14;

	public StandardEffectInfo m_enemyDebuff;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private BattleMonkBuffCharge_Prep m_prepAbility;

	private void Start()
	{
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			List<Ability> abilitiesAsList = component.GetAbilitiesAsList();
			using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ability ability = enumerator.Current;
					if (ability is BattleMonkBuffCharge_Prep)
					{
						this.m_prepAbility = (ability as BattleMonkBuffCharge_Prep);
					}
				}
			}
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (this.m_damage != 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damage));
		}
		this.m_enemyDebuff.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Enemy);
		return list;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	public int GetModdedDamage()
	{
		return (!(this.m_prepAbility == null)) ? this.m_prepAbility.GetModdedDamage() : this.m_damage;
	}

	public AbilityAreaShape GetEnemyHitShape()
	{
		AbilityAreaShape result;
		if (this.m_prepAbility == null)
		{
			result = this.m_damageEnemiesShape;
		}
		else
		{
			result = this.m_prepAbility.GetEnemyHitShape();
		}
		return result;
	}
}
