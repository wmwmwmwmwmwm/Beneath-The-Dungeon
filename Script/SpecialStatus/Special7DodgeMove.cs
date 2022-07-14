using System.Collections;
using UnityEngine;

public class Special7DodgeMove : MonoBehaviour, ISpecialStatusEventAttackDamage, ISpecialStatusEventReceiveDamage
{
	public void AttackDamageEffect(IntRef Damage, ElementalTypeEnum ElementalType)
	{
		Damage.Value = (int)(Damage.Value * 0.75f);
	}

	public void ReceiveDamageEffect(IntRef Damage)
	{
		Damage.Value = (int)(Damage.Value * 0.5f);
	}
}
