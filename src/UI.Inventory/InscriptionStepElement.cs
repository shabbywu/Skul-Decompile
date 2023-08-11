using System.Collections.Generic;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public sealed class InscriptionStepElement : MonoBehaviour, ILayoutElement
{
	public static readonly string activatedColor = "#755754";

	public static readonly string inactivatedColor = "#B2977B";

	[SerializeField]
	private TMP_Text _step;

	[SerializeField]
	private Image _arrow;

	[SerializeField]
	private Sprite _arrowActivated;

	[SerializeField]
	private Sprite _arrowDeactivated;

	[Space]
	[SerializeField]
	private TextMeshProUGUI _description;

	private float _minHeight = 30f;

	private float _maxHeight = 90f;

	public float minWidth => ((TMP_Text)_description).minWidth;

	public float preferredWidth => ((TMP_Text)_description).preferredWidth;

	public float flexibleWidth => ((TMP_Text)_description).flexibleWidth;

	public float minHeight => ((TMP_Text)_description).minHeight;

	public float preferredHeight
	{
		get
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			float num = math.clamp(((TMP_Text)_description).preferredHeight, _minHeight, _maxHeight);
			Vector2 sizeDelta = ((TMP_Text)_description).rectTransform.sizeDelta;
			sizeDelta.y = num;
			((TMP_Text)_description).rectTransform.sizeDelta = sizeDelta;
			return num;
		}
	}

	public float flexibleHeight => ((TMP_Text)_description).flexibleHeight;

	public int layoutPriority => ((TMP_Text)_description).layoutPriority;

	public float descriptionPreferredHeight => ((TMP_Text)_description).preferredHeight;

	public void CalculateLayoutInputHorizontal()
	{
		_description.CalculateLayoutInputHorizontal();
	}

	public void CalculateLayoutInputVertical()
	{
		_description.CalculateLayoutInputVertical();
	}

	public void Set(Inscription.Key key, IList<int> steps, int stepIndex, bool activated)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).gameObject.SetActive(true);
		Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
		Color color = default(Color);
		ColorUtility.TryParseHtmlString(activated ? activatedColor : inactivatedColor, ref color);
		_arrow.sprite = (activated ? _arrowActivated : _arrowDeactivated);
		_step.text = steps[stepIndex].ToString();
		((Graphic)_step).color = color;
		if ((Object)(object)_description != (Object)null)
		{
			((TMP_Text)_description).text = inscription.GetDescription(stepIndex);
			((Graphic)_description).color = color;
		}
	}

	public void ClampHeight(float minHeight, float maxHeight)
	{
		_minHeight = minHeight;
		_maxHeight = maxHeight;
	}
}
