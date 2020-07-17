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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Be.Stateless.BizTalk.Namespaces;
using Be.Stateless.BizTalk.Xml.Xsl.Extensions;
using Be.Stateless.Extensions;
using Be.Stateless.Linq.Extensions;
using Be.Stateless.Xml.XPath.Extensions;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Xml.Xsl
{
	[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
	public class XslCompiledTransformDescriptorBuilder
	{
		/// <summary>
		/// Create a <see cref="XslCompiledTransformDescriptorBuilder"/> instance that knows how to build the various constituents
		/// of a <see cref="XslCompiledTransformDescriptor"/> for the given <see cref="TransformBase"/>-derived transform.
		/// </summary>
		/// <param name="transform">The <see cref="TransformBase"/>-derived transform.</param>
		public XslCompiledTransformDescriptorBuilder(Type transform)
		{
			if (transform == null) throw new ArgumentNullException(nameof(transform));
			if (!transform.IsTransform())
				throw new ArgumentException(
					$"The type {transform.AssemblyQualifiedName} does not derive from TransformBase.",
					nameof(transform));
			var transformBase = Activator.CreateInstance(transform) as TransformBase;
			_transformBase = transformBase ?? throw new ArgumentException($"Cannot instantiate type '{transform.AssemblyQualifiedName}'.", nameof(transform));
			Transform = transform;
			_navigator = BuildNavigator();
		}

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
		protected Type Transform { get; }

		public virtual ExtensionRequirements BuildExtensionRequirements()
		{
			return !_navigator.LookupPrefix(ExtensionObjectNamespaces.MessageContext).IsNullOrEmpty()
				? ExtensionRequirements.MessageContext
				: ExtensionRequirements.None;
		}

		public virtual IXmlNamespaceResolver BuildNamespaceResolver()
		{
			var nsm = _navigator.GetNamespaceManager();
			_navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml)
				.ForEach(ns => nsm.AddNamespace(ns.Key, ns.Value));
			return nsm;
		}

		public virtual XslCompiledTransform BuildXslCompiledTransform()
		{
			var xslCompiledTransform = new XslCompiledTransform();
			xslCompiledTransform.Load(_navigator, XsltSettings.TrustedXslt, new XslMapUrlResolver(Transform));
			return xslCompiledTransform;
		}

		public virtual Stateless.Xml.Xsl.XsltArgumentList BuildXsltArgumentList()
		{
			return new(_transformBase.TransformArgs);
		}

		private XPathNavigator BuildNavigator()
		{
			using (var stringReader = XmlReader.Create(new StringReader(_transformBase.XmlContent), new() { XmlResolver = null }))
			{
				var navigator = new XPathDocument(stringReader).CreateNavigator();
				navigator.MoveToFollowing(XPathNodeType.Element);
				return navigator;
			}
		}

		private readonly XPathNavigator _navigator;
		private readonly TransformBase _transformBase;
	}
}
