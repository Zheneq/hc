using System.Collections.Generic;
using UnityEngine;

public class GenericSequenceProjectileInfo
{
	public Sequence m_parentSequence;

	public GenericSequenceProjectileAuthoredInfo m_authoredInfo;

	public Vector3 m_startPos = Vector3.zero;

	public Vector3 m_endPos = Vector3.zero;

	public ActorData[] m_targetActors;

	public bool m_finished;

	public float m_startDelay;

	public GameObject m_fx;

	public GameObject m_fxImpact;

	public List<GameObject> m_targetHitFx = new List<GameObject>();

	public Vector3 m_positionForSequenceHit;

	private float m_impactDuration;

	private float m_impactDurationLeft;

	private CRSpline m_spline;

	private float m_curSplineSpeed;

	private float m_splineSpeed;

	private float m_splineAcceleration;

	private float m_splineTraveled;

	private List<ActorData> m_actorsAlreadyHit = new List<ActorData>();

	public GenericSequenceProjectileInfo(Sequence parent, GenericSequenceProjectileAuthoredInfo authoredInfo, Vector3 startPos, Vector3 endPos, ActorData[] targetActors)
	{
		m_parentSequence = parent;
		m_authoredInfo = authoredInfo;
		m_impactDuration = Sequence.GetFXDuration(m_authoredInfo.m_fxImpactPrefab);
		m_startPos = startPos;
		m_endPos = endPos;
		m_positionForSequenceHit = endPos;
		m_targetActors = targetActors;
	}

