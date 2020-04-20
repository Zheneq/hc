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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Passive_TricksterAfterImage.GetFromActor(ActorData)).MethodHandle;
			}
			PassiveData passiveData = actor.GetPassiveData();
			if (passiveData != null)
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
				foreach (Passive passive in passiveData.m_passives)
				{
					if (passive != null)
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
