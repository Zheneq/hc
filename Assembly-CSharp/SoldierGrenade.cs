// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGrenade : Ability
{
	[Serializable]
	public class ShapeToDamage : ShapeToDataBase
	{
		public int m_damage;

		public ShapeToDamage(AbilityAreaShape shape, int damage)
		{
			m_shape = shape;
			m_damage = damage;
		}
	}

	[Header("-- Targeting --")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;
	public bool m_penetrateLos;
	[Header("-- On Hit Stuff --")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Space(10f)]
	public int m_allyHealAmount;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SoldierGrenade m_abilityMod;
	private AbilityData m_abilityData;
	private SoldierStimPack m_stimAbility;
	private List<ShapeToDamage> m_cachedShapeToDamage = new List<ShapeToDamage>();
	private List<AbilityAreaShape> m_shapes = new List<AbilityAreaShape>();
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Grenade";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_abilityData == null)
		{
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_stimAbility == null && m_abilityData != null)
		{
			m_stimAbility = m_abilityData.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack;
		}
		SetCachedFields();
		m_cachedShapeToDamage.Clear();
		m_cachedShapeToDamage.Add(new ShapeToDamage(GetShape(), GetDamageAmount()));
		if (m_abilityMod != null && m_abilityMod.m_useAdditionalShapeOverride)
		{
			foreach (ShapeToDamage shapeToDamage in m_abilityMod.m_additionalShapeToDamageOverride)
			{
				m_cachedShapeToDamage.Add(new ShapeToDamage(shapeToDamage.m_shape, shapeToDamage.m_damage));
			}
		}
		m_cachedShapeToDamage.Sort();
		m_shapes.Clear();
		foreach (ShapeToDamage shapeToDamage in m_cachedShapeToDamage)
		{
			m_shapes.Add(shapeToDamage.m_shape);
		}

		Targeter = new AbilityUtil_Targeter_MultipleShapes(
			this, 
			m_shapes, 
			new List<AbilityTooltipSubject> { AbilityTooltipSubject.Primary },
			PenetrateLos(),
			IncludeEnemies(),
			IncludeAllies());
	}

	public int GetDamageForShapeIndex(int shapeIndex)
	{
		if (m_cachedShapeToDamage != null && shapeIndex < m_cachedShapeToDamage.Count)
		{
			return m_cachedShapeToDamage[shapeIndex].m_damage;
		}
		return GetDamageAmount();
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	public AbilityAreaShape GetShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_shape)
			: m_shape;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetAllyHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public bool IncludeEnemies()
	{
		return GetDamageAmount() > 0 || GetEnemyHitEffect().m_applyEffect;
	}

	public bool IncludeAllies()
	{
		return GetAllyHealAmount() > 0 || GetAllyHitEffect().m_applyEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetAllyHealAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		ActorData actorData = ActorData;
		if (tooltipSubjectTypes != null && actorData != null)
		{
			Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
			List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = (Targeter as AbilityUtil_Targeter_MultipleShapes).GetHitActorContext();
			foreach (AbilityUtil_Targeter_MultipleShapes.HitActorContext item in hitActorContext)
			{
				if (item.m_actor == targetActor && targetActor.GetTeam() != actorData.GetTeam())
				{
					result[AbilityTooltipSymbol.Damage] = GetDamageForShapeIndex(item.m_hitShapeIndex);
					break;
				}
			}
			return result;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierGrenade abilityMod_SoldierGrenade = modAsBase as AbilityMod_SoldierGrenade;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_SoldierGrenade != null
			? abilityMod_SoldierGrenade.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierGrenade != null
			? abilityMod_SoldierGrenade.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AllyHealAmount", string.Empty, abilityMod_SoldierGrenade != null
			? abilityMod_SoldierGrenade.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierGrenade != null
			? abilityMod_SoldierGrenade.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
	}

	public override float GetRangeInSquares(int targetIndex)
	{
		float range = base.GetRangeInSquares(targetIndex);
		if (m_abilityData != null
		    && m_stimAbility != null
		    && m_stimAbility.GetGrenadeExtraRange() > 0f
		    && m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack))) // , true in rogues
		{
			range += m_stimAbility.GetGrenadeExtraRange();
		}
		return range;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierGrenade))
		{
			m_abilityMod = abilityMod as AbilityMod_SoldierGrenade;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				AreaEffectUtils.GetCenterOfShape(GetShape(), targets[0]),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetShape(), targets[0]);
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies());
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AreaEffectUtils.GetActorsInShapeLayers(
			m_shapes,
			targets[0].FreePos,
			Board.Get().GetSquare(targets[0].GridPos),
			PenetrateLos(),
			caster,
			relevantTeams,
			out List<List<ActorData>> actorsInLayers,
			nonActorTargetInfo);
		for (int i = 0; i < actorsInLayers.Count; i++)
		{
			foreach (ActorData actorData in actorsInLayers[i])
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
				if (actorData.GetTeam() != caster.GetTeam())
				{
					actorHitResults.SetBaseDamage(GetDamageForShapeIndex(i));
					actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
				}
				else
				{
					actorHitResults.SetBaseHealing(GetAllyHealAmount());
					actorHitResults.AddStandardEffectInfo(GetAllyHitEffect());
				}
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0 && caster.CurrentBoardSquare != null)
		{
			Vector3 origin = results.m_hitParameters.Origin;
			BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(origin);
			if (squareFromVec != null && !caster.CurrentBoardSquare.GetLOS(squareFromVec.x, squareFromVec.y))
			{
				caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SoldierStats.DamageFromGrenadesThrownOverWalls, results.FinalDamage);
			}
		}
	}
#endif
}
