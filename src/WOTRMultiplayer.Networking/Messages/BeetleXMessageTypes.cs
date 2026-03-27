using System;
using BeetleX;
using BeetleX.Buffers;
using BeetleX.Clients;

namespace WOTRMultiplayer.Networking.Messages
{
    public static class BeetleXMessageTypes
    {
        public static BeetleX.Packets.CustomTypeHeader MessageTypes { get; set; } = new BeetleX.Packets.CustomTypeHeader(BeetleX.Packets.MessageIDType.INT);

        public class ProtobufServerPacket : BeetleX.Packets.FixedHeaderPacket
        {
            public override IPacket Clone()
            {
                return new ProtobufServerPacket();
            }

            protected override object OnRead(ISession session, PipeStream stream)
            {
                Type type = MessageTypes.ReadType(stream);
                var size = CurrentSize - sizeof(int);
                return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(stream, null, type, size);
            }

            protected override void OnWrite(ISession session, object data, PipeStream stream)
            {
                MessageTypes.WriteType(data, stream);
                ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(stream, data);
            }
        }

        public class ProtobufClientPacket : BeetleX.Packets.FixeHeaderClientPacket
        {
            public override IClientPacket Clone()
            {
                return new ProtobufClientPacket();
            }

            protected override object OnRead(IClient client, PipeStream stream)
            {
                Type type = MessageTypes.ReadType(stream);
                var size = CurrentSize - sizeof(int);
                return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(stream, null, type, size);
            }

            protected override void OnWrite(object data, IClient client, PipeStream stream)
            {
                MessageTypes.WriteType(data, stream);
                ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(stream, data);
            }
        }
    }
}
