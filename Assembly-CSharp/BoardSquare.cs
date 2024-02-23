using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardSquare : MonoBehaviour
{
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
		Team = 2,
		Objective = 4,
		Revealed = 8
	}

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

	public int x
	{
		get { return m_pos.x; }
	}

	public int y
	{
		get { return m_pos.y; }
	}

	public float worldX
	{
		get { return m_pos.worldX; }
	}

	public float worldY
	{
		get { return m_pos.worldY; }
	}

	public int height
	{
		get { return m_pos.height; }
	}

	public GameObject occupant
	{
		get
		{
			return m_occupant;
		}
		set
		{
			m_occupant = value;
			m_occupantActor = m_occupant?.GetComponent<ActorData>();
		}
	}

	public ActorData OccupantActor
	{
		get
		{
			return m_occupantActor;
		}
		set
		{
			occupant = value?.gameObject;
		}
	}

	public int BrushRegion { get; set; }

	internal Bounds CameraBounds
	{
		get
		{
			Bounds? internalCameraBounds = m_internalCameraBounds;
			if (!internalCameraBounds.HasValue
				&& m_vertices != null
				&& m_vertices.Length > 1)
			{
				Vector3 center = Vector3.zero;
				foreach (Vector3 vector in m_vertices)
				{
					center += vector;
				}
				center /= m_vertices.Length;
				center += Vector3.up;
				Vector3 sizeSgn = 2.5f * (m_vertices[0] - center);
				Vector3 size = new Vector3(Mathf.Abs(sizeSgn.x), Mathf.Abs(sizeSgn.y), Mathf.Abs(sizeSgn.z));
				m_internalCameraBounds = new Bounds(center, size);
			}
			return m_internalCameraBounds.Value;
		}
	}

	internal Bounds? WorldBounds
	{
		get
		{
			Bounds? internalWorldBounds = m_internalWorldBounds;
			if (!internalWorldBounds.HasValue)
			{
				if (m_vertices != null && m_vertices.Length > 1)
				{
					Vector3 center = (m_vertices[0] + m_vertices[1]) / 2f;
					Vector3 size = m_vertices[0] - center;
					size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
					Bounds value = new Bounds(center, size);
					for (int i = 2; i < m_vertices.Length; i++)
					{
						value.Encapsulate(m_vertices[i]);
					}
					m_internalWorldBounds = value;
				}
			}
			return m_internalWorldBounds;
		}
	}

	public ThinCover.CoverType GetThinCover(ActorCover.CoverDirections squareSide)
	{
		return m_thinCoverTypes[(int)squareSide];
	}

	public void SetThinCover(ActorCover.CoverDirections squareSide, ThinCover.CoverType coverType)
	{
		m_thinCoverTypes[(int)squareSide] = coverType;
	}

	public bool IsNextToThinCover()
	{
		for (int i = 0; i < m_thinCoverTypes.Length; i++)
		{
			if (m_thinCoverTypes[i] != ThinCover.CoverType.None)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsNextToFullCover()
	{
		for (int i = 0; i < m_thinCoverTypes.Length; i++)
		{
			if (m_thinCoverTypes[i] == ThinCover.CoverType.Full)
			{
				return true;
			}
		}
		return false;
	}

	public GridPos GetGridPos()
	{
		return m_pos;
	}

	public bool IsInBrush()
	{
		return BrushRegion != -1;
	}

	public bool IsValidForKnockbackAndCharge()
	{
		return height <= Board.Get().BaselineHeight + 1;
	}

	public bool IsValidForGameplay()
	{
		return height == Board.BaselineHeightStatic;
	}

	public Vector3 GetCornerVertex(CornerType cornerType)
	{
		return m_vertices[(uint)cornerType];
	}

	public Vector3 ToVector3()
	{
		return new Vector3(m_pos.worldX, height, m_pos.worldY);
	}

	private void CalculateVertices(Board board, out Vector3[] vertices)
	{
		vertices = new Vector3[4];
		float squareSize = board.squareSize;
		float num = 0.5f * squareSize;
		float num2 = m_pos.x * board.squareSize;
		float num3 = m_pos.y * board.squareSize;
		vertices[0] = new Vector3(num2 - num, m_pos.height + m_highlightOffset, num3 - num);
		vertices[1] = new Vector3(num2 + num, m_pos.height + m_highlightOffset, num3 - num);
		vertices[2] = new Vector3(num2 + num, m_pos.height + m_highlightOffset, num3 + num);
		vertices[3] = new Vector3(num2 - num, m_pos.height + m_highlightOffset, num3 + num);
	}

	public float GetHighlightOffset()
	{
		return m_highlightOffset;
	}

	internal Vector3 CalcNearestPositionOnSquareEdge(Vector3 point)
	{
		int min1Idx = 0;
		int min2Idx = 1;
		float min1 = float.MaxValue;
		float min2 = float.MaxValue;
		for (int i = 0; i < m_vertices.Length; i++)
		{
			float sqrMagnitude = (point - m_vertices[i]).sqrMagnitude;
			if (sqrMagnitude < min1)
			{
				min2 = min1;
				min2Idx = min1Idx;
				min1 = sqrMagnitude;
				min1Idx = i;
			}
			else if (sqrMagnitude < min2)
			{
				min2 = sqrMagnitude;
				min2Idx = i;
			}
		}
		Vector3 a = m_vertices[min1Idx] - m_vertices[min2Idx];
		float magnitude = a.magnitude;
		Vector3 a2 = Vector3.Project(point - m_vertices[min2Idx], a / magnitude);
		if (a2.sqrMagnitude > magnitude * magnitude)
		{
			return m_vertices[min1Idx];
		}
		return a2 + m_vertices[min2Idx];
	}

	internal Plane CalcSidePlane(SideFlags side)
	{
		if ((side & SideFlags.Up) != 0)
		{
			return new Plane(Vector3.forward, GetCornerVertex(CornerType.UpperLeft));
		}
		if ((side & SideFlags.Down) != 0)
		{
			return new Plane(Vector3.back, GetCornerVertex(CornerType.LowerLeft));
		}
		if ((side & SideFlags.Left) != 0)
		{
			return new Plane(Vector3.left, GetCornerVertex(CornerType.UpperLeft));
		}
		if ((side & SideFlags.Right) != 0)
		{
			return new Plane(Vector3.right, GetCornerVertex(CornerType.UpperRight));
		}
		return default(Plane);
	}

	internal Bounds CalcSideBounds(SideFlags side)
	{
		Bounds result = default(Bounds);
		if ((side & SideFlags.Up) != 0)
		{
			result.SetMinMax(GetCornerVertex(CornerType.UpperLeft), GetCornerVertex(CornerType.UpperRight) + Vector3.up);
		}
		if ((side & SideFlags.Down) != 0)
		{
			result.SetMinMax(GetCornerVertex(CornerType.LowerLeft), GetCornerVertex(CornerType.LowerRight) + Vector3.up);
		}
		if ((side & SideFlags.Left) != 0)
		{
			result.SetMinMax(GetCornerVertex(CornerType.LowerLeft), GetCornerVertex(CornerType.UpperLeft) + Vector3.up);
		}
		if ((side & SideFlags.Right) != 0)
		{
			result.SetMinMax(GetCornerVertex(CornerType.LowerRight), GetCornerVertex(CornerType.UpperRight) + Vector3.up);
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
				BoardSquare boardSquare = board.GetSquareFromIndex(i, j);
				if (boardSquare == this)
				{
					array[i + j * board.GetMaxX()] = 1f;
				}
				else
				{
					array[i + j * board.GetMaxX()] = VectorUtils.GetLineOfSightPercentDistance(x, y, i, j, board, s_LoSHeightOffset, "LineOfSight");
				}
			}
		}
		return array;
	}

	// TODO
	public bool GetLOS(int indexX, int indexY)
	{
		if (Board.Get().m_losLookup != null)
		{
			return Board.Get().m_losLookup.GetLOSDistance(m_pos.x, m_pos.y, indexX, indexY) == 1f;
		}
		return false;
	}

	public float GetLOSDistance(int indexX, int indexY)
	{
		float result = 0f;
		if (Board.Get().m_losLookup != null)
		{
			result = Board.Get().m_losLookup.GetLOSDistance(m_pos.x, m_pos.y, indexX, indexY);
		}
		return result;
	}

	public void Setup(Board board, Material meshMaterial, GameObject losHighlightsParent)
	{
		SetupGridPosProp(board.squareSize);
		CalculateVertices(board, out m_vertices);
		if (height == -1)
		{
			return;
		}
		float x = m_pos.x * board.squareSize;
		float z = m_pos.y * board.squareSize;
		m_LOSHighlightObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
		m_LOSHighlightObj.name = new StringBuilder().Append("highlight").Append(m_pos.x).Append("x").Append(m_pos.y).ToString();
		DestroyImmediate(m_LOSHighlightObj.GetComponent<MeshCollider>());
		m_LOSHighlightObj.transform.parent = losHighlightsParent.transform;
		m_LOSHighlightObj.transform.position = new Vector3(x, m_pos.height + m_highlightOffset, z);
		m_LOSHighlightObj.transform.Rotate(new Vector3(90f, 0f, 0f));
		m_LOSHighlightObj.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		MeshRenderer component = m_LOSHighlightObj.GetComponent<MeshRenderer>();
		component.material = meshMaterial;
		component.enabled = true;
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.receiveShadows = false;
		m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
	}

	private void Start()
	{
		if (!m_LOSHighlightObj)
		{
			return;
		}
		m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
		m_LOSHighlightMesh = m_LOSHighlightObj.GetComponent<MeshFilter>().mesh;
	}

	public void ReevaluateSquare()
	{
		m_pos.x = m_gridPosProp.m_x;
		m_pos.y = m_gridPosProp.m_y;
		m_pos.height = m_gridPosProp.m_height;
	}

	private void Awake()
	{
		BrushRegion = -1;
		for (int i = 0; i < m_thinCoverTypes.Length; i++)
		{
			m_thinCoverTypes[i] = ThinCover.CoverType.None;
		}
	}

	private void OnDestroy()
	{
		m_occupant = null;
		m_occupantActor = null;
	}

	public void SetupGridPosProp(float squareSize)
	{
		if (m_gridPosProp == null)
		{
			m_gridPosProp = new GridPosProp();
		}
		m_gridPosProp.m_x = (int)(transform.position.x / squareSize);
		m_gridPosProp.m_y = (int)(transform.position.z / squareSize);
		m_gridPosProp.m_height = (int)transform.localScale.y;
		ReevaluateSquare();
	}

	public float HorizontalDistanceOnBoardTo(BoardSquare other)
	{
		float a = Mathf.Abs(x - other.x);
		float b = Mathf.Abs(y - other.y);
		float min = Mathf.Min(a, b);
		float max = Mathf.Max(a, b);
		return max - min + min * 1.5f;
	}

	public float HorizontalDistanceInSquaresTo(BoardSquare other)
	{
		Vector3 a = new Vector3(x, 0f, y);
		Vector3 b = new Vector3(other.x, 0f, other.y);
		return (a - b).magnitude;
	}

	public float HorizontalDistanceInSquaresTo_Squared(BoardSquare other)
	{
		Vector3 a = new Vector3(x, 0f, y);
		Vector3 b = new Vector3(other.x, 0f, other.y);
		return (a - b).sqrMagnitude;
	}

	public float HorizontalDistanceInWorldTo(BoardSquare other)
	{
		float a = m_pos.worldX - other.m_pos.worldX;
		float b = m_pos.worldY - other.m_pos.worldY;
		return Mathf.Sqrt(a * a + b * b);
	}

	public float HorizontalDistanceInSquaresToPos(Vector3 worldPos)
	{
		Vector3 vector = worldPos - ToVector3();
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude / Board.Get().squareSize;
	}

	public int VerticalDistanceTo(BoardSquare other)
	{
		return other.height - height;
	}

	public Color GetLOSHighlightMeshBaseColor()
	{
		if (m_LOSHighlightMesh != null && m_LOSHighlightMesh.colors.Length > 0)
		{
			return m_LOSHighlightMesh.colors[0];
		}
		return Color.white;
	}

	public void SetVisibleShade(int visibilityFlags, ref bool anySquareShadeChanged)
	{
		if (!m_LOSHighlightMesh)
		{
			return;
		}
		bool isSelf = (visibilityFlags & 1) != 0;
		bool isTeam = (visibilityFlags & 2) != 0;
		bool isObjective = (visibilityFlags & 4) != 0;
		bool isRevealed = (visibilityFlags & 8) != 0;
		Color result;
		if (!isRevealed && Board.Get().m_showLOS)
		{
			if (isObjective)
			{
				result = s_objectiveColor;
			}
			else if (isSelf)
			{
				result = s_visibleBySelfColor;
			}
			else if (isTeam)
			{
				result = s_visibleByTeamColor;
			}
			else
			{
				result = s_notVisibleColor;
			}
		}
		else
		{
			result = s_revealedColor;
		}
		bool isNotShowingLoS = false;
		if (!Board.Get().m_showLOS)
		{
			m_lastVisibleFlag = -1;
			isNotShowingLoS = true;
		}
		bool flag6 = m_lastVisibleFlag != (sbyte)visibilityFlags;
		if (isNotShowingLoS)
		{
			flag6 = result != GetLOSHighlightMeshBaseColor();
		}
		if (!flag6)
		{
			return;
		}
		if (!isNotShowingLoS)
		{
			m_lastVisibleFlag = (sbyte)visibilityFlags;
		}
		Mesh lOSHighlightMesh = m_LOSHighlightMesh;
		Vector3[] vertices = lOSHighlightMesh.vertices;
		Color32[] array = new Color32[vertices.Length];
		Color32 color = new Color32((byte)(result.r * 255f), (byte)(result.g * 255f), (byte)(result.b * 255f), (byte)(result.a * 255f));
		for (int i = 0; i < vertices.Length; i++)
		{
			array[i] = color;
		}
		lOSHighlightMesh.colors32 = array;
		anySquareShadeChanged = true;
	}

	private Color _001D(int visibilityFlags)
	{
		bool isSelf = (visibilityFlags & 1) != 0;
		bool isTeam = (visibilityFlags & 2) != 0;
		bool isObjective = (visibilityFlags & 4) != 0;
		bool isRevealed = (visibilityFlags & 8) != 0;
		InfluenceType influenceType = GetProximityInfluence(false);
		Color result;
		if (influenceType == InfluenceType.InfluencedByA)
		{
			result = new Color(ActorData.s_teamAColor.r, ActorData.s_teamAColor.g, ActorData.s_teamAColor.b);
		}
		else if (influenceType == InfluenceType.InfluencedByB)
		{
			result = new Color(ActorData.s_teamBColor.r, ActorData.s_teamBColor.g, ActorData.s_teamBColor.b);
		}
		else if (influenceType != InfluenceType.Contested)
		{
			result = new Color(1f, 1f, 1f);
		}
		else
		{
			result = new Color(ActorData.s_hostilePlayerColor.r, ActorData.s_hostilePlayerColor.g, ActorData.s_hostilePlayerColor.b);
		}
		float intensity;
		if (isSelf)
		{
			intensity = 1f;
		}
		else if (!isRevealed && !isObjective && !isTeam)
		{
			intensity = 0.25f;
		}
		else
		{
			intensity = 0.5f;
		}
		result.r *= intensity;
		result.g *= intensity;
		result.b *= intensity;
		result.a = 0.25f;
		return result;
	}

	public Vector3 GetOccupantRefPos()
	{
		return new Vector3(worldX, height, worldY);
	}

	public Vector3 GetOccupantLoSPos()
	{
		Vector3 worldPosition = GetOccupantRefPos();
		worldPosition.y += s_LoSHeightOffset;
		return worldPosition;
	}

	public Vector3 GetPosAtBaselineHeight()
	{
		Vector3 result = new Vector3(worldX, height, worldY);
		if (Board.Get() != null)
		{
			result.y = Board.Get().BaselineHeight;
		}
		return result;
	}

	public InfluenceType GetProximityInfluence(bool playersAffectInfluence)
	{
		if (occupant != null && occupant.GetComponent<ActorData>() != null)
		{
			ActorData component = occupant.GetComponent<ActorData>();
			if (component.GetTeam() == Team.TeamA)
			{
				return InfluenceType.InfluencedByA;
			}
			if (component.GetTeam() == Team.TeamB)
			{
				return InfluenceType.InfluencedByB;
			}
			return InfluenceType.Contested;
		}

		float minA = 1E+08f;
		float minB = 1E+08f;
		float limit = 36f;
		List<ActorData> allTeamMembersA = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
		foreach (ActorData actor in allTeamMembersA)
		{
			if (!actor.IsDead()
				&& (!GameplayUtils.IsPlayerControlled(actor) || playersAffectInfluence))
			{
				BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
				float dist = HorizontalDistanceInSquaresTo_Squared(currentBoardSquare);
				minA = Mathf.Min(minA, dist);
			}
		}
		List<ActorData> allTeamMembersB = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
		foreach (ActorData actor in allTeamMembersB)
		{
			if (!actor.IsDead()
				&& (!GameplayUtils.IsPlayerControlled(actor) || playersAffectInfluence))
			{
				BoardSquare currentBoardSquare2 = actor.GetCurrentBoardSquare();
				float dist = HorizontalDistanceInSquaresTo_Squared(currentBoardSquare2);
				minB = Mathf.Min(minB, dist);
			}
		}
		if (minA > limit && minB > limit)
		{
			return InfluenceType.NoInfluence;
		}
		if (Mathf.Abs(minA - minB) > 0.01f)
		{
			if (minA < minB)
			{
				return InfluenceType.InfluencedByA;
			}
			else
			{
				return InfluenceType.InfluencedByB;
			}
		}
		return InfluenceType.Contested;
	}

	public static string DebugString(BoardSquare square, bool mentionGameplayValid = false)
	{
		string text;
		if (square == null)
		{
			text = "(null)";
		}
		else
		{
			text = square.GetGridPos().ToString();
			if (mentionGameplayValid)
			{
				if (square.IsValidForGameplay())
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
}
