using UnityEngine;

public class ProximityMineGroundSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public float explosionRadius;

		public bool visibleToEnemies;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref explosionRadius);
			stream.Serialize(ref visibleToEnemies);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref explosionRadius);
			stream.Serialize(ref visibleToEnemies);
		}
	}

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
		bool active = CanShow();
		if (m_explosionBorder != null)
		{
			m_explosionBorder.SetActive(active);
		}
		m_mineArmed.SetActive(active);
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
			{
				m_explosionRadius = extraParams2.explosionRadius;
				m_visibleToEnemies = extraParams2.visibleToEnemies;
			}
		}
		while (true)
		{
			m_triggerBorder = null;
			m_explosionBorder = null;
			m_effectField = null;
			m_createdVFX = false;
			return;
		}
	}

	private bool ShouldShowTriggerBorder()
	{
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		bool flag = m_visibleToEnemies || GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam());
		bool flag2 = !GameFlowData.Get().IsOwnerTargeting();
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private bool ShouldShowExplosionBorder()
	{
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		int num;
		if (!m_visibleToEnemies)
		{
			num = (GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam()) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		bool flag2 = !GameFlowData.Get().IsOwnerTargeting();
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private bool CanShow()
	{
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		int result;
		if (!m_visibleToEnemies)
		{
			result = (GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam()) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool ShouldShowEffectField()
	{
		return !GameFlowData.Get().IsOwnerTargeting();
	}

	private void Update()
	{
		if (!m_createdVFX)
		{
			if (m_initialized && GameFlowData.Get().LocalPlayerData != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						bool flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(base.Caster.GetTeam());
						Vector3 targetPos = base.TargetPos;
						float x = targetPos.x;
						Vector3 targetPos2 = base.TargetPos;
						float y = targetPos2.y + 0.1f;
						Vector3 targetPos3 = base.TargetPos;
						Vector3 position = new Vector3(x, y, targetPos3.z);
						GameObject gameObject;
						if (flag)
						{
							gameObject = m_nonEnemyBorderPrefab;
						}
						else
						{
							gameObject = m_enemyBorderPrefab;
						}
						GameObject gameObject2 = gameObject;
						if (gameObject2 != null)
						{
							m_explosionBorder = InstantiateFX(gameObject2, position, Quaternion.identity);
							HighlightUtils.SetParticleSystemScale(m_explosionBorder, m_explosionRadius);
						}
						m_mineArmed = InstantiateFX(m_mineArmedPrefab, position, Quaternion.identity);
						if (m_mineArmed.GetComponent<FriendlyEnemyVFXSelector>() != null)
						{
							m_mineArmed.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
						}
						AudioManager.PostEvent(m_audioEventArm, m_mineArmed.gameObject);
						m_createdVFX = true;
						ShowVFXForState();
						return;
					}
					}
				}
			}
		}
		if (!m_createdVFX || !m_initialized)
		{
			return;
		}
		while (true)
		{
			ShowVFXForState();
			return;
		}
	}

	private void OnDisable()
	{
		if (m_triggerBorder != null)
		{
			Object.Destroy(m_triggerBorder);
			m_triggerBorder = null;
		}
		if (m_explosionBorder != null)
		{
			Object.Destroy(m_explosionBorder);
			m_explosionBorder = null;
		}
		if (m_effectField != null)
		{
			Object.Destroy(m_effectField);
			m_effectField = null;
		}
		if (!(m_mineArmed != null))
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_mineArmed);
			m_mineArmed = null;
			return;
		}
	}
}
