using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSMSystem : IGameEventListener
{
	private List<FSMState> states;

	private StateID currentStateID;

	private FSMState currentState;

	private NPCBrain associatedBrain;

	private Transition pendingTransition;

	private StateID startingState;

	public FSMSystem(NPCBrain _brainInstance)
	{
		this.states = new List<FSMState>();
		this.associatedBrain = _brainInstance;
		this.pendingTransition = Transition.NullTransition;
		this.IsTakingTurn = false;
	}

	public StateID StartingStateID
	{
		get
		{
			return this.startingState;
		}
		set
		{
			this.startingState = value;
			if (value != StateID.NullStateID && null == this.states.Find((FSMState x) => x.StateID == this.StartingStateID))
			{
				object[] array = new object[5];
				array[0] = "Error: Character '";
				int num = 1;
				object obj;
				if (this.associatedBrain)
				{
					obj = this.associatedBrain.ToString();
				}
				else
				{
					obj = "Unknown";
				}
				array[num] = obj;
				array[2] = "' does not have starting state: ";
				array[3] = this.StartingStateID;
				array[4] = " in their StateTable. Please add it!";
				Log.Error(string.Concat(array), new object[0]);
				this.startingState = StateID.NullStateID;
			}
		}
	}

	public StateID CurrentStateID
	{
		get
		{
			return this.currentStateID;
		}
	}

	public FSMState CurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	public bool IsTakingTurn { get; private set; }

	internal void Initialize()
	{
		if (!(this.currentState == null))
		{
			if (this.currentState.StateID != StateID.NullStateID)
			{
				return;
			}
		}
		StateID stateID;
		if (this.StartingStateID == StateID.NullStateID)
		{
			stateID = this.states[0].StateID;
		}
		else
		{
			stateID = this.StartingStateID;
		}
		this.currentStateID = stateID;
		this.currentState = this.states.Find((FSMState x) => x.StateID == this.currentStateID);
		this.currentState.OnEnter(this.currentState.MyBrain, StateID.NullStateID);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		bool flag = true;
		if (eventType != GameEventManager.EventType.ActorDamaged_Server)
		{
			if (eventType != GameEventManager.EventType.CharacterHealedOrBuffed)
			{
			}
			else
			{
				GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = args as GameEventManager.CharacterHealBuffArgs;
				if (characterHealBuffArgs.targetCharacter == this.currentState.MyActorData)
				{
					FSMState fsmstate = this.currentState;
					Transition transition;
					if (characterHealBuffArgs.healed)
					{
						transition = Transition.Healed;
					}
					else
					{
						transition = Transition.Buffed;
					}
					fsmstate.SetPendingTransition(transition);
					flag = false;
				}
			}
		}
		else
		{
			GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
			if (actorHitHealthChangeArgs.m_target == this.currentState.MyActorData)
			{
				this.currentState.SetPendingTransition(Transition.TookDamage);
				flag = false;
			}
		}
		if (!flag)
		{
		}
		this.CurrentState.OnGameEvent(eventType, args);
	}

	public void AddState(FSMState s)
	{
		if (s == null)
		{
			Debug.LogError("FSM ERROR: Null reference is not allowed when adding a state");
		}
		if (this.states.Find((FSMState x) => x.StateID == s.StateID) != null)
		{
			Debug.LogError("FSM ERROR: Impossible to add state " + s.StateID.ToString() + " because state has already been added");
			return;
		}
		this.states.Add(s);
	}

	public void DeleteState(StateID id)
	{
		if (id == StateID.NullStateID)
		{
			Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
			return;
		}
		using (List<FSMState>.Enumerator enumerator = this.states.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FSMState fsmstate = enumerator.Current;
				if (fsmstate.StateID == id)
				{
					this.states.Remove(fsmstate);
					return;
				}
			}
		}
		Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() + ". It was not on the list of states");
	}

	public void DestroyAllStates()
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			FSMState fsmstate = this.states[i];
			if (fsmstate != null)
			{
				UnityEngine.Object.Destroy(fsmstate.gameObject);
			}
		}
		this.states.Clear();
	}

	public bool CanTransistion(Transition trans)
	{
		bool result;
		if (this.currentState)
		{
			if (this.currentState.GetOutputState(trans) == StateID.NullStateID)
			{
				if (!this.currentState.GetOutputBrain(trans))
				{
					if (this.currentState.GetPopBrain(trans) != null)
					{
						result = this.currentState.GetPopBrain(trans).Value;
					}
					else
					{
						result = false;
					}
					goto IL_8C;
				}
			}
			result = true;
			IL_8C:;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void SetPendingTransition(Transition trans)
	{
		this.pendingTransition = trans;
	}

	public Transition GetPendingTransition()
	{
		return this.pendingTransition;
	}

	public void PerformTransition(Transition trans, NPCBrain onWho)
	{
		if (trans == Transition.NullTransition)
		{
			Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
			return;
		}
		StateID outputState = this.currentState.GetOutputState(trans);
		NPCBrain outputBrain = this.currentState.GetOutputBrain(trans);
		if (outputState == StateID.NullStateID && outputBrain != null)
		{
			return;
		}
		if (outputState == StateID.NullStateID)
		{
			Debug.LogError("FSM ERROR: State " + this.currentStateID.ToString() + " does not have a target state/brain  for transition " + trans.ToString());
			return;
		}
		this.currentStateID = outputState;
		using (List<FSMState>.Enumerator enumerator = this.states.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FSMState fsmstate = enumerator.Current;
				if (fsmstate.StateID == this.currentStateID)
				{
					StateID previousState = this.currentStateID;
					this.currentState.OnExit(onWho, fsmstate.StateID);
					this.currentState = fsmstate;
					this.currentState.OnEnter(onWho, previousState);
					return;
				}
			}
		}
	}

	internal IEnumerator TakeTurn()
	{
		while (this.pendingTransition != Transition.NullTransition)
		{
			Transition transition = this.pendingTransition;
			this.pendingTransition = Transition.NullTransition;
			this.PerformTransition(transition, this.associatedBrain);
			if (this.pendingTransition != transition)
			{
				if (this.pendingTransition != Transition.NullTransition)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Hmm, a transition caused a transition (",
						this.pendingTransition,
						"). Not sure if this is good. FSM: ",
						this.ToString()
					}), new object[0]);
				}
			}
		}
		this.IsTakingTurn = true;
		IEnumerator result = this.CurrentState.OnTurn(this.associatedBrain);
		this.IsTakingTurn = false;
		return result;
	}
}
