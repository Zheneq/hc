// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class Passive_Sensei : Passive
{
	[Header("-- For Ammo/Orb Generation --")]
	public int m_maxOrbs = 10;
	public int m_orbRegenPerTurn = 1;
	public int m_orbPerEnemyHit;
	public int m_orbPerTurnIfHitEnemy;
	public bool m_gainOrbFromOrbAbility;
	
#if SERVER
	// added in rogues
	private Sensei_SyncComponent m_syncComp;
	private AbilityData m_abilityData;
	private SenseiBasicAttack m_basicAttackAbility;
	private SenseiYingYangDash m_dashAbility;
	private AbilityData.ActionType m_dashActionType = AbilityData.ActionType.INVALID_ACTION;
	private SenseiHealAoE m_healAoeAbility;
	private AbilityData.ActionType m_healAoeActionType = AbilityData.ActionType.INVALID_ACTION;
	private SenseiAppendStatus m_buffDebuffAbility;
	internal int m_bideAbsorbRemainingOnEnd;
	internal int m_bideAbsorbEndTurn;
	private bool m_dealtDamageThisTurn;
	private HashSet<AbilityData.ActionType> m_damagingActionTypes = new HashSet<AbilityData.ActionType>();
	private int m_numEnemyHitsThisTurn;
	internal int m_numOrbsUsedByAbility;
	internal int m_lastBasicAttackShieldOnTurnStart;
	private Dictionary<ActorData, List<StandardEffectInfo>> m_actorToDelayedEffectInfo = new Dictionary<ActorData, List<StandardEffectInfo>>();

	// added in rogues
	internal int DashCooldownAdjust { get; set; }

	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_syncComp = Owner.GetComponent<Sensei_SyncComponent>();
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_basicAttackAbility = m_abilityData.GetAbilityOfType(typeof(SenseiBasicAttack)) as SenseiBasicAttack;
			m_dashAbility = m_abilityData.GetAbilityOfType(typeof(SenseiYingYangDash)) as SenseiYingYangDash;
			m_dashActionType = m_abilityData.GetActionTypeOfAbility(m_dashAbility);
			m_healAoeAbility = m_abilityData.GetAbilityOfType(typeof(SenseiHealAoE)) as SenseiHealAoE;
			m_healAoeActionType = m_abilityData.GetActionTypeOfAbility(m_healAoeAbility);
			m_buffDebuffAbility = m_abilityData.GetAbilityOfType(typeof(SenseiAppendStatus)) as SenseiAppendStatus;
		}
		DashCooldownAdjust = 0;
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		HandleOnTurnStart_PrimaryShield();
		HandleOnTurnStart_OrbCount();
		HandleOnTurnStart_YingYangDash();
		HandleOnTurnStart_HealAoeAbility();
		HandleOnTurnStart_Bide();
		HandleOnTurnStart_AppendStatus();
		m_numEnemyHitsThisTurn = 0;
		m_numOrbsUsedByAbility = 0;
		m_dealtDamageThisTurn = false;
		m_damagingActionTypes.Clear();
	}

	// added in rogues
	private void HandleOnTurnStart_PrimaryShield()
	{
		if (!Owner.IsDead() && m_lastBasicAttackShieldOnTurnStart > 0 && m_basicAttackAbility != null)
		{
			StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
			standardActorEffectData.SetValues(
				"Sensei Basic Attack Shield",
				Mathf.Max(1, m_basicAttackAbility.m_absorbShieldDuration),
				0,
				0,
				0,
				ServerCombatManager.HealingType.Effect,
				0,
				m_lastBasicAttackShieldOnTurnStart,
				new AbilityStatMod[0],
				new StatusType[0],
				StandardActorEffectData.StatusDelayMode.DefaultBehavior);
			StandardActorEffect effect = new StandardActorEffect(
				AsEffectSource(),
				Owner.GetCurrentBoardSquare(),
				Owner,
				Owner,
				standardActorEffectData);
			ServerEffectManager.Get().ApplyEffect(effect);
		}
		m_lastBasicAttackShieldOnTurnStart = 0;
	}

	// added in rogues
	private void HandleOnTurnStart_OrbCount()
	{
		if (m_syncComp != null && !Owner.IsDead() && GameFlowData.Get().CurrentTurn > 1)
		{
			int num = 0;
			if (m_orbRegenPerTurn > 0)
			{
				num += m_orbRegenPerTurn;
			}
			if (m_orbPerEnemyHit > 0 && m_numEnemyHitsThisTurn > 0)
			{
				num += m_orbPerEnemyHit * m_numEnemyHitsThisTurn;
			}
			if (m_orbPerTurnIfHitEnemy > 0 && m_numEnemyHitsThisTurn > 0)
			{
				num += m_orbPerTurnIfHitEnemy;
			}
			int num2 = Mathf.Clamp(m_syncComp.m_syncCurrentNumOrbs + num - m_numOrbsUsedByAbility, 0, 127);
			if (m_maxOrbs > 0 && num2 > m_maxOrbs)
			{
				num2 = m_maxOrbs;
			}
			m_syncComp.Networkm_syncCurrentNumOrbs = (sbyte)num2;
		}
	}

	// added in rogues
	private void HandleOnTurnStart_YingYangDash()
	{
		if (m_syncComp != null
		    && m_syncComp.m_syncTurnsForSecondYingYangDash > 0
		    && m_dashAbility != null
		    && m_abilityData != null)
		{
			Sensei_SyncComponent syncComp = m_syncComp;
			syncComp.Networkm_syncTurnsForSecondYingYangDash = (sbyte)(syncComp.m_syncTurnsForSecondYingYangDash - 1);
			if (m_syncComp.m_syncTurnsForSecondYingYangDash == 0)
			{
				int cooldownRemainingOverride = Mathf.Max(
					0, 
					m_dashAbility.GetModdedCooldown()
					- m_dashAbility.GetSecondCastTurns()
					- m_dashAbility.GetCdrIfNoSecondDash()
					+ DashCooldownAdjust);
				m_abilityData.OverrideCooldown(m_dashActionType, cooldownRemainingOverride);
				DashCooldownAdjust = 0;
			}
		}
	}

	// added in rogues
	private void HandleOnTurnStart_HealAoeAbility()
	{
		if (m_abilityData != null && m_healAoeAbility != null)
		{
			int num = 0;
			if (m_dealtDamageThisTurn && m_healAoeAbility.GetCdrForAnyDamage() > 0)
			{
				num += m_healAoeAbility.GetCdrForAnyDamage();
			}
			if (m_damagingActionTypes.Count > 0 && m_healAoeAbility.GetCdrForDamagePerUniqueAbility() > 0)
			{
				num += m_damagingActionTypes.Count * m_healAoeAbility.GetCdrForDamagePerUniqueAbility();
			}
			if (m_abilityData.IsActionInCooldown(m_healAoeActionType))
			{
				int cd = m_abilityData.GetCooldownRemaining(m_healAoeActionType) - num;
				cd = Mathf.Max(0, cd);
				m_abilityData.OverrideCooldown(m_healAoeActionType, cd);
			}
		}
	}

	// added in rogues
	private void HandleOnTurnStart_Bide()
	{
		if (m_syncComp != null && ServerEffectManager.Get().GetAllActorEffectsByCaster(Owner, typeof(SenseiBideEffect)).Count == 0)
		{
			m_syncComp.Networkm_syncBideExtraDamagePct = 0f;
		}
		if (m_bideAbsorbEndTurn > 0 && m_bideAbsorbEndTurn + 2 <= GameFlowData.Get().CurrentTurn)
		{
			m_bideAbsorbRemainingOnEnd = 0;
		}
	}

	// added in rogues
	private void HandleOnTurnStart_AppendStatus()
	{
		foreach (ActorData actorData in m_actorToDelayedEffectInfo.Keys)
		{
			if (actorData != null && !actorData.IsDead())
			{
				List<StandardEffectInfo> list = m_actorToDelayedEffectInfo[actorData];
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
				foreach (StandardEffectInfo effectInfo in list)
				{
					actorHitResults.AddStandardEffectInfo(effectInfo);
				}
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(actorData, Owner, actorHitResults, m_buffDebuffAbility);
			}
		}
		m_actorToDelayedEffectInfo.Clear();
	}

	// added in rogues
	public override void OnDamagedOther(ActorData damageTarget, DamageSource damageSource, int damageAmount)
	{
		base.OnDamagedOther(damageTarget, damageSource, damageAmount);
		if (damageAmount > 0)
		{
			m_dealtDamageThisTurn = true;
			if (damageSource.Ability != null && m_abilityData != null)
			{
				AbilityData.ActionType actionTypeOfAbility = m_abilityData.GetActionTypeOfAbility(damageSource.Ability);
				if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION && !AbilityData.IsCard(actionTypeOfAbility))
				{
					m_damagingActionTypes.Add(actionTypeOfAbility);
				}
			}
			bool flag = false;
			if (damageSource.Ability != null)
			{
				bool flag2 = damageSource.Ability is SenseiAmmoLaser || damageSource.Ability is SenseiHomingOrbs;
				if (m_gainOrbFromOrbAbility || !flag2)
				{
					flag = true;
				}
			}
			if (flag)
			{
				m_numEnemyHitsThisTurn++;
			}
		}
	}

	// added in rogues
	public override void OnAddToCooldownAttemptOnHit(AbilityData.ActionType actionType, int cooldownChangeDesired, int finalCooldown)
	{
		if (actionType == m_dashActionType && cooldownChangeDesired < 0 && m_syncComp.m_syncTurnsForSecondYingYangDash > 0)
		{
			DashCooldownAdjust = cooldownChangeDesired; // TODO SENSEI Multiple reductions do not stack
		}
	}

	// added in rogues
	public override void OnMiscHitEventUpdate(List<MiscHitEventPassiveUpdateParams> updateParams)
	{
		for (int i = 0; i < updateParams.Count; i++)
		{
			MiscHitEventPassiveUpdateParams miscHitEventPassiveUpdateParams = updateParams[i];
			if (miscHitEventPassiveUpdateParams is AppendEffectParam)
			{
				AppendEffectParam appendEffectParam = miscHitEventPassiveUpdateParams as AppendEffectParam;
				if (!m_actorToDelayedEffectInfo.ContainsKey(appendEffectParam.m_targetActor))
				{
					m_actorToDelayedEffectInfo[appendEffectParam.m_targetActor] = new List<StandardEffectInfo>();
				}
				m_actorToDelayedEffectInfo[appendEffectParam.m_targetActor].Add(appendEffectParam.m_effectInfoToApply);
			}
		}
	}

	// added in rogues
	public class AppendEffectParam : MiscHitEventPassiveUpdateParams
	{
		public ActorData m_targetActor;
		public StandardEffectInfo m_effectInfoToApply;

		public AppendEffectParam(ActorData targetActor, StandardEffectInfo effectToApply)
		{
			m_targetActor = targetActor;
			m_effectInfoToApply = effectToApply;
		}
	}
#endif
}
