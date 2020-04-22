using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorCover : NetworkBehaviour
{
	public enum CoverDirections
	{
		INVALID = -1,
		X_POS = 0,
		X_NEG = 1,
		Y_POS = 2,
		Y_NEG = 3,
		NUM = 4,
		FIRST = 0
	}

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

	private static Vector3[] m_coverDir;

	private float m_coverHeight = 2f;

	private float m_coverDirIndicatorHideTime = -1f;

	private float m_coverDirIndicatorFadeStartTime = -1f;

	private float m_coverDirIndicatorSpawnTime = -1f;

	private GameObject m_coverDirHighlight;

	private MeshRenderer[] m_coverDirIndicatorRenderers;

	private EasedFloatCubic m_coverDirIndicatorOpacity = new EasedFloatCubic(1f);

	private static int kListm_syncTempCoverProviders;

	static ActorCover()
	{
		m_coverDir = new Vector3[4];
		kListm_syncTempCoverProviders = 1438054992;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorCover), kListm_syncTempCoverProviders, InvokeSyncListm_syncTempCoverProviders);
		NetworkCRC.RegisterBehaviour("ActorCover", 0);
	}

	public bool HasAnyCover(bool recalculate = false)
	{
		if (recalculate)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			RecalculateCover();
		}
		bool result = false;
		for (int i = 0; i < m_hasCover.Length; i++)
		{
			if (m_hasCover[i])
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
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			InitCoverObjs(m_mouseOverCoverObjs, HighlightUtils.Get().m_coverIndicatorPrefab);
			InitCoverObjs(m_actorCoverObjs, HighlightUtils.Get().m_coverShieldOnlyPrefab);
			for (int j = 0; j < m_actorCoverObjs.Length; j++)
			{
				ParticleSystemRenderer[] array = null;
				if (m_actorCoverObjs[j] != null)
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
					array = m_actorCoverObjs[j].GetComponentsInChildren<ParticleSystemRenderer>();
				}
				else
				{
					array = new ParticleSystemRenderer[0];
				}
				m_actorCoverSymbolRenderers.Add(array);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				m_coverDir[1] = Vector3.left;
				m_coverDir[0] = Vector3.right;
				m_coverDir[3] = Vector3.back;
				m_coverDir[2] = Vector3.forward;
				m_owner = GetComponent<ActorData>();
				m_syncTempCoverProviders.InitializeBehaviour(this, kListm_syncTempCoverProviders);
				return;
			}
		}
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
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Clear();
			particleSystem.time = 0f;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private GameObject CreateCoverIndicatorObject(float yRotation, GameObject coverPrefab)
	{
		GameObject gameObject = Object.Instantiate(coverPrefab);
		gameObject.transform.Rotate(Vector3.up, yRotation);
		return gameObject;
	}

	private void SetCoverMeshColor(GameObject particleObject, Color color)
	{
		if (!(particleObject != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ParticleSystemRenderer[] componentsInChildren = particleObject.GetComponentsInChildren<ParticleSystemRenderer>();
			ParticleSystemRenderer[] array = componentsInChildren;
			foreach (ParticleSystemRenderer particleSystemRenderer in array)
			{
				AbilityUtil_Targeter.SetMaterialColor(particleSystemRenderer.materials, color);
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

	public void DisableCover()
	{
		for (int i = 0; i < 4; i++)
		{
			m_hasCover[i] = false;
			m_cachedHasCoverFromBarriers[i] = false;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public Vector3 GetCoverOffset(CoverDirections dir)
	{
		return GetCoverOffsetStatic(dir);
	}

	public static Vector3 GetCoverOffsetStatic(CoverDirections dir)
	{
		float num = Board.Get().squareSize * 0.5f;
		Vector3 result = Vector3.zero;
		if (dir == CoverDirections.X_POS)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = new Vector3(num, 0f, 0f);
		}
		else if (dir == CoverDirections.X_NEG)
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
			result = new Vector3(0f - num, 0f, 0f);
		}
		else if (dir == CoverDirections.Y_POS)
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
			result = new Vector3(0f, 0f, num);
		}
		else if (dir == CoverDirections.Y_NEG)
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
			result = new Vector3(0f, 0f, 0f - num);
		}
		return result;
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return Quaternion.LookRotation(Vector3.back);
			}
		default:
			return Quaternion.LookRotation(Vector3.forward);
		}
	}

	public bool HasNonThinCover(BoardSquare currentSquare, int xDelta, int yDelta, bool halfHeight)
	{
		bool result = false;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(currentSquare.x + xDelta, currentSquare.y + yDelta);
		if (boardSquare != null)
		{
			int num = boardSquare.height - currentSquare.height;
			result = ((!halfHeight) ? (num == 2) : (num == 1));
		}
		return result;
	}

	public float CoverRating(BoardSquare square)
	{
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(m_owner.GetOpposingTeam());
		float num = 0f;
		foreach (ActorData item in allTeamMembers)
		{
			if (item.IsDead())
			{
				continue;
			}
			while (true)
			{
				switch (2)
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
			if (!(item.GetCurrentBoardSquare() != null))
			{
				continue;
			}
			Vector3 vector = item.GetCurrentBoardSquare().transform.position - square.transform.position;
			if (!(vector.magnitude > Board.Get().squareSize * 1.5f))
			{
				continue;
			}
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
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
				if (!(vector.x < 0f))
				{
					goto IL_011c;
				}
				if (!HasNonThinCover(square, -1, 0, true))
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
					if (square.GetCoverInDirection(CoverDirections.X_NEG) != ThinCover.CoverType.Half)
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
						goto IL_011c;
					}
				}
				goto IL_0162;
			}
			if (vector.z < 0f)
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
				if (HasNonThinCover(square, 0, -1, true) || square.GetCoverInDirection(CoverDirections.Y_NEG) == ThinCover.CoverType.Half)
				{
					goto IL_0280;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (vector.z > 0f)
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
				if (!HasNonThinCover(square, 0, 1, true))
				{
					if (square.GetCoverInDirection(CoverDirections.Y_POS) != ThinCover.CoverType.Half)
					{
						goto IL_028d;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				goto IL_0280;
			}
			goto IL_028d;
			IL_0162:
			num += 1f;
			continue;
			IL_016f:
			if (!(vector.x < 0f))
			{
				goto IL_01b5;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!HasNonThinCover(square, -1, 0, false))
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
				if (square.GetCoverInDirection(CoverDirections.X_NEG) != ThinCover.CoverType.Full)
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
					goto IL_01b5;
				}
			}
			goto IL_01fb;
			IL_0280:
			num += 1f;
			continue;
			IL_011c:
			if (vector.x > 0f)
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
				if (!HasNonThinCover(square, 1, 0, true))
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
					if (square.GetCoverInDirection(CoverDirections.X_POS) != ThinCover.CoverType.Half)
					{
						goto IL_016f;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				goto IL_0162;
			}
			goto IL_016f;
			IL_028d:
			if (!(vector.z < 0f))
			{
				goto IL_02bd;
			}
			if (!HasNonThinCover(square, 0, -1, false))
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
				if (square.GetCoverInDirection(CoverDirections.Y_NEG) != ThinCover.CoverType.Full)
				{
					goto IL_02bd;
				}
			}
			goto IL_0303;
			IL_0303:
			num += 0.5f;
			continue;
			IL_02bd:
			if (!(vector.z > 0f))
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!HasNonThinCover(square, 0, 1, false))
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
				if (square.GetCoverInDirection(CoverDirections.Y_POS) != ThinCover.CoverType.Full)
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			goto IL_0303;
			IL_01fb:
			num += 0.5f;
			continue;
			IL_01b5:
			if (!(vector.x > 0f))
			{
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!HasNonThinCover(square, 1, 0, false))
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
				if (square.GetCoverInDirection(CoverDirections.X_POS) != ThinCover.CoverType.Full)
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			goto IL_01fb;
		}
		return num;
	}

	internal void UpdateCoverHighlights(BoardSquare currentSquare)
	{
		ActorData owner = m_owner;
		if (currentSquare != null && currentSquare.IsBaselineHeight())
		{
			ActorTurnSM actorTurnSM = owner.GetActorTurnSM();
			if (!(actorTurnSM != null))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				bool flag = actorTurnSM.AmTargetingAction();
				List<BoardSquare> result = null;
				Board.Get().GetStraightAdjacentSquares(currentSquare.x, currentSquare.y, ref result);
				if (result == null)
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
					for (int i = 0; i < result.Count; i++)
					{
						BoardSquare boardSquare = result[i];
						if (boardSquare == null)
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
							continue;
						}
						CoverDirections coverDirection = GetCoverDirection(currentSquare, boardSquare);
						int num = boardSquare.height - currentSquare.height;
						object obj;
						if ((int)coverDirection < m_mouseOverCoverObjs.Length)
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
							obj = m_mouseOverCoverObjs[(int)coverDirection];
						}
						else
						{
							obj = null;
						}
						GameObject gameObject = (GameObject)obj;
						if (!(gameObject != null))
						{
							continue;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num < 1)
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
							if (currentSquare.GetCoverInDirection(coverDirection) == ThinCover.CoverType.None)
							{
								goto IL_01e5;
							}
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (!flag)
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
							if (actorTurnSM.CurrentState != TurnStateEnum.PICKING_RESPAWN)
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
								Vector3 vector = new Vector3(currentSquare.worldX, (float)currentSquare.height + m_coverHeight, currentSquare.worldY);
								vector += GetCoverOffset(coverDirection);
								if (!(gameObject.transform.position != vector))
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
									if (gameObject.activeSelf)
									{
										continue;
									}
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								gameObject.transform.position = vector;
								gameObject.SetActive(true);
								ResetParticleTime(gameObject);
								continue;
							}
						}
						goto IL_01e5;
						IL_01e5:
						gameObject.SetActive(false);
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
			}
		}
		for (int j = 0; j < m_mouseOverCoverObjs.Length; j++)
		{
			GameObject gameObject2 = m_mouseOverCoverObjs[j];
			if ((bool)gameObject2)
			{
				gameObject2.SetActive(false);
			}
		}
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

	private void Update()
	{
		if (m_coverDirHighlight != null && m_coverDirIndicatorRenderers != null)
		{
			float opacity = (float)m_coverDirIndicatorOpacity * GetCoverDirInitialOpacity();
			for (int i = 0; i < m_coverDirIndicatorRenderers.Length; i++)
			{
				MeshRenderer meshRenderer = m_coverDirIndicatorRenderers[i];
				if (meshRenderer != null)
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
					AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, opacity);
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			float opacity2 = (float)m_coverDirIndicatorOpacity * GetCoverDirParticleInitialOpacity();
			for (int j = 0; j < m_hasCover.Length; j++)
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
				if (j >= m_actorCoverSymbolRenderers.Count)
				{
					break;
				}
				if (m_hasCover[j])
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
					ParticleSystemRenderer[] array = m_actorCoverSymbolRenderers[j];
					foreach (ParticleSystemRenderer particleSystemRenderer in array)
					{
						AbilityUtil_Targeter.SetMaterialOpacity(particleSystemRenderer.materials, opacity2);
					}
					while (true)
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
		}
		if (m_coverDirIndicatorSpawnTime > 0f && Time.time > m_coverDirIndicatorSpawnTime)
		{
			ShowAllRelevantCoverIndicator();
			m_coverDirIndicatorSpawnTime = -1f;
		}
		if (m_coverDirIndicatorFadeStartTime > 0f)
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
			if (Time.time > m_coverDirIndicatorFadeStartTime)
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
				m_coverDirIndicatorOpacity.EaseTo(0f, GetCoverDirIndicatorDuration() - GetCoverDirFadeoutStartDelay());
				m_coverDirIndicatorFadeStartTime = -1f;
			}
		}
		if (!(m_coverDirIndicatorHideTime > 0f))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (Time.time > m_coverDirIndicatorHideTime)
			{
				HideRelevantCover();
				DestroyCoverDirHighlight();
				m_coverDirIndicatorHideTime = -1f;
			}
			return;
		}
	}

	public void ShowRelevantCover(Vector3 damageOrigin)
	{
		List<CoverDirections> coverDirections = new List<CoverDirections>();
		if (IsInCoverWrt(damageOrigin, ref coverDirections))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					BoardSquare currentBoardSquare = m_owner.GetCurrentBoardSquare();
					for (int i = 0; i < 4; i++)
					{
						CoverDirections coverDirections2 = (CoverDirections)i;
						if (coverDirections.Contains(coverDirections2))
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
							Vector3 a = new Vector3(currentBoardSquare.worldX, (float)currentBoardSquare.height + m_coverHeight, currentBoardSquare.worldY);
							m_actorCoverObjs[i].transform.position = a + GetCoverOffset(coverDirections2);
							m_actorCoverObjs[i].SetActive(true);
						}
						else
						{
							m_actorCoverObjs[i].SetActive(false);
						}
					}
					return;
				}
				}
			}
		}
		HideRelevantCover();
	}

	public void StartShowMoveIntoCoverIndicator()
	{
		if (HasAnyCover())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_coverDirIndicatorSpawnTime = Time.time + GetCoverDirIndicatorSpawnDelay();
					return;
				}
			}
		}
		m_coverDirIndicatorSpawnTime = -1f;
	}

	public static GameObject CreateCoverDirIndicator(bool[] hasCoverFlags, Color color, float radiusInSquares)
	{
		float num;
		if ((bool)GameplayData.Get())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num4++;
			}
		}
		if (num4 == 2)
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
			flag = (hasCoverFlags[1] == hasCoverFlags[0]);
		}
		float borderStartOffset = 0.7f;
		GameObject gameObject2 = HighlightUtils.Get().CreateDynamicConeMesh(radiusInSquares, num2, false);
		HighlightUtils.Get().SetDynamicConeMeshBorderActive(gameObject2, false);
		UIDynamicCone component = gameObject2.GetComponent<UIDynamicCone>();
		if (component != null)
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
			component.SetBorderStartOffset(borderStartOffset);
		}
		Vector3 forward = Vector3.forward;
		if (num4 <= 3)
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
				Vector3 zero = Vector3.zero;
				for (int j = 0; j < m_coverDir.Length; j++)
				{
					if (hasCoverFlags[j])
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
						zero += m_coverDir[j];
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				forward = (zero / num4).normalized;
			}
		}
		if (num4 == 2)
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
			if (flag)
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
				GameObject gameObject3 = HighlightUtils.Get().CreateDynamicConeMesh(radiusInSquares, num2, false);
				HighlightUtils.Get().SetDynamicConeMeshBorderActive(gameObject3, false);
				UIDynamicCone component2 = gameObject3.GetComponent<UIDynamicCone>();
				if (component2 != null)
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
					component2.SetBorderStartOffset(borderStartOffset);
				}
				MeshRenderer[] componentsInChildren = gameObject3.GetComponentsInChildren<MeshRenderer>();
				MeshRenderer[] array = componentsInChildren;
				foreach (MeshRenderer meshRenderer in array)
				{
					if (HighlightUtils.Get() != null)
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
						AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, color);
					}
					AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, GetCoverDirInitialOpacity());
				}
				if (hasCoverFlags[1])
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
				goto IL_0320;
			}
		}
		if (num4 == 2)
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
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject2, radiusInSquares, 180f + num3);
		}
		else if (num4 == 3)
		{
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject2, radiusInSquares, 270f + num3);
		}
		else if (num4 == 4)
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
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject2, radiusInSquares, 360f);
		}
		goto IL_0320;
		IL_0320:
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localRotation = Quaternion.LookRotation(forward);
		gameObject2.transform.localPosition = Vector3.zero;
		MeshRenderer[] componentsInChildren2 = gameObject2.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array2 = componentsInChildren2;
		foreach (MeshRenderer meshRenderer2 in array2)
		{
			if (HighlightUtils.Get() != null)
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
				AbilityUtil_Targeter.SetMaterialColor(meshRenderer2.materials, color);
			}
			AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer2.materials, GetCoverDirInitialOpacity());
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return gameObject;
		}
	}

	private void ShowCoverIndicatorForDirection(CoverDirections dir)
	{
		BoardSquare boardSquare = (!(m_owner != null)) ? null : m_owner.GetCurrentBoardSquare();
		if (!(boardSquare != null))
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
			if ((int)dir >= m_hasCover.Length)
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
				if ((int)dir >= m_actorCoverObjs.Length)
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
					if (!(m_actorCoverObjs[(int)dir] != null))
					{
						return;
					}
					if (m_hasCover[(int)dir])
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
							{
								Vector3 a = new Vector3(boardSquare.worldX, (float)boardSquare.height + m_coverHeight, boardSquare.worldY);
								m_actorCoverObjs[(int)dir].transform.position = a + GetCoverOffset(dir);
								m_actorCoverObjs[(int)dir].SetActive(true);
								ResetParticleTime(m_actorCoverObjs[(int)dir]);
								return;
							}
							}
						}
					}
					m_actorCoverObjs[(int)dir].SetActive(false);
					return;
				}
			}
		}
	}

	private void ShowAllRelevantCoverIndicator()
	{
		if (!HasAnyCover())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		ShowCoverIndicatorForDirection(CoverDirections.X_NEG);
		ShowCoverIndicatorForDirection(CoverDirections.X_POS);
		ShowCoverIndicatorForDirection(CoverDirections.Y_NEG);
		ShowCoverIndicatorForDirection(CoverDirections.Y_POS);
		DestroyCoverDirHighlight();
		object obj;
		if (m_owner != null)
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
			obj = m_owner.GetCurrentBoardSquare();
		}
		else
		{
			obj = null;
		}
		BoardSquare boardSquare = (BoardSquare)obj;
		if (boardSquare != null)
		{
			Vector3 position = boardSquare.ToVector3();
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
		if (!(m_coverDirHighlight != null))
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
			HighlightUtils.DestroyObjectAndMaterials(m_coverDirHighlight);
			return;
		}
	}

	private static float GetCoverDirInitialOpacity()
	{
		if (HighlightUtils.Get() != null)
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
					return HighlightUtils.Get().m_coverDirIndicatorInitialOpacity;
				}
			}
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
					return Mathf.Max(0.1f, HighlightUtils.Get().m_coverDirIndicatorDuration);
				}
			}
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
			while (true)
			{
				switch (2)
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
			b = HighlightUtils.Get().m_coverDirFadeoutStartDelay;
		}
		return Mathf.Min(GetCoverDirIndicatorDuration(), b);
	}

	private static float GetCoverDirIndicatorRadius()
	{
		if (HighlightUtils.Get() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return HighlightUtils.Get().m_coverDirIndicatorRadiusInSquares;
				}
			}
		}
		return 3f;
	}

	public void AddTempCoverProvider(CoverDirections direction, bool ignoreMinDist)
	{
		if (NetworkServer.active)
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
			TempCoverInfo item = new TempCoverInfo(direction, ignoreMinDist);
			m_syncTempCoverProviders.Add(item);
			ResetTempCoverListFromSyncList();
		}
		RecalculateCover();
	}

	public void RemoveTempCoverProvider(CoverDirections direction, bool ignoreMinDist)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		bool flag = false;
		for (int num = m_syncTempCoverProviders.Count - 1; num >= 0; num--)
		{
			TempCoverInfo tempCoverInfo = m_syncTempCoverProviders[num];
			if (tempCoverInfo.m_coverDir == direction)
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
				TempCoverInfo tempCoverInfo2 = m_syncTempCoverProviders[num];
				if (tempCoverInfo2.m_ignoreMinDist == ignoreMinDist)
				{
					m_syncTempCoverProviders.RemoveAt(num);
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					ResetTempCoverListFromSyncList();
					RecalculateCover();
					return;
				}
			}
		}
		Log.Warning("RemoveTempCoverProvider did not find matching entry to remove");
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
			TempCoverInfo tempCoverInfo = m_syncTempCoverProviders[i];
			if (tempCoverInfo.m_ignoreMinDist)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				List<CoverDirections> tempCoverIgnoreMinDist = m_tempCoverIgnoreMinDist;
				TempCoverInfo tempCoverInfo2 = m_syncTempCoverProviders[i];
				tempCoverIgnoreMinDist.Add(tempCoverInfo2.m_coverDir);
			}
			else
			{
				List<CoverDirections> tempCoverProviders = m_tempCoverProviders;
				TempCoverInfo tempCoverInfo3 = m_syncTempCoverProviders[i];
				tempCoverProviders.Add(tempCoverInfo3.m_coverDir);
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

	public void UpdateCoverFromBarriers()
	{
		for (int i = 0; i < m_cachedHasCoverFromBarriers.Length; i++)
		{
			m_cachedHasCoverFromBarriers[i] = false;
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
			BoardSquare currentBoardSquare = m_owner.GetCurrentBoardSquare();
			if (!(BarrierManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (currentBoardSquare != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						BarrierManager.Get().UpdateCachedCoverDirections(m_owner, currentBoardSquare, ref m_cachedHasCoverFromBarriers);
						return;
					}
				}
				return;
			}
		}
	}

	internal static bool CalcCoverLevelGeoOnly(out bool[] hasCover, BoardSquare square)
	{
		return CalcCover(out hasCover, square, null, null, null, true);
	}

	internal static bool CalcCover(out bool[] hasCover, BoardSquare square, List<CoverDirections> tempCoversNormal, List<CoverDirections> tempCoversIgnoreMinDist, bool[] coverDirFromBarriers, bool minDistOk)
	{
		hasCover = new bool[4];
		bool flag = false;
		if (square != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					List<BoardSquare> result = null;
					Board.Get().GetStraightAdjacentSquares(square.x, square.y, ref result);
					{
						foreach (BoardSquare item in result)
						{
							CoverDirections coverDirection = GetCoverDirection(square, item);
							int num = item.height - square.height;
							if (!minDistOk)
							{
								int num2;
								if (tempCoversIgnoreMinDist != null)
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
									num2 = (tempCoversIgnoreMinDist.Contains(coverDirection) ? 1 : 0);
								}
								else
								{
									num2 = 0;
								}
								bool flag2 = (byte)num2 != 0;
								hasCover[(int)coverDirection] = flag2;
								flag = (flag || flag2);
								continue;
							}
							if (num < 1)
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
								if (square.GetCoverInDirection(coverDirection) == ThinCover.CoverType.None)
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
									if (tempCoversNormal != null)
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
										if (tempCoversNormal.Contains(coverDirection))
										{
											goto IL_0129;
										}
										while (true)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									if (tempCoversIgnoreMinDist != null)
									{
										if (tempCoversIgnoreMinDist.Contains(coverDirection))
										{
											goto IL_0129;
										}
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									if (coverDirFromBarriers != null)
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
										if (coverDirFromBarriers[(int)coverDirection])
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
											goto IL_0129;
										}
									}
									hasCover[(int)coverDirection] = false;
									continue;
								}
							}
							goto IL_0129;
							IL_0129:
							hasCover[(int)coverDirection] = true;
							flag = true;
						}
						return flag;
					}
				}
				}
			}
		}
		return flag;
	}

	public static CoverDirections GetCoverDirection(BoardSquare srcSquare, BoardSquare destSquare)
	{
		if (srcSquare.x > destSquare.x)
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
					return CoverDirections.X_NEG;
				}
			}
		}
		if (srcSquare.x < destSquare.x)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return CoverDirections.X_POS;
				}
			}
		}
		if (srcSquare.y > destSquare.y)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return CoverDirections.Y_NEG;
				}
			}
		}
		if (srcSquare.y < destSquare.y)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return CoverDirections.Y_POS;
				}
			}
		}
		return CoverDirections.INVALID;
	}

	public bool IsInCoverWrt(Vector3 damageOrigin)
	{
		List<CoverDirections> coverDirections = null;
		return IsInCoverWrt(damageOrigin, ref coverDirections);
	}

	public static bool IsInCoverWrt(Vector3 damageOrigin, BoardSquare targetSquare, List<CoverDirections> tempCoverProviders, List<CoverDirections> tempCoversIgnoreMinDist, bool[] coverDirFromBarriers)
	{
		List<CoverDirections> coverDirections = null;
		return IsInCoverWrt(damageOrigin, targetSquare, ref coverDirections, tempCoverProviders, tempCoversIgnoreMinDist, coverDirFromBarriers);
	}

	public bool IsInCoverWrt(Vector3 damageOrigin, ref List<CoverDirections> coverDirections)
	{
		ActorData component = GetComponent<ActorData>();
		BoardSquare currentBoardSquare = component.GetCurrentBoardSquare();
		return IsInCoverWrt(damageOrigin, currentBoardSquare, ref coverDirections, m_tempCoverProviders, m_tempCoverIgnoreMinDist, m_cachedHasCoverFromBarriers);
	}

	public bool IsInCoverForDirection(CoverDirections dir)
	{
		return m_hasCover[(int)dir];
	}

	public static bool IsInCoverWrt(Vector3 damageOrigin, BoardSquare targetSquare, ref List<CoverDirections> coverDirections, List<CoverDirections> tempCoverProviders, List<CoverDirections> tempCoversIgnoreMinDist, bool[] coverDirFromBarriers)
	{
		if (targetSquare == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return false;
			}
		}
		Vector3 b = targetSquare.ToVector3();
		Vector3 vector = damageOrigin - b;
		vector.y = 0f;
		float sqrMagnitude = vector.sqrMagnitude;
		float num = GameplayData.Get().m_coverMinDistance * Board.Get().squareSize;
		float num2 = num * num;
		bool flag = sqrMagnitude >= num2;
		if (!flag)
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
			if (tempCoversIgnoreMinDist != null)
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
				if (tempCoversIgnoreMinDist.Count != 0)
				{
					goto IL_00a1;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return false;
		}
		goto IL_00a1;
		IL_00a1:
		int numCoverSourcesByDirectionOnly = GetNumCoverSourcesByDirectionOnly(damageOrigin, targetSquare, ref coverDirections, tempCoverProviders, tempCoversIgnoreMinDist, coverDirFromBarriers, flag);
		return numCoverSourcesByDirectionOnly > 0;
	}

	public bool IsInCoverWrtDirectionOnly(Vector3 damageOrigin, BoardSquare targetSquare)
	{
		List<CoverDirections> coverDirections = null;
		return GetNumCoverSourcesByDirectionOnly(damageOrigin, targetSquare, ref coverDirections, m_tempCoverProviders, m_tempCoverIgnoreMinDist, m_cachedHasCoverFromBarriers, true) > 0;
	}

	private static int GetNumCoverSourcesByDirectionOnly(Vector3 damageOrigin, BoardSquare targetSquare, ref List<CoverDirections> coverDirections, List<CoverDirections> tempCoverProviders, List<CoverDirections> tempCoverIgnoreMinDist, bool[] coverDirFromBarriers, bool minDistOk)
	{
		int num = 0;
		Vector3 vector = targetSquare.ToVector3();
		bool flag = damageOrigin.x < vector.x;
		bool flag2 = damageOrigin.x > vector.x;
		bool flag3 = damageOrigin.z < vector.z;
		bool flag4 = damageOrigin.z > vector.z;
		Vector2 vector2 = new Vector2(damageOrigin.x - vector.x, damageOrigin.z - vector.z);
		Vector2 normalized = vector2.normalized;
		float num2 = 0.5f * Board.Get().squareSize;
		float num3 = GameplayData.Get().m_coverProtectionAngle / 2f;
		CalcCover(out bool[] hasCover, targetSquare, tempCoverProviders, tempCoverIgnoreMinDist, coverDirFromBarriers, minDistOk);
		if (hasCover[1])
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (flag)
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
				if (vector2.x < 0f - num2)
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
					Vector2 lhs = new Vector2(-1f, 0f);
					float num4 = Mathf.Acos(Vector2.Dot(lhs, normalized));
					float num5 = num4 * 57.29578f;
					if (num5 <= num3)
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
						num++;
						if (coverDirections != null)
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
							coverDirections.Add(CoverDirections.X_NEG);
						}
					}
				}
			}
		}
		if (hasCover[0] && flag2)
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
			if (vector2.x > num2)
			{
				Vector2 lhs2 = new Vector2(1f, 0f);
				float num6 = Mathf.Acos(Vector2.Dot(lhs2, normalized));
				float num7 = num6 * 57.29578f;
				if (num7 <= num3)
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
					num++;
					if (coverDirections != null)
					{
						coverDirections.Add(CoverDirections.X_POS);
					}
				}
			}
		}
		if (hasCover[3] && flag3)
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
			if (vector2.y < 0f - num2)
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
				Vector2 lhs3 = new Vector2(0f, -1f);
				float num8 = Mathf.Acos(Vector2.Dot(lhs3, normalized));
				float num9 = num8 * 57.29578f;
				if (num9 <= num3)
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
					num++;
					if (coverDirections != null)
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
						coverDirections.Add(CoverDirections.Y_NEG);
					}
				}
			}
		}
		if (hasCover[2])
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
			if (flag4 && vector2.y > num2)
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
				Vector2 lhs4 = new Vector2(0f, 1f);
				float num10 = Mathf.Acos(Vector2.Dot(lhs4, normalized));
				float num11 = num10 * 57.29578f;
				if (num11 <= num3)
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
					num++;
					if (coverDirections != null)
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
						coverDirections.Add(CoverDirections.Y_POS);
					}
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
				CoverRegion current = enumerator.Current;
				if (current.IsDirInCover(angle_deg))
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
							return true;
						}
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public List<CoverRegion> GetCoveredRegions()
	{
		List<CoverRegion> list = new List<CoverRegion>();
		ActorData owner = m_owner;
		if (owner == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("Trying to get the covered regions for a null actor.");
					return list;
				}
			}
		}
		if (owner.IsDead())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("Trying to get the covered regions for the dead actor " + owner.DisplayName + ", a " + owner.name + ".");
					return list;
				}
			}
		}
		if (owner.GetCurrentBoardSquare() == null)
		{
			Debug.LogError("Trying to get the covered regions for the (alive) actor " + owner.DisplayName + ", a " + owner.name + ", but the square is null.");
			return list;
		}
		BoardSquare currentBoardSquare = owner.GetCurrentBoardSquare();
		Vector3 center = currentBoardSquare.ToVector3();
		float num = GameplayData.Get().m_coverProtectionAngle / 2f;
		for (int i = 0; i < 4; i++)
		{
			if (m_hasCover[i])
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
				CoverDirections dir = (CoverDirections)i;
				float centerAngleOfDirection = GetCenterAngleOfDirection(dir);
				CoverRegion item = new CoverRegion(center, centerAngleOfDirection - num, centerAngleOfDirection + num);
				list.Add(item);
			}
		}
		if (list.Count != 0)
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
			if (list.Count != 1)
			{
				if (list.Count == 4)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
						{
							List<CoverRegion> list2 = new List<CoverRegion>();
							list2.Add(new CoverRegion(center, -720f, 720f));
							return list2;
						}
						}
					}
				}
				if (list.Count == 3)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							float num2 = list[0].m_startAngle;
							float num3 = list[0].m_endAngle;
							foreach (CoverRegion item2 in list)
							{
								num2 = Mathf.Min(num2, item2.m_startAngle);
								num3 = Mathf.Max(num3, item2.m_endAngle);
							}
							List<CoverRegion> list3 = new List<CoverRegion>();
							list3.Add(new CoverRegion(center, num2, num3));
							return list3;
						}
						}
					}
				}
				if (list.Count == 2)
				{
					CoverRegion coverRegion = list[0];
					CoverRegion coverRegion2 = list[1];
					int num4;
					if (coverRegion.m_startAngle <= coverRegion2.m_startAngle)
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
						num4 = ((coverRegion2.m_startAngle <= coverRegion.m_endAngle) ? 1 : 0);
					}
					else
					{
						num4 = 0;
					}
					bool flag = (byte)num4 != 0;
					bool flag2 = coverRegion.m_startAngle <= coverRegion2.m_endAngle && coverRegion2.m_endAngle <= coverRegion.m_endAngle;
					if (!flag)
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
						if (!flag2)
						{
							return list;
						}
					}
					float startAngle = Mathf.Min(coverRegion.m_startAngle, coverRegion2.m_startAngle);
					float endAngle = Mathf.Max(coverRegion.m_endAngle, coverRegion2.m_endAngle);
					List<CoverRegion> list4 = new List<CoverRegion>();
					list4.Add(new CoverRegion(center, startAngle, endAngle));
					return list4;
				}
				Log.Error("Actor " + owner.DisplayName + " in cover in " + list.Count + " directions.");
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
		foreach (CoverRegion item in coveredRegions)
		{
			bool flag2 = item.IsDirInCover(angle_deg);
			bool flag3 = item.IsDirInCover(angle_deg2);
			if (flag2)
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
				if (flag3)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							flag = true;
							goto end_IL_002a;
						}
					}
				}
			}
		}
		if (!flag)
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
			using (List<CoverRegion>.Enumerator enumerator2 = coveredRegions.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CoverRegion current2 = enumerator2.Current;
					if (current2.IsDirInCover(coneDirAngleDegrees))
					{
						bool flag4 = current2.IsDirInCover(angle_deg) && !current2.IsDirInCover(angle_deg2);
						int num2;
						if (!current2.IsDirInCover(angle_deg))
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
							num2 = (current2.IsDirInCover(angle_deg2) ? 1 : 0);
						}
						else
						{
							num2 = 0;
						}
						bool flag5 = (byte)num2 != 0;
						if (flag4)
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
							num = current2.m_endAngle - coneWidthDegrees / 2f;
						}
						else if (flag5)
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
							num = current2.m_startAngle + coneWidthDegrees / 2f;
						}
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		newDirAngleDegrees = num;
		newConeDir = VectorUtils.AngleDegreesToVector(num);
	}

	private static float GetCenterAngleOfDirection(CoverDirections dir)
	{
		switch (dir)
		{
		case CoverDirections.X_POS:
			return 0f;
		case CoverDirections.Y_POS:
			return 90f;
		case CoverDirections.X_NEG:
			return 180f;
		case CoverDirections.Y_NEG:
			return 270f;
		default:
			return 0f;
		}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("SyncList m_syncTempCoverProviders called on server.");
					return;
				}
			}
		}
		((ActorCover)obj).m_syncTempCoverProviders.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GeneratedNetworkCode._WriteStructSyncListTempCoverInfo_None(writer, m_syncTempCoverProviders);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
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
			GeneratedNetworkCode._WriteStructSyncListTempCoverInfo_None(writer, m_syncTempCoverProviders);
		}
		if (!flag)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
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
}
