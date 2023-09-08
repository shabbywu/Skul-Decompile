using System;
using Data;
using FX;
using GameResources;
using Hardmode.Darktech;
using Level;
using Singletons;
using UnityEngine;

public class MapReward : MonoBehaviour
{
	[Serializable]
	public class RewardTypeGameObjectArray : EnumArray<Type, GameObject>
	{
	}

	public enum Type
	{
		None,
		Gold,
		Head,
		Item,
		Adventurer,
		Boss
	}

	[NonSerialized]
	public Type type;

	[SerializeField]
	private SpriteRenderer _preview;

	[SerializeField]
	private RewardTypeGameObjectArray _rewardPrefabs;

	[SerializeField]
	private EffectInfo _spawnEffect;

	[SerializeField]
	private Transform _spawnEffectPosition;

	private GameObject _reward;

	public bool hasReward => (Object)(object)_rewardPrefabs[type] != (Object)null;

	public bool activated { get; set; }

	public bool isHardmodeItem
	{
		get
		{
			if (type == Type.Item)
			{
				return GameData.HardmodeProgress.hardmode;
			}
			return false;
		}
	}

	public event Action onLoot;

	private void Awake()
	{
		Object.Destroy((Object)(object)_preview);
	}

	public void LoadReward()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = _rewardPrefabs[type];
		if (isHardmodeItem)
		{
			val = CommonResource.instance.hardmodeChest;
		}
		if ((Object)(object)val == (Object)null)
		{
			this.onLoot?.Invoke();
			return;
		}
		_reward = Object.Instantiate<GameObject>(val, ((Component)this).transform.position, Quaternion.identity, ((Component)this).transform);
		_reward.gameObject.SetActive(false);
		if (isHardmodeItem)
		{
			HardmodeChest component = _reward.GetComponent<HardmodeChest>();
			if ((Object)(object)component != (Object)null)
			{
				component.TryToChangeOmenChest();
			}
			if (component.isOmenChest)
			{
				_spawnEffect.animation = Singleton<DarktechManager>.Instance.resource.omenChestSpawnEffect;
			}
		}
		_reward.GetComponent<ILootable>().onLoot += delegate
		{
			this.onLoot?.Invoke();
		};
	}

	public bool Activate()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		activated = true;
		if ((Object)(object)_reward == (Object)null)
		{
			return false;
		}
		_reward.gameObject.SetActive(true);
		_reward.GetComponent<ILootable>().Activate();
		_spawnEffect.Spawn(_spawnEffectPosition.position);
		return true;
	}
}
