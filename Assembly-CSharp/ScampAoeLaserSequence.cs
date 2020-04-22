using System.Collections.Generic;
using UnityEngine;

public class ScampAoeLaserSequence : Sequence
{
	private class LineJointObjToInstance
	{
		public GameObject m_lineStartAttachObject;

		public GameObject m_lineFxInstance;

		public float m_timeOfSpawn;

		public LineJointObjToInstance(GameObject attachObj, GameObject lineInst)
		{
			m_lineStartAttachObject = attachObj;
			m_lineFxInstance = lineInst;
			m_timeOfSpawn = Time.time;
		}
	}

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
	public Object m_startEvent;

	private Scamp_SyncComponent m_syncComp;

	private bool m_projectileSpawned;

	private List<GenericSequenceProjectileInfo> m_projectileList = new List<GenericSequenceProjectileInfo>();

	private List<LineJointObjToInstance> m_lineFxInstances = new List<LineJointObjToInstance>();

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (base.Caster != null)
		{
			m_syncComp = base.Caster.GetComponent<Scamp_SyncComponent>();
		}
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SpawnProjectiles();
			return;
		}
	}

	private void OnDisable()
	{
		if (m_projectileList.Count > 0)
		{
			for (int i = 0; i < m_projectileList.Count; i++)
			{
				m_projectileList[i].OnSequenceDisable();
			}
			m_projectileList.Clear();
		}
		if (m_lineFxInstances.Count <= 0)
		{
			return;
		}
		while (true)
		{
			for (int j = 0; j < m_lineFxInstances.Count; j++)
			{
				if (m_lineFxInstances[j].m_lineFxInstance != null)
				{
					Object.Destroy(m_lineFxInstances[j].m_lineFxInstance);
				}
			}
			while (true)
			{
				m_lineFxInstances.Clear();
				return;
			}
		}
	}

	private void Update()
	{
		if (m_projectileList.Count <= 0)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_projectileList.Count; i++)
			{
				m_projectileList[i].OnUpdate();
				if (i >= m_lineFxInstances.Count)
				{
					continue;
				}
				LineJointObjToInstance lineJointObjToInstance = m_lineFxInstances[i];
				if (m_lineDuration > 0f)
				{
					if (Time.time - lineJointObjToInstance.m_timeOfSpawn > m_lineDuration)
					{
						lineJointObjToInstance.m_lineFxInstance.SetActiveIfNeeded(false);
						continue;
					}
				}
				Vector3 position = lineJointObjToInstance.m_lineStartAttachObject.transform.position;
				Vector3 vector;
				if (m_projectileList[i].m_fx != null)
				{
					vector = m_projectileList[i].m_fx.transform.position;
				}
				else
				{
					vector = m_projectileList[i].m_endPos;
				}
				Vector3 value = vector;
				if (m_syncComp.m_suitWasActiveOnTurnStart)
				{
					value.y = Board.Get().BaselineHeight;
				}
				Sequence.SetAttribute(lineJointObjToInstance.m_lineFxInstance, "startPoint", position);
				Sequence.SetAttribute(lineJointObjToInstance.m_lineFxInstance, "endPoint", value);
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(parameter == m_startEvent))
		{
			return;
		}
		while (true)
		{
			SpawnProjectiles();
			return;
		}
	}

	private void SpawnProjectiles()
	{
		if (m_projectileList.Count != 0)
		{
			return;
		}
		while (true)
		{
			if (base.Targets.Length <= 0)
			{
				return;
			}
			while (true)
			{
				if (!(m_syncComp != null))
				{
					return;
				}
				while (true)
				{
					if (m_projectileSpawned)
					{
						return;
					}
					while (true)
					{
						m_projectileSpawned = true;
						GameObject referenceModel = GetReferenceModel(base.Caster, ReferenceModelType.Actor);
						if (m_syncComp.m_suitWasActiveOnTurnStart)
						{
							m_fxJointLeft.Initialize(referenceModel);
							m_fxJointRight.Initialize(referenceModel);
						}
						else
						{
							m_fxJointNoSuit.Initialize(referenceModel);
						}
						for (int i = 0; i < base.Targets.Length; i++)
						{
							ActorData actorData = base.Targets[i];
							Vector3 vector = base.Caster.GetTravelBoardSquareWorldPosition();
							GameObject attachObj = base.Caster.gameObject;
							GameObject gameObject = null;
							if (m_syncComp.m_suitWasActiveOnTurnStart)
							{
								gameObject = m_suitBeamFxPrefab;
								Vector3 lhs = base.Targets[i].transform.position - base.Caster.transform.position;
								lhs.y = 0f;
								float num = Vector3.Dot(lhs, base.Caster.transform.right);
								if (num > 0f)
								{
									if (m_fxJointRight.IsInitialized())
									{
										vector = m_fxJointRight.m_jointObject.transform.position;
										attachObj = m_fxJointRight.m_jointObject;
									}
								}
								else if (m_fxJointLeft.IsInitialized())
								{
									vector = m_fxJointLeft.m_jointObject.transform.position;
									attachObj = m_fxJointLeft.m_jointObject;
								}
							}
							else
							{
								gameObject = m_noSuitBeamFxPrefab;
								if (m_fxJointNoSuit.IsInitialized())
								{
									vector = m_fxJointNoSuit.m_jointObject.transform.position;
									attachObj = m_fxJointNoSuit.m_jointObject;
								}
							}
							GenericSequenceProjectileInfo item = new GenericSequenceProjectileInfo(this, m_authoredProjectileData, vector, actorData.GetTravelBoardSquareWorldPosition(), actorData.AsArray());
							m_projectileList.Add(item);
							if (gameObject != null)
							{
								GameObject lineInst = InstantiateFX(gameObject, vector, Quaternion.identity);
								LineJointObjToInstance item2 = new LineJointObjToInstance(attachObj, lineInst);
								m_lineFxInstances.Add(item2);
							}
						}
						while (true)
						{
							switch (7)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
			}
		}
	}
}
