using System.Collections.Generic;
using UnityEngine;

public class MovementPathStart : MonoBehaviour
{
	public MeshRenderer m_InsideMesh;

	public MeshRenderer m_MiddleMesh;

	public MeshRenderer m_OutsideMesh;

	public MeshRenderer m_ArrowMesh;

	public MeshRenderer m_CloseArrowMesh;

	public MeshRenderer m_FarArrowMesh;

	public Texture m_insideTexture;

	public Texture m_middleTexture;

	public Texture m_outsideTexture;

	public Texture m_arrowTexture;

	public Texture m_closeArrowTexture;

	public Texture m_farArrowTexture;

	public MeshRenderer[] m_chasingDiamonds;

	public MeshRenderer m_chasingInnerRing;

	public MeshRenderer m_chasingOuterRing;

	public MeshRenderer m_chasingArrow;

	public Texture m_chasingDiamondTexture;

	public Texture m_chasingInnerRingTexture;

	public Texture m_chasingOuterRingTexture;

	public Texture m_chasingArrowTexture;

	public MeshRenderer m_KnockbackMesh;

	public Animator m_animationController;

	public GameObject m_movementContainer;

	public GameObject m_chasingContainer;

	public GameObject m_knockbackContainer;

	public GameObject m_chasingDiamondsContainer;

	public MovementPathEnd endPiece;

	public GameObject linePiece;

	private GameObject m_objectMovingAcross;

	private List<Vector3> m_pointsToMoveAcross;

	private float m_totalLineDistance;

	private List<MeshRenderer> m_allMeshes;

	private const float c_glowMovementSpeed = 10f;

	private const float c_movementGlowPauseTime = 0.5f;

	private bool m_objectMovingAcrossIsActive;

	private int currentPoint;

	private float m_travelSpeed;

	private ActorData m_actorData;

	private float m_finishPauseTime;

	private List<MeshRenderer> AllMeshes
	{
		get
		{
			if (m_allMeshes == null)
			{
				CreateList();
			}
			return m_allMeshes;
		}
	}

