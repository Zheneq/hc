using System.Collections.Generic;
using UnityEngine;

public class MouseoverCoverManager
{
	public class OperationOnSquare_CoverHighlight : IOperationOnSquare
	{
		private MouseoverCoverManager m_mouseoverCoverManager;

		private BoardSquare m_startSquare;

		private float m_radius = 3f;

		public OperationOnSquare_CoverHighlight(MouseoverCoverManager manager)
		{
			m_mouseoverCoverManager = manager;
		}

		public void SetStartSquare(BoardSquare startSquare)
		{
			m_startSquare = startSquare;
		}

		public void SetRadius(float radius)
		{
			m_radius = radius;
		}

		public void OperateOnSquare(BoardSquare currentSquare, ActorData actor, bool squareHasLos)
		{
			if (!squareHasLos)
			{
				return;
			}
			while (true)
			{
				if (!(m_startSquare != null))
				{
					return;
				}
				while (true)
				{
					if (!(currentSquare != m_startSquare))
					{
						return;
					}
					Vector3 from = currentSquare.ToVector3() - m_startSquare.ToVector3();
					from.y = 0f;
					List<BoardSquare> result = null;
					Board.Get().GetCardinalAdjacentSquares(currentSquare.x, currentSquare.y, ref result);
					if (result == null)
					{
						return;
					}
					while (true)
					{
						for (int i = 0; i < result.Count; i++)
						{
							BoardSquare boardSquare = result[i];
							if (!(boardSquare != null))
							{
								continue;
							}
							if (!AreaEffectUtils.IsSquareInConeByActorRadius(boardSquare, m_startSquare.ToVector3(), 0f, 360f, m_radius, 0f, true, actor))
							{
								continue;
							}
							Vector3 to = boardSquare.ToVector3() - currentSquare.ToVector3();
							to.y = 0f;
							float num = Vector3.Angle(from, to);
							ActorCover.CoverDirections coverDirection = ActorCover.GetCoverDirection(currentSquare, boardSquare);
							int num2 = boardSquare.height - currentSquare.height;
							bool flag = num2 >= 1;
							bool flag2 = currentSquare.GetThinCover(coverDirection) != ThinCover.CoverType.None;
							if (!flag)
							{
								if (!flag2)
								{
									continue;
								}
								if (!(num < 130f))
								{
									continue;
								}
							}
							GameObject nextCoverHighight = m_mouseoverCoverManager.GetNextCoverHighight();
							Vector3 position = new Vector3(currentSquare.worldX, (float)currentSquare.height + HighlightUtils.Get().m_mouseoverHeightOffset, currentSquare.worldY);
							position += ActorCover.GetCoverOffsetStatic(coverDirection);
							nextCoverHighight.transform.position = position;
							nextCoverHighight.transform.rotation = ActorCover.GetCoverRotation(coverDirection);
							if (!nextCoverHighight.activeSelf)
							{
								UIManager.SetGameObjectActive(nextCoverHighight, true);
							}
							ParticleSystemRenderer[] componentsInChildren = nextCoverHighight.GetComponentsInChildren<ParticleSystemRenderer>();
							foreach (ParticleSystemRenderer particleSystemRenderer in componentsInChildren)
							{
								AbilityUtil_Targeter.SetMaterialOpacity(particleSystemRenderer.materials, HighlightUtils.Get().m_mouseoverCoverIconAlpha);
							}
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
		}

		public bool ShouldEarlyOut()
		{
			return false;
		}
	}

	private GameObject m_parentObject;

	private List<GameObject> m_coverHighlights = new List<GameObject>();

	private int m_nextHighlightIndex;

	private bool m_initialized;

	private const int c_initialAllocation = 6;

	private OperationOnSquare_CoverHighlight m_coverHighlightOp;

	private BoardSquare m_lastUpdateSquare;

	private GameObject m_coverDirHighlight;

	private MeshRenderer[] m_coverDirIndicatorRenderers;

	private bool[] m_hasCover = new bool[4];

	private float m_tsCoverDirIndicatorShow = -1f;

	public void Initialize(GameObject parentObject)
	{
		if (m_initialized)
		{
			return;
		}
		m_parentObject = parentObject;
		if (HighlightUtils.Get() != null && HighlightUtils.Get().m_mouseoverCoverShieldPrefab != null && HighlightUtils.Get().m_showMouseoverCoverIndicators)
		{
			for (int i = 0; i < 6; i++)
			{
				GameObject gameObject = CreateCoverHighlightInstance();
				if (gameObject != null)
				{
					m_coverHighlights.Add(gameObject);
				}
			}
		}
		m_coverHighlightOp = new OperationOnSquare_CoverHighlight(this);
		m_initialized = true;
	}

	private GameObject CreateCoverHighlightInstance()
	{
		GameObject mouseoverCoverShieldPrefab = HighlightUtils.Get().m_mouseoverCoverShieldPrefab;
		if (mouseoverCoverShieldPrefab != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					GameObject gameObject = Object.Instantiate(mouseoverCoverShieldPrefab);
					gameObject.transform.parent = m_parentObject.transform;
					UIManager.SetGameObjectActive(gameObject, false);
					return gameObject;
				}
				}
			}
		}
		return null;
	}

