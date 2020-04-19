using System;
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
		int curSegment = this.m_curSegment;
		base.UpdateProjectileFX();
		if (this.m_fx != null && !this.m_segmentPts.IsNullOrEmpty<Vector3>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieBouncingShotSequence.UpdateProjectileFX()).MethodHandle;
			}
			Animator animator = base.Caster.\u000E();
			animator.SetFloat(ValkyrieBouncingShotSequence.animDistToGoal, this.m_totalTravelDistance - this.m_distanceTraveled);
			if (this.m_curSegment != curSegment && this.m_curSegment == this.m_segmentPts.Count - 2)
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
				if (this.m_returnProjectileVfxPrefab != null)
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
					bool sequenceVisibility = this.m_fx != null && this.m_fx.activeInHierarchy;
					UnityEngine.Object.Destroy(this.m_fx);
					Quaternion rotation = default(Quaternion);
					rotation.SetLookRotation((this.m_segmentPts[this.m_curSegment + 1] - this.m_segmentPts[this.m_curSegment]).normalized);
					this.m_fx = base.InstantiateFX(this.m_returnProjectileVfxPrefab, this.m_segmentPts[this.m_curSegment], rotation, true, true);
					base.SetSequenceVisibility(sequenceVisibility);
					if (this.m_fx != null)
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
						if (this.m_projectileFxAttributes != null)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							foreach (KeyValuePair<string, float> keyValuePair in this.m_projectileFxAttributes)
							{
								Sequence.SetAttribute(this.m_fx, keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
				}
				else
				{
					Sequence.SetAttribute(this.m_fx, "projectileReturning", 1);
				}
				if (!string.IsNullOrEmpty(this.m_beginReturnAudioEvent))
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
					AudioManager.PostEvent(this.m_beginReturnAudioEvent, this.m_fx.gameObject);
				}
				Vector3 value = this.m_segmentPts[this.m_segmentPts.Count - 1];
				value.y += this.m_endingHeightOffset;
				this.m_segmentPts[this.m_segmentPts.Count - 1] = value;
			}
			else if (this.m_reachedDestination && !string.IsNullOrEmpty(this.m_endReturnAudioEvent))
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
				AudioManager.PostEvent(this.m_endReturnAudioEvent, base.Caster.gameObject);
			}
		}
	}
}
