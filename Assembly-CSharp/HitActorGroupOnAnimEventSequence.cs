using System.Collections.Generic;
using UnityEngine;

public class HitActorGroupOnAnimEventSequence : Sequence
{
	public class ActorParams : IExtraSequenceParams
	{
		public sbyte m_groupIdentifier;

		public List<ActorData> m_hitActors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref m_groupIdentifier);
			List<ActorData> hitActors = m_hitActors;
			int num;
			if (hitActors != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = (sbyte)hitActors.Count;
			}
			else
			{
				num = 0;
			}
			sbyte value = (sbyte)num;
			stream.Serialize(ref value);
			for (int i = 0; i < value; i++)
			{
				ActorData actorData = hitActors[i];
				int num2;
				if (actorData != null)
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
					num2 = (sbyte)actorData.ActorIndex;
				}
				else
				{
					num2 = (sbyte)ActorData.s_invalidActorIndex;
				}
				sbyte value2 = (sbyte)num2;
				stream.Serialize(ref value2);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref m_groupIdentifier);
			sbyte value = 0;
			stream.Serialize(ref value);
			m_hitActors = new List<ActorData>(value);
			for (int i = 0; i < value; i++)
			{
				sbyte value2 = (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref value2);
				ActorData item = GameFlowData.Get().FindActorByActorIndex(value2);
				m_hitActors.Add(item);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
	}

	[Separator("Arbitrary number to identify a relevant component, passed in by ability", true)]
	public int m_groupIdentidier = -1;

	[Separator("Anim Events", true)]
	[AnimEventPicker]
	public Object m_hitAnimEvent;

	[AnimEventPicker]
	public Object m_lastHitAnimEvent;

	[Separator("Hit Vfx/Audio on Targets", true)]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitAlighedWithCaster;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	[Header("-- Team restrictions for Hit VFX on Targets --")]
	public HitVFXSpawnTeam m_hitVfxSpawnTeamMode = HitVFXSpawnTeam.EnemyOnly;

	private List<ActorData> m_actorsToHit;

	private List<GameObject> m_fxImpacts = new List<GameObject>();

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (!(extraSequenceParams is ActorParams))
			{
				continue;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorParams actorParams = extraSequenceParams as ActorParams;
			if (m_groupIdentidier == actorParams.m_groupIdentifier)
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
				m_actorsToHit = new List<ActorData>(actorParams.m_hitActors);
			}
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

	private void OnDisable()
	{
		if (m_fxImpacts == null)
		{
			return;
		}
		for (int i = 0; i < m_fxImpacts.Count; i++)
		{
			if (m_fxImpacts[i] != null)
			{
				Object.Destroy(m_fxImpacts[i].gameObject);
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_fxImpacts.Clear();
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_actorsToHit == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (parameter == m_hitAnimEvent)
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
				bool flag = m_lastHitAnimEvent == null;
				for (int i = 0; i < m_actorsToHit.Count; i++)
				{
					SpawnImpactFXOnTarget(m_actorsToHit[i], flag);
				}
				if (flag)
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
					base.Source.OnSequenceHit(this, base.TargetPos);
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
				for (int j = 0; j < m_actorsToHit.Count; j++)
				{
					SpawnImpactFXOnTarget(m_actorsToHit[j], true);
				}
				base.Source.OnSequenceHit(this, base.TargetPos);
				return;
			}
		}
	}

	private void SpawnImpactFXOnTarget(ActorData targetActor, bool lastHit)
	{
		Vector3 targetHitPosition = GetTargetHitPosition(targetActor, m_hitFxJoint);
		Vector3 vector = targetHitPosition - base.Caster.transform.position;
		vector.y = 0f;
		vector.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
		bool flag = IsHitFXVisibleForActor(targetActor);
		Quaternion quaternion;
		if ((bool)m_hitFxPrefab && flag)
		{
			if (m_hitAlighedWithCaster)
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
				if (vector.magnitude > 0.001f)
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
					quaternion = Quaternion.LookRotation(vector);
					goto IL_00a4;
				}
			}
			quaternion = Quaternion.identity;
			goto IL_00a4;
		}
		goto IL_00c2;
		IL_00c2:
		if (!string.IsNullOrEmpty(m_hitAudioEvent))
		{
			AudioManager.PostEvent(m_hitAudioEvent, targetActor.gameObject);
		}
		SequenceSource source = base.Source;
		int ragdollActivation;
		if (lastHit)
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
			ragdollActivation = 1;
		}
		else
		{
			ragdollActivation = 0;
		}
		source.OnSequenceHit(this, targetActor, impulseInfo, (ActorModelData.RagdollActivation)ragdollActivation);
		return;
		IL_00a4:
		Quaternion rotation = quaternion;
		m_fxImpacts.Add(InstantiateFX(m_hitFxPrefab, targetHitPosition, rotation));
		goto IL_00c2;
	}

	private bool IsHitFXVisibleForActor(ActorData hitTarget)
	{
		return IsHitFXVisibleWrtTeamFilter(hitTarget, m_hitVfxSpawnTeamMode);
	}
}
