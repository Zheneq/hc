using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiDistanceMeleeAttackSequence : Sequence
{
	[Tooltip("FX at point(s) of impact")]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitAlignedWithCaster = true;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	[Header("-- Animation Events to Conditions --")]
	[AnimEventPicker]
	public List<MultiDistanceMeleeAttackSequence.EventToCondition> m_eventToConditions = new List<MultiDistanceMeleeAttackSequence.EventToCondition>();

	[AnimEventPicker]
	[Header(" -- Last Animation Event, to hit any remaining targets --")]
	public UnityEngine.Object m_lastHitEvent;

	private List<GameObject> m_hitFx;

	private HashSet<ActorData> m_alreadyHit;

	private void Update()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MultiDistanceMeleeAttackSequence.Update()).MethodHandle;
			}
			base.ProcessSequenceVisibility();
		}
	}

	private void SpawnHitFX(float withinDistanceInSquares)
	{
		if (this.m_alreadyHit == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MultiDistanceMeleeAttackSequence.SpawnHitFX(float)).MethodHandle;
			}
			this.m_alreadyHit = new HashSet<ActorData>();
		}
		if (this.m_hitFx == null)
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
			this.m_hitFx = new List<GameObject>();
		}
		if (base.Targets != null)
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
			float num = withinDistanceInSquares * Board.\u000E().squareSize;
			for (int i = 0; i < base.Targets.Length; i++)
			{
				Vector3 vector = base.Targets[i].transform.position - base.Caster.transform.position;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (!this.m_alreadyHit.Contains(base.Targets[i]))
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
					if (num >= 0f)
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
						if (magnitude > num)
						{
							goto IL_23F;
						}
					}
					this.m_alreadyHit.Add(base.Targets[i]);
					Vector3 targetHitPosition = base.GetTargetHitPosition(i, this.m_hitFxJoint);
					Vector3 vector2 = targetHitPosition - base.Caster.transform.position;
					vector2.y = 0f;
					vector2.Normalize();
					ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector2);
					Quaternion quaternion;
					if (this.m_hitAlignedWithCaster)
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
						quaternion = Quaternion.LookRotation(vector2);
					}
					else
					{
						quaternion = Quaternion.identity;
					}
					Quaternion rotation = quaternion;
					if (this.m_hitFxPrefab && base.IsHitFXVisible(base.Targets[i]))
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
						this.m_hitFx.Add(base.InstantiateFX(this.m_hitFxPrefab, targetHitPosition, rotation, true, true));
					}
					if (!string.IsNullOrEmpty(this.m_hitAudioEvent))
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
						AudioManager.PostEvent(this.m_hitAudioEvent, base.Targets[i].gameObject);
					}
					if (base.Targets[i] != null)
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
						base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
					}
				}
				IL_23F:;
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
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		for (int i = 0; i < this.m_eventToConditions.Count; i++)
		{
			MultiDistanceMeleeAttackSequence.EventToCondition eventToCondition = this.m_eventToConditions[i];
			if (eventToCondition.m_event == parameter)
			{
				this.SpawnHitFX(eventToCondition.m_distanceFromCasterInSquares);
			}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(MultiDistanceMeleeAttackSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
		}
		if (this.m_lastHitEvent == parameter)
		{
			this.SpawnHitFX(-1f);
			base.Source.OnSequenceHit(this, base.TargetPos, null);
		}
	}

	private void OnDisable()
	{
		if (this.m_hitFx != null)
		{
			foreach (GameObject gameObject in this.m_hitFx)
			{
				UnityEngine.Object.Destroy(gameObject.gameObject);
			}
			this.m_hitFx = null;
		}
	}

	[Serializable]
	public class EventToCondition
	{
		public UnityEngine.Object m_event;

		public float m_distanceFromCasterInSquares;
	}
}
