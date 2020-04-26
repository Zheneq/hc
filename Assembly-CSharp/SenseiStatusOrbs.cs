using System.Collections.Generic;
using UnityEngine;

public class SenseiStatusOrbs : Ability
{
	[Header("-- Targeting --")]
	public bool m_canTargetAlly = true;

	public bool m_canTargetEnemy = true;

	public bool m_canTagetSelf;

	public bool m_targetingIgnoreLos;

	[Header("-- Effect data for on cast hit targets --")]
	public StandardActorEffectData m_enemyCastHitEffectData;

	public StandardActorEffectData m_allyCastHitEffectData;

	[Header("-- Combat phase hit params --")]
	public int m_numOrbs = 3;

	public float m_orbHitRadius = 3.5f;

	public bool m_orbHitIgnoreLos;

	[Header("    - if targeted Ally on cast -")]
	public int m_fromAllyDamageOnHit = 10;

	public int m_fromAllySelfHealPerHit = 2;

	public StandardEffectInfo m_fromAllyEnemyHitEffect;

	[Header("    - if targeted Enemy on cast -")]
	public int m_fromEnemyHealOnHit = 5;

	public int m_fromEnemySelfDamagePerHit = 1;

	public StandardEffectInfo m_fromEnemyAllyHitEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	[Header("    - for orb originating from an Ally towards Enemy")]
	public GameObject m_orbToAllySequencePrefab;

	[Header("    - for orb originating from an Enemy towards Ally")]
	public GameObject m_orbToEnemySequencePrefab;

	[Header("    - for hit on effect's target when orbs launch")]
	public GameObject m_hitOnEffectTargetSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiStatusOrbs";
		}
		Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_AoE_AroundActor abilityUtil_Targeter_AoE_AroundActor = new AbilityUtil_Targeter_AoE_AroundActor(this, m_orbHitRadius, m_orbHitIgnoreLos, true, true, m_numOrbs, m_canTargetEnemy, m_canTargetAlly, m_canTagetSelf);
		abilityUtil_Targeter_AoE_AroundActor.m_customShouldIncludeActorDelegate = ShouldAddActorForTargeter;
		abilityUtil_Targeter_AoE_AroundActor.m_allyOccupantSubject = AbilityTooltipSubject.Tertiary;
		abilityUtil_Targeter_AoE_AroundActor.m_enemyOccupantSubject = AbilityTooltipSubject.Tertiary;
		base.Targeter = abilityUtil_Targeter_AoE_AroundActor;
	}

	private bool ShouldAddActorForTargeter(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor)
	{
		if (potentialActor != null)
		{
			BoardSquare boardSquare = Board.Get().GetBoardSquare(centerPos);
			ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquare, true, true, targetingActor);
			if (targetableActorOnSquare != null)
			{
				if (targetableActorOnSquare.IsVisibleToClient())
				{
					if (potentialActor != targetableActorOnSquare)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return potentialActor.GetTeam() != targetableActorOnSquare.GetTeam();
							}
						}
					}
				}
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		results.m_healing = 0;
		results.m_damage = 0;
		ActorData actorData = base.ActorData;
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Tertiary) > 0)
		{
			if (actorData.GetTeam() == targetActor.GetTeam())
			{
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int num = results.m_healing = Mathf.Max(0, m_fromAllySelfHealPerHit * visibleActorsCountByTooltipSubject);
			}
			else
			{
				int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				visibleActorsCountByTooltipSubject2 += base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Self);
				int num2 = results.m_damage = Mathf.Max(0, m_fromEnemySelfDamagePerHit * visibleActorsCountByTooltipSubject2);
			}
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = m_fromAllyDamageOnHit;
		}
		else
		{
			results.m_healing = m_fromEnemyHealOnHit;
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, m_canTargetEnemy, m_canTargetAlly, m_canTagetSelf, ValidateCheckPath.Ignore, !m_targetingIgnoreLos, true);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, m_canTargetEnemy, m_canTargetAlly, m_canTagetSelf, ValidateCheckPath.Ignore, !m_targetingIgnoreLos, true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
}
