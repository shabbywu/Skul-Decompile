using Characters.Actions;
using GameResources;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class ActionIcon : IconWithCooldown
{
	[SerializeField]
	private Image _silenceMask;

	public Action action { get; set; }

	protected override void Update()
	{
		if ((Object)(object)action == (Object)null)
		{
			return;
		}
		base.Update();
		((Graphic)base.icon).material = (action.canUse ? null : MaterialResource.ui_grayScale);
		if (action.owner.silence.value)
		{
			if (!((Component)_silenceMask).gameObject.activeInHierarchy)
			{
				((Component)_silenceMask).gameObject.SetActive(true);
			}
			((Graphic)base.icon).material = MaterialResource.ui_grayScale;
		}
		else if (((Component)_silenceMask).gameObject.activeInHierarchy)
		{
			((Component)_silenceMask).gameObject.SetActive(false);
		}
	}
}
