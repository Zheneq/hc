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
		if (component != null)
		{
			foreach (Ability ability in component.GetAbilitiesAsList())
			{
				BattleMonkBuffCharge_Prep prep = ability as BattleMonkBuffCharge_Prep;
				if (!ReferenceEquals(prep, null))
				{
					m_prepAbility = prep;
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
		return m_prepAbility != null
			? m_prepAbility.GetModdedDamage()
			: m_damage;
	}

	public AbilityAreaShape GetEnemyHitShape()
	{
		return m_prepAbility != null
			? m_prepAbility.GetEnemyHitShape()
			: m_damageEnemiesShape;
	}
}
