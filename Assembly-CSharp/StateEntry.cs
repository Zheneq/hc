using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateEntry
{
	[Tooltip("Link in a state object that exists in your level.")]
	public FSMState CurrentState;

	[Tooltip("Add any transitions you want to map for this state (ie when attacked, got this state)")]
	public List<TransitionTable> Transitions;
}
