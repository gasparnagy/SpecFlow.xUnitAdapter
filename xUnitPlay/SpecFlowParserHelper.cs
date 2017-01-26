using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser;

namespace xUnitPlay
{
    public static class SpecFlowParserHelper
    {
        public static async Task<SpecFlowDocument> ParseSpecFlowDocument(string featureFilePath)
        {
            var fileContent = await ReadAllTextAsync(featureFilePath);
            var parser = new SpecFlowGherkinParser(new CultureInfo("en-US"));
            var gherkinDocument = parser.Parse(new StringReader(fileContent), featureFilePath);
            return gherkinDocument;
        }

        private static async Task<string> ReadAllTextAsync(string filePath)
        {
            using (var reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
