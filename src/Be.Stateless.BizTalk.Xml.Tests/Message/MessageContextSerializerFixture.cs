#region Copyright & License

// Copyright © 2012 - 2022 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using Be.Stateless.BizTalk.ContextProperties;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Message
{
	public class MessageContextSerializerFixture
	{
		[Fact]
		public void SerializeDiscardsAuthorizationHttpHeader()
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(WcfProperties.HttpHeaders.Name, WcfProperties.HttpHeaders.Namespace, "Authorization: Bearer b3@r3r", false)
					})
				.Should().Be(
					@$"<context xmlns:s0=""{WcfProperties.HttpHeaders.Namespace}"">"
					+ @"<s0:p n=""HttpHeaders""></s0:p>"
					+ "</context>"
				);
		}

		[Theory]
		[InlineData("Authorization: Bearer b3@r3r\r\nContent-Type: application/xml\r\nAccept: application/xml")]
		[InlineData("Content-Type: application/xml\nAccept: application/xml\nAuthorization: Bearer b3@r3r")]
		[InlineData("Content-Type: application/xml\nAuthorization: Bearer b3@r3r\nAccept: application/xml")]
		[InlineData("Content-Type: application/xml\rAuthorization: Bearer b3@r3r\rAccept: application/xml")]
		[InlineData("Content-Type: application/xml\r\nAuthorization: Bearer b3@r3r\r\nAccept: application/xml")]
		public void SerializeDiscardsAuthorizationHttpHeaderAmongOtherHttpHeaders(string httpHeaders)
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(WcfProperties.HttpHeaders.Name, WcfProperties.HttpHeaders.Namespace, httpHeaders, false)
					})
				.Should().Be(
					@$"<context xmlns:s0=""{WcfProperties.HttpHeaders.Namespace}"">"
					+ @"<s0:p n=""HttpHeaders"">" + "Content-Type: application/xml\r\nAccept: application/xml" + "</s0:p>"
					+ "</context>"
				);
		}

		[Fact]
		public void SerializeDiscardsSensitiveProperties()
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(FileProperties.Password.Name, FileProperties.Password.Namespace, "p@ssw0rd", false),
						serializeProperty(new WCF.SharedAccessKey().Name.Name, WcfProperties.HttpHeaders.Namespace, "sh@r3d@cc3k3y", false),
						serializeProperty(new WCF.IssuerSecret().Name.Name, WcfProperties.HttpHeaders.Namespace, "s3cr3t", false),
						serializeProperty("myHardToFindPassWordProperty", "inYetHarderToFindNamespace", "s3cr3t", true)
					})
				.Should().Be(@"<context />");
		}

		[Fact]
		public void SerializeDistinguishedProperty()
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(
							"/*[local-name()='order' and namespace-uri()='urn:schemas.stateless.be:biztalk']/*[local-name()='id' and namespace-uri()='urn:schemas.stateless.be:biztalk']",
							"http://schemas.microsoft.com/BizTalk/2003/btsDistinguishedFields",
							"123456789",
							false)
					})
				.Should().Be(
					@"<context xmlns:s0=""http://schemas.microsoft.com/BizTalk/2003/btsDistinguishedFields"">"
					+ @"<s0:p n=""/*[local-name()='order' and namespace-uri()='urn:schemas.stateless.be:biztalk']/*[local-name()='id' and namespace-uri()='urn:schemas.stateless.be:biztalk']"">123456789</s0:p>"
					+ "</context>"
				);
		}

		[Fact]
		public void SerializeFactorsNamespacePrefixes()
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(BtsProperties.OutboundTransportLocation.Name, BtsProperties.OutboundTransportLocation.Namespace, @"file://c:\folder\ports\out", false),
						serializeProperty(FileProperties.ReceivedFileName.Name, FileProperties.ReceivedFileName.Namespace, "file-name", false),
					})
				.Should().Be(
					@$"<context xmlns:s0=""{BtsProperties.OutboundTransportLocation.Namespace}"" xmlns:s1=""{FileProperties.ReceivedFileName.Namespace}"">"
					+ @"<s0:p n=""OutboundTransportLocation"">file://c:\folder\ports\out</s0:p>"
					+ @"<s1:p n=""ReceivedFileName"">file-name</s1:p>"
					+ "</context>"
				);
		}

		[Fact]
		public void SerializeInvalidHexadecimalCharacter0X1A()
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(BtsProperties.OutboundTransportType.Name + (char) 0x1A, BtsProperties.OutboundTransportLocation.Namespace + (char) 0x1A, "FILE" + (char) 0x1A, false)
					})
				.Should().Be(
					@$"<context xmlns:s0=""{BtsProperties.OutboundTransportType.Namespace}&#x1A;"">"
					+ "<s0:p n=\"OutboundTransportType&#x1A;\">FILE&#x1A;</s0:p>"
					+ "</context>"
				);
		}

		[Fact]
		public void SerializePromotedProperty()
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(BtsProperties.OutboundTransportType.Name, BtsProperties.OutboundTransportType.Namespace, "FILE", true)
					})
				.Should().Be(
					@$"<context xmlns:s0=""{BtsProperties.OutboundTransportType.Namespace}"">"
					+ @"<s0:p n=""OutboundTransportType"" promoted=""true"">FILE</s0:p>"
					+ "</context>"
				);
		}

		[Fact]
		public void SerializePropertyInEmptyNamespace()
		{
			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(BtsProperties.OutboundTransportType.Name, string.Empty, "FILE", true),
						serializeProperty(BtsProperties.OutboundTransportLocation.Name, BtsProperties.OutboundTransportLocation.Namespace, @"file://c:\folder\ports\out", false)
					})
				.Should().Be(
					@$"<context xmlns:s1=""{BtsProperties.OutboundTransportLocation.Namespace}"">"
					+ @"<p n=""OutboundTransportType"" promoted=""true"">FILE</p>"
					+ @"<s1:p n=""OutboundTransportLocation"">file://c:\folder\ports\out</s1:p>"
					+ "</context>"
				);
		}
	}
}
