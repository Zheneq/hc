using System;
using System.Collections.Generic;
using UnityEngine;

public class HitActorGroupOnAnimEventSequence : Sequence
{
	[Separator("Arbitrary number to identify a relevant component, passed in by ability", true)]
	public int m_groupIdentidier = -1;

	[Separator("Anim Events", true)]
	[AnimEventPicker]
	public UnityEngine.Object m_hitAnimEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_lastHitAnimEvent;

	[Separator("Hit Vfx/Audio on Targets", true)]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitAlighedWithCaster;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	[Header("-- Team restrictions for Hit VFX on Targets --")]
	public Sequence.HitVFXSpawnTeam m_hitVfxSpawnTeamMode = Sequence.HitVFXSpawnTeam.EnemyOnly;

	private List<ActorData> m_actorsToHit;

	private List<GameObject> m_fxImpacts = new List<GameObject>();

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is HitActorGroupOnAnimEventSequence.ActorParams)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HitActorGroupOnAnimEventSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				HitActorGroupOnAnimEventSequence.ActorParams actorParams = extraSequenceParams as HitActorGroupOnAnimEventSequence.ActorParams;
				if (this.m_groupIdentidier == (int)actorParams.m_groupIdentifier)
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
					this.m_actorsToHit = new List<ActorData>(actorParams.m_hitActors);
				}
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitActorGroupOnAnimEventSequence.OnDisable()).MethodHandle;
			}
			this.m_fxImpacts.Clear();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_actorsToHit != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitActorGroupOnAnimEventSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			if (parameter == this.m_hitAnimEvent)
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
				bool flag = this.m_lastHitAnimEvent == null;
				for (int i = 0; i < this.m_actorsToHit.Count; i++)
				{
					this.SpawnImpactFXOnTarget(this.m_actorsToHit[i], flag);
				}
				if (flag)
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
					base.Source.OnSequenceHit(this, base.TargetPos, null);
				}
			}
			if (parameter == this.m_lastHitAnimEvent)
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
				for (int j = 0; j < this.m_actorsToHit.Count; j++)
				{
					this.SpawnImpactFXOnTarget(this.m_actorsToHit[j], true);
				}
				base.Source.OnSequenceHit(this, base.TargetPos, null);
			}
		}
	}

	private void SpawnImpactFXOnTarget(ActorData targetActor, bool lastHit)
	{
		Vector3 targetHitPosition = base.GetTargetHitPosition(targetActor, this.m_hitFxJoint);
		Vector3 vector = targetHitPosition - base.Caster.transform.position;
		vector.y = 0f;
		vector.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
		bool flag = this.IsHitFXVisibleForActor(targetActor);
		if (this.m_hitFxPrefab && flag)
		{
			Quaternion quaternion;
			if (this.m_hitAlighedWithCaster)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HitActorGroupOnAnimEventSequence.SpawnImpactFXOnTarget(ActorData, bool)).MethodHandle;
				}
				if (vector.magnitude > 0.001f)
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
					quaternion = Quaternion.LookRotation(vector);
					goto IL_A4;
				}
			}
			quaternion = Quaternion.identity;
			IL_A4:
			Quaternion rotation = quaternion;
			this.m_fxImpacts.Add(base.InstantiateFX(this.m_hitFxPrefab, targetHitPosition, rotation, true, true));
		}
		if (!string.IsNullOrEmpty(this.m_hitAudioEvent))
		{
			AudioManager.PostEvent(this.m_hitAudioEvent, targetActor.gameObject);
		}
		SequenceSource source = base.Source;
		ActorModelData.ImpulseInfo impulseInfo2 = impulseInfo;
		ActorModelData.RagdollActivation ragdollActivation;
		if (lastHit)
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
			ragdollActivation = ActorModelData.RagdollActivation.HealthBased;
		}
		else
		{
			ragdollActivation = ActorModelData.RagdollActivation.None;
		}
		source.OnSequenceHit(this, targetActor, impulseInfo2, ragdollActivation, true);
	}

	private bool IsHitFXVisibleForActor(ActorData hitTarget)
	{
		return base.IsHitFXVisibleWrtTeamFilter(hitTarget, this.m_hitVfxSpawnTeamMode);
	}

	public class ActorParams : Sequence.IExtraSequenceParams
	{
		public sbyte m_groupIdentifier;

		public List<ActorData> m_hitActors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_groupIdentifier);
			List<ActorData> hitActors = this.m_hitActors;
			sbyte b;
			if (hitActors != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HitActorGroupOnAnimEventSequence.ActorParams.XSP_SerializeToStream(IBitStream)).MethodHandle;
				}
				b = (sbyte)hitActors.Count;
			}
			else
			{
				b = 0;
			}
			sbyte b2 = b;
			stream.Serialize(ref b2);
			for (int i = 0; i < (int)b2; i++)
			{
				ActorData actorData = hitActors[i];
				sbyte b3;
				if (actorData != null)
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
					b3 = (sbyte)actorData.ActorIndex;
				}
				else
				{
					b3 = (sbyte)ActorData.s_invalidActorIndex;
				}
				sbyte b4 = b3;
				stream.Serialize(ref b4);
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

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_groupIdentifier);
			sbyte b = 0;
			stream.Serialize(ref b);
			this.m_hitActors = new List<ActorData>((int)b);
			for (int i = 0; i < (int)b; i++)
			{
				sbyte b2 = (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref b2);
				ActorData item = GameFlowData.Get().FindActorByActorIndex((int)b2);
				this.m_hitActors.Add(item);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitActorGroupOnAnimEventSequence.ActorParams.XSP_DeserializeFromStream(IBitStream)).MethodHandle;
			}
		}
	}
}
