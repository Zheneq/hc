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
		if (m_delayedLaserAbility != null && NetworkClient.active)
		{
			m_laserRangeMarkerForAlly = CreateHitAreaTemplate(m_delayedLaserAbility.GetWidth(), 0.5f * (Color.blue + Color.white), true);
			m_laserRangeMarkerForAlly.m_parentObj.SetActive(false);
		}
		m_actorData = GetComponent<ActorData>();
	}

	internal static HitAreaIndicatorHighlight CreateHitAreaTemplate(float widthInSquares, Color color, bool dotted, float lineWidth = 0.1f)
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
		    && m_canActivateDelayedLaser)
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
			m_laserRangeMarkerForAlly.SetPose(startPos, dir);
			m_laserRangeMarkerForAlly.AdjustSize(m_delayedLaserAbility.GetWidth(), magnitude / Board.Get().squareSize);
			m_laserRangeMarkerForAlly.SetVisible(visible);
		}
		else
		{
			m_laserRangeMarkerForAlly.SetVisible(false);
		}
	}

	private void UNetVersion()
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
}
