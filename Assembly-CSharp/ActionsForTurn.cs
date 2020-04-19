using System;
using UnityEngine;

[Serializable]
public class ActionsForTurn
{
	public AbilityData.ActionType m_actionToDo;

	public GameObject[] m_targets;

	public GameObject[] m_moveDestinations;

	public bool m_chase;
}
