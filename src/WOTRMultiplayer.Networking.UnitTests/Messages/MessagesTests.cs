using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            var duplicateIds = allMessages.GroupBy(x => x.MessageType.Id).Where(x => x.Count() > 1).ToList();

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
            var restrictedIdsCount = allMessages.Count(x => x.MessageType.Id == delimiter);

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
                .Select(x => x.MessageType.Id)
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
            var invalidProtoContracts = allProtoContracts.Where(x =>
                x.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(x => x.GetCustomAttribute<ProtoMemberAttribute>() == null)).ToList();

            // Assert
            Assert.That(invalidProtoContracts.Count, Is.EqualTo(0), "Missing ProtoMember: " + string.Join(", ", invalidProtoContracts.Select(x => x.Name)));
            Assert.That(allProtoContracts.Count, Is.GreaterThan(0));
        }

        [Test]
        public void NetworkMessages_HaveNoDuplicateProtoMemberIndexes()
        {
            // Arrange
            var allProtoContracts = Assembly
                .GetAssembly(typeof(BeetleXMessageTypes.ProtobufServerPacket))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<ProtoContractAttribute>() != null)
                .ToList();
            var invalidContracts = new List<Type>();

            // Act
            foreach (var message in allProtoContracts)
            {
                var properties = message.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<ProtoMemberAttribute>() != null).ToList();
                var allIndexes = properties.Select(x => x.GetCustomAttribute<ProtoMemberAttribute>().Tag).Distinct().ToList();
                if (allIndexes.Count != properties.Count)
                {
                    invalidContracts.Add(message);
                }
            }

            // Assert
            Assert.That(invalidContracts.Count, Is.EqualTo(0), "Corrupted ProtoContract: " + string.Join(", ", invalidContracts.Select(x => x.Name)));
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

        [Test]
        public void NetworkMessages_EveryMessageIsUsed()
        {
            // Arrange
            var rootLocation = Path.GetFullPath(@"..\..\..\..\WOTRMultiplayer\Services");

            var trees = new List<(CSharpCompilation, SemanticModel, ClassDeclarationSyntax)>
            {
               Create(Path.Combine(rootLocation, "MultiplayerClient.cs")),
               Create(Path.Combine(rootLocation, "MultiplayerHost.cs")),
               Create(Path.Combine(rootLocation, "MultiplayerActorBase.cs"))
            };

            var allMessages = Assembly
                .GetAssembly(typeof(BeetleXMessageTypes.ProtobufServerPacket))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<MessageTypeAttribute>() != null)
                .ToList();
            var notCreatedMessages = new List<Type>();

            // Act
            foreach (var message in allMessages)
            {
                if (!trees.Any(t =>
                {
                    var targetSymbol = t.Item1.GetTypeByMetadataName(message.FullName);
                    if (targetSymbol == null)
                    {
                        Assert.Fail($"Unable to find message symbol. Type={message.Name}");
                    }

                    var methods = t.Item3.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
                    foreach (var method in methods)
                    {
                        var createsMessage = method.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().Any(o =>
                        {
                            var type = t.Item2.GetTypeInfo(o).Type;
                            return SymbolEqualityComparer.Default.Equals(type, targetSymbol);
                        });

                        if (createsMessage)
                        {
                            return true;
                        }
                    }

                    return false;
                }))
                {
                    notCreatedMessages.Add(message);
                }
            }

            // Assert
            Assert.That(notCreatedMessages, Is.Empty, $"Some messages are never created ({notCreatedMessages.Count})");
        }

        private (CSharpCompilation, SemanticModel, ClassDeclarationSyntax) Create(string path)
        {
            var code = File.ReadAllText(path);
            var tree = CSharpSyntaxTree.ParseText(code);

            var classDeclaration = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();

            var compilation = CSharpCompilation.Create("whatever", [tree], [
                MetadataReference.CreateFromFile(typeof(WOTRMultiplayer.Main).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Networking.Messages.BeetleXMessageTypes.ProtobufServerPacket).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location)
                ]);
            var semanticModel = compilation.GetSemanticModel(tree);
            return (compilation, semanticModel, classDeclaration);
        }
    }
}
