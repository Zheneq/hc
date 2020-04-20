using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExplosionSequence : Sequence
{
	public GameObject m_explosionPrefab;

	public GameObject m_hitPrefab;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	public float m_initialShotDelay = 0.25f;

	public float m_hitDelay = 0.3f;

	public bool m_scale = true;

	private float m_radius;

	[AudioEvent(false)]
	public string m_audioEventExplode = "ablty/bazookagirl/cluster_explode";

	[AudioEvent(false)]
	public string m_audioEventHitOnTargets = string.Empty;

	private GameObject m_explosion;

	private bool m_created;

	private bool m_hitsSpawned;

	private float m_duration;

	private float m_onHitTime;

	private float m_hitDuration;

	private float m_createTime;

	private Team m_team;

	private List<GameObject> m_hitEffects;

	protected override void Start()
	{
		base.Start();
		this.m_duration = 0f;
		this.m_created = false;
		this.m_hitsSpawned = false;
		this.m_hitEffects = new List<GameObject>();
		this.m_hitsSpawned = false;
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExplosionSequence.ExtraParams extraParams2 = extraSequenceParams as ExplosionSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_radius = extraParams2.radius;
				this.m_team = extraParams2.team;
			}
		}
	}

	private void Update()
	{
		base.ProcessSequenceVisibility();
		float time = GameTime.time;
		if (this.m_startEvent == null)
		{
			if (!this.m_created)
			{
				if (this.m_explosion == null)
				{
					if (time - this.m_startTime >= this.m_initialShotDelay)
					{
						if (this.m_initialized)
						{
							this.InstantiateExplosion();
						}
					}
				}
			}
		}
		if (this.m_created)
		{
			if (time - this.m_createTime >= this.m_hitDelay)
			{
				if (!this.m_hitsSpawned)
				{
					this.SpawnHits();
				}
			}
		}
		if (this.m_hitsSpawned)
		{
			if (time - this.m_onHitTime >= this.m_hitDuration + 1f)
			{
				if (time - this.m_createTime > this.m_duration + 1f)
				{
					if (NetworkServer.active)
					{
						base.MarkForRemoval();
					}
				}
			}
		}
	}

	private void SpawnHits()
	{
		this.m_onHitTime = GameTime.time;
		this.m_hitsSpawned = true;
		ActorData[] array = base.Targets;
		if (array != null)
		{
			if (array.Length != 0)
			{
				goto IL_7C;
			}
		}
		array = AreaEffectUtils.GetActorsInRadius(base.TargetPos, this.m_radius, true, base.Caster, GameplayUtils.GetOtherTeamsThan(this.m_team), null, false, default(Vector3)).ToArray();
		IL_7C:
		foreach (ActorData actorData in array)
		{
			Vector3 bonePosition = actorData.GetBonePosition("upperRoot_JNT");
			this.m_hitDuration = Sequence.GetFXDuration(this.m_hitPrefab);
			if (this.m_hitPrefab != null)
			{
				GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.m_hitPrefab, bonePosition, Quaternion.identity);
				this.m_hitEffects.Add(item);
			}
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(this.m_radius, base.TargetPos);
			base.Source.OnSequenceHit(this, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			if (!string.IsNullOrEmpty(this.m_audioEventHitOnTargets))
			{
				AudioManager.PostEvent(this.m_audioEventHitOnTargets, actorData.gameObject);
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos, null);
	}

	private void OnDisable()
	{
		UnityEngine.Object.Destroy(this.m_explosion);
		if (this.m_hitEffects != null)
		{
			if (this.m_hitEffects.Count >= 1)
			{
				foreach (GameObject obj in this.m_hitEffects)
				{
					UnityEngine.Object.Destroy(obj);
				}
				this.m_hitEffects.Clear();
			}
		}
	}

	private void InstantiateExplosion()
	{
		this.m_createTime = GameTime.time;
		this.m_created = true;
		if (this.m_explosionPrefab != null)
		{
			this.m_explosion = UnityEngine.Object.Instantiate<GameObject>(this.m_explosionPrefab, base.TargetPos, Quaternion.identity);
			if (this.m_scale)
			{
				ParticleSystem[] componentsInChildren = this.m_explosion.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					ParticleSystem.MainModule main = particleSystem.main;
					ParticleSystem.MinMaxCurve startSize = main.startSize;
					startSize.constant *= this.m_radius * Board.Get().squareSize;
					main.startSize = startSize;
				}
			}
			this.m_duration = Sequence.GetFXDuration(this.m_hitPrefab);
			AudioManager.PostEvent(this.m_audioEventExplode, this.m_explosion.gameObject);
		}
		else
		{
			this.m_duration = 1f;
			if (Application.isEditor)
			{
				Debug.LogWarning("Sequence[" + base.gameObject.name + "] does not have a valid explosion sequence prefab, please add one if needed");
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.InstantiateExplosion();
		}
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public float radius;

		public Team team;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int num = (int)this.team;
			stream.Serialize(ref this.radius);
			stream.Serialize(ref num);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			int num = -1;
			stream.Serialize(ref this.radius);
			stream.Serialize(ref num);
			this.team = (Team)num;
		}
	}
}
