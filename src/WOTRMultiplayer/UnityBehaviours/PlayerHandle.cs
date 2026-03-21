using UnityEngine;
using WOTRMultiplayer.Entities;

namespace WOTRMultiplayer.UnityBehaviours
{
    public class PlayerHandle : MonoBehaviour
    {
        public NetworkPlayer Owner { get; set; }
    }
}
