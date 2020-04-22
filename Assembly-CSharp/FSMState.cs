using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMState : MonoBehaviour, IGameEventListener
{
	[HideInInspector]
	[SerializeField]
	protected StateID stateID;

	[Tooltip("Turn on to enable logging (OnEnter, OnExit, OnTurn) for this state")]
	public bool _001D;

	protected Dictionary<Transition, TransitionTable> transitionTableMap = new Dictionary<Transition, TransitionTable>();

	private FSMSystem _myFSMCached;

	private NPCBrain _myBrainCached;

	public StateID StateID => stateID;

	internal FSMSystem MyFSM
	{
		get
		{
			if (!_myBrainCached.enabled)
			{
				int num = 0;
				num++;
				if (MyFSMBrain.fsm == _myFSMCached)
				{
					num++;
				}
			}
			return _myFSMCached;
		}
		private set
		{
			_myFSMCached = value;
		}
	}

	internal NPCBrain MyBrain
	{
		get
		{
			if (!_myBrainCached.enabled)
			{
				if ((bool)base.transform)
				{
					if ((bool)base.transform.gameObject)
					{
						NPCBrain[] components = base.transform.gameObject.GetComponents<NPCBrain>();
						if (components != null)
						{
							NPCBrain[] array = components;
							foreach (NPCBrain nPCBrain in array)
							{
								if (!nPCBrain.enabled)
								{
									continue;
								}
								while (true)
								{
									return nPCBrain;
								}
							}
						}
					}
				}
			}
			return _myBrainCached;
		}
		private set
		{
			_myBrainCached = value;
		}
	}

	internal NPCBrain_StateMachine MyFSMBrain => MyBrain as NPCBrain_StateMachine;

	internal AbilityData MyAbilityData
	{
		get
		{
			object result;
			if ((bool)MyBrain)
			{
				result = MyBrain.GetComponent<AbilityData>();
			}
			else
			{
				result = null;
			}
			return (AbilityData)result;
		}
	}

	internal ActorData MyActorData => (!MyBrain) ? null : MyBrain.GetComponent<ActorData>();

	internal ActorTurnSM MyActorTurnSM
	{
		get
		{
			object result;
			if ((bool)MyBrain)
			{
				result = MyBrain.GetComponent<ActorTurnSM>();
			}
			else
			{
				result = null;
			}
			return (ActorTurnSM)result;
		}
	}

	internal BotController MyBOTController
	{
		get
		{
			object result;
			if ((bool)MyBrain)
			{
				result = MyBrain.GetComponent<BotController>();
			}
			else
			{
				result = null;
			}
			return (BotController)result;
		}
	}

	private void Start()
	{
	}

	internal void Initalize(NPCBrain assoicatedBrain, FSMSystem associatedFSM)
	{
		MyBrain = assoicatedBrain;
		MyFSM = associatedFSM;
	}

	public virtual void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.ScriptCommunication)
		{
			return;
		}
		GameEventManager.ScriptCommunicationArgs scriptCommunicationArgs = args as GameEventManager.ScriptCommunicationArgs;
		if (!(scriptCommunicationArgs.NextBrain != null))
		{
			if (!scriptCommunicationArgs.popBrain)
			{
				if (scriptCommunicationArgs.TransistionMessage == Transition.NullTransition)
				{
					return;
				}
				while (true)
				{
					SetPendingTransition(scriptCommunicationArgs.TransistionMessage);
					return;
				}
			}
		}
		MyBrain.NextBrain = scriptCommunicationArgs.NextBrain;
	}

	public bool SetPendingTransition(Transition trans)
	{
		if (MyFSM.CanTransistion(trans))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					Transition pendingTransition = MyFSM.GetPendingTransition();
					if (pendingTransition != 0 && pendingTransition != trans)
					{
						Debug.Log(string.Concat("NPC: ", MyBrain.name, " in state ", StateID, " already has a pending transition of ", pendingTransition, " but received a transition request of: ", trans, ". Overwriting!"));
					}
					MyBrain.SetPendingTransition(trans);
					return true;
				}
				}
			}
		}
		return false;
	}

	public void AddTransition(Transition trans, TransitionTable inTable)
	{
		if (trans != 0)
		{
			if (inTable != null)
			{
				if (inTable.StateID == StateID.NullStateID && inTable.BrainToPush == null)
				{
					if (!inTable.PopBrain)
					{
						goto IL_0057;
					}
				}
				if (transitionTableMap.ContainsKey(trans))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							Debug.LogError("FSMState ERROR: Assign State - State " + inTable.StateID.ToString() + " already has transition " + trans.ToString() + " - Impossible to assign to another state/brain to that transition");
							return;
						}
					}
				}
				transitionTableMap.Add(trans, inTable);
				return;
			}
		}
		goto IL_0057;
		IL_0057:
		Debug.LogError("FSMState ERROR: Either the Transistion is NULL or you didnt specific a state/brain to pop/push for transition: " + trans);
	}

	public void DeleteTransition(Transition trans)
	{
		if (trans == Transition.NullTransition)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("FSMState ERROR: NullTransition and NULL brain can not be removed");
					return;
				}
			}
		}
		if (MyFSM.GetPendingTransition() == trans)
		{
			Debug.LogWarning(string.Concat("Ack - tried to remove a transition of ", trans, " that I have a pending change to. Deleting pending transition"));
			MyFSM.SetPendingTransition(Transition.NullTransition);
		}
		transitionTableMap.Remove(trans);
	}

	public StateID GetOutputState(Transition trans)
	{
		if (transitionTableMap.ContainsKey(trans))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return transitionTableMap[trans].StateID;
				}
			}
		}
		return StateID.NullStateID;
	}

	public NPCBrain GetOutputBrain(Transition trans)
	{
		if (transitionTableMap.ContainsKey(trans))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return transitionTableMap[trans].BrainToPush;
				}
			}
		}
		return null;
	}

	public bool? GetPopBrain(Transition trans)
	{
		if (transitionTableMap.ContainsKey(trans))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return transitionTableMap[trans].PopBrain;
				}
			}
		}
		return null;
	}

	public virtual FSMState DeepCopy()
	{
		return Object.Instantiate(this);
	}

	public virtual void OnEnter(NPCBrain npc, StateID previousState)
	{
		if (!_001D)
		{
			return;
		}
		while (true)
		{
			Log.Info(string.Concat("OnEnter(): '", npc.name, "' NewState: '", StateID, "' PreviousState: '", previousState, "'"));
			return;
		}
	}

	public virtual void OnExit(NPCBrain npc, StateID nextState)
	{
		if (!_001D)
		{
			return;
		}
		while (true)
		{
			Log.Info(string.Concat("OnExit(): '", npc.name, "' NewState: '", StateID, "' PreviousState: '", nextState, "'"));
			return;
		}
	}

	public virtual IEnumerator OnTurn(NPCBrain npc)
	{
		yield break;
	}
}
