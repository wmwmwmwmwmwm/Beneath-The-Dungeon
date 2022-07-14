using System.Collections;
using UnityEngine;

public class Special16IceShield : MonoBehaviour, ISpecialStatusEventReceiveDamage, ISpecialStatusFormatDescription
{
	int ShieldLeft = 100;

	public void ReceiveDamageEffect(IntRef Damage)
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		int DamageAbsorbed = Mathf.Min(Damage.Value, ShieldLeft);
		ShieldLeft -= DamageAbsorbed;
		Damage.Value -= DamageAbsorbed;
		if (ShieldLeft == 0) DungeonController.Instance.RemoveSpecialStatus(DataComponent.UserBattleStatus, DataComponent);
	}

	public string GetDescription()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		return string.Format(DataComponent.KoreanDescription, ShieldLeft);
	}
}
