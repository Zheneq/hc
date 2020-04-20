using System;
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
			if (this.m_allMeshes == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.get_AllMeshes()).MethodHandle;
				}
				this.CreateList();
			}
			return this.m_allMeshes;
		}
	}

	private void CreateList()
	{
		this.m_allMeshes = new List<MeshRenderer>();
		if (this.m_InsideMesh != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.CreateList()).MethodHandle;
			}
			this.m_allMeshes.Add(this.m_InsideMesh);
			this.m_InsideMesh.materials[0].mainTexture = this.m_insideTexture;
		}
		if (this.m_MiddleMesh != null)
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
			this.m_allMeshes.Add(this.m_MiddleMesh);
			this.m_MiddleMesh.materials[0].mainTexture = this.m_middleTexture;
			Color color = this.m_MiddleMesh.materials[0].GetColor("_TintColor");
			color.a = 0.1f;
			this.m_MiddleMesh.materials[0].SetColor("_TintColor", color);
		}
		if (this.m_OutsideMesh != null)
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
			this.m_allMeshes.Add(this.m_OutsideMesh);
			this.m_OutsideMesh.materials[0].mainTexture = this.m_outsideTexture;
		}
		if (this.m_ArrowMesh != null)
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
			this.m_allMeshes.Add(this.m_ArrowMesh);
			this.m_ArrowMesh.materials[0].mainTexture = this.m_arrowTexture;
		}
		if (this.m_CloseArrowMesh != null)
		{
			this.m_allMeshes.Add(this.m_CloseArrowMesh);
			this.m_CloseArrowMesh.materials[0].mainTexture = this.m_closeArrowTexture;
		}
		if (this.m_FarArrowMesh != null)
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
			this.m_allMeshes.Add(this.m_FarArrowMesh);
			this.m_FarArrowMesh.materials[0].mainTexture = this.m_farArrowTexture;
		}
		if (this.m_chasingInnerRing != null)
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
			this.m_allMeshes.Add(this.m_chasingInnerRing);
			this.m_chasingInnerRing.materials[0].mainTexture = this.m_chasingInnerRingTexture;
			Color color2 = this.m_MiddleMesh.materials[0].GetColor("_TintColor");
			color2.a = 0.2f;
			this.m_MiddleMesh.materials[0].SetColor("_TintColor", color2);
		}
		if (this.m_chasingOuterRing != null)
		{
			this.m_allMeshes.Add(this.m_chasingOuterRing);
			this.m_chasingOuterRing.materials[0].mainTexture = this.m_chasingOuterRingTexture;
		}
		if (this.m_chasingArrow != null)
		{
			this.m_allMeshes.Add(this.m_chasingArrow);
			this.m_chasingArrow.materials[0].mainTexture = this.m_chasingArrowTexture;
		}
		if (this.m_KnockbackMesh != null)
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
			this.m_allMeshes.Add(this.m_KnockbackMesh);
		}
		for (int i = 0; i < this.m_chasingDiamonds.Length; i++)
		{
			if (this.m_chasingDiamonds[i] != null)
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
				this.m_allMeshes.Add(this.m_chasingDiamonds[i]);
				this.m_chasingDiamonds[i].materials[0].mainTexture = this.m_chasingDiamondTexture;
				Color color3 = this.m_chasingDiamonds[i].materials[0].GetColor("_TintColor");
				color3.a = 0.1f;
				this.m_chasingDiamonds[i].materials[0].SetColor("_TintColor", color3);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void AddLinePiece(GameObject newLine, GameObject parent)
	{
		this.linePiece = newLine;
		if (this.m_objectMovingAcross == null)
		{
			this.m_objectMovingAcross = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_movementGlowObject);
		}
		this.m_objectMovingAcross.transform.SetParent(parent.transform);
		if (this.m_pointsToMoveAcross != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.AddLinePiece(GameObject, GameObject)).MethodHandle;
			}
			this.m_pointsToMoveAcross.Clear();
		}
		else
		{
			this.m_pointsToMoveAcross = new List<Vector3>();
		}
		Vector3[] vertices = newLine.GetComponent<MeshFilter>().mesh.vertices;
		for (int i = 0; i < vertices.Length - 1; i += 2)
		{
			this.m_pointsToMoveAcross.Add((vertices[i] + vertices[i + 1]) * 0.5f);
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
		if (this.m_pointsToMoveAcross.Count > 1)
		{
			Vector3 normalized = (this.m_pointsToMoveAcross[this.m_pointsToMoveAcross.Count - 1] - this.m_pointsToMoveAcross[this.m_pointsToMoveAcross.Count - 2]).normalized;
			this.m_pointsToMoveAcross.Add(this.m_pointsToMoveAcross[this.m_pointsToMoveAcross.Count - 1] + normalized * Board.Get().squareSize * 0.2f);
		}
		this.m_totalLineDistance = 0f;
		for (int j = 1; j < this.m_pointsToMoveAcross.Count; j++)
		{
			this.m_totalLineDistance += (this.m_pointsToMoveAcross[j] - this.m_pointsToMoveAcross[j - 1]).magnitude;
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		this.currentPoint = 1;
		this.m_travelSpeed = 10f;
		this.m_objectMovingAcross.transform.position = this.m_pointsToMoveAcross[0];
	}

	public void HideCharacterMovementPanel()
	{
		if (UICharacterMovementPanel.Get() != null)
		{
			UICharacterMovementPanel.Get().RemoveMovementIndicator(this.m_actorData);
		}
	}

	public void SetCharacterMovementPanel(BoardSquare endLocation)
	{
		if (UICharacterMovementPanel.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.SetCharacterMovementPanel(BoardSquare)).MethodHandle;
			}
			UICharacterMovementPanel.Get().AddMovementIndicator(endLocation, this.m_actorData);
		}
	}

	public void Setup(ActorData actor, bool isChasing, AbilityUtil_Targeter.TargeterMovementType movementType)
	{
		if (this.m_actorData != null && this.m_actorData != actor)
		{
			this.HideCharacterMovementPanel();
		}
		this.m_actorData = actor;
		this.m_objectMovingAcrossIsActive = true;
		bool active = !isChasing;
		bool active2 = isChasing;
		bool flag = movementType == AbilityUtil_Targeter.TargeterMovementType.Knockback;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.Setup(ActorData, bool, AbilityUtil_Targeter.TargeterMovementType)).MethodHandle;
			}
			active = false;
			active2 = false;
		}
		this.m_movementContainer.SetActive(active);
		this.m_chasingContainer.SetActive(active2);
		this.m_knockbackContainer.SetActive(flag);
	}

	public void SetGlow(bool glowOn)
	{
		this.m_objectMovingAcrossIsActive = glowOn;
		if (this.m_objectMovingAcross != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.SetGlow(bool)).MethodHandle;
			}
			if (this.m_objectMovingAcross.GetComponent<PKFxFX>() != null)
			{
				if (this.m_objectMovingAcrossIsActive)
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
					this.m_objectMovingAcross.GetComponent<PKFxFX>().StartEffect();
				}
				else
				{
					this.m_objectMovingAcross.GetComponent<PKFxFX>().TerminateEffect();
				}
			}
		}
	}

	public void SetColor(Color newColor)
	{
		using (List<MeshRenderer>.Enumerator enumerator = this.AllMeshes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MeshRenderer meshRenderer = enumerator.Current;
				if (meshRenderer.materials.Length > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.SetColor(Color)).MethodHandle;
					}
					if (meshRenderer.materials[0] != null)
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
						meshRenderer.materials[0].SetColor("_TintColor", newColor);
					}
				}
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
	}

	private void OnDisable()
	{
		if (UICharacterMovementPanel.Get() != null)
		{
			this.HideCharacterMovementPanel();
		}
	}

	public void Update()
	{
		if (this.m_actorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathStart.Update()).MethodHandle;
			}
			if (this.m_actorData.QueuedMovementAllowsAbility)
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
				this.m_CloseArrowMesh.gameObject.SetActive(true);
				this.m_FarArrowMesh.gameObject.SetActive(false);
			}
			else
			{
				this.m_CloseArrowMesh.gameObject.SetActive(false);
				this.m_FarArrowMesh.gameObject.SetActive(true);
			}
		}
		if (this.linePiece != null)
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
			MeshRenderer component = this.linePiece.GetComponent<MeshRenderer>();
			if (component != null)
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
				if (component.material != null)
				{
					Vector2 textureOffset = component.material.GetTextureOffset("_MainTex");
					textureOffset.x -= Time.deltaTime;
					component.material.SetTextureOffset("_MainTex", textureOffset);
				}
			}
			if (this.currentPoint < this.m_pointsToMoveAcross.Count)
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
				if (this.m_finishPauseTime > 0f)
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
					this.m_finishPauseTime -= Time.deltaTime;
				}
				else
				{
					if (this.m_objectMovingAcrossIsActive && this.m_objectMovingAcross.GetComponent<PKFxFX>() != null)
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
						this.m_objectMovingAcross.GetComponent<PKFxFX>().StartEffect();
					}
					Vector3 normalized = (this.m_pointsToMoveAcross[this.currentPoint] - this.m_pointsToMoveAcross[this.currentPoint - 1]).normalized;
					Vector3 b = normalized * Time.deltaTime * this.m_travelSpeed;
					this.m_objectMovingAcross.transform.position = this.m_objectMovingAcross.transform.position + b;
					if ((this.m_pointsToMoveAcross[this.currentPoint - 1] - this.m_pointsToMoveAcross[this.currentPoint]).sqrMagnitude < (this.m_pointsToMoveAcross[this.currentPoint - 1] - this.m_objectMovingAcross.transform.position).sqrMagnitude)
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
						this.m_objectMovingAcross.transform.position = this.m_pointsToMoveAcross[this.currentPoint];
						this.currentPoint++;
					}
				}
			}
			else
			{
				if (this.m_objectMovingAcross.GetComponent<PKFxFX>() != null)
				{
					this.m_objectMovingAcross.GetComponent<PKFxFX>().TerminateEffect();
				}
				this.currentPoint = 1;
				this.m_objectMovingAcross.transform.position = this.m_pointsToMoveAcross[0];
				this.m_finishPauseTime = 0.5f;
			}
		}
	}
}
