using System.Collections;
using Level;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

public class CastleCameraController : MonoBehaviour
{
	private enum State
	{
		Inside,
		OutsideDiving,
		Outside
	}

	[SerializeField]
	private float _lightColorTransitionTime;

	[Space]
	[SerializeField]
	private CameraZone _inside;

	[SerializeField]
	private Color _insideLightColor;

	[SerializeField]
	private float _insideLightIntensity;

	[SerializeField]
	[Space]
	private BoxCollider2D _outside;

	[SerializeField]
	private CameraZone _outsideCameraZone;

	[SerializeField]
	private Color _outsideLightColor;

	[SerializeField]
	private float _outsideLightIntensity;

	[Space]
	[SerializeField]
	private Collider2D _portal;

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private SpriteRenderer _cover;

	private State _state;

	private CameraController _cameraController;

	private void Awake()
	{
		_cameraController = Scene<GameBase>.instance.cameraController;
	}

	private IEnumerator CUpdate()
	{
		yield return null;
		while (true)
		{
			if ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
			{
				yield return null;
				continue;
			}
			Vector3 position = ((Component)Singleton<Service>.Instance.levelManager.player).transform.position;
			if (((Bounds)(ref _inside.bounds)).Contains(position))
			{
				if (_state != 0)
				{
					yield return CTransitionToInside();
				}
			}
			else
			{
				Bounds bounds = ((Collider2D)_outside).bounds;
				if (((Bounds)(ref bounds)).Contains(position))
				{
					yield return CTransitionToOutside(position);
				}
			}
			yield return null;
		}
	}

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CUpdate());
	}

	private IEnumerator CTransitionToInside()
	{
		_state = State.Inside;
		float time = 0f;
		Color color = _cover.color;
		Map.Instance.ChangeLight(_insideLightColor, _insideLightIntensity, _lightColorTransitionTime);
		_cameraController.pause = true;
		_cameraController.zone = null;
		Vector3 originalPosition = ((Component)_cameraController).transform.position;
		Vector3 targetPosition = _inside.GetClampedPosition(_cameraController.camera, ((Component)_cameraController).transform.position);
		for (; time < 1f; time += Time.unscaledDeltaTime * 1.5f)
		{
			yield return null;
			((Component)_cameraController).transform.position = Vector3.Lerp(originalPosition, targetPosition, _curve.Evaluate(time));
		}
		for (; time < 1f; time += Time.unscaledDeltaTime * 1.5f)
		{
			yield return null;
			color.a = time;
			_cover.color = color;
			((Component)_cameraController).transform.position = Vector3.Lerp(originalPosition, targetPosition, time);
		}
		_cameraController.pause = false;
		_cameraController.zone = _inside;
	}

	private IEnumerator CTransitionToOutside(Vector3 playerPosition)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		_state = State.Outside;
		Map.Instance.ChangeLight(_outsideLightColor, _outsideLightIntensity, _lightColorTransitionTime);
		_cameraController.zone = _outsideCameraZone;
		Bounds bounds = _portal.bounds;
		if (((Bounds)(ref bounds)).Contains(playerPosition))
		{
			for (float time = 0f; time < 1f; time += Time.unscaledDeltaTime)
			{
				yield return null;
			}
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			if (Singleton<Service>.Instance.levelManager.currentChapter.type == Chapter.Type.Castle)
			{
				Singleton<Service>.Instance.levelManager.Load(Chapter.Type.Chapter1);
			}
			else
			{
				Singleton<Service>.Instance.levelManager.Load(Chapter.Type.HardmodeChapter1);
			}
		}
	}
}
