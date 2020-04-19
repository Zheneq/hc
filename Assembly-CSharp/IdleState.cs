using System;
using System.Collections;
using UnityEngine;

public class IdleState : FSMState
{
	private void Start()
	{
		this.stateID = StateID.Idle;
	}

	public override void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		base.OnGameEvent(eventType, args);
	}

	public override IEnumerator OnTurn(NPCBrain thisBrain)
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(IdleState.<OnTurn>c__Iterator0.MoveNext()).MethodHandle;
		}
		Debug.Log("Waiting!!");
		yield break;
	}

	public override void OnEnter(NPCBrain thisBrain, StateID previousState)
	{
		base.OnEnter(thisBrain, previousState);
	}

	public override void OnExit(NPCBrain thisBrain, StateID nextState)
	{
		base.OnExit(thisBrain, nextState);
	}
}
