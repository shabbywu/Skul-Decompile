using System;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class GhoulPassive : Ability, IAbilityInstance
{
	[SerializeField]
	[Space]
	[Range(1f, 100f)]
	private int _possibility;

	[SerializeField]
	private DroppedGhoulFlesh _fleshPrefab;

	[Header("Consume")]
	[SerializeField]
	private string _consumeKey = "consume";

	[SerializeField]
	[Tooltip("교대기로 킬할 경우 스택이 쌓이면서 힐할 양")]
	private float _healByConsumeKill;

	[SerializeField]
	[Header("Stat")]
	private int _maxStack;

	[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
	[SerializeField]
	private bool _refreshRemainTime = true;

	[SerializeField]
	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	private float _iconStacksPerStack = 1f;

	[SerializeField]
	private Stat.Values _statPerStack;

	private int _stacks;

	private Stat.Values _stat;

	private Vector2 _spawnOffset;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon
	{
		get
		{
			if (_stacks <= 0)
			{
				return null;
			}
			return _defaultIcon;
		}
	}

	public float iconFillAmount => 1f - remainTime / base.duration;

	public int iconStacks => (int)((float)_stacks * _iconStacksPerStack);

	public bool expired => false;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		Bounds bounds = ((Component)_fleshPrefab).GetComponent<Collider2D>().bounds;
		Vector3 size = ((Bounds)(ref bounds)).size;
		_spawnOffset.y = size.y * 0.6f;
		return this;
	}

	public void UpdateTime(float deltaTime)
	{
		if (_stacks != 0)
		{
			remainTime -= deltaTime;
			if (!(remainTime > 0f))
			{
				_stacks = 0;
				UpdateStack();
			}
		}
	}

	private void OnOwnerKilled(ITarget target, ref Damage damage)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target.character == (Object)null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
		{
			return;
		}
		if (damage.key.Equals(_consumeKey, StringComparison.OrdinalIgnoreCase))
		{
			AddStack();
			if (_healByConsumeKill > 0f)
			{
				owner.health.Heal(_healByConsumeKill);
			}
		}
		else if (MMMaths.PercentChance(_possibility))
		{
			_fleshPrefab.Spawn(Vector2.op_Implicit(damage.hitPoint + _spawnOffset), this);
		}
	}

	public void Attach()
	{
		remainTime = base.duration;
		Character character = owner;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(OnOwnerKilled));
		_stat = _statPerStack.Clone();
		_stacks = 0;
		owner.stat.AttachValues(_stat);
		UpdateStack();
	}

	public void Detach()
	{
		Character character = owner;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnOwnerKilled));
		owner.stat.DetachValues(_stat);
	}

	public void Refresh()
	{
		AddStack();
	}

	public void AddStack()
	{
		if (_refreshRemainTime)
		{
			remainTime = base.duration;
		}
		if (_stacks < _maxStack)
		{
			_stacks++;
		}
		UpdateStack();
	}

	private void UpdateStack()
	{
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)_statPerStack).values[i].GetStackedValue(_stacks);
		}
		owner.stat.SetNeedUpdate();
	}
}
