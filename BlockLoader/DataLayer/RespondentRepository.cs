using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BlockLoader.DataLayer
{
    public class RespondentRepository : IRespondentRepository
    {
		private const string RespondentElementName = "respondent";
		private const string ReachedBlocksElementName = "reachedblocks";
		private const string ReachedBlockElementName = "reachedblock";
		private const string CodeElementName = "code";

		private readonly XmlLoader _loader;
        private readonly string _filePath;
        public RespondentRepository(XmlLoader loader, string filePath)
        {
            _loader = loader;
            _filePath = filePath;
        }
		public IEnumerable<Respondent> LoadRespondents()
		{
			if (!File.Exists(_filePath))
			{
				throw new FileNotFoundException(_filePath);
			}

			var doc = LoadDocument(_filePath);
			if (doc?.Root == null)
			{
				throw new InvalidOperationException("Xml file is empty, or invalid.");
			}

			var respondentElements = doc.Root.Elements(RespondentElementName);

			return respondentElements.Select(CreateRespondentFromElement).Where(b => b != null);
		}
		private Respondent CreateRespondentFromElement(XElement respondentElement)
		{
			HashSet<string> reachedBlocks = null;

			var reachedBlocksElement = respondentElement.Element(ReachedBlocksElementName);

			if (reachedBlocksElement == null)
            {
				throw new InvalidOperationException("RespondentElement missing ReachedBlocks Element");
			}

			var reachedBlockElement = reachedBlocksElement.Elements(ReachedBlockElementName).Where(element => element != null);

			reachedBlocks = new HashSet<string>();

			foreach (string code in reachedBlockElement.Select(GetCodeFromReachedBlockElement).Where(c => c != null))
            {
				if (!reachedBlocks.Contains(code))
                {
					reachedBlocks.Add(code);
                }
            }

			return new Respondent(reachedBlocks);
		}
		private string GetCodeFromReachedBlockElement(XElement reachedBlockElement)
        {
			var attribute = reachedBlockElement.Attribute(XName.Get(CodeElementName));

			if (attribute == null)
            {
                throw new InvalidOperationException("ReachedBlockElement missing code attribute");
            }

			if (attribute.Value == null)
            {
				throw new FormatException("Code is null");
			}

			return attribute.Value;
        }
		private XDocument LoadDocument(string filePath)
		{
			return _loader.Load(filePath);
		}
	}
}
