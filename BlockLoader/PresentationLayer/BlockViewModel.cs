﻿using BlockLoader.Utils;

namespace BlockLoader.PresentationLayer
{
	public class BlockViewModel : NotifyPropertyChangedBase
	{
		public BlockViewModel(string code, int footage, string program, int respondentCount)
		{
			Code = code;
			Footage = footage;
			Program = program;
			RespondentCount = respondentCount;
		}
		private int _respondentcount;
		public string Code { get; }
		public int Footage { get; }
		public string Program { get; }

		public int RespondentCount
		{
			get { return _respondentcount; }
			set
			{
				if (value == RespondentCount)
				{
					return;
				}

				_respondentcount = value;
				NotifyPropertyChanged(() => RespondentCount);
			}
		}
	}
}