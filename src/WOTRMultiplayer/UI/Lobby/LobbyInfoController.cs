using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.UI.MVVM._VM.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WOTRMultiplayer.Extensions;
using WOTRMultiplayer.Unity;

namespace WOTRMultiplayer.UI.Lobby
{
    public class LobbyInfoController
    {
        public const string LobbyScreenRootObjectName = "LobbyScreen";
        public const string LobbyContentObjectName = "LobbyContent";

        public const string PlayersSectionObjectName = "PlayersSection";
        public const string PlayersSectionTitleObjectName = "PlayersSectionTitle";
        public const string PlayersSectionContentObjectName = "PlayersSectionContent";

        public const string PlayerContainerObjectName = "PlayerContainer";
        public const string PlayerNameObjectName = "PlayerName";
        public const string PlayerStatusObjectName = "PlayerStatus";

        public const string CharactersSectionObjectName = "CharactersSection";
        public const string CharactersSectionTitleObjectName = "CharactersSectionTitle";
        public const string CharactersSectionContentObjectName = "CharactersSectionContent";

        public const string CharacterContainerObjectName = "CharacterContainer";
        public const string CharacterPortraitObjectName = "CharacterPortrait";
        public const string CharacterOwnerObjectName = "CharacterOwner";

        private readonly GameObject _content;
        private GameObject PlayersSectionContent => _content.transform
            .Find(LobbyContentObjectName)
            .Find(PlayersSectionObjectName)
            .Find(PlayersSectionContentObjectName).gameObject;

        private GameObject CharactersInfoContainer => _content.transform
            .Find(LobbyContentObjectName)
            .Find(CharactersSectionObjectName)
            .Find(CharactersSectionContentObjectName).gameObject;
        public LobbyInfoController(GameObject content)
        {
            _content = content;
        }

        public void SaveSlotSelected(SaveSlotVM value)
        {
            Logging.Logger.Info($"Selected SaveSlo={value.SaveName.Value}");
            var players = Enumerable.Range(0, 1).Select(c => Guid.NewGuid().ToString().Split('-').First()).ToList();
            UpdatePlayers(players);
            UpdateCharacters(value);
        }

        public void UpdatePlayers(List<string> players)
        {
            PlayersSectionContent.CleanupAllChildren();
            var defaultMesh = Main.Multiplayer.Factory.GetDefaultMesh();
            var rnd = new System.Random();
            foreach (var playerName in players)
            {
                var playerContainerObject = Main.Multiplayer.Factory.CreateDefaultGameObject(PlayersSectionContent.transform);
                playerContainerObject.name = LobbyInfoController.PlayerContainerObjectName;
                var playerContainerHorizontal = playerContainerObject.AddComponent<HorizontalLayoutGroup>();
                playerContainerHorizontal.childAlignment = TextAnchor.LowerRight;

                var playerStatusObject = Main.Multiplayer.Factory.CreateDefaultGameObject(playerContainerObject.transform);
                playerStatusObject.name = LobbyInfoController.PlayerStatusObjectName;
                var playerStatus = playerStatusObject.AddComponent<TextMeshProUGUI>();
                playerStatus.alignment = TextAlignmentOptions.Right;
                playerStatus.material = defaultMesh.Material;
                var isOK = rnd.Next(0, 2) == 0;
                if (isOK)
                {
                    playerStatus.color = Color.green;
                    playerStatus.SetText("+");
                }
                else
                {
                    playerStatus.color = Color.red;
                    playerStatus.SetText("-");
                }

                var player = Main.Multiplayer.Factory.CreateDefaultGameObject(playerContainerObject.transform);
                player.name = LobbyInfoController.PlayerNameObjectName;
                var playerNameBox = player.AddComponent<TextMeshProUGUI>();
                playerNameBox.alignment = TextAlignmentOptions.Left;
                playerNameBox.material = defaultMesh.Material;
                playerNameBox.color = defaultMesh.Color;
                playerNameBox.SetText(playerName);

            }
        }

        private void UpdateCharacters(SaveSlotVM saveSlotVM)
        {
            for (int characterIndex = 0; characterIndex < UIFactory.GetMaxCharactersCount(); characterIndex++)
            {
                var sprite = GetPortraitSprite(characterIndex, saveSlotVM);
                var specificCharacterContainer = CharactersInfoContainer.transform.GetChild(characterIndex);
                var portrait = specificCharacterContainer.Find(CharacterPortraitObjectName);
                var dropdown = specificCharacterContainer.Find(CharacterOwnerObjectName);
                var img = portrait.GetComponent<Image>();
                img.sprite = sprite;
                img.color = sprite == null ? Color.clear : Color.white;
                //// TBD test stuff
                //var dropdownObject = dropdown.transform.Find(UIFactory.DropdownGameObjectName);
                //var tmpDropdown = dropdownObject.GetComponent<TMP_Dropdown>();
                //tmpDropdown.onValueChanged.RemoveAllListeners();
                //tmpDropdown.ClearOptions();
                //tmpDropdown.AddOptions(players);
                //tmpDropdown.onValueChanged.AddListener(index => OnCharacterOwnerChanged(tmpDropdown));
            }
        }

        private void OnCharacterOwnerChanged(TMP_Dropdown dropdown)
        {
            var player = dropdown.options.Count >= dropdown.value ? dropdown.options[dropdown.value].text : null;
            if (player == null)
            {
                Logging.Logger.Warning("Can't find selected player to assign character control");
                return;
            }

            var characterIndexComponent = dropdown.transform.parent?.GetComponent<CharacterIndexMonoBehavior>();

            if (characterIndexComponent == null)
            {
                Logging.Logger.Warning($"Can't find ${nameof(CharacterIndexMonoBehavior)} to assign character control");
                return;
            }

            Logging.Logger.Info($"Character owner changed. CharacterIndex={characterIndexComponent.CharacterIndex}, Player={player}");
        }

        private Sprite GetPortraitSprite(int slot, SaveSlotVM saveSlotVM)
        {
            return saveSlotVM.PartyPortraits.Value.Count > slot ? saveSlotVM.PartyPortraits.Value[slot].Portrait : null;
        }
    }
}
