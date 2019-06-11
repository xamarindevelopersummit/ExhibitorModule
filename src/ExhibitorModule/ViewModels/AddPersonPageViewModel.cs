using System;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using Prism.Commands;

namespace ExhibitorModule.ViewModels
{
    public class AddPersonPageViewModel : ViewModelBase
    {
        private readonly ILeadsService _leadService;

        public AddPersonPageViewModel(IBase @base, ILeadsService leadService) : base(@base)
        {
            Title = "NEW LEAD";
            _leadService = leadService;
            SaveCommand = new DelegateCommand(OnSaveTapped);
        }

        private async void OnSaveTapped()
        {
            await _leadService.AddUpdateLead(Current);
            await NavigationService.GoBackAsync();
        }

        public Lead Current { get; set; } = new Lead { VisitedAt = DateTime.Now };

        public DelegateCommand SaveCommand { get; set; }
    }
}
