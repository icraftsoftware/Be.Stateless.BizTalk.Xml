#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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

using System;
using Be.Stateless.BizTalk.ContextProperties;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Message
{
	public class MessageContextSerializerFixture
	{
		[Theory]
		[InlineData("Authorization: Bearer b3@r3r")]
		[InlineData("Content-Type: application/xml\nAuthorization: Bearer b3@r3r\nAccept: application/xml")]
		[InlineData("Content-Type: application/xml\rAuthorization: Bearer b3@r3r\rAccept: application/xml")]
		[InlineData("Content-Type: application/xml\r\nAuthorization: Bearer b3@r3r\r\nAccept: application/xml")]
		public void Serialize(string httpHeaders)
		{
			var redactedHttpHeaders = string.Join(
				Environment.NewLine,
				httpHeaders.Replace("Authorization: Bearer b3@r3r", string.Empty).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

			MessageContextSerializer.Serialize(
					serializeProperty => new[] {
						serializeProperty(BtsProperties.OutboundTransportLocation.Name, BtsProperties.OutboundTransportLocation.Namespace, @"file://c:\folder\ports\out", false),
						serializeProperty(BtsProperties.OutboundTransportType.Name, BtsProperties.OutboundTransportType.Namespace, "FILE", true),
						serializeProperty(FileProperties.Password.Name, FileProperties.Password.Namespace, "p@ssw0rd", false),
						serializeProperty(new WCF.SharedAccessKey().Name.Name, new WCF.SharedAccessKey().Name.Namespace, "sh@r3d@cc3k3y", false),
						serializeProperty(new WCF.IssuerSecret().Name.Name, new WCF.IssuerSecret().Name.Namespace, "s3cr3t", false),
						serializeProperty(WcfProperties.HttpHeaders.Name, WcfProperties.HttpHeaders.Namespace, httpHeaders, false),
						serializeProperty(
							"/*[local-name()='order' and namespace-uri()='urn:schemas.stateless.be:biztalk']/*[local-name()='id' and namespace-uri()='urn:schemas.stateless.be:biztalk']",
							"http://schemas.microsoft.com/BizTalk/2003/btsDistinguishedFields",
							"123456789",
							false)
					})
				.Should().Be(
					@"<context xmlns:s0=""http://schemas.microsoft.com/BizTalk/2003/system-properties"" xmlns:s1=""http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties"" xmlns:s2=""http://schemas.microsoft.com/BizTalk/2003/btsDistinguishedFields"">"
					+ @"<s0:p n=""OutboundTransportLocation"">file://c:\folder\ports\out</s0:p>"
					+ @"<s0:p n=""OutboundTransportType"" promoted=""true"">FILE</s0:p>"
					+ @"<s1:p n=""HttpHeaders"">" + redactedHttpHeaders + "</s1:p>"
					+ @"<s2:p n=""/*[local-name()='order' and namespace-uri()='urn:schemas.stateless.be:biztalk']/*[local-name()='id' and namespace-uri()='urn:schemas.stateless.be:biztalk']"">123456789</s2:p>"
					+ "</context>"
				);
		}
	}
}
