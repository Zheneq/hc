using System;

namespace TMPro
{
	internal interface ITweenValue
	{
		void TweenValue(float floatPercentage);

		bool ignoreTimeScale { get; }

		float duration { get; }

		bool ValidTarget();
	}
}
