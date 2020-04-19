using System;
using UnityEngine;

[Serializable]
public class JointPopupProperty
{
	public const string JOINT_ACTOR_ANY = "Any";

	public const string JOINT_ACTOR_ANY_HERO = "Any Hero";

	public static string s_defaultJoint = "root_JNT";

	public string m_joint = JointPopupProperty.s_defaultJoint;

	public string m_jointCharacter = "Any Hero";

	internal GameObject m_jointObject;

	internal bool IsInitialized()
	{
		return this.m_jointObject != null;
	}

	public void Initialize(GameObject obj)
	{
		this.m_jointObject = this.FindJointObject(obj);
	}

	public GameObject FindJointObject(GameObject obj)
	{
		GameObject gameObject = obj.FindInChildren(this.m_joint, 0);
		if (gameObject == null)
		{
			gameObject = obj.FindInChildren(JointPopupProperty.s_defaultJoint, 0);
		}
		return gameObject;
	}
}
