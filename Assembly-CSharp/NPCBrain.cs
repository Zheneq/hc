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
	public FSMSystem fsm
	{
		get;
		private set;
	}

	[HideInInspector]
	public NPCBrain NextBrain
	{
		get;
		internal set;
	}

	private void Start()
	{
		if (!(GetComponent<BotController>() == null))
		{
			return;
		}
		while (true)
		{
			base.name += " [Prime]";
			base.enabled = false;
			return;
		}
	}

	public void OnDestroy()
	{
		GameEventManager.Get().RemoveAllListenersFrom(this);
		if (m_allocatedStateTableParent != null)
		{
			Object.Destroy(m_allocatedStateTableParent);
			m_allocatedStateTableParent = null;
		}
		if (fsm == null)
		{
			return;
		}
		while (true)
		{
			fsm.DestroyAllStates();
			return;
		}
	}

	public bool CanTransistion(Transition trans)
	{
		int result;
		if (fsm != null)
		{
			result = (fsm.CanTransistion(trans) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (!(this != null) || fsm == null || !base.enabled)
		{
			return;
		}
		while (true)
		{
			fsm.OnGameEvent(eventType, args);
			return;
		}
	}

	public void SetTransition(Transition t)
	{
		fsm.PerformTransition(t, this);
	}

	public void SetPendingTransition(Transition t)
	{
		fsm.SetPendingTransition(t);
	}

	public Transition GetPendingTransition()
	{
		return fsm.GetPendingTransition();
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
		GetComponent<BotController>().SelectBotAbilityMods_Brainless();
	}

	public virtual void SelectBotCards()
	{
		GetComponent<BotController>().SelectBotCards_Brainless();
	}

	public IEnumerator FSMTakeTurn()
	{
		if (fsm != null)
		{
			yield return StartCoroutine(fsm.TakeTurn());
		}
		yield return StartCoroutine(DecideTurn());
		/*Error: Unable to find new state assignment for yield return*/;
	}

	protected virtual void MakeFSM(NPCBrain brainInstance)
	{
	}
}
