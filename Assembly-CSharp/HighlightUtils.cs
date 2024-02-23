using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class HighlightUtils : MonoBehaviour, IGameEventListener
{
	[Serializable]
	public class CoverDirIndicatorParams
	{
		public float m_radiusInSquares = 3f;

		public Color m_color = new Color(1f, 0.75f, 0.25f);

		public TargeterOpacityData[] m_opacity;
	}

	public enum MoveIntoCoverIndicatorTiming
	{
		ShowOnMoveEnd,
		ShowOnTurnStart
	}

	[Serializable]
	public struct AreaShapePrefab
	{
		public AbilityAreaShape m_shape;

		public GameObject m_prefab;
	}

	[Serializable]
	public struct TargeterOpacityData
	{
		public float m_timeSinceConfirmed;

		public float m_alpha;
	}

	public enum CursorType
	{
		MouseOverCursorType,
		TabTargetingCursorType,
		ShapeTargetingSquareCursorType,
		ShapeTargetingCornerCursorType,
		NoCursorType
	}

	public enum ScrollCursorDirection
	{
		N,
		NE,
		E,
		SE,
		S,
		SW,
		W,
		NW,
		Undefined
	}

	public class HighlightMesh
	{
		public List<Vector3> m_pos = new List<Vector3>();

		public List<Vector2> m_uv = new List<Vector2>();

		public Dictionary<BoardSquare, int> m_borderSquares = new Dictionary<BoardSquare, int>();

		private void ApplySlope(PieceType pieceType, ref Vector3[] positions, BoardSquare square)
		{
			Vector3 verticesAtCorner_zq = square.GetCornerVertex(BoardSquare.CornerType.UpperRight);
			float num = verticesAtCorner_zq.y;
			Vector3 verticesAtCorner_zq2 = square.GetCornerVertex(BoardSquare.CornerType.UpperLeft);
			float num2 = verticesAtCorner_zq2.y;
			Vector3 verticesAtCorner_zq3 = square.GetCornerVertex(BoardSquare.CornerType.LowerRight);
			float num3 = verticesAtCorner_zq3.y;
			Vector3 verticesAtCorner_zq4 = square.GetCornerVertex(BoardSquare.CornerType.LowerLeft);
			float num4 = verticesAtCorner_zq4.y;
			if (!square.IsValidForGameplay())
			{
				float num5 = Board.Get().BaselineHeight;
				float num6 = num5 + square.GetHighlightOffset();
				num = num6;
				num2 = num6;
				num3 = num6;
				num4 = num6;
			}
			float y = (num2 + num4) * 0.5f;
			float y2 = (num + num3) * 0.5f;
			float num7 = (num2 + num) * 0.5f;
			float num8 = (num4 + num3) * 0.5f;
			float y3 = (num7 + num8) * 0.5f;
			if (pieceType != PieceType.LowerLeftHorizontal)
			{
				if (pieceType != PieceType.LowerLeftVertical)
				{
					if (pieceType != PieceType.LowerLeftInnerCorner)
					{
						if (pieceType != PieceType.LowerLeftOuterCorner)
						{
							if (pieceType != PieceType.LowerRightHorizontal && pieceType != 0)
							{
								if (pieceType != PieceType.LowerRightInnerCorner)
								{
									if (pieceType != PieceType.LowerRightOuterCorner)
									{
										if (pieceType != PieceType.UpperLeftHorizontal)
										{
											if (pieceType != PieceType.UpperLeftVertical && pieceType != PieceType.UpperLeftInnerCorner)
											{
												if (pieceType != PieceType.UpperLeftOuterCorner)
												{
													if (pieceType != PieceType.UpperRightHorizontal)
													{
														if (pieceType != PieceType.UpperRightVertical && pieceType != PieceType.UpperRightInnerCorner)
														{
															if (pieceType != PieceType.UpperRightOuterCorner)
															{
																return;
															}
														}
													}
													positions[0].y = num7;
													positions[1].y = y2;
													positions[2].y = y3;
													positions[3].y = num;
													return;
												}
											}
										}
										positions[0].y = num2;
										positions[1].y = y3;
										positions[2].y = y;
										positions[3].y = num7;
										return;
									}
								}
							}
							positions[0].y = y3;
							positions[1].y = num3;
							positions[2].y = num8;
							positions[3].y = y2;
							return;
						}
					}
				}
			}
			positions[0].y = y;
			positions[1].y = num8;
			positions[2].y = num4;
			positions[3].y = y3;
		}

		public void AddPiece(PieceType pieceType, BoardSquare square)
		{
			if (!(square != null))
			{
				return;
			}
			while (true)
			{
				if (!m_borderSquares.ContainsKey(square))
				{
					m_borderSquares[square] = 0;
				}
				int mask = m_borderSquares[square];
				AddPieceTypeToMask(ref mask, pieceType);
				m_borderSquares[square] = mask;
				Vector3 b = square.ToVector3();
				b.y = 0.1f;
				HighlightPiece highlightPiece = Get().m_highlightPieces[(int)pieceType];
				Vector3[] positions = highlightPiece.m_pos;
				ApplySlope(pieceType, ref positions, square);
				Vector3[] array = positions;
				foreach (Vector3 a in array)
				{
					m_pos.Add(a + b);
				}
				while (true)
				{
					Vector2[] uv = highlightPiece.m_uv;
					foreach (Vector2 item in uv)
					{
						m_uv.Add(item);
					}
					while (true)
					{
						switch (1)
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

		public void AddVerticalConnector(Vector3 pt0, Vector3 pt1, Vector3 pt2, Vector3 pt3, bool left)
		{
			m_pos.Add(pt0);
			m_pos.Add(pt1);
			m_pos.Add(pt2);
			m_pos.Add(pt3);
			if (left)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_uv.Add(new Vector2(2f / 3f, 1f));
						m_uv.Add(new Vector2(0.333333343f, 0f));
						m_uv.Add(new Vector2(0.333333343f, 1f));
						m_uv.Add(new Vector2(2f / 3f, 0f));
						return;
					}
				}
			}
			m_uv.Add(new Vector2(2f / 3f, 0f));
			m_uv.Add(new Vector2(0.333333343f, 1f));
			m_uv.Add(new Vector2(0.333333343f, 0f));
			m_uv.Add(new Vector2(2f / 3f, 1f));
		}

		public void ProcessVerticalConnectors()
		{
			float num = Board.Get().squareSize * 0.5f;
			float num2 = num - 0.1f;
			float num3 = 0.5f;
			using (Dictionary<BoardSquare, int>.Enumerator enumerator = m_borderSquares.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<BoardSquare, int> current = enumerator.Current;
					BoardSquare key = current.Key;
					BoardSquare boardSquare = Board.Get().GetSquareFromIndex(key.x, key.y + 1);
					int value = current.Value;
					if (boardSquare != null)
					{
						if (m_borderSquares.ContainsKey(boardSquare))
						{
							if ((float)(boardSquare.height - key.height) >= num3)
							{
								if (IsPieceTypeInMask(value, PieceType.UpperLeftVertical) || IsPieceTypeInMask(value, PieceType.UpperLeftInnerCorner))
								{
									Vector3 pt = new Vector3(key.worldX - num, boardSquare.height, key.worldY + num2);
									Vector3 pt2 = new Vector3(key.worldX, key.height, key.worldY + num2);
									Vector3 pt3 = new Vector3(key.worldX - num, key.height, key.worldY + num2);
									Vector3 pt4 = new Vector3(key.worldX, boardSquare.height, key.worldY + num2);
									AddVerticalConnector(pt, pt2, pt3, pt4, true);
								}
								if (!IsPieceTypeInMask(value, PieceType.UpperRightVertical))
								{
									if (!IsPieceTypeInMask(value, PieceType.UpperRightInnerCorner))
									{
										goto IL_026a;
									}
								}
								Vector3 pt5 = new Vector3(key.worldX, boardSquare.height, key.worldY + num2);
								Vector3 pt6 = new Vector3(key.worldX + num, key.height, key.worldY + num2);
								Vector3 pt7 = new Vector3(key.worldX, key.height, key.worldY + num2);
								Vector3 pt8 = new Vector3(key.worldX + num, boardSquare.height, key.worldY + num2);
								AddVerticalConnector(pt5, pt6, pt7, pt8, false);
							}
						}
					}
					goto IL_026a;
					IL_03b1:
					if (!IsPieceTypeInMask(value, PieceType.LowerLeftHorizontal))
					{
						if (!IsPieceTypeInMask(value, PieceType.LowerLeftInnerCorner))
						{
							goto IL_0485;
						}
					}
					BoardSquare boardSquare2;
					Vector3 pt9 = new Vector3(key.worldX - num2, boardSquare2.height, key.worldY - num);
					Vector3 pt10 = new Vector3(key.worldX - num2, key.height, key.worldY);
					Vector3 pt11 = new Vector3(key.worldX - num2, key.height, key.worldY - num);
					Vector3 pt12 = new Vector3(key.worldX - num2, boardSquare2.height, key.worldY);
					AddVerticalConnector(pt9, pt10, pt11, pt12, true);
					goto IL_0485;
					IL_067c:
					BoardSquare boardSquare3 = Board.Get().GetSquareFromIndex(key.x + 1, key.y);
					if (!(boardSquare3 != null))
					{
						continue;
					}
					if (!m_borderSquares.ContainsKey(boardSquare3))
					{
						continue;
					}
					if (!((float)(boardSquare3.height - key.height) >= num3))
					{
						continue;
					}
					if (!IsPieceTypeInMask(value, PieceType.UpperRightHorizontal))
					{
						if (!IsPieceTypeInMask(value, PieceType.UpperRightInnerCorner))
						{
							goto IL_07bd;
						}
					}
					Vector3 pt13 = new Vector3(key.worldX + num2, boardSquare3.height, key.worldY + num);
					Vector3 pt14 = new Vector3(key.worldX + num2, key.height, key.worldY);
					Vector3 pt15 = new Vector3(key.worldX + num2, key.height, key.worldY + num);
					Vector3 pt16 = new Vector3(key.worldX + num2, boardSquare3.height, key.worldY);
					AddVerticalConnector(pt13, pt14, pt15, pt16, true);
					goto IL_07bd;
					IL_0485:
					BoardSquare boardSquare4 = Board.Get().GetSquareFromIndex(key.x, key.y - 1);
					if (boardSquare4 != null)
					{
						if (m_borderSquares.ContainsKey(boardSquare4))
						{
							if ((float)(boardSquare4.height - key.height) >= num3)
							{
								if (IsPieceTypeInMask(value, PieceType.LowerLeftVertical) || IsPieceTypeInMask(value, PieceType.LowerLeftInnerCorner))
								{
									Vector3 pt17 = new Vector3(key.worldX - num, boardSquare4.height, key.worldY - num2);
									Vector3 pt18 = new Vector3(key.worldX, key.height, key.worldY - num2);
									Vector3 pt19 = new Vector3(key.worldX - num, key.height, key.worldY - num2);
									Vector3 pt20 = new Vector3(key.worldX, boardSquare4.height, key.worldY - num2);
									AddVerticalConnector(pt17, pt18, pt19, pt20, true);
								}
								if (!IsPieceTypeInMask(value, PieceType.LowerRightVertical))
								{
									if (!IsPieceTypeInMask(value, PieceType.LowerRightInnerCorner))
									{
										goto IL_067c;
									}
								}
								Vector3 pt21 = new Vector3(key.worldX, boardSquare4.height, key.worldY - num2);
								Vector3 pt22 = new Vector3(key.worldX + num, key.height, key.worldY - num2);
								Vector3 pt23 = new Vector3(key.worldX, key.height, key.worldY - num2);
								Vector3 pt24 = new Vector3(key.worldX + num, boardSquare4.height, key.worldY - num2);
								AddVerticalConnector(pt21, pt22, pt23, pt24, false);
							}
						}
					}
					goto IL_067c;
					IL_026a:
					boardSquare2 = Board.Get().GetSquareFromIndex(key.x - 1, key.y);
					if (boardSquare2 != null && m_borderSquares.ContainsKey(boardSquare2))
					{
						if ((float)(boardSquare2.height - key.height) >= num3)
						{
							if (!IsPieceTypeInMask(value, PieceType.UpperLeftHorizontal))
							{
								if (!IsPieceTypeInMask(value, PieceType.UpperLeftInnerCorner))
								{
									goto IL_03b1;
								}
							}
							Vector3 pt25 = new Vector3(key.worldX - num2, boardSquare2.height, key.worldY);
							Vector3 pt26 = new Vector3(key.worldX - num2, key.height, key.worldY + num);
							Vector3 pt27 = new Vector3(key.worldX - num2, key.height, key.worldY);
							Vector3 pt28 = new Vector3(key.worldX - num2, boardSquare2.height, key.worldY + num);
							AddVerticalConnector(pt25, pt26, pt27, pt28, false);
							goto IL_03b1;
						}
					}
					goto IL_0485;
					IL_07bd:
					if (!IsPieceTypeInMask(value, PieceType.LowerRightHorizontal))
					{
						if (!IsPieceTypeInMask(value, PieceType.LowerRightInnerCorner))
						{
							continue;
						}
					}
					Vector3 pt29 = new Vector3(key.worldX + num2, boardSquare3.height, key.worldY);
					Vector3 pt30 = new Vector3(key.worldX + num2, key.height, key.worldY - num);
					Vector3 pt31 = new Vector3(key.worldX + num2, key.height, key.worldY);
					Vector3 pt32 = new Vector3(key.worldX + num2, boardSquare3.height, key.worldY - num);
					AddVerticalConnector(pt29, pt30, pt31, pt32, false);
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

		public GameObject CreateMeshObject(Material borderMaterial)
		{
			GameObject gameObject = new GameObject("HighlightMesh");
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			Mesh mesh = meshFilter.mesh;
			mesh.vertices = m_pos.ToArray();
			mesh.uv = m_uv.ToArray();
			Vector3[] array = new Vector3[mesh.vertices.Length];
			for (int i = 0; i < mesh.normals.Length; i++)
			{
				array[i] = Vector3.up;
			}
			mesh.normals = array;
			int[] array2 = new int[m_pos.Count / 4 * 6];
			for (int j = 0; j < m_pos.Count / 4; j++)
			{
				array2[j * 6] = j * 4;
				array2[j * 6 + 1] = j * 4 + 1;
				array2[j * 6 + 2] = j * 4 + 2;
				array2[j * 6 + 3] = j * 4;
				array2[j * 6 + 4] = j * 4 + 3;
				array2[j * 6 + 5] = j * 4 + 1;
			}
			while (true)
			{
				mesh.triangles = array2;
				gameObject.GetComponent<Renderer>().material = borderMaterial;
				return gameObject;
			}
		}
	}

	public class HighlightPiece
	{
		public Vector3[] m_pos = new Vector3[4];

		public Vector2[] m_uv = new Vector2[4];

		public int[] m_indices = new int[6]
		{
			0,
			1,
			2,
			0,
			3,
			1
		};

		public Vector3[] m_nrm = new Vector3[4]
		{
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		};
	}

	public enum PieceType
	{
		LowerRightVertical,
		LowerRightHorizontal,
		LowerRightInnerCorner,
		LowerRightOuterCorner,
		UpperRightVertical,
		UpperRightHorizontal,
		UpperRightInnerCorner,
		UpperRightOuterCorner,
		UpperLeftVertical,
		UpperLeftHorizontal,
		UpperLeftInnerCorner,
		UpperLeftOuterCorner,
		LowerLeftVertical,
		LowerLeftHorizontal,
		LowerLeftInnerCorner,
		LowerLeftOuterCorner,
		NumTypes
	}

	private static HighlightUtils s_instance;

	public Color s_defaultMovementColor = new Color(1f, 1f, 1f, 0.5f);

	public Color s_defaultPotentialMovementColor = new Color(0f, 1f, 1f, 0.5f);

	public Color s_defaultChasingColor = new Color(1f, 1f, 1f, 0.75f);

	public Color s_defaultPotentialChasingColor = new Color(0f, 0.4f, 0.4f, 0.5f);

	public Color s_defaultLostMovementColor = new Color(0.5f, 0f, 0f, 0.5f);

	public Color s_knockbackMovementLineColor = new Color(1f, 0.9f, 0.2f, 0.75f);

	public Color s_attackMovementLineColor = new Color(1f, 0f, 0f, 0.75f);

	public Color s_dashMovementLineColor = new Color(1f, 0f, 0f, 0.75f);

	public Color s_teleportMovementLineColor = new Color(0f, 1f, 1f, 0.5f);

	public Color s_flashMovementLineColor = Color.white * 0.5f;

	public float s_defaultDimMultiplier = 0.33f;

	[Separator("Movement Line", true)]
	public Material m_ArrowLineMaterial;

	public Material m_ArrowChaseLineMaterial;

	public Material m_ArrowDottedLineMaterial;

	public MovementPathStart m_ArrowStartPrefab;

	public MovementPathEnd m_ArrowEndPrefab;

	public TeleportPathStart m_teleportPrefab;

	public TeleportPathEnd m_teleportEndPrefab;

	[Separator("Valid Squares Selection Highlight", true)]
	public Material m_highlightMaterial;

	public Material m_dottedHighlightMaterial;

	public GameObject m_movementGlowObject;

	public GameObject m_innerCornerPiece;

	public GameObject m_horizontalSidePiece;

	public GameObject m_verticalSidePiece;

	public GameObject m_outerCornerPiece;

	public GameObject m_highlightFloodFillSquare;

	[Separator("Mouseover Cursors", true)]
	public GameObject m_targetingCursorPrefab;

	public GameObject m_mouseOverCursorPrefab;

	public GameObject m_freeCursorPrefab;

	public GameObject m_movementPrefab;

	public GameObject m_abilityTargetPrefab;

	public GameObject m_IdleTargetPrefab;

	public GameObject m_cornerCursorPrefab;

	public GameObject m_sprintHighlightPrefab;

	public GameObject m_chaseSquareCursorPrefab;

	public GameObject m_chaseSquareCursorSecondaryPrefab;

	public GameObject m_respawnSelectionCursorPrefab;

	private GameObject m_targetingCursor;

	[Separator("Cover", true)]
	public GameObject m_coverIndicatorPrefab;

	public GameObject m_coverShieldOnlyPrefab;

	[Header("-- Mouse-over Cover Indicators --")]
	public GameObject m_mouseoverCoverShieldPrefab;

	public bool m_showMouseoverCoverIndicators = true;

	public float m_mouseoverCoverAreaRadius = 1.5f;

	public float m_mouseoverCoverIconAlpha = 0.1f;

	public float m_mouseoverHeightOffset = 2f;

	public CoverDirIndicatorParams m_mouseoverCoverDirParams;

	[Header("-- Move Into Cover Indicators --")]
	public bool m_showMoveIntoCoverIndicators;

	public MoveIntoCoverIndicatorTiming m_coverDirIndicatorTiming = MoveIntoCoverIndicatorTiming.ShowOnTurnStart;

	public float m_coverDirIndicatorRadiusInSquares = 3f;

	[Space(5f)]
	public float m_coverDirIndicatorDuration = 3f;

	public float m_coverDirIndicatorStartDelay = 0.5f;

	public float m_coverDirFadeoutStartDelay = 1f;

	[Space(5f)]
	public Color m_coverDirIndicatorColor = new Color(1f, 0.75f, 0.25f);

	public float m_coverDirIndicatorInitialOpacity = 0.08f;

	public float m_coverDirParticleInitialOpacity = 0.5f;

	[Separator("Ability Range Indicator", true)]
	public bool m_showAbilityRangeIndicator = true;

	public Color m_abilityRangeIndicatorColor = new Color(1f, 1f, 1f, 0.15f);

	public float m_abilityRangeIndicatorWidth = 0.075f;

	[Separator("Movement Line Preview", true)]
	public bool m_showMovementPreview = true;

	public Color m_movementLinePreviewColor = new Color(0f, 0.3f, 0.8f, 0.15f);

	[Separator("For Actor VFX", true)]
	public GameObject m_chaseMouseoverPrefab;

	public GameObject m_moveSquareCursorPrefab;

	public bool m_useActorVfxForMovementDebuff;

	public GameObject m_concealedVFXPrefab;

	public GameObject m_footstepsVFXPrefab;

	[Header("-- Generic Status VFX on Actor --")]
	public StatusVfxPrefabToJoint[] m_statusVfxPrefabToJoint;

	[Header("-- Knocked Down Status VFX --")]
	public GameObject m_knockedBackVFXPrefab;

	[JointPopup("KnockedDown Status VFX Attach Joint")]
	public JointPopupProperty m_knockedBackVfxJoint;

	[Separator("For Brush Regions", true)]
	public GameObject m_brushFunctioningSquarePrefab;

	public GameObject m_brushDisruptedSquarePrefab;

	[Space(5f)]
	public GameObject m_brushFunctioningBorderPrefab;

	public GameObject m_brushDisruptedBorderPrefab;

	public float m_brushBorderHeightOffset;

	[Header("-- for moving in/out of brush --")]
	public GameObject m_moveIntoBrushVfxPrefab;

	public GameObject m_moveOutOfBrushVfxPrefab;

	[Separator("On Death VFX", true)]
	public GameObject m_onDeathVFXPrefab;

	public float m_onDeathVFXDuration = 3f;

	[Separator("On Respawn VFX", true)]
	public GameObject m_onRespawnVFXPrefab;

	public float m_onRespawnVFXDuration = 3f;

	[Tooltip("On first turn of death, when you first pick the location")]
	public GameObject m_respawnPositionFlareFriendlyVFXPrefab;

	[Tooltip("On first turn of death, when you first pick the location")]
	public GameObject m_respawnPositionFlareEnemyVFXPrefab;

	[Tooltip("On second turn of death, when you are about to respawn in the location")]
	public GameObject m_respawnPositionFinalFriendlyVFXPrefab;

	[Tooltip("On second turn of death, when you are about to respawn in the location")]
	public GameObject m_respawnPositionFinalEnemyVFXPrefab;

	public Shader m_recentlySpawnedShader;

	[Separator("Hit in Cover VFX", true)]
	public GameObject m_hitInCoverVFXPrefab;

	public float m_hitInCoverVFXDuration = 2f;

	[Separator("Knockback Hit while Unstoppable", true)]
	public GameObject m_knockbackHitWhileUnstoppableVFXPrefab;

	public float m_knockbackHitWhileUnstoppableVFXDuration = 3f;

	[Space(10f)]
	public float m_targeterBoundaryLineLength = 3.75f;

	public Texture2D m_scrollCursorEdge;

	public Texture2D m_scrollCursorCorner;

	[Space(20f)]
	[Header("-- Targeter VFX: Aoe, Laser --")]
	public GameObject m_AoECursorPrefab;

	public GameObject m_AoECursorAllyPrefab;

	public GameObject m_rectangleCursorPrefab;

	public GameObject m_bouncingLaserTargeterPrefab;

	public float m_AoECursorRadius = 2.25f;

	[Space(10f)]
	public GameObject m_teamAPersistentAoEPrefab;

	public GameObject m_teamBPersistentAoEPrefab;

	public GameObject m_allyPersistentAoEPrefab;

	public GameObject m_allyPersistentHelpfulAoEPrefab;

	public GameObject m_enemyPersistentAoEPrefab;

	public GameObject m_enemyPersistentHelpfulAoEPrefab;

	[Header("-- Targeter VFX: Boundary Lines --")]
	public GameObject m_targeterBoundaryLineInner;

	public GameObject m_targeterBoundaryLineOuter;

	[Header("-- Targeter VFX: Shapes --")]
	public AreaShapePrefab[] m_areaShapePrefabs;

	public bool m_showTargetingArcsForShapes = true;

	public float m_minDistForTargetingArc = 5f;

	public float m_targetingArcMovementSpeed = 5f;

	public float m_targetingArcMaxHeight = 2.5f;

	public int m_targetingArcNumSegments = 16;

	public Color m_targetingArcColor = Color.cyan;

	public Color m_targetingArcColorAllies = Color.gray;

	public GameObject m_targetingArcForShape;

	public Material m_targetingArcMaterial;

	[Header("-- Targeter VFX: Cones --")]
	public GameObject[] m_conePrefabs;

	public GameObject m_dynamicConePrefab;

	[Header("-- Targeter VFX: Dynamic Line Segment --")]
	public GameObject m_dynamicLineSegmentPrefab;

	[Header("-- Targeter Opacity --")]
	public bool m_setTargeterOpacityWhileTargeting;

	public float m_targeterOpacityWhileTargeting = 0.15f;

	public float m_targeterParticleSystemOpacityMultiplier = 3f;

	public TargeterOpacityData[] m_confirmedTargeterOpacity;

	public TargeterOpacityData[] m_allyTargeterOpacity;

	[Header("-- Color/Opacity for removing targeter")]
	public TargeterOpacityData[] m_targeterRemoveFadeOpacity;

	public Color m_targeterRemoveColor = Color.red;

	[Header("-- Opacity for movement")]
	public TargeterOpacityData[] m_movementLineOpacity;

	[Separator("Ability Icon Color", true)]
	public Color m_allyTargetedAbilityIconColor = Color.yellow;

	public Color m_allyCooldownAbilityIconColor = Color.gray;

	public Color m_allyAvailableAbilityIconColor = Color.white;

	public Color m_allyTargetingAbilityIconColor = Color.white;

	[Separator("Hidden/Affected Square Indicators (while targeting)", true)]
	public GameObject m_hiddenSquarePrefab;

	public GameObject m_affectedSquarePrefab;

	public bool m_showAffectedSquaresWhileTargeting;

	[Separator("For Base Circle Indicator", true)]
	public GameObject m_friendBaseCirclePrefab;

	public GameObject m_enemyBaseCirclePrefab;

	public float m_circlePrefabHeight;

	[JointPopup("Base Circle VFX Attach Joint")]
	public JointPopupProperty m_baseCircleJoint;

	public float m_baseCircleSizeInSquares = 1f;

	[Separator("Nameplate Setting", true)]
	public bool m_enableAccumulatedAllyNumbers;

	private bool m_startCalled;

	private bool m_hideCursor;

	private CursorType m_currentCursorType = CursorType.NoCursorType;

	private bool m_isCursorSet;

	private List<GameObject> m_highlightCursors = new List<GameObject>();

	private MouseoverCoverManager m_mouseoverCoverManager;

	private GameObject m_mouseoverCoverParent;

	private HighlightPiece[] m_highlightPieces = new HighlightPiece[16];

	private GameObject m_surroundedSquaresParent;

	private SquareIndicators m_surroundedSquareIndicators = new SquareIndicators(CreateSurroundedSquareObject, 15, 10, 0.12f);

	private GameObject m_hiddenSquareIndicatorParent;

	private SquareIndicators m_hiddenSquareIndicators;

	private GameObject m_affectedSquareIndicatorParent;

	private SquareIndicators m_affectedSquareIndicators;

	private GameObject m_abilityRangeIndicatorHighlight;

	private List<bool> m_rangeIndicatorMouseOverFlags;

	private float m_lastRangeIndicatorRadius;

	public bool m_cachedShouldShowAffectedSquares;

	[CompilerGenerated]
	private static SquareIndicators.CreateIndicatorDelegate _003C_003Ef__mg_0024cache0;

	//[CompilerGenerated]
	//private static SquareIndicators.CreateIndicatorDelegate CreateHiddenSquareIndicatorObject;

	//[CompilerGenerated]
	//private static SquareIndicators.CreateIndicatorDelegate CreateAffectedSquareIndicatorObject;

	internal GameObject ClampedMouseOverCursor
	{
		get;
		private set;
	}

	internal GameObject FreeMouseOverCursor
	{
		get;
		private set;
	}

	internal GameObject CornerMouseOverCursor
	{
		get;
		private set;
	}

	public GameObject MovementMouseOverCursor
	{
		get;
		private set;
	}

	internal GameObject AbilityTargetMouseOverCursor
	{
		get;
		private set;
	}

	internal GameObject IdleMouseOverCursor
	{
		get;
		private set;
	}

	public GameObject SprintMouseOverCursor
	{
		get;
		private set;
	}

	internal GameObject ChaseSquareCursor
	{
		get;
		private set;
	}

	internal GameObject ChaseSquareCursorAlt
	{
		get;
		private set;
	}

	internal GameObject RespawnSelectionCursor
	{
		get;
		private set;
	}

	internal Dictionary<ScrollCursorDirection, Texture2D> ScrollCursors
	{
		get;
		private set;
	}

	internal Dictionary<ScrollCursorDirection, Vector2> ScrollCursorsHotspot
	{
		get;
		private set;
	}

	public bool HideCursor
	{
		get
		{
			return m_hideCursor;
		}
		set
		{
			m_hideCursor = value;
			RefreshCursorVisibility();
		}
	}

	public HighlightUtils()
	{
		
		m_hiddenSquareIndicators = new SquareIndicators(CreateHiddenSquareIndicatorObject, 15, 10, 0.09f);
		
		m_affectedSquareIndicators = new SquareIndicators(CreateAffectedSquareIndicatorObject, 15, 10, 0.09f);
		m_rangeIndicatorMouseOverFlags = new List<bool>();
		m_lastRangeIndicatorRadius = -1f;
		
	}

	public static HighlightUtils Get()
	{
		return s_instance;
	}

	public static void DestroyBoundaryHighlightObject(GameObject highlightObject)
	{
		if (highlightObject != null)
		{
			Renderer[] componentsInChildren = highlightObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer.material != null)
				{
					UnityEngine.Object.Destroy(renderer.material);
				}
			}
			MeshFilter[] components = highlightObject.GetComponents<MeshFilter>();
			foreach (MeshFilter meshFilter in components)
			{
				UnityEngine.Object.Destroy(meshFilter.mesh);
			}
			UnityEngine.Object.DestroyImmediate(highlightObject);
		}
		if (s_instance != null)
		{
			s_instance.m_surroundedSquareIndicators.HideAllSquareIndicators();
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.VisualSceneLoaded)
		{
			return;
		}
		while (true)
		{
			m_surroundedSquareIndicators.Initialize();
			m_surroundedSquareIndicators.HideAllSquareIndicators();
			m_hiddenSquareIndicators.Initialize();
			m_hiddenSquareIndicators.HideAllSquareIndicators();
			m_mouseoverCoverManager.Initialize(m_mouseoverCoverParent);
			CreateRangeIndicatorHighlight();
			return;
		}
	}

	public static void DestroyMeshesOnObject(GameObject obj)
	{
		if (!(obj != null))
		{
			return;
		}
		while (true)
		{
			MeshFilter[] components = obj.GetComponents<MeshFilter>();
			MeshFilter[] array = components;
			foreach (MeshFilter meshFilter in array)
			{
				UnityEngine.Object.Destroy(meshFilter.mesh);
			}
			while (true)
			{
				MeshFilter[] componentsInChildren = obj.GetComponentsInChildren<MeshFilter>(true);
				MeshFilter[] array2 = componentsInChildren;
				foreach (MeshFilter meshFilter2 in array2)
				{
					UnityEngine.Object.Destroy(meshFilter2.mesh);
				}
				while (true)
				{
					switch (5)
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

	public static void DestroyMaterials(Material[] materials)
	{
		if (materials == null)
		{
			return;
		}
		foreach (Material material in materials)
		{
			if (material != null)
			{
				UnityEngine.Object.Destroy(material);
			}
		}
	}

	public static void SetParticleSystemScale(GameObject particleObject, float scale)
	{
		ParticleSystem[] componentsInChildren = particleObject.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSystem in array)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			ParticleSystem.MinMaxCurve startSize = main.startSize;
			startSize.constant *= scale;
			main.startSize = startSize;
		}
		while (true)
		{
			return;
		}
	}

	public static void DestroyObjectAndMaterials(GameObject highlightObj)
	{
		if (!(highlightObj != null))
		{
			return;
		}
		while (true)
		{
			Renderer[] components = highlightObj.GetComponents<Renderer>();
			Renderer[] array = components;
			foreach (Renderer renderer in array)
			{
				DestroyMaterials(renderer.materials);
			}
			Renderer[] componentsInChildren = highlightObj.GetComponentsInChildren<Renderer>(true);
			Renderer[] array2 = componentsInChildren;
			foreach (Renderer renderer2 in array2)
			{
				DestroyMaterials(renderer2.materials);
			}
			while (true)
			{
				UnityEngine.Object.Destroy(highlightObj);
				return;
			}
		}
	}

	private void Awake()
	{
		InitializeHighlightPieces();
		s_instance = this;
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
		m_mouseoverCoverParent = new GameObject("MouseoverCoverIndicators");
		UnityEngine.Object.DontDestroyOnLoad(m_mouseoverCoverParent);
		m_mouseoverCoverManager = new MouseoverCoverManager();
	}

	private void OnDestroy()
	{
		s_instance = null;
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
		m_surroundedSquareIndicators.ClearAllSquareIndicators();
		if (m_surroundedSquaresParent != null)
		{
			UnityEngine.Object.Destroy(m_surroundedSquaresParent);
			m_surroundedSquaresParent = null;
		}
		m_hiddenSquareIndicators.ClearAllSquareIndicators();
		if (m_hiddenSquareIndicatorParent != null)
		{
			UnityEngine.Object.Destroy(m_hiddenSquareIndicatorParent);
			m_hiddenSquareIndicatorParent = null;
		}
		m_affectedSquareIndicators.ClearAllSquareIndicators();
		if (m_affectedSquareIndicatorParent != null)
		{
			UnityEngine.Object.Destroy(m_affectedSquareIndicatorParent);
			m_affectedSquareIndicatorParent = null;
		}
		if (m_mouseoverCoverParent != null)
		{
			UnityEngine.Object.Destroy(m_mouseoverCoverParent);
			m_mouseoverCoverParent = null;
		}
		DestroyRangeIndicatorHighlight();
	}

	private void Initialize2DScrollCursors()
	{
		if (!(m_scrollCursorEdge != null))
		{
			if (!(m_scrollCursorCorner != null))
			{
				return;
			}
		}
		ScrollCursors = new Dictionary<ScrollCursorDirection, Texture2D>();
		ScrollCursorsHotspot = new Dictionary<ScrollCursorDirection, Vector2>();
		IEnumerator enumerator = Enum.GetValues(typeof(ScrollCursorDirection)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ScrollCursorDirection scrollCursorDirection = (ScrollCursorDirection)enumerator.Current;
				int num;
				int num4;
				int num5;
				int num9;
				switch (scrollCursorDirection)
				{
				case ScrollCursorDirection.E:
				case ScrollCursorDirection.W:
					if (m_scrollCursorEdge != null)
					{
						ScrollCursors[scrollCursorDirection] = new Texture2D(m_scrollCursorEdge.width, m_scrollCursorEdge.height);
						int num6;
						if (scrollCursorDirection == ScrollCursorDirection.W)
						{
							num6 = 0;
						}
						else
						{
							num6 = m_scrollCursorEdge.width - 1;
						}
						int num7 = num6;
						int num8 = m_scrollCursorEdge.height / 2;
						ScrollCursorsHotspot[scrollCursorDirection] = new Vector2(num7, num8);
					}
					break;
				case ScrollCursorDirection.N:
				case ScrollCursorDirection.S:
					if (m_scrollCursorEdge != null)
					{
						ScrollCursors[scrollCursorDirection] = new Texture2D(m_scrollCursorEdge.height, m_scrollCursorEdge.width);
						int num2 = m_scrollCursorEdge.height / 2;
						int num3 = (scrollCursorDirection != 0) ? (m_scrollCursorEdge.width - 1) : 0;
						ScrollCursorsHotspot[scrollCursorDirection] = new Vector2(num2, num3);
					}
					break;
				case ScrollCursorDirection.NE:
				case ScrollCursorDirection.SE:
				case ScrollCursorDirection.SW:
				case ScrollCursorDirection.NW:
					{
						if (!(m_scrollCursorCorner != null))
						{
							break;
						}
						ScrollCursors[scrollCursorDirection] = new Texture2D(m_scrollCursorCorner.width, m_scrollCursorCorner.height);
						if (scrollCursorDirection != ScrollCursorDirection.NW)
						{
							if (scrollCursorDirection != ScrollCursorDirection.SW)
							{
								num = m_scrollCursorCorner.width - 1;
								goto IL_0223;
							}
						}
						num = 0;
						goto IL_0223;
					}
					IL_0223:
					num4 = num;
					if (scrollCursorDirection != ScrollCursorDirection.NW)
					{
						if (scrollCursorDirection != ScrollCursorDirection.NE)
						{
							num5 = m_scrollCursorCorner.height - 1;
							goto IL_0253;
						}
					}
					num5 = 0;
					goto IL_0253;
					IL_0253:
					num9 = num5;
					ScrollCursorsHotspot[scrollCursorDirection] = new Vector2(num4, num9);
					break;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_0287;
					}
				}
			}
			end_IL_0287:;
		}
		for (int i = 0; i < m_scrollCursorEdge.width; i++)
		{
			for (int j = 0; j < m_scrollCursorEdge.height; j++)
			{
				if (m_scrollCursorEdge != null)
				{
					ScrollCursors[ScrollCursorDirection.W].SetPixel(i, j, m_scrollCursorEdge.GetPixel(i, j));
					ScrollCursors[ScrollCursorDirection.E].SetPixel(i, j, m_scrollCursorEdge.GetPixel(m_scrollCursorEdge.width - (i + 1), j));
					ScrollCursors[ScrollCursorDirection.N].SetPixel(i, j, m_scrollCursorEdge.GetPixel(m_scrollCursorEdge.height - (j + 1), i));
					ScrollCursors[ScrollCursorDirection.S].SetPixel(i, j, m_scrollCursorEdge.GetPixel(j, i));
				}
				if (m_scrollCursorCorner != null)
				{
					ScrollCursors[ScrollCursorDirection.NW].SetPixel(i, j, m_scrollCursorCorner.GetPixel(i, j));
					ScrollCursors[ScrollCursorDirection.SW].SetPixel(i, j, m_scrollCursorCorner.GetPixel(i, m_scrollCursorCorner.height - (j + 1)));
					ScrollCursors[ScrollCursorDirection.NE].SetPixel(i, j, m_scrollCursorCorner.GetPixel(m_scrollCursorCorner.width - (i + 1), j));
					ScrollCursors[ScrollCursorDirection.SE].SetPixel(i, j, m_scrollCursorCorner.GetPixel(m_scrollCursorCorner.width - (i + 1), m_scrollCursorCorner.height - (j + 1)));
				}
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Start()
	{
		m_highlightCursors.Clear();
		m_targetingCursor = UnityEngine.Object.Instantiate(m_targetingCursorPrefab);
		m_targetingCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(m_targetingCursor, false);
		m_highlightCursors.Add(m_targetingCursor);
		ClampedMouseOverCursor = UnityEngine.Object.Instantiate(m_mouseOverCursorPrefab);
		ClampedMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(ClampedMouseOverCursor, false);
		m_highlightCursors.Add(ClampedMouseOverCursor);
		FreeMouseOverCursor = UnityEngine.Object.Instantiate(m_freeCursorPrefab);
		FreeMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(FreeMouseOverCursor, false);
		m_highlightCursors.Add(FreeMouseOverCursor);
		CornerMouseOverCursor = UnityEngine.Object.Instantiate(m_cornerCursorPrefab);
		CornerMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(CornerMouseOverCursor, false);
		m_highlightCursors.Add(CornerMouseOverCursor);
		MovementMouseOverCursor = UnityEngine.Object.Instantiate(m_movementPrefab);
		MovementMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(MovementMouseOverCursor, false);
		m_highlightCursors.Add(MovementMouseOverCursor);
		AbilityTargetMouseOverCursor = UnityEngine.Object.Instantiate(m_abilityTargetPrefab);
		AbilityTargetMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(AbilityTargetMouseOverCursor, false);
		m_highlightCursors.Add(AbilityTargetMouseOverCursor);
		IdleMouseOverCursor = UnityEngine.Object.Instantiate(m_IdleTargetPrefab);
		IdleMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(IdleMouseOverCursor, false);
		m_highlightCursors.Add(IdleMouseOverCursor);
		if (m_respawnSelectionCursorPrefab != null)
		{
			RespawnSelectionCursor = UnityEngine.Object.Instantiate(m_respawnSelectionCursorPrefab);
			RespawnSelectionCursor.transform.parent = base.transform;
			UIManager.SetGameObjectActive(RespawnSelectionCursor, false);
			m_highlightCursors.Add(RespawnSelectionCursor);
		}
		if (m_chaseSquareCursorPrefab != null)
		{
			ChaseSquareCursor = UnityEngine.Object.Instantiate(m_chaseSquareCursorPrefab);
			ChaseSquareCursor.transform.parent = base.transform;
			UIManager.SetGameObjectActive(ChaseSquareCursor, false);
			m_highlightCursors.Add(ChaseSquareCursor);
		}
		if (m_chaseSquareCursorSecondaryPrefab != null)
		{
			ChaseSquareCursorAlt = UnityEngine.Object.Instantiate(m_chaseSquareCursorSecondaryPrefab);
			ChaseSquareCursorAlt.transform.parent = base.transform;
			UIManager.SetGameObjectActive(ChaseSquareCursorAlt, false);
			m_highlightCursors.Add(ChaseSquareCursorAlt);
		}
		Initialize2DScrollCursors();
		m_startCalled = true;
		RefreshCursorVisibility();
	}

	public void HideCursorHighlights()
	{
		for (int i = 0; i < m_highlightCursors.Count; i++)
		{
			GameObject gameObject = m_highlightCursors[i];
			if (gameObject != null)
			{
				UIManager.SetGameObjectActive(gameObject, false);
			}
		}
		while (true)
		{
			return;
		}
	}

	public GameObject CloneTargeterHighlight(GameObject source, AbilityUtil_Targeter targeter)
	{
		if (source != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					UIDynamicCone component = source.GetComponent<UIDynamicCone>();
					if (component != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
							{
								GameObject gameObject = CreateDynamicConeMesh(component.m_currentRadiusInWorld / Board.Get().squareSize, component.m_currentAngleInWorld, component.m_forceHideSides, targeter.GetTemplateSwapData());
								gameObject.transform.position = source.transform.position;
								gameObject.transform.rotation = source.transform.rotation;
								gameObject.transform.localScale = source.transform.localScale;
								return gameObject;
							}
							}
						}
					}
					UIDynamicLineSegment component2 = source.GetComponent<UIDynamicLineSegment>();
					if (component2 != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
							{
								GameObject gameObject2 = CreateDynamicLineSegmentMesh(component2.m_currentLengthInWorld / Board.Get().squareSize, component2.m_currentWidthInWorld, component2.m_dotted, component2.m_currentColor);
								gameObject2.transform.position = source.transform.position;
								gameObject2.transform.rotation = source.transform.rotation;
								gameObject2.transform.localScale = source.transform.localScale;
								return gameObject2;
							}
							}
						}
					}
					return UnityEngine.Object.Instantiate(source);
				}
				}
			}
		}
		return null;
	}

	public GameObject GetTargeterTemplatePrefabToUse(GameObject input, TargeterTemplateSwapData.TargeterTemplateType inputTemplateType, List<TargeterTemplateSwapData> templateSwaps)
	{
		GameObject result = input;
		if (input != null && templateSwaps != null)
		{
			if (templateSwaps.Count > 0)
			{
				int num = 0;
				while (true)
				{
					if (num < templateSwaps.Count)
					{
						if (templateSwaps[num].m_templateToReplace == inputTemplateType)
						{
							if (templateSwaps[num].m_prefabToUse != null)
							{
								result = templateSwaps[num].m_prefabToUse;
								break;
							}
						}
						num++;
						continue;
					}
					break;
				}
			}
		}
		return result;
	}

	public GameObject CreateRectangularCursor(float widthInWorld, float lengthInWorld, List<TargeterTemplateSwapData> templateSwapData = null)
	{
		GameObject gameObject = null;
		GameObject gameObject2 = m_rectangleCursorPrefab;
		if (templateSwapData != null)
		{
			gameObject2 = GetTargeterTemplatePrefabToUse(gameObject2, TargeterTemplateSwapData.TargeterTemplateType.Laser, templateSwapData);
		}
		gameObject = UnityEngine.Object.Instantiate(gameObject2);
		UIRectangleCursor component = gameObject.GetComponent<UIRectangleCursor>();
		if (component != null)
		{
			component.OnDimensionsChanged(widthInWorld, lengthInWorld);
		}
		return gameObject;
	}

	public void ResizeRectangularCursor(float widthInWorld, float lengthInWorld, GameObject highlight)
	{
		highlight.GetComponent<UIRectangleCursor>().OnDimensionsChanged(widthInWorld, lengthInWorld);
	}

	public void RotateAndResizeRectangularCursor(GameObject highlightObj, Vector3 startPos, Vector3 endPos, float widthInSquares)
	{
		startPos.y = GetHighlightHeight();
		endPos.y = startPos.y;
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		Vector3 vector = endPos - startPos;
		float magnitude = vector.magnitude;
		Get().ResizeRectangularCursor(widthInWorld, magnitude, highlightObj);
		Vector3 normalized = vector.normalized;
		highlightObj.transform.position = startPos;
		highlightObj.transform.rotation = Quaternion.LookRotation(normalized);
	}

	public GameObject CreateConeCursor(float radiusInWorld, float arcDegrees)
	{
		return CreateDynamicConeMesh(radiusInWorld / Board.Get().squareSize, arcDegrees, false);
	}

	public GameObject CreateConeCursor_FixedSize(float radiusInWorld, float arcDegrees)
	{
		GameObject gameObject = null;
		GameObject original = null;
		float num = float.MaxValue;
		GameObject[] conePrefabs = m_conePrefabs;
		foreach (GameObject gameObject2 in conePrefabs)
		{
			UIConeCursor component = gameObject2.GetComponent<UIConeCursor>();
			if (component == null)
			{
				Log.Error("HighlightUtils cone prefabs has a cone without a UIConeCursor component.");
				continue;
			}
			if (!(component.m_arcDegrees < 0f))
			{
				if (!(component.m_arcDegrees > 360f))
				{
					float num2 = Mathf.Abs(arcDegrees - component.m_arcDegrees);
					if (num2 < num)
					{
						num = num2;
						original = gameObject2;
					}
					continue;
				}
			}
			Log.Error(new StringBuilder().Append("HighlightUtils cone prefabs has a cone ").Append(gameObject2.name).Append(" with an invalid arcDegrees.  Valid degrees are in (0, 360).").ToString());
		}
		while (true)
		{
			gameObject = UnityEngine.Object.Instantiate(original);
			UIConeCursor component2 = gameObject.GetComponent<UIConeCursor>();
			component2.OnRadiusChanged(radiusInWorld);
			return gameObject;
		}
	}

	public GameObject CreateDynamicConeMesh(float initialRadiusInSquares, float initialAngle, bool forceHideSides, List<TargeterTemplateSwapData> templateSwapData = null)
	{
		GameObject gameObject = null;
		GameObject gameObject2 = m_dynamicConePrefab;
		if (templateSwapData != null)
		{
			gameObject2 = GetTargeterTemplatePrefabToUse(gameObject2, TargeterTemplateSwapData.TargeterTemplateType.DynamicCone, templateSwapData);
		}
		if (gameObject2 != null)
		{
			gameObject = UnityEngine.Object.Instantiate(gameObject2);
			if (gameObject != null)
			{
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
				if (component != null)
				{
					component.InitCone();
					component.SetForceHideSides(forceHideSides);
					AdjustDynamicConeMesh(gameObject, initialRadiusInSquares, initialAngle);
				}
			}
		}
		return gameObject;
	}

	public void AdjustDynamicConeMesh(GameObject highlight, float radiusInSquares, float angleDeg)
	{
		if (!(highlight != null))
		{
			return;
		}
		while (true)
		{
			UIDynamicCone component = highlight.GetComponent<UIDynamicCone>();
			if (component != null)
			{
				component.AdjustConeMeshVertices(angleDeg, radiusInSquares * Board.Get().squareSize);
			}
			return;
		}
	}

	public void SetDynamicConeMeshBorderActive(GameObject highlight, bool borderActive)
	{
		if (!(highlight != null))
		{
			return;
		}
		while (true)
		{
			UIDynamicCone component = highlight.GetComponent<UIDynamicCone>();
			if (component != null)
			{
				component.SetBorderActive(borderActive);
			}
			return;
		}
	}

	public GameObject CreateDynamicLineSegmentMesh(float lengthInSquares, float widthInWorld, bool dotted, Color color)
	{
		GameObject gameObject = null;
		if (m_dynamicLineSegmentPrefab != null)
		{
			gameObject = UnityEngine.Object.Instantiate(m_dynamicLineSegmentPrefab);
			UIDynamicLineSegment component = gameObject.GetComponent<UIDynamicLineSegment>();
			if (component != null)
			{
				float lengthInWorld = lengthInSquares * Board.Get().squareSize;
				component.CreateSegmentMesh(widthInWorld, dotted, color);
				component.AdjustDynamicLineSegmentMesh(lengthInWorld, color);
			}
		}
		else
		{
			Log.Error("no dynamic line segment prefab");
			gameObject = new GameObject("Error_NoDynamicLineSegmentPrefab");
		}
		return gameObject;
	}

	public void AdjustDynamicLineSegmentMesh(GameObject highlight, float lengthInSquares, Color color)
	{
		if (!(highlight != null))
		{
			return;
		}
		while (true)
		{
			UIDynamicLineSegment component = highlight.GetComponent<UIDynamicLineSegment>();
			if (component != null)
			{
				component.AdjustDynamicLineSegmentMesh(lengthInSquares * Board.Get().squareSize, color);
			}
			return;
		}
	}

	public void AdjustDynamicLineSegmentLength(GameObject highlight, float lengthInSquares)
	{
		if (!(highlight != null))
		{
			return;
		}
		UIDynamicLineSegment component = highlight.GetComponent<UIDynamicLineSegment>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			component.AdjustSegmentLength(lengthInSquares * Board.Get().squareSize);
			return;
		}
	}

	public GameObject CreateBouncingLaserCursor(Vector3 originalStart, List<Vector3> laserAnglePoints, float width)
	{
		GameObject gameObject = null;
		gameObject = UnityEngine.Object.Instantiate(m_bouncingLaserTargeterPrefab);
		UIBouncingLaserCursor component = gameObject.GetComponent<UIBouncingLaserCursor>();
		component.OnUpdated(originalStart, laserAnglePoints, width);
		return gameObject;
	}

	public GameObject CreateAoECursor(float radiusInWorld, bool isForLocalPlayer)
	{
		GameObject gameObject = null;
		if (GameFlowData.Get().activeOwnedActorData == null)
		{
			isForLocalPlayer = true;
		}
		if (isForLocalPlayer)
		{
			gameObject = UnityEngine.Object.Instantiate(m_AoECursorPrefab);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate(m_AoECursorAllyPrefab);
		}
		float scale = radiusInWorld / m_AoECursorRadius;
		SetParticleSystemScale(gameObject, scale);
		return gameObject;
	}

	public GameObject CreateShapeCursor(AbilityAreaShape shape, bool isForLocalPlayer)
	{
		GameObject gameObject = null;
		if (GameFlowData.Get().activeOwnedActorData == null)
		{
			isForLocalPlayer = true;
		}
		AreaShapePrefab[] areaShapePrefabs = m_areaShapePrefabs;
		for (int i = 0; i < areaShapePrefabs.Length; i++)
		{
			AreaShapePrefab areaShapePrefab = areaShapePrefabs[i];
			if (areaShapePrefab.m_shape == shape)
			{
				gameObject = UnityEngine.Object.Instantiate(areaShapePrefab.m_prefab);
				break;
			}
		}
		if (gameObject == null)
		{
			float num = (float)shape;
			gameObject = CreateAoECursor((num + 1.5f) * 0.4f, isForLocalPlayer);
		}
		return gameObject;
	}

	public GameObject CreateBoundaryLine(float lengthInSquares, bool innerVersion, bool openingDirection)
	{
		GameObject gameObject = null;
		if (innerVersion)
		{
			gameObject = UnityEngine.Object.Instantiate(m_targeterBoundaryLineInner);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate(m_targeterBoundaryLineOuter);
		}
		float x;
		if (openingDirection)
		{
			x = 1f;
		}
		else
		{
			x = -1f;
		}
		float z = lengthInSquares * Board.Get().squareSize / m_targeterBoundaryLineLength;
		gameObject.transform.localScale = new Vector3(x, 1f, z);
		return gameObject;
	}

	public void ResizeBoundaryLine(float lengthInSquares, GameObject highlight)
	{
		float z = lengthInSquares * Board.Get().squareSize / m_targeterBoundaryLineLength;
		Transform transform = highlight.transform;
		Vector3 localScale = highlight.transform.localScale;
		transform.localScale = new Vector3(localScale.x, 1f, z);
	}

	public GameObject CreateGridPatternHighlight(AbilityGridPattern pattern, float scale)
	{
		GameObject gameObject = new GameObject(new StringBuilder().Append("Targeter parent: ").Append(pattern).ToString());
		List<GameObject> list;
		switch (pattern)
		{
		case AbilityGridPattern.Plus_Two_x_Two:
			list = CreatePlusPatternObjects(scale * Board.Get().squareSize * 2f);
			break;
		case AbilityGridPattern.Plus_Four_x_Four:
			list = CreatePlusPatternObjects(scale * Board.Get().squareSize * 4f);
			break;
		default:
			list = new List<GameObject>();
			break;
		}
		using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				current.transform.parent = gameObject.transform;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return gameObject;
					}
					/*OpCode not supported: LdMemberToken*/;
					return gameObject;
				}
			}
		}
	}

	private List<GameObject> CreatePlusPatternObjects(float scale)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(Get().CreateBoundaryLine(1f, false, true));
		list.Add(Get().CreateBoundaryLine(1f, false, false));
		List<GameObject> list2 = new List<GameObject>();
		list2.Add(Get().CreateBoundaryLine(1f, false, true));
		list2.Add(Get().CreateBoundaryLine(1f, false, false));
		Vector3 vector = new Vector3(1f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 1f);
		List<GameObject> list3 = new List<GameObject>();
		list3.AddRange(list);
		list3.AddRange(list2);
		using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				current.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
				current.transform.localPosition += 0.5f * scale * vector;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_00c1;
				}
			}
			end_IL_00c1:;
		}
		foreach (GameObject item in list2)
		{
			item.transform.rotation = Quaternion.LookRotation(vector2, Vector3.up);
			item.transform.localPosition += 0.5f * scale * vector2;
		}
		using (List<GameObject>.Enumerator enumerator3 = list3.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				GameObject current3 = enumerator3.Current;
				Get().ResizeBoundaryLine(scale / Board.Get().squareSize, current3);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return list3;
				}
			}
		}
	}

	public GameObject CreateAoEPersistentVFX(float radius, Team team, Vector3 position)
	{
		GameObject gameObject = null;
		if (team == Team.TeamA)
		{
			gameObject = UnityEngine.Object.Instantiate(m_teamAPersistentAoEPrefab);
		}
		else if (team == Team.TeamB)
		{
			gameObject = UnityEngine.Object.Instantiate(m_teamBPersistentAoEPrefab);
		}
		else
		{
			Log.Error("Someone not on team A or team B doing a firebomb, which has no appropriate VFX.");
		}
		if (gameObject != null)
		{
			float scale = radius / m_AoECursorRadius;
			SetParticleSystemScale(gameObject, scale);
			gameObject.transform.position = position;
		}
		return gameObject;
	}

	public GameObject CreateAoEPersistentVFX(float radius, bool allied, bool helpful, Vector3 position)
	{
		GameObject gameObject = null;
		if (allied)
		{
			if (helpful)
			{
				gameObject = UnityEngine.Object.Instantiate(m_allyPersistentHelpfulAoEPrefab);
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate(m_allyPersistentAoEPrefab);
			}
		}
		else if (helpful)
		{
			gameObject = UnityEngine.Object.Instantiate(m_enemyPersistentHelpfulAoEPrefab);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate(m_enemyPersistentAoEPrefab);
		}
		if (gameObject != null)
		{
			float scale = radius / m_AoECursorRadius;
			SetParticleSystemScale(gameObject, scale);
			gameObject.transform.position = position;
		}
		return gameObject;
	}

	public CursorType GetCurrentCursorType()
	{
		return m_currentCursorType;
	}

	public void SetCursorType(CursorType cursorType)
	{
		m_currentCursorType = cursorType;
		RefreshCursorVisibility();
	}

	private bool ShouldShowSprintingCursor()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		object obj;
		if ((bool)activeOwnedActorData)
		{
			obj = activeOwnedActorData.GetActorMovement();
		}
		else
		{
			obj = null;
		}
		ActorMovement actorMovement = (ActorMovement)obj;
		object obj2;
		if ((bool)activeOwnedActorData)
		{
			obj2 = activeOwnedActorData.GetActorTurnSM();
		}
		else
		{
			obj2 = null;
		}
		ActorTurnSM actorTurnSM = (ActorTurnSM)obj2;
		if (!(activeOwnedActorData == null))
		{
			if (!(actorMovement == null))
			{
				if (actorMovement.SquaresCanMoveToWithQueuedAbility.Contains(Board.Get().PlayerClampedSquare))
				{
					return false;
				}
				if (activeOwnedActorData.RemainingHorizontalMovement <= 0f)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (activeOwnedActorData.MoveFromBoardSquare == Board.Get().PlayerClampedSquare)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (!actorTurnSM.AmStillDeciding())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (activeOwnedActorData.IsDead())
				{
					return false;
				}
				if (actorTurnSM.CurrentState == TurnStateEnum.PICKING_RESPAWN)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (!activeOwnedActorData.GetAbilityData().GetQueuedAbilitiesAllowSprinting())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (UIGameStatsWindow.Get().m_container.gameObject.activeSelf)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				return true;
			}
		}
		return false;
	}

	private void RefreshCursorVisibility()
	{
		if (!m_startCalled)
		{
			return;
		}
		while (true)
		{
			if (Board.Get() == null || GameFlowData.Get() == null)
			{
				return;
			}
			while (true)
			{
				if (HUD_UI.Get() == null)
				{
					while (true)
					{
						switch (3)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				object obj;
				if ((bool)activeOwnedActorData)
				{
					obj = activeOwnedActorData.GetActorMovement();
				}
				else
				{
					obj = null;
				}
				ActorMovement exists = (ActorMovement)obj;
				int num;
				if (ActorTurnSM.IsClientDecidingMovement())
				{
					num = (ActorData.WouldSquareBeChasedByClient(Board.Get().PlayerFreeSquare) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag = (byte)num != 0;
				int num2;
				if (Board.Get().PlayerClampedSquare != null)
				{
					num2 = (Board.Get().PlayerClampedSquare.IsValidForGameplay() ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				bool flag2 = (byte)num2 != 0;
				bool flag3 = true;
				bool flag4 = true;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				bool flag9 = false;
				bool flag10 = false;
				bool flag11 = false;
				bool flag12 = false;
				if (GameFlowData.Get().gameState != GameState.EndingGame)
				{
					if (GameFlowData.Get().gameState != GameState.Deployment)
					{
						goto IL_0148;
					}
				}
				flag3 = false;
				flag4 = false;
				bool flag13 = false;
				flag = false;
				flag2 = false;
				flag11 = false;
				flag12 = false;
				goto IL_0148;
				IL_0148:
				if (!HideCursor)
				{
					if (!UIUtils.IsMouseOnGUI())
					{
						switch (m_currentCursorType)
						{
						case CursorType.MouseOverCursorType:
							flag3 = false;
							if (flag2)
							{
								if (!flag)
								{
									flag8 = true;
									goto IL_021d;
								}
							}
							flag8 = false;
							goto IL_021d;
						case CursorType.ShapeTargetingSquareCursorType:
							flag3 = false;
							flag8 = false;
							flag4 = true;
							flag9 = false;
							flag11 = false;
							break;
						case CursorType.ShapeTargetingCornerCursorType:
							flag3 = false;
							flag8 = false;
							flag4 = false;
							flag9 = true;
							flag11 = false;
							break;
						case CursorType.TabTargetingCursorType:
							flag3 = true;
							flag8 = false;
							flag4 = false;
							flag9 = false;
							flag11 = false;
							break;
						case CursorType.NoCursorType:
							{
								flag3 = false;
								flag8 = false;
								flag4 = false;
								flag9 = false;
								flag11 = false;
								break;
							}
							IL_021d:
							if (flag)
							{
								flag11 = true;
							}
							else
							{
								flag11 = false;
							}
							if (Board.Get().PlayerFreeSquare != null)
							{
								flag4 = true;
							}
							else
							{
								flag4 = false;
							}
							flag9 = false;
							if (!exists)
							{
								break;
							}
							if (ShouldShowSprintingCursor())
							{
								flag10 = true;
							}
							if (activeOwnedActorData.RemainingHorizontalMovement != 0f)
							{
								if (activeOwnedActorData.GetActorTurnSM().AmStillDeciding())
								{
									break;
								}
							}
							flag8 = false;
							break;
						}
						if (activeOwnedActorData != null)
						{
							ActorCover actorCover = activeOwnedActorData.GetActorCover();
							if (actorCover != null)
							{
								if (m_currentCursorType == CursorType.MouseOverCursorType)
								{
									actorCover.UpdateCoverHighlights(Board.Get().PlayerFreeSquare);
								}
								else
								{
									actorCover.UpdateCoverHighlights(null);
								}
							}
						}
						goto IL_0367;
					}
				}
				flag3 = false;
				flag8 = false;
				flag4 = false;
				flag9 = false;
				flag11 = false;
				flag12 = false;
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					ActorCover actorCover2 = GameFlowData.Get().activeOwnedActorData.GetActorCover();
					if (actorCover2 != null)
					{
						actorCover2.UpdateCoverHighlights(null);
					}
				}
				goto IL_0367;
				IL_0367:
				if (activeOwnedActorData != null)
				{
					if (activeOwnedActorData.GetAbilityData().GetSelectedAbility() != null)
					{
						if (activeOwnedActorData.GetAbilityData().GetSelectedAbility().Targeter is AbilityUtil_Targeter_Shape)
						{
							flag4 = false;
							flag5 = true;
							flag6 = false;
							flag7 = false;
							flag10 = false;
							flag11 = false;
						}
						else
						{
							flag4 = false;
							flag5 = false;
							flag6 = false;
							flag7 = false;
							flag10 = false;
							flag11 = false;
						}
					}
					else if (activeOwnedActorData.GetActorTurnSM().CurrentState == TurnStateEnum.PICKING_RESPAWN)
					{
						flag12 = true;
						flag4 = false;
						flag5 = false;
						flag6 = false;
						flag7 = false;
						flag10 = false;
					}
					else
					{
						if (!(activeOwnedActorData.RemainingHorizontalMovement > 0f))
						{
							if (activeOwnedActorData.HasQueuedChase())
							{
								flag4 = false;
								flag5 = false;
								flag6 = false;
								flag7 = true;
								flag10 = false;
								goto IL_0452;
							}
						}
						flag4 = false;
						flag5 = false;
						flag6 = true;
						flag7 = false;
					}
					goto IL_0452;
				}
				goto IL_047f;
				IL_047f:
				if (!flag9)
				{
					if (!flag8)
					{
						flag4 = false;
						flag5 = false;
						flag6 = false;
						flag7 = false;
						flag10 = false;
					}
				}
				UIManager.SetGameObjectActive(MovementMouseOverCursor, flag6);
				UIManager.SetGameObjectActive(AbilityTargetMouseOverCursor, flag5);
				UIManager.SetGameObjectActive(IdleMouseOverCursor, flag7);
				UIManager.SetGameObjectActive(m_targetingCursor, flag3);
				UIManager.SetGameObjectActive(FreeMouseOverCursor, flag4);
				UIManager.SetGameObjectActive(ClampedMouseOverCursor, flag8);
				UIManager.SetGameObjectActive(CornerMouseOverCursor, flag9);
				if (ChaseSquareCursor != null)
				{
					UIManager.SetGameObjectActive(ChaseSquareCursor, flag11);
				}
				if (ChaseSquareCursorAlt != null)
				{
					UIManager.SetGameObjectActive(ChaseSquareCursorAlt, flag11);
				}
				if (SprintMouseOverCursor != null)
				{
					int num3;
					if (!(UIScreenManager.Get() == null))
					{
						num3 = (UIScreenManager.Get().GetHideHUDCompletely() ? 1 : 0);
					}
					else
					{
						num3 = 1;
					}
					bool flag14 = (byte)num3 != 0;
					GameObject sprintMouseOverCursor = SprintMouseOverCursor;
					int doActive;
					if (flag10)
					{
						doActive = ((!flag14) ? 1 : 0);
					}
					else
					{
						doActive = 0;
					}
					UIManager.SetGameObjectActive(sprintMouseOverCursor, (byte)doActive != 0);
				}
				if (RespawnSelectionCursor != null)
				{
					UIManager.SetGameObjectActive(RespawnSelectionCursor, flag12);
				}
				if (!(ActorDebugUtils.Get() != null))
				{
					return;
				}
				while (true)
				{
					if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.CursorState))
					{
						ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = ActorDebugUtils.Get().GetDebugCategoryInfo(ActorDebugUtils.DebugCategory.CursorState);
						string text = debugCategoryInfo.m_stringToDisplay = string.Format(new StringBuilder().Append("CursorType: ").Append(m_currentCursorType.ToString()).Append("\n{0} \tMovementMouseOverCursor \n{1} \tAbilityTargetMouseOverCursor \n{2} \tIdleMouseOverCursor \n{3} \tm_targetingCursor \n{4} \tFreeMouseOverCursor \n{5} \tClampedMouseOverCursor \n{6} \tCornerMouseOverCursor\n{7} \tshowChaseCursor \n{8} \tshowSprintCursor \n{9} \tchaseMouseover \n{10} \trespawnCursor \n").ToString(), flag6, flag5, flag7, flag3, flag4, flag8, flag9, flag11, flag10, flag, flag12);
					}
					return;
				}
				IL_0452:
				if (flag2)
				{
					if (flag)
					{
						flag4 = false;
						flag5 = false;
						flag6 = false;
						flag7 = false;
						flag10 = false;
						flag11 = true;
					}
				}
				goto IL_047f;
			}
		}
	}

	public void UpdateCursorPositions()
	{
		Vector3 position = Vector3.zero;
		Vector3 position2 = Vector3.zero;
		Vector3 playerFreeCornerPos = Board.Get().PlayerFreeCornerPos;
		if (Board.Get().PlayerClampedSquare != null)
		{
			position = Board.Get().PlayerClampedSquare.ToVector3();
			if (Board.Get().PlayerClampedSquare.height < 0)
			{
				position.y = Board.Get().BaselineHeight;
			}
		}
		if (Board.Get().PlayerFreeSquare != null)
		{
			position2 = Board.Get().PlayerFreeSquare.ToVector3();
			if (Board.Get().PlayerFreeSquare.height < 0)
			{
				position2.y = Board.Get().BaselineHeight;
			}
		}
		if (RespawnSelectionCursor != null)
		{
			RespawnSelectionCursor.transform.position = position;
		}
		Vector3 vector = new Vector3(0f, 0.1f, 0f);
		position += vector;
		position2 += vector;
		playerFreeCornerPos += vector;
		ClampedMouseOverCursor.transform.position = position;
		FreeMouseOverCursor.transform.position = position2;
		CornerMouseOverCursor.transform.position = playerFreeCornerPos;
		ChaseSquareCursor.transform.position = position2;
		ChaseSquareCursorAlt.transform.position = position2;
		m_targetingCursor.transform.position = position;
		MovementMouseOverCursor.transform.position = position;
		AbilityTargetMouseOverCursor.transform.position = position;
		IdleMouseOverCursor.transform.position = position;
		object obj;
		if (HUD_UI.Get() != null)
		{
			obj = HUD_UI.Get().GetTopLevelCanvas();
		}
		else
		{
			obj = null;
		}
		Canvas canvas = (Canvas)obj;
		if (canvas != null)
		{
			if (m_sprintHighlightPrefab != null)
			{
				if (SprintMouseOverCursor == null)
				{
					SprintMouseOverCursor = UnityEngine.Object.Instantiate(m_sprintHighlightPrefab);
					UIManager.SetGameObjectActive(SprintMouseOverCursor, false);
					SprintMouseOverCursor.transform.SetParent(canvas.transform, false);
					SprintMouseOverCursor.GetComponent<Canvas>().sortingOrder += m_sprintHighlightPrefab.GetComponent<Canvas>().sortingOrder;
				}
			}
			if (SprintMouseOverCursor != null)
			{
				RectTransform rectTransform = canvas.transform as RectTransform;
				Vector2 vector2 = Camera.main.WorldToViewportPoint(position);
				float x = vector2.x;
				Vector2 sizeDelta = rectTransform.sizeDelta;
				float x2 = x * sizeDelta.x;
				float y = vector2.y;
				Vector2 sizeDelta2 = rectTransform.sizeDelta;
				Vector2 v = new Vector2(x2, y * sizeDelta2.y);
				(SprintMouseOverCursor.transform as RectTransform).anchoredPosition3D = v;
				SprintMouseOverCursor.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		RefreshCursorVisibility();
	}

	private void ProcessUpperLeftCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetSquareFromIndex(square.x, square.y + 1);
		bool flag = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x - 1, square.y);
		bool flag2 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x - 1, square.y + 1);
		bool flag3 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		if (!flag && !flag2)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					highlighMesh.AddPiece(PieceType.UpperLeftOuterCorner, square);
					return;
				}
			}
		}
		if (!flag)
		{
			if (flag2)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						highlighMesh.AddPiece(PieceType.UpperLeftHorizontal, square);
						return;
					}
				}
			}
		}
		if (flag)
		{
			if (!flag2)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						highlighMesh.AddPiece(PieceType.UpperLeftVertical, square);
						return;
					}
				}
			}
		}
		if (!flag || !flag2)
		{
			return;
		}
		while (true)
		{
			if (!flag3)
			{
				while (true)
				{
					highlighMesh.AddPiece(PieceType.UpperLeftInnerCorner, square);
					return;
				}
			}
			return;
		}
	}

	private void ProcessUpperRightCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetSquareFromIndex(square.x, square.y + 1);
		bool flag = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x + 1, square.y);
		bool flag2 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x + 1, square.y + 1);
		bool flag3 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		if (!flag)
		{
			if (!flag2)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						highlighMesh.AddPiece(PieceType.UpperRightOuterCorner, square);
						return;
					}
				}
			}
		}
		if (!flag)
		{
			if (flag2)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						highlighMesh.AddPiece(PieceType.UpperRightHorizontal, square);
						return;
					}
				}
			}
		}
		if (flag)
		{
			if (!flag2)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						highlighMesh.AddPiece(PieceType.UpperRightVertical, square);
						return;
					}
				}
			}
		}
		if (!flag || !flag2)
		{
			return;
		}
		while (true)
		{
			if (!flag3)
			{
				highlighMesh.AddPiece(PieceType.UpperRightInnerCorner, square);
			}
			return;
		}
	}

	private void ProcessLowerLeftCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetSquareFromIndex(square.x, square.y - 1);
		bool flag = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x - 1, square.y);
		bool flag2 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x - 1, square.y - 1);
		bool flag3 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		if (!flag)
		{
			if (!flag2)
			{
				highlighMesh.AddPiece(PieceType.LowerLeftOuterCorner, square);
				return;
			}
		}
		if (!flag && flag2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					highlighMesh.AddPiece(PieceType.LowerLeftHorizontal, square);
					return;
				}
			}
		}
		if (flag)
		{
			if (!flag2)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						highlighMesh.AddPiece(PieceType.LowerLeftVertical, square);
						return;
					}
				}
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			if (!flag2)
			{
				return;
			}
			while (true)
			{
				if (!flag3)
				{
					while (true)
					{
						highlighMesh.AddPiece(PieceType.LowerLeftInnerCorner, square);
						return;
					}
				}
				return;
			}
		}
	}

	private void ProcessLowerRightCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetSquareFromIndex(square.x, square.y - 1);
		bool flag = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x + 1, square.y);
		bool flag2 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetSquareFromIndex(square.x + 1, square.y - 1);
		bool flag3 = IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		if (!flag)
		{
			if (!flag2)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						highlighMesh.AddPiece(PieceType.LowerRightOuterCorner, square);
						return;
					}
				}
			}
		}
		if (!flag)
		{
			if (flag2)
			{
				highlighMesh.AddPiece(PieceType.LowerRightHorizontal, square);
				return;
			}
		}
		if (flag && !flag2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					highlighMesh.AddPiece(PieceType.LowerRightVertical, square);
					return;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			if (flag2 && !flag3)
			{
				while (true)
				{
					highlighMesh.AddPiece(PieceType.LowerRightInnerCorner, square);
					return;
				}
			}
			return;
		}
	}

	private bool IsSquareOnSameSideOfAnyBorder(BoardSquare square, BoardSquare testSquare, HashSet<BoardSquare> squaresSet)
	{
		int result;
		if (!squaresSet.Contains(testSquare))
		{
			result = ((!squaresSet.Contains(square)) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool IsSquareConnected(BoardSquare square, BoardSquare testSquare, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, bool includeInternalBorders)
	{
		if (includeInternalBorders)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return IsSquareOnSameSideOfAnyBorder(square, testSquare, squaresSet);
				}
			}
		}
		int result;
		if (!squaresSet.Contains(testSquare))
		{
			result = ((!borderSet.Contains(testSquare)) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool BoardSquareIsInSet(int x, int y, HashSet<BoardSquare> squaresSet)
	{
		BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
		return squaresSet.Contains(boardSquare);
	}

	private bool IsSquareSurrounded(BoardSquare square, HashSet<BoardSquare> squaresSet)
	{
		int result;
		if (BoardSquareIsInSet(square.x - 1, square.y - 1, squaresSet) && BoardSquareIsInSet(square.x, square.y - 1, squaresSet))
		{
			if (BoardSquareIsInSet(square.x + 1, square.y - 1, squaresSet))
			{
				if (BoardSquareIsInSet(square.x - 1, square.y, squaresSet))
				{
					if (BoardSquareIsInSet(square.x + 1, square.y, squaresSet))
					{
						if (BoardSquareIsInSet(square.x - 1, square.y + 1, squaresSet))
						{
							if (BoardSquareIsInSet(square.x, square.y + 1, squaresSet))
							{
								result = (BoardSquareIsInSet(square.x + 1, square.y + 1, squaresSet) ? 1 : 0);
								goto IL_0136;
							}
						}
					}
				}
			}
		}
		result = 0;
		goto IL_0136;
		IL_0136:
		return (byte)result != 0;
	}

	private HashSet<BoardSquare> CalculateOuterBorder(HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> additionalFloodFillSquares)
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = int.MaxValue;
		int num4 = int.MinValue;
		using (HashSet<BoardSquare>.Enumerator enumerator = squaresSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.x < num)
				{
					num = current.x;
				}
				if (current.x > num2)
				{
					num2 = current.x;
				}
				if (current.y < num3)
				{
					num3 = current.y;
				}
				if (current.y > num4)
				{
					num4 = current.y;
				}
			}
		}
		num--;
		num2++;
		num3--;
		num4++;
		Board board = Board.Get();
		for (int i = num; i <= num2; i++)
		{
			BoardSquare boardSquare = board.GetSquareFromIndex(i, num3);
			if (boardSquare != null)
			{
				if (!squaresSet.Contains(boardSquare))
				{
					hashSet.Add(boardSquare);
				}
			}
			BoardSquare boardSquare2 = board.GetSquareFromIndex(i, num4);
			if (!(boardSquare2 != null))
			{
				continue;
			}
			if (!squaresSet.Contains(boardSquare2))
			{
				hashSet.Add(boardSquare2);
			}
		}
		while (true)
		{
			for (int j = num3; j <= num4; j++)
			{
				BoardSquare boardSquare3 = board.GetSquareFromIndex(num, j);
				if (boardSquare3 != null)
				{
					if (!squaresSet.Contains(boardSquare3))
					{
						hashSet.Add(boardSquare3);
					}
				}
				BoardSquare boardSquare4 = board.GetSquareFromIndex(num2, j);
				if (!(boardSquare4 != null))
				{
					continue;
				}
				if (!squaresSet.Contains(boardSquare4))
				{
					hashSet.Add(boardSquare4);
				}
			}
			List<BoardSquare> list = new List<BoardSquare>();
			using (HashSet<BoardSquare>.Enumerator enumerator2 = hashSet.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BoardSquare current2 = enumerator2.Current;
					list.Add(current2);
				}
			}
			if (additionalFloodFillSquares != null)
			{
				foreach (BoardSquare additionalFloodFillSquare in additionalFloodFillSquares)
				{
					if (!list.Contains(additionalFloodFillSquare))
					{
						list.Add(additionalFloodFillSquare);
					}
				}
			}
			while (list.Count > 0)
			{
				BoardSquare boardSquare5 = list[0];
				list.RemoveAt(0);
				for (int k = -1; k < 2; k++)
				{
					for (int l = -1; l < 2; l++)
					{
						int num5 = boardSquare5.x + k;
						int num6 = boardSquare5.y + l;
						if (num5 > num2)
						{
							continue;
						}
						if (num5 < num)
						{
							continue;
						}
						if (num6 > num4)
						{
							continue;
						}
						if (num6 < num3)
						{
							continue;
						}
						BoardSquare boardSquare6 = board.GetSquareFromIndex(num5, num6);
						if (!(boardSquare6 != null))
						{
							continue;
						}
						if (squaresSet.Contains(boardSquare6))
						{
							continue;
						}
						if (!hashSet.Contains(boardSquare6))
						{
							list.Add(boardSquare6);
							hashSet.Add(boardSquare6);
						}
					}
				}
			}
			while (true)
			{
				return hashSet;
			}
		}
	}

	public GameObject CreateBoundaryHighlight(HashSet<BoardSquare> squaresSet, Color borderColor, bool dotted = false, HashSet<BoardSquare> additionalFloodFillSquares = null, bool includeInternalBorders = false)
	{
		Material material;
		if (dotted)
		{
			material = m_dottedHighlightMaterial;
		}
		else
		{
			material = m_highlightMaterial;
		}
		Material borderMaterial = material;
		GameObject gameObject = null;
		HighlightMesh highlighMesh = new HighlightMesh();
		if (squaresSet.Count > 0)
		{
			HashSet<BoardSquare> borderSet = null;
			if (!includeInternalBorders)
			{
				borderSet = CalculateOuterBorder(squaresSet, additionalFloodFillSquares);
			}
			m_surroundedSquareIndicators.ResetNextIndicatorIndex();
			foreach (BoardSquare item in squaresSet)
			{
				if (!IsSquareSurrounded(item, squaresSet))
				{
					ProcessUpperLeftCorner(item, squaresSet, borderSet, ref highlighMesh, includeInternalBorders);
					ProcessUpperRightCorner(item, squaresSet, borderSet, ref highlighMesh, includeInternalBorders);
					ProcessLowerLeftCorner(item, squaresSet, borderSet, ref highlighMesh, includeInternalBorders);
					ProcessLowerRightCorner(item, squaresSet, borderSet, ref highlighMesh, includeInternalBorders);
				}
				if (includeInternalBorders)
				{
					m_surroundedSquareIndicators.ShowIndicatorForSquare(item);
				}
			}
			m_surroundedSquareIndicators.HideAllSquareIndicators(m_surroundedSquareIndicators.GetNextIndicatorIndex());
			gameObject = highlighMesh.CreateMeshObject(borderMaterial);
			borderColor.a = 1f;
			gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", borderColor);
		}
		return gameObject;
	}

	public GameObject CreateBoundaryHighlight(List<BoardSquare> squares, Color borderColor, bool includeInternalBorders = false)
	{
		HashSet<BoardSquare> squaresSet = new HashSet<BoardSquare>(squares);
		return CreateBoundaryHighlight(squaresSet, borderColor, false, null, includeInternalBorders);
	}

	public static void AddPieceTypeToMask(ref int mask, PieceType pieceType)
	{
		mask |= 1 << (int)pieceType;
	}

	public static bool IsPieceTypeInMask(int mask, PieceType pieceType)
	{
		return (mask & (1 << (int)pieceType)) != 0;
	}

	private Vector2[] HorizontalFlip(Vector2[] uvs)
	{
		Vector2[] array = new Vector2[4];
		array[0] = uvs[3];
		array[3] = uvs[0];
		array[1] = uvs[2];
		array[2] = uvs[1];
		return array;
	}

	private Vector2[] VerticalFlip(Vector2[] uvs)
	{
		Vector2[] array = new Vector2[4];
		array[0] = uvs[2];
		array[2] = uvs[0];
		array[1] = uvs[3];
		array[3] = uvs[1];
		return array;
	}

	private void InitializeHighlightPieces()
	{
		for (int i = 0; i < m_highlightPieces.Length; i++)
		{
			m_highlightPieces[i] = new HighlightPiece();
		}
		while (true)
		{
			Vector3[] pos = new Vector3[4]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(0.75f, 0f, -0.75f),
				new Vector3(0f, 0f, -0.75f),
				new Vector3(0.75f, 0f, 0f)
			};
			m_highlightPieces[0].m_pos = pos;
			m_highlightPieces[1].m_pos = pos;
			m_highlightPieces[2].m_pos = pos;
			m_highlightPieces[3].m_pos = pos;
			Vector3[] pos2 = new Vector3[4]
			{
				new Vector3(0f, 0f, 0.75f),
				new Vector3(0.75f, 0f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(0.75f, 0f, 0.75f)
			};
			m_highlightPieces[4].m_pos = pos2;
			m_highlightPieces[5].m_pos = pos2;
			m_highlightPieces[6].m_pos = pos2;
			m_highlightPieces[7].m_pos = pos2;
			Vector3[] pos3 = new Vector3[4]
			{
				new Vector3(-0.75f, 0f, 0.75f),
				new Vector3(0f, 0f, 0f),
				new Vector3(-0.75f, 0f, 0f),
				new Vector3(0f, 0f, 0.75f)
			};
			m_highlightPieces[8].m_pos = pos3;
			m_highlightPieces[9].m_pos = pos3;
			m_highlightPieces[10].m_pos = pos3;
			m_highlightPieces[11].m_pos = pos3;
			Vector3[] pos4 = new Vector3[4]
			{
				new Vector3(-0.75f, 0f, 0f),
				new Vector3(0f, 0f, -0.75f),
				new Vector3(-0.75f, 0f, -0.75f),
				new Vector3(0f, 0f, 0f)
			};
			m_highlightPieces[12].m_pos = pos4;
			m_highlightPieces[13].m_pos = pos4;
			m_highlightPieces[14].m_pos = pos4;
			m_highlightPieces[15].m_pos = pos4;
			Vector2[] array = new Vector2[4]
			{
				new Vector2(0.333333343f, 0f),
				new Vector2(2f / 3f, 1f),
				new Vector2(0.333333343f, 1f),
				new Vector2(2f / 3f, 0f)
			};
			m_highlightPieces[1].m_uv = array;
			m_highlightPieces[5].m_uv = VerticalFlip(array);
			m_highlightPieces[9].m_uv = VerticalFlip(array);
			m_highlightPieces[13].m_uv = array;
			Vector2[] array2 = new Vector2[4]
			{
				new Vector2(0.333333343f, 0f),
				new Vector2(2f / 3f, 1f),
				new Vector2(2f / 3f, 0f),
				new Vector2(0.333333343f, 1f)
			};
			m_highlightPieces[0].m_uv = array2;
			m_highlightPieces[4].m_uv = array2;
			m_highlightPieces[8].m_uv = HorizontalFlip(array2);
			m_highlightPieces[12].m_uv = HorizontalFlip(array2);
			Vector2[] array3 = new Vector2[4]
			{
				new Vector2(1f, 0f),
				new Vector2(2f / 3f, 1f),
				new Vector2(1f, 1f),
				new Vector2(2f / 3f, 0f)
			};
			m_highlightPieces[2].m_uv = array3;
			m_highlightPieces[6].m_uv = VerticalFlip(array3);
			m_highlightPieces[10].m_uv = HorizontalFlip(VerticalFlip(array3));
			m_highlightPieces[14].m_uv = HorizontalFlip(array3);
			Vector2[] array4 = new Vector2[4]
			{
				new Vector2(0.333333343f, 0f),
				new Vector2(0f, 1f),
				new Vector2(0.333333343f, 1f),
				new Vector2(0f, 0f)
			};
			m_highlightPieces[3].m_uv = array4;
			m_highlightPieces[7].m_uv = VerticalFlip(array4);
			m_highlightPieces[11].m_uv = HorizontalFlip(VerticalFlip(array4));
			m_highlightPieces[15].m_uv = HorizontalFlip(array4);
			return;
		}
	}

	public static float GetHighlightHeight()
	{
		return (float)Board.Get().BaselineHeight + 0.1f;
	}

	public void SetScrollCursor(ScrollCursorDirection direction)
	{
		if (Application.isEditor && CameraControls.Get() != null)
		{
			if (!CameraControls.Get().m_mouseMoveFringeInEditor)
			{
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		if (!m_scrollCursorEdge)
		{
			return;
		}
		while (true)
		{
			if (direction == ScrollCursorDirection.Undefined)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
						m_isCursorSet = false;
						return;
					}
				}
			}
			if (!ScrollCursors.ContainsKey(direction))
			{
				return;
			}
			while (true)
			{
				Texture2D texture2D = ScrollCursors[direction];
				if (!(texture2D != null))
				{
					return;
				}
				while (true)
				{
					Vector2 vector;
					if (ScrollCursorsHotspot.ContainsKey(direction))
					{
						vector = ScrollCursorsHotspot[direction];
					}
					else
					{
						vector = new Vector2(texture2D.width / 2, texture2D.height / 2);
					}
					Vector2 hotspot = vector;
					Cursor.SetCursor(texture2D, hotspot, CursorMode.Auto);
					m_isCursorSet = true;
					return;
				}
			}
		}
	}

	public void ResetCursor()
	{
		if (!m_isCursorSet)
		{
			return;
		}
		while (true)
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			m_isCursorSet = false;
			return;
		}
	}

	private static GameObject CreateSurroundedSquareObject()
	{
		if (s_instance != null)
		{
			if (s_instance.m_surroundedSquaresParent == null)
			{
				s_instance.m_surroundedSquaresParent = new GameObject("SurroundedSquaresParent");
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Get().m_highlightFloodFillSquare);
		gameObject.transform.localScale *= Board.Get().squareSize / 1.5f;
		if (s_instance != null)
		{
			gameObject.transform.parent = s_instance.m_surroundedSquaresParent.transform;
		}
		return gameObject;
	}

	private static GameObject CreateHiddenSquareIndicatorObject()
	{
		if (s_instance == null)
		{
			return null;
		}
		if (s_instance.m_hiddenSquareIndicatorParent == null)
		{
			s_instance.m_hiddenSquareIndicatorParent = new GameObject("HiddenSquareIndicatorParent");
		}
		if (s_instance.m_hiddenSquarePrefab == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(s_instance.m_hiddenSquarePrefab);
		gameObject.transform.localScale *= Board.Get().squareSize / 1.5f;
		gameObject.transform.parent = s_instance.m_hiddenSquareIndicatorParent.transform;
		return gameObject;
	}

	public static SquareIndicators GetHiddenSquaresContainer()
	{
		if (s_instance != null)
		{
			return s_instance.m_hiddenSquareIndicators;
		}
		return null;
	}

	private static GameObject CreateAffectedSquareIndicatorObject()
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		if (s_instance.m_affectedSquareIndicatorParent == null)
		{
			s_instance.m_affectedSquareIndicatorParent = new GameObject("AffectedSquareIndicatorParent");
		}
		if (s_instance.m_affectedSquarePrefab == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(s_instance.m_affectedSquarePrefab);
		gameObject.transform.localScale *= Board.Get().squareSize / 1.5f;
		gameObject.transform.parent = s_instance.m_affectedSquareIndicatorParent.transform;
		return gameObject;
	}

	public static SquareIndicators GetAffectedSquaresContainer()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return s_instance.m_affectedSquareIndicators;
				}
			}
		}
		return null;
	}

	private static bool ShouldShowAffectedSquares()
	{
		int result;
		if (s_instance != null)
		{
			if (s_instance.m_showAffectedSquaresWhileTargeting)
			{
				if (s_instance.m_affectedSquarePrefab != null)
				{
					result = (InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo) ? 1 : 0);
					goto IL_006a;
				}
			}
		}
		result = 0;
		goto IL_006a;
		IL_006a:
		return (byte)result != 0;
	}

	private void CreateRangeIndicatorHighlight()
	{
		m_rangeIndicatorMouseOverFlags.Clear();
		for (int i = 0; i < 14; i++)
		{
			m_rangeIndicatorMouseOverFlags.Add(false);
		}
		while (true)
		{
			if (!(m_abilityRangeIndicatorHighlight == null))
			{
				return;
			}
			while (true)
			{
				m_abilityRangeIndicatorHighlight = Get().CreateDynamicConeMesh(1f, 360f, true);
				m_abilityRangeIndicatorHighlight.name = "AbilityRangeIndicatorObject";
				m_abilityRangeIndicatorHighlight.transform.parent = base.transform;
				object obj;
				if ((bool)m_abilityRangeIndicatorHighlight)
				{
					obj = m_abilityRangeIndicatorHighlight.GetComponent<UIDynamicCone>();
				}
				else
				{
					obj = null;
				}
				UIDynamicCone uIDynamicCone = (UIDynamicCone)obj;
				uIDynamicCone.m_borderThickness = m_abilityRangeIndicatorWidth;
				uIDynamicCone.SetConeObjectActive(false);
				MeshRenderer[] componentsInChildren = m_abilityRangeIndicatorHighlight.GetComponentsInChildren<MeshRenderer>();
				MeshRenderer[] array = componentsInChildren;
				foreach (MeshRenderer meshRenderer in array)
				{
					if (Get() != null)
					{
						AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, m_abilityRangeIndicatorColor);
					}
					AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, m_abilityRangeIndicatorColor.a);
				}
				UIManager.SetGameObjectActive(m_abilityRangeIndicatorHighlight, false);
				return;
			}
		}
	}

	private void DestroyRangeIndicatorHighlight()
	{
		if (m_abilityRangeIndicatorHighlight != null)
		{
			DestroyObjectAndMaterials(m_abilityRangeIndicatorHighlight);
			m_abilityRangeIndicatorHighlight = null;
		}
	}

	private void AdjustRangeIndicatorHighlight(float radiusInSquares)
	{
		if (!(m_abilityRangeIndicatorHighlight != null))
		{
			return;
		}
		while (true)
		{
			m_lastRangeIndicatorRadius = radiusInSquares;
			Get().AdjustDynamicConeMesh(m_abilityRangeIndicatorHighlight, radiusInSquares, 360f);
			return;
		}
	}

	public void SetRangeIndicatorMouseOverFlag(int actionTypeInt, bool isMousingOver)
	{
		if (actionTypeInt < 0 || actionTypeInt >= m_rangeIndicatorMouseOverFlags.Count)
		{
			return;
		}
		while (true)
		{
			m_rangeIndicatorMouseOverFlags[actionTypeInt] = isMousingOver;
			return;
		}
	}

	public void UpdateRangeIndicatorHighlight()
	{
		object obj;
		if (GameFlowData.Get() != null)
		{
			obj = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			obj = null;
		}
		ActorData actorData = (ActorData)obj;
		if (!m_showAbilityRangeIndicator || !(m_abilityRangeIndicatorHighlight != null))
		{
			return;
		}
		while (true)
		{
			if (!(actorData != null) || !(actorData.GetAbilityData() != null))
			{
				return;
			}
			while (true)
			{
				if (!(actorData.GetActorTurnSM() != null))
				{
					return;
				}
				while (true)
				{
					AbilityData abilityData = actorData.GetAbilityData();
					bool flag = false;
					if (!actorData.IsDead())
					{
						if (actorData.GetActorTurnSM().CurrentState == TurnStateEnum.TARGETING_ACTION)
						{
							if (abilityData.GetSelectedAbility() != null)
							{
								if (abilityData.GetSelectedAbility().ShowTargetableRadiusWhileTargeting())
								{
									int selectedActionTypeForTargeting = (int)abilityData.GetSelectedActionTypeForTargeting();
									float targetableRadius = abilityData.GetTargetableRadius(selectedActionTypeForTargeting, actorData);
									if (targetableRadius > 0f)
									{
										AdjustAndShowRangeIndicator(actorData.GetFreePos(), targetableRadius);
										flag = true;
									}
								}
							}
							goto IL_01d7;
						}
					}
					if (!actorData.IsDead())
					{
						if (actorData.GetActorTurnSM().CurrentState == TurnStateEnum.DECIDING)
						{
							List<bool> rangeIndicatorMouseOverFlags = m_rangeIndicatorMouseOverFlags;
							for (int i = 0; i < rangeIndicatorMouseOverFlags.Count; i++)
							{
								if (!rangeIndicatorMouseOverFlags[i])
								{
									continue;
								}
								float targetableRadius2 = abilityData.GetTargetableRadius(i, actorData);
								if (targetableRadius2 > 0f)
								{
									AdjustAndShowRangeIndicator(actorData.GetFreePos(), targetableRadius2);
									flag = true;
								}
								break;
							}
						}
					}
					goto IL_01d7;
					IL_01d7:
					if (!flag && m_abilityRangeIndicatorHighlight.activeSelf)
					{
						while (true)
						{
							UIManager.SetGameObjectActive(m_abilityRangeIndicatorHighlight, false);
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void AdjustAndShowRangeIndicator(Vector3 centerPos, float radiusInSquares)
	{
		if (m_lastRangeIndicatorRadius != radiusInSquares)
		{
			AdjustRangeIndicatorHighlight(radiusInSquares);
		}
		centerPos.y = GetHighlightHeight();
		m_abilityRangeIndicatorHighlight.transform.position = centerPos;
		if (m_abilityRangeIndicatorHighlight.activeSelf)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_abilityRangeIndicatorHighlight, true);
			return;
		}
	}

	public void UpdateMouseoverCoverHighlight()
	{
		object obj;
		if (GameFlowData.Get() != null)
		{
			obj = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			obj = null;
		}
		ActorData actorData = (ActorData)obj;
		if (!(actorData != null))
		{
			return;
		}
		while (true)
		{
			if (!(actorData.GetActorCover() != null))
			{
				return;
			}
			while (true)
			{
				ActorCover actorCover = actorData.GetActorCover();
				if (!(actorCover != null))
				{
					return;
				}
				while (true)
				{
					if (m_currentCursorType == CursorType.MouseOverCursorType)
					{
						m_mouseoverCoverManager.UpdateCoverAroundSquare(Board.Get().PlayerFreeSquare);
					}
					else
					{
						m_mouseoverCoverManager.UpdateCoverAroundSquare(null);
					}
					return;
				}
			}
		}
	}

	public void UpdateShowAffectedSquareFlag()
	{
		m_cachedShouldShowAffectedSquares = ShouldShowAffectedSquares();
	}
}
