using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ProtoBuf;
using WOTRMultiplayer.Networking.Messages;

namespace WOTRMultiplayer.Networking.UnitTests.Messages
{
    [TestFixture]
    public class MessagesTests
    {
        [Test]
        public void NetworkMessages_HaveNoDuplicateMessageTypeIds()
        {
            // Arrange
            var allMessages = Assembly
                .GetAssembly(typeof(BeetleXMessageTypes.ProtobufServerPacket))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<MessageTypeAttribute>() != null)
                .Select(t => new { Type = t, MessageType = t.GetCustomAttribute<MessageTypeAttribute>() })
                .ToList();

            // Act
            var duplicateIds = allMessages.GroupBy(x => (int)x.MessageType.Id).Where(x => x.Count() > 1).ToList();

            // Assert
            Assert.That(duplicateIds.Count, Is.EqualTo(0), "Duplicate Ids: " + string.Join(", ", duplicateIds.Select(x => $"{x.Key} ({string.Join(",", x.Select(a => a.Type.Name).ToList())})")));
            Assert.That(allMessages.Count, Is.GreaterThan(0));
        }

        [TestCase((int)MessageTypes.Lobby.None)]
        [TestCase((int)MessageTypes.Game.None)]
        [TestCase((int)MessageTypes.Request.None)]
        public void NetworkMessages_ShouldNotUseDelimiterValueAsMessageTypeId(int delimiter)
        {
            // Arrange
            var allMessages = Assembly
                .GetAssembly(typeof(BeetleXMessageTypes.ProtobufServerPacket))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<MessageTypeAttribute>() != null)
                .Select(t => new { Type = t, MessageType = t.GetCustomAttribute<MessageTypeAttribute>() })
                .ToList();

            // Act
            var restrictedIdsCount = allMessages.Count(x => (int)x.MessageType.Id == delimiter);

            // Assert
            Assert.That(restrictedIdsCount, Is.EqualTo(0));
            Assert.That(allMessages.Count, Is.GreaterThan(0));
        }

        [TestCase(typeof(MessageTypes.Lobby))]
        [TestCase(typeof(MessageTypes.Game))]
        [TestCase(typeof(MessageTypes.Request))]
        public void NetworkMessages_EveryEnumValueIsInUse(Type type)
        {
            // Arrange
            var allMessageIds = Assembly
                .GetAssembly(typeof(BeetleXMessageTypes.ProtobufServerPacket))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<MessageTypeAttribute>() != null)
                .Select(t => new { Type = t, MessageType = t.GetCustomAttribute<MessageTypeAttribute>() })
                .Select(x => (int)x.MessageType.Id)
                .ToList();

            // element 1 is None
            var allEnumValues = Enum.GetValues(type).Cast<int>().Skip(1).ToList();

            // Act
            var notUsedValues = allEnumValues.Except(allMessageIds);

            // Assert
            Assert.That(notUsedValues.Count, Is.EqualTo(0), "Not used Enum Values: " + string.Join(", ", notUsedValues.Select(x => Enum.GetName(type, x))));
            Assert.That(allMessageIds.Count, Is.GreaterThan(0));
        }

        [Test]
        public void NetworkMessages_EachPublicPropertyIsMarkedWithProtoMember()
        {
            // Arrange
            var allProtoContracts = Assembly
                .GetAssembly(typeof(BeetleXMessageTypes.ProtobufServerPacket))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<ProtoContractAttribute>() != null)
                .ToList();

            // Act
            var invalidProtoContracts = allProtoContracts.Where(x => x.GetProperties(BindingFlags.Public | BindingFlags.Instance).Any(x => x.GetCustomAttribute<ProtoMemberAttribute>() == null)).ToList();

            // Assert
            Assert.That(invalidProtoContracts.Count, Is.EqualTo(0), "Missing ProtoMember: " + string.Join(", ", invalidProtoContracts.Select(x => x.Name)));
            Assert.That(allProtoContracts.Count, Is.GreaterThan(0));
        }

        [Test]
        public void NetworkMessages_EachMessageIsMarkedWithProtoContract()
        {
            // Arrange
            var allMessages = Assembly
                .GetAssembly(typeof(BeetleXMessageTypes.ProtobufServerPacket))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<MessageTypeAttribute>() != null)
                .ToList();

            // Act
            var invalidMessages = allMessages.Where(x => x.GetCustomAttribute<ProtoContractAttribute>() == null).ToList();

            // Assert
            Assert.That(invalidMessages.Count, Is.EqualTo(0), "Missing ProtoContractAttribute: " + string.Join(", ", invalidMessages.Select(x => x.Name)));
            Assert.That(allMessages.Count, Is.GreaterThan(0));
        }
    }
}
