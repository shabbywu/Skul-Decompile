using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords;

public class LightSwordFieldHelper : MonoBehaviour
{
	private class Interval
	{
		internal float left;

		internal float right;

		internal Interval(float left, float right)
		{
			this.left = left;
			this.right = right;
		}
	}

	[SerializeField]
	private Character _owner;

	[SerializeField]
	[MinMaxSlider(0f, 180f)]
	private Vector2 _fireRange;

	[SerializeField]
	private float _fireDistance;

	[SerializeField]
	private int _intervalCount;

	[SerializeField]
	private LightSwordPool _pool;

	private List<LightSword> _swords;

	private List<Interval> _intervals;

	private Bounds _platform;

	public List<LightSword> swords => _swords;

	public void Fire()
	{
		((MonoBehaviour)this).StartCoroutine(CFire());
	}

	public IEnumerator CFire()
	{
		if (_intervals == null)
		{
			MakeInterval();
		}
		_swords = _pool.Get();
		_platform = _owner.movement.controller.collisionState.lastStandingCollider.bounds;
		int count = _swords.Count - 1;
		int intervalIndex = 0;
		Vector2 destination = default(Vector2);
		int num;
		do
		{
			((Vector2)(ref destination))._002Ector(Random.Range(_intervals[intervalIndex].left, _intervals[intervalIndex].right), ((Bounds)(ref _platform)).max.y);
			float degree = Random.Range(_fireRange.x, _fireRange.y);
			Vector2 source = CalculateFirePosition(destination, degree);
			_swords[count].Fire(_owner, source, destination);
			intervalIndex = (intervalIndex + 1) % _intervalCount;
			yield return Chronometer.global.WaitForSeconds(0.1f);
			num = count - 1;
			count = num;
		}
		while (num >= 0);
	}

	public void Sign(Character owner)
	{
		_swords.ForEach(delegate(LightSword sword)
		{
			if (sword.active)
			{
				sword.Sign();
			}
		});
		((MonoBehaviour)this).StartCoroutine(CDraw(owner));
	}

	private IEnumerator CDraw(Character owner)
	{
		yield return Chronometer.global.WaitForSeconds(0.5f);
		Draw(owner);
	}

	public void Draw(Character owner)
	{
		_swords.ForEach(delegate(LightSword sword)
		{
			if (sword.active)
			{
				sword.Draw(owner);
			}
		});
	}

	public int GetActivatedSwordCount()
	{
		return _swords.Count((LightSword sword) => sword.active);
	}

	public LightSword GetClosestFromPlayer()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		float num = float.PositiveInfinity;
		LightSword lightSword = null;
		if (_swords == null)
		{
			return null;
		}
		foreach (LightSword sword in _swords)
		{
			if (!sword.active)
			{
				continue;
			}
			if ((Object)(object)lightSword == (Object)null)
			{
				lightSword = sword;
				num = Mathf.Abs(((Component)player).transform.position.x - sword.GetStuckPosition().x);
				continue;
			}
			Vector3 stuckPosition = sword.GetStuckPosition();
			float num2 = Mathf.Abs(((Component)player).transform.position.x - stuckPosition.x);
			if (num2 < num)
			{
				lightSword = sword;
				num = num2;
			}
		}
		return lightSword;
	}

	public LightSword GetBehindPlayer()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		float x = ((Component)Singleton<Service>.Instance.levelManager.player).transform.position.x;
		float x2 = ((Component)_owner).transform.position.x;
		LightSword lightSword = null;
		foreach (LightSword sword in _swords)
		{
			if (!sword.active)
			{
				continue;
			}
			float x3 = sword.GetStuckPosition().x;
			float num = x3 - x;
			float num2 = x3 - x2;
			if ((x >= x2 && (num2 < 0f || num < 0f)) || (x <= x2 && (num2 > 0f || num > 0f)))
			{
				continue;
			}
			if ((Object)(object)lightSword == (Object)null)
			{
				lightSword = sword;
				continue;
			}
			float num3 = Mathf.Abs(lightSword.GetStuckPosition().x - x);
			if (Mathf.Abs(num) < num3)
			{
				lightSword = sword;
			}
		}
		return lightSword;
	}

	public LightSword GetFarthestFromHero()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		float x = ((Component)_owner).transform.position.x;
		float num = float.NegativeInfinity;
		LightSword lightSword = null;
		foreach (LightSword sword in _swords)
		{
			if (!sword.active)
			{
				continue;
			}
			if ((Object)(object)lightSword == (Object)null)
			{
				lightSword = sword;
				continue;
			}
			float num2 = Mathf.Abs(sword.GetStuckPosition().x - x);
			if (num2 > num)
			{
				lightSword = sword;
				num = num2;
			}
		}
		return lightSword;
	}

	private void MakeInterval()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		_intervals = new List<Interval>();
		Bounds bounds = _owner.movement.controller.collisionState.lastStandingCollider.bounds;
		float num = 1f;
		float num2 = (((Bounds)(ref bounds)).size.x - num) / (float)_intervalCount;
		float num3 = ((Bounds)(ref bounds)).min.x + num;
		float num4 = num3 + num2;
		for (int i = 0; i < _intervalCount; i++)
		{
			_intervals.Add(new Interval(num3, num4));
			num3 = num4;
			num4 = num3 + num2;
		}
		ExtensionMethods.Shuffle<Interval>((IList<Interval>)_intervals);
	}

	private Vector2 CalculateFirePosition(Vector2 destination, float degree)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.right * _fireDistance;
		float num = degree * ((float)Math.PI / 180f);
		float num2 = val.x * Mathf.Cos(num) - val.y * Mathf.Sin(num);
		float num3 = val.x * Mathf.Sin(num) + val.y * Mathf.Cos(num);
		return new Vector2(num2, num3) + destination;
	}
}
