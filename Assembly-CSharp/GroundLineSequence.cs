using System;
using UnityEngine;

public class GroundLineSequence : Sequence
{
	private Vector3 m_startPos = Vector3.zero;

	private Vector3 m_endPos = Vector3.zero;

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[Tooltip("Height above the ground to place the FX.")]
	public float m_heightOffset = 0.1f;

	[AudioEvent(false)]
	public string m_audioEvent;

	private GameObject m_fx;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			GroundLineSequence.ExtraParams extraParams2 = extraSequenceParams as GroundLineSequence.ExtraParams;
			if (extraParams2 != null)
			{
				Vector3 b = new Vector3(0f, this.m_heightOffset, 0f);
				this.m_startPos = extraParams2.startPos + b;
				this.m_endPos = extraParams2.endPos + b;
			}
		}
	}

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
			this.SpawnFX();
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (this.m_fx != null)
			{
				if (this.m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
				{
					this.m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
				}
			}
			Sequence.SetAttribute(this.m_fx, "startPoint", this.m_startPos);
			Sequence.SetAttribute(this.m_fx, "endPoint", this.m_endPos);
		}
	}

	private void SpawnFX()
	{
		if (this.m_fxPrefab != null)
		{
			Vector3 startPos = this.m_startPos;
			Quaternion rotation = default(Quaternion);
			this.m_fx = base.InstantiateFX(this.m_fxPrefab, startPos, rotation, true, true);
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
			{
				Vector3 targetHitPosition = base.GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos, null);
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SpawnFX();
		}
	}

	private void OnDisable()
	{
		if (this.m_fx != null)
		{
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
	}

	protected override void OnStopVfxOnClient()
	{
		if (this.m_fx != null)
		{
			this.m_fx.SetActive(false);
		}
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public Vector3 startPos;

		public Vector3 endPos;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.startPos);
			stream.Serialize(ref this.endPos);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.startPos);
			stream.Serialize(ref this.endPos);
		}
	}
}
