using UnityEngine;

public class Passive_Martyr : Passive
{
	public enum CrystalGainMode
	{
		ByEnergy,
		ByDamageTaken
	}

	[Header("-- How to gain crystals --")]
	public CrystalGainMode m_crystalGainMode;
	[Header("-- [ByEnergy] Energy gain on Damage --")]
	public float m_energyGainPerDamageTaken = 1f;
	public bool m_skipEnergyGainIfUsingUlt;
	[Header("-- [ByEnergy] Energy Adjust On Respawn (final = Mult * Current + Adjust) --")]
	public int m_energyAdjustOnRespawn;
	public float m_energyMultOnRespawn = 1f;
	[Header("-- [ByEnergy] energy amount per crystal")]
	public int m_energyToCrystalConversion = 20;
	[Header("-- [ByDamageTaken] Crystals gained at end of turn = damage received this turn / conversion")]
	public float m_damageToCrystalConversion = 20f;
	[Header("-- Energy gained per crystal gained on beginning of turn")]
	public int m_energyGainPerCrystal;
	[Header("-- Crystal Count and Increments --")]
	public int m_maxCrystals;
	[Tooltip("The cap on crystal gain each turn. 0 means no cap.")]
	public int m_maxCrystalsGainedEachTurn;
	[Tooltip("A passive gain of crystals on each turn start. Can be negative.")]
	public int m_automaticCrystalGainedEachTurn;
	[Tooltip("All abilities get the crystal bonus even if SpendCrystals isn't used this turn")]
	public bool m_automaticCrystalBonus = true;

	private Martyr_SyncComponent m_syncComponent;

	public int DamageReceivedThisTurn
	{
		get;
		set;
	}
}
