// ROGUES
// SERVER
using System;

[Serializable]
public class MartyrProtectAllyThreshold : MartyrLaserThreshold
{
	public int m_additionalAbsorb = 5;
	public int m_additionalAbsorbOnAlly;
}
