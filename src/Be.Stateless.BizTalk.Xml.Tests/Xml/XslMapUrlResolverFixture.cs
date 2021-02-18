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

using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Xsl;
using Be.Stateless.IO;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Xml
{
	public class XslMapUrlResolverFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ResolveImportedAndIncludedEmbeddedXslt()
		{
			const string compositeMapResourceTransform = @"<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
	<xsl:import href='map://resource/Be.Stateless.BizTalk.Resources.Xsl.Imported.xsl'/>
	<xsl:include href='map://resource/Be.Stateless.BizTalk.Resources.Xsl.Included.xsl'/>
	<xsl:template match='*[3]'>Matched by Composite.xsl</xsl:template>
</xsl:stylesheet>";
			using (var reader = XmlReader.Create(new StringStream(compositeMapResourceTransform)))
			{
				Invoking(() => new XslCompiledTransform().Load(reader, XsltSettings.TrustedXslt, new XslMapUrlResolver(typeof(XslMapUrlResolverFixture))))
					.Should().NotThrow();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ResolveImportedAndIncludedMapTypes()
		{
			const string compositeMapTypeTransform = @"<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
	<xsl:import href='map://type/Be.Stateless.BizTalk.Dummies.Transform.CompoundMapTransform, Be.Stateless.BizTalk.Xml.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3707daa0b119fc14'/>
	<xsl:include href='map://type/Be.Stateless.BizTalk.Dummies.Transform.IdentityTransform, Be.Stateless.BizTalk.Xml.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3707daa0b119fc14'/>
	<xsl:variable name='attachment-fragment'>
		<Attachment/>
	</xsl:variable>
	<xsl:variable name='messageType' select=""'COMPOSITE_MESSAGE_TYPE'""/>
</xsl:stylesheet>";
			using (var reader = XmlReader.Create(new StringStream(compositeMapTypeTransform)))
			{
				Invoking(() => new XslCompiledTransform().Load(reader, XsltSettings.TrustedXslt, new XslMapUrlResolver(typeof(XslMapUrlResolverFixture))))
					.Should().NotThrow();
			}
		}
	}
}
