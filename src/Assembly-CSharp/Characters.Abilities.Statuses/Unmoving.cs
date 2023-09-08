using FX;
using GameResources;
using UnityEngine;

namespace Characters.Abilities.Statuses;

public class Unmoving : IAbility, IAbilityInstance
{
	private const string _floatingTextKey = "floating/status/stun";

	private const string _floatingTextColor = "#ffffff";

	private EffectInfo _effect;

	public Character attacker;

	public Character owner { get; private set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => null;

	public float iconFillAmount => remainTime / duration;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => 0;

	public bool expired => remainTime <= 0f;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Unmoving(Character owner)
	{
		this.owner = owner;
		duration = float.MaxValue;
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
	}

	public void Refresh()
	{
		remainTime = duration;
	}

	public void Attach()
	{
		remainTime = duration;
		if (owner.type == Character.Type.Boss)
		{
			owner.chronometer.animation.AttachTimeScale(this, 0f);
			owner.blockLook.Attach(this);
			if ((Object)(object)owner.movement != (Object)null)
			{
				owner.movement.blocked.Attach(this);
			}
		}
		else
		{
			if ((Object)(object)owner.hit == (Object)null || (Object)(object)owner.hit.action == (Object)null)
			{
				return;
			}
			owner.CancelAction();
			if (owner.hit.action.motions.Length >= 1)
			{
				int num = Random.Range(0, owner.hit.action.motions.Length);
				owner.animationController.Unmove(owner.hit.action.motions[num].animationInfo);
			}
		}
		owner.blockLook.Attach(this);
		if ((Object)(object)owner.movement != (Object)null)
		{
			owner.movement.blocked.Attach(this);
		}
	}

	public void Detach()
	{
		remainTime = 0f;
		if (owner.type == Character.Type.Boss)
		{
			owner.chronometer.animation.DetachTimeScale(this);
			owner.blockLook.Detach(this);
			if ((Object)(object)owner.movement != (Object)null)
			{
				owner.movement.blocked.Detach(this);
			}
		}
		else
		{
			owner.animationController.StopAll();
		}
		owner.blockLook.Detach(this);
		if ((Object)(object)owner.movement != (Object)null)
		{
			owner.movement.blocked.Detach(this);
		}
	}

	public void Initialize()
	{
		_effect = new EffectInfo(CommonResource.instance.bindingEffect)
		{
			attachInfo = new EffectInfo.AttachInfo(attach: true, layerOnly: false, 1, EffectInfo.AttachInfo.Pivot.Center),
			loop = true,
			trackChildren = true
		};
	}

	private void SpawnFloatingText()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds);
	}
}
