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
							RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.Initialize(GameObject)).MethodHandle;
						}
						this.m_coverHighlights.Add(gameObject);
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.CreateCoverHighlightInstance()).MethodHandle;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.UpdateCoverAroundSquare(BoardSquare)).MethodHandle;
			}
			if (!(HighlightUtils.Get() == null))
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
				if (HighlightUtils.Get().m_showMouseoverCoverIndicators)
				{
					ActorData actorData;
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
						actorData = GameFlowData.Get().activeOwnedActorData;
					}
					else
					{
						actorData = null;
					}
					ActorData actorData2 = actorData;
					if (!(actorData2 == null))
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
						if (!(coverCenterSquare == null))
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
							if (coverCenterSquare.\u0016())
							{
								ActorTurnSM actorTurnSM = actorData2.\u000E();
								if (actorTurnSM != null)
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
									if (!actorTurnSM.AmTargetingAction())
									{
										if (actorTurnSM.CurrentState != TurnStateEnum.DECIDING)
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_coverDirIndicatorRenderers != null)
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
				for (;;)
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
	}

	public bool HasAnyCover()
	{
		for (int i = 0; i < this.m_hasCover.Length; i++)
		{
			if (this.m_hasCover[i])
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.HasAnyCover()).MethodHandle;
				}
				return true;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.GetNextCoverHighight()).MethodHandle;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.HideCoverHighlights(int)).MethodHandle;
			}
			fromIndex = 0;
		}
		for (int i = fromIndex; i < this.m_coverHighlights.Count; i++)
		{
			if (!this.m_coverHighlights[i].activeSelf)
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.DestroyCoverDirHighlight()).MethodHandle;
			}
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(MouseoverCoverManager.OperationOnSquare_CoverHighlight.OperateOnSquare(BoardSquare, ActorData, bool)).MethodHandle;
				}
				if (this.m_startSquare != null)
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
					if (currentSquare != this.m_startSquare)
					{
						Vector3 from = currentSquare.ToVector3() - this.m_startSquare.ToVector3();
						from.y = 0f;
						List<BoardSquare> list = null;
						Board.\u000E().\u000E(currentSquare.x, currentSquare.y, ref list);
						if (list != null)
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
							for (int i = 0; i < list.Count; i++)
							{
								BoardSquare boardSquare = list[i];
								if (boardSquare != null)
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
									if (!AreaEffectUtils.IsSquareInConeByActorRadius(boardSquare, this.m_startSquare.ToVector3(), 0f, 360f, this.m_radius, 0f, true, actor, false, default(Vector3)))
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
									}
									else
									{
										Vector3 to = boardSquare.ToVector3() - currentSquare.ToVector3();
										to.y = 0f;
										float num = Vector3.Angle(from, to);
										ActorCover.CoverDirections coverDirection = ActorCover.GetCoverDirection(currentSquare, boardSquare);
										int num2 = boardSquare.height - currentSquare.height;
										bool flag = num2 >= 1;
										bool flag2 = currentSquare.\u001D(coverDirection) != ThinCover.CoverType.None;
										if (!flag)
										{
											if (!flag2)
											{
												goto IL_273;
											}
											for (;;)
											{
												switch (2)
												{
												case 0:
													continue;
												}
												break;
											}
											if (num >= 130f)
											{
												goto IL_273;
											}
											for (;;)
											{
												switch (4)
												{
												case 0:
													continue;
												}
												break;
											}
										}
										GameObject nextCoverHighight = this.m_mouseoverCoverManager.GetNextCoverHighight();
										Vector3 vector = new Vector3(currentSquare.worldX, (float)currentSquare.height + HighlightUtils.Get().m_mouseoverHeightOffset, currentSquare.worldY);
										vector += ActorCover.GetCoverOffsetStatic(coverDirection);
										nextCoverHighight.transform.position = vector;
										nextCoverHighight.transform.rotation = ActorCover.GetCoverRotation(coverDirection);
										if (!nextCoverHighight.activeSelf)
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
											UIManager.SetGameObjectActive(nextCoverHighight, true, null);
										}
										foreach (ParticleSystemRenderer particleSystemRenderer in nextCoverHighight.GetComponentsInChildren<ParticleSystemRenderer>())
										{
											AbilityUtil_Targeter.SetMaterialOpacity(particleSystemRenderer.materials, HighlightUtils.Get().m_mouseoverCoverIconAlpha);
										}
										for (;;)
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
								IL_273:;
							}
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
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
}
