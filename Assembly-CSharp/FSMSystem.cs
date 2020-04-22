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

	public StateID StartingStateID
	{
		get
		{
			return startingState;
		}
		set
		{
			startingState = value;
			if (value == StateID.NullStateID || !(null == states.Find((FSMState x) => x.StateID == StartingStateID)))
			{
				return;
			}
			object[] obj = new object[5]
			{
				"Error: Character '",
				null,
				null,
				null,
				null
			};
			object obj2;
			if ((bool)associatedBrain)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				obj2 = associatedBrain.ToString();
			}
			else
			{
				obj2 = "Unknown";
			}
			obj[1] = obj2;
			obj[2] = "' does not have starting state: ";
			obj[3] = StartingStateID;
			obj[4] = " in their StateTable. Please add it!";
			Log.Error(string.Concat(obj));
			startingState = StateID.NullStateID;
		}
	}

	public StateID CurrentStateID => currentStateID;

	public FSMState CurrentState => currentState;

	public bool IsTakingTurn
	{
		get;
		private set;
	}

	public FSMSystem(NPCBrain _brainInstance)
	{
		states = new List<FSMState>();
		associatedBrain = _brainInstance;
		pendingTransition = Transition.NullTransition;
		IsTakingTurn = false;
	}

	internal void Initialize()
	{
		if (!(currentState == null))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentState.StateID != 0)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		StateID num;
		if (StartingStateID == StateID.NullStateID)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			num = states[0].StateID;
		}
		else
		{
			num = StartingStateID;
		}
		currentStateID = num;
		currentState = states.Find((FSMState x) => x.StateID == currentStateID);
		currentState.OnEnter(currentState.MyBrain, StateID.NullStateID);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		bool flag = true;
		if (eventType != GameEventManager.EventType.ActorDamaged_Server)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (eventType != GameEventManager.EventType.CharacterHealedOrBuffed)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = args as GameEventManager.CharacterHealBuffArgs;
				if (characterHealBuffArgs.targetCharacter == currentState.MyActorData)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					FSMState fSMState = currentState;
					int num;
					if (characterHealBuffArgs.healed)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						num = 4;
					}
					else
					{
						num = 5;
					}
					fSMState.SetPendingTransition((Transition)num);
					flag = false;
				}
			}
		}
		else
		{
			GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
			if (actorHitHealthChangeArgs.m_target == currentState.MyActorData)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				currentState.SetPendingTransition(Transition.TookDamage);
				flag = false;
			}
		}
		if (!flag)
		{
			if (1 == 0)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		CurrentState.OnGameEvent(eventType, args);
	}

	public void AddState(FSMState s)
	{
		if (s == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Debug.LogError("FSM ERROR: Null reference is not allowed when adding a state");
		}
		if (states.Find((FSMState x) => x.StateID == s.StateID) != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("FSM ERROR: Impossible to add state " + s.StateID.ToString() + " because state has already been added");
					return;
				}
			}
		}
		states.Add(s);
	}

	public void DeleteState(StateID id)
	{
		if (id == StateID.NullStateID)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
					return;
				}
			}
		}
		using (List<FSMState>.Enumerator enumerator = states.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FSMState current = enumerator.Current;
				if (current.StateID == id)
				{
					states.Remove(current);
					return;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() + ". It was not on the list of states");
	}

	public void DestroyAllStates()
	{
		for (int i = 0; i < states.Count; i++)
		{
			FSMState fSMState = states[i];
			if (fSMState != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				UnityEngine.Object.Destroy(fSMState.gameObject);
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			states.Clear();
			return;
		}
	}

	public bool CanTransistion(Transition trans)
	{
		int result;
		if ((bool)currentState)
		{
			if (currentState.GetOutputState(trans) == StateID.NullStateID)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!currentState.GetOutputBrain(trans))
				{
					if (currentState.GetPopBrain(trans).HasValue)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						result = (currentState.GetPopBrain(trans).Value ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					goto IL_008f;
				}
			}
			result = 1;
		}
		else
		{
			result = 0;
		}
		goto IL_008f;
		IL_008f:
		return (byte)result != 0;
	}

	public void SetPendingTransition(Transition trans)
	{
		pendingTransition = trans;
	}

	public Transition GetPendingTransition()
	{
		return pendingTransition;
	}

	public void PerformTransition(Transition trans, NPCBrain onWho)
	{
		if (trans == Transition.NullTransition)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
					return;
				}
			}
		}
		StateID outputState = currentState.GetOutputState(trans);
		NPCBrain outputBrain = currentState.GetOutputBrain(trans);
		if (outputState == StateID.NullStateID && outputBrain != null)
		{
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (outputState == StateID.NullStateID)
		{
			Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state/brain  for transition " + trans);
			return;
		}
		currentStateID = outputState;
		using (List<FSMState>.Enumerator enumerator = states.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FSMState current = enumerator.Current;
				if (current.StateID == currentStateID)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							StateID previousState = currentStateID;
							currentState.OnExit(onWho, current.StateID);
							currentState = current;
							currentState.OnEnter(onWho, previousState);
							return;
						}
						}
					}
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	internal IEnumerator TakeTurn()
	{
		while (pendingTransition != 0)
		{
			Transition transition = pendingTransition;
			pendingTransition = Transition.NullTransition;
			PerformTransition(transition, associatedBrain);
			if (pendingTransition == transition)
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (pendingTransition != 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(string.Concat("Hmm, a transition caused a transition (", pendingTransition, "). Not sure if this is good. FSM: ", ToString()));
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			IsTakingTurn = true;
			IEnumerator result = CurrentState.OnTurn(associatedBrain);
			IsTakingTurn = false;
			return result;
		}
	}
}
