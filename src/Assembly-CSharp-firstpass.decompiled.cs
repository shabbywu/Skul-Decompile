using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyVersion("0.0.0.0")]
public class CopyCameraSize : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Camera _camera;

	[SerializeField]
	private Camera _sourceCamera;

	private void OnPreRender()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_camera.orthographicSize = _sourceCamera.orthographicSize;
		_camera.rect = _sourceCamera.rect;
	}
}
public static class Layers
{
	public const int @default = 0;

	public const int terrain = 8;

	public const int playerBody = 9;

	public const int monsterBody = 10;

	public const int prop = 11;

	public const int interaction = 12;

	public const int playerProjectile = 15;

	public const int monsterProjectile = 16;

	public const int platform = 17;

	public const int terrainFoothold = 18;

	public const int projectileBlock = 19;

	public const int playerBlock = 23;

	public const int minimap = 25;

	public const int collideWithTerrain = 27;

	public static readonly LayerMask footholdMask = LayerMask.op_Implicit(393216);

	public static readonly LayerMask groundMask = LayerMask.op_Implicit(8782080);

	public static readonly LayerMask terrainMask = LayerMask.op_Implicit(8651008);

	public static readonly LayerMask terrainMaskForProjectile = LayerMask.op_Implicit(786688);
}
public class Loader : MonoBehaviour
{
	[Serializable]
	private class UnityFloatEvent : UnityEvent<float>
	{
	}

	[SerializeField]
	private string[] _scenesToLoad;

	private List<AsyncOperation> _asyncOperations;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CLoad());
	}

	private IEnumerator CLoad()
	{
		yield return (object)new WaitForSeconds(1f);
		_asyncOperations = new List<AsyncOperation>(_scenesToLoad.Length);
		string[] scenesToLoad = _scenesToLoad;
		foreach (string scene in scenesToLoad)
		{
			yield return CLoadScene(scene);
		}
		foreach (AsyncOperation asyncOperation in _asyncOperations)
		{
			asyncOperation.allowSceneActivation = true;
		}
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	private IEnumerator CLoadScene(string scene)
	{
		int result;
		AsyncOperation operation = ((!int.TryParse(scene, out result)) ? SceneManager.LoadSceneAsync(scene, (LoadSceneMode)1) : SceneManager.LoadSceneAsync(result, (LoadSceneMode)1));
		_asyncOperations.Add(operation);
		operation.allowSceneActivation = false;
		while (operation.progress < 0.9f)
		{
			yield return null;
		}
	}
}
public enum Rarity
{
	Common,
	Rare,
	Unique,
	Legendary
}
[Serializable]
public class RarityPossibilities
{
	public static readonly ReadOnlyCollection<Rarity> values = EnumValues<Rarity>.Values;

	[SerializeField]
	[Range(0f, 100f)]
	private int[] _possibilities;

	public int this[int index] => _possibilities[index];

	public int this[Rarity rarity] => _possibilities[(int)rarity];

	public Rarity Evaluate(Random random)
	{
		return Evaluate(random, _possibilities);
	}

	public Rarity Evaluate()
	{
		return Evaluate(new Random(), _possibilities);
	}

	public RarityPossibilities(params int[] possibilities)
	{
		_possibilities = possibilities;
	}

	public static Rarity Evaluate(Random random, int[] possibilities)
	{
		int maxValue = possibilities.Sum();
		int num = random.Next(0, maxValue) + 1;
		for (int i = 0; i < possibilities.Length; i++)
		{
			num -= possibilities[i];
			if (num <= 0)
			{
				return values[i];
			}
		}
		return values[0];
	}
}
[Serializable]
public class RarityPrices
{
	public static readonly ReadOnlyCollection<string> names = EnumValues<Rarity>.Names;

	public static readonly ReadOnlyCollection<Rarity> values = EnumValues<Rar