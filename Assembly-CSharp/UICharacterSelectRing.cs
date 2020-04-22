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
		m_characterModelParent = new GameObject("ui_model_parent");
		m_characterModelParent.transform.parent = m_characterContainer;
		m_characterModelParent.transform.localPosition = Vector3.zero;
		m_characterModelParent.transform.localRotation = Quaternion.identity;
		m_characterModelParent.transform.localScale = Vector3.one;
	}

	private void Start()
	{
		PlayAnimation("ReadyOut");
		if (!(m_charSelectSpawnVFX != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_charSelectSpawnVFX.SetActive(false);
			return;
		}
	}

	public Transform GetContainerTransform()
	{
		return m_characterModelParent.transform;
	}

	public void PlayBaseObjectAnimation(string animName)
	{
		if (!(m_baseAnimator != null))
		{
			return;
		}
		bool flag = true;
		if (animName == "SlotIN")
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = m_baseAnimator.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (currentAnimatorClipInfo.Length > 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(currentAnimatorClipInfo[0].clip.name == "SlotIN"))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(currentAnimatorClipInfo[0].clip.name == "SlotIDLE"))
						{
							goto IL_00b9;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					flag = false;
				}
			}
		}
		goto IL_00b9;
		IL_00b9:
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (m_baseAnimator.isInitialized)
			{
				m_baseAnimator.Play(animName);
			}
			return;
		}
	}

	public void CheckReadyBand(bool isReady)
	{
		if (m_readyIsVisible == isReady)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			object animName;
			if (isReady)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				animName = "ReadyIn";
			}
			else
			{
				animName = "ReadyOut";
			}
			PlayAnimation((string)animName);
			return;
		}
	}

	public void PlayAnimation(string animName)
	{
		if (animName == "ReadyOut")
		{
			m_readyAnimation.gameObject.SetActive(false);
			m_readyIsVisible = false;
		}
		else if (animName == "ReadyIn")
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_readyAnimation.gameObject.SetActive(true);
			m_readyIsVisible = true;
		}
		if (m_readyAnimation.isInitialized)
		{
			m_readyAnimation.Play(animName);
		}
		if (!m_readyIsVisible)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (m_isInGameAnimation.gameObject.activeSelf)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					m_isInGameAnimation.gameObject.SetActive(false);
					return;
				}
			}
			return;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		MeshRenderer component = m_readyAnimation.GetComponent<MeshRenderer>();
		Localize component2 = m_readyAnimation.GetComponent<Localize>();
		if (!(component != null) || !(component2 != null))
		{
			return;
		}
		Texture texture = component2.FindTranslatedObject<Texture>(Localize.MainTranslation);
		if (!(texture != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			component.material.mainTexture = texture;
			return;
		}
	}

	public void OnModifyInGameLocalization()
	{
		if (string.IsNullOrEmpty(Localize.MainTranslation))
		{
			return;
		}
		MeshRenderer component = m_isInGameAnimation.GetComponent<MeshRenderer>();
		Localize component2 = m_isInGameAnimation.GetComponent<Localize>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(component2 != null))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				Texture texture = component2.FindTranslatedObject<Texture>(Localize.MainTranslation);
				if (texture != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						component.material.mainTexture = texture;
						return;
					}
				}
				return;
			}
		}
	}
}
