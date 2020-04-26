using System;
using UnityEngine;

[Serializable]
public class JointPopupProperty
{
	public const string JOINT_ACTOR_ANY = "Any";

	public const string JOINT_ACTOR_ANY_HERO = "Any Hero";

	public static string s_defaultJoint = "root_JNT";

	public string m_joint = s_defaultJoint;

	public string m_jointCharacter = "Any Hero";

	internal GameObject m_jointObject;

	internal bool IsInitialized()
	{
		return m_jointObject != null;
	}

	public void Initialize(GameObject obj)
	{
		m_jointObject = FindJointObject(obj);
	}

	public GameObject FindJointObject(GameObject obj)
	{
		GameObject gameObject = obj.FindInChildren(m_joint);
		if (gameObject == null)
		{
			gameObject = obj.FindInChildren(s_defaultJoint);
		}
		return gameObject;
	}
}
