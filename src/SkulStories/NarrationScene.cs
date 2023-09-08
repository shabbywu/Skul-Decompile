using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkulStories;

public class NarrationScene : MonoBehaviour
{
	[SerializeField]
	private Image _scene;

	[SerializeField]
	private Image _overlayScene;

	[SerializeField]
	private Color _target;

	private Vector3 _position;

	private bool _changed;

	private Vector2 _pivot;

	private Image _currentScene;

	private Color _originColor;

	public bool changed => _changed;

	public Image scene => _scene;

	public Image overlayScene => _overlayScene;

	private void Start()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_position = ((Component)_scene).transform.localPosition;
		_originColor = new Color(1f, 1f, 1f, 0f);
	}

	private void Initialize()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_overlayScene).color = _originColor;
		((Graphic)_currentScene).color = _originColor;
		((Graphic)_currentScene).rectTransform.pivot = _pivot;
		((Component)_currentScene).transform.localPosition = _position;
	}

	public void Change(Sprite sprite, bool overlay)
	{
		if (overlay)
		{
			ChangeOverlayScene(sprite);
		}
		else
		{
			ChangeScene(sprite);
		}
		_changed = true;
	}

	private void ChangeOverlayScene(Sprite sprite)
	{
		_overlayScene.sprite = sprite;
		_currentScene = _overlayScene;
	}

	private void ChangeScene(Sprite sprite)
	{
		_scene.sprite = sprite;
		_currentScene = _scene;
	}

	public void SetPivot(Vector2 pivot)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_pivot = pivot;
	}

	public IEnumerator CFadeIn(float speed)
	{
		Initialize();
		Color startColor = ((Graphic)_currentScene).color;
		Color different = _target - ((Graphic)_currentScene).color;
		float time = 0f;
		while (time < speed)
		{
			time += Chronometer.global.deltaTime;
			((Graphic)_currentScene).color = startColor + different * (time / speed);
			yield return null;
		}
		_changed = false;
		((Graphic)_currentScene).color = _target;
		_scene.sprite = _currentScene.sprite;
		((Graphic)_scene).rectTransform.pivot = _pivot;
		((Component)_scene).transform.localPosition = _position;
	}
}
