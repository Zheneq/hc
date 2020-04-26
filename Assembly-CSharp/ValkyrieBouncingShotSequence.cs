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
		if (!(m_fx != null) || m_segmentPts.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			Animator modelAnimator = base.Caster.GetModelAnimator();
			modelAnimator.SetFloat(animDistToGoal, m_totalTravelDistance - m_distanceTraveled);
			if (m_curSegment != curSegment && m_curSegment == m_segmentPts.Count - 2)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						if (m_returnProjectileVfxPrefab != null)
						{
							bool sequenceVisibility = m_fx != null && m_fx.activeInHierarchy;
							Object.Destroy(m_fx);
							Quaternion rotation = default(Quaternion);
							rotation.SetLookRotation((m_segmentPts[m_curSegment + 1] - m_segmentPts[m_curSegment]).normalized);
							m_fx = InstantiateFX(m_returnProjectileVfxPrefab, m_segmentPts[m_curSegment], rotation);
							SetSequenceVisibility(sequenceVisibility);
							if (m_fx != null)
							{
								if (m_projectileFxAttributes != null)
								{
									foreach (KeyValuePair<string, float> projectileFxAttribute in m_projectileFxAttributes)
									{
										Sequence.SetAttribute(m_fx, projectileFxAttribute.Key, projectileFxAttribute.Value);
									}
								}
							}
						}
						else
						{
							Sequence.SetAttribute(m_fx, "projectileReturning", 1);
						}
						if (!string.IsNullOrEmpty(m_beginReturnAudioEvent))
						{
							AudioManager.PostEvent(m_beginReturnAudioEvent, m_fx.gameObject);
						}
						Vector3 value = m_segmentPts[m_segmentPts.Count - 1];
						value.y += m_endingHeightOffset;
						m_segmentPts[m_segmentPts.Count - 1] = value;
						return;
					}
					}
				}
			}
			if (m_reachedDestination && !string.IsNullOrEmpty(m_endReturnAudioEvent))
			{
				while (true)
				{
					AudioManager.PostEvent(m_endReturnAudioEvent, base.Caster.gameObject);
					return;
				}
			}
			return;
		}
	}
}
