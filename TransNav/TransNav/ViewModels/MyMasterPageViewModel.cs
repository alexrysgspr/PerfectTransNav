using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TransNav.ViewModels
{
	public class MyMasterPageViewModel : ViewModelBase
	{
        public MyMasterPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
