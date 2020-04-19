using System;
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

	private static readonly string[] s_charSelStateNames = new string[]
	{
		"Char_Sel_Spawn",
		"Char_Sel_Idle",
		"Char_Sel_Idle_Aggro",
		"Char_Sel_Idle_Aggro_to_Char_Sel_Idle",
		"Char_Sel_Idle_to_Char_Sel_Idle_Aggro"
	};

	private void Awake()
	{
		this.m_modelAnimator = base.GetComponentInChildren<Animator>();
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
		this.m_shroudInstances = base.GetComponentsInChildren<ShroudInstance>();
		if (this.m_shroudInstances.Length > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.Start()).MethodHandle;
			}
			this.m_dirtyRenderersCache = true;
		}
		else
		{
			this.CacheRenderers();
		}
		SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].enabled)
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
					componentsInChildren[i].updateWhenOffscreen = true;
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
	}

	public void DelayEnablingOfShroudInstances()
	{
		ShroudInstance[] componentsInChildren = base.GetComponentsInChildren<ShroudInstance>();
		this.m_shroudInstancesToEnable = new bool[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.m_shroudInstancesToEnable[i] = componentsInChildren[i].enabled;
			componentsInChildren[i].enabled = false;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.DelayEnablingOfShroudInstances()).MethodHandle;
		}
	}

	private void OnDestroy()
	{
		if (this.m_renderers != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.OnDestroy()).MethodHandle;
			}
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				if (this.m_renderers[i] != null && this.m_renderers[i].materials != null)
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
					for (int j = 0; j < this.m_renderers[i].materials.Length; j++)
					{
						if (this.m_renderers[i].materials[j] != null)
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
							UnityEngine.Object.Destroy(this.m_renderers[i].materials[j]);
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
		}
	}

	private void CacheRenderers()
	{
		this.m_dirtyRenderersCache = false;
		this.m_renderers = base.gameObject.GetComponentsInChildren<Renderer>();
		if (this.m_renderers != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.CacheRenderers()).MethodHandle;
			}
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				Material[] materials = this.m_renderers[i].materials;
				if (materials != null)
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
		SetChildMaterialProperty[] componentsInChildren = base.gameObject.GetComponentsInChildren<SetChildMaterialProperty>();
		foreach (SetChildMaterialProperty setChildMaterialProperty in componentsInChildren)
		{
			setChildMaterialProperty.ReinitRenderersList();
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

	private void Update()
	{
		bool flag = false;
		if (this.m_shroudInstancesToEnable != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.Update()).MethodHandle;
			}
			for (int i = 0; i < this.m_shroudInstancesToEnable.Length; i++)
			{
				this.m_shroudInstances[i].enabled = this.m_shroudInstancesToEnable[i];
				flag = true;
			}
			this.m_shroudInstancesToEnable = null;
		}
		if (this.m_dirtyRenderersCache)
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
			if (!flag)
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
				this.CacheRenderers();
			}
		}
		this.UpdateSelectionOutline();
		if (Application.isEditor)
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
			this.\u001D();
		}
		this.SetParentLocalPositionOffset();
	}

	public void SetParentLocalPositionOffset()
	{
		if (this.m_setOffsetTowardsCamera)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.SetParentLocalPositionOffset()).MethodHandle;
			}
			if (this.m_offsetDistanceTowardsCamera != 0f)
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
				if (Camera.main != null)
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
					if (base.transform.parent != null)
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
						Vector3 direction = Camera.main.transform.position - base.transform.parent.parent.transform.position;
						direction.y = 0f;
						direction.Normalize();
						Vector3 a = base.transform.parent.InverseTransformDirection(direction);
						Vector3 localPosition = this.m_offsetDistanceTowardsCamera * a;
						base.transform.parent.localPosition = localPosition;
					}
				}
			}
		}
	}

	public void SetReady(bool ready)
	{
		if (this.m_modelAnimator != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.SetReady(bool)).MethodHandle;
			}
			if (this.m_modelAnimator.isInitialized)
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
				if (ready)
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
					this.m_modelAnimator.SetBool("DecisionPhase", false);
				}
				else
				{
					this.m_modelAnimator.SetBool("DecisionPhase", true);
				}
			}
		}
	}

	private bool ParamExists(Animator animator, string paramName)
	{
		for (int i = 0; i < animator.parameterCount; i++)
		{
			if (animator.parameters[i].name == paramName)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.ParamExists(Animator, string)).MethodHandle;
				}
				return true;
			}
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
		return false;
	}

	public void SetSkin(CharacterVisualInfo visualInfo)
	{
		if (this.m_modelAnimator != null && this.m_modelAnimator.isInitialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.SetSkin(CharacterVisualInfo)).MethodHandle;
			}
			if (this.ParamExists(this.m_modelAnimator, "SkinIndex"))
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
				this.m_modelAnimator.SetInteger("SkinIndex", visualInfo.skinIndex);
			}
			if (this.ParamExists(this.m_modelAnimator, "PatternIndex"))
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
				this.m_modelAnimator.SetInteger("PatternIndex", visualInfo.patternIndex);
			}
			if (this.ParamExists(this.m_modelAnimator, "ColorIndex"))
			{
				this.m_modelAnimator.SetInteger("ColorIndex", visualInfo.colorIndex);
			}
		}
	}

	public bool MousedOver(Camera viewCam = null)
	{
		if (!this.m_mouseOver)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.MousedOver(Camera)).MethodHandle;
			}
			if (viewCam != null)
			{
				UICharacterSelectRing componentInParent = base.gameObject.GetComponentInParent<UICharacterSelectRing>();
				Ray ray = viewCam.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit = default(RaycastHit);
				if (Physics.Raycast(ray.origin, ray.direction * 20f, out raycastHit, (float)viewCam.cullingMask))
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
					UICharacterSelectRing componentInParent2 = raycastHit.collider.gameObject.GetComponentInParent<UICharacterSelectRing>();
					if (componentInParent2 != null && componentInParent2 == componentInParent)
					{
						return true;
					}
				}
			}
		}
		return this.m_mouseOver;
	}

	public void SetMouseIsOver(bool mouseIsOver)
	{
		this.m_mouseOver = mouseIsOver;
	}

	private void UpdateSelectionOutline()
	{
		bool flag = this.m_mouseOver;
		if (UICharacterSelectWorldObjects.Get().IsVisible())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.UpdateSelectionOutline()).MethodHandle;
			}
			if (EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>().HasFocus())
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
				if (this.m_renderers == null)
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
				}
				else
				{
					if (flag)
					{
						goto IL_113;
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
					if (!(UICharacterSelectScreenController.Get() != null))
					{
						goto IL_113;
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
					if (!(UIFrontEnd.Get() != null))
					{
						goto IL_113;
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
					if (!UICharacterSelectScreenController.Get().m_changeFreelancerBtn.gameObject.activeSelf)
					{
						goto IL_113;
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
					UICharacterSelectRing componentInParent = base.gameObject.GetComponentInParent<UICharacterSelectRing>();
					if (componentInParent == UICharacterSelectWorldObjects.Get().m_ringAnimations[0])
					{
						flag = this.MousedOver(UIManager.Get().GetEnvirontmentCamera());
						goto IL_113;
					}
					goto IL_113;
				}
			}
		}
		flag = false;
		IL_113:
		if (flag)
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
			if (this.m_RollOverEffect == null)
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
				this.m_RollOverEffect = UnityEngine.Object.Instantiate<GameObject>(UIFrontEndUIResources.Get().m_RollOverPrefab);
				this.m_RollOverEffect.transform.SetParent(base.gameObject.transform);
				this.m_RollOverEffect.transform.localPosition = Vector3.zero;
			}
			if (Camera.main != null)
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
				PlayerSelectionEffect component = Camera.main.GetComponent<PlayerSelectionEffect>();
				if (component != null)
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
					component.m_drawSelection = true;
				}
			}
		}
		else if (this.m_RollOverEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_RollOverEffect);
		}
		if (flag != this.m_showingOutline)
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
			int num = LayerMask.NameToLayer("ActorSelected");
			int layer = LayerMask.NameToLayer("UIInWorld");
			if (flag)
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
				for (int i = 0; i < this.m_renderers.Length; i++)
				{
					this.m_renderers[i].gameObject.layer = num;
					for (int j = 0; j < this.m_renderers[i].materials.Length; j++)
					{
						if (this.m_renderers[i].materials[j].HasProperty("_Team"))
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
							float value = 1f;
							this.m_renderers[i].materials[j].SetFloat("_Team", value);
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
			else
			{
				for (int k = 0; k < this.m_renderers.Length; k++)
				{
					if (this.m_renderers[k].gameObject.layer == num)
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
						this.m_renderers[k].gameObject.layer = layer;
					}
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_showingOutline = flag;
		}
	}

	public static void SetCharSelectTrigger(Animator animator, bool transitionToNewCharacter, bool setTransitionParam)
	{
		if (animator != null)
		{
			if (animator.layerCount < 1)
			{
				return;
			}
			if (!UIActorModelData.IsInCharSelectAnimState(animator))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.SetCharSelectTrigger(Animator, bool, bool)).MethodHandle;
				}
				if (setTransitionParam)
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
					animator.SetBool("CharSelectToNewChar", transitionToNewCharacter);
				}
				animator.SetTrigger("CharSelect");
			}
		}
	}

	public static bool IsInCharSelectAnimState(Animator animator)
	{
		bool flag = false;
		if (animator.layerCount < 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorModelData.IsInCharSelectAnimState(Animator)).MethodHandle;
			}
			return false;
		}
		AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		int i = 0;
		while (i < UIActorModelData.s_charSelStateNames.Length)
		{
			if (flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					return flag;
				}
			}
			else
			{
				bool flag2;
				if (!flag)
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
					flag2 = currentAnimatorStateInfo.IsName(UIActorModelData.s_charSelStateNames[i]);
				}
				else
				{
					flag2 = true;
				}
				flag = flag2;
				i++;
			}
		}
		return flag;
	}

	internal void SetStateNameHashToNameMap(Dictionary<int, string> newValue)
	{
	}

	private void \u001D()
	{
	}
}
