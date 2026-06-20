using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIActorModelData : MonoBehaviour
{
	public bool m_setOffsetTowardsCamera;

	public float m_offsetDistanceTowardsCamera;

	private bool m_showingOutline;

	private bool m_mouseOver;

	private Renderer[] m_renderers;

	private bool[] m_shroudInstancesToEnable;

	private ShroudInstance[] m_shroudInstances;

	private Animator m_modelAnimator;

	private bool m_dirtyRenderersCache;

	private GameObject m_RollOverEffect;

	private static readonly string[] s_charSelStateNames = new string[5]
	{
		"Char_Sel_Spawn",
		"Char_Sel_Idle",
		"Char_Sel_Idle_Aggro",
		"Char_Sel_Idle_Aggro_to_Char_Sel_Idle",
		"Char_Sel_Idle_to_Char_Sel_Idle_Aggro"
	};

	private void Awake()
	{
		m_modelAnimator = GetComponentInChildren<Animator>();
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
		m_shroudInstances = GetComponentsInChildren<ShroudInstance>();
		if (m_shroudInstances.Length > 0)
		{
			m_dirtyRenderersCache = true;
		}
		else
		{
			CacheRenderers();
		}
		SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>();
		if (componentsInChildren == null)
		{
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].enabled)
			{
				componentsInChildren[i].updateWhenOffscreen = true;
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

	public void DelayEnablingOfShroudInstances()
	{
		ShroudInstance[] componentsInChildren = GetComponentsInChildren<ShroudInstance>();
		m_shroudInstancesToEnable = new bool[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			m_shroudInstancesToEnable[i] = componentsInChildren[i].enabled;
			componentsInChildren[i].enabled = false;
		}
		while (true)
		{
			return;
		}
	}

	private void OnDestroy()
	{
		if (m_renderers == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_renderers.Length; i++)
			{
				if (!(m_renderers[i] != null) || m_renderers[i].materials == null)
				{
					continue;
				}
				for (int j = 0; j < m_renderers[i].materials.Length; j++)
				{
					if (m_renderers[i].materials[j] != null)
					{
						Object.Destroy(m_renderers[i].materials[j]);
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
	}

	private void CacheRenderers()
	{
		m_dirtyRenderersCache = false;
		m_renderers = base.gameObject.GetComponentsInChildren<Renderer>();
		if (m_renderers != null)
		{
			for (int i = 0; i < m_renderers.Length; i++)
			{
				Material[] materials = m_renderers[i].materials;
				if (materials == null)
				{
					continue;
				}
				for (int j = 0; j < materials.Length; j++)
				{
					Texture mainTexture = materials[j].mainTexture;
					if (mainTexture != null)
					{
						mainTexture.mipMapBias = -1f;
					}
				}
			}
		}
		SetChildMaterialProperty[] componentsInChildren = base.gameObject.GetComponentsInChildren<SetChildMaterialProperty>();
		SetChildMaterialProperty[] array = componentsInChildren;
		foreach (SetChildMaterialProperty setChildMaterialProperty in array)
		{
			setChildMaterialProperty.ReinitRenderersList();
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

	private void Update()
	{
		bool flag = false;
		if (m_shroudInstancesToEnable != null)
		{
			for (int i = 0; i < m_shroudInstancesToEnable.Length; i++)
			{
				m_shroudInstances[i].enabled = m_shroudInstancesToEnable[i];
				flag = true;
			}
			m_shroudInstancesToEnable = null;
		}
		if (m_dirtyRenderersCache)
		{
			if (!flag)
			{
				CacheRenderers();
			}
		}
		UpdateSelectionOutline();
		if (Application.isEditor)
		{
			_001D();
		}
		SetParentLocalPositionOffset();
	}

	public void SetParentLocalPositionOffset()
	{
		if (!m_setOffsetTowardsCamera)
		{
			return;
		}
		while (true)
		{
			if (m_offsetDistanceTowardsCamera == 0f)
			{
				return;
			}
			while (true)
			{
				if (!(Camera.main != null))
				{
					return;
				}
				while (true)
				{
					if (base.transform.parent != null)
					{
						while (true)
						{
							Vector3 direction = Camera.main.transform.position - base.transform.parent.parent.transform.position;
							direction.y = 0f;
							direction.Normalize();
							Vector3 a = base.transform.parent.InverseTransformDirection(direction);
							Vector3 localPosition = m_offsetDistanceTowardsCamera * a;
							base.transform.parent.localPosition = localPosition;
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void SetReady(bool ready)
	{
		if (!(m_modelAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (!m_modelAnimator.isInitialized)
			{
				return;
			}
			while (true)
			{
				if (ready)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							m_modelAnimator.SetBool("DecisionPhase", false);
							return;
						}
					}
				}
				m_modelAnimator.SetBool("DecisionPhase", true);
				return;
			}
		}
	}

	private bool ParamExists(Animator animator, string paramName)
	{
		for (int i = 0; i < animator.parameterCount; i++)
		{
			if (!(animator.parameters[i].name == paramName))
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

	public void SetSkin(CharacterVisualInfo visualInfo)
	{
		if (!(m_modelAnimator != null) || !m_modelAnimator.isInitialized)
		{
			return;
		}
		while (true)
		{
			if (ParamExists(m_modelAnimator, "SkinIndex"))
			{
				m_modelAnimator.SetInteger("SkinIndex", visualInfo.skinIndex);
			}
			if (ParamExists(m_modelAnimator, "PatternIndex"))
			{
				m_modelAnimator.SetInteger("PatternIndex", visualInfo.patternIndex);
			}
			if (ParamExists(m_modelAnimator, "ColorIndex"))
			{
				m_modelAnimator.SetInteger("ColorIndex", visualInfo.colorIndex);
			}
			return;
		}
	}

	public bool MousedOver(Camera viewCam = null)
	{
		if (!m_mouseOver)
		{
			if (viewCam != null)
			{
				UICharacterSelectRing componentInParent = base.gameObject.GetComponentInParent<UICharacterSelectRing>();
				Ray ray = viewCam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo = default(RaycastHit);
				if (Physics.Raycast(ray.origin, ray.direction * 20f, out hitInfo, viewCam.cullingMask))
				{
					UICharacterSelectRing componentInParent2 = hitInfo.collider.gameObject.GetComponentInParent<UICharacterSelectRing>();
					if (componentInParent2 != null && componentInParent2 == componentInParent)
					{
						return true;
					}
				}
			}
		}
		return m_mouseOver;
	}

	public void SetMouseIsOver(bool mouseIsOver)
	{
		m_mouseOver = mouseIsOver;
	}

	private void UpdateSelectionOutline()
	{
		bool flag = m_mouseOver;
		if (UICharacterSelectWorldObjects.Get().IsVisible())
		{
			if (EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>().HasFocus())
			{
				if (m_renderers != null)
				{
					if (!flag)
					{
						if (UICharacterSelectScreenController.Get() != null)
						{
							if (UIFrontEnd.Get() != null)
							{
								if (UICharacterSelectScreenController.Get().m_changeFreelancerBtn.gameObject.activeSelf)
								{
									UICharacterSelectRing componentInParent = base.gameObject.GetComponentInParent<UICharacterSelectRing>();
									if (componentInParent == UICharacterSelectWorldObjects.Get().m_ringAnimations[0])
									{
										flag = MousedOver(UIManager.Get().GetEnvirontmentCamera());
									}
								}
							}
						}
					}
					goto IL_0113;
				}
			}
		}
		flag = false;
		goto IL_0113;
		IL_0113:
		if (flag)
		{
			if (m_RollOverEffect == null)
			{
				m_RollOverEffect = Object.Instantiate(UIFrontEndUIResources.Get().m_RollOverPrefab);
				m_RollOverEffect.transform.SetParent(base.gameObject.transform);
				m_RollOverEffect.transform.localPosition = Vector3.zero;
			}
			if (Camera.main != null)
			{
				PlayerSelectionEffect component = Camera.main.GetComponent<PlayerSelectionEffect>();
				if (component != null)
				{
					component.m_drawSelection = true;
				}
			}
		}
		else if (m_RollOverEffect != null)
		{
			Object.Destroy(m_RollOverEffect);
		}
		if (flag == m_showingOutline)
		{
			return;
		}
		while (true)
		{
			int num = LayerMask.NameToLayer("ActorSelected");
			int layer = LayerMask.NameToLayer("UIInWorld");
			if (flag)
			{
				for (int i = 0; i < m_renderers.Length; i++)
				{
					m_renderers[i].gameObject.layer = num;
					for (int j = 0; j < m_renderers[i].materials.Length; j++)
					{
						if (m_renderers[i].materials[j].HasProperty("_Team"))
						{
							float value = 1f;
							m_renderers[i].materials[j].SetFloat("_Team", value);
						}
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							goto end_IL_02bc;
						}
						continue;
						end_IL_02bc:
						break;
					}
				}
			}
			else
			{
				for (int k = 0; k < m_renderers.Length; k++)
				{
					if (m_renderers[k].gameObject.layer == num)
					{
						m_renderers[k].gameObject.layer = layer;
					}
				}
			}
			m_showingOutline = flag;
			return;
		}
	}

	public static void SetCharSelectTrigger(Animator animator, bool transitionToNewCharacter, bool setTransitionParam)
	{
		if (!(animator != null) || animator.layerCount < 1 || IsInCharSelectAnimState(animator))
		{
			return;
		}
		while (true)
		{
			if (setTransitionParam)
			{
				animator.SetBool("CharSelectToNewChar", transitionToNewCharacter);
			}
			animator.SetTrigger("CharSelect");
			return;
		}
	}

	public static bool IsInCharSelectAnimState(Animator animator)
	{
		bool flag = false;
		if (animator.layerCount < 1)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		for (int i = 0; i < s_charSelStateNames.Length; i++)
		{
			if (!flag)
			{
				int num;
				if (!flag)
				{
					num = (currentAnimatorStateInfo.IsName(s_charSelStateNames[i]) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				flag = ((byte)num != 0);
				continue;
			}
			break;
		}
		return flag;
	}

	internal void SetStateNameHashToNameMap(Dictionary<int, string> newValue)
	{
	}

	private void _001D()
	{
	}
}
