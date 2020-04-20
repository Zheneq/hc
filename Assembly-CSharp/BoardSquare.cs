using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardSquare : MonoBehaviour
{
	private ThinCover.CoverType[] m_thinCoverTypes = new ThinCover.CoverType[4];

	public GridPosProp m_gridPosProp;

	private GridPos m_pos = GridPos.s_invalid;

	[SerializeField]
	private GameObject m_LOSHighlightObj;

	private Mesh m_LOSHighlightMesh;

	private float m_highlightOffset = 0.05f;

	private GameObject m_occupant;

	private ActorData m_occupantActor;

	[SerializeField]
	private Vector3[] m_vertices;

	private Bounds? m_internalCameraBounds;

	private Bounds? m_internalWorldBounds;

	public static Color s_LOSHighlightColor = new Color(1f, 1f, 1f, 0.25f);

	public static Color s_mouseOverHighlightColor = new Color(0f, 1f, 0f, 0.5f);

	public static Color s_mouseOverOutOfRangeColor = new Color(0f, 0f, 0f, 0.5f);

	public static Color s_moveableHighlightColor = new Color(0f, 1f, 1f);

	public static Color s_targetableByAbilityHighlightColor = new Color(0.8f, 0.2f, 0f);

	public static Color s_respawnOptionHighlightColor = new Color(0.2f, 0.8f, 0f);

	public static float s_LoSHeightOffset = 1.6f;

	public static int s_boardSquareLayer = 9;

	private static Color s_revealedColor = new Color(0.25f, 0.25f, 0.5f, 0f);

	private static Color s_objectiveColor = new Color(0.5f, 0.75f, 0.5f, 0f);

	private static Color s_visibleBySelfColor = new Color(0f, 0f, 0f, 0f);

	private static Color s_visibleByTeamColor = new Color(0f, 0f, 0f, 0f);

	private static Color s_notVisibleColor = new Color(1f, 1f, 1f, 1f);

	private sbyte m_lastVisibleFlag = -1;

	public ThinCover.CoverType GetCoverInDirection(ActorCover.CoverDirections direction)
	{
		return this.m_thinCoverTypes[(int)direction];
	}

	public void SetThinCover(ActorCover.CoverDirections squareSide, ThinCover.CoverType coverType)
	{
		this.m_thinCoverTypes[(int)squareSide] = coverType;
	}

	public bool IsExposedFromAnyDirection_zq()
	{
		for (int i = 0; i < this.m_thinCoverTypes.Length; i++)
		{
			if (this.m_thinCoverTypes[i] != ThinCover.CoverType.None)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasFullCoverFromAnyDirection_zq()
	{
		for (int i = 0; i < this.m_thinCoverTypes.Length; i++)
		{
			if (this.m_thinCoverTypes[i] == ThinCover.CoverType.Full)
			{
				return true;
			}
		}
		return false;
	}

	public GridPos GetGridPos()
	{
		return this.m_pos;
	}

	public int x
	{
		get
		{
			return this.m_pos.x;
		}
	}

	public int y
	{
		get
		{
			return this.m_pos.y;
		}
	}

	public float worldX
	{
		get
		{
			return this.m_pos.worldX;
		}
	}

	public float worldY
	{
		get
		{
			return this.m_pos.worldY;
		}
	}

	public int height
	{
		get
		{
			return this.m_pos.height;
		}
	}

	public GameObject occupant
	{
		get
		{
			return this.m_occupant;
		}
		set
		{
			this.m_occupant = value;
			if (this.m_occupant != null)
			{
				this.m_occupantActor = this.m_occupant.GetComponent<ActorData>();
			}
			else
			{
				this.m_occupantActor = null;
			}
		}
	}

	public ActorData OccupantActor
	{
		get
		{
			return this.m_occupantActor;
		}
		set
		{
			if (value == null)
			{
				this.occupant = null;
			}
			else
			{
				this.occupant = value.gameObject;
			}
		}
	}

	public int BrushRegion { get; set; }

	public bool IsInBrushRegion()
	{
		return this.BrushRegion != -1;
	}

	public bool symbol_0015()
	{
		int num = 1;
		return this.height <= Board.Get().BaselineHeight + num;
	}

	public bool IsBaselineHeight()
	{
		return this.height == Board.BaselineHeightStatic;
	}

	internal Bounds CameraBounds
	{
		get
		{
			Bounds? internalCameraBounds = this.m_internalCameraBounds;
			if (internalCameraBounds == null)
			{
				if (this.m_vertices != null)
				{
					if (this.m_vertices.Length > 1)
					{
						Vector3 vector = Vector3.zero;
						foreach (Vector3 b in this.m_vertices)
						{
							vector += b;
						}
						vector /= (float)this.m_vertices.Length;
						vector += Vector3.up;
						Vector3 size = 2.5f * (this.m_vertices[0] - vector);
						size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
						this.m_internalCameraBounds = new Bounds?(new Bounds(vector, size));
					}
				}
			}
			return this.m_internalCameraBounds.Value;
		}
	}

	internal Bounds? WorldBounds
	{
		get
		{
			Bounds? internalWorldBounds = this.m_internalWorldBounds;
			if (internalWorldBounds == null)
			{
				if (this.m_vertices != null && this.m_vertices.Length > 1)
				{
					Vector3 vector = (this.m_vertices[0] + this.m_vertices[1]) / 2f;
					Vector3 size = this.m_vertices[0] - vector;
					size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
					Bounds value = new Bounds(vector, size);
					for (int i = 2; i < this.m_vertices.Length; i++)
					{
						value.Encapsulate(this.m_vertices[i]);
					}
					this.m_internalWorldBounds = new Bounds?(value);
				}
			}
			return this.m_internalWorldBounds;
		}
	}

	public Vector3 GetVerticesAtCorner_zq(BoardSquare.CornerType corner)
	{
		return this.m_vertices[(int)corner];
	}

	public Vector3 ToVector3()
	{
		return new Vector3(this.m_pos.worldX, (float)this.height, this.m_pos.worldY);
	}

	private void CalculateVertices(Board board, out Vector3[] vertices)
	{
		vertices = new Vector3[4];
		float squareSize = board.squareSize;
		float num = 0.5f * squareSize;
		float num2 = (float)this.m_pos.x * board.squareSize;
		float num3 = (float)this.m_pos.y * board.squareSize;
		vertices[0] = new Vector3(num2 - num, (float)this.m_pos.height + this.m_highlightOffset, num3 - num);
		vertices[1] = new Vector3(num2 + num, (float)this.m_pos.height + this.m_highlightOffset, num3 - num);
		vertices[2] = new Vector3(num2 + num, (float)this.m_pos.height + this.m_highlightOffset, num3 + num);
		vertices[3] = new Vector3(num2 - num, (float)this.m_pos.height + this.m_highlightOffset, num3 + num);
	}

	public float GetHighlightOffset()
	{
		return this.m_highlightOffset;
	}

	internal Vector3 CalcNearestPositionOnSquareEdge(Vector3 point)
	{
		int num = 0;
		int num2 = 1;
		float num3 = float.MaxValue;
		float num4 = float.MaxValue;
		for (int i = 0; i < this.m_vertices.Length; i++)
		{
			float sqrMagnitude = (point - this.m_vertices[i]).sqrMagnitude;
			if (sqrMagnitude < num3)
			{
				num4 = num3;
				num2 = num;
				num3 = sqrMagnitude;
				num = i;
			}
			else if (sqrMagnitude < num4)
			{
				num4 = sqrMagnitude;
				num2 = i;
			}
		}
		Vector3 a = this.m_vertices[num] - this.m_vertices[num2];
		float magnitude = a.magnitude;
		Vector3 vector = Vector3.Project(point - this.m_vertices[num2], a / magnitude);
		if (vector.sqrMagnitude > magnitude * magnitude)
		{
			vector = this.m_vertices[num];
		}
		else
		{
			vector += this.m_vertices[num2];
		}
		return vector;
	}

	internal Plane CalcSidePlane(SideFlags side)
	{
		Plane result = default(Plane);
		if ((byte)(side & SideFlags.Up) != 0)
		{
			result = new Plane(Vector3.forward, this.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperLeft));
		}
		else if ((byte)(side & SideFlags.Down) != 0)
		{
			result = new Plane(Vector3.back, this.GetVerticesAtCorner_zq(BoardSquare.CornerType.LowerLeft));
		}
		else if ((byte)(side & SideFlags.Left) != 0)
		{
			result = new Plane(Vector3.left, this.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperLeft));
		}
		else if ((byte)(side & SideFlags.Right) != 0)
		{
			result = new Plane(Vector3.right, this.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperRight));
		}
		return result;
	}

	internal Bounds CalcSideBounds(SideFlags side)
	{
		Bounds result = default(Bounds);
		if ((byte)(side & SideFlags.Up) != 0)
		{
			result.SetMinMax(this.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperLeft), this.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperRight) + Vector3.up);
		}
		if ((byte)(side & SideFlags.Down) != 0)
		{
			result.SetMinMax(this.GetVerticesAtCorner_zq(BoardSquare.CornerType.LowerLeft), this.GetVerticesAtCorner_zq(BoardSquare.CornerType.LowerRight) + Vector3.up);
		}
		if ((byte)(side & SideFlags.Left) != 0)
		{
			result.SetMinMax(this.GetVerticesAtCorner_zq(BoardSquare.CornerType.LowerLeft), this.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperLeft) + Vector3.up);
		}
		if ((byte)(side & SideFlags.Right) != 0)
		{
			result.SetMinMax(this.GetVerticesAtCorner_zq(BoardSquare.CornerType.LowerRight), this.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperRight) + Vector3.up);
		}
		return result;
	}

	public float[] CalculateLOS(Board board)
	{
		float[] array = new float[board.GetMaxX() * board.GetMaxY()];
		for (int i = 0; i < board.GetMaxX(); i++)
		{
			for (int j = 0; j < board.GetMaxY(); j++)
			{
				BoardSquare boardSquare = board.GetBoardSquare(i, j);
				if (boardSquare == this)
				{
					array[i + j * board.GetMaxX()] = 1f;
				}
				else
				{
					array[i + j * board.GetMaxX()] = VectorUtils.GetLineOfSightPercentDistance(this.x, this.y, i, j, board, BoardSquare.s_LoSHeightOffset, "LineOfSight");
				}
			}
		}
		return array;
	}

	public bool symbol_0013(int symbol_001D, int symbol_000E)
	{
		bool result = false;
		if (Board.Get().m_losLookup != null)
		{
			bool flag;
			if (Board.Get().m_losLookup.GetLOSDistance(this.m_pos.x, this.m_pos.y, symbol_001D, symbol_000E) == 1f)
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			result = flag;
		}
		return result;
	}

	public float GetLOSDistance(int xDest, int yDest)
	{
		float result = 0f;
		if (Board.Get().m_losLookup != null)
		{
			result = Board.Get().m_losLookup.GetLOSDistance(this.m_pos.x, this.m_pos.y, xDest, yDest);
		}
		return result;
	}

	public void Setup(Board board, Material meshMaterial, GameObject losHighlightsParent)
	{
		this.SetupGridPosProp(board.squareSize);
		this.CalculateVertices(board, out this.m_vertices);
		if (this.height != -1)
		{
			float x = (float)this.m_pos.x * board.squareSize;
			float z = (float)this.m_pos.y * board.squareSize;
			this.m_LOSHighlightObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
			this.m_LOSHighlightObj.name = string.Format("highlight{0}x{1}", this.m_pos.x, this.m_pos.y);
			UnityEngine.Object.DestroyImmediate(this.m_LOSHighlightObj.GetComponent<MeshCollider>());
			this.m_LOSHighlightObj.transform.parent = losHighlightsParent.transform;
			this.m_LOSHighlightObj.transform.position = new Vector3(x, (float)this.m_pos.height + this.m_highlightOffset, z);
			this.m_LOSHighlightObj.transform.Rotate(new Vector3(90f, 0f, 0f));
			this.m_LOSHighlightObj.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
			MeshRenderer component = this.m_LOSHighlightObj.GetComponent<MeshRenderer>();
			component.material = meshMaterial;
			component.enabled = true;
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			this.m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
		}
	}

	private void Start()
	{
		if (this.m_LOSHighlightObj)
		{
			this.m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
			this.m_LOSHighlightMesh = this.m_LOSHighlightObj.GetComponent<MeshFilter>().mesh;
		}
	}

	public void ReevaluateSquare()
	{
		this.m_pos.x = this.m_gridPosProp.m_x;
		this.m_pos.y = this.m_gridPosProp.m_y;
		this.m_pos.height = this.m_gridPosProp.m_height;
	}

	private void Awake()
	{
		this.BrushRegion = -1;
		for (int i = 0; i < this.m_thinCoverTypes.Length; i++)
		{
			this.m_thinCoverTypes[i] = ThinCover.CoverType.None;
		}
	}

	private void OnDestroy()
	{
		this.m_occupant = null;
		this.m_occupantActor = null;
	}

	public void SetupGridPosProp(float squareSize)
	{
		if (this.m_gridPosProp == null)
		{
			this.m_gridPosProp = new GridPosProp();
		}
		this.m_gridPosProp.m_x = (int)(base.transform.position.x / squareSize);
		this.m_gridPosProp.m_y = (int)(base.transform.position.z / squareSize);
		this.m_gridPosProp.m_height = (int)base.transform.localScale.y;
		this.ReevaluateSquare();
	}

	public float HorizontalDistanceOnBoardTo(BoardSquare other)
	{
		float a = (float)Mathf.Abs(this.x - other.x);
		float b = (float)Mathf.Abs(this.y - other.y);
		float num = Mathf.Min(a, b);
		float num2 = Mathf.Max(a, b);
		return num2 - num + num * 1.5f;
	}

	public float HorizontalDistanceInSquaresTo(BoardSquare other)
	{
		Vector3 a = new Vector3((float)this.x, 0f, (float)this.y);
		Vector3 b = new Vector3((float)other.x, 0f, (float)other.y);
		return (a - b).magnitude;
	}

	public float HorizontalDistanceInSquaresTo_Squared(BoardSquare other)
	{
		Vector3 a = new Vector3((float)this.x, 0f, (float)this.y);
		Vector3 b = new Vector3((float)other.x, 0f, (float)other.y);
		return (a - b).sqrMagnitude;
	}

	public float HorizontalDistanceInWorldTo(BoardSquare other)
	{
		float num = this.m_pos.worldX - other.m_pos.worldX;
		float num2 = this.m_pos.worldY - other.m_pos.worldY;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	public float HorizontalDistanceInSquaresToPos(Vector3 worldPos)
	{
		Vector3 b = this.ToVector3();
		Vector3 vector = worldPos - b;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude / Board.Get().squareSize;
	}

	public int VerticalDistanceTo(BoardSquare other)
	{
		return other.height - this.height;
	}

	public Color GetLOSHighlightMeshBaseColor()
	{
		if (this.m_LOSHighlightMesh != null)
		{
			if (this.m_LOSHighlightMesh.colors.Length > 0)
			{
				return this.m_LOSHighlightMesh.colors[0];
			}
		}
		return Color.white;
	}

	public unsafe void SetVisibleShade(int visibilityFlags, ref bool anySquareShadeChanged)
	{
		if (this.m_LOSHighlightMesh)
		{
			bool flag = (visibilityFlags & 8) != 0;
			bool flag2 = (visibilityFlags & 4) != 0;
			bool flag3 = (visibilityFlags & 1) != 0;
			bool flag4 = (visibilityFlags & 2) != 0;
			Color lhs;
			if (!flag)
			{
				if (!Board.Get().m_showLOS)
				{
				}
				else
				{
					if (flag2)
					{
						lhs = BoardSquare.s_objectiveColor;
						goto IL_C2;
					}
					if (flag3)
					{
						lhs = BoardSquare.s_visibleBySelfColor;
						goto IL_C2;
					}
					if (flag4)
					{
						lhs = BoardSquare.s_visibleByTeamColor;
						goto IL_C2;
					}
					lhs = BoardSquare.s_notVisibleColor;
					goto IL_C2;
				}
			}
			lhs = BoardSquare.s_revealedColor;
			IL_C2:
			bool flag5 = false;
			if (!Board.Get().m_showLOS)
			{
				this.m_lastVisibleFlag = -1;
				flag5 = true;
			}
			bool flag6 = (int)this.m_lastVisibleFlag != (int)((sbyte)visibilityFlags);
			if (flag5)
			{
				flag6 = (lhs != this.GetLOSHighlightMeshBaseColor());
			}
			if (flag6)
			{
				if (!flag5)
				{
					this.m_lastVisibleFlag = (sbyte)visibilityFlags;
				}
				Mesh loshighlightMesh = this.m_LOSHighlightMesh;
				Vector3[] vertices = loshighlightMesh.vertices;
				Color32[] array = new Color32[vertices.Length];
				int i = 0;
				Color32 color = new Color32((byte)(lhs.r * 255f), (byte)(lhs.g * 255f), (byte)(lhs.b * 255f), (byte)(lhs.a * 255f));
				while (i < vertices.Length)
				{
					array[i] = color;
					i++;
				}
				loshighlightMesh.colors32 = array;
				anySquareShadeChanged = true;
			}
		}
	}

	private Color symbol_001D(int symbol_001D)
	{
		bool flag = (symbol_001D & 8) != 0;
		bool flag2 = (symbol_001D & 4) != 0;
		bool flag3 = (symbol_001D & 1) != 0;
		bool flag4 = (symbol_001D & 2) != 0;
		InfluenceType influenceType = this.symbol_001D(false);
		Color result;
		if (influenceType == InfluenceType.InfluencedByA)
		{
			result = new Color(ActorData.s_teamAColor.r, ActorData.s_teamAColor.g, ActorData.s_teamAColor.b);
		}
		else if (influenceType == InfluenceType.InfluencedByB)
		{
			result = new Color(ActorData.s_teamBColor.r, ActorData.s_teamBColor.g, ActorData.s_teamBColor.b);
		}
		else if (influenceType == InfluenceType.Contested)
		{
			result = new Color(ActorData.s_hostilePlayerColor.r, ActorData.s_hostilePlayerColor.g, ActorData.s_hostilePlayerColor.b);
		}
		else
		{
			result = new Color(1f, 1f, 1f);
		}
		float num;
		if (flag3)
		{
			num = 1f;
		}
		else
		{
			if (!flag)
			{
				if (!flag2)
				{
					if (!flag4)
					{
						num = 0.25f;
						goto IL_126;
					}
				}
			}
			num = 0.5f;
		}
		IL_126:
		result.r *= num;
		result.g *= num;
		result.b *= num;
		result.a = 0.25f;
		return result;
	}

	public Vector3 GetWorldPosition()
	{
		Vector3 result = new Vector3(this.worldX, (float)this.height, this.worldY);
		return result;
	}

	public Vector3 GetWorldPositionForLoS()
	{
		Vector3 worldPosition = this.GetWorldPosition();
		worldPosition.y += BoardSquare.s_LoSHeightOffset;
		return worldPosition;
	}

	public Vector3 GetBaselineHeight()
	{
		Vector3 result = new Vector3(this.worldX, (float)this.height, this.worldY);
		if (Board.Get() != null)
		{
			result.y = (float)Board.Get().BaselineHeight;
		}
		return result;
	}

	public InfluenceType symbol_001D(bool symbol_001D)
	{
		InfluenceType result;
		if (this.occupant != null && this.occupant.GetComponent<ActorData>() != null)
		{
			ActorData component = this.occupant.GetComponent<ActorData>();
			if (component.GetTeam() == Team.TeamA)
			{
				result = InfluenceType.InfluencedByA;
			}
			else if (component.GetTeam() == Team.TeamB)
			{
				result = InfluenceType.InfluencedByB;
			}
			else
			{
				result = InfluenceType.Contested;
			}
		}
		else
		{
			float num = 1E+08f;
			float num2 = 1E+08f;
			float num3 = 36f;
			List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
			foreach (ActorData actorData in allTeamMembers)
			{
				if (actorData.IsDead())
				{
				}
				else
				{
					if (GameplayUtils.IsPlayerControlled(actorData))
					{
						if (!symbol_001D)
						{
							continue;
						}
					}
					BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
					float b = this.HorizontalDistanceInSquaresTo_Squared(currentBoardSquare);
					num = Mathf.Min(num, b);
				}
			}
			List<ActorData> allTeamMembers2 = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
			using (List<ActorData>.Enumerator enumerator2 = allTeamMembers2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData2 = enumerator2.Current;
					if (!actorData2.IsDead())
					{
						if (GameplayUtils.IsPlayerControlled(actorData2))
						{
							if (!symbol_001D)
							{
								continue;
							}
						}
						BoardSquare currentBoardSquare2 = actorData2.GetCurrentBoardSquare();
						float b2 = this.HorizontalDistanceInSquaresTo_Squared(currentBoardSquare2);
						num2 = Mathf.Min(num2, b2);
					}
				}
			}
			if (num > num3)
			{
				if (num2 > num3)
				{
					return InfluenceType.NoInfluence;
				}
			}
			if (Mathf.Abs(num - num2) > 0.01f)
			{
				if (num < num2)
				{
					result = InfluenceType.InfluencedByA;
				}
				else
				{
					result = InfluenceType.InfluencedByB;
				}
			}
			else
			{
				result = InfluenceType.Contested;
			}
		}
		return result;
	}

	public static string symbol_001D(BoardSquare symbol_001D, bool symbol_000E = false)
	{
		string text;
		if (symbol_001D == null)
		{
			text = "(null)";
		}
		else
		{
			text = symbol_001D.GetGridPos().ToString();
			if (symbol_000E)
			{
				if (symbol_001D.IsBaselineHeight())
				{
					text += "[valid]";
				}
				else
				{
					text += "[invalid]";
				}
			}
		}
		return text;
	}

	public enum CornerType : byte
	{
		LowerLeft,
		LowerRight,
		UpperRight,
		UpperLeft
	}

	public enum VisibilityFlags : byte
	{
		Self = 1,
		Team,
		Objective = 4,
		Revealed = 8
	}
}
