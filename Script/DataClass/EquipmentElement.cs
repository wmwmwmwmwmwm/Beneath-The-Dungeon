using UnityEngine;

public class EquipmentElement : MonoBehaviour, ISpecialStatusEventAttackDamage
{
	public enum EquipmentElementTypeEnum { HP, MP, SP, Armor, STR, DEX, INT, CON, Weapon, Elemental, Special }
	public EquipmentElementTypeEnum EquipmentElementType;
	public RarityTypeEnum Rarity;
	public int ID;
	public int Tier;
	public int Point;
	public bool CanDuplicated;
	public int HP, MP, SP, Armor;
	public int STR, DEX, INT, CON;
	public int Damage;
	public ElementalTypeEnum ElementalType;
	public string SpecialEffectDescription;
	public SpecialStatusData SpecialEffectPrefab, SpecialEffectObject;

	public void AttackDamageEffect(IntRef AttackDamage, ElementalTypeEnum ElementalType)
	{
		AttackDamage.Value += Damage;
	}

}
