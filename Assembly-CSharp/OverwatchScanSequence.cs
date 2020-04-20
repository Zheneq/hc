using System;
using UnityEngine;

public class OverwatchScanSequence : Sequence
{
	private Vector3 m_startPos = Vector3.zero;

	private Vector3 m_endPos = Vector3.zero;

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public Sequence.ReferenceModelType m_fxCasterJointReferenceType = Sequence.ReferenceModelType.TempSatellite;

	public OverwatchScanSequence.JointAttribute m_fxAttributeForJoint = OverwatchScanSequence.JointAttribute.TopPoint;

	public bool m_syncHeightOfEndToStart;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[Tooltip("Height above the ground to place the FX.")]
	public float m_heightOffset = 0.1f;

	[Tooltip("Distance in front of the startpoint to start")]
	public float m_startOffset = 0.5f;

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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(OverwatchScanSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				Vector3 b = new Vector3(0f, this.m_heightOffset, 0f);
				Vector3 a = extraParams2.endPos - extraParams2.startPos;
				a.Normalize();
				this.m_startPos = extraParams2.startPos + b + a * this.m_startOffset;
				this.m_endPos = extraParams2.endPos + b;
			}
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

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OverwatchScanSequence.FinishSetup()).MethodHandle;
			}
			this.SpawnFX();
		}
	}

	private void Update()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OverwatchScanSequence.Update()).MethodHandle;
			}
			base.ProcessSequenceVisibility();
			if (this.m_fx != null)
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
				if (this.m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
				{
					this.m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
				}
			}
			if (this.m_fxCasterJoint.IsInitialized())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_fxAttributeForJoint == OverwatchScanSequence.JointAttribute.StartPoint)
				{
					this.m_startPos = this.m_fxCasterJoint.m_jointObject.transform.position;
				}
				else if (this.m_fxAttributeForJoint == OverwatchScanSequence.JointAttribute.EndPoint)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_endPos = this.m_fxCasterJoint.m_jointObject.transform.position;
				}
				else if (this.m_fxAttributeForJoint == OverwatchScanSequence.JointAttribute.TopPoint)
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
					Sequence.SetAttribute(this.m_fx, "topPoint", this.m_fxCasterJoint.m_jointObject.transform.position);
				}
			}
			if (this.m_syncHeightOfEndToStart)
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
				this.m_endPos.y = this.m_startPos.y;
			}
			Sequence.SetAttribute(this.m_fx, "endPoint", this.m_endPos);
			Sequence.SetAttribute(this.m_fx, "startPoint", this.m_startPos);
		}
	}

	private void SpawnFX()
	{
		if (this.m_fxPrefab != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OverwatchScanSequence.SpawnFX()).MethodHandle;
			}
			Vector3 startPos = this.m_startPos;
			Quaternion rotation = default(Quaternion);
			this.m_fx = base.InstantiateFX(this.m_fxPrefab, startPos, rotation, true, true);
		}
		if (!this.m_fxCasterJoint.IsInitialized())
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
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_fxCasterJointReferenceType);
			if (referenceModel != null)
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
				this.m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
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
				Vector3 targetHitPosition = base.GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			}
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
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OverwatchScanSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SpawnFX();
		}
	}

	private void OnDisable()
	{
		if (this.m_fx != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OverwatchScanSequence.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
	}

	public enum JointAttribute
	{
		StartPoint,
		EndPoint,
		TopPoint
	}
}
