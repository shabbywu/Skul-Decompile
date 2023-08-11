using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class ChimeraDie : MonoBehaviour
{
	[SerializeField]
	[Header("Pause")]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _pauesOperations;

	[Subcomponent(typeof(OperationInfos))]
	[Header("Ready")]
	[SerializeField]
	private OperationInfos _readyOperations;

	[SerializeField]
	[Header("Start")]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _startOperations;

	[Header("BreakTerrain")]
	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _breakTerrainOperations;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	[Header("Struggle1")]
	private OperationInfos _struggle1Operations;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	[Header("Struggle2")]
	private OperationInfos _struggle2Operations;

	[Header("Fall")]
	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _fallOperations;

	[Header("Water")]
	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _waterOperations;

	[Header("KillEnemy")]
	[SerializeField]
	private Collider2D _killEnemyRange;

	private void Awake()
	{
		_pauesOperations.Initialize();
		_readyOperations.Initialize();
		_startOperations.Initialize();
		_breakTerrainOperations.Initialize();
		_struggle1Operations.Initialize();
		_struggle2Operations.Initialize();
		_fallOperations.Initialize();
		_waterOperations.Initialize();
	}

	public void KillAllEnemyInBounds(AIController controller)
	{
		((MonoBehaviour)this).StartCoroutine(KillLoop(controller));
	}

	private IEnumerator KillLoop(AIController controller)
	{
		float duration = 10f;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			List<Character> list = controller.FindEnemiesInRange(_killEnemyRange);
			for (int i = 0; i < list.Count; i++)
			{
				if (!((Object)(object)controller.character == (Object)(object)list[i]) && !((Object)(object)list[i].health == (Object)null))
				{
					list[i].health.Kill();
				}
			}
			elapsed += ((ChronometerBase)controller.character.chronometer.animation).deltaTime;
			yield return null;
		}
	}

	public void Pause(Character character)
	{
		((Component)_pauesOperations).gameObject.SetActive(true);
		_pauesOperations.Run(character);
	}

	public void Ready(Character character)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
	}

	public void Down(Character character)
	{
		((Component)_startOperations).gameObject.SetActive(true);
		_startOperations.Run(character);
	}

	public void BreakTerrain(Character character)
	{
		((Component)_breakTerrainOperations).gameObject.SetActive(true);
		_breakTerrainOperations.Run(character);
	}

	public void Struggle1(Character character)
	{
		((Component)_struggle1Operations).gameObject.SetActive(true);
		_struggle1Operations.Run(character);
	}

	public void Struggle2(Character character)
	{
		((Component)_struggle2Operations).gameObject.SetActive(true);
		_struggle2Operations.Run(character);
	}

	public void Fall(Character character)
	{
		((Component)_fallOperations).gameObject.SetActive(true);
		_fallOperations.Run(character);
	}

	public void Water(Character character, Chapter3Script script)
	{
		((Component)_waterOperations).gameObject.SetActive(true);
		_waterOperations.Run(character);
		script.EndOutro();
	}
}
