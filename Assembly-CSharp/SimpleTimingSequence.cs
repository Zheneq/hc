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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleTimingSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				SimpleTimingSequence.ExtraParams extraParams2 = extraSequenceParams as SimpleTimingSequence.ExtraParams;
				if (extraParams2.hitDelayTime > 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleTimingSequence.Update()).MethodHandle;
				}
				if (!this.m_calledOnHitForNullStartEvent)
				{
					this.StartHits();
					this.m_calledOnHitForNullStartEvent = true;
				}
			}
			if (!this.m_hitsDone)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_hitTime > 0f && GameTime.time > this.m_hitTime)
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleTimingSequence.StartHits()).MethodHandle;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleTimingSequence.DoSequenceHits()).MethodHandle;
			}
			foreach (ActorData actorData in base.Targets)
			{
				base.Source.OnSequenceHit(this, actorData, Sequence.CreateImpulseInfoWithActorForward(actorData), ActorModelData.RagdollActivation.HealthBased, true);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!string.IsNullOrEmpty(this.m_onHitAudioEvent))
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
			GameObject gameObject = null;
			if (base.Caster != null)
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
