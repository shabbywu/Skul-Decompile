using Characters.Player;
using InControl;
using UnityEngine;
using UserInput;

namespace Characters.Controllers;

public sealed class PlayerInput : MonoBehaviour
{
	public static readonly TrueOnlyLogicalSumList blocked = new TrueOnlyLogicalSumList(false);

	public static readonly TrueOnlyLogicalSumList reverseHorizontal = new TrueOnlyLogicalSumList(false);

	public readonly PlayerAction[] _map = (PlayerAction[])(object)new PlayerAction[Button.count];

	[GetComponent]
	[SerializeField]
	private Character _character;

	private WeaponInventory _weaponInventory;

	private QuintessenceInventory _quintessenceInventory;

	private CharacterInteraction _characterInteraction;

	public Vector2 direction;

	public PlayerAction attack
	{
		get
		{
			return _map[Button.Attack.index];
		}
		private set
		{
			_map[Button.Attack.index] = value;
		}
	}

	public PlayerAction dash
	{
		get
		{
			return _map[Button.Dash.index];
		}
		private set
		{
			_map[Button.Dash.index] = value;
		}
	}

	public PlayerAction jump
	{
		get
		{
			return _map[Button.Jump.index];
		}
		private set
		{
			_map[Button.Jump.index] = value;
		}
	}

	public PlayerAction skill
	{
		get
		{
			return _map[Button.Skill.index];
		}
		private set
		{
			_map[Button.Skill.index] = value;
		}
	}

	public PlayerAction skill2
	{
		get
		{
			return _map[Button.Skill2.index];
		}
		private set
		{
			_map[Button.Skill2.index] = value;
		}
	}

	public PlayerAction useItem
	{
		get
		{
			return _map[Button.UseItem.index];
		}
		private set
		{
			_map[Button.UseItem.index] = value;
		}
	}

	public PlayerAction notUsed
	{
		get
		{
			return _map[Button.None.index];
		}
		private set
		{
			_map[Button.None.index] = value;
		}
	}

	public PlayerAction interaction { get; private set; }

	public PlayerAction swap { get; private set; }

	public PlayerAction left { get; private set; }

	public PlayerAction right { get; private set; }

	public PlayerAction up { get; private set; }

	public PlayerAction down { get; private set; }

	public PlayerAction this[int index] => _map[index];

	public PlayerAction this[Button button] => _map[button.index];

	private void Awake()
	{
		attack = KeyMapper.Map.Attack;
		dash = KeyMapper.Map.Dash;
		jump = KeyMapper.Map.Jump;
		skill = KeyMapper.Map.Skill1;
		skill2 = KeyMapper.Map.Skill2;
		interaction = KeyMapper.Map.Interaction;
		swap = KeyMapper.Map.Swap;
		useItem = KeyMapper.Map.Quintessence;
		left = KeyMapper.Map.Left;
		right = KeyMapper.Map.Right;
		up = KeyMapper.Map.Up;
		down = KeyMapper.Map.Down;
		_weaponInventory = ((Component)this).GetComponent<WeaponInventory>();
		_quintessenceInventory = ((Component)this).GetComponent<QuintessenceInventory>();
		_characterInteraction = ((Component)this).GetComponent<CharacterInteraction>();
	}

	private void Update()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		if (blocked.value)
		{
			return;
		}
		direction = ((TwoAxisInputControl)KeyMapper.Map.Move).Vector;
		if (direction.x > 0.33f)
		{
			if (reverseHorizontal.value)
			{
				_character.movement.MoveHorizontal(Vector2.left);
			}
			else
			{
				_character.movement.MoveHorizontal(Vector2.right);
			}
		}
		if (direction.x < -0.33f)
		{
			if (reverseHorizontal.value)
			{
				_character.movement.MoveHorizontal(Vector2.right);
			}
			else
			{
				_character.movement.MoveHorizontal(Vector2.left);
			}
		}
		if (direction.y > 0.33f)
		{
			_character.movement.MoveVertical(Vector2.up);
		}
		if (direction.y < -0.33f)
		{
			_character.movement.MoveVertical(Vector2.down);
		}
		for (int i = 0; i < _character.actions.Count; i++)
		{
			if (_character.actions[i].Process())
			{
				return;
			}
		}
		if (((OneAxisInputControl)swap).WasPressed && !_character.silence.value)
		{
			_weaponInventory.NextWeapon();
			return;
		}
		if (((OneAxisInputControl)useItem).WasPressed)
		{
			_quintessenceInventory.UseAt(0);
			return;
		}
		if (((OneAxisInputControl)interaction).WasPressed)
		{
			_characterInteraction.InteractionKeyWasPressed();
		}
		if (((OneAxisInputControl)interaction).WasReleased)
		{
			_characterInteraction.InteractionKeyWasReleased();
		}
	}
}
