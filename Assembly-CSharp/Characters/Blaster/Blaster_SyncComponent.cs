// ROGUES
// SERVER
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
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_sideB, lengthInSquares, m_color);
			}
			if (m_front != null)
			{
				Vector3 localPosition = m_front.transform.localPosition;
				localPosition.z = lengthInSquares * Board.Get().squareSize;
				m_front.transform.localPosition = localPosition;
			}
		}

		public void AdjustSize(float widthInSquares, float lengthInSquares)
		{
			float width = widthInSquares * Board.Get().squareSize;
			if (m_sideA != null)
			{
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_sideA, lengthInSquares, m_color);
				m_sideA.transform.localPosition = new Vector3(0.5f * width, 0f, 0f);
			}
			if (m_sideB != null)
			{
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_sideB, lengthInSquares, m_color);
				m_sideB.transform.localPosition = new Vector3(-0.5f * width, 0f, 0f);
			}
			if (m_front != null)
			{
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_front, widthInSquares, m_color);
				m_front.transform.localPosition = new Vector3(-0.5f * width, 0f, lengthInSquares * Board.Get().squareSize);
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
			if (m_parentObj != null && m_parentObj.activeSelf != visible)
			{
				m_parentObj.SetActive(visible);
			}
		}
	}

	// reactor
	[SyncVar]
	public int m_overchargeBuffs;
	// renamed in rogues
	// [SyncVar]
	// public int m_overchargeCount;
	
	// removed in rogues
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

	// reactor
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
	// rogues
	// public int Networkm_overchargeCount
	// {
	// 	get
	// 	{
	// 		return m_overchargeCount;
	// 	}
	// 	[param: In]
	// 	set
	// 	{
	// 		SetSyncVar(value, ref m_overchargeCount, 1UL);
	// 	}
	// }

	// removed in rogues
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
			SetSyncVar(value, ref m_canActivateDelayedLaser, 4u);  // 2UL in rogues
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
			SetSyncVar(value, ref m_delayedLaserStartPos, 8u);  // 4UL in rogues
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
			SetSyncVar(value, ref m_delayedLaserAimDir, 16u);  // 8UL in rogues
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
			SetSyncVar(value, ref m_lastPlacementTurn, 32u);  // 16UL in rogues
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
			SetSyncVar(value, ref m_lastUltCastTurn, 64u);  // 32UL in rogues
		}
	}

	private void Start()
	{
		m_delayedLaserAbility = (GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterDelayedLaser)) as BlasterDelayedLaser);
		if (m_delayedLaserAbility != null && NetworkClient.active)
		{
			m_laserRangeMarkerForAlly = CreateHitAreaTemplate(m_delayedLaserAbility.GetWidth(), 0.5f * (Color.blue + Color.white), true);  // no dotted param in oruges
			m_laserRangeMarkerForAlly.m_parentObj.SetActive(false);
		}
		m_actorData = GetComponent<ActorData>();
	}

	internal static HitAreaIndicatorHighlight CreateHitAreaTemplate(float widthInSquares, Color color, bool dotted, float lineWidth = 0.1f)  // dotted = true in rogues
	{
		float num = widthInSquares * Board.Get().squareSize;
		GameObject gameObject = new GameObject("Blaster_RangeIndicator")
		{
			transform =
			{
				position = Vector3.zero,
				localRotation = Quaternion.identity
			}
		};
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
		HitAreaIndicatorHighlight hitAreaIndicatorHighlight = new HitAreaIndicatorHighlight
		{
			m_parentObj = gameObject,
			m_sideA = gameObject2,
			m_sideB = gameObject3,
			m_front = gameObject4,
			m_color = color
		};
		return hitAreaIndicatorHighlight;
	}

	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null
		    && m_actorData.GetCurrentBoardSquare() != null
		    && GameFlowData.Get() != null
		    && GameFlowData.Get().gameState == GameState.BothTeams_Decision
		    && m_canActivateDelayedLaser
		    && m_delayedLaserAbility != null)  // NOTE CHANGE null check added in rogues
		{
			bool visible = activeOwnedActorData.GetTeam() == m_actorData.GetTeam();
			Vector3 dir = m_delayedLaserAimDir;
			if (m_delayedLaserAbility.TriggerAimAtBlaster())
			{
				Vector3 dirAtBlaster = m_actorData.GetCurrentBoardSquare().ToVector3() - m_delayedLaserStartPos;
				dirAtBlaster.y = 0f;
				dirAtBlaster.Normalize();
				if (dirAtBlaster.magnitude > 0f)
				{
					dir = dirAtBlaster;
				}
			}
			Vector3 endPos = VectorUtils.GetLaserEndPoint(m_delayedLaserStartPos, dir, m_delayedLaserAbility.GetLength() * Board.Get().squareSize, m_delayedLaserAbility.m_penetrateLineOfSight, m_actorData);
			Vector3 startPos = m_delayedLaserStartPos;
			startPos = VectorUtils.GetAdjustedStartPosWithOffset(startPos, endPos, GameWideData.Get().m_laserInitialOffsetInSquares);
			startPos.y = Board.Get().BaselineHeight + 0.01f;
			Vector3 dirNormal = endPos - startPos;
			dirNormal.y = 0f;
			float magnitude = dirNormal.magnitude;
			if (m_laserRangeMarkerForAlly != null)  // NOTE CHANGE null check added in rogues
			{
				m_laserRangeMarkerForAlly.SetPose(startPos, dir);
				m_laserRangeMarkerForAlly.AdjustSize(m_delayedLaserAbility.GetWidth(), magnitude / Board.Get().squareSize);
				m_laserRangeMarkerForAlly.SetVisible(visible);
			}
		}
		else if (m_laserRangeMarkerForAlly != null)  // NOTE CHANGE null check added in rogues
		{
			m_laserRangeMarkerForAlly.SetVisible(false);
		}
	}

	private void UNetVersion()  // MirrorProcessed in rogues
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_overchargeBuffs);
			writer.WritePackedUInt32((uint)m_overchargeUses);
			writer.Write(m_canActivateDelayedLaser);
			writer.Write(m_delayedLaserStartPos);
			writer.Write(m_delayedLaserAimDir);
			writer.WritePackedUInt32((uint)m_lastPlacementTurn);
			writer.WritePackedUInt32((uint)m_lastUltCastTurn);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_overchargeBuffs);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_overchargeUses);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_canActivateDelayedLaser);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_delayedLaserStartPos);
		}
		if ((syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_delayedLaserAimDir);
		}
		if ((syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastPlacementTurn);
		}
		if ((syncVarDirtyBits & 0x40) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastUltCastTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// rogues
	// public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	// {
	// 	bool result = base.OnSerialize(writer, forceAll);
	// 	if (forceAll)
	// 	{
	// 		writer.WritePackedInt32(m_overchargeCount);
	// 		writer.Write(m_canActivateDelayedLaser);
	// 		writer.Write(m_delayedLaserStartPos);
	// 		writer.Write(m_delayedLaserAimDir);
	// 		writer.WritePackedInt32(m_lastPlacementTurn);
	// 		writer.WritePackedInt32(m_lastUltCastTurn);
	// 		return true;
	// 	}
	// 	writer.WritePackedUInt64(syncVarDirtyBits);
	// 	if ((syncVarDirtyBits & 1UL) != 0UL)
	// 	{
	// 		writer.WritePackedInt32(m_overchargeCount);
	// 		result = true;
	// 	}
	// 	if ((syncVarDirtyBits & 2UL) != 0UL)
	// 	{
	// 		writer.Write(m_canActivateDelayedLaser);
	// 		result = true;
	// 	}
	// 	if ((syncVarDirtyBits & 4UL) != 0UL)
	// 	{
	// 		writer.Write(m_delayedLaserStartPos);
	// 		result = true;
	// 	}
	// 	if ((syncVarDirtyBits & 8UL) != 0UL)
	// 	{
	// 		writer.Write(m_delayedLaserAimDir);
	// 		result = true;
	// 	}
	// 	if ((syncVarDirtyBits & 16UL) != 0UL)
	// 	{
	// 		writer.WritePackedInt32(m_lastPlacementTurn);
	// 		result = true;
	// 	}
	// 	if ((syncVarDirtyBits & 32UL) != 0UL)
	// 	{
	// 		writer.WritePackedInt32(m_lastUltCastTurn);
	// 		result = true;
	// 	}
	// 	return result;
	// }

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_overchargeBuffs = (int)reader.ReadPackedUInt32();
			m_overchargeUses = (int)reader.ReadPackedUInt32();
			m_canActivateDelayedLaser = reader.ReadBoolean();
			m_delayedLaserStartPos = reader.ReadVector3();
			m_delayedLaserAimDir = reader.ReadVector3();
			m_lastPlacementTurn = (int)reader.ReadPackedUInt32();
			m_lastUltCastTurn = (int)reader.ReadPackedUInt32();
			return;
		}
		int dirtyBits = (int)reader.ReadPackedUInt32();
		if ((dirtyBits & 1) != 0)
		{
			m_overchargeBuffs = (int)reader.ReadPackedUInt32();
		}
		if ((dirtyBits & 2) != 0)
		{
			m_overchargeUses = (int)reader.ReadPackedUInt32();
		}
		if ((dirtyBits & 4) != 0)
		{
			m_canActivateDelayedLaser = reader.ReadBoolean();
		}
		if ((dirtyBits & 8) != 0)
		{
			m_delayedLaserStartPos = reader.ReadVector3();
		}
		if ((dirtyBits & 0x10) != 0)
		{
			m_delayedLaserAimDir = reader.ReadVector3();
		}
		if ((dirtyBits & 0x20) != 0)
		{
			m_lastPlacementTurn = (int)reader.ReadPackedUInt32();
		}
		if ((dirtyBits & 0x40) != 0)
		{
			m_lastUltCastTurn = (int)reader.ReadPackedUInt32();
		}
	}

	// rogues
	// public override void OnDeserialize(NetworkReader reader, bool initialState)
	// {
	// 	base.OnDeserialize(reader, initialState);
	// 	if (initialState)
	// 	{
	// 		int networkm_overchargeCount = reader.ReadPackedInt32();
	// 		Networkm_overchargeCount = networkm_overchargeCount;
	// 		bool networkm_canActivateDelayedLaser = reader.ReadBoolean();
	// 		Networkm_canActivateDelayedLaser = networkm_canActivateDelayedLaser;
	// 		Vector3 networkm_delayedLaserStartPos = reader.ReadVector3();
	// 		Networkm_delayedLaserStartPos = networkm_delayedLaserStartPos;
	// 		Vector3 networkm_delayedLaserAimDir = reader.ReadVector3();
	// 		Networkm_delayedLaserAimDir = networkm_delayedLaserAimDir;
	// 		int networkm_lastPlacementTurn = reader.ReadPackedInt32();
	// 		Networkm_lastPlacementTurn = networkm_lastPlacementTurn;
	// 		int networkm_lastUltCastTurn = reader.ReadPackedInt32();
	// 		Networkm_lastUltCastTurn = networkm_lastUltCastTurn;
	// 		return;
	// 	}
	// 	long num = (long)reader.ReadPackedUInt64();
	// 	if ((num & 1L) != 0L)
	// 	{
	// 		int networkm_overchargeCount2 = reader.ReadPackedInt32();
	// 		Networkm_overchargeCount = networkm_overchargeCount2;
	// 	}
	// 	if ((num & 2L) != 0L)
	// 	{
	// 		bool networkm_canActivateDelayedLaser2 = reader.ReadBoolean();
	// 		Networkm_canActivateDelayedLaser = networkm_canActivateDelayedLaser2;
	// 	}
	// 	if ((num & 4L) != 0L)
	// 	{
	// 		Vector3 networkm_delayedLaserStartPos2 = reader.ReadVector3();
	// 		Networkm_delayedLaserStartPos = networkm_delayedLaserStartPos2;
	// 	}
	// 	if ((num & 8L) != 0L)
	// 	{
	// 		Vector3 networkm_delayedLaserAimDir2 = reader.ReadVector3();
	// 		Networkm_delayedLaserAimDir = networkm_delayedLaserAimDir2;
	// 	}
	// 	if ((num & 16L) != 0L)
	// 	{
	// 		int networkm_lastPlacementTurn2 = reader.ReadPackedInt32();
	// 		Networkm_lastPlacementTurn = networkm_lastPlacementTurn2;
	// 	}
	// 	if ((num & 32L) != 0L)
	// 	{
	// 		int networkm_lastUltCastTurn2 = reader.ReadPackedInt32();
	// 		Networkm_lastUltCastTurn = networkm_lastUltCastTurn2;
	// 	}
	// }
}
