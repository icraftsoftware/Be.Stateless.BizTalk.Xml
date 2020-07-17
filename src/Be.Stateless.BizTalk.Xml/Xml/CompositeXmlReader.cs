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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Be.Stateless.BizTalk.Schemas;
using Be.Stateless.Extensions;
using Be.Stateless.Linq.Extensions;
using Be.Stateless.Xml;
using Microsoft.BizTalk.Streaming;

namespace Be.Stateless.BizTalk.Xml
{
	/// <summary>
	/// Aggregates, at the XML information set level, several <see cref="Stream"/>s whose contents is XML.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Because the input streams are aggregated at the XML information set level, the contents of the input streams can be of
	/// various and distinct encodings and might, as well, include or not an <see cref="XmlDeclaration"/>.
	/// </para>
	/// <para>
	/// The contents of the aggregated <see cref="Stream"/>s is wrapped in an XML structured as follows:
	/// <code>
	/// <![CDATA[
	/// <agg:Root xmlns:agg="http://schemas.microsoft.com/BizTalk/2003/aggschema">
	///   <agg:InputMessagePart_0>
	///     ... content of 1st message part ...
	///   </agg:InputMessagePart_0>
	///   <agg:InputMessagePart_1>
	///     ... content of 2nd message part ...
	///   </agg:InputMessagePart_1>
	///   ...
	///   <agg:InputMessagePart_n>
	///     ... content of nth message part ...
	///   </agg:InputMessagePart_n>
	/// </agg:Root>
	/// ]]>
	/// </code>
	/// </para>
	/// </remarks>
	/// <seealso href="Microsoft.XLANGs.Core.Service.CompositeStreamReader, Microsoft.BizTalk.BtsDbVersion, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"/>
	[SuppressMessage("ReSharper", "LocalizableElement")]
	public class CompositeXmlReader : XmlReaderWrapper
	{
		#region Nested Type: CompoundXmlReader

		private class CompoundXmlReader : XmlReaderWrapper
		{
			public CompoundXmlReader(XmlReader reader) : base(reader) { }

			#region Base Class Member Overrides

			public override int Depth => InnerReader.Depth + 2;

			#endregion
		}

		#endregion

		/// <summary>
		/// Creates a new <see cref="CompositeXmlReader"/> instance wrapping a set of XML data <see cref="XmlReader"/>s as
		/// specified by <paramref name="readers"/> and with the specified <see cref="XmlReaderSettings"/> object.
		/// </summary>
		/// <param name="readers">
		/// The set of compound <see cref="XmlReader"/>s containing the pieces of XML data to wrap.
		/// </param>
		/// <param name="settings">
		/// The <see cref="XmlReaderSettings"/> object used to configure the new <see cref="CompositeXmlReader"/> instance. This
		/// value can be <c>null</c>.
		/// </param>
		/// <returns>
		/// A <see cref="CompositeXmlReader"/> object to read the compound XML <see cref="Stream"/>s as a whole XML composite.
		/// </returns>
		public static XmlReader Create(IEnumerable<XmlReader> readers, XmlReaderSettings settings = null)
		{
			var enumerable = readers as XmlReader[] ?? readers.ToArray(); // checks that streams is not null as well
			if (!enumerable.Any()) throw new ArgumentException("List of compound XmlReaders is empty.", nameof(readers));

			// ensuring all compound XmlReaders share the same XmlNameTable requires to turn them into streams that will be wrapped
			// again into other new XmlReaders that, this time, offer explicit control on the XmlNameTable. At least this is
			// necessary for System.Xml.XmlTextReaderImpl ---a common default XmlReader implementation returned by
			// XmlReader.Create(),--- as it does not honor the virtual NameTable property getter when accessing its NameTable
			// internally; it is not possible to wrap it into another XmlReaderWrapper that overrides the NameTable getter on
			// purpose. XmlTextReaderImpl should probably have been sealed...
			var streams = enumerable
				.Select(r => (Stream) new XmlTranslatorStream(r.ReadState == ReadState.Initial ? r : r.ReadSubtree()));
			return Create(streams, settings);
		}

