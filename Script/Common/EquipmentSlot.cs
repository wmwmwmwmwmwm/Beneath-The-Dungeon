using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
	public EquipmentSlotTypeEnum EquipmentSlotType;
	public GameObject IconObject;
	public ItemIcon IconComponent;
	public EquipmentData ThisEquipmentData { get; private set; }

	public void SetEquipmentData(EquipmentData EquipmentDataToSet)
	{
		ThisEquipmentData = EquipmentDataToSet;
		if (ThisEquipmentData)
		{
			IconObject.SetActive(true);
			IconComponent.UpdateRarityColor(ThisEquipmentData.Rarity);
			IconComponent.Icon.sprite = ThisEquipmentData.IconData.IconSprite;
		}
		else
		{
			IconObject.SetActive(false);
		}
	}
}
