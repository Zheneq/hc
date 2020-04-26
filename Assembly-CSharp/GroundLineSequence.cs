using UnityEngine;

public class GroundLineSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public Vector3 startPos;

		public Vector3 endPos;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref startPos);
			stream.Serialize(ref endPos);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref startPos);
			stream.Serialize(ref endPos);
		}
	}

	private Vector3 m_startPos = Vector3.zero;

	private Vector3 m_endPos = Vector3.zero;

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	[Tooltip("Height above the ground to place the FX.")]
	public float m_heightOffset = 0.1f;

	[AudioEvent(false)]
	public string m_audioEvent;

	private GameObject m_fx;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
			{
				Vector3 b = new Vector3(0f, m_heightOffset, 0f);
				m_startPos = extraParams2.startPos + b;
				m_endPos = extraParams2.endPos + b;
			}
		}
	}

	public override void FinishSetup()
	{
		if (m_startEvent == null)
		{
			SpawnFX();
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (m_fx != null)
			{
				if (m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
				{
					m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
				}
			}
			Sequence.SetAttribute(m_fx, "startPoint", m_startPos);
			Sequence.SetAttribute(m_fx, "endPoint", m_endPos);
			return;
		}
	}

	private void SpawnFX()
	{
		if (m_fxPrefab != null)
		{
			Vector3 startPos = m_startPos;
			m_fx = InstantiateFX(m_fxPrefab, startPos, default(Quaternion));
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
			{
				Vector3 targetHitPosition = GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo);
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos);
		if (string.IsNullOrEmpty(m_audioEvent))
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
		{
			return;
		}
		while (true)
		{
			SpawnFX();
			return;
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
		}
	}

	protected override void OnStopVfxOnClient()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			m_fx.SetActive(false);
			return;
		}
	}
}
