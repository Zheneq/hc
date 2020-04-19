using System;
using Newtonsoft.Json;

[Serializable]
public class LeakyBucket
{
	private double m_points;

	private Rate m_leakRate;

	private DateTime m_lastUpdate;

	private TimeSpan m_timeOffset;

	public LeakyBucket(double maxPoints, TimeSpan leakPeriod) : this(new Rate(maxPoints, leakPeriod))
	{
	}

	public LeakyBucket() : this(new Rate(0.0, TimeSpan.Zero))
	{
	}

	public LeakyBucket(Rate rate)
	{
		this.m_leakRate = rate;
		this.Reset();
	}

	public void Reset()
	{
		this.m_points = 0.0;
		this.m_lastUpdate = this.Now;
		this.m_timeOffset = TimeSpan.Zero;
	}

	public double CurrentPoints
	{
		get
		{
			return Math.Max(0.0, this.m_points - this.Elapsed.TotalSeconds * this.m_leakRate.AmountPerSecond);
		}
		set
		{
			this.m_points = value;
			this.m_lastUpdate = this.Now;
		}
	}

	[JsonIgnore]
	public double MaxPoints
	{
		get
		{
			return this.m_leakRate.Amount;
		}
	}

	[JsonIgnore]
	public TimeSpan LeakPeriod
	{
		get
		{
			return this.m_leakRate.Period;
		}
	}

	public Rate LeakRate
	{
		get
		{
			return this.m_leakRate;
		}
		set
		{
			this.m_leakRate = value;
		}
	}

	public TimeSpan TimeOffset
	{
		get
		{
			return this.m_timeOffset;
		}
		set
		{
			this.m_timeOffset = value;
		}
	}

	public DateTime LastUpdate
	{
		get
		{
			return this.m_lastUpdate;
		}
		set
		{
			this.m_lastUpdate = value;
		}
	}

	public bool TryAdd(double points = 1.0)
	{
		bool result = false;
		if (this.CanAdd(points))
		{
			this.Add(points);
			result = true;
		}
		return result;
	}

	public void Add(double points = 1.0)
	{
		this.Update();
		this.m_points += points;
		this.m_lastUpdate = this.Now;
	}

	public bool CanAdd(double points = 1.0)
	{
		return this.CurrentPoints + points <= this.MaxPoints;
	}

	public void Update()
	{
		this.m_points = this.CurrentPoints;
		this.m_lastUpdate = this.Now;
	}

	private DateTime Now
	{
		get
		{
			return DateTime.UtcNow + this.m_timeOffset;
		}
	}

	private TimeSpan Elapsed
	{
		get
		{
			DateTime now = this.Now;
			TimeSpan result;
			if (now > this.m_lastUpdate)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LeakyBucket.get_Elapsed()).MethodHandle;
				}
				result = this.Now - this.m_lastUpdate;
			}
			else
			{
				result = TimeSpan.Zero;
			}
			return result;
		}
	}

	public TimeSpan Predict(double points = 1.0)
	{
		this.Update();
		if (this.CurrentPoints + points <= this.MaxPoints)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LeakyBucket.Predict(double)).MethodHandle;
			}
			return TimeSpan.Zero;
		}
		return TimeSpan.FromSeconds((this.CurrentPoints + points - this.MaxPoints) / this.m_leakRate.AmountPerSecond);
	}
}
