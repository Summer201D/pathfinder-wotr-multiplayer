using System;
using System.Linq;
using HarmonyLib;
using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook.Metamagic;
using Microsoft.Extensions.Logging;
using UniRx;
using WOTRMultiplayer.Entities.Combat;
using WOTRMultiplayer.Entities.SpellbookManagement;

namespace WOTRMultiplayer.HarmonyPatches.SpellbookManagement
{
    [HarmonyPatch]
    public class MetamagicPatches
    {
        [HarmonyPatch(typeof(SpellbookMetamagicMixerVM), nameof(SpellbookMetamagicMixerVM.TryWriteNewSpell))]
        [HarmonyPrefix]
        public static void SpellbookMetamagicMixerVM_TryWriteNewSpell_Prefix(SpellbookMetamagicMixerVM __instance)
        {
            if (!Main.Multiplayer.IsActive || !__instance.CanWriteNewSpell())
            {
                return;
            }

            var metamagicSpell = CreateMetamagicSpell(__instance);
            Main.Multiplayer.OnSpellbookMetamagicSpellCreated(metamagicSpell);
        }

        private static NetworkMetamagicSpell CreateMetamagicSpell(SpellbookMetamagicMixerVM mixerVM)
        {
            try
            {
                var currentSpell = mixerVM.m_CurrentSpell.Value?.SpellData;
                if (currentSpell == null)
                {
                    return null;
                }

                var metamagicSpell = new NetworkMetamagicSpell
                {
                    Ability = Main.Mapper.Map<NetworkAbility>(currentSpell),
                    MetamagicFeatures = mixerVM.m_MetamagicBuilder.Value.AppliedMetamagics.Cast<int>().ToList() ?? [],
                    BorderNumber = mixerVM.SpellbookDecorator.SpellbookDecoratorBorder.BorderSelector.SelectedEntity.Value?.Index,
                    DecorationColorNumber = mixerVM.SpellbookDecorator.SpellbookDecoratorColor.m_CurrentColorVM.Value?.Index,
                    UnitId = mixerVM.m_Unit.Value.Unit.UniqueId
                };

                return metamagicSpell;
            }
            catch (Exception ex)
            {
                Main.GetLogger<MetamagicPatches>().LogError(ex, "Error creating metamagic spell");
                throw;
            }
        }
    }
}
