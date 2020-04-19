using System;
using UnityEngine;

public class Passive : MonoBehaviour
{
	public string m_passiveName;

	protected ActorData m_owner;

	public GameObject m_sequencePrefab;

	public AbilityStatMod[] m_passiveStatMods;

	public StatusType[] m_passiveStatusChanges;

	public EmptyTurnData[] m_onEmptyTurn;

	public PassiveEventData[] m_eventResponses;

	public StandardActorEffectData[] m_effectsOnEveryTurn;

	public ActorData Owner
	{
		get
		{
			return this.m_owner;
		}
		set
		{
			this.m_owner = value;
		}
	}
}
