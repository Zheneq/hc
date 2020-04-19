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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProximityMineGroundSequence.ShowVFXForState()).MethodHandle;
			}
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ProximityMineGroundSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_explosionRadius = extraParams2.explosionRadius;
				this.m_visibleToEnemies = extraParams2.visibleToEnemies;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProximityMineGroundSequence.ShouldShowTriggerBorder()).MethodHandle;
			}
			return false;
		}
		bool flag = this.m_visibleToEnemies || GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.\u000E());
		bool flag2 = !GameFlowData.Get().IsOwnerTargeting();
		bool result;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProximityMineGroundSequence.ShouldShowExplosionBorder()).MethodHandle;
			}
			return false;
		}
		bool flag;
		if (!this.m_visibleToEnemies)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.\u000E());
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProximityMineGroundSequence.CanShow()).MethodHandle;
			}
			return false;
		}
		bool result;
		if (!this.m_visibleToEnemies)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.\u000E());
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProximityMineGroundSequence.Update()).MethodHandle;
			}
			if (this.m_initialized && GameFlowData.Get().LocalPlayerData != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.\u000E());
				Vector3 position = new Vector3(base.TargetPos.x, base.TargetPos.y + 0.1f, base.TargetPos.z);
				GameObject gameObject;
				if (flag)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					gameObject = this.m_nonEnemyBorderPrefab;
				}
				else
				{
					gameObject = this.m_enemyBorderPrefab;
				}
				GameObject gameObject2 = gameObject;
				if (gameObject2 != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_explosionBorder = base.InstantiateFX(gameObject2, position, Quaternion.identity, true, true);
					HighlightUtils.SetParticleSystemScale(this.m_explosionBorder, this.m_explosionRadius);
				}
				this.m_mineArmed = base.InstantiateFX(this.m_mineArmedPrefab, position, Quaternion.identity, true, true);
				if (this.m_mineArmed.GetComponent<FriendlyEnemyVFXSelector>() != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_mineArmed.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.\u000E());
				}
				AudioManager.PostEvent(this.m_audioEventArm, this.m_mineArmed.gameObject);
				this.m_createdVFX = true;
				this.ShowVFXForState();
				return;
			}
		}
		if (this.m_createdVFX && this.m_initialized)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProximityMineGroundSequence.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_explosionBorder);
			this.m_explosionBorder = null;
		}
		if (this.m_effectField != null)
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
			UnityEngine.Object.Destroy(this.m_effectField);
			this.m_effectField = null;
		}
		if (this.m_mineArmed != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
