using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Gear.Weapons;

public sealed class GraveDiggerGraveContainer : MonoBehaviour
{
	private Character _owner;

	private List<GraveDiggerGrave> _graves;

	[SerializeField]
	private Collider2D _graveActivatingRange;

	public bool hasActivatedGrave;

	private void Awake()
	{
		_owner = Singleton<Service>.Instance.levelManager.player;
		_graves = new List<GraveDiggerGrave>();
		Singleton<Service>.Instance.levelManager.onMapLoaded += Clear;
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= Clear;
		}
	}

	public void Clear()
	{
		foreach (GraveDiggerGrave grafe in _graves)
		{
			grafe.Despawn();
		}
		_graves.Clear();
	}

	private void Update()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		hasActivatedGrave = false;
		foreach (GraveDiggerGrave grafe in _graves)
		{
			if (_graveActivatingRange.OverlapPoint(Vector2.op_Implicit(grafe.position)))
			{
				grafe.Activate();
				hasActivatedGrave = true;
			}
			else
			{
				grafe.Deactivate();
			}
		}
	}

	public void Add(GraveDiggerGrave grave)
	{
		_graves.Add(grave);
	}

	public bool Remove(GraveDiggerGrave grave)
	{
		return _graves.Remove(grave);
	}

	public void SummonMinionFromGraves(Minion minionPrefab)
	{
		SummonMinionFromGraves(minionPrefab, int.MaxValue);
	}

	public void SummonMinionFromGraves(Minion minionPrefab, int maxCount)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (maxCount <= 0)
		{
			return;
		}
		_graves.Sort(delegate(GraveDiggerGrave a, GraveDiggerGrave b)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			float x = ((Component)_owner).transform.position.x;
			float num2 = math.abs(x - a.position.x);
			float num3 = math.abs(x - b.position.x);
			return (num2 < num3) ? 1 : (-1);
		});
		int count = _graves.Count;
		for (int i = 0; i < maxCount; i++)
		{
			int num = count - i - 1;
			if (num >= 0)
			{
				GraveDiggerGrave graveDiggerGrave = _graves[num];
				if (!graveDiggerGrave.activated)
				{
					maxCount++;
					continue;
				}
				_owner.playerComponents.minionLeader.Summon(minionPrefab, ((Component)graveDiggerGrave).transform.position, null);
				graveDiggerGrave.Despawn();
				_graves.RemoveAt(num);
				continue;
			}
			break;
		}
	}
}
