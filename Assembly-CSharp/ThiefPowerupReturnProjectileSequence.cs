using System;
using UnityEngine;

public class ThiefPowerupReturnProjectileSequence : ArcingProjectileSequence
{
	public class PowerupTypeExtraParams : IExtraSequenceParams
	{
		public int powerupCategory;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref powerupCategory);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
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
			GameObject result = null;
			if (catToVfx != null)
			{
				foreach (PowerUpCategoryToVFX powerUpCategoryToVFX in catToVfx)
				{
					if (powerUpCategoryToVFX.m_category == (PowerUp.PowerUpCategory)categoryInt && powerUpCategoryToVFX.m_vfxPrefab != null)
					{
						result = powerUpCategoryToVFX.m_vfxPrefab;
						break;
					}
				}
			}
			return result;
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
			PowerupTypeExtraParams powerupTypeExtraParams = extraSequenceParams as PowerupTypeExtraParams;
			if (powerupTypeExtraParams != null)
			{
				m_powerupCategoryInt = powerupTypeExtraParams.powerupCategory;
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	protected override GameObject GetProjectileFxPrefab()
	{
		GameObject gameObject = PowerUpCategoryToVFX.GetPrefabForCategory(m_powerupToProjectileVFXPrefab, m_powerupCategoryInt);
		if (gameObject == null)
		{
			gameObject = m_fxPrefab;
		}
		return gameObject;
	}

	protected override GameObject GetImpactFxPrefab()
	{
		GameObject gameObject = PowerUpCategoryToVFX.GetPrefabForCategory(m_powerupToImpactVFXPrefab, m_powerupCategoryInt);
		if (gameObject == null)
		{
			gameObject = m_fxImpactPrefab;
		}
		return gameObject;
	}
}
