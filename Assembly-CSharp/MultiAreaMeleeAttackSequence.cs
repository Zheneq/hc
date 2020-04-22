using System.Collections.Generic;
using UnityEngine;

public class MultiAreaMeleeAttackSequence : Sequence
{
	[Tooltip("FX at point(s) of impact")]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitAlignedWithCaster = true;

	[AnimEventPicker]
	public Object m_leftHitEvent;

	[AnimEventPicker]
	public Object m_rightHitEvent;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	private List<GameObject> m_hitFx;

	private HashSet<ActorData> m_alreadyHit;

	private bool m_leftHitSpawned;

	private bool m_rightHitSpawned;

	private bool Finished()
	{
		bool result = false;
		if (m_leftHitSpawned)
		{
			if (m_rightHitSpawned)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						result = true;
						{
							foreach (GameObject item in m_hitFx)
							{
								if (item != null)
								{
									if (item.activeSelf)
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
								}
							}
							return result;
						}
					}
				}
			}
		}
		return result;
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			ProcessSequenceVisibility();
			if (Finished())
			{
				while (true)
				{
					MarkForRemoval();
					return;
				}
			}
			return;
		}
	}

	private void SpawnHitFX(bool left)
	{
		if (m_alreadyHit == null)
		{
			m_alreadyHit = new HashSet<ActorData>();
		}
		if (m_hitFx == null)
		{
			m_hitFx = new List<GameObject>();
		}
		int num;
		if (!m_leftHitSpawned)
		{
			num = (m_rightHitSpawned ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		if (left)
		{
			m_leftHitSpawned = true;
		}
		else
		{
			m_rightHitSpawned = true;
		}
		if (base.Targets != null)
		{
			for (int i = 0; i < base.Targets.Length; i++)
			{
				Vector3 lhs = base.Targets[i].transform.position - base.Caster.transform.position;
				lhs.y = 0f;
				float num2 = Vector3.Dot(lhs, base.Caster.transform.right);
				if (num2 > 0f)
				{
					if (!left)
					{
						goto IL_012d;
					}
				}
				if (num2 <= 0f)
				{
					if (left)
					{
						goto IL_012d;
					}
				}
				if (!flag)
				{
					continue;
				}
				goto IL_012d;
				IL_012d:
				if (m_alreadyHit.Contains(base.Targets[i]))
				{
					continue;
				}
				m_alreadyHit.Add(base.Targets[i]);
				Vector3 targetHitPosition = GetTargetHitPosition(i, m_hitFxJoint);
				Vector3 vector = targetHitPosition - base.Caster.transform.position;
				vector.y = 0f;
				vector.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
				Quaternion rotation = (!m_hitAlignedWithCaster) ? Quaternion.identity : Quaternion.LookRotation(vector);
				if ((bool)m_hitFxPrefab)
				{
					if (IsHitFXVisible(base.Targets[i]))
					{
						m_hitFx.Add(InstantiateFX(m_hitFxPrefab, targetHitPosition, rotation));
					}
				}
				if (!string.IsNullOrEmpty(m_hitAudioEvent))
				{
					AudioManager.PostEvent(m_hitAudioEvent, base.Targets[i].gameObject);
				}
				if (base.Targets[i] != null)
				{
					base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo);
				}
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos);
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_leftHitEvent == parameter)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					SpawnHitFX(true);
					return;
				}
			}
		}
		if (!(m_rightHitEvent == parameter))
		{
			return;
		}
		while (true)
		{
			SpawnHitFX(false);
			return;
		}
	}

	private void OnDisable()
	{
		if (m_hitFx == null)
		{
			return;
		}
		while (true)
		{
			using (List<GameObject>.Enumerator enumerator = m_hitFx.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					Object.Destroy(current.gameObject);
				}
			}
			m_hitFx = null;
			return;
		}
	}
}
