using System;
using System.Linq;
using Characters.Abilities.Constraints;
using Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities.Customs;

[Serializable]
public class MummyGunDropPassive : Ability, IAbilityInstance
{
	[Serializable]
	private class DroppedGuns : ReorderableArray<DroppedGuns.Property>
	{
		[Serializable]
		internal class Property
		{
			[SerializeField]
			private float _weight;

			[SerializeField]
			private DroppedMummyGun _droppedGun;

			public float weight => _weight;

			public DroppedMummyGun droppedGun => _droppedGun;
		}
	}

	[Constraint.Subcomponent]
	[SerializeField]
	private Constraint.Subcomponents _constraints;

	[SerializeField]
	private MummyPassiveComponent _mummyPassive;

	[FormerlySerializedAs("_possibility")]
	[SerializeField]
	[Range(1f, 100f)]
	private int _gunDropPossibilityByKill;

	[Header("Supply")]
	[Space]
	[SerializeField]
	[FormerlySerializedAs("_supply")]
	private DroppedMummyGunSupply _supplyPrefab;

	[SerializeField]
	[Information("보급 주기, 0이면 보급되지 않습니다.", InformationAttribute.InformationType.Info, false)]
	private float _supplyInterval;

	private float _remainSupplyTime;

	[SerializeField]
	private CustomFloat _supplyWidth = new CustomFloat(-3f, 5f);

	[SerializeField]
	private CustomFloat _supplyHeight = new CustomFloat(6.5f, 7.5f);

	[Header("Weights")]
	[SerializeField]
	[FormerlySerializedAs("_guns")]
	private DroppedGuns _gunsByKill;

	[SerializeField]
	private DroppedGuns _gunsByPeriodicSupply;

	[SerializeField]
	private DroppedGuns _gunsBySwapSupply;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => _defaultIcon;

	public float iconFillAmount
	{
		get
		{
			if (_supplyInterval != 0f)
			{
				return 1f - _remainSupplyTime / _supplyInterval;
			}
			return 0f;
		}
	}

	public int iconStacks => 0;

	public bool expired => false;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	public void SupplyGunBySwap()
	{
		SupplyGun(_gunsBySwapSupply);
	}

	public void UpdateTime(float deltaTime)
	{
		if (_supplyInterval != 0f && _constraints.Pass())
		{
			_remainSupplyTime -= deltaTime;
			if (!(_remainSupplyTime > 0f))
			{
				_remainSupplyTime = _supplyInterval;
				SupplyGun(_gunsByPeriodicSupply);
			}
		}
	}

	private bool IsInTerrain(Vector3 position)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		position.y += 0.5f;
		return !Object.op_Implicit((Object)(object)Physics2D.OverlapPoint(Vector2.op_Implicit(position), LayerMask.op_Implicit(Layers.terrainMask)));
	}

	private void SupplyGun(DroppedGuns guns)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)owner).transform.position;
		Collider2D lastStandingCollider = owner.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider != (Object)null)
		{
			Bounds bounds = lastStandingCollider.bounds;
			val.y = ((Bounds)(ref bounds)).max.y;
		}
		Vector3 one = Vector3.one;
		for (int i = 0; i < 10; i++)
		{
			one.x = _supplyWidth.value;
			RaycastHit2D val2 = Physics2D.Raycast(Vector2.op_Implicit(val + one), Vector2.down, 5f, LayerMask.op_Implicit(Layers.groundMask));
			if (RaycastHit2D.op_Implicit(val2) && IsInTerrain(Vector2.op_Implicit(((RaycastHit2D)(ref val2)).point)))
			{
				val = Vector2.op_Implicit(((RaycastHit2D)(ref val2)).point);
				break;
			}
		}
		float y = val.y;
		val.y += _supplyHeight.value;
		DroppedMummyGun droppedMummyGun = DropGun(guns, Vector3.one);
		if (!((Object)(object)droppedMummyGun == (Object)null))
		{
			_supplyPrefab.Spawn(droppedMummyGun, val, y);
		}
	}

	private void OnOwnerKilled(ITarget target, ref Damage damage)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target.character == (Object)null) && target.character.type != Character.Type.Dummy && target.character.type != Character.Type.Trap && MMMaths.PercentChance(_gunDropPossibilityByKill))
		{
			DropGun(_gunsByKill, Vector2.op_Implicit(damage.hitPoint));
		}
	}

	private DroppedMummyGun GetRandomGun(DroppedGuns guns)
	{
		DroppedGuns.Property[] values = guns.values;
		float num = Random.Range(0f, values.Sum((DroppedGuns.Property a) => a.weight));
		for (int i = 0; i < values.Length; i++)
		{
			num -= values[i].weight;
			if (num <= 0f)
			{
				return values[i].droppedGun;
			}
		}
		return null;
	}

	private DroppedMummyGun DropGun(DroppedGuns guns, Vector3 position)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		DroppedMummyGun randomGun = GetRandomGun(guns);
		if ((Object)(object)randomGun == (Object)null)
		{
			return null;
		}
		return randomGun.Spawn(position, _mummyPassive.baseAbility);
	}

	public void Attach()
	{
		remainTime = 0f;
		Character character = owner;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(OnOwnerKilled));
	}

	public void Detach()
	{
		Character character = owner;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnOwnerKilled));
	}

	public void Refresh()
	{
	}
}
