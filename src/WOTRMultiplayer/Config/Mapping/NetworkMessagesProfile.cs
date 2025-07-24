using AutoMapper;
using WOTRMultiplayer.MP.Entities;
using WOTRMultiplayer.MP.Entities.Rolls;

namespace WOTRMultiplayer.Config.Mapping
{
    public class NetworkMessagesProfile : Profile
    {
        public NetworkMessagesProfile()
        {
            CreateMap<NetworkVector3, Networking.Messages.NetworkVector3>()
                .ReverseMap();

            CreateMap<NetworkClick, Networking.Messages.NetworkClick>()
                .ReverseMap();

            CreateMap<NetworkAbilityUse, Networking.Messages.NetworkAbilityUse>()
                .ReverseMap();

            CreateMap<NetworkActionsState, Networking.Messages.NetworkActionsState>()
                .ReverseMap();

            CreateMap<NetworkCombatAction, Networking.Messages.NetworkCombatAction>()
                .ReverseMap();

            CreateMap<NetworkDiceRoll, Networking.Messages.NetworkDiceRoll>()
               .ReverseMap();

            CreateMap<NetworkDamageValueRoll, Networking.Messages.NetworkDamageValueRoll>()
                .ReverseMap();

            CreateMap<NetworkUnit, Networking.Messages.NetworkUnit>()
                .ReverseMap();
        }
    }
}
