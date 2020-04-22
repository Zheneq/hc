using Newtonsoft.Json;
using System;

[Serializable]
public class LeakyBucket
{
	private double m_points;

	private Rate m_leakRate;

	private DateTime m_lastUpdate;

	private TimeSpan m_timeOffset;

	public double CurrentPoints
	{
		get
		{
			return Math.Max(0.0, m_points - Elapsed.TotalSeconds * m_leakRate.AmountPerSecond);
		}
		set
		{
			m_points = value;
			m_lastUpdate = Now;
		}
	}

	[JsonIgnore]
	public double MaxPoints => m_leakRate.Amount;

	[JsonIgnore]
	public TimeSpan LeakPeriod => m_leakRate.Period;

	public Rate LeakRate
	{
		get
		{
			return m_leakRate;
		}
		set
		{
			m_leakRate = value;
		}
	}

	public TimeSpan TimeOffset
	{
		get
		{
			return m_timeOffset;
		}
		set
		{
			m_timeOffset = value;
		}
	}

	public DateTime LastUpdate
	{
		get
		{
			return m_lastUpdate;
		}
		set
		{
			m_lastUpdate = value;
		}
	}

	private DateTime Now => DateTime.UtcNow + m_timeOffset;

	private TimeSpan Elapsed
	{
		get
		{
			DateTime now = Now;
			TimeSpan result;
			if (now > m_lastUpdate)
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
				result = Now - m_lastUpdate;
			}
			else
			{
				result = TimeSpan.Zero;
			}
			return result;
		}
	}

	public LeakyBucket(double maxPoints, TimeSpan leakPeriod)
		: this(new Rate(maxPoints, leakPeriod))
	{
	}

	public LeakyBucket()
		: this(new Rate(0.0, TimeSpan.Zero))
	{
	}

	public LeakyBucket(Rate rate)
	{
		m_leakRate = rate;
		Reset();
	}

	public void Reset()
	{
		m_points = 0.0;
		m_lastUpdate = Now;
		m_timeOffset = TimeSpan.Zero;
	}

	public bool TryAdd(double points = 1.0)
	{
		bool result = false;
		if (CanAdd(points))
		{
			Add(points);
			result = true;
		}
		return result;
	}

	public void Add(double points = 1.0)
	{
		Update();
		m_points += points;
		m_lastUpdate = Now;
	}

	public bool CanAdd(double points = 1.0)
	{
		return CurrentPoints + points <= MaxPoints;
	}

	public void Update()
	{
		m_points = CurrentPoints;
		m_lastUpdate = Now;
	}

	public TimeSpan Predict(double points = 1.0)
	{
		Update();
		if (CurrentPoints + points <= MaxPoints)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return TimeSpan.Zero;
				}
			}
		}
		return TimeSpan.FromSeconds((CurrentPoints + points - MaxPoints) / m_leakRate.AmountPerSecond);
	}
}
