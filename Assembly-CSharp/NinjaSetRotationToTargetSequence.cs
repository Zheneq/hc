using System;
using System.Collections.Generic;
using UnityEngine;

public class NinjaSetRotationToTargetSequence : Sequence
{
	public enum RotateHitMode
	{
		RotateAndHitEachEvent,
		InBursts
	}

	private class AngleToActor : IComparable<AngleToActor>
	{
		public float m_angleWithHorizontal;

		public ActorData m_actor;

		public int m_lastHitOnEventNum;

		public bool m_didLastHit;

		public AngleToActor(float horizontalAngle, ActorData actor)
		{
			m_angleWithHorizontal = horizontalAngle;
			m_actor = actor;
		}

		public int CompareTo(AngleToActor other)
		{
			if (other == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return 1;
					}
				}
			}
			return m_angleWithHorizontal.CompareTo(other.m_angleWithHorizontal);
		}
	}

	[Separator("Anim Event to let sequence consider rotating, depending on number of total expected events", true)]
	[AnimEventPicker]
	public UnityEngine.Object m_rotateSignalAnimEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_hitAnimEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_lastHitAnimEvent;

	[Header("-- Expected number of rotation signal anim events. Use 0 if should try to rotate on every signal")]
	public int m_expectedRotateSignalCount;

	[Separator("Hit Vfx/Audio on Targets", true)]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	[Separator("Gameplay Hit", true)]
	public bool m_doGameplayHits = true;

	[Separator("Anim Param for Charge End Anim Selection, 1 if has target, 0 otherwise", true)]
	public bool m_setParamForChargeEndAnim = true;

	public string m_chargeEndParamName = "UltimateHit";

	private int m_numSignalEventsReceived;

	private int m_signalEventPerRotation = 1;

	private bool m_casterInTargetsList;

	private int m_currRotateTargetIndex = -1;

	private int m_lastGameplayHitTargetIndex = -1;

	private int m_numHitEventsReceived;

	private List<GameObject> m_fxImpacts = new List<GameObject>();

	public RotateHitMode m_rotateHitMode;

	private List<AngleToActor> m_angleToTargetActor = new List<AngleToActor>();

	public override void FinishSetup()
	{
		int num = 0;
		m_angleToTargetActor.Clear();
		if (base.Caster != null && base.Targets != null)
		{
			Vector3 travelBoardSquareWorldPosition = base.Caster.GetTravelBoardSquareWorldPosition();
			for (int i = 0; i < base.Targets.Length; i++)
			{
				ActorData actorData = base.Targets[i];
				if (!(actorData != null))
				{
					continue;
				}
				while (true)
				{
					switch (4)
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
				Vector3 travelBoardSquareWorldPosition2 = actorData.GetTravelBoardSquareWorldPosition();
				if (actorData != base.Caster)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					num++;
					Vector3 vec = travelBoardSquareWorldPosition2 - travelBoardSquareWorldPosition;
					vec.y = 0f;
					float horizontalAngle = VectorUtils.HorizontalAngle_Deg(vec);
					m_angleToTargetActor.Add(new AngleToActor(horizontalAngle, actorData));
				}
				else
				{
					m_casterInTargetsList = true;
				}
			}
			m_angleToTargetActor.Sort();
		}
		if (m_rotateHitMode == RotateHitMode.RotateAndHitEachEvent)
		{
			m_currRotateTargetIndex = -1;
		}
		else
		{
			if (num > 0 && m_expectedRotateSignalCount > 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_signalEventPerRotation = Mathf.Max(1, Mathf.RoundToInt((float)m_expectedRotateSignalCount / (float)num));
			}
			else
			{
				m_signalEventPerRotation = 1;
			}
			for (int j = 0; j < m_angleToTargetActor.Count; j++)
			{
				if (j == m_angleToTargetActor.Count - 1)
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
					if (m_expectedRotateSignalCount > 0)
					{
						m_angleToTargetActor[j].m_lastHitOnEventNum = m_expectedRotateSignalCount;
						continue;
					}
				}
				m_angleToTargetActor[j].m_lastHitOnEventNum = (j + 1) * m_signalEventPerRotation;
			}
		}
		if (!m_setParamForChargeEndAnim)
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
			if (string.IsNullOrEmpty(m_chargeEndParamName))
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				Animator modelAnimator = base.Caster.GetModelAnimator();
				if (!(modelAnimator != null))
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					int num2;
					if (num > 0)
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
						num2 = 1;
					}
					else
					{
						num2 = 0;
					}
					int value = num2;
					modelAnimator.SetInteger(m_chargeEndParamName, value);
					if (num != 0)
					{
						return;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						if (m_casterInTargetsList)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								DoGameplayHitsOnCaster();
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	private void OnDisable()
	{
		if (m_fxImpacts == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_fxImpacts.Count; i++)
			{
				if (m_fxImpacts[i] != null)
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
					UnityEngine.Object.Destroy(m_fxImpacts[i].gameObject);
				}
			}
			m_fxImpacts.Clear();
			return;
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == m_rotateSignalAnimEvent && base.Caster != null)
		{
			while (true)
			{
				switch (3)
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
			if (m_rotateHitMode == RotateHitMode.RotateAndHitEachEvent)
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
				if (m_angleToTargetActor.Count > 0)
				{
					m_currRotateTargetIndex++;
					m_currRotateTargetIndex %= m_angleToTargetActor.Count;
					ActorData actor = m_angleToTargetActor[m_currRotateTargetIndex].m_actor;
					base.Caster.TurnToPositionInstant(actor.GetTravelBoardSquareWorldPosition());
				}
			}
			else
			{
				int num = m_numSignalEventsReceived / m_signalEventPerRotation;
				if (num > m_currRotateTargetIndex)
				{
					if (m_currRotateTargetIndex >= 0)
					{
						for (int i = Mathf.Max(0, m_lastGameplayHitTargetIndex); i < m_currRotateTargetIndex; i++)
						{
							if (i < m_angleToTargetActor.Count)
							{
								SpawnImpactFXOnTarget(m_angleToTargetActor[i].m_actor, true);
								m_angleToTargetActor[i].m_didLastHit = true;
								continue;
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
					}
					m_currRotateTargetIndex = num;
					if (m_currRotateTargetIndex < m_angleToTargetActor.Count)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						ActorData actor2 = m_angleToTargetActor[m_currRotateTargetIndex].m_actor;
						base.Caster.TurnToPositionInstant(actor2.GetTravelBoardSquareWorldPosition());
					}
				}
			}
			m_numSignalEventsReceived++;
		}
		if (parameter == m_hitAnimEvent)
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
			if (m_currRotateTargetIndex >= 0 && m_currRotateTargetIndex < m_angleToTargetActor.Count)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_numHitEventsReceived++;
				if (m_rotateHitMode == RotateHitMode.RotateAndHitEachEvent)
				{
					bool flag = m_numHitEventsReceived > m_expectedRotateSignalCount - m_angleToTargetActor.Count;
					ActorData actor3 = m_angleToTargetActor[m_currRotateTargetIndex].m_actor;
					SpawnImpactFXOnTarget(actor3, flag);
					if (flag)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						m_angleToTargetActor[m_currRotateTargetIndex].m_didLastHit = true;
					}
				}
				else
				{
					bool flag2 = m_angleToTargetActor[m_currRotateTargetIndex].m_lastHitOnEventNum == m_numHitEventsReceived;
					ActorData actor4 = m_angleToTargetActor[m_currRotateTargetIndex].m_actor;
					SpawnImpactFXOnTarget(actor4, flag2);
					if (flag2)
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
						m_lastGameplayHitTargetIndex = m_currRotateTargetIndex;
						m_angleToTargetActor[m_currRotateTargetIndex].m_didLastHit = true;
					}
				}
			}
		}
		if (!(parameter == m_lastHitAnimEvent))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			DoGameplayHitsOnCaster();
			if (m_rotateHitMode == RotateHitMode.RotateAndHitEachEvent)
			{
				for (int j = 0; j < m_angleToTargetActor.Count; j++)
				{
					if (!m_angleToTargetActor[j].m_didLastHit)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						SpawnImpactFXOnTarget(m_angleToTargetActor[j].m_actor, true);
					}
				}
				return;
			}
			for (int k = m_lastGameplayHitTargetIndex + 1; k < m_angleToTargetActor.Count; k++)
			{
				ActorData actor5 = m_angleToTargetActor[k].m_actor;
				SpawnImpactFXOnTarget(actor5, true);
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

	private void DoGameplayHitsOnCaster()
	{
		if (!m_doGameplayHits)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.Source.OnSequenceHit(this, base.TargetPos);
			if (m_casterInTargetsList)
			{
				base.Source.OnSequenceHit(this, base.Caster, Sequence.CreateImpulseInfoWithActorForward(base.Caster));
			}
			return;
		}
	}

	private void SpawnImpactFXOnTarget(ActorData targetActor, bool lastHit)
	{
		Vector3 targetHitPosition = GetTargetHitPosition(targetActor, m_hitFxJoint);
		Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
		hitDirection.y = 0f;
		hitDirection.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
		if ((bool)m_hitFxPrefab)
		{
			m_fxImpacts.Add(InstantiateFX(m_hitFxPrefab, targetHitPosition, Quaternion.identity));
		}
		if (!string.IsNullOrEmpty(m_hitAudioEvent))
		{
			AudioManager.PostEvent(m_hitAudioEvent, targetActor.gameObject);
		}
		if (m_doGameplayHits)
		{
			base.Source.OnSequenceHit(this, targetActor, impulseInfo, lastHit ? ActorModelData.RagdollActivation.HealthBased : ActorModelData.RagdollActivation.None);
		}
	}
}
