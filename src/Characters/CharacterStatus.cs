using System;
using Characters.Abilities.Statuses;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters;

public class CharacterStatus : MonoBehaviour
{
	public enum Timing
	{
		Apply,
		Refresh,
		Release
	}

	private class Grades
	{
		internal delegate void OnChanged(int old, int @new);

		private readonly int[] _grades;

		internal int max { get; private set; }

		internal event OnChanged onChanged;

		internal Grades(int maxGrade)
		{
			_grades = new int[maxGrade];
			max = -1;
		}

		internal void Attach(int grade)
		{
			_grades[grade]++;
			if (grade > max)
			{
				this.onChanged?.Invoke(max + 1, grade + 1);
				max = grade;
			}
		}

		internal void Detach(int grade)
		{
			_grades[grade]--;
			if (grade != max || _grades[grade] != 0)
			{
				return;
			}
			for (int num = grade - 1; num >= 0; num--)
			{
				if (_grades[num] > 0)
				{
					this.onChanged?.Invoke(max + 1, num + 1);
					max = num;
					return;
				}
			}
			this.onChanged?.Invoke(max + 1, 0);
			max = -1;
		}
	}

	private abstract class Status
	{
		protected readonly CharacterStatus _characterStatus;

		protected readonly Character _character;

		protected internal int currentGrade { get; protected set; }

		internal Status(CharacterStatus characterStatus)
		{
			_characterStatus = characterStatus;
			_character = characterStatus._character;
		}

		internal abstract void Apply(int grade);

		internal abstract void Stop();
	}

	[Serializable]
	public class ApplyInfo
	{
		public Kind kind;

		public ApplyInfo(Kind kind)
		{
			this.kind = kind;
		}
	}

	public enum Kind
	{
		Stun,
		Freeze,
		Burn,
		Wound,
		Poison,
		Unmoving
	}

	public delegate void OnTimeDelegate(Character attacker, Character target);

	public delegate void OnApplyDelegate(Character attacker, Character target, ApplyInfo applyInfo);

	public static readonly string AttackKeyBurn = "burn";

	public static readonly string AttackKeyEmber = "ember";

	public static readonly string AttackKeyPoison = "poison";

	public static readonly string AttackKeyFreeze = "freeze";

	public static readonly string AttackKeyBleed = "bleed";

	public readonly SumInt gradeBonuses = new SumInt(0);

	public readonly TrueOnlyLogicalSumList unstoppable = new TrueOnlyLogicalSumList(false);

	private const string _unstoppableFloatingTextKey = "floating/unstoppable";

	private const string _unstoppableFloatingTextColor = "#a3a3a3";

	[GetComponent]
	[SerializeField]
	private Character _character;

	private readonly EnumArray<Kind, Status> _statuses = new EnumArray<Kind, Status>();

	private int _freezeMaxStack = 1;

	public int this[Kind kind] => _statuses[kind].currentGrade;

	public bool stuned => !stun.expired;

	public bool freezed => !freeze.expired;

	public bool burning => !burn.expired;

	public bool wounded => !wound.expired;

	public bool poisoned => !poison.expired;

	public bool unmovable => !unmoving.expired;

	public Stun stun { get; private set; }

	public Burn burn { get; private set; }

	public Wound wound { get; private set; }

	public Freeze freeze { get; private set; }

	public Poison poison { get; private set; }

	public Unmoving unmoving { get; private set; }

	public bool hasAny
	{
		get
		{
			if (!stuned && !freezed && !burning && !wounded)
			{
				return poisoned;
			}
			return true;
		}
	}

	public bool giveStoppingPowerOnPoison { get; set; }

	public bool canBleedCritical { get; set; }

	public int freezeMaxHitStack
	{
		get
		{
			return _freezeMaxStack;
		}
		set
		{
			_freezeMaxStack = value;
		}
	}

	public EnumArray<Kind, SumFloat> durationMultiplier { get; set; } = new EnumArray<Kind, SumFloat>((SumFloat[])(object)new SumFloat[7]
	{
		new SumFloat(1f),
		new SumFloat(1f),
		new SumFloat(1f),
		new SumFloat(1f),
		new SumFloat(1f),
		new SumFloat(1f),
		new SumFloat(1f)
	});


	public event OnTimeDelegate onApplyBleed;

	public event OnTimeDelegate onApplyWound;

	public event OnTimeDelegate onApplyFreeze;

	public event OnTimeDelegate onRefreshFreeze;

	public event OnTimeDelegate onReleaseFreeze;

	public event OnTimeDelegate onApplyBurn;

	public event OnTimeDelegate onRefreshBurn;

	public event OnTimeDelegate onReleaseBurn;

	public event OnTimeDelegate onGaveBurnDamage;

	public event OnTimeDelegate onGaveEmberDamage;

	public event OnTimeDelegate onApplyPoison;

