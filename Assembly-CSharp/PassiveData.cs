// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveData : MonoBehaviour
{
	public Passive[] m_passives;
	public string m_toolTip;
	public string m_toolTipTitle = "Passive";

	// server- or rouges-only
#if SERVER
	private bool m_initialized;
#endif

	public Passive GetPassiveOfType(Type passiveType)
	{
		foreach (Passive passive in m_passives)
		{
			if (passive != null && passive.GetType() == passiveType)
			{
				return passive;
			}
		}
		return null;
	}

	public T GetPassiveOfType<T>() where T : Passive
	{
		foreach (Passive passive in m_passives)
		{
			if (passive != null && passive.GetType() == typeof(T))
			{
				return passive as T;
			}
		}
		return null;
	}

	// server-only below this line
#if SERVER
	private void Start()
	{
		Initialize();
	}

	public void Initialize()
	{
		if (!m_initialized)
		{
			ActorData component = GetComponent<ActorData>();
			foreach (Passive passive in m_passives)
			{
				passive.Owner = component;
				passive.Startup();
			}
			m_initialized = true;
		}
	}

	public void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnDamaged(damageCaster, damageSource, damageAmount);
		}
	}

	public void OnHealed(ActorData healCaster, DamageSource healSource, int healAmount)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnHealed(healCaster, healSource, healAmount);
		}
	}

	public void OnActorRespawn()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnActorRespawn();
		}
	}

	public void OnDied(List<UnresolvedHealthChange> killers)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnDied(killers);
		}
	}

	public void OnDamagedOther(ActorData damageTarget, DamageSource damageSource, int damageAmount)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnDamagedOther(damageTarget, damageSource, damageAmount);
		}
	}

	public void OnHealedOther(ActorData healTarget, DamageSource healSource, int healAmount)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnHealedOther(healTarget, healSource, healAmount);
		}
	}

	public void OnAbilitiesDone()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAbilitiesDone();
		}
	}

	public void OnMovementResultsGathered(MovementCollection stabilizedMovements)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnMovementResultsGathered(stabilizedMovements);
		}
	}

	public void OnMovementStart(BoardSquare startSquare, BoardSquarePathInfo gameplayPathRemaining, ActorData.MovementType movementType)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnMovementStart(startSquare, gameplayPathRemaining, movementType);
		}
	}

	public void AddInvalidEvadeDestinations(List<ServerEvadeUtils.EvadeInfo> evades, List<BoardSquare> invalidSquares)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].AddInvalidEvadeDestinations(evades, invalidSquares);
		}
	}

	public void AddInvalidKnockbackDestinations(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks, List<BoardSquare> invalidSquares)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].AddInvalidKnockbackDestinations(incomingKnockbacks, invalidSquares);
		}
	}

	public void OnServerLastKnownPosUpdateBegin()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnServerLastKnownPosUpdateBegin();
		}
	}

	public void OnServerLastKnownPosUpdateEnd()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnServerLastKnownPosUpdateEnd();
		}
	}

	public void OnGainingEffect(Effect effect)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnGainingEffect(effect);
		}
	}

	public void OnLosingEffect(Effect effect)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnLosingEffect(effect);
		}
	}

	public void OnAppliedEffectOnOther(Effect appliedEffect)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAppliedEffectOnOther(appliedEffect);
		}
	}

	public void OnTurnStart()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnTurnStart_Base();
		}
	}

	public void OnTurnEnd()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnTurnEnd_Base();
		}
	}

	public void PreGatherResultsForPlayerAction(Ability ability)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].PreGatherResultsForPlayerAction(ability);
		}
	}

	public void OnResolveStart()
	{
		ActorData component = GetComponent<ActorData>();
		bool hasAbilities = ServerActionBuffer.Get().HasPendingAbilityRequest(component, true);
		bool hasMovement = ServerActionBuffer.Get().HasPendingMovementRequest(component);
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnResolveStart(hasAbilities, hasMovement);
		}
	}

	public void OnAbilityQueued(Ability ability, bool queued)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAbilityQueued(ability, queued);
		}
	}

	public void OnAbilityCastResolved(Ability ability)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAbilityCastResolved(ability);
		}
	}

	public void OnAbilityPhaseStart(AbilityPriority phase)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAbilityPhaseStart(phase);
		}
	}

	public void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAbilityPhaseEnd(phase);
		}
	}

	public void OnEvadesProcessed()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnEvadesProcessed();
		}
	}

	public void OnBreakInvisibility()
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnBreakInvisibility();
		}
	}

	public int CalculateEnergyGainOnDamageFromPassives(int damage, ActorData caster, Ability sourceAbility, Effect sourceEffect)
	{
		int num = 0;
		foreach (Passive passive in m_passives)
		{
			num += passive.CalculateEnergyGainOnDamage(damage, caster, sourceAbility, sourceEffect);
		}
		return num;
	}

	public void OnAddToCooldownAttemptOnHit(AbilityData.ActionType actionType, int cooldownChangeDesired, int finalCooldown)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAddToCooldownAttemptOnHit(actionType, cooldownChangeDesired, finalCooldown);
		}
	}

	public void OnAccumulatedMovementDeniedByMe(float amount)
	{
		Passive[] passives = m_passives;
		for (int i = 0; i < passives.Length; i++)
		{
			passives[i].OnAccumulatedMovementDeniedByMe(amount);
		}
	}
#endif
}
