using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class IntRef { public int Value; }
public class FloatRef { public float Value; }

public static class Util
{
	public static void CalculatePossibilities<T>(List<T> FullList, Func<T, int> Getter, Action<T, float> MinSetter, Action<T, float> MaxSetter)
	{
		int TotalPossibility = FullList.Sum(Getter);
		float LastPossibility = 0f;
		foreach (T OneItem in FullList)
		{
			float Possibility = Getter(OneItem) / (float)TotalPossibility;
			MinSetter(OneItem, LastPossibility);
			MaxSetter(OneItem, LastPossibility + Possibility);
			LastPossibility += Possibility;
		}
	}

	public static T GetWeightedItem<T>(List<T> FullList, Func<T, float> MinGetter, Func<T, float> MaxGetter)
	{
		float RandomValue = Random.value;
		foreach (T OneItem in FullList)
		{
			float MinValue = MinGetter(OneItem);
			float MaxValue = MaxGetter(OneItem);
			if (RandomValue >= MinValue && RandomValue <= MaxValue)
			{
				return OneItem;
			}
		}
		return default;
	}

	public static float Mod(float a, float b)
	{
		return (a % b + b) % b;
	}
}