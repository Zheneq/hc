using System;
using UnityEngine;

public class ThiefPowerupReturnProjectileSequence : ArcingProjectileSequence
{
	[Header("-- Powerup to VFX mapping --")]
	public ThiefPowerupReturnProjectileSequence.PowerUpCategoryToVFX[] m_powerupToProjectileVFXPrefab;

	[Space(10f)]
	public ThiefPowerupReturnProjectileSequence.PowerUpCategoryToVFX[] m_powerupToImpactVFXPrefab;

	private int m_powerupCategoryInt = -1;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams powerupTypeExtraParams = extraSequenceParams as ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams;
			if (powerupTypeExtraParams != null)
			{
				this.m_powerupCategoryInt = powerupTypeExtraParams.powerupCategory;
			}
		}
	}

	protected override GameObject GetProjectileFxPrefab()
	{
		GameObject gameObject = ThiefPowerupReturnProjectileSequence.PowerUpCategoryToVFX.GetPrefabForCategory(this.m_powerupToProjectileVFXPrefab, this.m_powerupCategoryInt);
		if (gameObject == null)
		{
			gameObject = this.m_fxPrefab;
		}
		return gameObject;
	}

	protected override GameObject GetImpactFxPrefab()
	{
		GameObject gameObject = ThiefPowerupReturnProjectileSequence.PowerUpCategoryToVFX.GetPrefabForCategory(this.m_powerupToImpactVFXPrefab, this.m_powerupCategoryInt);
		if (gameObject == null)
		{
			gameObject = this.m_fxImpactPrefab;
		}
		return gameObject;
	}

	public class PowerupTypeExtraParams : Sequence.IExtraSequenceParams
	{
		public int powerupCategory;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.powerupCategory);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.powerupCategory);
		}
	}

	[Serializable]
	public class PowerUpCategoryToVFX
	{
		public PowerUp.PowerUpCategory m_category;

		public GameObject m_vfxPrefab;

		public static GameObject GetPrefabForCategory(ThiefPowerupReturnProjectileSequence.PowerUpCategoryToVFX[] catToVfx, int categoryInt)
		{
			GameObject result = null;
			if (catToVfx != null)
			{
				foreach (ThiefPowerupReturnProjectileSequence.PowerUpCategoryToVFX powerUpCategoryToVFX in catToVfx)
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
}
