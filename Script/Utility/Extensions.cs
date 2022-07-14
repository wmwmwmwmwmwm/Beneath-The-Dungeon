using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static Vector3 WithX(this Vector3 v, float x)
	{
		v.x = x;
		return v;
	}

	public static Vector3 WithY(this Vector3 v, float y)
	{
		v.y = y;
		return v;
	}

	public static Vector3 WithZ(this Vector3 v, float z)
	{
		v.z = z;
		return v;
	}

	public static Color WithAlpha(this Color color, float alpha)
	{
		color.a = alpha;
		return color;
	}

	public static Vector2 WithX(this Vector2 v, float x)
	{
		v.x = x;
		return v;
	}

	public static Vector2 WithY(this Vector2 v, float y)
	{
		v.y = y;
		return v;
	}

	public static T PickOne<T>(this List<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}

	public static void ShuffleList<T>(this List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			int RandomIndex = Random.Range(0, list.Count);
			T temp = list[i];
			list[i] = list[RandomIndex];
			list[RandomIndex] = temp;
		}
	}

	public static List<T> SampleList<T>(this List<T> list, int SampleCount)
	{
		List<T> DuplicateList = new List<T>(list);
		ShuffleList(DuplicateList);
		if (DuplicateList.Count <= SampleCount)
			return DuplicateList;
		DuplicateList.RemoveRange(SampleCount, DuplicateList.Count - SampleCount);
		return DuplicateList;
	}

	public static void DestroyChildren(this Transform tr)
	{
		foreach (Transform child in tr)
		{
			Object.Destroy(child.gameObject);
		}
	}

	public static string GetSignedText(this int i)
	{
		if (i > 0) return string.Format("+ {0}", Mathf.Abs(i).ToString());
		else if (i < 0) return string.Format("- {0}", Mathf.Abs(i).ToString());
		else return i.ToString();
	}

	public static float ForceParseToFloat(this string str)
	{
		if (float.TryParse(str, out float resultFloat)) return resultFloat;
		else if (int.TryParse(str, out int resultInt)) return resultInt;
		else
		{
			Debug.LogWarning($"{str} 오타");
			return 0f;
		}
	}

	public static string ToPercentString(this float f)
	{
		int Percent = (int)(f * 100f);
		return string.Format("{0} %", Percent);
	}

	public static void TryAndAddDictionaryList<TKey, T>(this Dictionary<TKey, List<T>> dict, TKey key, T item)
	{
		if (!dict.ContainsKey(key)) dict.Add(key, new List<T>());
		dict[key].Add(item);
	}
}
