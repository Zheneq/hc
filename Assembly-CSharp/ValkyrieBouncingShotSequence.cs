using System.Collections.Generic;
using UnityEngine;

public class ValkyrieBouncingShotSequence : BouncingShotSequence
{
	[Header("-- Valkyrie specific stuff --")]
	public GameObject m_returnProjectileVfxPrefab;
	public float m_endingHeightOffset;
	[AudioEvent(false)]
	public string m_beginReturnAudioEvent;
	[AudioEvent(false)]
	public string m_endReturnAudioEvent;

	private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");

	protected override void UpdateProjectileFX()
	{
		int curSegment = m_curSegment;
		base.UpdateProjectileFX();
		if (m_fx == null || m_segmentPts.IsNullOrEmpty())
		{
			return;
		}
		Animator modelAnimator = Caster.GetModelAnimator();
		modelAnimator.SetFloat(animDistToGoal, m_totalTravelDistance - m_distanceTraveled);
		if (m_curSegment != curSegment && m_curSegment == m_segmentPts.Count - 2)
		{
			if (m_returnProjectileVfxPrefab != null)
			{
				bool sequenceVisibility = m_fx != null && m_fx.activeInHierarchy;
				Destroy(m_fx);
				Quaternion rotation = default(Quaternion);
				rotation.SetLookRotation((m_segmentPts[m_curSegment + 1] - m_segmentPts[m_curSegment]).normalized);
				m_fx = InstantiateFX(m_returnProjectileVfxPrefab, m_segmentPts[m_curSegment], rotation);
				SetSequenceVisibility(sequenceVisibility);
				if (m_fx != null && m_projectileFxAttributes != null)
				{
					foreach (KeyValuePair<string, float> projectileFxAttribute in m_projectileFxAttributes)
					{
						SetAttribute(m_fx, projectileFxAttribute.Key, projectileFxAttribute.Value);
					}
				}
			}
			else
			{
				SetAttribute(m_fx, "projectileReturning", 1);
			}
			if (!string.IsNullOrEmpty(m_beginReturnAudioEvent))
			{
				AudioManager.PostEvent(m_beginReturnAudioEvent, m_fx.gameObject);
			}
			Vector3 segmentPoint = m_segmentPts[m_segmentPts.Count - 1];
			segmentPoint.y += m_endingHeightOffset;
			m_segmentPts[m_segmentPts.Count - 1] = segmentPoint;
		}
		else if (m_reachedDestination && !string.IsNullOrEmpty(m_endReturnAudioEvent))
		{
			AudioManager.PostEvent(m_endReturnAudioEvent, Caster.gameObject);
		}
	}
}
