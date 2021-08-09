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
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Xml
{
	/// <summary>
	/// Resolves Uniform Resource Identifiers (URIs) denoting either assembly-embedded XSLT resources or BizTalk Transform type's
	/// strong names.
	/// </summary>
	/// <remarks>
	/// The set of Uniform Resource Identifier (URI) that is supported is as follows:
	/// <list type="bullet">
	/// <item>
	/// <c>map://type/&lt;BizTalk Map's strong type name&gt;</c>, for instance
	/// <c>map://type/Be.Stateless.BizTalk.Xml.Xsl.IdentityTransform, Be.Stateless.BizTalk.Xml.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3707daa0b119fc14</c>
	/// </item>
	/// <item>
	/// <c>map://resource/&lt;embedded resource's full name&gt;</c>, for instance
	/// <c>map://resource/Be.Stateless.BizTalk.Xml.Data.Included.xsl</c>
	/// </item>
	/// <item>
	/// <c>map://resource/&lt;embedded resource's name&gt;</c>, for instance
	/// <c>map://resource/Data.Imported.xsl</c>
	/// </item>
	/// <item>
	/// <c>a file's absolute path without scheme</c>, for instance
	/// <c>C:\Files\Projects\be.stateless\BizTalkFactory\src\BizTalk.Common.Tests\Xml\Data\Imported.xsl</c>. Notice that this is
	/// natively supported by the <see cref="XmlUrlResolver"/> from which this class derives.
	/// </item>
	/// </list>
	/// </remarks>
	public class XslMapUrlResolver : XmlUrlResolver
	{
		public XslMapUrlResolver(Type type)
		{
			ReferenceType = type ?? throw new ArgumentNullException(nameof(type));
		}

		#region Base Class Member Overrides

		/// <summary>
		/// Maps a URI to an object that contains the actual resource.
		/// </summary>
		/// <param name="absoluteUri">
		/// The URI returned from <see cref="ResolveUri"/>.
		/// </param>
		/// <param name="role">
		/// Currently not used.
		/// </param>
		/// <param name="ofObjectToReturn">
		/// The type of object to return.
		/// </param>
		/// <returns>
		/// One of <see cref="Stream"/>, <see cref="XmlReader"/>, or <see cref="IXPathNavigable"/>.
		/// </returns>
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "InvertIf")]
		[SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			if (absoluteUri == null) throw new ArgumentNullException(nameof(absoluteUri));
			if (absoluteUri.Scheme == MAP_SCHEME)
			{
				if (absoluteUri.Host == TYPE_HOST)
				{
					var typeName = Uri.UnescapeDataString(absoluteUri.Segments[1]);
					var type = Type.GetType(typeName, true);
					var transform = (TransformBase) Activator.CreateInstance(type);
					// http://stackoverflow.com/questions/11864564/xslcompiledtransform-and-custom-xmlurlresolver-an-entry-with-the-same-key-alre
					var baseUri = absoluteUri.GetLeftPart(UriPartial.Authority) + "/" + type.FullName;
					using (var reader = XmlReader.Create(new StringReader(transform.XmlContent), new() { XmlResolver = null }, baseUri))
					{
						// http://stackoverflow.com/questions/1440023/can-i-assign-a-baseuri-to-an-xdocument
						var xDocument = XDocument.Load(reader, LoadOptions.SetBaseUri);
						// XDocument and XPathNavigator do not implement IDisposable while XmlReader does; to avoid IDisposable
						// issues, it is therefore simpler to return an XPathNavigator
						return xDocument.CreateNavigator();
					}
				}
				if (absoluteUri.Host == RESOURCE_HOST)
				{
					var assembly = ReferenceType.Assembly;
					// first look for a resource referenced by a simple name and if not found (i.e. null) by a full name
					var stream = assembly.GetManifestResourceStream(ReferenceType, absoluteUri.Segments[1])
						?? assembly.GetManifestResourceStream(absoluteUri.Segments[1]);
					return stream;
				}
			}
			return base.GetEntity(absoluteUri, role, ofObjectToReturn);
		}

		/// <summary>
		/// Resolves the absolute URI from the base and relative URIs.
		/// </summary>
		/// <param name="baseUri">
		/// The base URI used to resolve the relative URI.
		/// </param>
		/// <param name="relativeUri">
		/// The URI to resolve. The URI can be absolute or relative. If absolute, this value effectively replaces the baseUri
		/// value. If relative, it combines with the baseUri to make an absolute URI.
		/// </param>
		/// <returns>
		/// The absolute URI or null if the relative URI cannot be resolved.
		/// </returns>
		[SuppressMessage("ReSharper", "InvertIf")]
		[SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			var uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
			if (uri.Scheme == MAP_SCHEME)
			{
				if (uri.Host == TYPE_HOST) return uri;
				if (uri.Host == RESOURCE_HOST) return uri;
			}
			return base.ResolveUri(baseUri, relativeUri);
		}

		#endregion

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
		protected Type ReferenceType { get; }

		// ReSharper disable once MemberCanBePrivate.Global
		[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
		protected const string MAP_SCHEME = "map";

		// ReSharper disable once MemberCanBePrivate.Global
		[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
		protected const string RESOURCE_HOST = "resource";

		// ReSharper disable once MemberCanBePrivate.Global
		[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
		protected const string TYPE_HOST = "type";
	}
}
