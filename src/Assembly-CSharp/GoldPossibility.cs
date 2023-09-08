using System;
using Services;
using Singletons;
using UnityEngine;

[Serializable]
public class GoldPossibility
{
	[SerializeField]
	[Range(0f, 100f)]
	private float _possibility;

	[SerializeField]
	private int _min;

	[SerializeField]
	private int _max;

	[SerializeField]
	private int _goldAmountPerCoin;

	public bool Drop(Vector3 position)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!MMMaths.Chance(_possibility / 100f))
		{
			return false;
		}
		int num = Random.Range(_min, _max);
		Singleton<Service>.Instance.levelManager.DropGold(num, Mathf.Max(num / _goldAmountPerCoin, 1), position);
		return true;
	}
}
