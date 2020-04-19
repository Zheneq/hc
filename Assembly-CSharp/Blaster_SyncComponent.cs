using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Blaster_SyncComponent : NetworkBehaviour
{
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

	private Blaster_SyncComponent.HitAreaIndicatorHighlight m_laserRangeMarkerForAlly;

	private void Start()
	{
		this.m_delayedLaserAbility = (base.GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterDelayedLaser)) as BlasterDelayedLaser);
		if (this.m_delayedLaserAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Blaster_SyncComponent.Start()).MethodHandle;
			}
			if (NetworkClient.active)
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
				this.m_laserRangeMarkerForAlly = Blaster_SyncComponent.CreateHitAreaTemplate(this.m_delayedLaserAbility.GetWidth(), 0.5f * (Color.blue + Color.white), true, 0.1f);
				this.m_laserRangeMarkerForAlly.m_parentObj.SetActive(false);
			}
		}
		this.m_actorData = base.GetComponent<ActorData>();
	}

	internal static Blaster_SyncComponent.HitAreaIndicatorHighlight CreateHitAreaTemplate(float widthInSquares, Color color, bool dotted, float lineWidth = 0.1f)
	{
		float num = widthInSquares * Board.\u000E().squareSize;
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
		return new Blaster_SyncComponent.HitAreaIndicatorHighlight
		{
			m_parentObj = gameObject,
			m_sideA = gameObject2,
			m_sideB = gameObject3,
			m_front = gameObject4,
			m_color = color
		};
	}

	private void Update()
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Blaster_SyncComponent.Update()).MethodHandle;
			}
			bool flag = false;
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				if (this.m_actorData.\u0012() != null)
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
					if (GameFlowData.Get() != null)
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
						if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
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
							if (this.m_canActivateDelayedLaser)
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
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
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
				bool visible = activeOwnedActorData.\u000E() == this.m_actorData.\u000E();
				Vector3 vector = this.m_delayedLaserAimDir;
				if (this.m_delayedLaserAbility.TriggerAimAtBlaster())
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
					Vector3 vector2 = this.m_actorData.\u0012().ToVector3() - this.m_delayedLaserStartPos;
					vector2.y = 0f;
					vector2.Normalize();
					if (vector2.magnitude > 0f)
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
						vector = vector2;
					}
				}
				Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(this.m_delayedLaserStartPos, vector, this.m_delayedLaserAbility.GetLength() * Board.\u000E().squareSize, this.m_delayedLaserAbility.m_penetrateLineOfSight, this.m_actorData, null, true);
				Vector3 vector3 = this.m_delayedLaserStartPos;
				float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
				vector3 = VectorUtils.GetAdjustedStartPosWithOffset(vector3, laserEndPoint, laserInitialOffsetInSquares);
				vector3.y = (float)Board.\u000E().BaselineHeight + 0.01f;
				Vector3 vector4 = laserEndPoint - vector3;
				vector4.y = 0f;
				float magnitude = vector4.magnitude;
				this.m_laserRangeMarkerForAlly.SetPose(vector3, vector);
				this.m_laserRangeMarkerForAlly.AdjustSize(this.m_delayedLaserAbility.GetWidth(), magnitude / Board.\u000E().squareSize);
				this.m_laserRangeMarkerForAlly.SetVisible(visible);
			}
			else
			{
				this.m_laserRangeMarkerForAlly.SetVisible(false);
			}
		}
	}

	private void UNetVersion()
	{
	}

	public int Networkm_overchargeBuffs
	{
		get
		{
			return this.m_overchargeBuffs;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_overchargeBuffs, 1U);
		}
	}

	public int Networkm_overchargeUses
	{
		get
		{
			return this.m_overchargeUses;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_overchargeUses, 2U);
		}
	}

	public bool Networkm_canActivateDelayedLaser
	{
		get
		{
			return this.m_canActivateDelayedLaser;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_canActivateDelayedLaser, 4U);
		}
	}

	public Vector3 Networkm_delayedLaserStartPos
	{
		get
		{
			return this.m_delayedLaserStartPos;
		}
		[param: In]
		set
		{
			base.SetSyncVar<Vector3>(value, ref this.m_delayedLaserStartPos, 8U);
		}
	}

	public Vector3 Networkm_delayedLaserAimDir
	{
		get
		{
			return this.m_delayedLaserAimDir;
		}
		[param: In]
		set
		{
			base.SetSyncVar<Vector3>(value, ref this.m_delayedLaserAimDir, 0x10U);
		}
	}

	public int Networkm_lastPlacementTurn
	{
		get
		{
			return this.m_lastPlacementTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_lastPlacementTurn, 0x20U);
		}
	}

	public int Networkm_lastUltCastTurn
	{
		get
		{
			return this.m_lastUltCastTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_lastUltCastTurn, 0x40U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Blaster_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32((uint)this.m_overchargeBuffs);
			writer.WritePackedUInt32((uint)this.m_overchargeUses);
			writer.Write(this.m_canActivateDelayedLaser);
			writer.Write(this.m_delayedLaserStartPos);
			writer.Write(this.m_delayedLaserAimDir);
			writer.WritePackedUInt32((uint)this.m_lastPlacementTurn);
			writer.WritePackedUInt32((uint)this.m_lastUltCastTurn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_overchargeBuffs);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_overchargeUses);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_canActivateDelayedLaser);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_delayedLaserStartPos);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_delayedLaserAimDir);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_lastPlacementTurn);
		}
		if ((base.syncVarDirtyBits & 0x40U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_lastUltCastTurn);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Blaster_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_overchargeBuffs = (int)reader.ReadPackedUInt32();
			this.m_overchargeUses = (int)reader.ReadPackedUInt32();
			this.m_canActivateDelayedLaser = reader.ReadBoolean();
			this.m_delayedLaserStartPos = reader.ReadVector3();
			this.m_delayedLaserAimDir = reader.ReadVector3();
			this.m_lastPlacementTurn = (int)reader.ReadPackedUInt32();
			this.m_lastUltCastTurn = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_overchargeBuffs = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
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
			this.m_overchargeUses = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			this.m_canActivateDelayedLaser = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
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
			this.m_delayedLaserStartPos = reader.ReadVector3();
		}
		if ((num & 0x10) != 0)
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
			this.m_delayedLaserAimDir = reader.ReadVector3();
		}
		if ((num & 0x20) != 0)
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
			this.m_lastPlacementTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40) != 0)
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
			this.m_lastUltCastTurn = (int)reader.ReadPackedUInt32();
		}
	}

	internal class HitAreaIndicatorHighlight
	{
		public GameObject m_parentObj;

		public GameObject m_sideA;

		public GameObject m_sideB;

		public GameObject m_front;

		public Color m_color = Color.red;

		public void AdjustLength(float lengthInSquares)
		{
			if (this.m_sideA != null)
			{
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(this.m_sideA, lengthInSquares, this.m_color);
			}
			if (this.m_sideB != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Blaster_SyncComponent.HitAreaIndicatorHighlight.AdjustLength(float)).MethodHandle;
				}
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(this.m_sideB, lengthInSquares, this.m_color);
			}
			if (this.m_front != null)
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
				Vector3 localPosition = this.m_front.transform.localPosition;
				localPosition.z = lengthInSquares * Board.\u000E().squareSize;
				this.m_front.transform.localPosition = localPosition;
			}
		}

		public void AdjustSize(float widthInSquares, float lengthInSquares)
		{
			float num = widthInSquares * Board.\u000E().squareSize;
			if (this.m_sideA != null)
			{
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(this.m_sideA, lengthInSquares, this.m_color);
				this.m_sideA.transform.localPosition = new Vector3(0.5f * num, 0f, 0f);
			}
			if (this.m_sideB != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Blaster_SyncComponent.HitAreaIndicatorHighlight.AdjustSize(float, float)).MethodHandle;
				}
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(this.m_sideB, lengthInSquares, this.m_color);
				this.m_sideB.transform.localPosition = new Vector3(-0.5f * num, 0f, 0f);
			}
			if (this.m_front != null)
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
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(this.m_front, widthInSquares, this.m_color);
				this.m_front.transform.localPosition = new Vector3(-0.5f * num, 0f, lengthInSquares * Board.\u000E().squareSize);
			}
		}

		public void SetPose(Vector3 position, Vector3 lookRotation)
		{
			if (this.m_parentObj != null)
			{
				this.m_parentObj.transform.position = position;
				this.m_parentObj.transform.localRotation = Quaternion.LookRotation(lookRotation);
			}
		}

		public void SetVisible(bool visible)
		{
			if (this.m_parentObj != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Blaster_SyncComponent.HitAreaIndicatorHighlight.SetVisible(bool)).MethodHandle;
				}
				if (this.m_parentObj.activeSelf != visible)
				{
					this.m_parentObj.SetActive(visible);
				}
			}
		}
	}
}
