#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
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
using System.Xml;
using System.Xml.Xsl;
using Be.Stateless.Extensions;
using Microsoft.BizTalk.Message.Interop;

namespace Be.Stateless.BizTalk.Message.ExtensionObjects
{
	/// <summary>
	/// XSLT extension object offering support for the <see cref="IBaseMessageContext"/> of the current <see
	/// cref="IBaseMessage"/>.
	/// </summary>
	/// <seealso cref="XsltArgumentList.AddExtensionObject"/>
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "XSLT Extension Object.")]
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "XSLT Extension Object.")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "XSLT Extension Object.")]
	public class BaseMessageContextFunctions
	{
		public BaseMessageContextFunctions(IBaseMessageContext context, IXmlNamespaceResolver xmlNamespaceResolver)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_xmlNamespaceResolver = xmlNamespaceResolver ?? throw new ArgumentNullException(nameof(xmlNamespaceResolver));
		}

		/// <summary>
		/// Returns the value of the property, identified by its XML Qualified name, from the current message context.
		/// </summary>
		/// <param name="qName">
		/// The XML Qualified name of the property, e.g. <c>bts:MessageType</c>.
		/// </param>
		/// <returns>
		/// The property value as a <see cref="string"/>.
		/// </returns>
		/// <remarks>
		/// <paramref name="qName"/> has to be an XML Qualified of the form <c>ns:name</c> where
		/// <list type="bullet">
		/// <item>
		/// <c>ns</c> is the prefix that the XSLT, to which an instance of this class will be added as an extension object,
		/// defines when declaring the target namespace of some property schema;
		/// </item>
		/// <item>
		/// <c>name</c> is the name a property defined in the latter property schema.
		/// </item>
		/// </list>
		/// </remarks>
		public object Read(string qName)
		{
			var qn = qName.ToQName(_xmlNamespaceResolver);
			return _context.Read(qn.Name, qn.Namespace) ?? string.Empty;
		}

		/// <summary>
		/// Promote the property, identified by its XML Qualified name, with a given value into the current message context.
		/// </summary>
		/// <param name="qName">
		/// The XML Qualified name of the property, e.g. <c>bts:MessageType</c>.
		/// </param>
		/// <param name="value">
		/// The property value as a <see cref="string"/>.
		/// </param>
		/// <remarks>
		/// <paramref name="qName"/> has to be an XML Qualified of the form <c>ns:name</c> where
		/// <list type="bullet">
		/// <item>
		/// <c>ns</c> is the prefix that the XSLT, to which an instance of this class will be added as an extension object,
		/// defines when declaring the target namespace of some property schema;
		/// </item>
		/// <item>
		/// <c>name</c> is the name a property defined in the latter property schema.
		/// </item>
		/// </list>
		/// </remarks>
		public void Promote(string qName, string value)
		{
			var qn = qName.ToQName(_xmlNamespaceResolver);
			_context.Promote(qn.Name, qn.Namespace, value);
		}

		/// <summary>
		/// Write the property, identified by its XML Qualified name, with a given value into the current message context.
		/// </summary>
		/// <param name="qName">
		/// The XML Qualified name of the property, e.g. <c>bts:MessageType</c>.
		/// </param>
		/// <param name="value">
		/// The property value as a <see cref="string"/>.
		/// </param>
		/// <remarks>
		/// <paramref name="qName"/> has to be an XML Qualified of the form <c>ns:name</c> where
		/// <list type="bullet">
		/// <item>
		/// <c>ns</c> is the prefix that the XSLT, to which an instance of this class will be added as an extension object,
		/// defines when declaring the target namespace of some property schema;
		/// </item>
		/// <item>
		/// <c>name</c> is the name a property defined in the latter property schema.
		/// </item>
		/// </list>
		/// </remarks>
		public void Write(string qName, string value)
		{
			var qn = qName.ToQName(_xmlNamespaceResolver);
			_context.Write(qn.Name, qn.Namespace, value);
		}

		private readonly IBaseMessageContext _context;
		private readonly IXmlNamespaceResolver _xmlNamespaceResolver;
	}
}
