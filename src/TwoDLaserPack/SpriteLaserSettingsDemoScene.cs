using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack;

public class SpriteLaserSettingsDemoScene : MonoBehaviour
{
	public SpriteBasedLaser SpriteBasedLaser;

	public Toggle toggleisActive;

	public Toggle toggleignoreCollisions;

	public Toggle togglelaserRotationEnabled;

	public Toggle togglelerpLaserRotation;

	public Toggle toggleuseArc;

	public Toggle toggleOscillateLaser;

	public Slider sliderlaserArcMaxYDown;

	public Slider sliderlaserArcMaxYUp;

	public Slider slidermaxLaserRaycastDistance;

	public Slider sliderturningRate;

	public Slider sliderOscillationThreshold;

	public Slider sliderOscillationSpeed;

	public Button buttonSwitch;

	public Text textValue;

	public Material[] LaserMaterials;

	public GameObject laserStartPieceBlue;

	public GameObject laserStartPieceRed;

	public GameObject laserMidPieceBlue;

	public GameObject laserMidPieceRed;

	public GameObject laserEndPieceBlue;

	public GameObject laserEndPieceRed;

	private int selectedMaterialIndex;

	private int maxSelectedIndex;

	private void Start()
	{
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		if ((Object)(object)SpriteBasedLaser == (Object)null)
		{
			Debug.LogError((object)"You need to reference a valid LineBasedLaser on this script.");
		}
		((UnityEvent<bool>)(object)toggleisActive.onValueChanged).AddListener((UnityAction<bool>)OnLaserActiveChanged);
		((UnityEvent<bool>)(object)toggleignoreCollisions.onValueChanged).AddListener((UnityAction<bool>)OnLaserToggleCollisionsChanged);
		((UnityEvent<bool>)(object)togglelaserRotationEnabled.onValueChanged).AddListener((UnityAction<bool>)OnLaserAllowRotationChanged);
		((UnityEvent<bool>)(object)togglelerpLaserRotation.onValueChanged).AddListener((UnityAction<bool>)OnLaserLerpRotationChanged);
		((UnityEvent<bool>)(object)toggleuseArc.onValueChanged).AddListener((UnityAction<bool>)OnUseArcValueChanged);
		((UnityEvent<bool>)(object)toggleOscillateLaser.onValueChanged).AddListener((UnityAction<bool>)OnOscillateLaserChanged);
		((UnityEvent<float>)(object)sliderlaserArcMaxYDown.onValueChanged).AddListener((UnityAction<float>)OnArcMaxYDownValueChanged);
		((UnityEvent<float>)(object)sliderlaserArcMaxYUp.onValueChanged).AddListener((UnityAction<float>)OnArcMaxYUpValueChanged);
		((UnityEvent<float>)(object)slidermaxLaserRaycastDistance.onValueChanged).AddListener((UnityAction<float>)OnLaserRaycastDistanceChanged);
		((UnityEvent<float>)(object)sliderturningRate.onValueChanged).AddListener((UnityAction<float>)OnLaserTurningRateChanged);
		((UnityEvent<float>)(object)sliderOscillationThreshold.onValueChanged).AddListener((UnityAction<float>)OnOscillationThresholdChanged);
		((UnityEvent<float>)(object)sliderOscillationSpeed.onValueChanged).AddListener((UnityAction<float>)OnOscillationSpeedChanged);
		((UnityEvent)buttonSwitch.onClick).AddListener(new UnityAction(OnButtonClick));
		selectedMaterialIndex = 1;
		maxSelectedIndex = LaserMaterials.Length - 1;
	}

	private void OnOscillationSpeedChanged(float oscillationSpeed)
	{
		SpriteBasedLaser.oscillationSpeed = oscillationSpeed;
	}

	private void OnOscillationThresholdChanged(float oscillationThreshold)
	{
		SpriteBasedLaser.oscillationThreshold = oscillationThreshold;
	}

	private void OnOscillateLaserChanged(bool oscillateLaser)
	{
		SpriteBasedLaser.oscillateLaser = oscillateLaser;
	}

	private void OnButtonClick()
	{
		if (selectedMaterialIndex < maxSelectedIndex)
		{
			selectedMaterialIndex++;
			((Renderer)SpriteBasedLaser.laserLineRendererArc).material = LaserMaterials[selectedMaterialIndex];
			((Component)SpriteBasedLaser.hitSparkParticleSystem).GetComponent<Renderer>().material = LaserMaterials[selectedMaterialIndex];
			SpriteBasedLaser.laserStartPiece = laserStartPieceRed;
			SpriteBasedLaser.laserMiddlePiece = laserMidPieceRed;
			SpriteBasedLaser.laserEndPiece = laserEndPieceRed;
		}
		else
		{
			selectedMaterialIndex = 0;
			((Renderer)SpriteBasedLaser.laserLineRendererArc).material = LaserMaterials[selectedMaterialIndex];
			((Component)SpriteBasedLaser.hitSparkParticleSystem).GetComponent<Renderer>().material = LaserMaterials[selectedMaterialIndex];
			SpriteBasedLaser.laserStartPiece = laserStartPieceBlue;
			SpriteBasedLaser.laserMiddlePiece = laserMidPieceBlue;
			SpriteBasedLaser.laserEndPiece = laserEndPieceBlue;
		}
		SpriteBasedLaser.DisableLaserGameObjectComponents();
	}

	private void OnLaserTurningRateChanged(float turningRate)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.turningRate = turningRate;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser turning rate: " + Math.Round(turningRate, 2);
	}

	private void OnLaserRaycastDistanceChanged(float raycastDistance)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.maxLaserRaycastDistance = raycastDistance;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser raycast max distance: " + Math.Round(raycastDistance, 2);
	}

	private void OnArcMaxYUpValueChanged(float maxYValueUp)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.laserArcMaxYUp = maxYValueUp;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser arc maximum up arc height: " + Math.Round(maxYValueUp, 2);
	}

	private void OnArcMaxYDownValueChanged(float maxYValueDown)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.laserArcMaxYDown = maxYValueDown;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser arc maximum down arc height: " + Math.Round(maxYValueDown, 2);
	}

	private void OnUseArcValueChanged(bool useArc)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.useArc = useArc;
		((Selectable)sliderlaserArcMaxYDown).interactable = useArc;
		((Selectable)sliderlaserArcMaxYUp).interactable = useArc;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser arc enabled: " + useArc;
	}

	private void OnLaserLerpRotationChanged(bool lerpLaserRotation)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.lerpLaserRotation = lerpLaserRotation;
		((Selectable)sliderturningRate).interactable = lerpLaserRotation;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Lerp laser rotation: " + lerpLaserRotation;
	}

	private void OnLaserAllowRotationChanged(bool allowRotation)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.laserRotationEnabled = allowRotation;
		((Selectable)togglelerpLaserRotation).interactable = allowRotation;
		((Selectable)sliderturningRate).interactable = allowRotation;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser rotation enabled: " + allowRotation;
	}

	private void OnLaserToggleCollisionsChanged(bool ignoreCollisions)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.ignoreCollisions = ignoreCollisions;
		((Graphic)textValue).color = Color.white;
		textValue.text = "Ignore laser collisions: " + ignoreCollisions;
	}

	private void OnLaserActiveChanged(bool state)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		SpriteBasedLaser.SetLaserState(state);
		((Graphic)textValue).color = Color.white;
		textValue.text = "Laser active: " + state;
	}

	private void Update()
	{
	}
}
