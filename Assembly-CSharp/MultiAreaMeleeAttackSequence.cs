using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiAreaMeleeAttackSequence : Sequence
{
	[Tooltip("FX at point(s) of impact")]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitAlignedWithCaster = true;

	[AnimEventPicker]
	public UnityEngine.Object m_leftHitEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_rightHitEvent;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	private List<GameObject> m_hitFx;

	private HashSet<ActorData> m_alreadyHit;

	private bool m_leftHitSpawned;

	private bool m_rightHitSpawned;

	private bool Finished()
	{
		bool result = false;
		if (this.m_leftHitSpawned)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MultiAreaMeleeAttackSequence.Finished()).MethodHandle;
			}
			if (this.m_rightHitSpawned)
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
				result = true;
				foreach (GameObject gameObject in this.m_hitFx)
				{
					if (gameObject != null)
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
						if (gameObject.activeSelf)
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
							result = false;
							break;
						}
					}
				}
			}
		}
		return result;
	}

	private void Update()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MultiAreaMeleeAttackSequence.Update()).MethodHandle;
			}
			base.ProcessSequenceVisibility();
			if (this.Finished())
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
				base.MarkForRemoval();
			}
		}
	}

	private void SpawnHitFX(bool left)
	{
		if (this.m_alreadyHit == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MultiAreaMeleeAttackSequence.SpawnHitFX(bool)).MethodHandle;
			}
			this.m_alreadyHit = new HashSet<ActorData>();
		}
		if (this.m_hitFx == null)
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
			this.m_hitFx = new List<GameObject>();
		}
		bool flag;
		if (!this.m_leftHitSpawned)
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
			flag = this.m_rightHitSpawned;
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		if (left)
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
			this.m_leftHitSpawned = true;
		}
		else
		{
			this.m_rightHitSpawned = true;
		}
		if (base.Targets != null)
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
			int i = 0;
			while (i < base.Targets.Length)
			{
				Vector3 lhs = base.Targets[i].transform.position - base.Caster.transform.position;
				lhs.y = 0f;
				float num = Vector3.Dot(lhs, base.Caster.transform.right);
				if (num <= 0f)
				{
					goto IL_112;
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
				if (!left)
				{
					goto IL_12D;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_112;
				}
				IL_28F:
				i++;
				continue;
				IL_112:
				if (num <= 0f)
				{
					if (left)
					{
						goto IL_12D;
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
				if (!flag2)
				{
					goto IL_28F;
				}
				IL_12D:
				if (this.m_alreadyHit.Contains(base.Targets[i]))
				{
					goto IL_28F;
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
				this.m_alreadyHit.Add(base.Targets[i]);
				Vector3 targetHitPosition = base.GetTargetHitPosition(i, this.m_hitFxJoint);
				Vector3 vector = targetHitPosition - base.Caster.transform.position;
				vector.y = 0f;
				vector.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
				Quaternion rotation = (!this.m_hitAlignedWithCaster) ? Quaternion.identity : Quaternion.LookRotation(vector);
				if (this.m_hitFxPrefab)
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
					if (base.IsHitFXVisible(base.Targets[i]))
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
						this.m_hitFx.Add(base.InstantiateFX(this.m_hitFxPrefab, targetHitPosition, rotation, true, true));
					}
				}
				if (!string.IsNullOrEmpty(this.m_hitAudioEvent))
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
					AudioManager.PostEvent(this.m_hitAudioEvent, base.Targets[i].gameObject);
				}
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
					base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
					goto IL_28F;
				}
				goto IL_28F;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos, null);
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_leftHitEvent == parameter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MultiAreaMeleeAttackSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SpawnHitFX(true);
		}
		else if (this.m_rightHitEvent == parameter)
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
			this.SpawnHitFX(false);
		}
	}

	private void OnDisable()
	{
		if (this.m_hitFx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MultiAreaMeleeAttackSequence.OnDisable()).MethodHandle;
			}
			using (List<GameObject>.Enumerator enumerator = this.m_hitFx.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					UnityEngine.Object.Destroy(gameObject.gameObject);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_hitFx = null;
		}
	}
}
