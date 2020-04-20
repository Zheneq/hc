using System;
using UnityEngine;

public class ProximityMineGroundSequence : Sequence
{
	private GameObject m_triggerBorder;

	private GameObject m_explosionBorder;

	private GameObject m_effectField;

	private GameObject m_mineArmed;

	public GameObject m_mineArmedPrefab;

	public GameObject m_enemyBorderPrefab;

	public GameObject m_nonEnemyBorderPrefab;

	internal float m_triggerRadius = 0.5f;

	internal float m_explosionRadius = 0.5f;

	public bool m_visibleToEnemies = true;

	[AudioEvent(false)]
	public string m_audioEventArm = "ablty/scoundrel/mine_activate";

	private bool m_createdVFX;

	private void ShowVFXForState()
	{
		bool active = this.CanShow();
		if (this.m_explosionBorder != null)
		{
			this.m_explosionBorder.SetActive(active);
		}
		this.m_mineArmed.SetActive(active);
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ProximityMineGroundSequence.ExtraParams extraParams2 = extraSequenceParams as ProximityMineGroundSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_explosionRadius = extraParams2.explosionRadius;
				this.m_visibleToEnemies = extraParams2.visibleToEnemies;
			}
		}
		this.m_triggerBorder = null;
		this.m_explosionBorder = null;
		this.m_effectField = null;
		this.m_createdVFX = false;
	}

	private bool ShouldShowTriggerBorder()
	{
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			return false;
		}
		bool flag = this.m_visibleToEnemies || GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam());
		bool flag2 = !GameFlowData.Get().IsOwnerTargeting();
		bool result;
		if (flag)
		{
			result = flag2;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private bool ShouldShowExplosionBorder()
	{
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			return false;
		}
		bool flag;
		if (!this.m_visibleToEnemies)
		{
			flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam());
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3 = !GameFlowData.Get().IsOwnerTargeting();
		bool result;
		if (flag2)
		{
			result = flag3;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private bool CanShow()
	{
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			return false;
		}
		bool result;
		if (!this.m_visibleToEnemies)
		{
			result = GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam());
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool ShouldShowEffectField()
	{
		return !GameFlowData.Get().IsOwnerTargeting();
	}

	private void Update()
	{
		if (!this.m_createdVFX)
		{
			if (this.m_initialized && GameFlowData.Get().LocalPlayerData != null)
			{
				bool flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam());
				Vector3 position = new Vector3(base.TargetPos.x, base.TargetPos.y + 0.1f, base.TargetPos.z);
				GameObject gameObject;
				if (flag)
				{
					gameObject = this.m_nonEnemyBorderPrefab;
				}
				else
				{
					gameObject = this.m_enemyBorderPrefab;
				}
				GameObject gameObject2 = gameObject;
				if (gameObject2 != null)
				{
					this.m_explosionBorder = base.InstantiateFX(gameObject2, position, Quaternion.identity, true, true);
					HighlightUtils.SetParticleSystemScale(this.m_explosionBorder, this.m_explosionRadius);
				}
				this.m_mineArmed = base.InstantiateFX(this.m_mineArmedPrefab, position, Quaternion.identity, true, true);
				if (this.m_mineArmed.GetComponent<FriendlyEnemyVFXSelector>() != null)
				{
					this.m_mineArmed.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
				}
				AudioManager.PostEvent(this.m_audioEventArm, this.m_mineArmed.gameObject);
				this.m_createdVFX = true;
				this.ShowVFXForState();
				return;
			}
		}
		if (this.m_createdVFX && this.m_initialized)
		{
			this.ShowVFXForState();
		}
	}

	private void OnDisable()
	{
		if (this.m_triggerBorder != null)
		{
			UnityEngine.Object.Destroy(this.m_triggerBorder);
			this.m_triggerBorder = null;
		}
		if (this.m_explosionBorder != null)
		{
			UnityEngine.Object.Destroy(this.m_explosionBorder);
			this.m_explosionBorder = null;
		}
		if (this.m_effectField != null)
		{
			UnityEngine.Object.Destroy(this.m_effectField);
			this.m_effectField = null;
		}
		if (this.m_mineArmed != null)
		{
			UnityEngine.Object.Destroy(this.m_mineArmed);
			this.m_mineArmed = null;
		}
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public float explosionRadius;

		public bool visibleToEnemies;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.explosionRadius);
			stream.Serialize(ref this.visibleToEnemies);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.explosionRadius);
			stream.Serialize(ref this.visibleToEnemies);
		}
	}
}
