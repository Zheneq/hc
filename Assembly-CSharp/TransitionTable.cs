using System;
using UnityEngine;

[Serializable]
public class TransitionTable
{
	[Tooltip("When we receive this transition message, we will transition to StateID")]
	public Transition Transition;

	[Tooltip("The StateID that we transition to. Make sure this exists in your StateTable. If BOTH the brain and the state are set, the brain is ignored.")]
	public StateID StateID;

	[Tooltip("The Brain to push onto the stack when we get this message. If BOTH the brain and the state are set, the brain is ignored.")]
	public NPCBrain BrainToPush;

	[Tooltip("If this is set, we destroy the current brain and return back to the previous. If the current brain is the only brain, nothing happens.")]
	public bool PopBrain;
}
