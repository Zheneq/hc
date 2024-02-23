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
		if (actor == null)
		{
			return null;
		}
		PassiveData passiveData = actor.GetPassiveData();
		if (passiveData == null)
		{
			return null;
		}
		foreach (Passive passive in passiveData.m_passives)
		{
			Passive_TricksterAfterImage image = passive as Passive_TricksterAfterImage;
			if (passive != null && !ReferenceEquals(image, null))
			{
				return image;
			}
		}
		return null;
	}
}
