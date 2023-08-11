using System;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GearPopup;

public class ItemSelectNavigation : MonoBehaviour
{
	[SerializeField]
	private Image _focus;

	[SerializeField]
	private ItemSelectElement[] _items;

	public int selectedItemIndex { get; private set; }

	public event Action<int> onItemSelected;

	private void OnEnable()
	{
		ItemInventory item = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
		for (int i = 0; i < _items.Length; i++)
		{
			ItemSelectElement obj = _items[i];
			obj.SetIcon(item.items[i].icon);
			int cachedIndex = i;
			obj.onSelected = delegate
			{
				selectedItemIndex = cachedIndex;
				this.onItemSelected?.Invoke(cachedIndex);
			};
		}
	}

	private void Update()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (!((Object)(object)currentSelectedGameObject == (Object)null))
		{
			((Transform)((Graphic)_focus).rectTransform).position = currentSelectedGameObject.transform.position;
		}
	}
}