	public void UpdateCoverAroundSquare(BoardSquare coverCenterSquare)
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (HighlightUtils.Get() == null)
			{
				return;
			}
			while (true)
			{
				if (!HighlightUtils.Get().m_showMouseoverCoverIndicators)
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
				if (!(actorData == null))
				{
					if (!(coverCenterSquare == null))
					{
						if (coverCenterSquare.IsValidForGameplay())
						{
							ActorTurnSM actorTurnSM = actorData.GetActorTurnSM();
							if (actorTurnSM != null)
							{
								if (!actorTurnSM.AmTargetingAction())
								{
									if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
									{
										if (m_lastUpdateSquare != coverCenterSquare)
										{
											m_nextHighlightIndex = 0;
											m_coverHighlightOp.SetStartSquare(coverCenterSquare);
											m_coverHighlightOp.SetRadius(HighlightUtils.Get().m_mouseoverCoverAreaRadius);
											AreaEffectUtils.OperateOnSquaresInCone(m_coverHighlightOp, coverCenterSquare.ToVector3(), 0f, 360f, HighlightUtils.Get().m_mouseoverCoverAreaRadius, 0f, actorData, true);
											HideCoverHighlights(m_nextHighlightIndex);
											DestroyCoverDirHighlight();
											ActorCover.CalcCoverLevelGeoOnly(out m_hasCover, coverCenterSquare);
											if (HasAnyCover())
											{
												m_tsCoverDirIndicatorShow = Time.time;
												Vector3 position = coverCenterSquare.ToVector3();
												position.y = HighlightUtils.GetHighlightHeight();
												HighlightUtils.CoverDirIndicatorParams mouseoverCoverDirParams = HighlightUtils.Get().m_mouseoverCoverDirParams;
												m_coverDirHighlight = ActorCover.CreateCoverDirIndicator(m_hasCover, mouseoverCoverDirParams.m_color, mouseoverCoverDirParams.m_radiusInSquares);
												m_coverDirHighlight.transform.position = position;
												m_coverDirIndicatorRenderers = m_coverDirHighlight.GetComponentsInChildren<MeshRenderer>();
											}
											m_lastUpdateSquare = coverCenterSquare;
										}
										goto IL_0219;
									}
								}
								HideCoverHighlights(0);
								m_lastUpdateSquare = null;
							}
							goto IL_0219;
						}
					}
				}
				HideCoverHighlights(0);
				m_lastUpdateSquare = null;
				goto IL_0219;
				IL_0219:
				if (!(m_coverDirHighlight != null))
				{
					return;
				}
				while (true)
				{
					if (m_coverDirIndicatorRenderers == null)
					{
						return;
					}
					while (true)
					{
						float timeSinceChange = Time.time - m_tsCoverDirIndicatorShow;
						float opacityFromTargeterData = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_mouseoverCoverDirParams.m_opacity, timeSinceChange);
						for (int i = 0; i < m_coverDirIndicatorRenderers.Length; i++)
						{
							MeshRenderer meshRenderer = m_coverDirIndicatorRenderers[i];
							if (meshRenderer != null)
							{
								AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, opacityFromTargeterData);
							}
						}
						return;
					}
				}
			}
		}
	}

	public bool HasAnyCover()
	{
		for (int i = 0; i < m_hasCover.Length; i++)
		{
			if (!m_hasCover[i])
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public GameObject GetNextCoverHighight()
	{
		GameObject gameObject = null;
		if (m_nextHighlightIndex < m_coverHighlights.Count)
		{
			gameObject = m_coverHighlights[m_nextHighlightIndex];
			m_nextHighlightIndex++;
		}
		else
		{
			GameObject gameObject2 = CreateCoverHighlightInstance();
			if (gameObject2 != null)
			{
				m_coverHighlights.Add(gameObject2);
			}
			m_nextHighlightIndex = m_coverHighlights.Count;
			gameObject = gameObject2;
		}
		return gameObject;
	}

	public void HideCoverHighlights(int fromIndex)
	{
		if (fromIndex < 0)
		{
			fromIndex = 0;
		}
		for (int i = fromIndex; i < m_coverHighlights.Count; i++)
		{
			if (!m_coverHighlights[i].activeSelf)
			{
				break;
			}
			UIManager.SetGameObjectActive(m_coverHighlights[i], false);
		}
		m_nextHighlightIndex = fromIndex;
		m_tsCoverDirIndicatorShow = -1f;
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
			HighlightUtils.DestroyObjectAndMaterials(m_coverDirHighlight);
			return;
		}
	}
}
