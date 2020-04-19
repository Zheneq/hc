using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiHomingOrbs : Ability
{
	[Header("-- Orb Targeting --")]
	public int m_numHomingOrbs = 4;

	public int m_maxOrbsPerVolley = 0x3E7;

	public float m_homingRadius = 3f;

	public bool m_canHitAllies = true;

	public bool m_canHitEnemies = true;

	[Header("-- Orb Hit Stuff --")]
	public int m_selfHealPerHit;

	public int m_allyHealAmount = 0x19;

	public int m_enemyDamageAmount = 0x19;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_enemyHitEffect;

	public int m_orbDuration = 4;

	[Header("-- Animation --")]
	public int m_orbLaunchAnimIndex = 0xB;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentOnCasterSequencePrefab;

	public GameObject m_orbSequence;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHomingOrbs.Start()).MethodHandle;
			}
			this.m_abilityName = "Sensei Homing Orbs";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetHomingRadius(), false, this.CanHitEnemies(), this.CanHitAllies(), Mathf.Min(this.GetNumHomingOrbs(), this.GetMaxOrbsPerVolley()));
		abilityUtil_Targeter_AoE_Smooth.SetAffectedGroups(this.CanHitEnemies(), this.CanHitAllies(), this.GetSelfHealPerHit() > 0);
		abilityUtil_Targeter_AoE_Smooth.m_affectCasterDelegate = new AbilityUtil_Targeter_AoE_Smooth.IsAffectingCasterDelegate(this.TargeterAddCasterDelegate);
		base.Targeter = abilityUtil_Targeter_AoE_Smooth;
		base.Targeter.ShowArcToShape = false;
	}

	private bool TargeterAddCasterDelegate(ActorData caster, List<ActorData> addedSoFar)
	{
		bool result;
		if (this.GetSelfHealPerHit() > 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHomingOrbs.TargeterAddCasterDelegate(ActorData, List<ActorData>)).MethodHandle;
			}
			result = (addedSoFar.Count > 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void SetCachedFields()
	{
		this.m_cachedAllyHitEffect = this.m_allyHitEffect;
		this.m_cachedEnemyHitEffect = this.m_enemyHitEffect;
	}

	public int GetNumHomingOrbs()
	{
		return this.m_numHomingOrbs;
	}

	public int GetMaxOrbsPerVolley()
	{
		return this.m_maxOrbsPerVolley;
	}

	public float GetHomingRadius()
	{
		return this.m_homingRadius;
	}

	public bool CanHitAllies()
	{
		return this.m_canHitAllies;
	}

	public bool CanHitEnemies()
	{
		return this.m_canHitEnemies;
	}

	public int GetSelfHealPerHit()
	{
		return this.m_selfHealPerHit;
	}

	public int GetAllyHealAmount()
	{
		return this.m_allyHealAmount;
	}

	public int GetEnemyDamageAmount()
	{
		return this.m_enemyDamageAmount;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHomingOrbs.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHomingOrbs.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public int GetOrbDuration()
	{
		return this.m_orbDuration;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetEnemyDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetAllyHealAmount());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSelfHealPerHit());
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (this.GetSelfHealPerHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHomingOrbs.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
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
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				results.m_healing = (visibleActorsCountByTooltipSubject + visibleActorsCountByTooltipSubject2) * this.GetSelfHealPerHit();
			}
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "NumHomingOrbs", string.Empty, this.m_numHomingOrbs, false);
		base.AddTokenInt(tokens, "SelfHealPerReactHit", string.Empty, this.m_selfHealPerHit, false);
		base.AddTokenInt(tokens, "AllyHealAmount", string.Empty, this.m_allyHealAmount, false);
		base.AddTokenInt(tokens, "EnemyDamageAmount", string.Empty, this.m_enemyDamageAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool result;
		if (animIndex != this.m_orbLaunchAnimIndex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHomingOrbs.CanTriggerAnimAtIndexForTaunt(int)).MethodHandle;
			}
			result = base.CanTriggerAnimAtIndexForTaunt(animIndex);
		}
		else
		{
			result = true;
		}
		return result;
	}
}