	public void OnSequenceDisable()
	{
		if (m_fx != null)
		{
			Object.Destroy(m_fx);
			m_fx = null;
		}
		if (m_fxImpact != null)
		{
			Object.Destroy(m_fxImpact);
			m_fxImpact = null;
		}
		if (m_targetHitFx == null || m_targetHitFx.Count <= 0)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_targetHitFx.Count; i++)
			{
				Object.Destroy(m_targetHitFx[i]);
			}
			m_targetHitFx.Clear();
			return;
		}
	}

	private void SpawnFX()
	{
		Vector3[] splinePath = GetSplinePath();
		m_spline = new CRSpline(splinePath);
		Vector3 a = m_spline.Interp(0.05f);
		(a - splinePath[1]).Normalize();
		Quaternion rotation = default(Quaternion);
		Debug.DrawLine(m_startPos, m_endPos, Color.red, 5f);
		float num = (splinePath[1] - splinePath[2]).magnitude + (splinePath[2] - splinePath[3]).magnitude;
		float num2 = num / m_authoredInfo.m_projectileSpeed;
		m_splineSpeed = 1f / num2;
		m_splineAcceleration = 0f;
		m_curSplineSpeed = m_splineSpeed;
		m_fx = m_parentSequence.InstantiateFX(m_authoredInfo.m_fxPrefab, splinePath[1], rotation, true, false);
		if (!string.IsNullOrEmpty(m_authoredInfo.m_spawnAudioEvent))
		{
			AudioManager.PostEvent(m_authoredInfo.m_spawnAudioEvent, m_parentSequence.Caster.gameObject);
		}
	}

	public void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		if ((bool)m_authoredInfo.m_fxImpactPrefab)
		{
			m_fxImpact = m_parentSequence.InstantiateFX(m_authoredInfo.m_fxImpactPrefab, impactPos, impactRot);
			m_impactDurationLeft = m_impactDuration;
		}
		if (!string.IsNullOrEmpty(m_authoredInfo.m_impactAudioEvent))
		{
			AudioManager.PostEvent(m_authoredInfo.m_impactAudioEvent, m_fx.gameObject);
		}
	}

	protected virtual void SpawnTargetHitFx(ActorData target)
	{
		if (!(target != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_fx != null))
			{
				return;
			}
			while (true)
			{
				if (!m_parentSequence.IsHitFXVisibleWrtTeamFilter(target, m_authoredInfo.m_hitFxTeamFilter))
				{
					return;
				}
				if (m_authoredInfo.m_targetHitFxPrefab != null)
				{
					GameObject gameObject = m_authoredInfo.m_hitPosJoint.FindJointObject(target.gameObject);
					Vector3 vector;
					if (gameObject != null)
					{
						vector = gameObject.transform.position;
					}
					else
					{
						vector = target.GetTravelBoardSquareWorldPosition();
					}
					Vector3 position = vector;
					GameObject gameObject2 = m_parentSequence.InstantiateFX(m_authoredInfo.m_targetHitFxPrefab, position, m_fx.transform.rotation);
					if (gameObject2 != null)
					{
						if (m_authoredInfo.m_targetHitFxAttachToJoint)
						{
							m_parentSequence.AttachToBone(gameObject2, gameObject);
							gameObject2.transform.localPosition = Vector3.zero;
							gameObject2.transform.localScale = Vector3.one;
							gameObject2.transform.localRotation = Quaternion.identity;
						}
						else
						{
							gameObject2.transform.parent = m_parentSequence.transform;
						}
						m_targetHitFx.Add(gameObject2);
					}
				}
				if (!string.IsNullOrEmpty(m_authoredInfo.m_targetHitAudioEvent))
				{
					AudioManager.PostEvent(m_authoredInfo.m_targetHitAudioEvent, target.gameObject);
				}
				return;
			}
		}
	}

	public Vector3[] GetSplinePath()
	{
		Vector3 startPos = m_startPos;
		Vector3[] array = new Vector3[5];
		if (m_authoredInfo.m_maxHeight == 0f)
		{
			Vector3 endPos = m_endPos;
			endPos.y += m_authoredInfo.m_yOffset;
			Vector3 b = endPos - startPos;
			array[0] = startPos - b;
			array[1] = startPos;
			array[2] = (startPos + endPos) * 0.5f;
			array[3] = endPos;
			array[4] = endPos + b;
		}
		else
		{
			Vector3 endPos2 = m_endPos;
			array[0] = startPos + Vector3.down * m_authoredInfo.m_maxHeight;
			array[1] = startPos;
			array[2] = (startPos + endPos2) * 0.5f + Vector3.up * m_authoredInfo.m_maxHeight;
			array[3] = endPos2;
			array[4] = endPos2 + Vector3.down * m_authoredInfo.m_maxHeight;
		}
		if (m_authoredInfo.m_reverseDirection)
		{
			Vector3 vector = array[0];
			array[0] = array[4];
			array[4] = vector;
			vector = array[1];
			array[1] = array[3];
			array[3] = vector;
		}
		return array;
	}

	public void OnUpdate()
	{
		if (m_fx == null)
		{
			m_startDelay -= GameTime.deltaTime;
			if (!(m_startDelay <= 0f))
			{
				return;
			}
			while (true)
			{
				GameObject referenceModel = m_parentSequence.GetReferenceModel(m_parentSequence.Caster, m_authoredInfo.m_jointReferenceType);
				if (referenceModel != null)
				{
					m_authoredInfo.m_fxJoint.Initialize(referenceModel);
				}
				SpawnFX();
				return;
			}
		}
		if (m_fx.activeSelf)
		{
			m_curSplineSpeed += m_splineAcceleration;
			m_curSplineSpeed = Mathf.Min(m_splineSpeed, m_curSplineSpeed);
			m_splineTraveled += m_curSplineSpeed * GameTime.deltaTime;
			if (m_splineTraveled < m_authoredInfo.m_splineFractionUntilImpact)
			{
				Vector3 vector = m_spline.Interp(m_splineTraveled);
				Quaternion rotation = default(Quaternion);
				rotation.SetLookRotation((vector - m_fx.transform.position).normalized);
				Vector3 vector2 = vector - m_fx.transform.position;
				vector2.Normalize();
				m_fx.transform.position = vector;
				m_fx.transform.rotation = rotation;
				if (m_targetActors != null)
				{
					ActorData[] targetActors = m_targetActors;
					foreach (ActorData actorData in targetActors)
					{
						if (!(actorData != null))
						{
							continue;
						}
						if (m_actorsAlreadyHit.Contains(actorData))
						{
							continue;
						}
						Vector3 rhs = actorData.transform.position - m_fx.transform.position;
						if (Vector3.Dot(vector2, rhs) < 0f)
						{
							Vector3 position = m_fx.transform.position;
							ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, vector2);
							m_parentSequence.Source.OnSequenceHit(m_parentSequence, actorData, impulseInfo);
							m_actorsAlreadyHit.Add(actorData);
							SpawnTargetHitFx(actorData);
						}
					}
				}
			}
			else
			{
				if (m_authoredInfo.m_spawnImpactAtFXDespawn)
				{
					SpawnImpactFX(m_fx.transform.position, m_fx.transform.rotation);
				}
				else
				{
					SpawnImpactFX(m_endPos, Quaternion.identity);
				}
				m_fx.SetActive(false);
				m_finished = true;
				if (m_targetActors != null)
				{
					ActorData[] targetActors2 = m_targetActors;
					foreach (ActorData actorData2 in targetActors2)
					{
						if (!m_actorsAlreadyHit.Contains(actorData2))
						{
							m_parentSequence.Source.OnSequenceHit(m_parentSequence, actorData2, Sequence.CreateImpulseInfoWithObjectPose(m_fx));
							SpawnTargetHitFx(actorData2);
						}
					}
				}
				m_parentSequence.Source.OnSequenceHit(m_parentSequence, m_positionForSequenceHit);
			}
		}
		if (!(m_fxImpact != null))
		{
			return;
		}
		while (true)
		{
			if (!m_fxImpact.activeSelf)
			{
				return;
			}
			if (m_impactDurationLeft > 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_impactDurationLeft -= GameTime.deltaTime;
						return;
					}
				}
			}
			m_finished = true;
			return;
		}
	}
}
