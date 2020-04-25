using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain : MonoBehaviour, IGameEventListener
{
	[Tooltip("Create new states in your scene, then add them to this list.")]
	public List<StateEntry> StateTable = new List<StateEntry>();

	[Tooltip("ID of the starting state for this FSM. NullStateID will use the first state in StateTable. Make sure StartingState exists in the StateTable")]
	public StateID StartingState;

	private GameObject m_allocatedStateTableParent;

	[HideInInspector]
	public FSMSystem fsm { get; private set; }

	[HideInInspector]
	public NPCBrain NextBrain { get; internal set; }

	private void Start()
	{
		if (base.GetComponent<BotController>() == null)
		{
			base.name += " [Prime]";
			base.enabled = false;
		}
	}

	public void OnDestroy()
	{
		GameEventManager.Get().RemoveAllListenersFrom(this);
		if (this.m_allocatedStateTableParent != null)
		{
			UnityEngine.Object.Destroy(this.m_allocatedStateTableParent);
			this.m_allocatedStateTableParent = null;
		}
		if (this.fsm != null)
		{
			this.fsm.DestroyAllStates();
		}
	}

	public bool CanTransistion(Transition trans)
	{
		bool result;
		if (this.fsm != null)
		{
			result = this.fsm.CanTransistion(trans);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (this != null && this.fsm != null && base.enabled)
		{
			this.fsm.OnGameEvent(eventType, args);
		}
	}

	public void SetTransition(Transition t)
	{
		this.fsm.PerformTransition(t, this);
	}

	public void SetPendingTransition(Transition t)
	{
		this.fsm.SetPendingTransition(t);
	}

	public Transition GetPendingTransition()
	{
		return this.fsm.GetPendingTransition();
	}

	public virtual NPCBrain Create(BotController bot, Transform destination)
	{
		return null;
	}

	public virtual IEnumerator DecideTurn()
	{
		yield break;
	}

	public virtual void SelectBotAbilityMods()
	{
		base.GetComponent<BotController>().SelectBotAbilityMods_Brainless();
	}

	public virtual void SelectBotCards()
	{
		base.GetComponent<BotController>().SelectBotCards_Brainless();
	}

	public IEnumerator FSMTakeTurn()
	{
		if (this.fsm != null)
		{
			yield return base.StartCoroutine(this.fsm.TakeTurn());
		}
		else
		{
			yield return base.StartCoroutine(this.DecideTurn());
		}
		yield break;
	}

	protected virtual void MakeFSM(NPCBrain brainInstance)
	{
	}
}
