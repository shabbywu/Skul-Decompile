using System;
using System.Linq;
using Characters.Abilities;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class FairyTale : InscriptionInstance
{
	private sealed class StatBonus : IAbility, IAbilityInstance
	{
		[NonSerialized]
		public double currentStatBonus;

		[NonSerialized]
		public Stat.Values stat = new Stat.Values(new Stat.Value(0, 0, 0.0));

		private Character _owner;

		Character IAbilityInstance.owner => _owner;

		public IAbility ability => this;

		public float remainTime { get; set; }

		public bool attached => true;

		public Sprite icon { get; set; }

		public float iconFillAmount => 0f;

		public bool iconFillInversed => false;

		public bool iconFillFlipped => false;

		public int iconStacks => (int)(currentStatBonus * 100.0);

		public bool expired => false;

		public float duration { get; set; }

		public int iconPriority => 0;

		public bool removeOnSwapWeapon => false;

		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		public StatBonus(Character owner)
		{
			_owner = owner;
		}

		public void Initialize()
		{
		}

		public void UpdateTime(float deltaTime)
		{
		}

		public void Refresh()
		{
		}

		void IAbilityInstance.Attach()
		{
			_owner.stat.AttachValues(stat);
		}

		void IAbilityInstance.Detach()
		{
			_owner.stat.DetachValues(stat);
		}

		public void UpdateStat()
		{
			((ReorderableArray<Stat.Value>)stat).values[0].value = currentStatBonus;
			_owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	[Header("2세트 효과")]
	private Sprite _icon;

	[SerializeField]
	private double _statBonusPerSpiritCount = 0.10000000149011612;

	[Space]
	[Header("5세트 효과")]
	[SerializeField]
	private Transform _oberonSlot;

	[SerializeField]
	[Space]
	private AssetReference _oberonReference;

	[SerializeField]
	private AssetReference _darkOberonReference;

	private Oberon _oberonInstance;

	private Oberon _darkOberonInstance;

	private AsyncOperationHandle<GameObject> _oberonHandle;

	private AsyncOperationHandle<GameObject> _darkOberonHandle;

	private StatBonus _statBonus;

	private Stat.Category statCategory => Stat.Category.PercentPoint;

	private Stat.Kind statKind => Stat.Kind.SpiritAttackCooldownSpeed;

	private void SpawnOberon()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_oberonInstance != (Object)null))
		{
			if (!_oberonHandle.IsValid())
			{
				_oberonHandle = _oberonReference.LoadAssetAsync<GameObject>();
			}
			GameObject val = Object.Instantiate<GameObject>(_oberonHandle.WaitForCompletion());
			ReleaseAddressableHandleOnDestroy.Reserve(val, _oberonHandle);
			_oberonInstance = val.GetComponent<Oberon>();
			_oberonInstance.Initialize(base.character, _oberonSlot);
		}
	}

	private void SpawnDarkOberon()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_darkOberonInstance != (Object)null))
		{
			if (!_darkOberonReference.IsValid())
			{
				_darkOberonHandle = _darkOberonReference.LoadAssetAsync<GameObject>();
			}
			GameObject val = Object.Instantiate<GameObject>(_darkOberonHandle.WaitForCompletion());
			ReleaseAddressableHandleOnDestroy.Reserve(val, _darkOberonHandle);
			_darkOberonInstance = val.GetComponent<Oberon>();
			_darkOberonInstance.Initialize(base.character, _oberonSlot);
		}
	}

	private void ReleaseOberon()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_oberonInstance != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)_oberonInstance).gameObject);
			_oberonInstance = null;
		}
		if (_oberonHandle.IsValid())
		{
			Addressables.Release<GameObject>(_oberonHandle);
		}
	}

	private void ReleaseDarkOberon()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_darkOberonInstance != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)_darkOberonInstance).gameObject);
			_darkOberonInstance = null;
		}
		if (_darkOberonHandle.IsValid())
		{
			Addressables.Release<GameObject>(_darkOberonHandle);
		}
	}

	protected override void Initialize()
	{
		_statBonus = new StatBonus(base.character);
		_statBonus.Initialize();
		_statBonus.icon = _icon;
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step != 0)
		{
			if (!keyword.isMaxStep)
			{
				ReleaseOberon();
				ReleaseDarkOberon();
			}
			else if (keyword.omen)
			{
				ReleaseOberon();
				SpawnDarkOberon();
			}
			else
			{
				ReleaseDarkOberon();
				SpawnOberon();
			}
		}
	}

	public override void Attach()
	{
		base.character.ability.Add(_statBonus);
	}

	public override void Detach()
	{
		base.character.ability.Remove(_statBonus);
		ReleaseOberon();
		ReleaseDarkOberon();
	}

	private void OnDestroy()
	{
		ReleaseOberon();
		ReleaseDarkOberon();
	}

	private void UpdateStat()
	{
		int num = base.character.playerComponents.inventory.item.items.Count((Item item) => !((Object)(object)item == (Object)null) && item.gearTag == Gear.Tag.Spirit);
		_statBonus.currentStatBonus = _statBonusPerSpiritCount * (double)num;
		if (statCategory.index == Stat.Category.Percent.index)
		{
			_statBonus.currentStatBonus = _statBonus.currentStatBonus * 0.01 + 1.0;
		}
		else if (statCategory.index == Stat.Category.PercentPoint.index)
		{
			_statBonus.currentStatBonus *= 0.01;
		}
		((ReorderableArray<Stat.Value>)_statBonus.stat).values[0].categoryIndex = statCategory.index;
		((ReorderableArray<Stat.Value>)_statBonus.stat).values[0].kindIndex = statKind.index;
		_statBonus.UpdateStat();
	}
}
