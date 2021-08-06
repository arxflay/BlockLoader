﻿using System;
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

		public MainWindowViewModel(IBlockRepository blockRepository, IRespondentRepository respondentRepository)
		{
			_blockRepository = blockRepository;
			_respondentRepository = respondentRepository;
			IsGridVisible = false;
			Blocks = new ObservableCollection<BlockViewModel>();
			LoadBlocksCommand = new AsyncDelegateCommand(LoadBlocks);
		}

		public ObservableCollection<BlockViewModel> Blocks { get; }

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

		public ICommand LoadBlocksCommand { get; }
		
		public async Task LoadBlocks()
		{
			IsBusy = true;
			IsGridVisible = false;

			try
			{
				ReachedBlockCounter counter = new ReachedBlockCounter();
				var respondents = _respondentRepository.LoadRespondents();
				var blocks = await Task.Run(() => _blockRepository.LoadBlocks());
				Blocks.Clear();
				foreach (var block in blocks)
				{
					Blocks.Add(CreateBlockViewModel(block, counter.Count(respondents, block.Code)));
				}

				IsGridVisible = true;
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

		private static BlockViewModel CreateBlockViewModel(Block block, int respondentCount)
		{
			return new BlockViewModel(block.Code, block.Footage, block.Program, respondentCount);
		}
	}
}