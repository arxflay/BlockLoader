using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BlockLoader.DataLayer;
using BlockLoader.Properties;
using BlockLoader.Utils;

namespace BlockLoader.PresentationLayer
{
	public class MainWindowViewModel : NotifyPropertyChangedBase
	{
		private readonly IBlockRepository _blockRepository;
		private readonly IRespondentRepository _respondentRepository;
		private bool _isBusy;
		private bool _isGridVisible;
		private bool _isCountButtonEnabled;
		private bool _isCountRowVisible;

		public MainWindowViewModel(IBlockRepository blockRepository, IRespondentRepository respondentRepository)
		{
			_blockRepository = blockRepository;
			_respondentRepository = respondentRepository;
			IsGridVisible = false;
			IsCountButtonEnabled = false;
			IsCountRowVisible = false;
			Blocks = new ObservableCollection<BlockViewModel>();
			Respondents = new ObservableCollection<Respondent>();
			LoadBlocksCommand = new AsyncDelegateCommand(LoadBlocks);
			CountRespondentsCommand = new AsyncDelegateCommand(CountRespondents);
		}

		public ObservableCollection<BlockViewModel> Blocks { get; }
		public ObservableCollection<Respondent> Respondents { get; }
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				if (value == _isBusy)
				{
					return;
				}

				_isBusy = value;
				NotifyPropertyChanged(() => IsBusy);
			}
		}

		public bool IsGridVisible
		{
			get { return _isGridVisible; }
			set
			{
				if (value == _isGridVisible)
				{
					return;
				}

				_isGridVisible = value;
				NotifyPropertyChanged(() => IsGridVisible);
			}
		}

		public bool IsCountButtonEnabled
		{
			get { return _isCountButtonEnabled; }
			set
            {
				if (value == _isCountButtonEnabled)
                {
					return;
                }

				_isCountButtonEnabled = value;
				NotifyPropertyChanged(() => IsCountButtonEnabled);
            }
		}

		public bool IsCountRowVisible
        {
			get { return _isCountRowVisible; }
			set
			{
				if (value == _isCountRowVisible)
				{
					return;
				}

				_isCountRowVisible = value;
				NotifyPropertyChanged(() => IsCountRowVisible);
			}
		}

		public ICommand LoadBlocksCommand { get; }
		public ICommand CountRespondentsCommand { get; }

		public async Task LoadBlocks()
		{
			IsBusy = true;
			IsGridVisible = false;

			try
			{
				await LoadRespondents();
				var blocks = await Task.Run(() => _blockRepository.LoadBlocks());
				Blocks.Clear();

				foreach (var block in blocks)
				{
					Blocks.Add(CreateBlockViewModel(block));
				}

				IsGridVisible = true;
				IsCountButtonEnabled = true;
			}
			catch (Exception)
			{
				MessageBox.Show(Resources.ErrorLoadingBlocks, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				Blocks.Clear();
			}
			finally
			{
				IsBusy = false;
			}
		}
		public async Task LoadRespondents()
        {
			IsBusy = true;
			try
			{
				var respondents = await Task.Run(() => _respondentRepository.LoadRespondents());

				foreach(var respondent in respondents)
                {
					Respondents.Add(respondent);
                }
			}
			catch (Exception)
            {
				MessageBox.Show(Resources.ErrorLoadingRespondents, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				Respondents.Clear();
			}
			finally
            {
				IsBusy = false;
            }
		}
		public async Task CountRespondents()
        {
			IsBusy = true;
			try
			{
				ReachedBlockCounter counter = new ReachedBlockCounter();
				await Task.Run(() =>
				{
					foreach (var block in Blocks)
					{
						block.RespondentCount = counter.Count(Respondents, block.Code);
					}
				});
				IsCountRowVisible = true;
			}
			catch (Exception)
            {
				MessageBox.Show(Resources.ErrorLoadingRespondents, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
			finally
            {
				IsBusy = false;
            }
		}


		private static BlockViewModel CreateBlockViewModel(Block block, int respondentCount = 0)
		{
			return new BlockViewModel(block.Code, block.Footage, block.Program, respondentCount);
		}
	}
}