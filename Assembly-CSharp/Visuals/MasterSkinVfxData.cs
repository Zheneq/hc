using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkinVfxData : MonoBehaviour
{
	public enum VfxSize
	{
		Small,
		Medium,
		Large
	}

	[Serializable]
	public class CharacterToJoint
	{
		public CharacterType m_characterType;

		[JointPopup("Fx Attach Joint")]
		public JointPopupProperty m_joint;
	}

	public bool m_addMasterSkinVfx = true;

	[Separator("Master Skin Vfx Prefabs", true)]
	public GameObject m_masterVfxPrefabSmall;

	public GameObject m_masterVfxPrefabMedium;

	public GameObject m_masterVfxPrefabLarge;

	[Separator("Vfx Scales for small, med, and large (attribute name: scaleControl)", true)]
	public float m_vfxScaleSmall = 0.35f;

	public float m_vfxScaleMed = 0.55f;

	public float m_vfxScaleLarge = 1f;

	private const string c_vfxScaleAttribute = "scaleControl";

	[JointPopup("Fx Attach Joint")]
	public JointPopupProperty m_fxJoint;

	[Separator("By default, use medium version of vfx for characters. List explicit override for Small/Large characters in lists below", true)]
	public List<CharacterType> m_smallCharacters;

	public List<CharacterType> m_largeCharacters;

	[Separator("Joint Overrides", true)]
	public List<CharacterToJoint> m_fxJointOverrides;

	private Dictionary<CharacterType, VfxSize> m_charToSizeOverrides = new Dictionary<CharacterType, VfxSize>();

	private Dictionary<CharacterType, JointPopupProperty> m_charToJointOverrides = new Dictionary<CharacterType, JointPopupProperty>();

	private static MasterSkinVfxData s_instance;

	public static MasterSkinVfxData Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		if (m_smallCharacters != null)
		{
			using (List<CharacterType>.Enumerator enumerator = m_smallCharacters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterType current = enumerator.Current;
					if (!m_charToSizeOverrides.ContainsKey(current))
					{
						m_charToSizeOverrides[current] = VfxSize.Small;
					}
				}
			}
		}
		if (m_largeCharacters != null)
		{
			using (List<CharacterType>.Enumerator enumerator2 = m_largeCharacters.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharacterType current2 = enumerator2.Current;
					if (!m_charToSizeOverrides.ContainsKey(current2))
					{
						m_charToSizeOverrides[current2] = VfxSize.Large;
					}
				}
			}
		}
		if (m_fxJointOverrides == null)
		{
			return;
		}
		while (true)
		{
			foreach (CharacterToJoint fxJointOverride in m_fxJointOverrides)
			{
				if (fxJointOverride.m_characterType.IsValidForHumanGameplay())
				{
					if (!m_charToJointOverrides.ContainsKey(fxJointOverride.m_characterType))
					{
						m_charToJointOverrides[fxJointOverride.m_characterType] = fxJointOverride.m_joint;
					}
					else
					{
						Log.Warning("MasterSkinVfxData has multiple joint overrides defined for " + fxJointOverride.m_characterType);
					}
				}
			}
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(s_instance == this))
		{
			return;
		}
		while (true)
		{
			s_instance = null;
			return;
		}
	}

	public float GetVfxScaleValueForCharacter(CharacterType charType)
	{
		VfxSize vfxSize = VfxSize.Medium;
		if (m_charToSizeOverrides.ContainsKey(charType))
		{
			vfxSize = m_charToSizeOverrides[charType];
		}
		switch (vfxSize)
		{
		case VfxSize.Small:
			while (true)
			{
				return m_vfxScaleSmall;
			}
		case VfxSize.Medium:
			return m_vfxScaleMed;
		default:
			return m_vfxScaleLarge;
		}
	}

	public GameObject GetVfXPrefabForCharacter(CharacterType charType)
	{
		GameObject result = m_masterVfxPrefabMedium;
		if (m_charToSizeOverrides.ContainsKey(charType))
		{
			VfxSize vfxSize = m_charToSizeOverrides[charType];
			if (vfxSize == VfxSize.Small)
			{
				result = m_masterVfxPrefabSmall;
			}
			else if (vfxSize == VfxSize.Large)
			{
				result = m_masterVfxPrefabLarge;
			}
		}
		return result;
	}

	public JointPopupProperty GetJointForCharacter(CharacterType charType)
	{
		JointPopupProperty result = m_fxJoint;
		if (m_charToJointOverrides.ContainsKey(charType))
		{
			result = m_charToJointOverrides[charType];
		}
		return result;
	}

	public GameObject AddMasterSkinVfxOnCharacterObject(GameObject characterObj, CharacterType charType, float scaleMult)
	{
		GameObject gameObject = null;
		GameObject vfXPrefabForCharacter = GetVfXPrefabForCharacter(charType);
		if (vfXPrefabForCharacter != null)
		{
			JointPopupProperty jointForCharacter = GetJointForCharacter(charType);
			jointForCharacter.Initialize(characterObj);
			gameObject = UnityEngine.Object.Instantiate(vfXPrefabForCharacter);
			gameObject.transform.parent = jointForCharacter.m_jointObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			float vfxScaleValueForCharacter = GetVfxScaleValueForCharacter(charType);
			vfxScaleValueForCharacter *= scaleMult;
			vfxScaleValueForCharacter = Mathf.Max(0.05f, vfxScaleValueForCharacter);
			Sequence.SetAttribute(gameObject, "scaleControl", vfxScaleValueForCharacter);
		}
		return gameObject;
	}
}
