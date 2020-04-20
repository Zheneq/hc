using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseoverCoverManager
{
	private GameObject m_parentObject;

	private List<GameObject> m_coverHighlights = new List<GameObject>();

	private int m_nextHighlightIndex;

	private bool m_initialized;

	private const int c_initialAllocation = 6;

	private MouseoverCoverManager.OperationOnSquare_CoverHighlight m_coverHighlightOp;

	private BoardSquare m_lastUpdateSquare;

	private GameObject m_coverDirHighlight;

	private MeshRenderer[] m_coverDirIndicatorRenderers;

	private bool[] m_hasCover = new bool[4];

	private float m_tsCoverDirIndicatorShow = -1f;

	public void Initialize(GameObject parentObject)
	{
		if (!this.m_initialized)
		{
			this.m_parentObject = parentObject;
			if (HighlightUtils.Get() != null && HighlightUtils.Get().m_mouseoverCoverShieldPrefab != null && HighlightUtils.Get().m_showMouseoverCoverIndicators)
			{
				for (int i = 0; i < 6; i++)
				{
					GameObject gameObject = this.CreateCoverHighlightInstance();
					if (gameObject != null)
					{
						this.m_coverHighlights.Add(gameObject);
					}
				}
			}
			this.m_coverHighlightOp = new MouseoverCoverManager.OperationOnSquare_CoverHighlight(this);
			this.m_initialized = true;
		}
	}

	private GameObject CreateCoverHighlightInstance()
	{
		GameObject mouseoverCoverShieldPrefab = HighlightUtils.Get().m_mouseoverCoverShieldPrefab;
		if (mouseoverCoverShieldPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(mouseoverCoverShieldPrefab);
			gameObject.transform.parent = this.m_parentObject.transform;
			UIManager.SetGameObjectActive(gameObject, false, null);
			return gameObject;
		}
		return null;
	}

	public void UpdateCoverAroundSquare(BoardSquare coverCenterSquare)
	{
		if (this.m_initialized)
		{
			if (!(HighlightUtils.Get() == null))
			{
				if (HighlightUtils.Get().m_showMouseoverCoverIndicators)
				{
					ActorData actorData;
					if (GameFlowData.Get() != null)
					{
						actorData = GameFlowData.Get().activeOwnedActorData;
					}
					else
					{
						actorData = null;
					}
					ActorData actorData2 = actorData;
					if (!(actorData2 == null))
					{
						if (!(coverCenterSquare == null))
						{
							if (coverCenterSquare.IsBaselineHeight())
							{
								ActorTurnSM actorTurnSM = actorData2.GetActorTurnSM();
								if (actorTurnSM != null)
								{
									if (!actorTurnSM.AmTargetingAction())
									{
										if (actorTurnSM.CurrentState != TurnStateEnum.DECIDING)
										{
										}
										else
										{
											if (this.m_lastUpdateSquare != coverCenterSquare)
											{
												this.m_nextHighlightIndex = 0;
												this.m_coverHighlightOp.SetStartSquare(coverCenterSquare);
												this.m_coverHighlightOp.SetRadius(HighlightUtils.Get().m_mouseoverCoverAreaRadius);
												AreaEffectUtils.OperateOnSquaresInCone(this.m_coverHighlightOp, coverCenterSquare.ToVector3(), 0f, 360f, HighlightUtils.Get().m_mouseoverCoverAreaRadius, 0f, actorData2, true, null);
												this.HideCoverHighlights(this.m_nextHighlightIndex);
												this.DestroyCoverDirHighlight();
												ActorCover.CalcCoverLevelGeoOnly(out this.m_hasCover, coverCenterSquare);
												if (this.HasAnyCover())
												{
													this.m_tsCoverDirIndicatorShow = Time.time;
													Vector3 position = coverCenterSquare.ToVector3();
													position.y = HighlightUtils.GetHighlightHeight();
													HighlightUtils.CoverDirIndicatorParams mouseoverCoverDirParams = HighlightUtils.Get().m_mouseoverCoverDirParams;
													this.m_coverDirHighlight = ActorCover.CreateCoverDirIndicator(this.m_hasCover, mouseoverCoverDirParams.m_color, mouseoverCoverDirParams.m_radiusInSquares);
													this.m_coverDirHighlight.transform.position = position;
													this.m_coverDirIndicatorRenderers = this.m_coverDirHighlight.GetComponentsInChildren<MeshRenderer>();
												}
												this.m_lastUpdateSquare = coverCenterSquare;
												goto IL_219;
											}
											goto IL_219;
										}
									}
									this.HideCoverHighlights(0);
									this.m_lastUpdateSquare = null;
									goto IL_219;
								}
								goto IL_219;
							}
						}
					}
					this.HideCoverHighlights(0);
					this.m_lastUpdateSquare = null;
					IL_219:
					if (this.m_coverDirHighlight != null)
					{
						if (this.m_coverDirIndicatorRenderers != null)
						{
							float timeSinceChange = Time.time - this.m_tsCoverDirIndicatorShow;
							float opacityFromTargeterData = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_mouseoverCoverDirParams.m_opacity, timeSinceChange);
							for (int i = 0; i < this.m_coverDirIndicatorRenderers.Length; i++)
							{
								MeshRenderer meshRenderer = this.m_coverDirIndicatorRenderers[i];
								if (meshRenderer != null)
								{
									AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, opacityFromTargeterData);
								}
							}
						}
					}
					return;
				}
			}
		}
	}

	public bool HasAnyCover()
	{
		for (int i = 0; i < this.m_hasCover.Length; i++)
		{
			if (this.m_hasCover[i])
			{
				return true;
			}
		}
		return false;
	}

	public GameObject GetNextCoverHighight()
	{
		GameObject result;
		if (this.m_nextHighlightIndex < this.m_coverHighlights.Count)
		{
			result = this.m_coverHighlights[this.m_nextHighlightIndex];
			this.m_nextHighlightIndex++;
		}
		else
		{
			GameObject gameObject = this.CreateCoverHighlightInstance();
			if (gameObject != null)
			{
				this.m_coverHighlights.Add(gameObject);
			}
			this.m_nextHighlightIndex = this.m_coverHighlights.Count;
			result = gameObject;
		}
		return result;
	}

	public void HideCoverHighlights(int fromIndex)
	{
		if (fromIndex < 0)
		{
			fromIndex = 0;
		}
		for (int i = fromIndex; i < this.m_coverHighlights.Count; i++)
		{
			if (!this.m_coverHighlights[i].activeSelf)
			{
				break;
			}
			UIManager.SetGameObjectActive(this.m_coverHighlights[i], false, null);
		}
		this.m_nextHighlightIndex = fromIndex;
		this.m_tsCoverDirIndicatorShow = -1f;
	}

	private void DestroyCoverDirHighlight()
	{
		this.m_coverDirIndicatorRenderers = null;
		if (this.m_coverDirHighlight != null)
		{
			HighlightUtils.DestroyObjectAndMaterials(this.m_coverDirHighlight);
		}
	}

	public class OperationOnSquare_CoverHighlight : IOperationOnSquare
	{
		private MouseoverCoverManager m_mouseoverCoverManager;

		private BoardSquare m_startSquare;

		private float m_radius = 3f;

		public OperationOnSquare_CoverHighlight(MouseoverCoverManager manager)
		{
			this.m_mouseoverCoverManager = manager;
		}

		public void SetStartSquare(BoardSquare startSquare)
		{
			this.m_startSquare = startSquare;
		}

		public void SetRadius(float radius)
		{
			this.m_radius = radius;
		}

		public void OperateOnSquare(BoardSquare currentSquare, ActorData actor, bool squareHasLos)
		{
			if (squareHasLos)
			{
				if (this.m_startSquare != null)
				{
					if (currentSquare != this.m_startSquare)
					{
						Vector3 from = currentSquare.ToVector3() - this.m_startSquare.ToVector3();
						from.y = 0f;
						List<BoardSquare> list = null;
						Board.Get().GetStraightAdjacentSquares(currentSquare.x, currentSquare.y, ref list);
						if (list != null)
						{
							for (int i = 0; i < list.Count; i++)
							{
								BoardSquare boardSquare = list[i];
								if (boardSquare != null)
								{
									if (!AreaEffectUtils.IsSquareInConeByActorRadius(boardSquare, this.m_startSquare.ToVector3(), 0f, 360f, this.m_radius, 0f, true, actor, false, default(Vector3)))
									{
									}
									else
									{
										Vector3 to = boardSquare.ToVector3() - currentSquare.ToVector3();
										to.y = 0f;
										float num = Vector3.Angle(from, to);
										ActorCover.CoverDirections coverDirection = ActorCover.GetCoverDirection(currentSquare, boardSquare);
										int num2 = boardSquare.height - currentSquare.height;
										bool flag = num2 >= 1;
										bool flag2 = currentSquare.GetCoverInDirection(coverDirection) != ThinCover.CoverType.None;
										if (!flag)
										{
											if (!flag2)
											{
												goto IL_273;
											}
											if (num >= 130f)
											{
												goto IL_273;
											}
										}
										GameObject nextCoverHighight = this.m_mouseoverCoverManager.GetNextCoverHighight();
										Vector3 vector = new Vector3(currentSquare.worldX, (float)currentSquare.height + HighlightUtils.Get().m_mouseoverHeightOffset, currentSquare.worldY);
										vector += ActorCover.GetCoverOffsetStatic(coverDirection);
										nextCoverHighight.transform.position = vector;
										nextCoverHighight.transform.rotation = ActorCover.GetCoverRotation(coverDirection);
										if (!nextCoverHighight.activeSelf)
										{
											UIManager.SetGameObjectActive(nextCoverHighight, true, null);
										}
										foreach (ParticleSystemRenderer particleSystemRenderer in nextCoverHighight.GetComponentsInChildren<ParticleSystemRenderer>())
										{
											AbilityUtil_Targeter.SetMaterialOpacity(particleSystemRenderer.materials, HighlightUtils.Get().m_mouseoverCoverIconAlpha);
										}
									}
								}
								IL_273:;
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
}
