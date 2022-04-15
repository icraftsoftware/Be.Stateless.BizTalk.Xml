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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.Extensions;
using WCF;

namespace Be.Stateless.BizTalk.Message
{
	public static class MessageContextSerializer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Serialize(Func<Func<string, string, object, bool, XElement>, IEnumerable<XElement>> forEachProperty)
		{
			if (forEachProperty == null) throw new ArgumentNullException(nameof(forEachProperty));
			// cache xmlns while constructing xml info set...
			var nsCache = new XmlDictionary();
			var xmlDocument = new XElement(
				"context",
				forEachProperty((name, ns, value, isPromoted) => SerializeProperty(nsCache, name, ns, value, isPromoted))
			);
			// ... and declare/alias all of them at the root element level to minimize xml string size
			for (var i = 0; nsCache.TryLookup(i, out var xds); i++)
			{
				if (!xds.Value.IsNullOrEmpty()) xmlDocument.Add(new XAttribute(XNamespace.Xmlns + "s" + xds.Key.ToString(CultureInfo.InvariantCulture), xds.Value));
			}

			// take control of serialization in order to output invalid XML characters such as 0x1A under their encoded entity form
			var builder = new StringBuilder(7 * 1024); // 7 KB is the average size of message contexts
			using (var writer = XmlWriter.Create(builder, _xmlWriterSettings))
			{
				xmlDocument.WriteTo(writer);
			}
			return builder.ToString();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static XElement SerializeProperty(XmlDictionary nsCache, string name, string ns, object value, bool isPromoted)
		{
			// give each property element a name of 'p' and store its actual name inside the 'n' attribute, which avoids
			// the cost of the name.IsValidQName() check for each of them as the name could be an xpath expression in the
			// case of a distinguished property
			return name.IsSensitiveProperty()
				? null
				: new XElement(
					(XNamespace) nsCache.Add(ns).Value + "p",
					new XAttribute("n", name),
					isPromoted ? new XAttribute("promoted", true) : null,
					name.IsHttpHeaders(ns) ? value.ToString().Redact() : value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsSensitiveProperty(this string name)
		{
			return name.IndexOf(nameof(Password), StringComparison.OrdinalIgnoreCase) > -1
				|| name.Equals(nameof(SharedAccessKey), StringComparison.OrdinalIgnoreCase)
				|| name.Equals(nameof(IssuerSecret), StringComparison.OrdinalIgnoreCase);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsHttpHeaders(this string name, string ns)
		{
			return name.Equals(WcfProperties.HttpHeaders.Name, StringComparison.OrdinalIgnoreCase)
				&& ns.Equals(WcfProperties.HttpHeaders.Namespace, StringComparison.OrdinalIgnoreCase);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string Redact(this string value)
		{
			return value.IndexOf(nameof(HttpRequestHeaders.Authorization), StringComparison.OrdinalIgnoreCase) < 0
				? value
				: string.Join(
					Environment.NewLine,
					value
						.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
						.Select(h => h.Trim())
						.Where(h => !h.StartsWith(nameof(HttpRequestHeaders.Authorization), StringComparison.OrdinalIgnoreCase)));
		}

		private static readonly XmlWriterSettings _xmlWriterSettings = new() {
			CheckCharacters = false, // allows to output equivalent entities for otherwise invalid XML characters
			ConformanceLevel = ConformanceLevel.Fragment,
			OmitXmlDeclaration = true
		};
	}
}
