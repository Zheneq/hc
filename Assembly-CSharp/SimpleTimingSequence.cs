using System;
using UnityEngine;

public class SimpleTimingSequence : Sequence
{
	[AnimEventPicker]
	[Tooltip("Animation Event to listen for to play the on hit")]
	public UnityEngine.Object m_startEvent;

	public float m_hitDelay;

	[AudioEvent(false)]
	public string m_onHitAudioEvent = string.Empty;

	private bool m_calledOnHitForNullStartEvent;

	private bool m_hitsDone;

	private float m_hitTime;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is SimpleTimingSequence.ExtraParams)
			{
				SimpleTimingSequence.ExtraParams extraParams2 = extraSequenceParams as SimpleTimingSequence.ExtraParams;
				if (extraParams2.hitDelayTime > 0f)
				{
					this.m_hitDelay = extraParams2.hitDelayTime;
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (this.m_startEvent == null)
			{
				if (!this.m_calledOnHitForNullStartEvent)
				{
					this.StartHits();
					this.m_calledOnHitForNullStartEvent = true;
				}
			}
			if (!this.m_hitsDone)
			{
				if (this.m_hitTime > 0f && GameTime.time > this.m_hitTime)
				{
					this.DoSequenceHits();
				}
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.StartHits();
		}
	}

	private void StartHits()
	{
		if (this.m_hitDelay <= 0f)
		{
			this.DoSequenceHits();
		}
		else if (this.m_hitTime == 0f)
		{
			this.m_hitTime = GameTime.time + this.m_hitDelay;
		}
	}

	protected virtual void DoSequenceHits()
	{
		base.Source.OnSequenceHit(this, base.TargetPos, null);
		if (base.Targets != null)
		{
			foreach (ActorData actorData in base.Targets)
			{
				base.Source.OnSequenceHit(this, actorData, Sequence.CreateImpulseInfoWithActorForward(actorData), ActorModelData.RagdollActivation.HealthBased, true);
			}
		}
		if (!string.IsNullOrEmpty(this.m_onHitAudioEvent))
		{
			GameObject gameObject = null;
			if (base.Caster != null)
			{
				gameObject = base.Caster.gameObject;
			}
			if (gameObject != null)
			{
				AudioManager.PostEvent(this.m_onHitAudioEvent, gameObject);
			}
		}
		this.m_hitsDone = true;
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public float hitDelayTime = -1f;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.hitDelayTime);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.hitDelayTime);
		}
	}
}
