using UnityEngine;

public class BazookaGirlDelayedMissileEffectSequence : Sequence
{
	public GameObject m_fxPrefab;

	private const float FLOOR_OFFSET = 0.12f;
	private GameObject m_fx;

	[AudioEvent(false)]
	public string m_audioEventApply;
	[AudioEvent(false)]
	public string m_audioEventFire = "ablty/bazookagirl/above_fire";

	protected override void OnStopVfxOnClient()
	{
		if (m_fx != null)
		{
			m_fx.SetActive(false);
		}
	}

	private void Update()
	{
		if (!m_fxPrefab || !m_initialized || m_fx != null || Caster == null)
		{
			return;
		}
		m_fx = InstantiateFX(m_fxPrefab);
		if (m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
		{
			m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(Caster.GetTeam());
		}
		m_fx.transform.position = TargetPos + Vector3.up * 0.12f;
		m_fx.transform.localRotation = Quaternion.identity;
	}

	private void OnDisable()
	{
		if (m_fx)
		{
			Destroy(m_fx);
		}
	}
}
