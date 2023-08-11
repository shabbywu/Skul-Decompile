using System;
using Characters.Player;
using Data;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
	private const int _shakeBaseFps = 60;

	private const float _maxShakeInterval = 1f / 60f;

	[NonSerialized]
	public bool pause;

	[SerializeField]
	[GetComponent]
	private Camera _camera;

	[GetComponent]
	[SerializeField]
	private PixelPerfectCamera _pixelPerfectCamera;

	[SerializeField]
	private Vector3 _offset;

	[SerializeField]
	private Method _moveEaseMethod = (Method)21;

	[SerializeField]
	private float _moveSpeed = 1f;

	private float _moveTime;

	private EasingFunction _moveEase;

	[SerializeField]
	private Method _zoomEaseMethod = (Method)21;

	[SerializeField]
	private float _zoomSpeed = 1f;

	private float _zoomTime;

	private float _formerZoom = 1f;

	private float _targetZoom = 1f;

	private EasingFunction _zoomEase;

	[SerializeField]
	private float _trackSpeed = 1f;

	private Transform _targetToTrack;

	private Vector3 _targetPosition;

	private Vector3 _position;

	private Vector3 _delta;

	private Vector3 _shakeAmount;

	private float _timeToNextShake;

	private PlayerCameraController _playerCameraController;

	public readonly MaxOnlyTimedFloats shake = new MaxOnlyTimedFloats();

	public CameraZone zone;

	public float trackSpeed
	{
		get
		{
			return _trackSpeed;
		}
		set
		{
			_trackSpeed = value;
		}
	}

	public Vector3 delta
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (!pause)
			{
				return _delta;
			}
			return Vector3.zero;
		}
	}

	public PixelPerfectCamera pixelPerfectcamera => _pixelPerfectCamera;

	public Camera camera => _camera;

	public float zoom => _pixelPerfectCamera.zoom;

	private void Awake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		_camera.transparencySortMode = (TransparencySortMode)2;
		_position = ((Component)this).transform.position;
		_moveEase = new EasingFunction(_moveEaseMethod);
		_zoomEase = new EasingFunction(_zoomEaseMethod);
	}

	public void Update()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		if (pause)
		{
			_position = ((Component)this).transform.position;
			return;
		}
		Vector3 val;
		if (Object.op_Implicit((Object)(object)_targetToTrack))
		{
			val = ((!((Object)(object)_playerCameraController == (Object)null)) ? Vector3.Lerp(_position, _playerCameraController.trackPosition, Time.unscaledDeltaTime * _playerCameraController.trackSpeed) : Vector3.Lerp(_position, _targetToTrack.position + _offset, Time.unscaledDeltaTime * _trackSpeed));
		}
		else
		{
			_moveTime += Time.unscaledDeltaTime * _moveSpeed;
			if (_moveTime >= 1f)
			{
				val = _targetPosition;
			}
			else
			{
				float num = ((EasingFunction)(ref _moveEase)).function.Invoke(0f, 1f, _moveTime);
				val = Vector3.LerpUnclamped(_position, _targetPosition, num);
			}
		}
		if ((Object)(object)_pixelPerfectCamera != (Object)null)
		{
			_zoomTime += Time.unscaledDeltaTime * _zoomSpeed;
			if (_zoomTime >= 1f)
			{
				_pixelPerfectCamera.zoom = _targetZoom;
			}
			else
			{
				_pixelPerfectCamera.zoom = ((EasingFunction)(ref _zoomEase)).function.Invoke(_formerZoom, _targetZoom, _zoomTime);
			}
		}
		val.z = _position.z;
		Vector3 position = _position;
		_position = zone?.GetClampedPosition(_camera, val) ?? val;
		_delta = _position - position;
		_timeToNextShake -= Time.deltaTime;
		if (_timeToNextShake < 0f)
		{
			_shakeAmount = Random.insideUnitSphere * shake.value * GameData.Settings.cameraShakeIntensity;
			_shakeAmount *= 2f;
			_timeToNextShake = 1f / 60f;
		}
		((Component)this).transform.position = _position + _shakeAmount;
		shake.Update();
	}

	public void StartTrack(Transform target)
	{
		_playerCameraController = ((Component)target).GetComponent<PlayerCameraController>();
		_targetToTrack = target;
	}

	public void StopTrack()
	{
		_targetToTrack = null;
	}

	public void Move(Vector3 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		position += _offset;
		position.z = _position.z;
		_position = (_targetPosition = position);
		_moveTime = 0f;
	}

	public void Zoom(float percent, float zoomSpeed = 1f)
	{
		_targetZoom = percent;
		_zoomSpeed = zoomSpeed;
		_zoomTime = 0f;
		_formerZoom = _pixelPerfectCamera.zoom;
	}

	public void RenderEndingScene()
	{
		_playerCameraController.RenderDeathCamera();
	}

	public void Shake(float amount, float duration)
	{
		shake.Attach((object)this, amount, duration);
	}

	public void UpdateCameraPosition()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		_position = ((Component)_camera).gameObject.transform.position;
	}
}
