using System.Threading.Tasks;
using System.Collections.Generic;
using BlockLoader.DataLayer;
using BlockLoader.PresentationLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockLoader.Tests
{
	[TestClass]
	public class MainWindowViewModelTests
	{
		[TestMethod]
		public async Task LoadBlocks_AllBlocksLoaded()
		{
			var blocks = new[]
			             {
				             new Block("a", 20, "Neváhej a toč"),
				             new Block("b", 25, "Neváhej a koukej"),
				             new Block("c", 35, "Neváhej a padej")
			             };

            var mainWindowViewModel = new MainWindowViewModel(new BlockRepositoryFake(blocks), null);
			await mainWindowViewModel.LoadBlocks();

			Assert.AreEqual(blocks.Length, mainWindowViewModel.Blocks.Count);

			Assert.AreEqual(blocks[0].Program, mainWindowViewModel.Blocks[0].Program);
			Assert.AreEqual(blocks[1].Program, mainWindowViewModel.Blocks[1].Program);
			Assert.AreEqual(blocks[2].Program, mainWindowViewModel.Blocks[2].Program);
		}

		[TestMethod]
		public async Task LoadRespondents_AllRespondentsLoaded()
        {

			Respondent[] respondents = CreateRespondents();

			var mainWindowViewModel = new MainWindowViewModel(null, new RespondentRepositoryFake(respondents));

			await mainWindowViewModel.LoadRespondents();

			Assert.AreEqual(respondents.Length, mainWindowViewModel.Respondents.Count);

			Assert.AreEqual(respondents[0].ReachedBlocks.Count, mainWindowViewModel.Respondents[0].ReachedBlocks.Count);
			Assert.AreEqual(respondents[1].ReachedBlocks.Count, mainWindowViewModel.Respondents[1].ReachedBlocks.Count);
			Assert.AreEqual(respondents[2].ReachedBlocks.Count, mainWindowViewModel.Respondents[2].ReachedBlocks.Count);

			Assert.AreEqual(respondents[0].ReachedBlocks, mainWindowViewModel.Respondents[0].ReachedBlocks);
			Assert.AreEqual(respondents[1].ReachedBlocks, mainWindowViewModel.Respondents[1].ReachedBlocks);
			Assert.AreEqual(respondents[2].ReachedBlocks, mainWindowViewModel.Respondents[2].ReachedBlocks);

		}

		[TestMethod]
		public void ReachedBlockCounter_Test()
        {
			Respondent[] respondents = CreateRespondents();

			ReachedBlockCounter counter = new ReachedBlockCounter();

			Assert.AreEqual(counter.Count(respondents, "A01"), 2);
			Assert.AreEqual(counter.Count(respondents, "A02"), 2);
			Assert.AreEqual(counter.Count(respondents, "B01"), 1);
			Assert.AreEqual(counter.Count(respondents, "C01"), 3);
			Assert.AreEqual(counter.Count(respondents, "C02"), 2);
			Assert.AreEqual(counter.Count(respondents, "D01"), 3);
			Assert.AreEqual(counter.Count(respondents, "D02"), 3);
			Assert.AreEqual(counter.Count(respondents, "D03"), 2);
		}
		
		public Respondent[] CreateRespondents()
        {
			HashSet<string> testhashset1 = new HashSet<string>()
										   {
												"A01", "A02", "C01", "D01", "D02"
										   };
			HashSet<string> testhashset2 = new HashSet<string>()
										   {
												"C01", "C02", "D01", "D02", "D03"
										   };
			HashSet<string> testhashset3 = new HashSet<string>()
										   {
												"A01", "A02", "B01", "C01", "C02", "D01", "D02", "D03"
										   };

			Respondent[] respondents = new Respondent[]
									   {
										   new Respondent(testhashset1),
										   new Respondent(testhashset2),
										   new Respondent(testhashset3)
									   };

			return respondents;
		}
	}
}