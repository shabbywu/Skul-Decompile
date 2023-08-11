using System.Collections.Generic;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public sealed class AbyssShadowPool : MonoBehaviour
{
	[SerializeField]
	private Quintessence _quintessence;

	[SerializeField]
	private int _maxCount = 30;

	[SerializeField]
	private AbyssShadow _abyssShadowPrefab;

	private Queue<AbyssShadow> _pool;

	private HashSet<AbyssShadow> _actives;

	private void Awake()
	{
		Load();
	}

	private void Load()
	{
		_pool = new Queue<AbyssShadow>(_maxCount);
		_actives = new HashSet<AbyssShadow>();
		for (int i = 0; i < _maxCount; i++)
		{
			AbyssShadow abyssShadow = Object.Instantiate<AbyssShadow>(_abyssShadowPrefab);
			abyssShadow.Initialize(this);
			((Component)abyssShadow).gameObject.SetActive(false);
			_quintessence.onDiscard += delegate
			{
				Object.Destroy((Object)(object)((Component)abyssShadow).gameObject);
			};
			_pool.Enqueue(abyssShadow);
		}
	}

	public void Push(AbyssShadow abyssShadow)
	{
		_actives.Remove(abyssShadow);
		if (!_pool.Contains(abyssShadow))
		{
			((Component)abyssShadow).transform.SetParent(((Component)this).transform);
			_pool.Enqueue(abyssShadow);
		}
	}

	public void PopAndSpawn(Transform point)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		AbyssShadow abyssShadow = _pool.Dequeue();
		abyssShadow.Spawn(Vector2.op_Implicit(point.position));
		_actives.Add(abyssShadow);
	}

	public void Bomb()
	{
		foreach (AbyssShadow active in _actives)
		{
			active.Bomb();
		}
	}
}
