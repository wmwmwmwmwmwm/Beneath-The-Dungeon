using UnityEngine;

public class Special10Guard : MonoBehaviour, ISpecialStatusEventReceiveDamage
{
	public void ReceiveDamageEffect(IntRef Damage)
	{
		Damage.Value = (int)(Damage.Value * 0.2f);
	}
}
