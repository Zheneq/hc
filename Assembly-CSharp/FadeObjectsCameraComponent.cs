using System;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectsCameraComponent : MonoBehaviour
{
	public Shader m_nonActorFadeShader;

	public float m_fadeTransparency = 0.4f;

	public float m_fadeTime = 0.72f;

	public float m_fadeTimeNoDepthSort;

	public float m_fadeHeight = 0.2f;

	public float m_fadeEndFloorOffset = 1.2f;

	public float m_cameraHeightForVerticalFade = 7f;

	public float m_collisionRadius = 0.5f;

	public bool m_enableMouseReveal;

	public float m_minAlphaTopDepth = 0.01f;

	public GameObject m_alwaysVisibleObjectInScene;

	public const float c_updateInterval = 0.25f;

	private float m_timeTillNextUpdate;

	private const float VERTICAL_CLEARANCE_CINEMATIC = 0.1f;

	private const float VERTICAL_CLEARANCE = 1f;

	private int m_verticalFadeHeightID;

	private int m_verticalFadeEndWorldYID;

	internal bool m_disableMouseReveal;

	private List<GameObject> m_desiredVisibleObjects = new List<GameObject>();

	private bool m_mouseCursorInDesiredVisibleObjects;

	private bool m_markedForResetVisibleObjects;

	private RaycastHit[] m_sphereCastHitsBuffer = new RaycastHit[0x19];

	private static FadeObjectsCameraComponent s_instance;

	private const float c_debugForceAllVertOffset = -75f;

	private const float c_debugForceAllRadius = 150f;

	private bool m_forceFadeAll;

	internal bool IsMouseRevealEnabled()
	{
		bool result;
		if (this.m_enableMouseReveal)
		{
			result = !this.m_disableMouseReveal;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void MarkForResetVisibleObjects()
	{
		this.m_markedForResetVisibleObjects = true;
	}

	internal static FadeObjectsCameraComponent Get()
	{
		return FadeObjectsCameraComponent.s_instance;
	}

	private void Awake()
	{
		FadeObjectsCameraComponent.s_instance = this;
		base.enabled = !GameManager.IsEditorAndNotGame();
	}

	private void Start()
	{
		this.m_verticalFadeHeightID = Shader.PropertyToID("_VerticalFadeHeight");
		this.m_verticalFadeEndWorldYID = Shader.PropertyToID("_VerticalFadeEndWorldY");
		this.SetGlobalFloatsForShaders();
	}

	private void OnDestroy()
	{
		FadeObjectsCameraComponent.s_instance = null;
	}

	private void SetGlobalFloatsForShaders()
	{
		Shader.SetGlobalFloat(this.m_verticalFadeHeightID, this.m_fadeHeight);
		if (Board.Get() != null)
		{
			Shader.SetGlobalFloat(this.m_verticalFadeEndWorldYID, (float)Board.Get().BaselineHeight + this.m_fadeEndFloorOffset);
		}
	}

	public void AddDesiredVisibleObject(GameObject obj)
	{
		this.m_desiredVisibleObjects.Add(obj);
		this.m_timeTillNextUpdate = 0f;
	}

	public void ClearDesiredVisibleObjects()
	{
		this.m_desiredVisibleObjects.Clear();
		this.m_timeTillNextUpdate = 0f;
	}

	public void ResetDesiredVisibleObjects()
	{
		this.m_desiredVisibleObjects.Clear();
		if (this.IsMouseRevealEnabled())
		{
			if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Done)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					ActorTurnSM actorTurnSM = GameFlowData.Get().activeOwnedActorData.GetActorTurnSM();
					if (actorTurnSM.AmTargetingAction())
					{
						if (actorTurnSM.GetSelectedTargetingParadigm() != Ability.TargetingParadigm.BoardSquare)
						{
							goto IL_1A6;
						}
					}
					this.m_desiredVisibleObjects.Add(HighlightUtils.Get().ClampedMouseOverCursor);
					this.m_desiredVisibleObjects.Add(HighlightUtils.Get().FreeMouseOverCursor);
					this.m_desiredVisibleObjects.Add(HighlightUtils.Get().CornerMouseOverCursor);
					this.m_desiredVisibleObjects.Add(HighlightUtils.Get().MovementMouseOverCursor);
					this.m_desiredVisibleObjects.Add(HighlightUtils.Get().AbilityTargetMouseOverCursor);
					this.m_desiredVisibleObjects.Add(HighlightUtils.Get().IdleMouseOverCursor);
					this.m_desiredVisibleObjects.Add(HighlightUtils.Get().SprintMouseOverCursor);
					if (HighlightUtils.Get().ChaseSquareCursor != null)
					{
						this.m_desiredVisibleObjects.Add(HighlightUtils.Get().ChaseSquareCursor);
					}
					if (HighlightUtils.Get().ChaseSquareCursorAlt != null)
					{
						this.m_desiredVisibleObjects.Add(HighlightUtils.Get().ChaseSquareCursorAlt);
					}
				}
			}
		}
		IL_1A6:
		this.m_mouseCursorInDesiredVisibleObjects = (this.m_desiredVisibleObjects.Count > 0);
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
				{
					if (actorData.IsVisibleToClient())
					{
						if (!actorData.IsDead())
						{
							if (actorData.CurrentBoardSquare != null)
							{
								this.m_desiredVisibleObjects.Add(actorData.gameObject);
							}
						}
					}
				}
			}
		}
		if (this.m_desiredVisibleObjects.Count == 0)
		{
			if (GameFlowData.Get() != null)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
				{
					if (activeOwnedActorData.IsDead())
					{
						if (activeOwnedActorData.RespawnPickedPositionSquare != null)
						{
							this.m_desiredVisibleObjects.Add(activeOwnedActorData.RespawnPickedPositionSquare.gameObject);
						}
						else if (!activeOwnedActorData.respawnSquares.IsNullOrEmpty<BoardSquare>())
						{
							this.m_desiredVisibleObjects.Add(activeOwnedActorData.respawnSquares[0].gameObject);
						}
					}
				}
			}
		}
		if (this.m_desiredVisibleObjects.Count == 0)
		{
			if (this.m_alwaysVisibleObjectInScene != null)
			{
				this.m_desiredVisibleObjects.Add(this.m_alwaysVisibleObjectInScene);
			}
		}
		this.m_markedForResetVisibleObjects = false;
	}

	private float GetFadeTime()
	{
		bool flag = CameraManager.Get().InCinematic();
		float result;
		if (flag)
		{
			result = 0f;
		}
		else if (DepthTextureRenderer.Instance != null && DepthTextureRenderer.Instance.IsFunctioning())
		{
			result = this.m_fadeTime;
		}
		else
		{
			result = this.m_fadeTimeNoDepthSort;
		}
		return result;
	}

	private void RevealPosition(Vector3 position)
	{
		bool flag = CameraManager.Get().InCinematic();
		bool forceFadeAll = this.ForceFadeAll;
		float num;
		if (flag)
		{
			num = 0.1f;
		}
		else
		{
			num = 1f;
		}
		float num2 = num;
		float num3 = this.m_collisionRadius;
		if (forceFadeAll)
		{
			num2 = -75f;
			num3 = 150f;
		}
		Vector3 vector = position;
		vector.y += num2 + num3;
		Vector3 vector2 = base.transform.position - vector;
		float magnitude = vector2.magnitude;
		if (magnitude == 0f)
		{
			return;
		}
		if (forceFadeAll)
		{
			vector2.y = 0f;
		}
		vector2 /= magnitude;
		int num4 = Physics.SphereCastNonAlloc(vector, num3, vector2, this.m_sphereCastHitsBuffer, magnitude, Camera.main.cullingMask);
		for (int i = 0; i < num4; i++)
		{
			RaycastHit raycastHit = this.m_sphereCastHitsBuffer[i];
			if (Vector3.Dot(raycastHit.point - vector, base.transform.position - vector) > 0f)
			{
				Renderer renderer = raycastHit.collider.GetComponent<Renderer>();
				ActorModelData actorModelData = null;
				GameObject gameObject = null;
				if (raycastHit.collider.gameObject.layer == ActorData.Layer)
				{
					if (!flag)
					{
						goto IL_57A;
					}
					gameObject = GameFlowData.FindParentBelowRoot(raycastHit.collider.gameObject);
					actorModelData = gameObject.GetComponent<ActorData>().GetActorModelData();
				}
				bool flag2 = false;
				if (gameObject != null)
				{
					using (List<GameObject>.Enumerator enumerator = this.m_desiredVisibleObjects.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameObject gameObject2 = enumerator.Current;
							if (gameObject2 != null)
							{
								if (GameFlowData.FindParentBelowRoot(gameObject2) == gameObject)
								{
									flag2 = true;
									goto IL_240;
								}
							}
						}
					}
				}
				IL_240:
				if (!(renderer != null))
				{
					goto IL_27D;
				}
				if (!this.m_desiredVisibleObjects.Contains(raycastHit.collider.gameObject))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_27D;
					}
				}
				goto IL_57A;
				IL_27D:
				if (!(actorModelData != null) || !flag2)
				{
					FadeObjectGroup component = raycastHit.collider.GetComponent<FadeObjectGroup>();
					if (renderer == null)
					{
						if (actorModelData == null)
						{
							if (component == null)
							{
								goto IL_57A;
							}
						}
					}
					if (flag)
					{
						goto IL_303;
					}
					float num5;
					if (forceFadeAll)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							goto IL_303;
						}
					}
					else
					{
						num5 = this.m_fadeTransparency;
					}
					IL_310:
					float num6 = num5;
					float fadeTime = this.GetFadeTime();
					float fadeStartDelayDuration = (!flag) ? 0.3f : 0f;
					if (actorModelData != null)
					{
						actorModelData.SetCameraTransparency(num6, fadeTime, fadeStartDelayDuration);
						goto IL_57A;
					}
					if (component != null)
					{
						bool flag3;
						if (!component.ShouldProcessEvenIfRendererIsDisabled())
						{
							flag3 = component.AreRenderersEnabled();
						}
						else
						{
							flag3 = true;
						}
						bool flag4 = flag3;
						if (flag4)
						{
							if (component.gameObject.activeInHierarchy)
							{
								component.SetTargetTransparency(num6, fadeTime, fadeTime, this.m_nonActorFadeShader);
							}
						}
						goto IL_57A;
					}
					FadeObject fadeObject = renderer.GetComponent<FadeObject>();
					if (!(fadeObject != null))
					{
						goto IL_3EC;
					}
					if (!fadeObject.ShouldProcessEvenIfRendererIsDisabled())
					{
						goto IL_3EC;
					}
					bool flag5 = true;
					IL_3F6:
					bool flag6 = flag5;
					if (!flag6)
					{
						goto IL_57A;
					}
					if (!renderer.gameObject.activeInHierarchy)
					{
						goto IL_57A;
					}
					if (!(renderer.material != null))
					{
						goto IL_57A;
					}
					if (fadeObject == null)
					{
						fadeObject = renderer.gameObject.AddComponent<FadeObject>();
					}
					fadeObject.SetTargetTransparency(num6, fadeTime, fadeTime, this.m_nonActorFadeShader);
					Renderer[] componentsInChildren = renderer.gameObject.GetComponentsInChildren<Renderer>();
					if (componentsInChildren != null)
					{
						int j = 0;
						while (j < componentsInChildren.Length)
						{
							renderer = componentsInChildren[j];
							fadeObject = renderer.GetComponent<FadeObject>();
							if (!(fadeObject != null))
							{
								goto IL_4CD;
							}
							if (!fadeObject.ShouldProcessEvenIfRendererIsDisabled())
							{
								goto IL_4CD;
							}
							bool flag7 = true;
							IL_4D9:
							flag6 = flag7;
							if (flag6)
							{
								if (renderer.gameObject.activeInHierarchy)
								{
									if (renderer.material != null)
									{
										fadeObject = renderer.GetComponent<FadeObject>();
										if (fadeObject == null)
										{
											fadeObject = renderer.gameObject.AddComponent<FadeObject>();
										}
										fadeObject.SetTargetTransparency(num6, fadeTime, fadeTime, this.m_nonActorFadeShader);
									}
								}
							}
							j++;
							continue;
							IL_4CD:
							flag7 = renderer.enabled;
							goto IL_4D9;
						}
						goto IL_57A;
					}
					goto IL_57A;
					IL_3EC:
					flag5 = renderer.enabled;
					goto IL_3F6;
					IL_303:
					num5 = 0f;
					goto IL_310;
				}
			}
			IL_57A:;
		}
	}

	private void Update()
	{
		if (!(CameraManager.Get() == null))
		{
			if (GameFlowData.Get() != null)
			{
				if (!GameFlowData.Get().Started)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return;
					}
				}
			}
			if (this.m_markedForResetVisibleObjects)
			{
				this.ResetDesiredVisibleObjects();
			}
			bool flag = CameraManager.Get().InCinematic();
			if (flag)
			{
				this.m_timeTillNextUpdate = 0f;
			}
			else
			{
				this.m_timeTillNextUpdate -= Time.unscaledDeltaTime;
				if (this.m_timeTillNextUpdate > 0f)
				{
					return;
				}
				this.m_timeTillNextUpdate = 0.25f;
			}
			for (int i = 0; i < this.m_desiredVisibleObjects.Count; i++)
			{
				GameObject gameObject = this.m_desiredVisibleObjects[i];
				if (gameObject != null)
				{
					this.RevealPosition(gameObject.transform.position);
					if (this.ForceFadeAll)
					{
						break;
					}
				}
			}
			if (this.IsMouseRevealEnabled() && !this.m_mouseCursorInDesiredVisibleObjects)
			{
				this.RevealPosition(Board.Get().PlayerFreePos);
			}
			return;
		}
	}

	public bool ForceFadeAll
	{
		get
		{
			return false;
		}
		set
		{
			this.m_forceFadeAll = value;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		if (Board.Get() != null)
		{
			bool flag;
			if (CameraManager.Get() != null)
			{
				flag = CameraManager.Get().InCinematic();
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			bool forceFadeAll = this.ForceFadeAll;
			float num;
			if (flag2)
			{
				num = 0.1f;
			}
			else
			{
				num = 1f;
			}
			float num2 = num;
			float num3 = this.m_collisionRadius;
			if (forceFadeAll)
			{
				num2 = -75f;
				num3 = 150f;
			}
			Gizmos.color = Color.yellow;
			foreach (GameObject gameObject in this.m_desiredVisibleObjects)
			{
				if (gameObject != null)
				{
					Vector3 position = gameObject.transform.position;
					position.y += num3 + num2;
					Gizmos.DrawWireSphere(position, num3);
				}
			}
			if (this.IsMouseRevealEnabled())
			{
				if (!this.m_mouseCursorInDesiredVisibleObjects)
				{
					Vector3 playerFreePos = Board.Get().PlayerFreePos;
					playerFreePos.y += num3 + num2;
					Gizmos.DrawWireSphere(playerFreePos, num3);
				}
			}
		}
	}
}
