using System;
using System.Text;
using UnityEngine.Networking;

internal static class GameplayRandom
{
	private static uint m_w;

	private static uint m_z;

	static GameplayRandom()
	{
		m_w = 521288629u;
		m_z = 362436069u;
	}

	internal static void OnSerializeHelper(IBitStream stream)
	{
		uint value = m_w;
		uint value2 = m_z;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		m_w = value;
		m_z = value2;
	}

	internal static void SetSeed(uint u, uint v)
	{
		if (u != 0)
		{
			m_w = u;
		}
		if (v == 0)
		{
			return;
		}
		while (true)
		{
			m_z = v;
			return;
		}
	}

	internal static void SetSeed(uint u)
	{
		m_w = u;
	}

	internal static void SetSeedFromSystemTime()
	{
		long num = DateTime.Now.ToFileTime();
		SetSeed((uint)(num >> 16), (uint)(num % 4294967296L));
	}

	internal static uint GetUint()
	{
		if (!NetworkServer.active)
		{
			Log.Error("GameplayRandom functions should only be called on the server. Results should be sent to clients");
		}
		m_z = 36969 * (m_z & 0xFFFF) + (m_z >> 16);
		m_w = 18000 * (m_w & 0xFFFF) + (m_w >> 16);
		return (m_z << 16) + m_w;
	}

	internal static int Range(int min, int max)
	{
		int num = (int)(GetUint() >> 1);
		int result;
		if (max <= min)
		{
			result = min;
		}
		else
		{
			result = min + num % max;
		}
		return result;
	}

	internal static double GetUniform()
	{
		uint @uint = GetUint();
		return ((double)@uint + 1.0) * 2.3283064354544941E-10;
	}

	internal static double GetNormal()
	{
		double uniform = GetUniform();
		double uniform2 = GetUniform();
		double num = Math.Sqrt(-2.0 * Math.Log(uniform));
		double a = Math.PI * 2.0 * uniform2;
		return num * Math.Sin(a);
	}

	internal static double GetNormal(double mean, double standardDeviation)
	{
		if (standardDeviation <= 0.0)
		{
			string paramName = new StringBuilder().Append("Shape must be positive. Received ").Append(standardDeviation).Append(".").ToString();
			throw new ArgumentOutOfRangeException(paramName);
		}
		return mean + standardDeviation * GetNormal();
	}

	internal static double GetExponential()
	{
		return 0.0 - Math.Log(GetUniform());
	}

	internal static double GetExponential(double mean)
	{
		if (mean <= 0.0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					string paramName = new StringBuilder().Append("Mean must be positive. Received ").Append(mean).Append(".").ToString();
					throw new ArgumentOutOfRangeException(paramName);
				}
				}
			}
		}
		return mean * GetExponential();
	}

	internal static double GetGamma(double shape, double scale)
	{
		if (shape >= 1.0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					double num = shape - 0.33333333333333331;
					double num2 = 1.0 / Math.Sqrt(9.0 * num);
					while (true)
					{
						IL_004e:
						double normal = GetNormal();
						double num3 = 1.0 + num2 * normal;
						if (!(num3 <= 0.0))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
								{
									num3 = num3 * num3 * num3;
									double uniform = GetUniform();
									double num4 = normal * normal;
									if (!(uniform < 1.0 - 0.0331 * num4 * num4))
									{
										if (!(Math.Log(uniform) < 0.5 * num4 + num * (1.0 - num3 + Math.Log(num3))))
										{
											goto IL_004e;
										}
									}
									return scale * num * num3;
								}
								}
							}
						}
					}
				}
				}
			}
		}
		if (shape <= 0.0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					string paramName = new StringBuilder().Append("Shape must be positive. Received ").Append(shape).Append(".").ToString();
					throw new ArgumentOutOfRangeException(paramName);
				}
				}
			}
		}
		double gamma = GetGamma(shape + 1.0, 1.0);
		double uniform2 = GetUniform();
		return scale * gamma * Math.Pow(uniform2, 1.0 / shape);
	}

	internal static double GetChiSquare(double degreesOfFreedom)
	{
		return GetGamma(0.5 * degreesOfFreedom, 2.0);
	}

	internal static double GetInverseGamma(double shape, double scale)
	{
		return 1.0 / GetGamma(shape, 1.0 / scale);
	}

	internal static double GetWeibull(double shape, double scale)
	{
		if (!(shape <= 0.0))
		{
			if (!(scale <= 0.0))
			{
				return scale * Math.Pow(0.0 - Math.Log(GetUniform()), 1.0 / shape);
			}
		}
		string paramName = new StringBuilder().Append("Shape and scale parameters must be positive. Recieved shape ").Append(shape).Append(" and scale").Append(scale).Append(".").ToString();
		throw new ArgumentOutOfRangeException(paramName);
	}

	internal static double GetCauchy(double median, double scale)
	{
		if (scale <= 0.0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					string message = new StringBuilder().Append("Scale must be positive. Received ").Append(scale).Append(".").ToString();
					throw new ArgumentException(message);
				}
				}
			}
		}
		double uniform = GetUniform();
		return median + scale * Math.Tan(Math.PI * (uniform - 0.5));
	}

	internal static double GetStudentT(double degreesOfFreedom)
	{
		if (degreesOfFreedom <= 0.0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					string message = new StringBuilder().Append("Degrees of freedom must be positive. Received ").Append(degreesOfFreedom).Append(".").ToString();
					throw new ArgumentException(message);
				}
				}
			}
		}
		double normal = GetNormal();
		double chiSquare = GetChiSquare(degreesOfFreedom);
		return normal / Math.Sqrt(chiSquare / degreesOfFreedom);
	}

	internal static double GetLaplace(double mean, double scale)
	{
		double uniform = GetUniform();
		double result;
		if (uniform < 0.5)
		{
			result = mean + scale * Math.Log(2.0 * uniform);
		}
		else
		{
			result = mean - scale * Math.Log(2.0 * (1.0 - uniform));
		}
		return result;
	}

	internal static double GetLogNormal(double mu, double sigma)
	{
		return Math.Exp(GetNormal(mu, sigma));
	}

	internal static double GetBeta(double a, double b)
	{
		if (!(a <= 0.0))
		{
			if (!(b <= 0.0))
			{
				double gamma = GetGamma(a, 1.0);
				double gamma2 = GetGamma(b, 1.0);
				return gamma / (gamma + gamma2);
			}
		}
		string paramName = new StringBuilder().Append("Beta parameters must be positive. Received ").Append(a).Append(" and ").Append(b).Append(".").ToString();
		throw new ArgumentOutOfRangeException(paramName);
	}
}
