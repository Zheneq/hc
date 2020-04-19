using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMState : MonoBehaviour, IGameEventListener
{
	[HideInInspector]
	[SerializeField]
	protected StateID stateID;

	[Tooltip("Turn on to enable logging (OnEnter, OnExit, OnTurn) for this state")]
	public bool \u001D;

	protected Dictionary<Transition, TransitionTable> transitionTableMap = new Dictionary<Transition, TransitionTable>();

	private FSMSystem _myFSMCached;

	private NPCBrain _myBrainCached;

	public StateID StateID
	{
		get
		{
			return this.stateID;
		}
	}

	internal FSMSystem MyFSM
	{
		get
		{
			if (!this._myBrainCached.enabled)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.get_MyFSM()).MethodHandle;
				}
				int num = 0;
				num++;
				if (this.MyFSMBrain.fsm == this._myFSMCached)
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
					num++;
				}
			}
			return this._myFSMCached;
		}
		private set
		{
			this._myFSMCached = value;
		}
	}

	internal NPCBrain MyBrain
	{
		get
		{
			if (!this._myBrainCached.enabled)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.get_MyBrain()).MethodHandle;
				}
				if (base.transform)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (base.transform.gameObject)
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
						NPCBrain[] components = base.transform.gameObject.GetComponents<NPCBrain>();
						if (components != null)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							foreach (NPCBrain npcbrain in components)
							{
								if (npcbrain.enabled)
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
									return npcbrain;
								}
							}
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
				}
			}
			return this._myBrainCached;
		}
		private set
		{
			this._myBrainCached = value;
		}
	}

	internal NPCBrain_StateMachine MyFSMBrain
	{
		get
		{
			return this.MyBrain as NPCBrain_StateMachine;
		}
	}

	internal AbilityData MyAbilityData
	{
		get
		{
			AbilityData result;
			if (this.MyBrain)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.get_MyAbilityData()).MethodHandle;
				}
				result = this.MyBrain.GetComponent<AbilityData>();
			}
			else
			{
				result = null;
			}
			return result;
		}
	}

	internal ActorData MyActorData
	{
		get
		{
			return (!this.MyBrain) ? null : this.MyBrain.GetComponent<ActorData>();
		}
	}

	internal ActorTurnSM MyActorTurnSM
	{
		get
		{
			ActorTurnSM result;
			if (this.MyBrain)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.get_MyActorTurnSM()).MethodHandle;
				}
				result = this.MyBrain.GetComponent<ActorTurnSM>();
			}
			else
			{
				result = null;
			}
			return result;
		}
	}

	internal BotController MyBOTController
	{
		get
		{
			BotController result;
			if (this.MyBrain)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.get_MyBOTController()).MethodHandle;
				}
				result = this.MyBrain.GetComponent<BotController>();
			}
			else
			{
				result = null;
			}
			return result;
		}
	}

	private void Start()
	{
	}

	internal void Initalize(NPCBrain assoicatedBrain, FSMSystem associatedFSM)
	{
		this.MyBrain = assoicatedBrain;
		this.MyFSM = associatedFSM;
	}

	public virtual void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ScriptCommunication)
		{
			GameEventManager.ScriptCommunicationArgs scriptCommunicationArgs = args as GameEventManager.ScriptCommunicationArgs;
			if (!(scriptCommunicationArgs.NextBrain != null))
			{
				if (scriptCommunicationArgs.popBrain)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
					}
				}
				else
				{
					if (scriptCommunicationArgs.TransistionMessage != Transition.NullTransition)
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
						this.SetPendingTransition(scriptCommunicationArgs.TransistionMessage);
						goto IL_6F;
					}
					goto IL_6F;
				}
			}
			this.MyBrain.NextBrain = scriptCommunicationArgs.NextBrain;
			IL_6F:;
		}
	}

	public bool SetPendingTransition(Transition trans)
	{
		if (this.MyFSM.CanTransistion(trans))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.SetPendingTransition(Transition)).MethodHandle;
			}
			Transition pendingTransition = this.MyFSM.GetPendingTransition();
			if (pendingTransition != Transition.NullTransition && pendingTransition != trans)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.Log(string.Concat(new object[]
				{
					"NPC: ",
					this.MyBrain.name,
					" in state ",
					this.StateID,
					" already has a pending transition of ",
					pendingTransition,
					" but received a transition request of: ",
					trans,
					". Overwriting!"
				}));
			}
			this.MyBrain.SetPendingTransition(trans);
			return true;
		}
		return false;
	}

	public void AddTransition(Transition trans, TransitionTable inTable)
	{
		if (trans != Transition.NullTransition)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.AddTransition(Transition, TransitionTable)).MethodHandle;
			}
			if (inTable != null)
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
				if (inTable.StateID == StateID.NullStateID && inTable.BrainToPush == null)
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
					if (!inTable.PopBrain)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							goto IL_57;
						}
					}
				}
				if (this.transitionTableMap.ContainsKey(trans))
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
					Debug.LogError(string.Concat(new string[]
					{
						"FSMState ERROR: Assign State - State ",
						inTable.StateID.ToString(),
						" already has transition ",
						trans.ToString(),
						" - Impossible to assign to another state/brain to that transition"
					}));
					return;
				}
				this.transitionTableMap.Add(trans, inTable);
				return;
			}
		}
		IL_57:
		Debug.LogError("FSMState ERROR: Either the Transistion is NULL or you didnt specific a state/brain to pop/push for transition: " + trans);
	}

	public void DeleteTransition(Transition trans)
	{
		if (trans == Transition.NullTransition)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.DeleteTransition(Transition)).MethodHandle;
			}
			Debug.LogWarning("FSMState ERROR: NullTransition and NULL brain can not be removed");
			return;
		}
		if (this.MyFSM.GetPendingTransition() == trans)
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
			Debug.LogWarning("Ack - tried to remove a transition of " + trans + " that I have a pending change to. Deleting pending transition");
			this.MyFSM.SetPendingTransition(Transition.NullTransition);
		}
		this.transitionTableMap.Remove(trans);
	}

	public StateID GetOutputState(Transition trans)
	{
		if (this.transitionTableMap.ContainsKey(trans))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.GetOutputState(Transition)).MethodHandle;
			}
			return this.transitionTableMap[trans].StateID;
		}
		return StateID.NullStateID;
	}

	public NPCBrain GetOutputBrain(Transition trans)
	{
		if (this.transitionTableMap.ContainsKey(trans))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.GetOutputBrain(Transition)).MethodHandle;
			}
			return this.transitionTableMap[trans].BrainToPush;
		}
		return null;
	}

	public bool? GetPopBrain(Transition trans)
	{
		if (this.transitionTableMap.ContainsKey(trans))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.GetPopBrain(Transition)).MethodHandle;
			}
			return new bool?(this.transitionTableMap[trans].PopBrain);
		}
		return null;
	}

	public virtual FSMState DeepCopy()
	{
		return UnityEngine.Object.Instantiate<FSMState>(this);
	}

	public virtual void OnEnter(NPCBrain npc, StateID previousState)
	{
		if (this.\u001D)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.OnEnter(NPCBrain, StateID)).MethodHandle;
			}
			Log.Info(string.Concat(new object[]
			{
				"OnEnter(): '",
				npc.name,
				"' NewState: '",
				this.StateID,
				"' PreviousState: '",
				previousState,
				"'"
			}), new object[0]);
		}
	}

	public virtual void OnExit(NPCBrain npc, StateID nextState)
	{
		if (this.\u001D)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FSMState.OnExit(NPCBrain, StateID)).MethodHandle;
			}
			Log.Info(string.Concat(new object[]
			{
				"OnExit(): '",
				npc.name,
				"' NewState: '",
				this.StateID,
				"' PreviousState: '",
				nextState,
				"'"
			}), new object[0]);
		}
	}

	public virtual IEnumerator OnTurn(NPCBrain npc)
	{
		yield break;
	}
}
