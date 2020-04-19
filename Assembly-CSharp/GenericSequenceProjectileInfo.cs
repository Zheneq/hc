using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericSequenceProjectileInfo
{
	public Sequence m_parentSequence;

	public GenericSequenceProjectileAuthoredInfo m_authoredInfo;

	public Vector3 m_startPos = Vector3.zero;

	public Vector3 m_endPos = Vector3.zero;

	public ActorData[] m_targetActors;

	public bool m_finished;

	public float m_startDelay;

	public GameObject m_fx;

	public GameObject m_fxImpact;

	public List<GameObject> m_targetHitFx = new List<GameObject>();

	public Vector3 m_positionForSequenceHit;

	private float m_impactDuration;

	private float m_impactDurationLeft;

	private CRSpline m_spline;

	private float m_curSplineSpeed;

	private float m_splineSpeed;

	private float m_splineAcceleration;

	private float m_splineTraveled;

	private List<ActorData> m_actorsAlreadyHit = new List<ActorData>();

	public GenericSequenceProjectileInfo(Sequence parent, GenericSequenceProjectileAuthoredInfo authoredInfo, Vector3 startPos, Vector3 endPos, ActorData[] targetActors)
	{
		this.m_parentSequence = parent;
		this.m_authoredInfo = authoredInfo;
		this.m_impactDuration = Sequence.GetFXDuration(this.m_authoredInfo.m_fxImpactPrefab);
		this.m_startPos = startPos;
		this.m_endPos = endPos;
		this.m_positionForSequenceHit = endPos;
		this.m_targetActors = targetActors;
	}

	public void OnSequenceDisable()
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericSequenceProjectileInfo.OnSequenceDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
		if (this.m_fxImpact != null)
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
			UnityEngine.Object.Destroy(this.m_fxImpact);
			this.m_fxImpact = null;
		}
		if (this.m_targetHitFx != null && this.m_targetHitFx.Count > 0)
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
			for (int i = 0; i < this.m_targetHitFx.Count; i++)
			{
				UnityEngine.Object.Destroy(this.m_targetHitFx[i]);
			}
			this.m_targetHitFx.Clear();
		}
	}

	private void SpawnFX()
	{
		Vector3[] splinePath = this.GetSplinePath();
		this.m_spline = new CRSpline(splinePath);
		Vector3 a = this.m_spline.Interp(0.05f);
		(a - splinePath[1]).Normalize();
		Quaternion rotation = default(Quaternion);
		Debug.DrawLine(this.m_startPos, this.m_endPos, Color.red, 5f);
		float num = (splinePath[1] - splinePath[2]).magnitude + (splinePath[2] - splinePath[3]).magnitude;
		float num2 = num / this.m_authoredInfo.m_projectileSpeed;
		this.m_splineSpeed = 1f / num2;
		this.m_splineAcceleration = 0f;
		this.m_curSplineSpeed = this.m_splineSpeed;
		this.m_fx = this.m_parentSequence.InstantiateFX(this.m_authoredInfo.m_fxPrefab, splinePath[1], rotation, true, false);
		if (!string.IsNullOrEmpty(this.m_authoredInfo.m_spawnAudioEvent))
		{
			AudioManager.PostEvent(this.m_authoredInfo.m_spawnAudioEvent, this.m_parentSequence.Caster.gameObject);
		}
	}

	public void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (this.m_authoredInfo.m_fxImpactPrefab)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericSequenceProjectileInfo.SpawnImpactFX(Vector3, Quaternion)).MethodHandle;
			}
			this.m_fxImpact = this.m_parentSequence.InstantiateFX(this.m_authoredInfo.m_fxImpactPrefab, impactPos, impactRot, true, true);
			this.m_impactDurationLeft = this.m_impactDuration;
		}
		if (!string.IsNullOrEmpty(this.m_authoredInfo.m_impactAudioEvent))
		{
			AudioManager.PostEvent(this.m_authoredInfo.m_impactAudioEvent, this.m_fx.gameObject);
		}
	}

	protected virtual void SpawnTargetHitFx(ActorData target)
	{
		if (target != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericSequenceProjectileInfo.SpawnTargetHitFx(ActorData)).MethodHandle;
			}
			if (this.m_fx != null)
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
				if (this.m_parentSequence.IsHitFXVisibleWrtTeamFilter(target, this.m_authoredInfo.m_hitFxTeamFilter))
				{
					if (this.m_authoredInfo.m_targetHitFxPrefab != null)
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
						GameObject gameObject = this.m_authoredInfo.m_hitPosJoint.FindJointObject(target.gameObject);
						Vector3 vector;
						if (gameObject != null)
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
							vector = gameObject.transform.position;
						}
						else
						{
							vector = target.\u0016();
						}
						Vector3 position = vector;
						GameObject gameObject2 = this.m_parentSequence.InstantiateFX(this.m_authoredInfo.m_targetHitFxPrefab, position, this.m_fx.transform.rotation, true, true);
						if (gameObject2 != null)
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
							if (this.m_authoredInfo.m_targetHitFxAttachToJoint)
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
								this.m_parentSequence.AttachToBone(gameObject2, gameObject);
								gameObject2.transform.localPosition = Vector3.zero;
								gameObject2.transform.localScale = Vector3.one;
								gameObject2.transform.localRotation = Quaternion.identity;
							}
							else
							{
								gameObject2.transform.parent = this.m_parentSequence.transform;
							}
							this.m_targetHitFx.Add(gameObject2);
						}
					}
					if (!string.IsNullOrEmpty(this.m_authoredInfo.m_targetHitAudioEvent))
					{
						AudioManager.PostEvent(this.m_authoredInfo.m_targetHitAudioEvent, target.gameObject);
					}
				}
			}
		}
	}

	public Vector3[] GetSplinePath()
	{
		Vector3 startPos = this.m_startPos;
		Vector3[] array = new Vector3[5];
		if (this.m_authoredInfo.m_maxHeight == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericSequenceProjectileInfo.GetSplinePath()).MethodHandle;
			}
			Vector3 endPos = this.m_endPos;
			endPos.y += this.m_authoredInfo.m_yOffset;
			Vector3 b = endPos - startPos;
			array[0] = startPos - b;
			array[1] = startPos;
			array[2] = (startPos + endPos) * 0.5f;
			array[3] = endPos;
			array[4] = endPos + b;
		}
		else
		{
			Vector3 endPos2 = this.m_endPos;
			array[0] = startPos + Vector3.down * this.m_authoredInfo.m_maxHeight;
			array[1] = startPos;
			array[2] = (startPos + endPos2) * 0.5f + Vector3.up * this.m_authoredInfo.m_maxHeight;
			array[3] = endPos2;
			array[4] = endPos2 + Vector3.down * this.m_authoredInfo.m_maxHeight;
		}
		if (this.m_authoredInfo.m_reverseDirection)
		{
			Vector3 vector = array[0];
			array[0] = array[4];
			array[4] = vector;
			vector = array[1];
			array[1] = array[3];
			array[3] = vector;
		}
		return array;
	}

	public void OnUpdate()
	{
		if (this.m_fx == null)
		{
			this.m_startDelay -= GameTime.deltaTime;
			if (this.m_startDelay <= 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GenericSequenceProjectileInfo.OnUpdate()).MethodHandle;
				}
				GameObject referenceModel = this.m_parentSequence.GetReferenceModel(this.m_parentSequence.Caster, this.m_authoredInfo.m_jointReferenceType);
				if (referenceModel != null)
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
					this.m_authoredInfo.m_fxJoint.Initialize(referenceModel);
				}
				this.SpawnFX();
			}
		}
		else
		{
			if (this.m_fx.activeSelf)
			{
				this.m_curSplineSpeed += this.m_splineAcceleration;
				this.m_curSplineSpeed = Mathf.Min(this.m_splineSpeed, this.m_curSplineSpeed);
				this.m_splineTraveled += this.m_curSplineSpeed * GameTime.deltaTime;
				if (this.m_splineTraveled < this.m_authoredInfo.m_splineFractionUntilImpact)
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
					Vector3 vector = this.m_spline.Interp(this.m_splineTraveled);
					Quaternion rotation = default(Quaternion);
					rotation.SetLookRotation((vector - this.m_fx.transform.position).normalized);
					Vector3 vector2 = vector - this.m_fx.transform.position;
					vector2.Normalize();
					this.m_fx.transform.position = vector;
					this.m_fx.transform.rotation = rotation;
					if (this.m_targetActors != null)
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
						foreach (ActorData actorData in this.m_targetActors)
						{
							if (actorData != null)
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
								if (!this.m_actorsAlreadyHit.Contains(actorData))
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
									Vector3 rhs = actorData.transform.position - this.m_fx.transform.position;
									if (Vector3.Dot(vector2, rhs) < 0f)
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
										Vector3 position = this.m_fx.transform.position;
										ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, vector2);
										this.m_parentSequence.Source.OnSequenceHit(this.m_parentSequence, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
										this.m_actorsAlreadyHit.Add(actorData);
										this.SpawnTargetHitFx(actorData);
									}
								}
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
				}
				else
				{
					if (this.m_authoredInfo.m_spawnImpactAtFXDespawn)
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
						this.SpawnImpactFX(this.m_fx.transform.position, this.m_fx.transform.rotation);
					}
					else
					{
						this.SpawnImpactFX(this.m_endPos, Quaternion.identity);
					}
					this.m_fx.SetActive(false);
					this.m_finished = true;
					if (this.m_targetActors != null)
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
						foreach (ActorData actorData2 in this.m_targetActors)
						{
							if (!this.m_actorsAlreadyHit.Contains(actorData2))
							{
								this.m_parentSequence.Source.OnSequenceHit(this.m_parentSequence, actorData2, Sequence.CreateImpulseInfoWithObjectPose(this.m_fx), ActorModelData.RagdollActivation.HealthBased, true);
								this.SpawnTargetHitFx(actorData2);
							}
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
					this.m_parentSequence.Source.OnSequenceHit(this.m_parentSequence, this.m_positionForSequenceHit, null);
				}
			}
			if (this.m_fxImpact != null)
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
				if (this.m_fxImpact.activeSelf)
				{
					if (this.m_impactDurationLeft > 0f)
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
						this.m_impactDurationLeft -= GameTime.deltaTime;
					}
					else
					{
						this.m_finished = true;
					}
				}
			}
		}
	}
}
