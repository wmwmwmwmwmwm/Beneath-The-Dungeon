using System.Collections;
using UnityEngine;

public class Special6Bleeding : MonoBehaviour, ISpecialStatusEventReceiveDamage
{
	public void ReceiveDamageEffect(IntRef Damage)
	{
		Damage.Value = (int)(Damage.Value * 1.3f);
	}
}
