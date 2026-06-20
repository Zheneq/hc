using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Passive_Exo : Passive
{
	[Header("-- Anim")]
	public int m_anchoredIdleType = 1;
	public int m_unanchoredIdleType;
	[HideInInspector]
	public int m_laserLastCastTurn = -1;
	[HideInInspector]
	public Barrier m_persistingBarrierInstance;
	[HideInInspector]
	public int m_currentConsecutiveSweeps;

#if SERVER
	// added in rogues
	private AbilityData m_abilityData;
	private ExoAnchorLaser m_anchorLaserAbility;
	private ExoTetherTrap m_tetherAbility;
	private AbilityData.ActionType m_anchorLaserActionType = AbilityData.ActionType.INVALID_ACTION;
	private AbilityData.ActionType m_tetherActionType = AbilityData.ActionType.INVALID_ACTION;
	private Exo_SyncComponent m_syncComponent;
	internal bool m_tetherEndedWithoutBreaking;
	internal List<ActorData> m_anchorLaserHitActorsThisTurn = new List<ActorData>();
	private List<AbilityData.ActionType> m_actionPreventUlt = new List<AbilityData.ActionType>();

	// added in rogues
	public bool IsAnchored()
	{
		return m_syncComponent != null && m_syncComponent.m_anchored;
	}

	// added in rogues
	private void ClearAnchored()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		SetAnchored(false);
		SetIdleTypeAndFacing();
	}

	// added in rogues
	public void SetAnchored(bool anchored)
	{
		if (!NetworkServer.active || m_syncComponent == null)
		{
			return;
		}
		if (!anchored)
		{
			m_syncComponent.Networkm_turnsAnchored = 0;
			m_syncComponent.Networkm_laserBarrierIsUp = false;
		}
		m_syncComponent.Networkm_anchored = anchored;
	}

	// added in rogues
	protected override void OnStartup()
	{
		m_abilityData = Owner.GetAbilityData();
		if (m_abilityData != null)
		{
			m_anchorLaserAbility = (m_abilityData.GetAbilityOfType(typeof(ExoAnchorLaser)) as ExoAnchorLaser);
			m_tetherAbility = (m_abilityData.GetAbilityOfType(typeof(ExoTetherTrap)) as ExoTetherTrap);
			if (m_anchorLaserAbility != null)
			{
				m_anchorLaserActionType = m_abilityData.GetActionTypeOfAbility(m_anchorLaserAbility);
			}
			if (m_tetherAbility != null)
			{
				m_tetherActionType = m_abilityData.GetActionTypeOfAbility(m_tetherAbility);
			}
			for (int i = 0; i <= 9; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				if (actionType != AbilityData.ActionType.ABILITY_4)
				{
					Ability abilityOfActionType = m_abilityData.GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && !abilityOfActionType.IsFreeAction())
					{
						m_actionPreventUlt.Add(actionType);
					}
				}
			}
		}
		m_syncComponent = Owner.GetComponent<Exo_SyncComponent>();
	}

	// added in rogues
	public override void OnTurnStart()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		base.OnTurnStart();
		if (!ServerEffectManager.Get().HasEffect(Owner, typeof(ExoPreventRagdollEffect)))
		{
			ExoPreventRagdollEffect effect = new ExoPreventRagdollEffect(AsEffectSource(), Owner);
			ServerEffectManager.Get().ApplyEffect(effect, 1);
		}
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_wasAnchoredOnTurnStart != m_syncComponent.m_anchored)
			{
				m_syncComponent.Networkm_wasAnchoredOnTurnStart = m_syncComponent.m_anchored;
				if (m_anchorLaserAbility != null && m_anchorLaserAbility.ShouldUpdateMovementOnAnchorChange())
				{
					Owner.GetActorMovement().UpdateSquaresCanMoveTo();
					if (Owner.GetActorController() != null)
					{
						Owner.GetActorController().CallRpcUpdateRemainingMovement(Owner.RemainingHorizontalMovement, Owner.RemainingMovementWithQueuedAbility);
					}
				}
			}
			if (m_syncComponent.m_anchored)
			{
				Exo_SyncComponent syncComponent = m_syncComponent;
				syncComponent.Networkm_turnsAnchored = (short)(syncComponent.m_turnsAnchored + 1);
			}
		}
		if (m_tetherAbility != null && m_tetherEndedWithoutBreaking)
		{
			int cdrOnTetherEndIfNotTriggered = m_tetherAbility.GetCdrOnTetherEndIfNotTriggered();
			if (cdrOnTetherEndIfNotTriggered > 0)
			{
				int num = m_abilityData.GetCooldownRemaining(m_tetherActionType);
				if (num > 0)
				{
					num = Mathf.Max(0, num - cdrOnTetherEndIfNotTriggered);
					m_abilityData.OverrideCooldown(m_tetherActionType, num);
				}
			}
			m_tetherEndedWithoutBreaking = false;
		}
		m_anchorLaserHitActorsThisTurn.Clear();
	}

	// added in rogues
	public override void OnTurnEnd()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		base.OnTurnEnd();
		if (m_persistingBarrierInstance != null
		    // TODO EXO check this
		    // reactor
		    && !ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(ExoAnchorLaser))
		    // rogues
		    // && !Owner.GetActorTurnSM().PveIsAbilityAtIndexUsed((int)m_anchorLaserActionType)
		    )
		{
			RemoveAnchoredLaser();
		}
	}

	// added in rogues
	public override void PreGatherResultsForPlayerAction(Ability ability)
	{
		if (!NetworkServer.active)
		{
			return;
		}

		AbilityData abilityData = GetComponent<AbilityData>();
		if (ability != null
		    && !(ability is ExoAnchorLaser)
		    && !(ability is ExoShield)
		    && IsAnchored()
		    // custom -- might cata should not turn the laser off
		    && !(abilityData != null
		         && AbilityData.IsCard(abilityData.GetActionTypeOfAbility(ability))
		         && ability.IsFreeAction())
			// end custom
		   ) 
		{
			RemoveAnchoredLaser();
		}
	}

	// added in rogues
	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (phase == AbilityPriority.Prep_Defense
		    && IsAnchored()
		    && m_anchorLaserAbility != null
		    && !Owner.GetAbilityData().HasQueuedAbilityOfType(m_anchorLaserAbility.GetType()))  // , true in rogues
		{
			RemoveAnchoredLaser();
		}
	}

	// added in rogues
	public override void OnAbilitiesDone()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		base.OnAbilitiesDone();
		if (Owner.IsDead())
		{
			RemoveAnchoredLaser();
		}
	}

	// added in rogues
	private void SetIdleTypeAndFacing()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		int num = m_syncComponent.m_anchored ? m_anchoredIdleType : m_unanchoredIdleType;
		if (Owner.GetModelAnimator() != null && Owner.GetModelAnimator().GetInteger("IdleType") != num)
		{
			Owner.GetModelAnimator().SetInteger("IdleType", num);
			if (num == 0)
			{
				Owner.GetModelAnimator().SetTrigger("ExitAnchor");
			}
		}
		m_syncComponent.CallRpcSetIdleType(num);
	}

	// added in rogues
	public void ClearExitAnchorAnimTrigger()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (Owner.GetModelAnimator() != null)
		{
			Owner.GetModelAnimator().ResetTrigger("ExitAnchor");
		}
		// m_syncComponent.CallRpcClearExitAnchorTrigger();
	}

	// added in rogues
	public void SetLaserSweepAnimDirection(bool sweepingToTheRight)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (Owner.GetModelAnimator() != null)
		{
			Owner.GetModelAnimator().SetBool("SweepingRight", sweepingToTheRight);
		}
		m_syncComponent.CallRpcSetSweepingRight(sweepingToTheRight);
	}

	// added in rogues
	public override void OnMovementStart(BoardSquare startSquare, BoardSquarePathInfo gameplayPathRemaining, ActorData.MovementType movementType)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		RemoveAnchoredLaser();
	}

	// added in rogues
	private void RemoveAnchoredLaser()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		ClearAnchored();
		if (m_persistingBarrierInstance != null && BarrierManager.Get().HasBarrier(m_persistingBarrierInstance))
		{
			BarrierManager.Get().RemoveBarrierAndLinkedSiblings(m_persistingBarrierInstance);
		}
		m_persistingBarrierInstance = null;
	}
#endif
}
