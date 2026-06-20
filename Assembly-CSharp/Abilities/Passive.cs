// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Passive : MonoBehaviour
{
	public string m_passiveName;
	protected ActorData m_owner;

	// added in rogues
#if SERVER
	private EffectSource m_effectSource;
#endif

	public GameObject m_sequencePrefab;
	public AbilityStatMod[] m_passiveStatMods;
	public StatusType[] m_passiveStatusChanges;
	public EmptyTurnData[] m_onEmptyTurn;
	public PassiveEventData[] m_eventResponses;
	public StandardActorEffectData[] m_effectsOnEveryTurn;

	public ActorData Owner
	{
		get
		{
			return m_owner;
		}
		set
		{
			m_owner = value;
		}
	}

	// server-only below this line
#if SERVER
	private void Awake()
	{
		m_effectSource = new EffectSource(this);
	}

	public virtual void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		OnEvent(PassiveEventType.TookDamage, damageCaster, damageSource.Ability);
	}

	public virtual void OnHealed(ActorData healCaster, DamageSource healSource, int healAmount)
	{
		OnEvent(PassiveEventType.TookHealing, healCaster, healSource.Ability);
	}

	public virtual void OnActorRespawn()
	{
	}

	public virtual void OnDied(List<UnresolvedHealthChange> killers)
	{
	}

	public virtual void OnDamagedOther(ActorData damageTarget, DamageSource damageSource, int damageAmount)
	{
		OnEvent(PassiveEventType.DidDamage, damageTarget, damageSource.Ability);
	}

	public virtual void OnHealedOther(ActorData healTarget, DamageSource healSource, int healAmount)
	{
		OnEvent(PassiveEventType.DidHealing, healTarget, healSource.Ability);
	}

	public virtual void OnAbilityQueued(Ability ability, bool queued)
	{
	}

	public virtual void OnAbilityCastResolved(Ability ability)
	{
		OnEvent(PassiveEventType.CastAbility, null, ability);
	}

	public virtual void OnAbilityPhaseStart(AbilityPriority phase)
	{
	}

	public virtual void OnAbilityPhaseEnd(AbilityPriority phase)
	{
	}

	public virtual void OnEvadesProcessed()
	{
	}

	public virtual void OnMovementStart(BoardSquare startSquare, BoardSquarePathInfo gameplayPathRemaining, ActorData.MovementType connectionType)
	{
	}

	public virtual void OnAbilitiesDone()
	{
	}

	public virtual void OnMovementResultsGathered(MovementCollection stabilizedMovements)
	{
	}

	public virtual void AddInvalidEvadeDestinations(List<ServerEvadeUtils.EvadeInfo> evades, List<BoardSquare> invalidSquares)
	{
	}

	public virtual void AddInvalidKnockbackDestinations(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks, List<BoardSquare> invalidSquares)
	{
	}

	public virtual void OnServerLastKnownPosUpdateBegin()
	{
	}

	public virtual void OnServerLastKnownPosUpdateEnd()
	{
	}

	public void OnTurnStart_Base()
	{
		if (!Owner.IsDead())
		{
			foreach (StandardActorEffectData data in m_effectsOnEveryTurn)
			{
				StandardActorEffect effect = new StandardActorEffect(AsEffectSource(), Owner.GetCurrentBoardSquare(), Owner, Owner, data);
				ServerEffectManager.Get().ApplyEffect(effect, 1);
			}
		}
		OnTurnStart();
	}

	public void OnTurnEnd_Base()
	{
		OnTurnEnd();
	}

	public virtual void PreGatherResultsForPlayerAction(Ability ability)
	{
	}

	public virtual void OnTurnStart()
	{
		OnEvent(PassiveEventType.TurnStart, null, null);
	}

	public virtual void OnTurnEnd()
	{
	}

	public virtual void OnGainingEffect(Effect effect)
	{
	}

	public virtual void OnLosingEffect(Effect effect)
	{
	}

	public virtual void OnAppliedEffectOnOther(Effect appliedEffect)
	{
	}

	protected virtual void OnStartup()
	{
	}

	public void Startup()
	{
		if (NetworkServer.active)
		{
			ActorStats actorStats = Owner.GetActorStats();
			foreach (AbilityStatMod statMod in m_passiveStatMods)
			{
				actorStats.AddStatMod(statMod);
			}
			ActorStatus actorStatus = Owner.GetActorStatus();
			foreach (StatusType status in m_passiveStatusChanges)
			{
				actorStatus.AddStatus(status, 0);
			}
		}
		OnStartup();
	}

	public virtual void OnResolveStart(bool hasAbilities, bool hasMovement)
	{
		foreach (EmptyTurnData emptyTurnData in m_onEmptyTurn)
		{
			bool flag;
			switch (emptyTurnData.m_triggerLevel)
			{
				case TurnEmptinessLevel.Empty:
					flag = (!hasAbilities && !hasMovement);
					break;
				case TurnEmptinessLevel.NoAbilities:
					flag = !hasAbilities;
					break;
				case TurnEmptinessLevel.NoMovement:
					flag = !hasMovement;
					break;
				case TurnEmptinessLevel.AbilitiesWithoutMovement:
					flag = (hasAbilities && !hasMovement);
					break;
				case TurnEmptinessLevel.MovementWithoutAbilities:
					flag = (!hasAbilities && hasMovement);
					break;
				case TurnEmptinessLevel.Full:
					flag = (hasAbilities && hasMovement);
					break;
				case TurnEmptinessLevel.Anything:
					flag = true;
					break;
				default:
					flag = false;
					break;
			}
			if (flag)
			{
				emptyTurnData.m_responseOnSelf.ApplyResponse(m_owner, this);
			}
		}
	}

	public virtual void OnBreakInvisibility()
	{
	}

	public virtual void OnEvent(PassiveEventType eventType, ActorData otherActor, Ability cause)
	{
		foreach (PassiveEventData passiveEventData in m_eventResponses)
		{
			if (passiveEventData.m_eventType == eventType)
			{
				bool flag;
				if (passiveEventData.m_causalAbilities.Length == 0)
				{
					flag = true;
				}
				else if (cause == null)
				{
					flag = false;
				}
				else
				{
					flag = false;
					foreach (Ability ability in passiveEventData.m_causalAbilities)
					{
						if (ability == cause)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					passiveEventData.m_responseOnSelf.ApplyResponse(this.m_owner, this);
					if (otherActor != null)
					{
						passiveEventData.m_responseOnOther.ApplyResponse(otherActor, this);
					}
				}
			}
		}
	}

	public virtual void OnMiscHitEventUpdate(List<MiscHitEventPassiveUpdateParams> updateParams)
	{
	}

	public virtual int CalculateEnergyGainOnDamage(int damage, ActorData caster, Ability sourceAbility, Effect sourceEffect)
	{
		return 0;
	}

	public virtual void OnAddToCooldownAttemptOnHit(AbilityData.ActionType actionType, int cooldownChangeDesired, int finalCooldown)
	{
	}

	public virtual void OnAccumulatedMovementDeniedByMe(float amount)
	{
	}

	public EffectSource AsEffectSource()
	{
		return m_effectSource;
	}
#endif
}
