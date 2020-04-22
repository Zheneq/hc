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
						while (true)
						{
							switch (4)
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
						m_charToSizeOverrides[current] = VfxSize.Small;
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (m_largeCharacters != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			using (List<CharacterType>.Enumerator enumerator2 = m_largeCharacters.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharacterType current2 = enumerator2.Current;
					if (!m_charToSizeOverrides.ContainsKey(current2))
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
						m_charToSizeOverrides[current2] = VfxSize.Large;
					}
				}
				while (true)
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
		if (m_fxJointOverrides == null)
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
			foreach (CharacterToJoint fxJointOverride in m_fxJointOverrides)
			{
				if (fxJointOverride.m_characterType.IsValidForHumanGameplay())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_charToJointOverrides.ContainsKey(fxJointOverride.m_characterType))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
			while (true)
			{
				switch (2)
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
			VfxSize vfxSize = m_charToSizeOverrides[charType];
			if (vfxSize == VfxSize.Small)
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
			while (true)
			{
				switch (5)
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
			while (true)
			{
				switch (7)
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
