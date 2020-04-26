using System.Collections.Generic;
using UnityEngine;

public class BattleMonkBuffCharge_Dash : Ability
{
	[Space(10f)]
	public AbilityAreaShape m_damageEnemiesShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public bool m_damageAoePenetratesLoS;

	public int m_damage = 20;

	public StandardEffectInfo m_enemyDebuff;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private BattleMonkBuffCharge_Prep m_prepAbility;

	private void Start()
	{
		AbilityData component = GetComponent<AbilityData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			List<Ability> abilitiesAsList = component.GetAbilitiesAsList();
			using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ability current = enumerator.Current;
					if (current is BattleMonkBuffCharge_Prep)
					{
						m_prepAbility = (current as BattleMonkBuffCharge_Prep);
					}
				}
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_damage != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damage));
		}
		m_enemyDebuff.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	public int GetModdedDamage()
	{
		return (!(m_prepAbility == null)) ? m_prepAbility.GetModdedDamage() : m_damage;
	}

	public AbilityAreaShape GetEnemyHitShape()
	{
		AbilityAreaShape result;
		if (m_prepAbility == null)
		{
			result = m_damageEnemiesShape;
		}
		else
		{
			result = m_prepAbility.GetEnemyHitShape();
		}
		return result;
	}
}
