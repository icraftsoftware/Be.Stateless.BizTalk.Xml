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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Xml
{
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API")]
	public static class ValidatingXmlReader
	{
		public static XmlReader Create<T>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T>(contentProcessing));
		}

		public static XmlReader Create<T>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T>(contentProcessing));
		}

		public static XmlReader Create<T>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T>(contentProcessing));
		}

		public static XmlReader Create<T1, T2>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2>(contentProcessing));
		}

		public static XmlReader Create<T1, T2>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2>(contentProcessing));
		}

		public static XmlReader Create<T1, T2>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2, T3>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7, T8>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
			where T8 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7, T8>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7, T8>(TextReader input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
			where T8 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7, T8>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7, T8>(XmlReader reader, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
			where T8 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7, T8>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Stream input, XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
			where T8 : SchemaBase, new()
			where T9 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TextReader input,
			XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
			where T8 : SchemaBase, new()
			where T9 : SchemaBase, new()
		{
			return XmlReader.Create(input, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(contentProcessing));
		}

		public static XmlReader Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			XmlReader reader,
			XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
			where T8 : SchemaBase, new()
			where T9 : SchemaBase, new()
		{
			return XmlReader.Create(reader, ValidatingXmlReaderSettings.Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(contentProcessing));
		}
	}
}
