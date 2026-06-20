// ROGUES
// SERVER
using System;
using UnityEngine;

// same in reactor & rouges save for serialization
public class ThiefPowerupReturnProjectileSequence : ArcingProjectileSequence
{
	public class PowerupTypeExtraParams : IExtraSequenceParams
	{
		public int powerupCategory;

		public override void XSP_SerializeToStream(IBitStream stream) // NetworkWriter writer in rogues
		{
			stream.Serialize(ref powerupCategory);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream) // NetworkReader reader in rogues
		{
			stream.Serialize(ref powerupCategory);
		}
	}

	[Serializable]
	public class PowerUpCategoryToVFX
	{
		public PowerUp.PowerUpCategory m_category;
		public GameObject m_vfxPrefab;

		public static GameObject GetPrefabForCategory(PowerUpCategoryToVFX[] catToVfx, int categoryInt)
		{
			if (catToVfx == null)
			{
				return null;
			}
			foreach (PowerUpCategoryToVFX powerUpCategoryToVFX in catToVfx)
			{
				if (powerUpCategoryToVFX.m_category == (PowerUp.PowerUpCategory)categoryInt
				    && powerUpCategoryToVFX.m_vfxPrefab != null)
				{
					return powerUpCategoryToVFX.m_vfxPrefab;
				}
			}
			return null;
		}
	}

	[Header("-- Powerup to VFX mapping --")]
	public PowerUpCategoryToVFX[] m_powerupToProjectileVFXPrefab;
	[Space(10f)]
	public PowerUpCategoryToVFX[] m_powerupToImpactVFXPrefab;

	private int m_powerupCategoryInt = -1;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is PowerupTypeExtraParams powerupTypeExtraParams)
			{
				m_powerupCategoryInt = powerupTypeExtraParams.powerupCategory;
			}
		}
	}

	protected override GameObject GetProjectileFxPrefab()
	{
		GameObject prefab = PowerUpCategoryToVFX.GetPrefabForCategory(m_powerupToProjectileVFXPrefab, m_powerupCategoryInt);
		if (prefab == null)
		{
			prefab = m_fxPrefab;
		}
		return prefab;
	}

	protected override GameObject GetImpactFxPrefab()
	{
		GameObject prefab = PowerUpCategoryToVFX.GetPrefabForCategory(m_powerupToImpactVFXPrefab, m_powerupCategoryInt);
		if (prefab == null)
		{
			prefab = m_fxImpactPrefab;
		}
		return prefab;
	}
}
