using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BlockLoader.DataLayer;
using BlockLoader.Properties;
using BlockLoader.Utils;
using BlockLoader.Business;
using System.Collections.Generic;

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
		private bool _isLoadBlocksEnabled;

		public MainWindowViewModel(IBlockRepository blockRepository, IRespondentRepository respondentRepository)
		{
			_blockRepository = blockRepository;
			_respondentRepository = respondentRepository;
			IsGridVisible = false;
			IsCountButtonEnabled = false;
			IsCountRowVisible = false;
			IsLoadBlocksEnabled = true;
			IsCountButtonEnabled = false;
			Blocks = new ObservableCollection<BlockViewModel>();
			Respondents = new List<Respondent>();
			LoadBlocksCommand = new AsyncDelegateCommand(LoadBlocksAsync);
			CountRespondentsCommand = new AsyncDelegateCommand(CountRespondentsAsync);
		}

		public ObservableCollection<BlockViewModel> Blocks { get; }
		public List<Respondent> Respondents { get; }

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

		public bool IsLoadBlocksEnabled
		{
			get { return _isLoadBlocksEnabled; }
			set
			{
				if (value == _isLoadBlocksEnabled)
				{
					return;
				}

				_isLoadBlocksEnabled = value;
				NotifyPropertyChanged(() => IsLoadBlocksEnabled);
			}
		}

		public ICommand LoadBlocksCommand { get; }
		public ICommand CountRespondentsCommand { get; }

		public async Task LoadBlocksAsync()
		{
			IsBusy = true;
			IsGridVisible = false;

			if(IsCountRowVisible)
				IsCountRowVisible = false;

			if (IsCountButtonEnabled)
				IsCountButtonEnabled = false;

			try
			{		
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
		public async Task LoadRespondentsAsync()
        {
			IsBusy = true;
			IsGridVisible = false;
			try
			{
				var respondents = await Task.Run(() => _respondentRepository.LoadRespondents());

				foreach (var respondent in respondents)
                {
					Respondents.Add(respondent);
                }

				IsGridVisible = true;
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
		public async Task CountRespondentsAsync()
        {
			IsBusy = true;
			IsLoadBlocksEnabled = false;
			try
			{
				if (Respondents.Count != 0)
					Respondents.Clear();
				
				await LoadRespondentsAsync();

				ReachedBlockCounter counter = new ReachedBlockCounter();

				for (int i = 0; i < Blocks.Count; i++)
				{
					var block = Blocks[i];
					int count = counter.Count(Respondents, block.Code);
					Blocks[i] = new BlockViewModel(block.Code, block.Footage, block.Program, count);
				}

				IsCountRowVisible = true;
				IsLoadBlocksEnabled = true;
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


		private static BlockViewModel CreateBlockViewModel(Block block)
		{
			return new BlockViewModel(block.Code, block.Footage, block.Program);
		}
	}
}
