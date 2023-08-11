using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack;

public class DemoPlayerHealth : MonoBehaviour
{
	public GameObject bloodSplatPrefab;

	public GameObject playerPrefab;

	public Button restartButton;

	public Text healthText;

	private LineBasedLaser[] allLasersInScene;

	public ParticleSystem bloodParticleSystem;

	[SerializeField]
	private int _healthPoints;

	public int HealthPoints
	{
		get
		{
			return _healthPoints;
		}
		set
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			_healthPoints = value;
			if (_healthPoints <= 0)
			{
				if ((Object)(object)bloodSplatPrefab != (Object)null)
				{
					Object.Instantiate<GameObject>(bloodSplatPrefab, ((Component)this).transform.position, Quaternion.identity);
				}
				healthText.text = "Health: 0";
				((Component)this).gameObject.GetComponent<Renderer>().enabled = false;
				((Behaviour)((Component)this).gameObject.GetComponent<PlayerMovement>()).enabled = false;
				((Component)restartButton).gameObject.SetActive(true);
				LineBasedLaser[] array = allLasersInScene;
				foreach (LineBasedLaser obj in array)
				{
					obj.OnLaserHitTriggered -= LaserOnOnLaserHitTriggered;
					obj.SetLaserState(enabledStatus: false);
				}
			}
			else
			{
				healthText.text = "Health: " + _healthPoints;
			}
		}
	}

	private void Start()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		_healthPoints = 10;
		if ((Object)(object)restartButton == (Object)null)
		{
			restartButton = ((IEnumerable<Button>)Object.FindObjectsOfType<Button>()).FirstOrDefault((Func<Button, bool>)((Button b) => ((Object)b).name == "ButtonReplay"));
		}
		healthText = ((IEnumerable<Text>)Object.FindObjectsOfType<Text>()).FirstOrDefault((Func<Text, bool>)((Text t) => ((Object)t).name == "TextHealth"));
		healthText.text = "Health: 10";
		allLasersInScene = Object.FindObjectsOfType<LineBasedLaser>();
		((UnityEventBase)restartButton.onClick).RemoveAllListeners();
		((UnityEvent)restartButton.onClick).AddListener(new UnityAction(OnRestartButtonClick));
		if (allLasersInScene.Any())
		{
			LineBasedLaser[] array = allLasersInScene;
			foreach (LineBasedLaser obj in array)
			{
				obj.OnLaserHitTriggered += LaserOnOnLaserHitTriggered;
				obj.SetLaserState(enabledStatus: true);
				obj.targetGo = ((Component)this).gameObject;
			}
		}
		((Behaviour)((Component)this).gameObject.GetComponent<PlayerMovement>()).enabled = true;
		((Component)this).gameObject.GetComponent<Renderer>().enabled = true;
		((Component)restartButton).gameObject.SetActive(false);
	}

	private void OnRestartButtonClick()
	{
		CreateNewPlayer();
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	private void CreateNewPlayer()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		GameObject targetGo = Object.Instantiate<GameObject>(playerPrefab, Vector2.op_Implicit(new Vector2(6.26f, -2.8f)), Quaternion.identity);
		LineBasedLaser[] array = allLasersInScene;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].targetGo = targetGo;
		}
	}

	private void LaserOnOnLaserHitTriggered(RaycastHit2D hitInfo)
	{
		if ((Object)(object)((Component)((RaycastHit2D)(ref hitInfo)).collider).gameObject == (Object)(object)((Component)this).gameObject && (Object)(object)bloodParticleSystem != (Object)null)
		{
			bloodParticleSystem.Play();
			HealthPoints--;
		}
	}

	private void Update()
	{
	}
}
