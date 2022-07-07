// ROGUES
// SERVER
using UnityEngine;

public class Passive_Rampart : Passive
{
	[Header("-- Normal Shield Barrier Info")]
	public int m_normalShieldDuration = 2;
	public StandardBarrierData m_normalShieldBarrierData;
	[Header("-- Anim")]
	public int m_unshieldedIdleType;
	public int m_shieldedIdleType = 1;
	[Header("-- Sequences")]
	public GameObject m_removeShieldWaitSequencePrefab;

	private StandardBarrierData m_cachedShieldBarrierData;
	
#if SERVER
	// added in rogues
	private Rampart_SyncComponent m_syncComp;
	private int m_shieldTurnsUntilRemove;
	private Barrier m_shieldBarrier;
	private Vector3 m_shieldFacingDir = Vector3.forward;
	private bool m_reapplyShieldInEvade;
	private bool m_reapplyShieldOnTurnStart;
#endif
	
#if SERVER
	// custom
	private static readonly int animIdleType = Animator.StringToHash("IdleType");
	private static readonly int animForceIdle = Animator.StringToHash("ForceIdle");
#endif

	public StandardBarrierData GetShieldBarrierData()
	{
		return m_cachedShieldBarrierData ?? m_normalShieldBarrierData;
	}

	public void SetCachedShieldBarrierData(AbilityModPropertyBarrierDataV2 barrierMod)
	{
#if SERVER
		if (m_normalShieldBarrierData == null) // added in rogues
		{
			// TODO check this
			// custom
			m_normalShieldBarrierData = new StandardBarrierData();
			// rogues
			// m_normalShieldBarrierData = ScriptableObject.CreateInstance<StandardBarrierData>();
		}
#endif
		m_cachedShieldBarrierData = barrierMod?.GetModifiedValue(m_normalShieldBarrierData);
	}

#if SERVER
	// added in rogues
	public Barrier GetCurrentShieldBarrier()
	{
		return m_shieldBarrier;
	}

	// added in rogues
	public void SetShieldBarrier(Barrier barrier, Vector3 facing)
	{
		m_shieldBarrier = barrier;
		m_shieldFacingDir = facing;
		m_shieldTurnsUntilRemove = m_normalShieldDuration;
	}

	// added in rogues
	private void ReapplyShield()
	{
		if (m_shieldBarrier != null)
		{
			Debug.LogError("Current Shield still exist, shouldn't reapply");
			return;
		}
		if (GetShieldBarrierData() != null)
		{
			Vector3 center = Owner.GetFreePos() + 0.5f * Board.Get().squareSize * m_shieldFacingDir;
			Barrier barrier = new Barrier("Rampart Shield", center, m_shieldFacingDir, Owner, GetShieldBarrierData());
			barrier.SetSourceAbility(Owner.GetAbilityData().GetAbilityOfType(typeof(RampartAimShield)));
			m_shieldBarrier = barrier;
			BarrierManager.Get().AddBarrier(barrier, false, out _);  // , false in rogues
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}
	}

	// added in rogues
	private void RemoveShield()
	{
		if (m_shieldBarrier != null)
		{
			BarrierManager.Get().RemoveBarrier(m_shieldBarrier);
			m_shieldBarrier = null;
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}
	}

	// added in rogues
	public void SetIdleTypeForShield(bool hasShield)
	{
		if (Owner.GetModelAnimator() != null)
		{
			int num = hasShield ? m_shieldedIdleType : m_unshieldedIdleType;
			int integer = Owner.GetModelAnimator().GetInteger(animIdleType);
			if (integer != num)
			{
				Owner.GetModelAnimator().SetInteger(animIdleType, num);
			}
			if (hasShield)
			{
				if (integer != num)
				{
					Owner.GetModelAnimator().SetTrigger(animForceIdle);
				}
				if (GetCurrentShieldBarrier() != null)
				{
					Owner.TurnToDirection(m_shieldFacingDir);
					m_syncComp.CallRpcSetFacingDirection(m_shieldFacingDir);
				}
			}
			m_syncComp.CallRpcSetIdleType(num);
		}
	}

	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_syncComp = GetComponent<Rampart_SyncComponent>();
	}

	// added in rogues
	public override void OnTurnStart()
	{
		if (!ServerEffectManager.Get().HasEffect(Owner, typeof(RampartShieldIdleSetterEffect)))
		{
			RampartShieldIdleSetterEffect effect = new RampartShieldIdleSetterEffect(AsEffectSource(), Owner, this);
			ServerEffectManager.Get().ApplyEffect(effect, 1);
		}
		m_reapplyShieldInEvade = false;
		bool flag = false;
		if (m_shieldTurnsUntilRemove > 0)
		{
			m_shieldTurnsUntilRemove--;
			if (m_shieldTurnsUntilRemove <= 0)
			{
				RemoveShield();
				flag = true;
			}
		}
		if (m_shieldTurnsUntilRemove > 0 && m_reapplyShieldOnTurnStart)
		{
			ReapplyShield();
			flag = false;
		}
		if (flag)
		{
			SetIdleTypeForShield(false);
		}
		m_reapplyShieldOnTurnStart = false;
	}

	// added in rogues
	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		if (phase == AbilityPriority.Prep_Offense && m_shieldBarrier != null && ServerActionBuffer.Get().ActorIsEvading(Owner))
		{
			m_reapplyShieldInEvade = true;
			RemoveShield();
		}
		if (phase == AbilityPriority.Evasion && m_reapplyShieldInEvade && m_shieldBarrier == null && GetShieldBarrierData() != null)
		{
			ReapplyShield();
		}
		if (GetCurrentShieldBarrier() != null && phase != AbilityPriority.Combat_Final)
		{
			SetIdleTypeForShield(true);
		}
	}

	// added in rogues
	public override void OnAbilitiesDone()
	{
		base.OnAbilitiesDone();
		if ((m_shieldTurnsUntilRemove == 1 || Owner.IsDead()) && m_shieldBarrier != null)
		{
			m_shieldTurnsUntilRemove = 0;
			SetIdleTypeForShield(false);
			RemoveShield();
		}
	}

	// added in rogues
	public override void OnMovementResultsGathered(MovementCollection stabilizedMovements)
	{
		foreach (MovementInstance inst in stabilizedMovements.m_movementInstances)
		{
			if (inst.m_mover == Owner && m_shieldBarrier != null)
			{
				BarrierManager.Get().RemoveBarrier(m_shieldBarrier);
				m_shieldBarrier = null;
				m_reapplyShieldOnTurnStart = true;
				GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
			}
		}
	}
#endif
}
