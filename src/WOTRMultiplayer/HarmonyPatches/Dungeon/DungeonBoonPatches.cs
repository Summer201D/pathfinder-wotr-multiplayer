using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Dungeon.ChooseBoon;
using Owlcat.Runtime.UI.Controls.Other;
using UniRx;
using WOTRMultiplayer.Entities.Dungeon;

namespace WOTRMultiplayer.HarmonyPatches.Dungeon
{
    [HarmonyPatch]
    public class DungeonBoonPatches
    {
        [HarmonyPatch(typeof(DungeonChooseBoonPCView), nameof(DungeonChooseBoonPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void DungeonChooseBoonPCView_BindViewImplementation_Postfix(DungeonChooseBoonPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.AddDisposable(__instance.ViewModel.BoonSelector.SelectedEntity.Subscribe(boon =>
            {
                var networkBoon = new NetworkBoon
                {
                    Index = __instance.ViewModel.BoonSelector.EntitiesCollection.IndexOf(boon),
                    Id = boon.Boon.AssetGuid.ToString(),
                    Name = boon.Boon.name
                };

                Main.Multiplayer.OnDungeonBoonSelected(networkBoon);
            }));


            __instance.AddDisposable(__instance.m_ConfirmButton.OnLeftClickAsObservable().Subscribe(_ =>
            {
                Main.Multiplayer.OnDungeonBoonConfirmed();
            }));

            Main.Multiplayer.OnDungeonBoonSelectorShown();
        }
    }
}
