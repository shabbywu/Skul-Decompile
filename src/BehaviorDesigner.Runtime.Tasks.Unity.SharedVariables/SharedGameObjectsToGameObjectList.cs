using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;

[TaskDescription("Sets the SharedGameObjectList values from the GameObjects. Returns Success.")]
[TaskCategory("Unity/SharedVariable")]
public class SharedGameObjectsToGameObjectList : Action
{
	[Tooltip("The GameObjects value")]
	public SharedGameObject[] gameObjects;

	[Tooltip("The SharedTransformList to set")]
	[RequiredField]
	public SharedGameObjectList storedGameObjectList;

	public override void OnAwake()
	{
		((SharedVariable<List<GameObject>>)storedGameObjectList).Value = new List<GameObject>();
	}

	public override TaskStatus OnUpdate()
	{
		if (gameObjects != null && gameObjects.Length != 0)
		{
			((SharedVariable<List<GameObject>>)storedGameObjectList).Value.Clear();
			for (int i = 0; i < gameObjects.Length; i++)
			{
				((SharedVariable<List<GameObject>>)storedGameObjectList).Value.Add(((SharedVariable<GameObject>)gameObjects[i]).Value);
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		gameObjects = null;
		storedGameObjectList = null;
	}
}