	public event OnTimeDelegate onRefreshPoison;

	public event OnTimeDelegate onReleasePoison;

	public event OnTimeDelegate onApplyStun;

	public event OnTimeDelegate onRefreshStun;

	public event OnTimeDelegate onReleaseStun;

	public event OnApplyDelegate onApply;

	public bool isLockStatus(Kind kind)
	{
		if (kind != 0)
		{
			return kind == Kind.Freeze;
		}
		return true;
	}

	private void Awake()
	{
		stun = new Stun(_character);
		stun.Initialize();
		freeze = new Freeze(_character);
		freeze.Initialize();
		burn = new Burn(_character);
		burn.Initialize();
		wound = new Wound(_character);
		wound.Initialize();
		poison = new Poison(_character);
		poison.Initialize();
		unmoving = new Unmoving(_character);
		unmoving.Initialize();
	}

	public bool Apply(Character attacker, ApplyInfo applyInfo)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (_character.invulnerable.value)
		{
			return false;
		}
		if (unstoppable.value && isLockStatus(applyInfo.kind))
		{
			Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)_character.collider).bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString("floating/unstoppable"), Vector2.op_Implicit(val), "#a3a3a3");
			return false;
		}
		bool flag = false;
		switch (applyInfo.kind)
		{
		case Kind.Stun:
			flag = ApplyStun(attacker);
			break;
		case Kind.Freeze:
			flag = ApplyFreeze(attacker);
			break;
		case Kind.Burn:
			flag = ApplyBurn(attacker);
			break;
		case Kind.Wound:
			flag = ApplyWound(attacker);
			break;
		case Kind.Poison:
			flag = ApplyPoison(attacker);
			break;
		case Kind.Unmoving:
			flag = ApplyUnmoving(attacker);
			break;
		}
		if (flag)
		{
			this.onApply?.Invoke(attacker, _character, applyInfo);
		}
		return flag;
	}

	public void RemoveStun()
	{
		_character.ability.Remove(stun);
	}

	public void RemoveAllStatus()
	{
		_character.ability.Remove(stun);
		_character.ability.Remove(freeze);
		_character.ability.Remove(burn);
		_character.ability.Remove(wound);
		_character.ability.Remove(poison);
		_character.ability.Remove(unmoving);
	}

	private bool ApplyUnmoving(Character attacker)
	{
		unmoving.attacker = attacker;
		_character.ability.Add(unmoving);
		return true;
	}

	private bool ApplyStun(Character attacker)
	{
		stun.attacker = attacker;
		if ((Object)(object)attacker.status != (Object)null)
		{
			stun.durationMultiplier = ((Sum<float>)(object)attacker.status.durationMultiplier[Kind.Stun]).total;
			stun.onAttached = attacker.status.onApplyStun;
			stun.onRefreshed = attacker.status.onRefreshStun;
			stun.onDetached = attacker.status.onReleaseStun;
		}
		_character.ability.Add(stun);
		return true;
	}

	private bool ApplyFreeze(Character attacker)
	{
		freeze.attacker = attacker;
		if ((Object)(object)attacker.status != (Object)null)
		{
			freeze.durationMultiplier = ((Sum<float>)(object)attacker.status.durationMultiplier[Kind.Freeze]).total;
			freeze.hitStack = attacker.status.freezeMaxHitStack;
			freeze.onAttached = attacker.status.onApplyFreeze;
			freeze.onRefreshed = attacker.status.onRefreshFreeze;
			freeze.onDetached = attacker.status.onReleaseFreeze;
		}
		_character.ability.Add(freeze);
		return true;
	}

	private bool ApplyBurn(Character attacker)
	{
		burn.attacker = attacker;
		if ((Object)(object)attacker.status != (Object)null)
		{
			burn.durationMultiplier = ((Sum<float>)(object)attacker.status.durationMultiplier[Kind.Burn]).total;
			burn.onAttached = attacker.status.onApplyBurn;
			burn.onRefreshed = attacker.status.onRefreshBurn;
			burn.onDetached = attacker.status.onReleaseBurn;
			burn.onTookBurnDamage = attacker.status.onGaveBurnDamage;
			burn.onTookEmberDamage = attacker.status.onGaveEmberDamage;
		}
		_character.ability.Add(burn);
		return true;
	}

	private bool ApplyWound(Character attacker)
	{
		wound.attacker = attacker;
		if ((Object)(object)attacker.status != (Object)null)
		{
			wound.critical = attacker.status.canBleedCritical;
			wound.onAttached = attacker.status.onApplyWound;
			wound.onDetached = attacker.status.onApplyBleed;
		}
		_character.ability.Add(wound);
		return true;
	}

	private bool ApplyPoison(Character attacker)
	{
		poison.attacker = attacker;
		if ((Object)(object)attacker.status != (Object)null)
		{
			poison.durationMultiplier = ((Sum<float>)(object)attacker.status.durationMultiplier[Kind.Poison]).total;
			poison.stoppingPower = attacker.status.giveStoppingPowerOnPoison;
			poison.onAttached = attacker.status.onApplyPoison;
			poison.onRefreshed = attacker.status.onRefreshPoison;
			poison.onDetached = attacker.status.onReleasePoison;
		}
		_character.ability.Add(poison);
		return true;
	}

	public bool IsApplying(Kind kind)
	{
		switch (kind)
		{
		case Kind.Stun:
			if (stuned)
			{
				return stun.remainTime < stun.duration;
			}
			return false;
		case Kind.Freeze:
			if (freezed)
			{
				return freeze.remainTime < freeze.duration;
			}
			return false;
		case Kind.Burn:
			if (burning)
			{
				return burn.remainTime < burn.duration;
			}
			return false;
		case Kind.Wound:
			if (wounded)
			{
				return wound.attached;
			}
			return false;
		case Kind.Poison:
			if (poisoned)
			{
				return poison.remainTime < poison.duration;
			}
			return false;
		case Kind.Unmoving:
			return unmovable;
		default:
			return false;
		}
	}

	public bool IsApplying(EnumArray<Kind, bool> enumArray)
	{
		for (int i = 0; i < enumArray.Count; i++)
		{
			if (enumArray.Array[i] && IsApplying(enumArray.Keys[i]))
			{
				return true;
			}
		}
		return false;
	}

	public void Register(Kind kind, Timing timing, OnTimeDelegate invoke)
	{
		switch (kind)
		{
		case Kind.Wound:
			if (timing == Timing.Apply)
			{
				onApplyWound += invoke;
			}
			else
			{
				onApplyBleed += invoke;
			}
			break;
		case Kind.Burn:
			switch (timing)
			{
			case Timing.Apply:
				onApplyBurn += invoke;
				onRefreshStun += invoke;
				break;
			case Timing.Refresh:
				onRefreshBurn += invoke;
				break;
			default:
				onReleaseBurn += invoke;
				break;
			}
			break;
		case Kind.Freeze:
			switch (timing)
			{
			case Timing.Apply:
				onApplyFreeze += invoke;
				onRefreshFreeze += invoke;
				break;
			case Timing.Refresh:
				onRefreshFreeze += invoke;
				break;
			default:
				onReleaseFreeze += invoke;
				break;
			}
			break;
		case Kind.Poison:
			switch (timing)
			{
			case Timing.Apply:
				onApplyPoison += invoke;
				onRefreshPoison += invoke;
				break;
			case Timing.Refresh:
				onRefreshPoison += invoke;
				break;
			default:
				onReleasePoison += invoke;
				break;
			}
			break;
		case Kind.Stun:
			switch (timing)
			{
			case Timing.Apply:
				onApplyStun += invoke;
				onRefreshStun += invoke;
				break;
			case Timing.Refresh:
				onRefreshStun += invoke;
				break;
			default:
				onReleaseStun += invoke;
				break;
			}
			break;
		default:
			Debug.Log((object)$"{kind} 미구현");
			break;
		}
	}

	public void Unregister(Kind kind, Timing timing, OnTimeDelegate invoke)
	{
		switch (kind)
		{
		case Kind.Wound:
			if (timing == Timing.Apply)
			{
				onApplyWound -= invoke;
			}
			else
			{
				onApplyBleed -= invoke;
			}
			break;
		case Kind.Burn:
			switch (timing)
			{
			case Timing.Apply:
				onApplyBurn -= invoke;
				onRefreshBurn -= invoke;
				break;
			case Timing.Refresh:
				onRefreshBurn -= invoke;
				break;
			default:
				onReleaseBurn -= invoke;
				break;
			}
			break;
		case Kind.Freeze:
			switch (timing)
			{
			case Timing.Apply:
				onApplyFreeze -= invoke;
				onRefreshFreeze -= invoke;
				break;
			case Timing.Refresh:
				onRefreshFreeze -= invoke;
				break;
			default:
				onReleaseFreeze -= invoke;
				break;
			}
			break;
		case Kind.Poison:
			switch (timing)
			{
			case Timing.Apply:
				onApplyPoison -= invoke;
				onRefreshPoison -= invoke;
				break;
			case Timing.Refresh:
				onRefreshPoison -= invoke;
				break;
			default:
				onReleasePoison -= invoke;
				break;
			}
			break;
		case Kind.Stun:
			switch (timing)
			{
			case Timing.Apply:
				onApplyStun -= invoke;
				onRefreshStun -= invoke;
				break;
			case Timing.Refresh:
				onRefreshStun -= invoke;
				break;
			default:
				onReleaseStun -= invoke;
				break;
			}
			break;
		default:
			Debug.Log((object)$"{kind} 미구현");
			break;
		}
	}
}
