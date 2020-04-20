using System;
using System.Collections.Generic;
using UnityEngine;

public class NinjaSetRotationToTargetSequence : Sequence
{
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

	public NinjaSetRotationToTargetSequence.RotateHitMode m_rotateHitMode;

	private List<NinjaSetRotationToTargetSequence.AngleToActor> m_angleToTargetActor = new List<NinjaSetRotationToTargetSequence.AngleToActor>();

	public override void FinishSetup()
	{
		int num = 0;
		this.m_angleToTargetActor.Clear();
		if (base.Caster != null && base.Targets != null)
		{
			Vector3 travelBoardSquareWorldPosition = base.Caster.GetTravelBoardSquareWorldPosition();
			for (int i = 0; i < base.Targets.Length; i++)
			{
				ActorData actorData = base.Targets[i];
				if (actorData != null)
				{
					Vector3 travelBoardSquareWorldPosition2 = actorData.GetTravelBoardSquareWorldPosition();
					if (actorData != base.Caster)
					{
						num++;
						Vector3 vec = travelBoardSquareWorldPosition2 - travelBoardSquareWorldPosition;
						vec.y = 0f;
						float horizontalAngle = VectorUtils.HorizontalAngle_Deg(vec);
						this.m_angleToTargetActor.Add(new NinjaSetRotationToTargetSequence.AngleToActor(horizontalAngle, actorData));
					}
					else
					{
						this.m_casterInTargetsList = true;
					}
				}
			}
			this.m_angleToTargetActor.Sort();
		}
		if (this.m_rotateHitMode == NinjaSetRotationToTargetSequence.RotateHitMode.RotateAndHitEachEvent)
		{
			this.m_currRotateTargetIndex = -1;
		}
		else
		{
			if (num > 0 && this.m_expectedRotateSignalCount > 0)
			{
				this.m_signalEventPerRotation = Mathf.Max(1, Mathf.RoundToInt((float)this.m_expectedRotateSignalCount / (float)num));
			}
			else
			{
				this.m_signalEventPerRotation = 1;
			}
			int j = 0;
			while (j < this.m_angleToTargetActor.Count)
			{
				if (j != this.m_angleToTargetActor.Count - 1)
				{
					goto IL_187;
				}
				if (this.m_expectedRotateSignalCount <= 0)
				{
					goto IL_187;
				}
				this.m_angleToTargetActor[j].m_lastHitOnEventNum = this.m_expectedRotateSignalCount;
				IL_1A6:
				j++;
				continue;
				IL_187:
				this.m_angleToTargetActor[j].m_lastHitOnEventNum = (j + 1) * this.m_signalEventPerRotation;
				goto IL_1A6;
			}
		}
		if (this.m_setParamForChargeEndAnim)
		{
			if (!string.IsNullOrEmpty(this.m_chargeEndParamName))
			{
				Animator modelAnimator = base.Caster.GetModelAnimator();
				if (modelAnimator != null)
				{
					int num2;
					if (num > 0)
					{
						num2 = 1;
					}
					else
					{
						num2 = 0;
					}
					int value = num2;
					modelAnimator.SetInteger(this.m_chargeEndParamName, value);
					if (num == 0)
					{
						if (this.m_casterInTargetsList)
						{
							this.DoGameplayHitsOnCaster();
						}
					}
				}
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_fxImpacts != null)
		{
			for (int i = 0; i < this.m_fxImpacts.Count; i++)
			{
				if (this.m_fxImpacts[i] != null)
				{
					UnityEngine.Object.Destroy(this.m_fxImpacts[i].gameObject);
				}
			}
			this.m_fxImpacts.Clear();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_rotateSignalAnimEvent && base.Caster != null)
		{
			if (this.m_rotateHitMode == NinjaSetRotationToTargetSequence.RotateHitMode.RotateAndHitEachEvent)
			{
				if (this.m_angleToTargetActor.Count > 0)
				{
					this.m_currRotateTargetIndex++;
					this.m_currRotateTargetIndex %= this.m_angleToTargetActor.Count;
					ActorData actor = this.m_angleToTargetActor[this.m_currRotateTargetIndex].m_actor;
					base.Caster.TurnToPositionInstant(actor.GetTravelBoardSquareWorldPosition());
				}
			}
			else
			{
				int num = this.m_numSignalEventsReceived / this.m_signalEventPerRotation;
				if (num > this.m_currRotateTargetIndex)
				{
					if (this.m_currRotateTargetIndex >= 0)
					{
						int i = Mathf.Max(0, this.m_lastGameplayHitTargetIndex);
						while (i < this.m_currRotateTargetIndex)
						{
							if (i >= this.m_angleToTargetActor.Count)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									goto IL_13F;
								}
							}
							else
							{
								this.SpawnImpactFXOnTarget(this.m_angleToTargetActor[i].m_actor, true);
								this.m_angleToTargetActor[i].m_didLastHit = true;
								i++;
							}
						}
					}
					IL_13F:
					this.m_currRotateTargetIndex = num;
					if (this.m_currRotateTargetIndex < this.m_angleToTargetActor.Count)
					{
						ActorData actor2 = this.m_angleToTargetActor[this.m_currRotateTargetIndex].m_actor;
						base.Caster.TurnToPositionInstant(actor2.GetTravelBoardSquareWorldPosition());
					}
				}
			}
			this.m_numSignalEventsReceived++;
		}
		if (parameter == this.m_hitAnimEvent)
		{
			if (this.m_currRotateTargetIndex >= 0 && this.m_currRotateTargetIndex < this.m_angleToTargetActor.Count)
			{
				this.m_numHitEventsReceived++;
				if (this.m_rotateHitMode == NinjaSetRotationToTargetSequence.RotateHitMode.RotateAndHitEachEvent)
				{
					bool flag = this.m_numHitEventsReceived > this.m_expectedRotateSignalCount - this.m_angleToTargetActor.Count;
					ActorData actor3 = this.m_angleToTargetActor[this.m_currRotateTargetIndex].m_actor;
					this.SpawnImpactFXOnTarget(actor3, flag);
					if (flag)
					{
						this.m_angleToTargetActor[this.m_currRotateTargetIndex].m_didLastHit = true;
					}
				}
				else
				{
					bool flag2 = this.m_angleToTargetActor[this.m_currRotateTargetIndex].m_lastHitOnEventNum == this.m_numHitEventsReceived;
					ActorData actor4 = this.m_angleToTargetActor[this.m_currRotateTargetIndex].m_actor;
					this.SpawnImpactFXOnTarget(actor4, flag2);
					if (flag2)
					{
						this.m_lastGameplayHitTargetIndex = this.m_currRotateTargetIndex;
						this.m_angleToTargetActor[this.m_currRotateTargetIndex].m_didLastHit = true;
					}
				}
			}
		}
		if (parameter == this.m_lastHitAnimEvent)
		{
			this.DoGameplayHitsOnCaster();
			if (this.m_rotateHitMode == NinjaSetRotationToTargetSequence.RotateHitMode.RotateAndHitEachEvent)
			{
				for (int j = 0; j < this.m_angleToTargetActor.Count; j++)
				{
					if (!this.m_angleToTargetActor[j].m_didLastHit)
					{
						this.SpawnImpactFXOnTarget(this.m_angleToTargetActor[j].m_actor, true);
					}
				}
			}
			else
			{
				for (int k = this.m_lastGameplayHitTargetIndex + 1; k < this.m_angleToTargetActor.Count; k++)
				{
					ActorData actor5 = this.m_angleToTargetActor[k].m_actor;
					this.SpawnImpactFXOnTarget(actor5, true);
				}
			}
		}
	}

	private void DoGameplayHitsOnCaster()
	{
		if (this.m_doGameplayHits)
		{
			base.Source.OnSequenceHit(this, base.TargetPos, null);
			if (this.m_casterInTargetsList)
			{
				base.Source.OnSequenceHit(this, base.Caster, Sequence.CreateImpulseInfoWithActorForward(base.Caster), ActorModelData.RagdollActivation.HealthBased, true);
			}
		}
	}

	private void SpawnImpactFXOnTarget(ActorData targetActor, bool lastHit)
	{
		Vector3 targetHitPosition = base.GetTargetHitPosition(targetActor, this.m_hitFxJoint);
		Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
		hitDirection.y = 0f;
		hitDirection.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
		if (this.m_hitFxPrefab)
		{
			this.m_fxImpacts.Add(base.InstantiateFX(this.m_hitFxPrefab, targetHitPosition, Quaternion.identity, true, true));
		}
		if (!string.IsNullOrEmpty(this.m_hitAudioEvent))
		{
			AudioManager.PostEvent(this.m_hitAudioEvent, targetActor.gameObject);
		}
		if (this.m_doGameplayHits)
		{
			base.Source.OnSequenceHit(this, targetActor, impulseInfo, (!lastHit) ? ActorModelData.RagdollActivation.None : ActorModelData.RagdollActivation.HealthBased, true);
		}
	}

	public enum RotateHitMode
	{
		RotateAndHitEachEvent,
		InBursts
	}

	private class AngleToActor : IComparable<NinjaSetRotationToTargetSequence.AngleToActor>
	{
		public float m_angleWithHorizontal;

		public ActorData m_actor;

		public int m_lastHitOnEventNum;

		public bool m_didLastHit;

		public AngleToActor(float horizontalAngle, ActorData actor)
		{
			this.m_angleWithHorizontal = horizontalAngle;
			this.m_actor = actor;
		}

		public int CompareTo(NinjaSetRotationToTargetSequence.AngleToActor other)
		{
			if (other == null)
			{
				return 1;
			}
			return this.m_angleWithHorizontal.CompareTo(other.m_angleWithHorizontal);
		}
	}
}
