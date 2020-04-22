using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Blaster_SyncComponent : NetworkBehaviour
{
	internal class HitAreaIndicatorHighlight
	{
		public GameObject m_parentObj;

		public GameObject m_sideA;

		public GameObject m_sideB;

		public GameObject m_front;

		public Color m_color = Color.red;

		public void AdjustLength(float lengthInSquares)
		{
			if (m_sideA != null)
			{
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_sideA, lengthInSquares, m_color);
			}
			if (m_sideB != null)
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
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_sideB, lengthInSquares, m_color);
			}
			if (!(m_front != null))
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
				Vector3 localPosition = m_front.transform.localPosition;
				localPosition.z = lengthInSquares * Board.Get().squareSize;
				m_front.transform.localPosition = localPosition;
				return;
			}
		}

		public void AdjustSize(float widthInSquares, float lengthInSquares)
		{
			float num = widthInSquares * Board.Get().squareSize;
			if (m_sideA != null)
			{
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_sideA, lengthInSquares, m_color);
				m_sideA.transform.localPosition = new Vector3(0.5f * num, 0f, 0f);
			}
			if (m_sideB != null)
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
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_sideB, lengthInSquares, m_color);
				m_sideB.transform.localPosition = new Vector3(-0.5f * num, 0f, 0f);
			}
			if (!(m_front != null))
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
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_front, widthInSquares, m_color);
				m_front.transform.localPosition = new Vector3(-0.5f * num, 0f, lengthInSquares * Board.Get().squareSize);
				return;
			}
		}

		public void SetPose(Vector3 position, Vector3 lookRotation)
		{
			if (m_parentObj != null)
			{
				m_parentObj.transform.position = position;
				m_parentObj.transform.localRotation = Quaternion.LookRotation(lookRotation);
			}
		}

		public void SetVisible(bool visible)
		{
			if (!(m_parentObj != null))
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_parentObj.activeSelf != visible)
				{
					m_parentObj.SetActive(visible);
				}
				return;
			}
		}
	}

	[SyncVar]
	public int m_overchargeBuffs;

	[SyncVar]
	public int m_overchargeUses;

	[SyncVar]
	public bool m_canActivateDelayedLaser;

	[SyncVar]
	public Vector3 m_delayedLaserStartPos = Vector3.zero;

	[SyncVar]
	public Vector3 m_delayedLaserAimDir = Vector3.forward;

	[SyncVar]
	public int m_lastPlacementTurn;

	[SyncVar]
	public int m_lastUltCastTurn;

	public int m_lastCinematicRequested;

	public int m_lastOverchargeTurn = -1;

	private ActorData m_actorData;

	private BlasterDelayedLaser m_delayedLaserAbility;

	private HitAreaIndicatorHighlight m_laserRangeMarkerForAlly;

	public int Networkm_overchargeBuffs
	{
		get
		{
			return m_overchargeBuffs;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_overchargeBuffs, 1u);
		}
	}

	public int Networkm_overchargeUses
	{
		get
		{
			return m_overchargeUses;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_overchargeUses, 2u);
		}
	}

	public bool Networkm_canActivateDelayedLaser
	{
		get
		{
			return m_canActivateDelayedLaser;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_canActivateDelayedLaser, 4u);
		}
	}

	public Vector3 Networkm_delayedLaserStartPos
	{
		get
		{
			return m_delayedLaserStartPos;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_delayedLaserStartPos, 8u);
		}
	}

	public Vector3 Networkm_delayedLaserAimDir
	{
		get
		{
			return m_delayedLaserAimDir;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_delayedLaserAimDir, 16u);
		}
	}

	public int Networkm_lastPlacementTurn
	{
		get
		{
			return m_lastPlacementTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastPlacementTurn, 32u);
		}
	}

	public int Networkm_lastUltCastTurn
	{
		get
		{
			return m_lastUltCastTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastUltCastTurn, 64u);
		}
	}

	private void Start()
	{
		m_delayedLaserAbility = (GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterDelayedLaser)) as BlasterDelayedLaser);
		if (m_delayedLaserAbility != null)
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
			if (NetworkClient.active)
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
				m_laserRangeMarkerForAlly = CreateHitAreaTemplate(m_delayedLaserAbility.GetWidth(), 0.5f * (Color.blue + Color.white), true);
				m_laserRangeMarkerForAlly.m_parentObj.SetActive(false);
			}
		}
		m_actorData = GetComponent<ActorData>();
	}

	internal static HitAreaIndicatorHighlight CreateHitAreaTemplate(float widthInSquares, Color color, bool dotted, float lineWidth = 0.1f)
	{
		float num = widthInSquares * Board.Get().squareSize;
		GameObject gameObject = new GameObject("Blaster_RangeIndicator");
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		GameObject gameObject2 = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, lineWidth, dotted, color);
		GameObject gameObject3 = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, lineWidth, dotted, color);
		GameObject gameObject4 = HighlightUtils.Get().CreateDynamicLineSegmentMesh(widthInSquares, lineWidth, dotted, color);
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = new Vector3(0.5f * num, 0f, 0f);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject3.transform.parent = gameObject.transform;
		gameObject3.transform.localPosition = new Vector3(-0.5f * num, 0f, 0f);
		gameObject3.transform.localRotation = Quaternion.identity;
		gameObject4.transform.parent = gameObject.transform;
		gameObject4.transform.localPosition = new Vector3(-0.5f * num, 0f, 1f);
		gameObject4.transform.localRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
		HitAreaIndicatorHighlight hitAreaIndicatorHighlight = new HitAreaIndicatorHighlight();
		hitAreaIndicatorHighlight.m_parentObj = gameObject;
		hitAreaIndicatorHighlight.m_sideA = gameObject2;
		hitAreaIndicatorHighlight.m_sideB = gameObject3;
		hitAreaIndicatorHighlight.m_front = gameObject4;
		hitAreaIndicatorHighlight.m_color = color;
		return hitAreaIndicatorHighlight;
	}

	private void Update()
	{
		if (!NetworkClient.active)
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
			bool flag = false;
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				if (m_actorData.GetCurrentBoardSquare() != null)
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
					if (GameFlowData.Get() != null)
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
						if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
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
							if (m_canActivateDelayedLaser)
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
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						bool visible = activeOwnedActorData.GetTeam() == m_actorData.GetTeam();
						Vector3 vector = m_delayedLaserAimDir;
						if (m_delayedLaserAbility.TriggerAimAtBlaster())
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
							Vector3 vector2 = m_actorData.GetCurrentBoardSquare().ToVector3() - m_delayedLaserStartPos;
							vector2.y = 0f;
							vector2.Normalize();
							if (vector2.magnitude > 0f)
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
								vector = vector2;
							}
						}
						Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(m_delayedLaserStartPos, vector, m_delayedLaserAbility.GetLength() * Board.Get().squareSize, m_delayedLaserAbility.m_penetrateLineOfSight, m_actorData);
						Vector3 vector3 = m_delayedLaserStartPos;
						float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
						vector3 = VectorUtils.GetAdjustedStartPosWithOffset(vector3, laserEndPoint, laserInitialOffsetInSquares);
						vector3.y = (float)Board.Get().BaselineHeight + 0.01f;
						Vector3 vector4 = laserEndPoint - vector3;
						vector4.y = 0f;
						float magnitude = vector4.magnitude;
						m_laserRangeMarkerForAlly.SetPose(vector3, vector);
						m_laserRangeMarkerForAlly.AdjustSize(m_delayedLaserAbility.GetWidth(), magnitude / Board.Get().squareSize);
						m_laserRangeMarkerForAlly.SetVisible(visible);
						return;
					}
					}
				}
			}
			m_laserRangeMarkerForAlly.SetVisible(false);
			return;
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					writer.WritePackedUInt32((uint)m_overchargeBuffs);
					writer.WritePackedUInt32((uint)m_overchargeUses);
					writer.Write(m_canActivateDelayedLaser);
					writer.Write(m_delayedLaserStartPos);
					writer.Write(m_delayedLaserAimDir);
					writer.WritePackedUInt32((uint)m_lastPlacementTurn);
					writer.WritePackedUInt32((uint)m_lastUltCastTurn);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_overchargeBuffs);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_overchargeUses);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_canActivateDelayedLaser);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_delayedLaserStartPos);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_delayedLaserAimDir);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastPlacementTurn);
		}
		if ((base.syncVarDirtyBits & 0x40) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastUltCastTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_overchargeBuffs = (int)reader.ReadPackedUInt32();
					m_overchargeUses = (int)reader.ReadPackedUInt32();
					m_canActivateDelayedLaser = reader.ReadBoolean();
					m_delayedLaserStartPos = reader.ReadVector3();
					m_delayedLaserAimDir = reader.ReadVector3();
					m_lastPlacementTurn = (int)reader.ReadPackedUInt32();
					m_lastUltCastTurn = (int)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_overchargeBuffs = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
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
			m_overchargeUses = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			m_canActivateDelayedLaser = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
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
			m_delayedLaserStartPos = reader.ReadVector3();
		}
		if ((num & 0x10) != 0)
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
			m_delayedLaserAimDir = reader.ReadVector3();
		}
		if ((num & 0x20) != 0)
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
			m_lastPlacementTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40) == 0)
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
			m_lastUltCastTurn = (int)reader.ReadPackedUInt32();
			return;
		}
	}
}
