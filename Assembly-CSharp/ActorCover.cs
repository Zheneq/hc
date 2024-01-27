using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorCover : NetworkBehaviour
{
	private bool[] m_hasCover = new bool[4];
	private bool[] m_cachedHasCoverFromBarriers = new bool[4];
	private SyncListTempCoverInfo m_syncTempCoverProviders = new SyncListTempCoverInfo();
	private List<CoverDirections> m_tempCoverProviders = new List<CoverDirections>();
	private List<CoverDirections> m_tempCoverIgnoreMinDist = new List<CoverDirections>();
	private GameObject m_coverParent;
	private ActorData m_owner;
	private GameObject[] m_mouseOverCoverObjs = new GameObject[4];
	private GameObject[] m_actorCoverObjs = new GameObject[4];
	private List<ParticleSystemRenderer[]> m_actorCoverSymbolRenderers = new List<ParticleSystemRenderer[]>();
	private static Vector3[] m_coverDir = new Vector3[4];
	private float m_coverHeight = 2f;
	private float m_coverDirIndicatorHideTime = -1f;
	private float m_coverDirIndicatorFadeStartTime = -1f;
	private float m_coverDirIndicatorSpawnTime = -1f;
	private GameObject m_coverDirHighlight;
	private MeshRenderer[] m_coverDirIndicatorRenderers;
	private EasedFloatCubic m_coverDirIndicatorOpacity = new EasedFloatCubic(1f);
	private static int kListm_syncTempCoverProviders = 0x55B6FA50;

	static ActorCover()
	{
		RegisterSyncListDelegate(typeof(ActorCover), kListm_syncTempCoverProviders, InvokeSyncListm_syncTempCoverProviders);
		NetworkCRC.RegisterBehaviour("ActorCover", 0);
	}

	public bool HasAnyCover(bool recalculate = false)
	{
		if (recalculate)
		{
			RecalculateCover();
		}
		bool result = false;
		foreach (bool hasCover in m_hasCover)
		{
			if (hasCover)
			{
				result = true;
			}
		}
		return result;
	}

	private void Awake()
	{
		m_coverParent = GameObject.Find("CoverParent");
		if (!m_coverParent)
		{
			m_coverParent = new GameObject("CoverParent");
		}
		for (int i = 0; i < 4; i++)
		{
			m_hasCover[i] = false;
			m_cachedHasCoverFromBarriers[i] = false;
		}
		InitCoverObjs(m_mouseOverCoverObjs, HighlightUtils.Get().m_coverIndicatorPrefab);
		InitCoverObjs(m_actorCoverObjs, HighlightUtils.Get().m_coverShieldOnlyPrefab);
		foreach (GameObject coverObj in m_actorCoverObjs)
		{
			ParticleSystemRenderer[] item = coverObj != null
				? coverObj.GetComponentsInChildren<ParticleSystemRenderer>()
				: new ParticleSystemRenderer[0];
			m_actorCoverSymbolRenderers.Add(item);
		}
		m_coverDir[(int)CoverDirections.X_NEG] = Vector3.left; 
		m_coverDir[(int)CoverDirections.X_POS] = Vector3.right; 
		m_coverDir[(int)CoverDirections.Y_NEG] = Vector3.back;
		m_coverDir[(int)CoverDirections.Y_POS] = Vector3.forward;
		m_owner = GetComponent<ActorData>();
		m_syncTempCoverProviders.InitializeBehaviour(this, kListm_syncTempCoverProviders);
	}

	public override void OnStartClient()
	{
		m_syncTempCoverProviders.Callback = SyncListCallbackTempCoverProviders;
	}

	private void SyncListCallbackTempCoverProviders(SyncList<TempCoverInfo>.Operation op, int index)
	{
		ResetTempCoverListFromSyncList();
	}

	private void InitCoverObjs(GameObject[] coverObjs, GameObject coverPrefab)
	{
		coverObjs[0] = CreateCoverIndicatorObject(-90f, coverPrefab);
		coverObjs[1] = CreateCoverIndicatorObject(90f, coverPrefab);
		coverObjs[2] = CreateCoverIndicatorObject(180f, coverPrefab);
		coverObjs[3] = CreateCoverIndicatorObject(0f, coverPrefab);
		coverObjs[0].SetActive(false);
		coverObjs[1].SetActive(false);
		coverObjs[2].SetActive(false);
		coverObjs[3].SetActive(false);
		coverObjs[0].transform.parent = m_coverParent.transform;
		coverObjs[1].transform.parent = m_coverParent.transform;
		coverObjs[2].transform.parent = m_coverParent.transform;
		coverObjs[3].transform.parent = m_coverParent.transform;
	}

	public static void ResetParticleTime(GameObject particleObject)
	{
		ParticleSystem[] componentsInChildren = particleObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			particleSystem.Clear();
			particleSystem.time = 0f;
		}
	}

	private GameObject CreateCoverIndicatorObject(float yRotation, GameObject coverPrefab)
	{
		GameObject coverObj = Instantiate(coverPrefab);
		coverObj.transform.Rotate(Vector3.up, yRotation);
		return coverObj;
	}

	private void SetCoverMeshColor(GameObject particleObject, Color color)
	{
		if (particleObject == null)
		{
			return;
		}
		ParticleSystemRenderer[] particleSystemRenderers = particleObject.GetComponentsInChildren<ParticleSystemRenderer>();
		foreach (ParticleSystemRenderer particleSystemRenderer in particleSystemRenderers)
		{
			AbilityUtil_Targeter.SetMaterialColor(particleSystemRenderer.materials, color);
		}
	}

	public void DisableCover()
	{
		for (int i = 0; i < 4; i++)
		{
			m_hasCover[i] = false;
			m_cachedHasCoverFromBarriers[i] = false;
		}
	}

	public Vector3 GetCoverOffset(CoverDirections dir)
	{
		return GetCoverOffsetStatic(dir);
	}

	public static Vector3 GetCoverOffsetStatic(CoverDirections dir)
	{
		float length = Board.Get().squareSize * 0.5f;
		Vector3 offset = Vector3.zero;
		if (dir == CoverDirections.X_POS)
		{
			offset = new Vector3(length, 0f, 0f);
		}
		else if (dir == CoverDirections.X_NEG)
		{
			offset = new Vector3(-length, 0f, 0f);
		}
		else if (dir == CoverDirections.Y_POS)
		{
			offset = new Vector3(0f, 0f, length);
		}
		else if (dir == CoverDirections.Y_NEG)
		{
			offset = new Vector3(0f, 0f, -length);
		}
		return offset;
	}

	public static Quaternion GetCoverRotation(CoverDirections dir)
	{
		switch (dir)
		{
			case CoverDirections.X_POS:
				return Quaternion.LookRotation(Vector3.left);
			case CoverDirections.X_NEG:
				return Quaternion.LookRotation(Vector3.right);
			case CoverDirections.Y_POS:
				return Quaternion.LookRotation(Vector3.back);
			default:
				return Quaternion.LookRotation(Vector3.forward);
		}
	}

	public bool HasNonThinCover(BoardSquare currentSquare, int xDelta, int yDelta, bool halfHeight)
	{
		bool result = false;
		BoardSquare boardSquare = Board.Get().GetSquareFromIndex(currentSquare.x + xDelta, currentSquare.y + yDelta);
		if (boardSquare != null)
		{
			int heightDiff = boardSquare.height - currentSquare.height;
			result = halfHeight
				? heightDiff == 1
				: heightDiff == 2;
		}
		return result;
	}

	public float CoverRating(BoardSquare square)
	{
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(m_owner.GetEnemyTeam());
		float num = 0f;
		foreach (ActorData actorData in allTeamMembers)
		{
			if (!actorData.IsDead())
			{
				if (actorData.GetCurrentBoardSquare() != null)
				{
					Vector3 vector = actorData.GetCurrentBoardSquare().transform.position - square.transform.position;
					if (vector.magnitude > Board.Get().squareSize * 1.5f)
					{
						if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
						{
							if (vector.x >= 0f)
							{
								goto IL_11C;
							}
							if (HasNonThinCover(square, -1, 0, true))
							{
								goto IL_162;
							}
							if (square.GetThinCover(CoverDirections.X_NEG) == ThinCover.CoverType.Half)
							{
								goto IL_162;
							}
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								goto IL_11C;
							}
							continue;
							IL_11C:
							if (vector.x > 0f)
							{
								if (HasNonThinCover(square, 1, 0, true))
								{
									goto IL_162;
								}
								if (square.GetThinCover(CoverDirections.X_POS) == ThinCover.CoverType.Half)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										goto IL_162;
									}
								}
							}
							if (vector.x >= 0f)
							{
								goto IL_1B5;
							}
							if (!HasNonThinCover(square, -1, 0, false))
							{
								if (square.GetThinCover(CoverDirections.X_NEG) != ThinCover.CoverType.Full)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										goto IL_1B5;
									}
								}
							}
							IL_1FB:
							num += 0.5f;
							continue;
							IL_1B5:
							if (vector.x <= 0f)
							{
								continue;
							}
							if (HasNonThinCover(square, 1, 0, false))
							{
								goto IL_1FB;
							}
							if (square.GetThinCover(CoverDirections.X_POS) != ThinCover.CoverType.Full)
							{
								continue;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								goto IL_1FB;
							}
							IL_162:
							num += 1f;
						}
						else
						{
							if (vector.z >= 0f)
							{
								goto IL_244;
							}
							if (!HasNonThinCover(square, 0, -1, true) && square.GetThinCover(CoverDirections.Y_NEG) != ThinCover.CoverType.Half)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									goto IL_244;
								}
							}
							IL_280:
							num += 1f;
							continue;
							IL_244:
							if (vector.z > 0f)
							{
								if (HasNonThinCover(square, 0, 1, true))
								{
									goto IL_280;
								}
								if (square.GetThinCover(CoverDirections.Y_POS) == ThinCover.CoverType.Half)
								{
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										goto IL_280;
									}
								}
							}
							if (vector.z < 0f)
							{
								if (HasNonThinCover(square, 0, -1, false))
								{
									goto IL_303;
								}
								if (square.GetThinCover(CoverDirections.Y_NEG) == ThinCover.CoverType.Full)
								{
									goto IL_303;
								}
							}
							if (vector.z <= 0f)
							{
								continue;
							}
							if (!HasNonThinCover(square, 0, 1, false))
							{
								if (square.GetThinCover(CoverDirections.Y_POS) != ThinCover.CoverType.Full)
								{
									continue;
								}
							}
							IL_303:
							num += 0.5f;
						}
					}
				}
			}
		}
		return num;
	}

	internal void UpdateCoverHighlights(BoardSquare currentSquare)
	{
		ActorData owner = m_owner;
		if (currentSquare != null && currentSquare.IsValidForGameplay())
		{
			ActorTurnSM actorTurnSM = owner.GetActorTurnSM();
			if (actorTurnSM != null)
			{
				bool flag = actorTurnSM.AmTargetingAction();
				List<BoardSquare> list = null;
				Board.Get().GetCardinalAdjacentSquares(currentSquare.x, currentSquare.y, ref list);
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						BoardSquare boardSquare = list[i];
						if (boardSquare == null)
						{
						}
						else
						{
							CoverDirections coverDirection = GetCoverDirection(currentSquare, boardSquare);
							int num = boardSquare.height - currentSquare.height;
							GameObject gameObject;
							if (coverDirection < (CoverDirections)m_mouseOverCoverObjs.Length)
							{
								gameObject = m_mouseOverCoverObjs[(int)coverDirection];
							}
							else
							{
								gameObject = null;
							}
							GameObject gameObject2 = gameObject;
							if (gameObject2 != null)
							{
								if (num < 1)
								{
									if (currentSquare.GetThinCover(coverDirection) == ThinCover.CoverType.None)
									{
										goto IL_1E5;
									}
								}
								if (!flag)
								{
									if (actorTurnSM.CurrentState != TurnStateEnum.PICKING_RESPAWN)
									{
										Vector3 vector = new Vector3(currentSquare.worldX, currentSquare.height + m_coverHeight, currentSquare.worldY);
										vector += GetCoverOffset(coverDirection);
										if (gameObject2.transform.position != vector)
										{
											goto IL_1C6;
										}
										if (!gameObject2.activeSelf)
										{
											for (;;)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												goto IL_1C6;
											}
										}
										IL_1E3:
										goto IL_1ED;
										IL_1C6:
										gameObject2.transform.position = vector;
										gameObject2.SetActive(true);
										ResetParticleTime(gameObject2);
										goto IL_1E3;
									}
								}
								IL_1E5:
								gameObject2.SetActive(false);
							}
						}
						IL_1ED:;
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < m_mouseOverCoverObjs.Length; j++)
			{
				GameObject gameObject3 = m_mouseOverCoverObjs[j];
				if (gameObject3)
				{
					gameObject3.SetActive(false);
				}
			}
		}
	}

	private void Update()
	{
		if (m_coverDirHighlight != null && m_coverDirIndicatorRenderers != null)
		{
			float opacity = m_coverDirIndicatorOpacity * GetCoverDirInitialOpacity();
			for (int i = 0; i < m_coverDirIndicatorRenderers.Length; i++)
			{
				MeshRenderer meshRenderer = m_coverDirIndicatorRenderers[i];
				if (meshRenderer != null)
				{
					AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, opacity);
				}
			}
			float opacity2 = m_coverDirIndicatorOpacity * GetCoverDirParticleInitialOpacity();
			for (int j = 0; j < m_hasCover.Length; j++)
			{
				if (j >= m_actorCoverSymbolRenderers.Count)
				{
					break;
				}
				if (m_hasCover[j])
				{
					foreach (ParticleSystemRenderer particleSystemRenderer in m_actorCoverSymbolRenderers[j])
					{
						AbilityUtil_Targeter.SetMaterialOpacity(particleSystemRenderer.materials, opacity2);
					}
				}
			}
		}
		if (m_coverDirIndicatorSpawnTime > 0f && Time.time > m_coverDirIndicatorSpawnTime)
		{
			ShowAllRelevantCoverIndicator();
			m_coverDirIndicatorSpawnTime = -1f;
		}
		if (m_coverDirIndicatorFadeStartTime > 0f)
		{
			if (Time.time > m_coverDirIndicatorFadeStartTime)
			{
				m_coverDirIndicatorOpacity.EaseTo(0f, GetCoverDirIndicatorDuration() - GetCoverDirFadeoutStartDelay());
				m_coverDirIndicatorFadeStartTime = -1f;
			}
		}
		if (m_coverDirIndicatorHideTime > 0f)
		{
			if (Time.time > m_coverDirIndicatorHideTime)
			{
				HideRelevantCover();
				DestroyCoverDirHighlight();
				m_coverDirIndicatorHideTime = -1f;
			}
		}
	}

	public void ShowRelevantCover(Vector3 damageOrigin)
	{
		List<CoverDirections> list = new List<CoverDirections>();
		if (IsInCoverWrt(damageOrigin, ref list))
		{
			BoardSquare currentBoardSquare = m_owner.GetCurrentBoardSquare();
			for (int i = 0; i < 4; i++)
			{
				CoverDirections coverDirections = (CoverDirections)i;
				if (list.Contains(coverDirections))
				{
					Vector3 a = new Vector3(currentBoardSquare.worldX, currentBoardSquare.height + m_coverHeight, currentBoardSquare.worldY);
					m_actorCoverObjs[i].transform.position = a + GetCoverOffset(coverDirections);
					m_actorCoverObjs[i].SetActive(true);
				}
				else
				{
					m_actorCoverObjs[i].SetActive(false);
				}
			}
		}
		else
		{
			HideRelevantCover();
		}
	}

	public void StartShowMoveIntoCoverIndicator()
	{
		if (HasAnyCover())
		{
			m_coverDirIndicatorSpawnTime = Time.time + GetCoverDirIndicatorSpawnDelay();
		}
		else
		{
			m_coverDirIndicatorSpawnTime = -1f;
		}
	}

	public static GameObject CreateCoverDirIndicator(bool[] hasCoverFlags, Color color, float radiusInSquares)
	{
		float num;
		if (GameplayData.Get())
		{
			num = GameplayData.Get().m_coverProtectionAngle;
		}
		else
		{
			num = 110f;
		}
		float num2 = num;
		float num3 = num2 - 90f;
		GameObject gameObject = new GameObject("CoverDirHighlightParent");
		int num4 = 0;
		bool flag = false;
		for (int i = 0; i < hasCoverFlags.Length; i++)
		{
			if (hasCoverFlags[i])
			{
				num4++;
			}
		}
		if (num4 == 2)
		{
			flag = (hasCoverFlags[1] == hasCoverFlags[0]);
		}
		float borderStartOffset = 0.7f;
		GameObject gameObject2 = HighlightUtils.Get().CreateDynamicConeMesh(radiusInSquares, num2, false);
		HighlightUtils.Get().SetDynamicConeMeshBorderActive(gameObject2, false);
		UIDynamicCone component = gameObject2.GetComponent<UIDynamicCone>();
		if (component != null)
		{
			component.SetBorderStartOffset(borderStartOffset);
		}
		Vector3 forward = Vector3.forward;
		if (num4 <= 3)
		{
			if (!flag)
			{
				Vector3 a = Vector3.zero;
				for (int j = 0; j < m_coverDir.Length; j++)
				{
					if (hasCoverFlags[j])
					{
						a += m_coverDir[j];
					}
				}
				forward = (a / num4).normalized;
			}
		}
		if (num4 == 2)
		{
			if (flag)
			{
				GameObject gameObject3 = HighlightUtils.Get().CreateDynamicConeMesh(radiusInSquares, num2, false);
				HighlightUtils.Get().SetDynamicConeMeshBorderActive(gameObject3, false);
				UIDynamicCone component2 = gameObject3.GetComponent<UIDynamicCone>();
				if (component2 != null)
				{
					component2.SetBorderStartOffset(borderStartOffset);
				}
				MeshRenderer[] componentsInChildren = gameObject3.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer meshRenderer in componentsInChildren)
				{
					if (HighlightUtils.Get() != null)
					{
						AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, color);
					}
					AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, GetCoverDirInitialOpacity());
				}
				if (hasCoverFlags[1])
				{
					forward = Vector3.right;
					gameObject3.transform.localRotation = Quaternion.LookRotation(Vector3.left);
				}
				else
				{
					forward = Vector3.forward;
					gameObject3.transform.localRotation = Quaternion.LookRotation(Vector3.back);
				}
				gameObject3.transform.parent = gameObject.transform;
				gameObject3.transform.localPosition = Vector3.zero;
				goto IL_320;
			}
		}
		if (num4 == 2)
		{
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject2, radiusInSquares, 180f + num3);
		}
		else if (num4 == 3)
		{
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject2, radiusInSquares, 270f + num3);
		}
		else if (num4 == 4)
		{
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject2, radiusInSquares, 360f);
		}
		IL_320:
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localRotation = Quaternion.LookRotation(forward);
		gameObject2.transform.localPosition = Vector3.zero;
		MeshRenderer[] componentsInChildren2 = gameObject2.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer2 in componentsInChildren2)
		{
			if (HighlightUtils.Get() != null)
			{
				AbilityUtil_Targeter.SetMaterialColor(meshRenderer2.materials, color);
			}
			AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer2.materials, GetCoverDirInitialOpacity());
		}
		return gameObject;
	}

	private void ShowCoverIndicatorForDirection(CoverDirections dir)
	{
		BoardSquare boardSquare = (!(m_owner != null)) ? null : m_owner.GetCurrentBoardSquare();
		if (boardSquare != null)
		{
			if (dir < (CoverDirections)m_hasCover.Length)
			{
				if (dir < (CoverDirections)m_actorCoverObjs.Length)
				{
					if (m_actorCoverObjs[(int)dir] != null)
					{
						if (m_hasCover[(int)dir])
						{
							Vector3 a = new Vector3(boardSquare.worldX, boardSquare.height + m_coverHeight, boardSquare.worldY);
							m_actorCoverObjs[(int)dir].transform.position = a + GetCoverOffset(dir);
							m_actorCoverObjs[(int)dir].SetActive(true);
							ResetParticleTime(m_actorCoverObjs[(int)dir]);
						}
						else
						{
							m_actorCoverObjs[(int)dir].SetActive(false);
						}
					}
				}
			}
		}
	}

	private void ShowAllRelevantCoverIndicator()
	{
		if (!HasAnyCover())
		{
			return;
		}
		ShowCoverIndicatorForDirection(CoverDirections.X_NEG);
		ShowCoverIndicatorForDirection(CoverDirections.X_POS);
		ShowCoverIndicatorForDirection(CoverDirections.Y_NEG);
		ShowCoverIndicatorForDirection(CoverDirections.Y_POS);
		DestroyCoverDirHighlight();
		BoardSquare boardSquare;
		if (m_owner != null)
		{
			boardSquare = m_owner.GetCurrentBoardSquare();
		}
		else
		{
			boardSquare = null;
		}
		BoardSquare boardSquare2 = boardSquare;
		if (boardSquare2 != null)
		{
			Vector3 position = boardSquare2.ToVector3();
			position.y = HighlightUtils.GetHighlightHeight();
			m_coverDirHighlight = CreateCoverDirIndicator(m_hasCover, HighlightUtils.Get().m_coverDirIndicatorColor, GetCoverDirIndicatorRadius());
			m_coverDirHighlight.transform.position = position;
			m_coverDirIndicatorRenderers = m_coverDirHighlight.GetComponentsInChildren<MeshRenderer>();
		}
		m_coverDirIndicatorOpacity = new EasedFloatCubic(1f);
		m_coverDirIndicatorOpacity.EaseTo(1f, 0.1f);
		m_coverDirIndicatorHideTime = Time.time + GetCoverDirIndicatorDuration();
		m_coverDirIndicatorFadeStartTime = Time.time + GetCoverDirFadeoutStartDelay();
	}

	public void HideRelevantCover()
	{
		for (int i = 0; i < 4; i++)
		{
			m_actorCoverObjs[i].SetActive(false);
		}
	}

	private void DestroyCoverDirHighlight()
	{
		m_coverDirIndicatorRenderers = null;
		if (m_coverDirHighlight != null)
		{
			HighlightUtils.DestroyObjectAndMaterials(m_coverDirHighlight);
		}
	}

	private static float GetCoverDirInitialOpacity()
	{
		if (HighlightUtils.Get() != null)
		{
			return HighlightUtils.Get().m_coverDirIndicatorInitialOpacity;
		}
		return 0.08f;
	}

	private static float GetCoverDirParticleInitialOpacity()
	{
		if (HighlightUtils.Get() != null)
		{
			return HighlightUtils.Get().m_coverDirParticleInitialOpacity;
		}
		return 0.5f;
	}

	private static float GetCoverDirIndicatorDuration()
	{
		if (HighlightUtils.Get() != null)
		{
			return Mathf.Max(0.1f, HighlightUtils.Get().m_coverDirIndicatorDuration);
		}
		return 3f;
	}

	private static float GetCoverDirIndicatorSpawnDelay()
	{
		if (HighlightUtils.Get() != null)
		{
			return Mathf.Max(0f, HighlightUtils.Get().m_coverDirIndicatorStartDelay);
		}
		return 0f;
	}

	private static float GetCoverDirFadeoutStartDelay()
	{
		float b = 1f;
		if (HighlightUtils.Get() != null)
		{
			b = HighlightUtils.Get().m_coverDirFadeoutStartDelay;
		}
		return Mathf.Min(GetCoverDirIndicatorDuration(), b);
	}

	private static float GetCoverDirIndicatorRadius()
	{
		if (HighlightUtils.Get() != null)
		{
			return HighlightUtils.Get().m_coverDirIndicatorRadiusInSquares;
		}
		return 3f;
	}

	public void AddTempCoverProvider(CoverDirections direction, bool ignoreMinDist)
	{
		if (NetworkServer.active)
		{
			TempCoverInfo item = new TempCoverInfo(direction, ignoreMinDist);
			m_syncTempCoverProviders.Add(item);
			ResetTempCoverListFromSyncList();
		}
		RecalculateCover();
	}

	public void RemoveTempCoverProvider(CoverDirections direction, bool ignoreMinDist)
	{
		if (NetworkServer.active)
		{
			bool flag = false;
			for (int i = m_syncTempCoverProviders.Count - 1; i >= 0; i--)
			{
				if (m_syncTempCoverProviders[i].m_coverDir == direction)
				{
					if (m_syncTempCoverProviders[i].m_ignoreMinDist == ignoreMinDist)
					{
						m_syncTempCoverProviders.RemoveAt(i);
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				ResetTempCoverListFromSyncList();
				RecalculateCover();
			}
			else
			{
				Log.Warning("RemoveTempCoverProvider did not find matching entry to remove");
			}
		}
	}

	public void ClearTempCoverProviders()
	{
		if (NetworkServer.active)
		{
			m_syncTempCoverProviders.Clear();
			ResetTempCoverListFromSyncList();
		}
		RecalculateCover();
	}

	public void RecalculateCover()
	{
		ActorData owner = m_owner;
		UpdateCoverFromBarriers();
		BoardSquare currentBoardSquare = owner.GetCurrentBoardSquare();
		CalcCover(out m_hasCover, currentBoardSquare, m_tempCoverProviders, m_tempCoverIgnoreMinDist, m_cachedHasCoverFromBarriers, true);
	}

	private void ResetTempCoverListFromSyncList()
	{
		m_tempCoverProviders.Clear();
		m_tempCoverIgnoreMinDist.Clear();
		for (int i = 0; i < m_syncTempCoverProviders.Count; i++)
		{
			if (m_syncTempCoverProviders[i].m_ignoreMinDist)
			{
				m_tempCoverIgnoreMinDist.Add(m_syncTempCoverProviders[i].m_coverDir);
			}
			else
			{
				m_tempCoverProviders.Add(m_syncTempCoverProviders[i].m_coverDir);
			}
		}
	}

	public void UpdateCoverFromBarriers()
	{
		for (int i = 0; i < m_cachedHasCoverFromBarriers.Length; i++)
		{
			m_cachedHasCoverFromBarriers[i] = false;
		}
		BoardSquare currentBoardSquare = m_owner.GetCurrentBoardSquare();
		if (BarrierManager.Get() != null)
		{
			if (currentBoardSquare != null)
			{
				BarrierManager.Get().UpdateCachedCoverDirections(m_owner, currentBoardSquare, ref m_cachedHasCoverFromBarriers);
			}
		}
	}

	internal static bool CalcCoverLevelGeoOnly(out bool[] hasCover, BoardSquare square)
	{
		return CalcCover(out hasCover, square, null, null, null, true);
	}

	internal static bool CalcCover(
		out bool[] hasCover,
		BoardSquare square,
		List<CoverDirections> tempCoversNormal,
		List<CoverDirections> tempCoversIgnoreMinDist,
		bool[] coverDirFromBarriers,
		bool minDistOk)
	{
		hasCover = new bool[4];
		if (square == null)
		{
			return false;
		}
		bool hasAnyCover = false;
		List<BoardSquare> list = null;
		Board.Get().GetCardinalAdjacentSquares(square.x, square.y, ref list);
		foreach (BoardSquare adjacentSquare in list)
		{
			CoverDirections coverDirection = GetCoverDirection(square, adjacentSquare);
			int heightDiff = adjacentSquare.height - square.height;
			if (minDistOk)
			{
				if (heightDiff < 1
				    && square.GetThinCover(coverDirection) == ThinCover.CoverType.None
				    && (tempCoversNormal == null || !tempCoversNormal.Contains(coverDirection))
				    && (tempCoversIgnoreMinDist == null || !tempCoversIgnoreMinDist.Contains(coverDirection))
				    && (coverDirFromBarriers == null || !coverDirFromBarriers[(int)coverDirection]))
				{
					hasCover[(int)coverDirection] = false;
				}
				else
				{
					hasCover[(int)coverDirection] = true;
					hasAnyCover = true;
				}
			}
			else
			{
				bool ignoreMinDist = tempCoversIgnoreMinDist != null && tempCoversIgnoreMinDist.Contains(coverDirection);
				hasCover[(int)coverDirection] = ignoreMinDist;
				hasAnyCover = hasAnyCover || ignoreMinDist;
			}
		}
		return hasAnyCover;
	}

	public static CoverDirections GetCoverDirection(BoardSquare srcSquare, BoardSquare destSquare)
	{
		CoverDirections result;
		if (srcSquare.x > destSquare.x)
		{
			result = CoverDirections.X_NEG;
		}
		else if (srcSquare.x < destSquare.x)
		{
			result = CoverDirections.X_POS;
		}
		else if (srcSquare.y > destSquare.y)
		{
			result = CoverDirections.Y_NEG;
		}
		else if (srcSquare.y < destSquare.y)
		{
			result = CoverDirections.Y_POS;
		}
		else
		{
			result = CoverDirections.INVALID;
		}
		return result;
	}

	public bool IsInCoverWrt(Vector3 damageOrigin)
	{
		List<CoverDirections> list = null;
		return IsInCoverWrt(damageOrigin, ref list);
	}

	public static bool IsInCoverWrt(Vector3 damageOrigin, BoardSquare targetSquare, List<CoverDirections> tempCoverProviders, List<CoverDirections> tempCoversIgnoreMinDist, bool[] coverDirFromBarriers)
	{
		List<CoverDirections> list = null;
		return IsInCoverWrt(damageOrigin, targetSquare, ref list, tempCoverProviders, tempCoversIgnoreMinDist, coverDirFromBarriers);
	}

	public bool IsInCoverWrt(Vector3 damageOrigin, ref List<CoverDirections> coverDirections)
	{
		BoardSquare currentBoardSquare = GetComponent<ActorData>().GetCurrentBoardSquare();
		return IsInCoverWrt(
			damageOrigin,
			currentBoardSquare,
			ref coverDirections,
			m_tempCoverProviders,
			m_tempCoverIgnoreMinDist,
			m_cachedHasCoverFromBarriers);
	}

	public bool IsInCoverForDirection(CoverDirections dir)
	{
		return m_hasCover[(int)dir];
	}

	public static bool IsInCoverWrt(
		Vector3 damageOrigin,
		BoardSquare targetSquare,
		ref List<CoverDirections> coverDirections,
		List<CoverDirections> tempCoverProviders,
		List<CoverDirections> tempCoversIgnoreMinDist,
		bool[] coverDirFromBarriers)
	{
		if (targetSquare == null)
		{
			return false;
		}
		Vector3 targetPos = targetSquare.ToVector3();
		Vector3 vector = damageOrigin - targetPos;
		vector.y = 0f;
		float sqrMagnitude = vector.sqrMagnitude;
		float minDist = GameplayData.Get().m_coverMinDistance * Board.Get().squareSize;
		float sqrMinDist = minDist * minDist;
		bool isRanged = sqrMagnitude >= sqrMinDist;
		if (!isRanged && (tempCoversIgnoreMinDist == null || tempCoversIgnoreMinDist.Count == 0))
		{
			return false;
		}
		int numCoverSourcesByDirectionOnly = GetNumCoverSourcesByDirectionOnly(
			damageOrigin,
			targetSquare,
			ref coverDirections,
			tempCoverProviders,
			tempCoversIgnoreMinDist,
			coverDirFromBarriers,
			isRanged);
		return numCoverSourcesByDirectionOnly > 0;
	}

	public bool IsInCoverWrtDirectionOnly(Vector3 damageOrigin, BoardSquare targetSquare)
	{
		List<CoverDirections> list = null;
		return GetNumCoverSourcesByDirectionOnly(damageOrigin, targetSquare, ref list, m_tempCoverProviders, m_tempCoverIgnoreMinDist, m_cachedHasCoverFromBarriers, true) > 0;
	}

	private static int GetNumCoverSourcesByDirectionOnly(
		Vector3 damageOrigin,
		BoardSquare targetSquare,
		ref List<CoverDirections> coverDirections,
		List<CoverDirections> tempCoverProviders,
		List<CoverDirections> tempCoverIgnoreMinDist,
		bool[] coverDirFromBarriers,
		bool minDistOk)
	{
		int num = 0;
		Vector3 targetPos = targetSquare.ToVector3();
		bool isXNeg = damageOrigin.x < targetPos.x;
		bool isXPos = damageOrigin.x > targetPos.x;
		bool isYNeg = damageOrigin.z < targetPos.z;
		bool isYPos = damageOrigin.z > targetPos.z;
		Vector2 vector = new Vector2(damageOrigin.x - targetPos.x, damageOrigin.z - targetPos.z);
		Vector2 dir = vector.normalized;
		float halfSquareSize = 0.5f * Board.Get().squareSize;
		float halfCoverAngle = GameplayData.Get().m_coverProtectionAngle / 2f;
		CalcCover(out bool[] array, targetSquare, tempCoverProviders, tempCoverIgnoreMinDist, coverDirFromBarriers, minDistOk);
		if (array[(int)CoverDirections.X_NEG]
		    && isXNeg
		    && vector.x < -halfSquareSize)
		{
			Vector2 xNegVector = new Vector2(-1f, 0f);
			float attackAngle = Mathf.Acos(Vector2.Dot(xNegVector, dir)) * Mathf.Rad2Deg; 
			if (attackAngle <= halfCoverAngle)
			{
				num++;
				if (coverDirections != null)
				{
					coverDirections.Add(CoverDirections.X_NEG);
				}
			}
		}
		if (array[(int)CoverDirections.X_POS]
		    && isXPos
		    && vector.x > halfSquareSize)
		{
			Vector2 xPosVector = new Vector2(1f, 0f);
			float attackAngle = Mathf.Acos(Vector2.Dot(xPosVector, dir)) * Mathf.Rad2Deg;
			if (attackAngle <= halfCoverAngle)
			{
				num++;
				if (coverDirections != null)
				{
					coverDirections.Add(CoverDirections.X_POS);
				}
			}
		}
		if (array[(int)CoverDirections.Y_NEG]
		    && isYNeg
		    && vector.y < -halfSquareSize)
		{
			Vector2 yNegVector = new Vector2(0f, -1f);
			float attackAngle = Mathf.Acos(Vector2.Dot(yNegVector, dir)) * Mathf.Rad2Deg;
			if (attackAngle <= halfCoverAngle)
			{
				num++;
				if (coverDirections != null)
				{
					coverDirections.Add(CoverDirections.Y_NEG);
				}
			}
		}
		if (array[(int)CoverDirections.Y_POS] && isYPos && vector.y > halfSquareSize)
		{
			Vector2 yPosVector = new Vector2(0f, 1f);
			float attackAngle = Mathf.Acos(Vector2.Dot(yPosVector, dir)) * Mathf.Rad2Deg;
			if (attackAngle <= halfCoverAngle)
			{
				num++;
				if (coverDirections != null)
				{
					coverDirections.Add(CoverDirections.Y_POS);
				}
			}
		}
		return num;
	}

	public bool IsDirInCover(Vector3 dir)
	{
		float angle_deg = VectorUtils.HorizontalAngle_Deg(dir);
		List<CoverRegion> coveredRegions = GetCoveredRegions();
		bool result = false;
		using (List<CoverRegion>.Enumerator enumerator = coveredRegions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CoverRegion coverRegion = enumerator.Current;
				if (coverRegion.IsDirInCover(angle_deg))
				{
					return true;
				}
			}
		}
		return result;
	}

	public List<CoverRegion> GetCoveredRegions()
	{
		List<CoverRegion> list = new List<CoverRegion>();
		ActorData owner = m_owner;
		if (owner == null)
		{
			Debug.LogError("Trying to get the covered regions for a null actor.");
			return list;
		}
		if (owner.IsDead())
		{
			Debug.LogError(string.Concat("Trying to get the covered regions for the dead actor ", owner.DisplayName, ", a ", owner.name, "."));
			return list;
		}
		if (owner.GetCurrentBoardSquare() == null)
		{
			Debug.LogError(string.Concat("Trying to get the covered regions for the (alive) actor ", owner.DisplayName, ", a ", owner.name, ", but the square is null."));
			return list;
		}
		BoardSquare currentBoardSquare = owner.GetCurrentBoardSquare();
		Vector3 center = currentBoardSquare.ToVector3();
		float num = GameplayData.Get().m_coverProtectionAngle / 2f;
		for (int i = 0; i < 4; i++)
		{
			if (m_hasCover[i])
			{
				CoverDirections dir = (CoverDirections)i;
				float centerAngleOfDirection = GetCenterAngleOfDirection(dir);
				CoverRegion item = new CoverRegion(center, centerAngleOfDirection - num, centerAngleOfDirection + num);
				list.Add(item);
			}
		}
		if (list.Count != 0)
		{
			if (list.Count != 1)
			{
				if (list.Count == 4)
				{
					return new List<CoverRegion>
					{
						new CoverRegion(center, -720f, 720f)
					};
				}
				if (list.Count == 3)
				{
					float num2 = list[0].m_startAngle;
					float num3 = list[0].m_endAngle;
					foreach (CoverRegion coverRegion in list)
					{
						num2 = Mathf.Min(num2, coverRegion.m_startAngle);
						num3 = Mathf.Max(num3, coverRegion.m_endAngle);
					}
					return new List<CoverRegion>
					{
						new CoverRegion(center, num2, num3)
					};
				}
				if (list.Count == 2)
				{
					CoverRegion coverRegion2 = list[0];
					CoverRegion coverRegion3 = list[1];
					bool flag;
					if (coverRegion2.m_startAngle <= coverRegion3.m_startAngle)
					{
						flag = (coverRegion3.m_startAngle <= coverRegion2.m_endAngle);
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					bool flag3 = coverRegion2.m_startAngle <= coverRegion3.m_endAngle && coverRegion3.m_endAngle <= coverRegion2.m_endAngle;
					if (!flag2)
					{
						if (!flag3)
						{
							return list;
						}
					}
					float startAngle = Mathf.Min(coverRegion2.m_startAngle, coverRegion3.m_startAngle);
					float endAngle = Mathf.Max(coverRegion2.m_endAngle, coverRegion3.m_endAngle);
					return new List<CoverRegion>
					{
						new CoverRegion(center, startAngle, endAngle)
					};
				}
				Log.Error(string.Concat("Actor ", owner.DisplayName, " in cover in ", list.Count, " directions."));
				return list;
			}
		}
		return list;
	}

	public void ClampConeToValidCover(float coneDirAngleDegrees, float coneWidthDegrees, out float newDirAngleDegrees, out Vector3 newConeDir)
	{
		float num = coneDirAngleDegrees;
		List<CoverRegion> coveredRegions = GetCoveredRegions();
		bool flag = false;
		float angle_deg = coneDirAngleDegrees - coneWidthDegrees / 2f;
		float angle_deg2 = coneDirAngleDegrees + coneWidthDegrees / 2f;
		foreach (CoverRegion coverRegion in coveredRegions)
		{
			bool flag2 = coverRegion.IsDirInCover(angle_deg);
			bool flag3 = coverRegion.IsDirInCover(angle_deg2);
			if (flag2)
			{
				if (flag3)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			using (List<CoverRegion>.Enumerator enumerator2 = coveredRegions.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CoverRegion coverRegion2 = enumerator2.Current;
					bool flag4 = coverRegion2.IsDirInCover(coneDirAngleDegrees);
					if (flag4)
					{
						bool flag5 = coverRegion2.IsDirInCover(angle_deg) && !coverRegion2.IsDirInCover(angle_deg2);
						bool flag6;
						if (!coverRegion2.IsDirInCover(angle_deg))
						{
							flag6 = coverRegion2.IsDirInCover(angle_deg2);
						}
						else
						{
							flag6 = false;
						}
						bool flag7 = flag6;
						if (flag5)
						{
							num = coverRegion2.m_endAngle - coneWidthDegrees / 2f;
						}
						else if (flag7)
						{
							num = coverRegion2.m_startAngle + coneWidthDegrees / 2f;
						}
					}
				}
			}
		}
		newDirAngleDegrees = num;
		newConeDir = VectorUtils.AngleDegreesToVector(num);
	}

	private static float GetCenterAngleOfDirection(CoverDirections dir)
	{
		float result;
		switch (dir)
		{
		case CoverDirections.X_POS:
			result = 0f;
			break;
		case CoverDirections.X_NEG:
			result = 180f;
			break;
		case CoverDirections.Y_POS:
			result = 90f;
			break;
		case CoverDirections.Y_NEG:
			result = 270f;
			break;
		default:
			result = 0f;
			break;
		}
		return result;
	}

	public static bool DoesCoverDirectionProvideCoverFromPos(CoverDirections dir, Vector3 coveredPos, Vector3 attackOriginPos)
	{
		float num = GameplayData.Get().m_coverProtectionAngle / 2f;
		float centerAngleOfDirection = GetCenterAngleOfDirection(dir);
		CoverRegion coverRegion = new CoverRegion(coveredPos, centerAngleOfDirection - num, centerAngleOfDirection + num);
		return coverRegion.IsInCoverFromPos(attackOriginPos);
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_syncTempCoverProviders(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_syncTempCoverProviders called on server.");
			return;
		}
		((ActorCover)obj).m_syncTempCoverProviders.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			GeneratedNetworkCode._WriteStructSyncListTempCoverInfo_None(writer, m_syncTempCoverProviders);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			GeneratedNetworkCode._WriteStructSyncListTempCoverInfo_None(writer, m_syncTempCoverProviders);
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
			GeneratedNetworkCode._ReadStructSyncListTempCoverInfo_None(reader, m_syncTempCoverProviders);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			GeneratedNetworkCode._ReadStructSyncListTempCoverInfo_None(reader, m_syncTempCoverProviders);
		}
	}

	public enum CoverDirections
	{
		INVALID = -1,
		X_POS,
		X_NEG,
		Y_POS,
		Y_NEG,
		NUM,
		FIRST = 0
	}
}
