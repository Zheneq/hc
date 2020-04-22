using System.Collections;
using UnityEngine;

public class IdleState : FSMState
{
	private void Start()
	{
		stateID = StateID.Idle;
	}

	public override void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		base.OnGameEvent(eventType, args);
	}

	public override IEnumerator OnTurn(NPCBrain thisBrain)
	{
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Debug.Log("Waiting!!");
			yield break;
		}
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
