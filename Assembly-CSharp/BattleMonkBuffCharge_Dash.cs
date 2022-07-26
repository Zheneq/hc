// ROGUES
// SERVER
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
				if (ability is BattleMonkBuffCharge_Prep prep)
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
	
#if SERVER
	// added in rogues
	private List<ActorData> FindHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> result = GetModdedDamage() > 0 || m_enemyDebuff.m_applyEffect
			? AreaEffectUtils.GetActorsInShape(
				GetEnemyHitShape(),
				targets[0],
				m_damageAoePenetratesLoS,
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo)
			: new List<ActorData>();
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref result);
		return result;
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> list = FindHitActors(targets, caster, null);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetEnemyHitShape(), targets[0]);
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, centerOfShape, list.ToArray(), caster, additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> list = FindHitActors(targets, caster, nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetEnemyHitShape(), targets[0]);
		foreach (ActorData target in list)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, centerOfShape);
			abilityResults.StoreActorHit(new ActorHitResults(GetModdedDamage(), HitActionType.Damage, m_enemyDebuff, hitParams));
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