	private void CreateList()
	{
		m_allMeshes = new List<MeshRenderer>();
		if (m_InsideMesh != null)
		{
			m_allMeshes.Add(m_InsideMesh);
			m_InsideMesh.materials[0].mainTexture = m_insideTexture;
		}
		if (m_MiddleMesh != null)
		{
			m_allMeshes.Add(m_MiddleMesh);
			m_MiddleMesh.materials[0].mainTexture = m_middleTexture;
			Color color = m_MiddleMesh.materials[0].GetColor("_TintColor");
			color.a = 0.1f;
			m_MiddleMesh.materials[0].SetColor("_TintColor", color);
		}
		if (m_OutsideMesh != null)
		{
			m_allMeshes.Add(m_OutsideMesh);
			m_OutsideMesh.materials[0].mainTexture = m_outsideTexture;
		}
		if (m_ArrowMesh != null)
		{
			m_allMeshes.Add(m_ArrowMesh);
			m_ArrowMesh.materials[0].mainTexture = m_arrowTexture;
		}
		if (m_CloseArrowMesh != null)
		{
			m_allMeshes.Add(m_CloseArrowMesh);
			m_CloseArrowMesh.materials[0].mainTexture = m_closeArrowTexture;
		}
		if (m_FarArrowMesh != null)
		{
			m_allMeshes.Add(m_FarArrowMesh);
			m_FarArrowMesh.materials[0].mainTexture = m_farArrowTexture;
		}
		if (m_chasingInnerRing != null)
		{
			m_allMeshes.Add(m_chasingInnerRing);
			m_chasingInnerRing.materials[0].mainTexture = m_chasingInnerRingTexture;
			Color color2 = m_MiddleMesh.materials[0].GetColor("_TintColor");
			color2.a = 0.2f;
			m_MiddleMesh.materials[0].SetColor("_TintColor", color2);
		}
		if (m_chasingOuterRing != null)
		{
			m_allMeshes.Add(m_chasingOuterRing);
			m_chasingOuterRing.materials[0].mainTexture = m_chasingOuterRingTexture;
		}
		if (m_chasingArrow != null)
		{
			m_allMeshes.Add(m_chasingArrow);
			m_chasingArrow.materials[0].mainTexture = m_chasingArrowTexture;
		}
		if (m_KnockbackMesh != null)
		{
			m_allMeshes.Add(m_KnockbackMesh);
		}
		for (int i = 0; i < m_chasingDiamonds.Length; i++)
		{
			if (m_chasingDiamonds[i] != null)
			{
				m_allMeshes.Add(m_chasingDiamonds[i]);
				m_chasingDiamonds[i].materials[0].mainTexture = m_chasingDiamondTexture;
				Color color3 = m_chasingDiamonds[i].materials[0].GetColor("_TintColor");
				color3.a = 0.1f;
				m_chasingDiamonds[i].materials[0].SetColor("_TintColor", color3);
			}
		}
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

	public void AddLinePiece(GameObject newLine, GameObject parent)
	{
		linePiece = newLine;
		if (m_objectMovingAcross == null)
		{
			m_objectMovingAcross = Object.Instantiate(HighlightUtils.Get().m_movementGlowObject);
		}
		m_objectMovingAcross.transform.SetParent(parent.transform);
		if (m_pointsToMoveAcross != null)
		{
			m_pointsToMoveAcross.Clear();
		}
		else
		{
			m_pointsToMoveAcross = new List<Vector3>();
		}
		Vector3[] vertices = newLine.GetComponent<MeshFilter>().mesh.vertices;
		for (int i = 0; i < vertices.Length - 1; i += 2)
		{
			m_pointsToMoveAcross.Add((vertices[i] + vertices[i + 1]) * 0.5f);
		}
		while (true)
		{
			if (m_pointsToMoveAcross.Count > 1)
			{
				Vector3 normalized = (m_pointsToMoveAcross[m_pointsToMoveAcross.Count - 1] - m_pointsToMoveAcross[m_pointsToMoveAcross.Count - 2]).normalized;
				m_pointsToMoveAcross.Add(m_pointsToMoveAcross[m_pointsToMoveAcross.Count - 1] + normalized * Board.Get().squareSize * 0.2f);
			}
			m_totalLineDistance = 0f;
			for (int j = 1; j < m_pointsToMoveAcross.Count; j++)
			{
				m_totalLineDistance += (m_pointsToMoveAcross[j] - m_pointsToMoveAcross[j - 1]).magnitude;
			}
			while (true)
			{
				currentPoint = 1;
				m_travelSpeed = 10f;
				m_objectMovingAcross.transform.position = m_pointsToMoveAcross[0];
				return;
			}
		}
	}

	public void HideCharacterMovementPanel()
	{
		if (UICharacterMovementPanel.Get() != null)
		{
			UICharacterMovementPanel.Get().RemoveMovementIndicator(m_actorData);
		}
	}

	public void SetCharacterMovementPanel(BoardSquare endLocation)
	{
		if (!(UICharacterMovementPanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			UICharacterMovementPanel.Get().AddMovementIndicator(endLocation, m_actorData);
			return;
		}
	}

	public void Setup(ActorData actor, bool isChasing, AbilityUtil_Targeter.TargeterMovementType movementType)
	{
		if (m_actorData != null && m_actorData != actor)
		{
			HideCharacterMovementPanel();
		}
		m_actorData = actor;
		m_objectMovingAcrossIsActive = true;
		bool active = !isChasing;
		bool active2 = isChasing;
		bool flag = movementType == AbilityUtil_Targeter.TargeterMovementType.Knockback;
		if (flag)
		{
			active = false;
			active2 = false;
		}
		m_movementContainer.SetActive(active);
		m_chasingContainer.SetActive(active2);
		m_knockbackContainer.SetActive(flag);
	}

	public void SetGlow(bool glowOn)
	{
		m_objectMovingAcrossIsActive = glowOn;
		if (!(m_objectMovingAcross != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_objectMovingAcross.GetComponent<PKFxFX>() != null))
			{
				return;
			}
			if (m_objectMovingAcrossIsActive)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_objectMovingAcross.GetComponent<PKFxFX>().StartEffect();
						return;
					}
				}
			}
			m_objectMovingAcross.GetComponent<PKFxFX>().TerminateEffect();
			return;
		}
	}

	public void SetColor(Color newColor)
	{
		using (List<MeshRenderer>.Enumerator enumerator = AllMeshes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MeshRenderer current = enumerator.Current;
				if (current.materials.Length > 0)
				{
					if (current.materials[0] != null)
					{
						current.materials[0].SetColor("_TintColor", newColor);
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
	}

	private void OnDisable()
	{
		if (UICharacterMovementPanel.Get() != null)
		{
			HideCharacterMovementPanel();
		}
	}

	public void Update()
	{
		if (m_actorData != null)
		{
			if (m_actorData.QueuedMovementAllowsAbility)
			{
				m_CloseArrowMesh.gameObject.SetActive(true);
				m_FarArrowMesh.gameObject.SetActive(false);
			}
			else
			{
				m_CloseArrowMesh.gameObject.SetActive(false);
				m_FarArrowMesh.gameObject.SetActive(true);
			}
		}
		if (!(linePiece != null))
		{
			return;
		}
		while (true)
		{
			MeshRenderer component = linePiece.GetComponent<MeshRenderer>();
			if (component != null)
			{
				if (component.material != null)
				{
					Vector2 textureOffset = component.material.GetTextureOffset("_MainTex");
					textureOffset.x -= Time.deltaTime;
					component.material.SetTextureOffset("_MainTex", textureOffset);
				}
			}
			if (currentPoint < m_pointsToMoveAcross.Count)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						if (m_finishPauseTime > 0f)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									m_finishPauseTime -= Time.deltaTime;
									return;
								}
							}
						}
						if (m_objectMovingAcrossIsActive && m_objectMovingAcross.GetComponent<PKFxFX>() != null)
						{
							m_objectMovingAcross.GetComponent<PKFxFX>().StartEffect();
						}
						Vector3 normalized = (m_pointsToMoveAcross[currentPoint] - m_pointsToMoveAcross[currentPoint - 1]).normalized;
						Vector3 b = normalized * Time.deltaTime * m_travelSpeed;
						m_objectMovingAcross.transform.position = m_objectMovingAcross.transform.position + b;
						if ((m_pointsToMoveAcross[currentPoint - 1] - m_pointsToMoveAcross[currentPoint]).sqrMagnitude < (m_pointsToMoveAcross[currentPoint - 1] - m_objectMovingAcross.transform.position).sqrMagnitude)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									m_objectMovingAcross.transform.position = m_pointsToMoveAcross[currentPoint];
									currentPoint++;
									return;
								}
							}
						}
						return;
					}
					}
				}
			}
			if (m_objectMovingAcross.GetComponent<PKFxFX>() != null)
			{
				m_objectMovingAcross.GetComponent<PKFxFX>().TerminateEffect();
			}
			currentPoint = 1;
			m_objectMovingAcross.transform.position = m_pointsToMoveAcross[0];
			m_finishPauseTime = 0.5f;
			return;
		}
	}
}
