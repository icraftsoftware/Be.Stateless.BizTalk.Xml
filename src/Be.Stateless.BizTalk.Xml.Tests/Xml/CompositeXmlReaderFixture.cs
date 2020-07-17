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
using System.IO;
using System.Text;
using System.Xml;
using Be.Stateless.BizTalk.Dummies.Transform;
using Be.Stateless.BizTalk.Xml.Xsl;
using Be.Stateless.IO;
using Be.Stateless.Linq.Extensions;
using Be.Stateless.Reflection;
using Be.Stateless.Text.Extensions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.Xml
{
	public class CompositeXmlReaderFixture
	{
		[Fact]
		public void AggregatesIndividualStreamsOfDifferentEncodings()
		{
			using (var part1 = new MemoryStream(_part1))
			using (var part2 = new MemoryStream(_part2))
			using (var part3 = new MemoryStream(_part3))
			using (var composite = CompositeXmlReader.Create(new[] { part1, part2, part3 }))
			{
				composite.Read();
				composite.ReadOuterXml().Should().Be(EXPECTED);
			}
		}

		[Fact]
		public void AggregatesInitializedXmlReaders()
		{
			var part1 = XmlReader.Create(new StringReader(PART_1), new() { CloseInput = true });
			var part2 = XmlReader.Create(new StringReader(PART_2), new() { CloseInput = true });
			var part3 = XmlReader.Create(new StringReader(PART_3), new() { CloseInput = true });

			part1.MoveToContent();
			part2.MoveToContent();
			part3.MoveToContent();

			using (var composite = CompositeXmlReader.Create(new[] { part1, part2, part3 }))
			{
				composite.MoveToContent();
				composite.ReadOuterXml().Should().Be(EXPECTED);
			}
		}

		[Fact]
		public void AggregatesUninitializedXmlReaders()
		{
			var part1 = XmlReader.Create(new StringReader(PART_1), new() { CloseInput = true });
			var part2 = XmlReader.Create(new StringReader(PART_2), new() { CloseInput = true });
			var part3 = XmlReader.Create(new StringReader(PART_3), new() { CloseInput = true });

			using (var composite = CompositeXmlReader.Create(new[] { part1, part2, part3 }))
			{
				composite.MoveToContent();
				composite.ReadOuterXml().Should().Be(EXPECTED);
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void CompoundXmlReadersShareXmlNameTable()
		{
			var part1 = XmlReader.Create(new StringReader(PART_1), new() { CloseInput = true });
			var part2 = XmlReader.Create(new StringReader(PART_2), new() { CloseInput = true });
			var part3 = XmlReader.Create(new StringReader(PART_3), new() { CloseInput = true });

			using (var composite = CompositeXmlReader.Create(new[] { part1, part2, part3 }))
			{
				var outlineReader = (XmlReader) Reflector.GetField(composite, "_outlineReader");
				outlineReader.NameTable.Should().BeSameAs(composite.NameTable);

				var readers = (XmlReader[]) Reflector.GetField(composite, "_readers");
				readers.ForEach(r => r.NameTable.Should().BeSameAs(composite.NameTable));
				// TODO create XUnit custom constraint for readers.Select(r => r.NameTable).Should().AllBeSameAs(composite.NameTable);
			}
		}

		[Fact]
		public void InputStreamsAreClosed()
		{
			var mock1 = new Mock<MemoryStream>(MockBehavior.Default, _part1) { CallBase = true };
			var mock2 = new Mock<MemoryStream>(MockBehavior.Default, _part2) { CallBase = true };
			var mock3 = new Mock<MemoryStream>(MockBehavior.Default, _part3) { CallBase = true };

			using (CompositeXmlReader.Create(new[] { mock1.Object, mock2.Object, mock3.Object }, new() { CloseInput = true })) { }

			mock1.Verify(s => s.Close());
			mock2.Verify(s => s.Close());
			mock3.Verify(s => s.Close());
		}

		[Fact]
		public void SupportsXslTransformOfAggregatedStreams()
		{
			using (var part1 = new StringStream(PART_1))
			using (var part2 = new StringStream(PART_2))
			using (var part3 = new StringStream(PART_3))
			using (var composite = CompositeXmlReader.Create(new[] { part1, part2, part3 }))
			{
				var xslTransformDescriptor = new XslCompiledTransformDescriptor(new(typeof(IdentityTransform)));
				var builder = new StringBuilder();
				using (var writer = XmlWriter.Create(builder))
				{
					xslTransformDescriptor.XslCompiledTransform.Transform(composite, xslTransformDescriptor.Arguments, writer!);
				}
				builder.GetReaderAtContent().ReadOuterXml().Should().Be(EXPECTED);
			}
		}

		[Fact]
		public void SupportsXslTransformOfAggregatedXmlReaders()
		{
			using (var part1 = new StringStream(PART_1))
			using (var part2 = new StringStream(PART_2))
			using (var part3 = new StringStream(PART_3))
			using (var composite = CompositeXmlReader.Create(new[] { XmlReader.Create(part1), XmlReader.Create(part2), XmlReader.Create(part3) }))
			{
				var xslTransformDescriptor = new XslCompiledTransformDescriptor(new(typeof(IdentityTransform)));
				var builder = new StringBuilder();
				using (var writer = XmlWriter.Create(builder))
				{
					xslTransformDescriptor.XslCompiledTransform.Transform(composite, xslTransformDescriptor.Arguments, writer!);
				}
				builder.GetReaderAtContent().ReadOuterXml().Should().Be(EXPECTED);
			}
		}

		private readonly byte[] _part1 = Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + PART_1);
		private readonly byte[] _part2 = Encoding.Unicode.GetBytes("<?xml version=\"1.0\" encoding=\"utf-16\" ?>" + PART_2);
		private readonly byte[] _part3 = Encoding.GetEncoding("iso-8859-1").GetBytes("<?xml version=\"1.0\" encoding=\"iso-8859-1\" ?>" + PART_3);

		private const string EXPECTED = "<agg:Root xmlns:agg=\"http://schemas.microsoft.com/BizTalk/2003/aggschema\">" +
			"<agg:InputMessagePart_0>" + PART_1 + "</agg:InputMessagePart_0>" +
			"<agg:InputMessagePart_1>" + PART_2 + "</agg:InputMessagePart_1>" +
			"<agg:InputMessagePart_2>" + PART_3 + "</agg:InputMessagePart_2></agg:Root>";

		private const string PART_1 = "<part-one xmlns=\"part-one\"><child-one>one</child-one></part-one>";
		private const string PART_2 = "<part-two xmlns=\"part-two\"><child-two>two</child-two></part-two>";
		private const string PART_3 = "<part-six xmlns=\"part-six\"><child-six>six</child-six></part-six>";
	}
}
