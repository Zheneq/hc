using System;

[Serializable]
public class MartyrSacrificeThreshold : MartyrLaserThreshold
{
	public int m_additionalHealToAlly = 5;
	public int m_additionalDamageToEnemy = 5;
	public int m_additionalDamageToSelf = 2;
}
