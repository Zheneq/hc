using System;
using UnityEngine.Networking;

internal static class GameplayRandom
{
	private static uint m_w = 0x1F123BB5U;

	private static uint m_z = 0x159A55E5U;

	internal static void OnSerializeHelper(IBitStream stream)
	{
		uint w = GameplayRandom.m_w;
		uint z = GameplayRandom.m_z;
		stream.Serialize(ref w);
		stream.Serialize(ref z);
		GameplayRandom.m_w = w;
		GameplayRandom.m_z = z;
	}

	internal static void SetSeed(uint u, uint v)
	{
		if (u != 0U)
		{
			GameplayRandom.m_w = u;
		}
		if (v != 0U)
		{
			GameplayRandom.m_z = v;
		}
	}

	internal static void SetSeed(uint u)
	{
		GameplayRandom.m_w = u;
	}

	internal static void SetSeedFromSystemTime()
	{
		long num = DateTime.Now.ToFileTime();
		GameplayRandom.SetSeed((uint)(num >> 0x10), (uint)(num % 0x100000000L));
	}

	internal static uint GetUint()
	{
		if (!NetworkServer.active)
		{
			Log.Error("GameplayRandom functions should only be called on the server. Results should be sent to clients", new object[0]);
		}
		GameplayRandom.m_z = 0x9069U * (GameplayRandom.m_z & 0xFFFFU) + (GameplayRandom.m_z >> 0x10);
		GameplayRandom.m_w = 0x4650U * (GameplayRandom.m_w & 0xFFFFU) + (GameplayRandom.m_w >> 0x10);
		return (GameplayRandom.m_z << 0x10) + GameplayRandom.m_w;
	}

	internal static int Range(int min, int max)
	{
		int num = (int)(GameplayRandom.GetUint() >> 1);
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
		uint @uint = GameplayRandom.GetUint();
		return (@uint + 1.0) * 2.3283064354544941E-10;
	}

	internal static double GetNormal()
	{
		double uniform = GameplayRandom.GetUniform();
		double uniform2 = GameplayRandom.GetUniform();
		double num = Math.Sqrt(-2.0 * Math.Log(uniform));
		double a = 6.2831853071795862 * uniform2;
		return num * Math.Sin(a);
	}

	internal static double GetNormal(double mean, double standardDeviation)
	{
		if (standardDeviation <= 0.0)
		{
			string paramName = string.Format("Shape must be positive. Received {0}.", standardDeviation);
			throw new ArgumentOutOfRangeException(paramName);
		}
		return mean + standardDeviation * GameplayRandom.GetNormal();
	}

	internal static double GetExponential()
	{
		return -Math.Log(GameplayRandom.GetUniform());
	}

	internal static double GetExponential(double mean)
	{
		if (mean <= 0.0)
		{
			string paramName = string.Format("Mean must be positive. Received {0}.", mean);
			throw new ArgumentOutOfRangeException(paramName);
		}
		return mean * GameplayRandom.GetExponential();
	}

	internal static double GetGamma(double shape, double scale)
	{
		if (shape >= 1.0)
		{
			double num = shape - 0.33333333333333331;
			double num2 = 1.0 / Math.Sqrt(9.0 * num);
			double num3;
			double uniform;
			double num4;
			do
			{
				double normal;
				do
				{
					normal = GameplayRandom.GetNormal();
					num3 = 1.0 + num2 * normal;
				}
				while (num3 <= 0.0);
				num3 = num3 * num3 * num3;
				uniform = GameplayRandom.GetUniform();
				num4 = normal * normal;
				if (uniform < 1.0 - 0.0331 * num4 * num4)
				{
					goto IL_E5;
				}
			}
			while (Math.Log(uniform) >= 0.5 * num4 + num * (1.0 - num3 + Math.Log(num3)));
			IL_E5:
			return scale * num * num3;
		}
		if (shape <= 0.0)
		{
			string paramName = string.Format("Shape must be positive. Received {0}.", shape);
			throw new ArgumentOutOfRangeException(paramName);
		}
		double gamma = GameplayRandom.GetGamma(shape + 1.0, 1.0);
		double uniform2 = GameplayRandom.GetUniform();
		return scale * gamma * Math.Pow(uniform2, 1.0 / shape);
	}

	internal static double GetChiSquare(double degreesOfFreedom)
	{
		return GameplayRandom.GetGamma(0.5 * degreesOfFreedom, 2.0);
	}

	internal static double GetInverseGamma(double shape, double scale)
	{
		return 1.0 / GameplayRandom.GetGamma(shape, 1.0 / scale);
	}

	internal static double GetWeibull(double shape, double scale)
	{
		if (shape > 0.0)
		{
			if (scale > 0.0)
			{
				return scale * Math.Pow(-Math.Log(GameplayRandom.GetUniform()), 1.0 / shape);
			}
		}
		string paramName = string.Format("Shape and scale parameters must be positive. Recieved shape {0} and scale{1}.", shape, scale);
		throw new ArgumentOutOfRangeException(paramName);
	}

	internal static double GetCauchy(double median, double scale)
	{
		if (scale <= 0.0)
		{
			string message = string.Format("Scale must be positive. Received {0}.", scale);
			throw new ArgumentException(message);
		}
		double uniform = GameplayRandom.GetUniform();
		return median + scale * Math.Tan(3.1415926535897931 * (uniform - 0.5));
	}

	internal static double GetStudentT(double degreesOfFreedom)
	{
		if (degreesOfFreedom <= 0.0)
		{
			string message = string.Format("Degrees of freedom must be positive. Received {0}.", degreesOfFreedom);
			throw new ArgumentException(message);
		}
		double normal = GameplayRandom.GetNormal();
		double chiSquare = GameplayRandom.GetChiSquare(degreesOfFreedom);
		return normal / Math.Sqrt(chiSquare / degreesOfFreedom);
	}

	internal static double GetLaplace(double mean, double scale)
	{
		double uniform = GameplayRandom.GetUniform();
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
		return Math.Exp(GameplayRandom.GetNormal(mu, sigma));
	}

	internal static double GetBeta(double a, double b)
	{
		if (a > 0.0)
		{
			if (b > 0.0)
			{
				double gamma = GameplayRandom.GetGamma(a, 1.0);
				double gamma2 = GameplayRandom.GetGamma(b, 1.0);
				return gamma / (gamma + gamma2);
			}
		}
		string paramName = string.Format("Beta parameters must be positive. Received {0} and {1}.", a, b);
		throw new ArgumentOutOfRangeException(paramName);
	}
}
