using System;
using WOTRMultiplayer.MP.Entities.Equipment;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IEquipmentDefinitions
    {
        Type GetSlotType(NetworkEquipmentSlotType slotType);

        NetworkEquipmentSlotType? GetSlotType(Type type);
    }
}
