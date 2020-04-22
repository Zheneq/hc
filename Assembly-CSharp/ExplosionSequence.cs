using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExplosionSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public float radius;

		public Team team;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int value = (int)team;
			stream.Serialize(ref radius);
			stream.Serialize(ref value);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			int value = -1;
			stream.Serialize(ref radius);
			stream.Serialize(ref value);
			team = (Team)value;
		}
	}

	public GameObject m_explosionPrefab;

	public GameObject m_hitPrefab;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

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
		m_duration = 0f;
		m_created = false;
		m_hitsSpawned = false;
		m_hitEffects = new List<GameObject>();
		m_hitsSpawned = false;
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
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
				m_radius = extraParams2.radius;
				m_team = extraParams2.team;
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Update()
	{
		ProcessSequenceVisibility();
		float time = GameTime.time;
		if (m_startEvent == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_created)
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
				if (m_explosion == null)
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
					if (time - m_startTime >= m_initialShotDelay)
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
						if (m_initialized)
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
							InstantiateExplosion();
						}
					}
				}
			}
		}
		if (m_created)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (time - m_createTime >= m_hitDelay)
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
				if (!m_hitsSpawned)
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
					SpawnHits();
				}
			}
		}
		if (!m_hitsSpawned)
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
			if (!(time - m_onHitTime >= m_hitDuration + 1f))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (!(time - m_createTime > m_duration + 1f))
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (NetworkServer.active)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							MarkForRemoval();
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void SpawnHits()
	{
		m_onHitTime = GameTime.time;
		m_hitsSpawned = true;
		ActorData[] array = base.Targets;
		if (array != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (array.Length != 0)
			{
				goto IL_007c;
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
		}
		array = AreaEffectUtils.GetActorsInRadius(base.TargetPos, m_radius, true, base.Caster, GameplayUtils.GetOtherTeamsThan(m_team), null).ToArray();
		goto IL_007c;
		IL_007c:
		ActorData[] array2 = array;
		foreach (ActorData actorData in array2)
		{
			Vector3 bonePosition = actorData.GetBonePosition("upperRoot_JNT");
			m_hitDuration = Sequence.GetFXDuration(m_hitPrefab);
			if (m_hitPrefab != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				GameObject item = Object.Instantiate(m_hitPrefab, bonePosition, Quaternion.identity);
				m_hitEffects.Add(item);
			}
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(m_radius, base.TargetPos);
			base.Source.OnSequenceHit(this, actorData, impulseInfo);
			if (!string.IsNullOrEmpty(m_audioEventHitOnTargets))
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
				AudioManager.PostEvent(m_audioEventHitOnTargets, actorData.gameObject);
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			base.Source.OnSequenceHit(this, base.TargetPos);
			return;
		}
	}

	private void OnDisable()
	{
		Object.Destroy(m_explosion);
		if (m_hitEffects == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_hitEffects.Count >= 1)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					foreach (GameObject hitEffect in m_hitEffects)
					{
						Object.Destroy(hitEffect);
					}
					m_hitEffects.Clear();
					return;
				}
			}
			return;
		}
	}

	private void InstantiateExplosion()
	{
		m_createTime = GameTime.time;
		m_created = true;
		if (m_explosionPrefab != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_explosion = Object.Instantiate(m_explosionPrefab, base.TargetPos, Quaternion.identity);
					if (m_scale)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						ParticleSystem[] componentsInChildren = m_explosion.GetComponentsInChildren<ParticleSystem>();
						ParticleSystem[] array = componentsInChildren;
						foreach (ParticleSystem particleSystem in array)
						{
							ParticleSystem.MainModule main = particleSystem.main;
							ParticleSystem.MinMaxCurve startSize = main.startSize;
							startSize.constant *= m_radius * Board.Get().squareSize;
							main.startSize = startSize;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					m_duration = Sequence.GetFXDuration(m_hitPrefab);
					AudioManager.PostEvent(m_audioEventExplode, m_explosion.gameObject);
					return;
				}
			}
		}
		m_duration = 1f;
		if (!Application.isEditor)
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
			Debug.LogWarning("Sequence[" + base.gameObject.name + "] does not have a valid explosion sequence prefab, please add one if needed");
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			InstantiateExplosion();
			return;
		}
	}
}
