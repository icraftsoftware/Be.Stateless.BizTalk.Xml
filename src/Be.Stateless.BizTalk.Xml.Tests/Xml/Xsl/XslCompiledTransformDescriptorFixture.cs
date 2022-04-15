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

using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Dummies.Transform;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Resources;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Xml.Xsl
{
	public class XslCompiledTransformDescriptorFixture
	{
		[Fact]
		public void ExtensionRequirementsRequireMessageContext()
		{
			var descriptor = new XslCompiledTransformDescriptor(new(typeof(CompoundContextMapTransform)));
			descriptor.ExtensionRequirements.Should().Be(ExtensionRequirements.MessageContext);
			descriptor.ExtensionRequirements.RequireMessageContext().Should().BeTrue();
			descriptor.ExtensionRequirements.RequireNone().Should().BeFalse();
		}

		[Fact]
		public void ExtensionRequirementsRequireNone()
		{
			var descriptor = new XslCompiledTransformDescriptor(new(typeof(IdentityTransform)));
			descriptor.ExtensionRequirements.Should().Be(ExtensionRequirements.None);
			descriptor.ExtensionRequirements.RequireMessageContext().Should().BeFalse();
			descriptor.ExtensionRequirements.RequireNone().Should().BeTrue();
		}

		[Fact]
		public void ImplicitlyReliesOnXslMapUrlResolver()
		{
			Invoking(() => new XslCompiledTransformDescriptor(new(typeof(CompoundMapTransform)))).Should().NotThrow();
		}

		[Fact]
		public void NamespaceResolverIncludesCustomAndStandardXmlNamespaces()
		{
			var sut = new XslCompiledTransformDescriptor(new(typeof(CompoundContextMapTransform)));
			var namespaceResolver = sut.NamespaceResolver;

			((IEnumerable) namespaceResolver).Cast<string>().Should().BeEquivalentTo(string.Empty, "xmlns", "xml", "xs", "xsi", "xsl", "bf", "bts", "ctx");
			namespaceResolver.LookupNamespace(string.Empty).Should().Be(string.Empty);
			namespaceResolver.LookupNamespace("xmlns").Should().Be(XNamespace.Xmlns.NamespaceName);
			namespaceResolver.LookupNamespace("xml").Should().Be(XNamespace.Xml.NamespaceName);
			namespaceResolver.LookupNamespace("xs").Should().Be(XmlSchema.Namespace);
			namespaceResolver.LookupNamespace("xsi").Should().Be(XmlSchema.InstanceNamespace);
			namespaceResolver.LookupNamespace("xsl").Should().Be("http://www.w3.org/1999/XSL/Transform");
			namespaceResolver.LookupNamespace("bf").Should().Be(BizTalkFactoryProperties.ContextBuilderTypeName.Namespace);
			namespaceResolver.LookupNamespace("bts").Should().Be(BtsProperties.ActualRetryCount.Namespace);
			namespaceResolver.LookupNamespace("ctx").Should().Be("urn:extensions.stateless.be:biztalk:message:context:2012:12");
			namespaceResolver.LookupNamespace("tp").Should().BeNull();
		}

		[Fact]
		public void NamespaceResolverIsNullWhenMessageContextIsNotRequired()
		{
			var descriptor = new XslCompiledTransformDescriptor(new(typeof(IdentityTransform)));
			descriptor.NamespaceResolver.Should().BeNull();
		}

		[Fact]
		public void XsltSpaceEntitiesAreNotDiscarded()
		{
			var sut = new XslCompiledTransformDescriptor(new(typeof(TextTransform)));

			using (var output = new MemoryStream())
			{
				var settings = sut.CompiledXslt.OutputSettings.Clone();
				settings.CloseOutput = false;
				settings.Encoding = Encoding.UTF8;
				settings.NewLineHandling = NewLineHandling.None;
				using (var inputStream = ResourceManager.Load(Assembly.GetExecutingAssembly(), "Be.Stateless.BizTalk.Resources.Data.input.xml"))
				using (var inputReader = XmlReader.Create(inputStream))
				using (var outputWriter = XmlWriter.Create(output, settings))
				{
					sut.CompiledXslt.Transform(inputReader, outputWriter);
				}
				output.Rewind().ReadToEnd().Should().Be(ResourceManager.Load(Assembly.GetExecutingAssembly(), "Be.Stateless.BizTalk.Resources.Data.output.csv").ReadToEnd());
			}
		}
	}
}
