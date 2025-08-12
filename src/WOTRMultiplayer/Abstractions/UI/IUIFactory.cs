using System;
using Kingmaker.UI.MVVM._PCView.SaveLoad;
using Kingmaker.UI.MVVM._PCView.Settings.Entities;
using TMPro;
using UnityEngine;
using WOTRMultiplayer.Abstractions.UI.Windows;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.Abstractions.UI
{
    public interface IUIFactory
    {
        IMultiplayerWindow InitializeMultiplayerWindow(InitializeMultiplayerContext context, Action onShow);

        GameObject CreateDefaultGameObject(Transform parent);
        GameObject CreateDropdown(float preferedWidth, Transform parent);
        GameObject CreateButton(Transform transform);
        GameObject CreateInput(Transform transform);
        GameObject CreateLobbyWindowContent(Transform parent);
        SaveLoadPCView CreateSaveLoadPCView(Transform parent);
        ILobbyWindow InitializeEscMenuLobbyWindow(InitializeEscMenuLobbyWindowContext context, Action onShow);
        GameObject CreateBackgroundArt(Transform parent);
        WOTRMultiplayer.UI.Mesh GetDefaultMesh();
        void StoreBorderDecoration(GameObject gameObject);
        void StoreDefaultGameObject(GameObject gameObject);
        void StoreDefaultTextMesh(TextMeshProUGUI defaultTextMesh);
        void StoreDropdownPrefab(SettingsEntityDropdownPCView view);
        void StoreInputPrefab(GameObject inputObject);
        void StoreButtonPrefab(GameObject buttonObject);

        void StoreSaveLoadPCViewPrefab(SaveLoadPCView view);
        void StoreBackgroundArt(GameObject gameObject);

        void DestroyLobbyWindow(ILobbyWindow lobbyWindow);
    }
}
