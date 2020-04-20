using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkinVfxData : MonoBehaviour
{
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
	public List<MasterSkinVfxData.CharacterToJoint> m_fxJointOverrides;

	private Dictionary<CharacterType, MasterSkinVfxData.VfxSize> m_charToSizeOverrides = new Dictionary<CharacterType, MasterSkinVfxData.VfxSize>();

	private Dictionary<CharacterType, JointPopupProperty> m_charToJointOverrides = new Dictionary<CharacterType, JointPopupProperty>();

	private static MasterSkinVfxData s_instance;

	public static MasterSkinVfxData Get()
	{
		return MasterSkinVfxData.s_instance;
	}

	private void Awake()
	{
		MasterSkinVfxData.s_instance = this;
		if (this.m_smallCharacters != null)
		{
			using (List<CharacterType>.Enumerator enumerator = this.m_smallCharacters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterType key = enumerator.Current;
					if (!this.m_charToSizeOverrides.ContainsKey(key))
					{
						this.m_charToSizeOverrides[key] = MasterSkinVfxData.VfxSize.Small;
					}
				}
			}
		}
		if (this.m_largeCharacters != null)
		{
			using (List<CharacterType>.Enumerator enumerator2 = this.m_largeCharacters.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharacterType key2 = enumerator2.Current;
					if (!this.m_charToSizeOverrides.ContainsKey(key2))
					{
						this.m_charToSizeOverrides[key2] = MasterSkinVfxData.VfxSize.Large;
					}
				}
			}
		}
		if (this.m_fxJointOverrides != null)
		{
			foreach (MasterSkinVfxData.CharacterToJoint characterToJoint in this.m_fxJointOverrides)
			{
				if (characterToJoint.m_characterType.IsValidForHumanGameplay())
				{
					if (!this.m_charToJointOverrides.ContainsKey(characterToJoint.m_characterType))
					{
						this.m_charToJointOverrides[characterToJoint.m_characterType] = characterToJoint.m_joint;
					}
					else
					{
						Log.Warning("MasterSkinVfxData has multiple joint overrides defined for " + characterToJoint.m_characterType, new object[0]);
					}
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (MasterSkinVfxData.s_instance == this)
		{
			MasterSkinVfxData.s_instance = null;
		}
	}

	public float GetVfxScaleValueForCharacter(CharacterType charType)
	{
		MasterSkinVfxData.VfxSize vfxSize = MasterSkinVfxData.VfxSize.Medium;
		if (this.m_charToSizeOverrides.ContainsKey(charType))
		{
			vfxSize = this.m_charToSizeOverrides[charType];
		}
		if (vfxSize == MasterSkinVfxData.VfxSize.Small)
		{
			return this.m_vfxScaleSmall;
		}
		if (vfxSize == MasterSkinVfxData.VfxSize.Medium)
		{
			return this.m_vfxScaleMed;
		}
		return this.m_vfxScaleLarge;
	}

	public GameObject GetVfXPrefabForCharacter(CharacterType charType)
	{
		GameObject result = this.m_masterVfxPrefabMedium;
		if (this.m_charToSizeOverrides.ContainsKey(charType))
		{
			MasterSkinVfxData.VfxSize vfxSize = this.m_charToSizeOverrides[charType];
			if (vfxSize == MasterSkinVfxData.VfxSize.Small)
			{
				result = this.m_masterVfxPrefabSmall;
			}
			else if (vfxSize == MasterSkinVfxData.VfxSize.Large)
			{
				result = this.m_masterVfxPrefabLarge;
			}
		}
		return result;
	}

	public JointPopupProperty GetJointForCharacter(CharacterType charType)
	{
		JointPopupProperty result = this.m_fxJoint;
		if (this.m_charToJointOverrides.ContainsKey(charType))
		{
			result = this.m_charToJointOverrides[charType];
		}
		return result;
	}

	public GameObject AddMasterSkinVfxOnCharacterObject(GameObject characterObj, CharacterType charType, float scaleMult)
	{
		GameObject gameObject = null;
		GameObject vfXPrefabForCharacter = this.GetVfXPrefabForCharacter(charType);
		if (vfXPrefabForCharacter != null)
		{
			JointPopupProperty jointForCharacter = this.GetJointForCharacter(charType);
			jointForCharacter.Initialize(characterObj);
			gameObject = UnityEngine.Object.Instantiate<GameObject>(vfXPrefabForCharacter);
			gameObject.transform.parent = jointForCharacter.m_jointObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			float num = this.GetVfxScaleValueForCharacter(charType);
			num *= scaleMult;
			num = Mathf.Max(0.05f, num);
			Sequence.SetAttribute(gameObject, "scaleControl", num);
		}
		return gameObject;
	}

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
}
