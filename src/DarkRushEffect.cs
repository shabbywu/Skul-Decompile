using Hardmode;
using Singletons;
using UnityEngine;

public class DarkRushEffect : MonoBehaviour
{
	[SerializeField]
	private Transform _sign;

	[SerializeField]
	private SpriteRenderer _signSpriteRender;

	[SerializeField]
	private Transform _impact;

	[SerializeField]
	private SpriteRenderer _impactSpriteRender;

	[SerializeField]
	private Transform _impactHardmode;

	[SerializeField]
	private SpriteRenderer _impactSpriteRenderHardmode;

	public void ShowSign()
	{
		((Component)_sign).gameObject.SetActive(true);
	}

	public void HideSign()
	{
		((Component)_sign).gameObject.SetActive(false);
	}

	public void SetSignEffectOrder(int order)
	{
		((Renderer)_signSpriteRender).sortingOrder = order;
	}

	public void ShowImpact()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			((Component)_impactHardmode).gameObject.SetActive(true);
		}
		else
		{
			((Component)_impact).gameObject.SetActive(true);
		}
	}

	public void HideImpact()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			((Component)_impactHardmode).gameObject.SetActive(false);
		}
		else
		{
			((Component)_impact).gameObject.SetActive(false);
		}
	}

	public void SetImpactEffectOrder(int order)
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			((Renderer)_impactSpriteRenderHardmode).sortingOrder = order;
		}
		else
		{
			((Renderer)_impactSpriteRender).sortingOrder = order;
		}
	}
}
