using System;
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
	public int m_fromAllyDamageOnHit = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiStatusOrbs.Start()).MethodHandle;
			}
			this.m_abilityName = "SenseiStatusOrbs";
		}
		this.Setup();
	}

	private void Setup()
	{
		base.Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, this.m_orbHitRadius, this.m_orbHitIgnoreLos, true, true, this.m_numOrbs, this.m_canTargetEnemy, this.m_canTargetAlly, this.m_canTagetSelf)
		{
			m_customShouldIncludeActorDelegate = new AbilityUtil_Targeter_AoE_Smooth.ShouldIncludeActorDelegate(this.ShouldAddActorForTargeter),
			m_allyOccupantSubject = AbilityTooltipSubject.Tertiary,
			m_enemyOccupantSubject = AbilityTooltipSubject.Tertiary
		};
	}

	private bool ShouldAddActorForTargeter(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor)
	{
		if (potentialActor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiStatusOrbs.ShouldAddActorForTargeter(ActorData, Vector3, ActorData)).MethodHandle;
			}
			BoardSquare boardSquare = Board.Get().GetBoardSquare(centerPos);
			ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquare, true, true, targetingActor);
			if (targetableActorOnSquare != null)
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
				if (targetableActorOnSquare.IsVisibleToClient())
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
					if (potentialActor != targetableActorOnSquare)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						return potentialActor.GetTeam() != targetableActorOnSquare.GetTeam();
					}
				}
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, 1);
		return result;
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiStatusOrbs.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
				}
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int healing = Mathf.Max(0, this.m_fromAllySelfHealPerHit * visibleActorsCountByTooltipSubject);
				results.m_healing = healing;
			}
			else
			{
				int num = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				num += base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Self);
				int damage = Mathf.Max(0, this.m_fromEnemySelfDamagePerHit * num);
				results.m_damage = damage;
			}
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
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
			results.m_damage = this.m_fromAllyDamageOnHit;
		}
		else
		{
			results.m_healing = this.m_fromEnemyHealOnHit;
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.m_canTargetEnemy, this.m_canTargetAlly, this.m_canTagetSelf, Ability.ValidateCheckPath.Ignore, !this.m_targetingIgnoreLos, true, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, this.m_canTargetEnemy, this.m_canTargetAlly, this.m_canTagetSelf, Ability.ValidateCheckPath.Ignore, !this.m_targetingIgnoreLos, true, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
}
