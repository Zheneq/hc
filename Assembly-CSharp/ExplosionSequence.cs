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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ExplosionSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_radius = extraParams2.radius;
				this.m_team = extraParams2.team;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void Update()
	{
		base.ProcessSequenceVisibility();
		float time = GameTime.time;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExplosionSequence.Update()).MethodHandle;
			}
			if (!this.m_created)
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
				if (this.m_explosion == null)
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
					if (time - this.m_startTime >= this.m_initialShotDelay)
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
							this.InstantiateExplosion();
						}
					}
				}
			}
		}
		if (this.m_created)
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
			if (time - this.m_createTime >= this.m_hitDelay)
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
				if (!this.m_hitsSpawned)
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
					this.SpawnHits();
				}
			}
		}
		if (this.m_hitsSpawned)
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
			if (time - this.m_onHitTime >= this.m_hitDuration + 1f)
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
				if (time - this.m_createTime > this.m_duration + 1f)
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
					if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExplosionSequence.SpawnHits()).MethodHandle;
			}
			if (array.Length != 0)
			{
				goto IL_7C;
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
		}
		array = AreaEffectUtils.GetActorsInRadius(base.TargetPos, this.m_radius, true, base.Caster, GameplayUtils.GetOtherTeamsThan(this.m_team), null, false, default(Vector3)).ToArray();
		IL_7C:
		foreach (ActorData actorData in array)
		{
			Vector3 position = actorData.\u000E("upperRoot_JNT");
			this.m_hitDuration = Sequence.GetFXDuration(this.m_hitPrefab);
			if (this.m_hitPrefab != null)
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
				GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.m_hitPrefab, position, Quaternion.identity);
				this.m_hitEffects.Add(item);
			}
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(this.m_radius, base.TargetPos);
			base.Source.OnSequenceHit(this, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			if (!string.IsNullOrEmpty(this.m_audioEventHitOnTargets))
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
				AudioManager.PostEvent(this.m_audioEventHitOnTargets, actorData.gameObject);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		base.Source.OnSequenceHit(this, base.TargetPos, null);
	}

	private void OnDisable()
	{
		UnityEngine.Object.Destroy(this.m_explosion);
		if (this.m_hitEffects != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExplosionSequence.OnDisable()).MethodHandle;
			}
			if (this.m_hitEffects.Count >= 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExplosionSequence.InstantiateExplosion()).MethodHandle;
			}
			this.m_explosion = UnityEngine.Object.Instantiate<GameObject>(this.m_explosionPrefab, base.TargetPos, Quaternion.identity);
			if (this.m_scale)
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
				ParticleSystem[] componentsInChildren = this.m_explosion.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					ParticleSystem.MainModule main = particleSystem.main;
					ParticleSystem.MinMaxCurve startSize = main.startSize;
					startSize.constant *= this.m_radius * Board.\u000E().squareSize;
					main.startSize = startSize;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogWarning("Sequence[" + base.gameObject.name + "] does not have a valid explosion sequence prefab, please add one if needed");
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExplosionSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
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
