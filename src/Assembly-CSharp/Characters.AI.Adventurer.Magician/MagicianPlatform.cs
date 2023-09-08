using System.Collections;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician;

public class MagicianPlatform : MonoBehaviour
{
	[SerializeField]
	private bool _left;

	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private float _lifeTime;

	private MagicianPlatformController _controller;

	public void Initialize(MagicianPlatformController controller)
	{
		_controller = controller;
	}

	public void Show()
	{
		((Component)this).gameObject.SetActive(true);
		((Behaviour)_collider).enabled = true;
		((MonoBehaviour)this).StartCoroutine(CStartLifeCycle());
	}

	private void Hide()
	{
		((Component)this).gameObject.SetActive(false);
		((Behaviour)_collider).enabled = false;
		_controller.AddPlatform(this, _left);
	}

	private IEnumerator CStartLifeCycle()
	{
		FadeOut();
		yield return Chronometer.global.WaitForSeconds(_lifeTime);
		FadeIn();
	}

	private void FadeOut()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut());
	}

	private IEnumerator CFadeOut()
	{
		float t = 0f;
		SetFadeAlpha(0f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 2f)
		{
			SetFadeAlpha(t);
			yield return null;
		}
		SetFadeAlpha(1f);
	}

	private void FadeIn()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeIn());
	}

	private IEnumerator CFadeIn()
	{
		float t = 0f;
		SetFadeAlpha(1f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 2f)
		{
			SetFadeAlpha(1f - t);
			yield return null;
		}
		SetFadeAlpha(0f);
		Hide();
	}

	private void SetFadeAlpha(float alpha)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Color color = _renderer.color;
		color.a = alpha;
		_renderer.color = color;
	}
}
