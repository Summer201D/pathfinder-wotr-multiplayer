using System.Reflection;
using BeetleX;
using BeetleX.Buffers;
using WOTRMultiplayer.Networking.Abstractions;

namespace WOTRMultiplayer.Networking
{
    public class TcpClientFactory : ITcpClientFactory
    {
        public ITcpClient Create(string host, int port)
        {
            var defaultGroup = typeof(BufferPoolGroup).GetField("mDefaultGroup", BindingFlags.NonPublic | BindingFlags.Static);

            var bufferSize = 1024 * 64;
            var numberOfGroups = 12;
            var count = 1024 * 8 / numberOfGroups;
            var maxCount = 20480 * 8 / numberOfGroups;
            var defaultGroupValue = new BufferPoolGroup(bufferSize, count, maxCount, numberOfGroups);
            defaultGroup.SetValue(null, defaultGroupValue);

            var client = SocketFactory.CreateClient<BeetleTcpClient>(new Messages.ProtobufClientPacket(), host, port);
            return client;
        }
    }
}
