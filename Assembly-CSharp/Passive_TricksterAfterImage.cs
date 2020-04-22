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
			PassiveData passiveData = actor.GetPassiveData();
			if (passiveData != null)
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
				Passive[] passives = passiveData.m_passives;
				foreach (Passive passive in passives)
				{
					if (passive != null)
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
