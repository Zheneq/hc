using UnityEngine;

public class SpoilsSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	public GameObject m_inaccessibleFxPrefab;

	public float m_projectileSpeed;

	public float m_projectileAcceleration = 0.25f;

	[AudioEvent(false)]
	public string m_audioEvent;

	public float m_startHeight = 1f;

	public float m_maxHeight = 4f;

	private GameObject m_fx;

	private CRSpline m_spline;

	private float m_splineSpeed;

	private float m_splineTraveled;

	private float m_curSplineSpeed;

	private float m_splineAcceleration;

	public Team m_pickupTeam = Team.Objects;

	private bool m_didSetFinalPos;

	private bool m_ignoreSpawnSpline;

	private void OnDisable()
	{
		if (m_fx != null)
		{
			Object.Destroy(m_fx);
			m_fx = null;
		}
		m_initialized = false;
	}

	private void SpawnFX()
	{
		Vector3[] array = new Vector3[5]
		{
			base.TargetPos + Vector3.down * (m_maxHeight - m_startHeight),
			base.TargetPos + Vector3.up * m_startHeight,
			base.TargetPos + Vector3.up * m_maxHeight,
			base.TargetPos,
			base.TargetPos + Vector3.down * m_maxHeight
		};
		m_spline = new CRSpline(array);
		float num = (array[1] - array[2]).magnitude + (array[2] - array[3]).magnitude;
		float num2 = num / m_projectileSpeed;
		m_splineSpeed = 1f / num2;
		m_splineAcceleration = m_projectileAcceleration * m_splineSpeed / m_projectileSpeed;
		bool flag;
		if (m_pickupTeam != Team.Objects)
		{
			if (!(GameFlowData.Get() == null))
			{
				if (!(GameFlowData.Get().activeOwnedActorData == null))
				{
					if (!(m_inaccessibleFxPrefab == null))
					{
						flag = (m_pickupTeam != GameFlowData.Get().activeOwnedActorData.GetTeam());
						goto IL_01f2;
					}
				}
			}
		}
		flag = false;
		goto IL_01f2;
		IL_01f2:
		GameObject prefab;
		if (flag)
		{
			prefab = m_inaccessibleFxPrefab;
		}
		else
		{
			prefab = m_fxPrefab;
		}
		m_fx = InstantiateFX(prefab, array[1], Quaternion.identity);
		if (string.IsNullOrEmpty(m_audioEvent))
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(m_audioEvent, (!base.Caster) ? null : base.Caster.gameObject);
			return;
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (m_fx == null)
			{
				SpawnFX();
			}
			else if (!m_didSetFinalPos)
			{
				if (!m_ignoreSpawnSpline)
				{
					m_curSplineSpeed += m_splineAcceleration;
					m_curSplineSpeed = Mathf.Min(m_splineSpeed, m_curSplineSpeed);
					m_splineTraveled += m_curSplineSpeed * GameTime.deltaTime;
					if (m_splineTraveled < 1f)
					{
						Vector3 position = m_spline.Interp(m_splineTraveled);
						m_fx.transform.position = position;
					}
					else
					{
						m_fx.transform.position = base.TargetPos;
						m_didSetFinalPos = true;
					}
				}
				else
				{
					m_fx.transform.position = base.TargetPos;
					m_didSetFinalPos = true;
				}
			}
			ProcessSequenceVisibility();
			return;
		}
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			PowerUp.ExtraParams extraParams2 = extraSequenceParams as PowerUp.ExtraParams;
			if (extraParams2 != null)
			{
				m_pickupTeam = (Team)extraParams2.m_pickupTeamAsInt;
				m_ignoreSpawnSpline = extraParams2.m_ignoreSpawnSpline;
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
