using System.Collections;
using Characters.Abilities.Constraints;
using Scenes;
using UnityEngine;

namespace Characters;

public sealed class CharacterSpotLight : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private Transform _spotlight;

	[SerializeField]
	private Renderer _renderer;

	private Constraint[] _constraints = new Constraint[4]
	{
		new LetterBox(),
		new Dialogue(),
		new Story(),
		new Characters.Abilities.Constraints.EndingCredit()
	};

	private float _fadeSpeed = 1.5f;

	private bool activated;

	private CoroutineReference _fadeCoroutine;

	private bool _constranitsFade;

	private void Update()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (!activated)
		{
			return;
		}
		if ((Object)(object)_owner == (Object)null)
		{
			Object.Destroy((Object)(object)((Component)_spotlight).gameObject);
			return;
		}
		if (Vector2.SqrMagnitude(Vector2.op_Implicit(((Component)Scene<GameBase>.instance.cameraController).transform.position - ((Component)_owner).transform.position)) > 1000f)
		{
			((Component)_spotlight).gameObject.SetActive(false);
		}
		else
		{
			((Component)_spotlight).gameObject.SetActive(true);
		}
		_spotlight.position = ((Component)_owner).transform.position;
		if (_constraints.Pass())
		{
			if (!_constranitsFade)
			{
				return;
			}
			_constranitsFade = false;
			((CoroutineReference)(ref _fadeCoroutine)).Stop();
			_fadeCoroutine = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CActivate());
		}
		if (!_constranitsFade)
		{
			_constranitsFade = true;
			((CoroutineReference)(ref _fadeCoroutine)).Stop();
			_fadeCoroutine = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CDeactivate());
		}
	}

	public void Activate()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		activated = true;
		((CoroutineReference)(ref _fadeCoroutine)).Stop();
		_fadeCoroutine = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CActivate());
	}

	private IEnumerator CActivate()
	{
		((Component)_spotlight).gameObject.SetActive(true);
		for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * _fadeSpeed)
		{
			SetFadeAlpha(t);
			yield return null;
		}
		SetFadeAlpha(1f);
	}

	public void Deactivate()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		activated = false;
		((CoroutineReference)(ref _fadeCoroutine)).Stop();
		_fadeCoroutine = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CDeactivate());
	}

	private IEnumerator CDeactivate()
	{
		for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * _fadeSpeed)
		{
			SetFadeAlpha(1f - t);
			yield return null;
		}
		SetFadeAlpha(0f);
		((Component)_spotlight).gameObject.SetActive(false);
	}

	private void SetFadeAlpha(float alpha)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Color color = _renderer.material.color;
		color.a = alpha;
		_renderer.material.color = color;
	}

	private void OnDestroy()
	{
		Object.Destroy((Object)(object)((Component)_spotlight).gameObject);
	}
}
