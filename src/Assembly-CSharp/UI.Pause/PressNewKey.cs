using System;
using GameResources;
using InControl;
using UnityEngine;
using UserInput;

namespace UI.Pause;

public class PressNewKey : MonoBehaviour
{
	private static readonly KeyBindingSource escapeBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)13 });

	private static readonly KeyBindingSource enterBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)72 });

	private static readonly KeyBindingSource leftShiftBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)5 });

	private static readonly KeyBindingSource rightShiftBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)9 });

	private static readonly KeyBindingSource shiftBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)1 });

	private static readonly KeyBindingSource leftControlBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)8 });

	private static readonly KeyBindingSource rightControlBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)12 });

	private static readonly KeyBindingSource controlBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)4 });

	private static readonly KeyBindingSource leftAltBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)6 });

	private static readonly KeyBindingSource rightAltBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)10 });

	private static readonly KeyBindingSource altBinding = new KeyBindingSource((Key[])(object)new Key[1] { (Key)2 });

	private PlayerAction _currentAction;

	private BindingSource _oldBinding;

	private void Awake()
	{
		BindingListenOptions listenOptions = ((PlayerActionSet)KeyMapper.Map).ListenOptions;
		listenOptions.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Combine(listenOptions.OnBindingAdded, (Action<PlayerAction, BindingSource>)delegate(PlayerAction action, BindingSource addedBinding)
		{
			if (addedBinding == (BindingSource)(object)leftShiftBinding || addedBinding == (BindingSource)(object)rightShiftBinding)
			{
				action.ReplaceBinding(addedBinding, (BindingSource)(object)shiftBinding);
			}
			if (addedBinding == (BindingSource)(object)leftControlBinding || addedBinding == (BindingSource)(object)rightControlBinding)
			{
				action.ReplaceBinding(addedBinding, (BindingSource)(object)controlBinding);
			}
			if (addedBinding == (BindingSource)(object)leftAltBinding || addedBinding == (BindingSource)(object)rightAltBinding)
			{
				action.ReplaceBinding(addedBinding, (BindingSource)(object)altBinding);
			}
		});
		BindingListenOptions listenOptions2 = ((PlayerActionSet)KeyMapper.Map).ListenOptions;
		listenOptions2.OnBindingFound = (Func<PlayerAction, BindingSource, bool>)Delegate.Combine(listenOptions2.OnBindingFound, (Func<PlayerAction, BindingSource, bool>)delegate(PlayerAction action, BindingSource foundBinding)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Invalid comparison between Unknown and I4
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Invalid comparison between Unknown and I4
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Invalid comparison between Unknown and I4
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Invalid comparison between Unknown and I4
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Invalid comparison between Unknown and I4
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Invalid comparison between Unknown and I4
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Invalid comparison between Unknown and I4
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Invalid comparison between Unknown and I4
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Invalid comparison between Unknown and I4
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Invalid comparison between Unknown and I4
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Invalid comparison between Unknown and I4
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Invalid comparison between Unknown and I4
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Invalid comparison between Unknown and I4
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Invalid comparison between Unknown and I4
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Invalid comparison between Unknown and I4
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Invalid comparison between Unknown and I4
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Invalid comparison between Unknown and I4
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Invalid comparison between Unknown and I4
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Invalid comparison between Unknown and I4
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Invalid comparison between Unknown and I4
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Invalid comparison between Unknown and I4
			if (KeyMap.SimplifyBindingSourceType(_oldBinding.BindingSourceType) != KeyMap.SimplifyBindingSourceType(foundBinding.BindingSourceType))
			{
				return false;
			}
			DeviceBindingSource val = (DeviceBindingSource)(object)((foundBinding is DeviceBindingSource) ? foundBinding : null);
			if (val != null)
			{
				if ((int)val.Control == 1 || (int)val.Control == 2 || (int)val.Control == 3 || (int)val.Control == 4 || (int)val.Control == 5 || (int)val.Control == 11 || (int)val.Control == 12 || (int)val.Control == 13 || (int)val.Control == 14)
				{
					return false;
				}
				if ((int)val.Control == 106 || (int)val.Control == 104 || (int)val.Control == 101 || (int)val.Control == 113 || (int)val.Control == 308 || (int)val.Control == 102 || (int)val.Control == 100 || (int)val.Control == 109 || (int)val.Control == 107 || (int)val.Control == 105 || (int)val.Control == 114 || (int)val.Control == 307 || (int)val.Control == 111 || (int)val.Control == 300)
				{
					action.StopListeningForBinding();
					((Component)this).gameObject.SetActive(false);
					return false;
				}
			}
			if (foundBinding == (BindingSource)(object)enterBinding)
			{
				return false;
			}
			if (foundBinding == (BindingSource)(object)escapeBinding)
			{
				action.StopListeningForBinding();
				((Component)this).gameObject.SetActive(false);
				return false;
			}
			if (foundBinding == (BindingSource)(object)leftShiftBinding || foundBinding == (BindingSource)(object)rightShiftBinding)
			{
				foundBinding = (BindingSource)(object)shiftBinding;
			}
			if (foundBinding == (BindingSource)(object)leftControlBinding || foundBinding == (BindingSource)(object)rightControlBinding)
			{
				foundBinding = (BindingSource)(object)controlBinding;
			}
			if (foundBinding == (BindingSource)(object)leftAltBinding || foundBinding == (BindingSource)(object)rightAltBinding)
			{
				foundBinding = (BindingSource)(object)altBinding;
			}
			if (!CommonResource.instance.TryGetKeyIcon(foundBinding, out var _))
			{
				return false;
			}
			if (KeyMapper.Map.gameActions.Contains(action))
			{
				for (int i = 0; i < KeyMapper.Map.gameActions.Count; i++)
				{
					PlayerAction val2 = KeyMapper.Map.gameActions[i];
					if (action != val2)
					{
						foreach (BindingSource binding in val2.Bindings)
						{
							if (binding == foundBinding)
							{
								BindingSource val3 = null;
								BindingSource oldBinding = _oldBinding;
								KeyBindingSource val4 = (KeyBindingSource)(object)((oldBinding is KeyBindingSource) ? oldBinding : null);
								if (val4 == null)
								{
									MouseBindingSource val5 = (MouseBindingSource)(object)((oldBinding is MouseBindingSource) ? oldBinding : null);
									if (val5 == null)
									{
										DeviceBindingSource val6 = (DeviceBindingSource)(object)((oldBinding is DeviceBindingSource) ? oldBinding : null);
										if (val6 != null)
										{
											val3 = (BindingSource)new DeviceBindingSource(val6.Control);
										}
									}
									else
									{
										val3 = (BindingSource)new MouseBindingSource(val5.Control);
									}
								}
								else
								{
									val3 = (BindingSource)new KeyBindingSource(val4.Control);
								}
								if (val3 != (BindingSource)null)
								{
									val2.ReplaceBinding(binding, val3);
								}
								break;
							}
						}
					}
				}
			}
			((Component)this).gameObject.SetActive(false);
			return true;
		});
	}

	public void ListenForBinding(PlayerAction action, BindingSource binding)
	{
		((Component)this).gameObject.SetActive(true);
		_currentAction = action;
		_oldBinding = binding;
		KeyMapper.Map.SetListenOptions();
		_currentAction.ListenForBindingReplacing(_oldBinding);
	}

	private void Update()
	{
		if (!_currentAction.IsListeningForBinding)
		{
			((Component)this).gameObject.SetActive(false);
		}
	}
}