		/// <summary>
		/// Creates a new <see cref="CompositeXmlReader"/> instance wrapping a set of XML data <see cref="Stream"/>s as specified
		/// by <paramref name="streams"/> and with the specified <see cref="XmlReaderSettings"/> object.
		/// </summary>
		/// <param name="streams">
		/// The set of compound <see cref="Stream"/>s containing the pieces of XML data to wrap.
		/// </param>
		/// <param name="settings">
		/// The <see cref="XmlReaderSettings"/> object used to configure the new <see cref="CompositeXmlReader"/> instance. This
		/// value can be <c>null</c>.
		/// </param>
		/// <returns>
		/// A <see cref="CompositeXmlReader"/> object to read the compound XML <see cref="Stream"/>s as a whole XML composite.
		/// </returns>
		public static XmlReader Create(IEnumerable<Stream> streams, XmlReaderSettings settings = null)
		{
			var enumerable = streams as Stream[] ?? streams.ToArray(); // checks that streams is not null as well
			if (!enumerable.Any()) throw new ArgumentException("List of compound streams is empty.", nameof(streams));

			// it is *essential* that all XmlReaders share the same XmlNameTable for any (most) XslTransform(s) to succeed
			settings = settings.IfNotNull(s => s.Clone()) ?? new XmlReaderSettings { XmlResolver = null };
			settings.NameTable = new NameTable();

			var compoundReaders = enumerable
				.Select(s => new CompoundXmlReader(XmlReader.Create(s, settings)))
				.ToArray();
			var outlineReader = CreateOutline(compoundReaders.Length, settings);
			return new CompositeXmlReader(outlineReader, compoundReaders);
		}

		private static XmlReader CreateOutline(int parts, XmlReaderSettings settings)
		{
			var builder = new StringBuilder(1024);
			builder.Append($"<agg:Root xmlns:agg=\"{SchemaNamespaces.BizTalkAggregate}\">");
			for (var i = 0; i < parts; i++)
			{
				builder.AppendFormat(CultureInfo.InvariantCulture, "<agg:InputMessagePart_{0}></agg:InputMessagePart_{0}>", i);
			}

			builder.Append("</agg:Root>");

			settings = settings.Clone();
			settings.CloseInput = true;
			return Create(new StringReader(builder.ToString()), settings);
		}

		private CompositeXmlReader(XmlReader outlineReader, CompoundXmlReader[] compoundReaders) : base(outlineReader)
		{
			_outlineReader = outlineReader;
			_readers = compoundReaders;
		}

		#region Base Class Member Overrides

		public override void Close()
		{
			_outlineReader.Close();
			_readers.ForEach(r => r.Close());
		}

		public override bool Read()
		{
			// determine both next _state and InnerReader, and always call InnerReader.Read() once
			switch (_state)
			{
				case CompositeXmlReaderState.RootAggregateOpeningTag:
					_state = CompositeXmlReaderState.MessagePartWrapperOpeningTag;
					return InnerReader.Read();

				case CompositeXmlReaderState.MessagePartWrapperOpeningTag:
					_state = CompositeXmlReaderState.MessagePartDocumentElement;
					return InnerReader.Read();

				case CompositeXmlReaderState.MessagePartDocumentElement:
					_state = CompositeXmlReaderState.MessagePartContent;
					InnerReader = _readers[_currentMessagePartIndex];
					// read along but skip any XmlDeclaration
					while (InnerReader.Read())
					{
						if (InnerReader.NodeType != XmlNodeType.XmlDeclaration) return true;
					}
					// switch to next state if fall through
					return Read();

				case CompositeXmlReaderState.MessagePartContent:
					if (InnerReader.Read()) return true;
					// switch to next state if fall through
					_state = CompositeXmlReaderState.MessagePartWrapperClosingTag;
					InnerReader = _outlineReader;
					return Read();

				case CompositeXmlReaderState.MessagePartWrapperClosingTag:
					_state = ++_currentMessagePartIndex < _readers.Length
						? CompositeXmlReaderState.MessagePartWrapperOpeningTag
						: CompositeXmlReaderState.RootAggregateClosingTag;
					return InnerReader.Read();

				case CompositeXmlReaderState.RootAggregateClosingTag:
					return InnerReader.Read();

				default:
					throw new InvalidOperationException($"Unexpected state value: {_state}.");
			}
		}

		#endregion

		private readonly XmlReader _outlineReader;
		private readonly CompoundXmlReader[] _readers;
		private int _currentMessagePartIndex;
		private CompositeXmlReaderState _state = CompositeXmlReaderState.RootAggregateOpeningTag;
	}
}
