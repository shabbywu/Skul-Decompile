using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Gear.Synergy;

[Serializable]
public class InscriptionSettings
{
	[SerializeField]
	private AssetReference _reference;

	[SerializeField]
	private AssetReference _omenItem;

	[SerializeField]
	private int[] _steps;

	public AssetReference reference => _reference;

	public AssetReference omenItem => _omenItem;

	public int[] steps => _steps;
}
