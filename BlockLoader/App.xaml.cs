using System.Windows;
using BlockLoader.DataLayer;
using BlockLoader.PresentationLayer;

namespace BlockLoader
{
	public partial class App : Application
	{
		private const string BlocksFileName = @"Data\Blocks.xml";
		private const string RespondentsFileName = @"Data\Respondents.xml";

		protected override void OnStartup(StartupEventArgs e)
		{
			//Composition root:
			var blockRepository = new BlockRepository(new XmlLoader(), BlocksFileName);
			var respondentRepository = new RespondentRepository(new XmlLoader(), RespondentsFileName);
			var window = new MainWindowView
			             {
				             DataContext = new MainWindowViewModel(blockRepository, respondentRepository)
			             };
			window.Show();
			base.OnStartup(e);
		}
	}
}