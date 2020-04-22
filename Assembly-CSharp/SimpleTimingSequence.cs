using UnityEngine;

public class SimpleTimingSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public float hitDelayTime = -1f;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref hitDelayTime);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref hitDelayTime);
		}
	}

	[AnimEventPicker]
	[Tooltip("Animation Event to listen for to play the on hit")]
	public Object m_startEvent;

	public float m_hitDelay;

	[AudioEvent(false)]
	public string m_onHitAudioEvent = string.Empty;

	private bool m_calledOnHitForNullStartEvent;

	private bool m_hitsDone;

	private float m_hitTime;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (!(extraSequenceParams is ExtraParams))
			{
				continue;
			}
			while (true)
			{
				switch (6)
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
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2.hitDelayTime > 0f)
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
				m_hitDelay = extraParams2.hitDelayTime;
			}
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		if (m_startEvent == null)
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
			if (!m_calledOnHitForNullStartEvent)
			{
				StartHits();
				m_calledOnHitForNullStartEvent = true;
			}
		}
		if (m_hitsDone)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (m_hitTime > 0f && GameTime.time > m_hitTime)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					DoSequenceHits();
					return;
				}
			}
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			StartHits();
		}
	}

	private void StartHits()
	{
		if (m_hitDelay <= 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					DoSequenceHits();
					return;
				}
			}
		}
		if (m_hitTime == 0f)
		{
			m_hitTime = GameTime.time + m_hitDelay;
		}
	}

	protected virtual void DoSequenceHits()
	{
		base.Source.OnSequenceHit(this, base.TargetPos);
		if (base.Targets != null)
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
			ActorData[] targets = base.Targets;
			foreach (ActorData actorData in targets)
			{
				base.Source.OnSequenceHit(this, actorData, Sequence.CreateImpulseInfoWithActorForward(actorData));
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!string.IsNullOrEmpty(m_onHitAudioEvent))
		{
			while (true)
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
				while (true)
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
				AudioManager.PostEvent(m_onHitAudioEvent, gameObject);
			}
		}
		m_hitsDone = true;
	}
}
