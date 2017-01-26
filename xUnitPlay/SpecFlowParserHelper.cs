using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gherkin.Ast;
using TechTalk.SpecFlow.Parser;

namespace xUnitPlay
{
    public static class SpecFlowParserHelper
    {
        public static async Task<SpecFlowDocument> ParseSpecFlowDocumentAsync(string featureFilePath)
        {
            var fileContent = await ReadAllTextAsync(featureFilePath);
            var parser = new SpecFlowGherkinParser(new CultureInfo("en-US"));
            var gherkinDocument = parser.Parse(new StringReader(fileContent), featureFilePath);
            return gherkinDocument;
        }

        public static SpecFlowDocument ParseSpecFlowDocument(string featureFilePath)
        {
            using (var reader = new StreamReader(featureFilePath))
            {
                var parser = new SpecFlowGherkinParser(new CultureInfo("en-US"));
                var gherkinDocument = parser.Parse(reader, featureFilePath);
                return gherkinDocument;
            }
        }

        private static async Task<string> ReadAllTextAsync(string filePath)
        {
            using (var reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static IEnumerable<string> GetTags(this IEnumerable<Tag> tagList)
        {
            if (tagList == null)
                yield break;
            foreach (var tag in tagList)
            {
                yield return tag.Name.TrimStart('@');
            }
        }
    }
}
