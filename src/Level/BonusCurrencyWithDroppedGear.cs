using Characters;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public sealed class BonusCurrencyWithDroppedGear : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private Sprite _goldIcon;

	[SerializeField]
	private Sprite _boneIcon;

	[SerializeField]
	private Sprite _darkQuartzIcon;

	private GameData.Currency.Type _type;

	private int _amount;

	private int _count;

	private DroppedGear _gear;

	public void Attach(DroppedGear gear, GameData.Currency.Type type, int amount, int count)
	{
		_gear = gear;
		_type = type;
		_amount = amount;
		_count = count;
		switch (_type)
		{
		case GameData.Currency.Type.Bone:
			_renderer.sprite = _boneIcon;
			break;
		case GameData.Currency.Type.Gold:
			_renderer.sprite = _goldIcon;
			break;
		case GameData.Currency.Type.DarkQuartz:
			_renderer.sprite = _darkQuartzIcon;
			break;
		}
		gear.onLoot += Earn;
		gear.onDestroy += Earn;
	}

	private void Earn(Character character)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (_gear.gear.destructible)
		{
			_gear.onLoot -= Earn;
			_gear.onDestroy -= Earn;
			Singleton<Service>.Instance.levelManager.DropCurrency(_type, _amount, _count, ((Component)this).transform.position);
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}

	private void OnDestroy()
	{
		if (!((Object)(object)_gear == (Object)null))
		{
			_gear.onLoot -= Earn;
			_gear.onDestroy -= Earn;
		}
	}
}
