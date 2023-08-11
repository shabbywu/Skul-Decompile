using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack;

public class LaserSettingsDemoScene : MonoBehaviour
{
	public LineBasedLaser LineBasedLaser;

	public DemoFollowScript FollowScript;

	public Toggle toggleisActive;

	public Toggle toggleignoreCollisions;

	public Toggle togglelaserRotationEnabled;

	public Toggle togglelerpLaserRotation;

	public Toggle toggleuseArc;

	public Toggle toggleTargetMouse;

	public Slider slidertexOffsetSpeed;

	public Slider sliderlaserArcMaxYDown;

	public Slider sliderlaserArcMaxYUp;

	public Slider slidermaxLaserRaycastDistance;

	public Slider sliderturningRate;

	public Button buttonSwitch;

	public Text textValue;

	public Material[] LaserMaterials;

	private int selectedMaterialIndex;

	private int maxSelectedIndex;

	private bool targetShouldTrackMouse;

	private void Start()
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		if ((Object)(object)LineBasedLaser == (Object)null)
		{
			Debug.LogError((object)"You need to reference a valid LineBasedLaser on this script.");
		}
		((UnityEvent<bool>)(object)toggleisActive.onValueChanged).AddListener((UnityAction<bool>)OnLaserActiveChanged);
		((UnityEvent<bool>)(object)toggleignoreCollisions.onValueChanged).AddListener((UnityAction<bool>)OnLaserToggleCollisionsChanged);
		((UnityEvent<bool>)(object)togglelaserRotationEnabled.onValueChanged).AddListener((UnityAction<bool>)OnLaserAllowRotationChanged);
		((UnityEvent<bool>)(object)togglelerpLaserRotation.onValueChanged).AddListener((UnityAction<bool>)OnLaserLerpRotationChanged);
		((UnityEvent<bool>)(object)toggleuseArc.onValueChanged).AddListener((UnityAction<bool>)OnUseArcValueChanged);
		((UnityEvent<bool>)(object)toggleTargetMouse.onValueChanged).AddListener((UnityAction<bool>)OnToggleFollowMouse);
		((UnityEvent<float>)(object)slidertexOffsetSpeed.onValueChanged).AddListener((UnityAction<float>)OnTextureOffsetSpeedChanged);
		((UnityEvent<float>)(object)sliderlaserArcMaxYDown.onValueChanged).AddListener((UnityAction<float>)OnArcMaxYDownValueChanged);
		((UnityEvent<float>)(object)sliderlaserArcMaxYUp.onValueChanged).AddListener((UnityAction<float>)OnArcMaxYUpValueChanged);
		((UnityEvent<float>)(object)slidermaxLaserRaycastDistance.onValueChanged).AddListener((UnityAction<float>)OnLaserRaycastDistanceChanged);
		((UnityEvent<float>)(object)sliderturningRate.onValueChanged).AddListener((UnityAction<float>)OnLaserTurningRateChanged);
		((UnityEvent)buttonSwitch.onClick).AddListener(new UnityAction(OnButtonClick));
		selectedMaterialIndex = 1;
		maxSelectedIndex = LaserMaterials.Length - 1;
	}

	private void OnToggleFollowMouse(bool followMouse)
	{
		targetShouldTrackMouse = followMouse;
		if (targetShouldTrackMouse)
		{
			((Behaviour)FollowScript).enabled = false;
		}
		else
		{
			((Behaviour)FollowScript).enabled = true;
		}
	}

	private void OnButtonClick()
	{
		if (selectedMaterialIndex < maxSelectedIndex)
		{
			selectedMaterialIndex++;
			((Renderer)LineBasedLaser.laserLineRenderer).material = LaserMaterials[selectedMaterialIndex];
			((Renderer)LineBasedLaser.laserLineRendererArc).material = LaserMaterials[selectedMaterialIndex];
			((Component)LineBasedLaser.hitSparkParticleSystem).GetComponent<Renderer>().material = LaserMaterials[selectedMaterialIndex];
		}
		else
		{
			selectedMaterialIndex = 0;
			((Renderer)LineBasedLaser.laserLineRenderer).material = LaserMaterials[selectedMaterialIndex];
			((Renderer)LineBasedLaser.laserLineRendererArc).material = LaserMaterials[selectedMaterialIndex];
			((Component)LineBasedLaser.hitSparkParticleSystem).GetComponent<Renderer>().material = LaserMaterials[selectedMaterialIndex];
		}
	}

	private void OnLaserTurningRateChanged(float turningRate)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.turningRate = turningRate;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser turning rate: " + Math.Round(turningRate, 2);
	}

	private void OnLaserRaycastDistanceChanged(float raycastDistance)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.maxLaserRaycastDistance = raycastDistance;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser raycast max distance: " + Math.Round(raycastDistance, 2);
	}

	private void OnArcMaxYUpValueChanged(float maxYValueUp)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.laserArcMaxYUp = maxYValueUp;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser arc maximum up arc height: " + Math.Round(maxYValueUp, 2);
	}

	private void OnArcMaxYDownValueChanged(float maxYValueDown)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.laserArcMaxYDown = maxYValueDown;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser arc maximum down arc height: " + Math.Round(maxYValueDown, 2);
	}

	private void OnTextureOffsetSpeedChanged(float offsetSpeed)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.laserTexOffsetSpeed = offsetSpeed;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser texture offset speed: " + Math.Round(offsetSpeed, 2);
	}

	private void OnUseArcValueChanged(bool useArc)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.useArc = useArc;
		((Selectable)sliderlaserArcMaxYDown).interactable = useArc;
		((Selectable)sliderlaserArcMaxYUp).interactable = useArc;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser arc enabled: " + useArc;
	}

	private void OnLaserLerpRotationChanged(bool lerpLaserRotation)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.lerpLaserRotation = lerpLaserRotation;
		((Selectable)sliderturningRate).interactable = lerpLaserRotation;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Lerp laser rotation: " + lerpLaserRotation;
	}

	private void OnLaserAllowRotationChanged(bool allowRotation)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.laserRotationEnabled = allowRotation;
		((Selectable)togglelerpLaserRotation).interactable = allowRotation;
		((Selectable)sliderturningRate).interactable = allowRotation;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser rotation enabled: " + allowRotation;
	}

	private void OnLaserToggleCollisionsChanged(bool ignoreCollisions)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.ignoreCollisions = ignoreCollisions;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Ignore laser collisions: " + ignoreCollisions;
	}

	private void OnLaserActiveChanged(bool state)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		LineBasedLaser.SetLaserState(state);
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser active: " + state;
	}

	private void Update()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (targetShouldTrackMouse)
		{
			Vector3 val = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(val.x, val.y);
			LineBasedLaser.targetGo.transform.position = Vector2.op_Implicit(val2);
		}
	}
}
