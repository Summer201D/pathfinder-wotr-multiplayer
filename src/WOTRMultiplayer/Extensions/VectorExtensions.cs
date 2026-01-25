using UnityEngine;
using WOTRMultiplayer.Entities;

namespace WOTRMultiplayer.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ToUnityVector3(this NetworkVector3 networkVector)
        {
            if (networkVector == null)
            {
                return default;
            }

            var vector = new Vector3(networkVector.X, networkVector.Y, networkVector.Z);
            return vector;
        }

        public static NetworkVector3 ToNetworkVector3(this Vector3 vector)
        {
            var networkVector = new NetworkVector3(vector.x, vector.y, vector.z);
            return networkVector;
        }
    }
}
