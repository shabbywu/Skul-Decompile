using System.Collections;
using Characters.Gear;
using GameResources;
using Level;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool;

public class GearListElement : MonoBehaviour
{
	private static readonly EnumArray<Rarity, Color> _rarityColorTable = new EnumArray<Rarity, Color>((Color[])(object)new Color[4]
	{
		Color.black,
		Color.blue,
		Color.magenta,
		Color.red
	});

	[SerializeField]
	private Button _button;

	[SerializeField]
	private Image _thumbnail;

	[SerializeField]
	private TMP_Text _text;

	public Gear.Type type { get; private set; }

	public GearReference gearReference { get; set; }

	public string text => _text.text;

	public void Set(GearReference gearReference)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		this.gearReference = gearReference;
		type = gearReference.type;
		_text.text = Localization.GetLocalizedString(gearReference.displayNameKey);
		if (string.IsNullOrWhiteSpace(_text.text))
		{
			_text.text = gearReference.name;
		}
		if (gearReference.obtainable)
		{
			((Graphic)_text).color = _rarityColorTable[gearReference.rarity];
		}
		else
		{
			((Graphic)_text).color = Color.gray;
		}
		_thumbnail.sprite = gearReference.thumbnail;
		((UnityEvent)_button.onClick).AddListener((UnityAction)delegate
		{
			((MonoBehaviour)this).StartCoroutine(CDropGear(gearReference));
		});
	}

	private IEnumerator CDropGear(GearReference gearReference)
	{
		GearRequest request = gearReference.LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		levelManager.DropGear(request, ((Component)levelManager.player).transform.position);
	}
}
