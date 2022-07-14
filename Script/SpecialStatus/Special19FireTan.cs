using System.Collections;
using UnityEngine;

public class Special19FireTan : MonoBehaviour, ISpecialStatusEventAttackDamage
{
	public void AttackDamageEffect(IntRef Damage, ElementalTypeEnum ElementalType)
	{
		if (ElementalType == ElementalTypeEnum.Fire) Damage.Value += (int)(Damage.Value * 0.3f);
	}
}
