// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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
			return other != null
				? m_angleWithHorizontal.CompareTo(other.m_angleWithHorizontal)
				: 1;
		}
	}

	[Separator("Anim Event to let sequence consider rotating, depending on number of total expected events")]
	[AnimEventPicker]
	public Object m_rotateSignalAnimEvent;
	[AnimEventPicker]
	public Object m_hitAnimEvent;
	[AnimEventPicker]
	public Object m_lastHitAnimEvent;
	[Header("-- Expected number of rotation signal anim events. Use 0 if should try to rotate on every signal")]
	public int m_expectedRotateSignalCount;
	[Separator("Hit Vfx/Audio on Targets")]
	public GameObject m_hitFxPrefab;
	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;
	[AudioEvent(false)]
	public string m_hitAudioEvent;
	[Separator("Gameplay Hit")]
	public bool m_doGameplayHits = true;
	[Separator("Anim Param for Charge End Anim Selection, 1 if has target, 0 otherwise")]
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
		if (Caster != null && Targets != null)
		{
			Vector3 travelBoardSquareWorldPosition = Caster.GetFreePos();
			foreach (ActorData actorData in Targets)
			{
				if (actorData == null)
				{
					continue;
				}
				Vector3 travelBoardSquareWorldPosition2 = actorData.GetFreePos();
				if (actorData != Caster)
				{
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
				m_signalEventPerRotation = Mathf.Max(1, Mathf.RoundToInt(m_expectedRotateSignalCount / (float)num));
			}
			else
			{
				m_signalEventPerRotation = 1;
			}
			for (int j = 0; j < m_angleToTargetActor.Count; j++)
			{
				if (j == m_angleToTargetActor.Count - 1 && m_expectedRotateSignalCount > 0)
				{
					m_angleToTargetActor[j].m_lastHitOnEventNum = m_expectedRotateSignalCount;
				}
				else
				{
					m_angleToTargetActor[j].m_lastHitOnEventNum = (j + 1) * m_signalEventPerRotation;
				}
			}
		}
		if (m_setParamForChargeEndAnim && !string.IsNullOrEmpty(m_chargeEndParamName))
		{
			Animator modelAnimator = Caster.GetModelAnimator();
			if (modelAnimator != null)
			{
				int value = num > 0 ? 1 : 0;
				modelAnimator.SetInteger(m_chargeEndParamName, value);
				if (num == 0 && m_casterInTargetsList)
				{
					DoGameplayHitsOnCaster();
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
		foreach (GameObject fx in m_fxImpacts)
		{
			if (fx != null)
			{
				Destroy(fx.gameObject);
			}
		}
		m_fxImpacts.Clear();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (parameter == m_rotateSignalAnimEvent && Caster != null)
		{
			if (m_rotateHitMode == RotateHitMode.RotateAndHitEachEvent)
			{
				if (m_angleToTargetActor.Count > 0)
				{
					m_currRotateTargetIndex++;
					m_currRotateTargetIndex %= m_angleToTargetActor.Count;
					ActorData actor = m_angleToTargetActor[m_currRotateTargetIndex].m_actor;
					Caster.TurnToPositionInstant(actor.GetFreePos());
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
							break;
						}
					}
					m_currRotateTargetIndex = num;
					if (m_currRotateTargetIndex < m_angleToTargetActor.Count)
					{
						ActorData actor2 = m_angleToTargetActor[m_currRotateTargetIndex].m_actor;
						Caster.TurnToPositionInstant(actor2.GetFreePos());
					}
				}
			}
			m_numSignalEventsReceived++;
		}
		if (parameter == m_hitAnimEvent)
		{
			if (m_currRotateTargetIndex >= 0 && m_currRotateTargetIndex < m_angleToTargetActor.Count)
			{
				m_numHitEventsReceived++;
				if (m_rotateHitMode == RotateHitMode.RotateAndHitEachEvent)
				{
					bool flag = m_numHitEventsReceived > m_expectedRotateSignalCount - m_angleToTargetActor.Count;
					ActorData actor3 = m_angleToTargetActor[m_currRotateTargetIndex].m_actor;
					SpawnImpactFXOnTarget(actor3, flag);
					if (flag)
					{
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
						m_lastGameplayHitTargetIndex = m_currRotateTargetIndex;
						m_angleToTargetActor[m_currRotateTargetIndex].m_didLastHit = true;
					}
				}
			}
		}
		if (parameter == m_lastHitAnimEvent)
		{
			DoGameplayHitsOnCaster();
			if (m_rotateHitMode == RotateHitMode.RotateAndHitEachEvent)
			{
				foreach (AngleToActor angleToActor in m_angleToTargetActor)
				{
					if (!angleToActor.m_didLastHit)
					{
						SpawnImpactFXOnTarget(angleToActor.m_actor, true);
					}
				}
			}
			else
			{
				for (int k = m_lastGameplayHitTargetIndex + 1; k < m_angleToTargetActor.Count; k++)
				{
					ActorData actor5 = m_angleToTargetActor[k].m_actor;
					SpawnImpactFXOnTarget(actor5, true);
				}
			}
		}
	}

	private void DoGameplayHitsOnCaster()
	{
		if (m_doGameplayHits)
		{
			Source.OnSequenceHit(this, TargetPos);
			if (m_casterInTargetsList)
			{
				// reactor
				Source.OnSequenceHit(this, Caster, CreateImpulseInfoWithActorForward(Caster));
				// rogues
				// Source.OnSequenceHit(this, Caster, null);
			}
		}
	}

	private void SpawnImpactFXOnTarget(ActorData targetActor, bool lastHit)
	{
		Vector3 targetHitPosition = GetTargetHitPosition(targetActor, m_hitFxJoint);
		Vector3 hitDirection = targetHitPosition - Caster.transform.position;
		hitDirection.y = 0f;
		hitDirection.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
		if (m_hitFxPrefab != null)
		{
			m_fxImpacts.Add(InstantiateFX(m_hitFxPrefab, targetHitPosition, Quaternion.identity));
		}
		if (!string.IsNullOrEmpty(m_hitAudioEvent))
		{
			AudioManager.PostEvent(m_hitAudioEvent, targetActor.gameObject);
		}
		if (m_doGameplayHits)
		{
			Source.OnSequenceHit(
				this,
				targetActor,
				impulseInfo,
				lastHit
					? ActorModelData.RagdollActivation.HealthBased
					: ActorModelData.RagdollActivation.None);
		}
	}
}
