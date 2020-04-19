using System;

public class NumericHitResultScratch
{
	public int m_damage;

	public int m_healing;

	public int m_energyGain;

	public int m_energyLoss;

	public void Reset()
	{
		this.m_damage = 0;
		this.m_healing = 0;
		this.m_energyGain = 0;
		this.m_energyLoss = 0;
	}
}
