using System;
using I2.Loc;
using UnityEngine;

public class UICharacterSelectRing : MonoBehaviour
{
	public Transform m_characterContainer;

	public Animator m_readyAnimation;

	public Animator m_isInGameAnimation;

	public Animator m_baseAnimator;

	public GameObject m_nameObject;

	public GameObject m_charSelectSpawnVFX;

	private GameObject m_characterModelParent;

	private bool m_readyIsVisible;

	private void Awake()
	{
		this.m_characterModelParent = new GameObject("ui_model_parent");
		this.m_characterModelParent.transform.parent = this.m_characterContainer;
		this.m_characterModelParent.transform.localPosition = Vector3.zero;
		this.m_characterModelParent.transform.localRotation = Quaternion.identity;
		this.m_characterModelParent.transform.localScale = Vector3.one;
	}

	private void Start()
	{
		this.PlayAnimation("ReadyOut");
		if (this.m_charSelectSpawnVFX != null)
		{
			this.m_charSelectSpawnVFX.SetActive(false);
		}
	}

	public Transform GetContainerTransform()
	{
		return this.m_characterModelParent.transform;
	}

	public void PlayBaseObjectAnimation(string animName)
	{
		if (this.m_baseAnimator != null)
		{
			bool flag = true;
			if (animName == "SlotIN")
			{
				AnimatorClipInfo[] currentAnimatorClipInfo = this.m_baseAnimator.GetCurrentAnimatorClipInfo(0);
				if (currentAnimatorClipInfo != null)
				{
					if (currentAnimatorClipInfo.Length > 0)
					{
						if (!(currentAnimatorClipInfo[0].clip.name == "SlotIN"))
						{
							if (!(currentAnimatorClipInfo[0].clip.name == "SlotIDLE"))
							{
								goto IL_B9;
							}
						}
						flag = false;
					}
				}
			}
			IL_B9:
			if (flag)
			{
				if (this.m_baseAnimator.isInitialized)
				{
					this.m_baseAnimator.Play(animName);
				}
			}
		}
	}

	public void CheckReadyBand(bool isReady)
	{
		if (this.m_readyIsVisible != isReady)
		{
			string animName;
			if (isReady)
			{
				animName = "ReadyIn";
			}
			else
			{
				animName = "ReadyOut";
			}
			this.PlayAnimation(animName);
		}
	}

	public void PlayAnimation(string animName)
	{
		if (animName == "ReadyOut")
		{
			this.m_readyAnimation.gameObject.SetActive(false);
			this.m_readyIsVisible = false;
		}
		else if (animName == "ReadyIn")
		{
			this.m_readyAnimation.gameObject.SetActive(true);
			this.m_readyIsVisible = true;
		}
		if (this.m_readyAnimation.isInitialized)
		{
			this.m_readyAnimation.Play(animName);
		}
		if (this.m_readyIsVisible)
		{
			if (this.m_isInGameAnimation.gameObject.activeSelf)
			{
				this.m_isInGameAnimation.gameObject.SetActive(false);
			}
		}
	}

	public void SetClickable(bool clickable)
	{
	}

	public void SetHidden(bool hidden)
	{
	}

	public void OnModifyReadyLocalization()
	{
		if (string.IsNullOrEmpty(Localize.MainTranslation))
		{
			return;
		}
		MeshRenderer component = this.m_readyAnimation.GetComponent<MeshRenderer>();
		Localize component2 = this.m_readyAnimation.GetComponent<Localize>();
		if (component != null && component2 != null)
		{
			Texture texture = component2.FindTranslatedObject<Texture>(Localize.MainTranslation);
			if (texture != null)
			{
				component.material.mainTexture = texture;
			}
		}
	}

	public void OnModifyInGameLocalization()
	{
		if (string.IsNullOrEmpty(Localize.MainTranslation))
		{
			return;
		}
		MeshRenderer component = this.m_isInGameAnimation.GetComponent<MeshRenderer>();
		Localize component2 = this.m_isInGameAnimation.GetComponent<Localize>();
		if (component != null)
		{
			if (component2 != null)
			{
				Texture texture = component2.FindTranslatedObject<Texture>(Localize.MainTranslation);
				if (texture != null)
				{
					component.material.mainTexture = texture;
				}
			}
		}
	}
}
