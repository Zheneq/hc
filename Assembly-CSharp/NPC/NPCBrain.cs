// ROGUES
// SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain : MonoBehaviour, IGameEventListener  // IGameEventListener removed in rogues
{
	// removed in rogues
	[Tooltip("Create new states in your scene, then add them to this list.")]
	public List<StateEntry> StateTable = new List<StateEntry>();
	// removed in rogues
	[Tooltip("ID of the starting state for this FSM. NullStateID will use the first state in StateTable. Make sure StartingState exists in the StateTable")]
	// removed in rogues
	public StateID StartingState;

	// removed in rogues
	private GameObject m_allocatedStateTableParent;
	
#if SERVER // rogues?
	// added in rogues
	[HideInInspector]
	public BotController m_botController;

	// added in rogues
	[HideInInspector]
	public int m_decisionPriority = 1;
#endif

	// removed in rogues
	[HideInInspector]
	public FSMSystem fsm { get; private set; }
	
	// removed in rogues
	[HideInInspector]
	public NPCBrain NextBrain { get; internal set; }

	private void Start()
	{
		if (GetComponent<BotController>() == null)
		{
			name += " [Prime]";
			enabled = false;
		}
	}

	// removed in rogues
	public void OnDestroy()
	{
		GameEventManager.Get().RemoveAllListenersFrom(this);
		if (m_allocatedStateTableParent != null)
		{
			Destroy(m_allocatedStateTableParent);
			m_allocatedStateTableParent = null;
		}
		if (fsm != null)
		{
			fsm.DestroyAllStates();
		}
	}

	// removed in rogues
	public bool CanTransistion(Transition trans)
	{
		return fsm != null && fsm.CanTransistion(trans);
	}

	// removed in rogues
	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (this != null && fsm != null && enabled)
		{
			fsm.OnGameEvent(eventType, args);
		}
	}

	// removed in rogues
	public void SetTransition(Transition t)
	{
		fsm.PerformTransition(t, this);
	}

	// removed in rogues
	public void SetPendingTransition(Transition t)
	{
		fsm.SetPendingTransition(t);
	}

	// removed in rogues
	public Transition GetPendingTransition()
	{
		return fsm.GetPendingTransition();
	}

	public virtual NPCBrain Create(BotController bot, Transform destination)
	{
		return null;
	}
	
#if SERVER // rogues?
	// added in rogues
	public virtual bool ShouldStopBrain()
	{
		return false;
	}
	
	// rogues
	public virtual bool ShouldDoAbilityBeforeMovement()
	{
		return false;
	}

	// rogues
	public virtual IEnumerator DecideAbilities()
	{
		yield break;
	}

	// rogues
	public virtual IEnumerator DecideMovement()
	{
		yield break;
	}
#endif

	// removed in rogues
	public virtual IEnumerator DecideTurn()
	{
		yield break;
	}

	// removed in rogues
	public virtual void SelectBotAbilityMods()
	{
		GetComponent<BotController>().SelectBotAbilityMods_Brainless();
	}

	// removed in rogues
	public virtual void SelectBotCards()
	{
		GetComponent<BotController>().SelectBotCards_Brainless();
	}

	// removed in rogues
	public IEnumerator FSMTakeTurn()
	{
		if (fsm != null)
		{
			yield return StartCoroutine(fsm.TakeTurn());
		}
		else
		{
			yield return StartCoroutine(DecideTurn());
		}
	}

	// removed in rogues
	protected virtual void MakeFSM(NPCBrain brainInstance)
	{
	}
}
