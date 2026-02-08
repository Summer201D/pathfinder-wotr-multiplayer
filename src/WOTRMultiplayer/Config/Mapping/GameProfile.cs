using AutoMapper;
using Kingmaker.UnitLogic.Abilities;
using WOTRMultiplayer.Entities.Combat;

namespace WOTRMultiplayer.Config.Mapping
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<AbilityData, NetworkAbility>().ConstructUsing(a => CreateNetworkAbility(a));
        }

        private NetworkAbility CreateNetworkAbility(AbilityData abilityData)
        {
            if (abilityData == null)
            {
                return null;
            }
            abilityData.SpellLevel
            var ability = new NetworkAbility
            {
                Id = abilityData.UniqueId,
                Name = abilityData.NameForAcronym
                SpellbookId = abilityData.Spellbook?.Blueprint.Name.Key,
                ConvertedFromId = abilityData.ConvertedFrom?.UniqueId,
            };
            return ability;
        }
    }
}
