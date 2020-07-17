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

using System.Xml;
using System.Xml.Schema;
using Microsoft.XLANGs.BaseTypes;
using ValidatingXmlReaderSettingsBase = Be.Stateless.Xml.ValidatingXmlReaderSettings;

namespace Be.Stateless.BizTalk.Xml
{
	public static class ValidatingXmlReaderSettings
	{
		public static XmlReaderSettings Create<T>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict) where T : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(contentProcessing, new T().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(contentProcessing, new T1().CreateResolvedSchema(), new T2().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2, T3>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(
				contentProcessing,
				new T1().CreateResolvedSchema(),
				new T2().CreateResolvedSchema(),
				new T3().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2, T3, T4>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(
				contentProcessing,
				new T1().CreateResolvedSchema(),
				new T2().CreateResolvedSchema(),
				new T3().CreateResolvedSchema(),
				new T4().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2, T3, T4, T5>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(
				contentProcessing,
				new T1().CreateResolvedSchema(),
				new T2().CreateResolvedSchema(),
				new T3().CreateResolvedSchema(),
				new T4().CreateResolvedSchema(),
				new T5().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2, T3, T4, T5, T6>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(
				contentProcessing,
				new T1().CreateResolvedSchema(),
				new T2().CreateResolvedSchema(),
				new T3().CreateResolvedSchema(),
				new T4().CreateResolvedSchema(),
				new T5().CreateResolvedSchema(),
				new T6().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2, T3, T4, T5, T6, T7>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(
				contentProcessing,
				new T1().CreateResolvedSchema(),
				new T2().CreateResolvedSchema(),
				new T3().CreateResolvedSchema(),
				new T4().CreateResolvedSchema(),
				new T5().CreateResolvedSchema(),
				new T6().CreateResolvedSchema(),
				new T7().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2, T3, T4, T5, T6, T7, T8>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
			where T1 : SchemaBase, new()
			where T2 : SchemaBase, new()
			where T3 : SchemaBase, new()
			where T4 : SchemaBase, new()
			where T5 : SchemaBase, new()
			where T6 : SchemaBase, new()
			where T7 : SchemaBase, new()
			where T8 : SchemaBase, new()
		{
			return ValidatingXmlReaderSettingsBase.Create(
				contentProcessing,
				new T1().CreateResolvedSchema(),
				new T2().CreateResolvedSchema(),
				new T3().CreateResolvedSchema(),
				new T4().CreateResolvedSchema(),
				new T5().CreateResolvedSchema(),
				new T6().CreateResolvedSchema(),
				new T7().CreateResolvedSchema(),
				new T8().CreateResolvedSchema());
		}

		public static XmlReaderSettings Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Strict)
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
			return ValidatingXmlReaderSettingsBase.Create(
				contentProcessing,
				new T1().CreateResolvedSchema(),
				new T2().CreateResolvedSchema(),
				new T3().CreateResolvedSchema(),
				new T4().CreateResolvedSchema(),
				new T5().CreateResolvedSchema(),
				new T6().CreateResolvedSchema(),
				new T7().CreateResolvedSchema(),
				new T8().CreateResolvedSchema(),
				new T9().CreateResolvedSchema());
		}
	}
}
