using UnityEngine;

public static class EquipmentHelper
{
	public static bool IsEqualEquipmentType(EquipmentSlotTypeEnum SlotType1, EquipmentSlotTypeEnum SlotType2)
	{
		return SlotType1 switch
		{
			EquipmentSlotTypeEnum.Head => SlotType2 == EquipmentSlotTypeEnum.Head,
			EquipmentSlotTypeEnum.Neck => SlotType2 == EquipmentSlotTypeEnum.Neck,
			EquipmentSlotTypeEnum.Body => SlotType2 == EquipmentSlotTypeEnum.Body,
			EquipmentSlotTypeEnum.Weapon => SlotType2 == EquipmentSlotTypeEnum.Weapon,
			EquipmentSlotTypeEnum.Glove => SlotType2 == EquipmentSlotTypeEnum.Glove,
			EquipmentSlotTypeEnum.Ring1 => SlotType2 == EquipmentSlotTypeEnum.Ring1 || SlotType2 == EquipmentSlotTypeEnum.Ring2,
			EquipmentSlotTypeEnum.Ring2 => SlotType2 == EquipmentSlotTypeEnum.Ring1 || SlotType2 == EquipmentSlotTypeEnum.Ring2,
			EquipmentSlotTypeEnum.Boots => SlotType2 == EquipmentSlotTypeEnum.Boots,
			_ => false,
		};
	}

	public static EquipmentSlotTypeEnum EquipmentTypeToSlotType(EquipmentTypeEnum EquipmentType)
	{
		return EquipmentType switch
		{
			EquipmentTypeEnum.Head => EquipmentSlotTypeEnum.Head,
			EquipmentTypeEnum.Neck => EquipmentSlotTypeEnum.Neck,
			EquipmentTypeEnum.Body => EquipmentSlotTypeEnum.Body,
			EquipmentTypeEnum.Weapon => EquipmentSlotTypeEnum.Weapon,
			EquipmentTypeEnum.Glove => EquipmentSlotTypeEnum.Glove,
			EquipmentTypeEnum.Ring => EquipmentSlotTypeEnum.Ring1,
			EquipmentTypeEnum.Boots => EquipmentSlotTypeEnum.Boots,
			_ => 0,
		};
	}
}
