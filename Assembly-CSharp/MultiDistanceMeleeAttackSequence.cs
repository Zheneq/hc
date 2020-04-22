using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiDistanceMeleeAttackSequence : Sequence
{
	[Serializable]
	public class EventToCondition
	{
		public UnityEngine.Object m_event;

		public float m_distanceFromCasterInSquares;
	}

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitAlignedWithCaster = true;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	[Header("-- Animation Events to Conditions --")]
	[AnimEventPicker]
	public List<EventToCondition> m_eventToConditions = new List<EventToCondition>();

	[AnimEventPicker]
	[Header(" -- Last Animation Event, to hit any remaining targets --")]
	public UnityEngine.Object m_lastHitEvent;

	private List<GameObject> m_hitFx;

	private HashSet<ActorData> m_alreadyHit;

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ProcessSequenceVisibility();
			return;
		}
	}

	private void SpawnHitFX(float withinDistanceInSquares)
	{
		if (m_alreadyHit == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_alreadyHit = new HashSet<ActorData>();
		}
		if (m_hitFx == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_hitFx = new List<GameObject>();
		}
		if (base.Targets == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			float num = withinDistanceInSquares * Board.Get().squareSize;
			for (int i = 0; i < base.Targets.Length; i++)
			{
				Vector3 vector = base.Targets[i].transform.position - base.Caster.transform.position;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (m_alreadyHit.Contains(base.Targets[i]))
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(num < 0f))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(magnitude <= num))
					{
						continue;
					}
				}
				m_alreadyHit.Add(base.Targets[i]);
				Vector3 targetHitPosition = GetTargetHitPosition(i, m_hitFxJoint);
				Vector3 vector2 = targetHitPosition - base.Caster.transform.position;
				vector2.y = 0f;
				vector2.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector2);
				Quaternion quaternion;
				if (m_hitAlignedWithCaster)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					quaternion = Quaternion.LookRotation(vector2);
				}
				else
				{
					quaternion = Quaternion.identity;
				}
				Quaternion rotation = quaternion;
				if ((bool)m_hitFxPrefab && IsHitFXVisible(base.Targets[i]))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_hitFx.Add(InstantiateFX(m_hitFxPrefab, targetHitPosition, rotation));
				}
				if (!string.IsNullOrEmpty(m_hitAudioEvent))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					AudioManager.PostEvent(m_hitAudioEvent, base.Targets[i].gameObject);
				}
				if (base.Targets[i] != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo);
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		for (int i = 0; i < m_eventToConditions.Count; i++)
		{
			EventToCondition eventToCondition = m_eventToConditions[i];
			if (eventToCondition.m_event == parameter)
			{
				SpawnHitFX(eventToCondition.m_distanceFromCasterInSquares);
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_lastHitEvent == parameter)
			{
				SpawnHitFX(-1f);
				base.Source.OnSequenceHit(this, base.TargetPos);
			}
			return;
		}
	}

	private void OnDisable()
	{
		if (m_hitFx != null)
		{
			foreach (GameObject item in m_hitFx)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			m_hitFx = null;
		}
	}
}
