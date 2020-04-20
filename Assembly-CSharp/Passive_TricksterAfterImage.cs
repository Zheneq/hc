using System;
using UnityEngine;

public class Passive_TricksterAfterImage : Passive
{
	[Header("-- Clone Info")]
	public GameObject m_afterImagePrefab;

	public CharacterResourceLink m_afterImageResourceLink;

	[Header("-- Duration and Despawning")]
	public int m_maxDuration = 2;

	public SpoilsSpawnData m_spoilSpawnOnDisappear;

	[Header("-- When to create clone on movement")]
	public bool m_cloneOnTeleport;

	public bool m_cloneOnKnockback;

	[Header("-- Animation")]
	public AbilityData.ActionType m_onEntryAnimIndex = AbilityData.ActionType.ABILITY_6;

	internal const int c_maxAfterImages = 2;

	public static Passive_TricksterAfterImage GetFromActor(ActorData actor)
	{
		if (actor != null)
		{
			PassiveData passiveData = actor.GetPassiveData();
			if (passiveData != null)
			{
				foreach (Passive passive in passiveData.m_passives)
				{
					if (passive != null)
					{
						if (passive is Passive_TricksterAfterImage)
						{
							return passive as Passive_TricksterAfterImage;
						}
					}
				}
			}
		}
		return null;
	}
}
