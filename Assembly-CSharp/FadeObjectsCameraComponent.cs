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

	private RaycastHit[] m_sphereCastHitsBuffer = new RaycastHit[25];

	private static FadeObjectsCameraComponent s_instance;

	private const float c_debugForceAllVertOffset = -75f;

	private const float c_debugForceAllRadius = 150f;

	private bool m_forceFadeAll;

	public bool ForceFadeAll
	{
		get
		{
			return false;
		}
		set
		{
			m_forceFadeAll = value;
		}
	}

	internal bool IsMouseRevealEnabled()
	{
		int result;
		if (m_enableMouseReveal)
		{
			result = ((!m_disableMouseReveal) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void MarkForResetVisibleObjects()
	{
		m_markedForResetVisibleObjects = true;
	}

	internal static FadeObjectsCameraComponent Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		base.enabled = !GameManager.IsEditorAndNotGame();
	}

	private void Start()
	{
		m_verticalFadeHeightID = Shader.PropertyToID("_VerticalFadeHeight");
		m_verticalFadeEndWorldYID = Shader.PropertyToID("_VerticalFadeEndWorldY");
		SetGlobalFloatsForShaders();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void SetGlobalFloatsForShaders()
	{
		Shader.SetGlobalFloat(m_verticalFadeHeightID, m_fadeHeight);
		if (!(Board.Get() != null))
		{
			return;
		}
		while (true)
		{
			Shader.SetGlobalFloat(m_verticalFadeEndWorldYID, (float)Board.Get().BaselineHeight + m_fadeEndFloorOffset);
			return;
		}
	}

	public void AddDesiredVisibleObject(GameObject obj)
	{
		m_desiredVisibleObjects.Add(obj);
		m_timeTillNextUpdate = 0f;
	}

	public void ClearDesiredVisibleObjects()
	{
		m_desiredVisibleObjects.Clear();
		m_timeTillNextUpdate = 0f;
	}

	public void ResetDesiredVisibleObjects()
	{
		m_desiredVisibleObjects.Clear();
		if (IsMouseRevealEnabled())
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
							goto IL_01a6;
						}
					}
					m_desiredVisibleObjects.Add(HighlightUtils.Get().ClampedMouseOverCursor);
					m_desiredVisibleObjects.Add(HighlightUtils.Get().FreeMouseOverCursor);
					m_desiredVisibleObjects.Add(HighlightUtils.Get().CornerMouseOverCursor);
					m_desiredVisibleObjects.Add(HighlightUtils.Get().MovementMouseOverCursor);
					m_desiredVisibleObjects.Add(HighlightUtils.Get().AbilityTargetMouseOverCursor);
					m_desiredVisibleObjects.Add(HighlightUtils.Get().IdleMouseOverCursor);
					m_desiredVisibleObjects.Add(HighlightUtils.Get().SprintMouseOverCursor);
					if (HighlightUtils.Get().ChaseSquareCursor != null)
					{
						m_desiredVisibleObjects.Add(HighlightUtils.Get().ChaseSquareCursor);
					}
					if (HighlightUtils.Get().ChaseSquareCursorAlt != null)
					{
						m_desiredVisibleObjects.Add(HighlightUtils.Get().ChaseSquareCursorAlt);
					}
				}
			}
		}
		goto IL_01a6;
		IL_01a6:
		m_mouseCursorInDesiredVisibleObjects = (m_desiredVisibleObjects.Count > 0);
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null)
				{
					if (current.IsVisibleToClient())
					{
						if (!current.IsDead())
						{
							if (current.CurrentBoardSquare != null)
							{
								m_desiredVisibleObjects.Add(current.gameObject);
							}
						}
					}
				}
			}
		}
		if (m_desiredVisibleObjects.Count == 0)
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
							m_desiredVisibleObjects.Add(activeOwnedActorData.RespawnPickedPositionSquare.gameObject);
						}
						else if (!activeOwnedActorData.respawnSquares.IsNullOrEmpty())
						{
							m_desiredVisibleObjects.Add(activeOwnedActorData.respawnSquares[0].gameObject);
						}
					}
				}
			}
		}
		if (m_desiredVisibleObjects.Count == 0)
		{
			if (m_alwaysVisibleObjectInScene != null)
			{
				m_desiredVisibleObjects.Add(m_alwaysVisibleObjectInScene);
			}
		}
		m_markedForResetVisibleObjects = false;
	}

	private float GetFadeTime()
	{
		float result;
		if (CameraManager.Get().InCinematic())
		{
			result = 0f;
		}
		else if (DepthTextureRenderer.Instance != null && DepthTextureRenderer.Instance.IsFunctioning())
		{
			result = m_fadeTime;
		}
		else
		{
			result = m_fadeTimeNoDepthSort;
		}
		return result;
	}

	private void RevealPosition(Vector3 position)
	{
		bool flag = CameraManager.Get().InCinematic();
		bool forceFadeAll = ForceFadeAll;
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
		float num3 = m_collisionRadius;
		if (forceFadeAll)
		{
			num2 = -75f;
			num3 = 150f;
		}
		Vector3 vector = position;
		vector.y += num2 + num3;
		Vector3 direction = base.transform.position - vector;
		float magnitude = direction.magnitude;
		if (magnitude == 0f)
		{
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
		if (forceFadeAll)
		{
			direction.y = 0f;
		}
		direction /= magnitude;
		int num4 = Physics.SphereCastNonAlloc(vector, num3, direction, m_sphereCastHitsBuffer, magnitude, Camera.main.cullingMask);
		for (int i = 0; i < num4; i++)
		{
			RaycastHit raycastHit = m_sphereCastHitsBuffer[i];
			if (!(Vector3.Dot(raycastHit.point - vector, base.transform.position - vector) > 0f))
			{
				continue;
			}
			Renderer component = raycastHit.collider.GetComponent<Renderer>();
			ActorModelData actorModelData = null;
			GameObject gameObject = null;
			if (raycastHit.collider.gameObject.layer == ActorData.Layer)
			{
				if (!flag)
				{
					continue;
				}
				gameObject = GameFlowData.FindParentBelowRoot(raycastHit.collider.gameObject);
				actorModelData = gameObject.GetComponent<ActorData>().GetActorModelData();
			}
			bool flag2 = false;
			if (gameObject != null)
			{
				using (List<GameObject>.Enumerator enumerator = m_desiredVisibleObjects.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
						{
							break;
						}
						GameObject current = enumerator.Current;
						if (current != null)
						{
							if (GameFlowData.FindParentBelowRoot(current) == gameObject)
							{
								flag2 = true;
								break;
							}
						}
					}
				}
			}
			if (component != null)
			{
				if (m_desiredVisibleObjects.Contains(raycastHit.collider.gameObject))
				{
					continue;
				}
			}
			if (actorModelData != null && flag2)
			{
				continue;
			}
			FadeObjectGroup component2 = raycastHit.collider.GetComponent<FadeObjectGroup>();
			if (component == null)
			{
				if (actorModelData == null)
				{
					if (component2 == null)
					{
						continue;
					}
				}
			}
			float num5;
			if (!flag)
			{
				if (!forceFadeAll)
				{
					num5 = m_fadeTransparency;
					goto IL_0310;
				}
			}
			num5 = 0f;
			goto IL_0310;
			IL_0310:
			float num6 = num5;
			float fadeTime = GetFadeTime();
			float fadeStartDelayDuration = (!flag) ? 0.3f : 0f;
			if (actorModelData != null)
			{
				actorModelData.SetCameraTransparency(num6, fadeTime, fadeStartDelayDuration);
				continue;
			}
			if (component2 != null)
			{
				int num7;
				if (!component2.ShouldProcessEvenIfRendererIsDisabled())
				{
					num7 = (component2.AreRenderersEnabled() ? 1 : 0);
				}
				else
				{
					num7 = 1;
				}
				if (num7 == 0)
				{
					continue;
				}
				if (component2.gameObject.activeInHierarchy)
				{
					component2.SetTargetTransparency(num6, fadeTime, fadeTime, m_nonActorFadeShader);
				}
				continue;
			}
			FadeObject fadeObject = component.GetComponent<FadeObject>();
			int num8;
			if (fadeObject != null)
			{
				if (fadeObject.ShouldProcessEvenIfRendererIsDisabled())
				{
					num8 = 1;
					goto IL_03f6;
				}
			}
			num8 = (component.enabled ? 1 : 0);
			goto IL_03f6;
			IL_03f6:
			if (num8 == 0)
			{
				continue;
			}
			if (!component.gameObject.activeInHierarchy)
			{
				continue;
			}
			if (!(component.material != null))
			{
				continue;
			}
			if (fadeObject == null)
			{
				fadeObject = component.gameObject.AddComponent<FadeObject>();
			}
			fadeObject.SetTargetTransparency(num6, fadeTime, fadeTime, m_nonActorFadeShader);
			Renderer[] componentsInChildren = component.gameObject.GetComponentsInChildren<Renderer>();
			if (componentsInChildren == null)
			{
				continue;
			}
			for (int num9 = 0; num9 < componentsInChildren.Length; num9++)
			{
				component = componentsInChildren[num9];
				fadeObject = component.GetComponent<FadeObject>();
				int num10;
				if (fadeObject != null)
				{
					if (fadeObject.ShouldProcessEvenIfRendererIsDisabled())
					{
						num10 = 1;
						goto IL_04d9;
					}
				}
				num10 = (component.enabled ? 1 : 0);
				goto IL_04d9;
				IL_04d9:
				if (num10 != 0)
				{
					if (component.gameObject.activeInHierarchy)
					{
						if (component.material != null)
						{
							fadeObject = component.GetComponent<FadeObject>();
							if (fadeObject == null)
							{
								fadeObject = component.gameObject.AddComponent<FadeObject>();
							}
							fadeObject.SetTargetTransparency(num6, fadeTime, fadeTime, m_nonActorFadeShader);
						}
					}
				}
			}
		}
		while (true)
		{
			switch (4)
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
		if (CameraManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get() != null)
			{
				if (!GameFlowData.Get().Started)
				{
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
			if (m_markedForResetVisibleObjects)
			{
				ResetDesiredVisibleObjects();
			}
			if (CameraManager.Get().InCinematic())
			{
				m_timeTillNextUpdate = 0f;
			}
			else
			{
				m_timeTillNextUpdate -= Time.unscaledDeltaTime;
				if (m_timeTillNextUpdate > 0f)
				{
					while (true)
					{
						switch (6)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				m_timeTillNextUpdate = 0.25f;
			}
			for (int i = 0; i < m_desiredVisibleObjects.Count; i++)
			{
				GameObject gameObject = m_desiredVisibleObjects[i];
				if (!(gameObject != null))
				{
					continue;
				}
				RevealPosition(gameObject.transform.position);
				if (ForceFadeAll)
				{
					break;
				}
			}
			if (IsMouseRevealEnabled() && !m_mouseCursorInDesiredVisibleObjects)
			{
				while (true)
				{
					RevealPosition(Board.Get().PlayerFreePos);
					return;
				}
			}
			return;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!(Board.Get() != null))
		{
			return;
		}
		while (true)
		{
			int num;
			if (CameraManager.Get() != null)
			{
				num = (CameraManager.Get().InCinematic() ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			bool forceFadeAll = ForceFadeAll;
			float num2;
			if (flag)
			{
				num2 = 0.1f;
			}
			else
			{
				num2 = 1f;
			}
			float num3 = num2;
			float num4 = m_collisionRadius;
			if (forceFadeAll)
			{
				num3 = -75f;
				num4 = 150f;
			}
			Gizmos.color = Color.yellow;
			foreach (GameObject desiredVisibleObject in m_desiredVisibleObjects)
			{
				if (desiredVisibleObject != null)
				{
					Vector3 position = desiredVisibleObject.transform.position;
					position.y += num4 + num3;
					Gizmos.DrawWireSphere(position, num4);
				}
			}
			if (!IsMouseRevealEnabled())
			{
				return;
			}
			while (true)
			{
				if (!m_mouseCursorInDesiredVisibleObjects)
				{
					while (true)
					{
						Vector3 playerFreePos = Board.Get().PlayerFreePos;
						playerFreePos.y += num4 + num3;
						Gizmos.DrawWireSphere(playerFreePos, num4);
						return;
					}
				}
				return;
			}
		}
	}
}
