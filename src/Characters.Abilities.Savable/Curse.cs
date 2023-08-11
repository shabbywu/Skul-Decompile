using System.Collections;
using System.Linq;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable;

public sealed class Curse : IAbility, IAbilityInstance, ISavableAbility
{
	private static float valuePerStack = 0.1f;

	private const int _stackUpStep = 3;

	private int _stack = 1;

	private Stat.Values _statPerStack = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1f + valuePerStack));

	private Stat.Values _statClone;

	private EffectInfo _effectInfo;

	private SoundInfo _soundInfo;

	public Character owner { get; private set; }

	public IAbility ability => this;

	public bool attached => true;

	public Sprite icon => SavableAbilityResource.instance.curseIcon;

	public float iconFillAmount => remainTime / duration;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => _stack;

	public bool expired => false;

	public float duration => 2.1474836E+09f;

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public int index { get; set; }

	public float remainTime { get; set; }

	public float stack
	{
		get
		{
			return _stack;
		}
		set
		{
			_stack = (int)value;
			if (_stack > 0)
			{
				((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CLoadStack());
			}
		}
	}

	private string floatingText => Localization.GetLocalizedString("floating/curseoflight");

	public void Attach()
	{
		_statClone = _statPerStack.Clone();
		SpawnEffects();
	}

	public void Detach()
	{
		_stack = 1;
		Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(0f);
		owner.stat.DetachValues(_statClone);
	}

	public void Refresh()
	{
		_stack++;
		if (_stack == 3)
		{
			AttachStatBonus();
		}
		else if (_stack % 3 == 0)
		{
			UpdateStack();
		}
		SpawnEffects();
	}

	public void Initialize()
	{
	}

	public void UpdateTime(float deltaTime)
	{
	}

	public IAbilityInstance CreateInstance(Character owner)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		EffectInfo obj = new EffectInfo(SavableAbilityResource.instance.curseAttachEffect)
		{
			attachInfo = new EffectInfo.AttachInfo(attach: true, layerOnly: false, 1, EffectInfo.AttachInfo.Pivot.Bottom),
			trackChildren = false
		};
		SortingLayer val = SortingLayer.layers.Last();
		obj.sortingLayerId = ((SortingLayer)(ref val)).id;
		_effectInfo = obj;
		_soundInfo = new SoundInfo(SavableAbilityResource.instance.curseAttachSound);
		return this;
	}

	private void AttachStatBonus()
	{
		Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(_stack);
		owner.stat.AttachValues(_statClone);
		SpawnBuffText();
	}

	private void UpdateStack()
	{
		Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(_stack);
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_statClone).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_statClone).values[i].value = ((ReorderableArray<Stat.Value>)_statClone).values[i].value + (double)valuePerStack;
		}
		owner.stat.SetNeedUpdate();
		SpawnBuffText();
	}

	private IEnumerator CLoadStack()
	{
		while ((Object)(object)owner == (Object)null)
		{
			yield return null;
		}
		LoadStack();
	}

	private void LoadStack()
	{
		Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(_stack);
		owner.stat.AttachValues(_statClone);
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_statClone).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_statClone).values[i].value = ((ReorderableArray<Stat.Value>)_statClone).values[i].value + (double)(valuePerStack * (float)Mathf.FloorToInt((float)(_stack / 3)));
		}
		owner.stat.SetNeedUpdate();
		SpawnBuffText();
	}

	private void SpawnEffects()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_soundInfo, ((Component)owner).transform.position);
		_effectInfo.Spawn(((Component)owner).transform.position, owner);
	}

	private void SpawnBuffText()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)(_stack / 3) * valuePerStack * 100f;
		string text = string.Format(floatingText, num);
		Bounds bounds = ((Collider2D)owner.collider).bounds;
		Vector3 center = ((Bounds)(ref bounds)).center;
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(text, center);
	}
}
