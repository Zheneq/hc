using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampAoeLaserSequence : Sequence
{
	[Separator("Projectile Info (ignore joints here)", true)]
	public GenericSequenceProjectileAuthoredInfo m_authoredProjectileData;

	[JointPopup("Left Joint - If Has Suit")]
	public JointPopupProperty m_fxJointLeft;

	[JointPopup("Right Joint - If Has Suit")]
	public JointPopupProperty m_fxJointRight;

	[JointPopup("Joint If Out of Suit")]
	public JointPopupProperty m_fxJointNoSuit;

	[Separator("Line Vfx Prefabs", true)]
	public GameObject m_suitBeamFxPrefab;

	public GameObject m_noSuitBeamFxPrefab;

	[Header("-- Lifetime of line, used if > 0")]
	public float m_lineDuration = 2f;

	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	private Scamp_SyncComponent m_syncComp;

	private bool m_projectileSpawned;

	private List<GenericSequenceProjectileInfo> m_projectileList = new List<GenericSequenceProjectileInfo>();

	private List<ScampAoeLaserSequence.LineJointObjToInstance> m_lineFxInstances = new List<ScampAoeLaserSequence.LineJointObjToInstance>();

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (base.Caster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampAoeLaserSequence.FinishSetup()).MethodHandle;
			}
			this.m_syncComp = base.Caster.GetComponent<Scamp_SyncComponent>();
		}
		if (this.m_startEvent == null)
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
			this.SpawnProjectiles();
		}
	}

	private void OnDisable()
	{
		if (this.m_projectileList.Count > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampAoeLaserSequence.OnDisable()).MethodHandle;
			}
			for (int i = 0; i < this.m_projectileList.Count; i++)
			{
				this.m_projectileList[i].OnSequenceDisable();
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
			this.m_projectileList.Clear();
		}
		if (this.m_lineFxInstances.Count > 0)
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
			for (int j = 0; j < this.m_lineFxInstances.Count; j++)
			{
				if (this.m_lineFxInstances[j].m_lineFxInstance != null)
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
					UnityEngine.Object.Destroy(this.m_lineFxInstances[j].m_lineFxInstance);
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
			this.m_lineFxInstances.Clear();
		}
	}

	private void Update()
	{
		if (this.m_projectileList.Count > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampAoeLaserSequence.Update()).MethodHandle;
			}
			for (int i = 0; i < this.m_projectileList.Count; i++)
			{
				this.m_projectileList[i].OnUpdate();
				if (i < this.m_lineFxInstances.Count)
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
					ScampAoeLaserSequence.LineJointObjToInstance lineJointObjToInstance = this.m_lineFxInstances[i];
					if (this.m_lineDuration > 0f)
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
						if (Time.time - lineJointObjToInstance.m_timeOfSpawn > this.m_lineDuration)
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
							lineJointObjToInstance.m_lineFxInstance.SetActiveIfNeeded(false);
							goto IL_172;
						}
					}
					Vector3 position = lineJointObjToInstance.m_lineStartAttachObject.transform.position;
					Vector3 vector;
					if (this.m_projectileList[i].m_fx != null)
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
						vector = this.m_projectileList[i].m_fx.transform.position;
					}
					else
					{
						vector = this.m_projectileList[i].m_endPos;
					}
					Vector3 value = vector;
					if (this.m_syncComp.m_suitWasActiveOnTurnStart)
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
						value.y = (float)Board.\u000E().BaselineHeight;
					}
					Sequence.SetAttribute(lineJointObjToInstance.m_lineFxInstance, "startPoint", position);
					Sequence.SetAttribute(lineJointObjToInstance.m_lineFxInstance, "endPoint", value);
				}
				IL_172:;
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
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startEvent)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampAoeLaserSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SpawnProjectiles();
		}
	}

	private void SpawnProjectiles()
	{
		if (this.m_projectileList.Count == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampAoeLaserSequence.SpawnProjectiles()).MethodHandle;
			}
			if (base.Targets.Length > 0)
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
				if (this.m_syncComp != null)
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
					if (!this.m_projectileSpawned)
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
						this.m_projectileSpawned = true;
						GameObject referenceModel = base.GetReferenceModel(base.Caster, Sequence.ReferenceModelType.Actor);
						if (this.m_syncComp.m_suitWasActiveOnTurnStart)
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
							this.m_fxJointLeft.Initialize(referenceModel);
							this.m_fxJointRight.Initialize(referenceModel);
						}
						else
						{
							this.m_fxJointNoSuit.Initialize(referenceModel);
						}
						for (int i = 0; i < base.Targets.Length; i++)
						{
							ActorData actorData = base.Targets[i];
							Vector3 vector = base.Caster.\u0016();
							GameObject attachObj = base.Caster.gameObject;
							GameObject gameObject;
							if (this.m_syncComp.m_suitWasActiveOnTurnStart)
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
								gameObject = this.m_suitBeamFxPrefab;
								Vector3 lhs = base.Targets[i].transform.position - base.Caster.transform.position;
								lhs.y = 0f;
								float num = Vector3.Dot(lhs, base.Caster.transform.right);
								if (num > 0f)
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
									if (this.m_fxJointRight.IsInitialized())
									{
										vector = this.m_fxJointRight.m_jointObject.transform.position;
										attachObj = this.m_fxJointRight.m_jointObject;
									}
								}
								else if (this.m_fxJointLeft.IsInitialized())
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
									vector = this.m_fxJointLeft.m_jointObject.transform.position;
									attachObj = this.m_fxJointLeft.m_jointObject;
								}
							}
							else
							{
								gameObject = this.m_noSuitBeamFxPrefab;
								if (this.m_fxJointNoSuit.IsInitialized())
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
									vector = this.m_fxJointNoSuit.m_jointObject.transform.position;
									attachObj = this.m_fxJointNoSuit.m_jointObject;
								}
							}
							GenericSequenceProjectileInfo item = new GenericSequenceProjectileInfo(this, this.m_authoredProjectileData, vector, actorData.\u0016(), actorData.AsArray());
							this.m_projectileList.Add(item);
							if (gameObject != null)
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
								GameObject lineInst = base.InstantiateFX(gameObject, vector, Quaternion.identity, true, true);
								ScampAoeLaserSequence.LineJointObjToInstance item2 = new ScampAoeLaserSequence.LineJointObjToInstance(attachObj, lineInst);
								this.m_lineFxInstances.Add(item2);
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
					}
				}
			}
		}
	}

	private class LineJointObjToInstance
	{
		public GameObject m_lineStartAttachObject;

		public GameObject m_lineFxInstance;

		public float m_timeOfSpawn;

		public LineJointObjToInstance(GameObject attachObj, GameObject lineInst)
		{
			this.m_lineStartAttachObject = attachObj;
			this.m_lineFxInstance = lineInst;
			this.m_timeOfSpawn = Time.time;
		}
	}
}
