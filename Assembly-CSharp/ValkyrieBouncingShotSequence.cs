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

	// Added in reactor
	//private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");

	// Changed in rouges
	protected override void UpdateProjectileFX()
	{
		int curSegment = m_curSegment;
		base.UpdateProjectileFX();
		if (m_fx != null && !m_segmentPts.IsNullOrEmpty<Vector3>())
		{
			base.Caster.GetModelAnimator().SetFloat("DistToGoal", m_totalTravelDistance - m_distanceTraveled);
			if (m_curSegment != curSegment && m_curSegment == m_segmentPts.Count - 2)
			{
				if (m_returnProjectileVfxPrefab != null)
				{
					bool sequenceVisibility = m_fx != null && m_fx.activeInHierarchy;
					Object.Destroy(m_fx);
					Quaternion rotation = default(Quaternion);
					rotation.SetLookRotation((m_segmentPts[m_curSegment + 1] - m_segmentPts[m_curSegment]).normalized);
					m_fx = base.InstantiateFX(m_returnProjectileVfxPrefab, m_segmentPts[m_curSegment], rotation, true);
					base.SetSequenceVisibility(sequenceVisibility);
					if (!(m_fx != null) || m_projectileFxAttributes == null)
					{
						goto IL_18D;
					}
					using (Dictionary<string, float>.Enumerator enumerator = m_projectileFxAttributes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, float> keyValuePair = enumerator.Current;
							SetAttribute(m_fx, keyValuePair.Key, keyValuePair.Value);
						}
						goto IL_18D;
					}
				}
				SetAttribute(m_fx, "projectileReturning", 1);
			IL_18D:
				if (!string.IsNullOrEmpty(m_beginReturnAudioEvent))
				{
					AudioManager.PostEvent(m_beginReturnAudioEvent, m_fx.gameObject);
				}
				Vector3 vector = m_segmentPts[m_segmentPts.Count - 1];
				vector.y += m_endingHeightOffset;
				m_segmentPts[m_segmentPts.Count - 1] = vector;
				return;
			}
			if (m_reachedDestination && !string.IsNullOrEmpty(m_endReturnAudioEvent))
			{
				AudioManager.PostEvent(m_endReturnAudioEvent, base.Caster.gameObject);
			}
		}
	}
}
