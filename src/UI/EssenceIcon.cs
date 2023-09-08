using Characters.Gear.Quintessences.Constraints;
using GameResources;
using UnityEngine.UI;

namespace UI;

public class EssenceIcon : IconWithCooldown
{
	public Constraint.Subcomponents constraints { get; set; }

	protected override void Update()
	{
		if (constraints != null)
		{
			base.Update();
			((Graphic)base.icon).material = ((constraints.components.Pass() && base.cooldown.canUse) ? null : MaterialResource.ui_grayScale);
		}
	}
}
