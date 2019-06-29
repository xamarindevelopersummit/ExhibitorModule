using System;
using ExhibitorModule.Common;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ExhibitorModule.ViewModels
{
    public class NotesDialogViewModel : ViewModelBase, IDialogAware
    {
        private readonly ILeadsService _leadsService;

        public NotesDialogViewModel(IBase @base, ILeadsService leadsService) : base(@base)
        {
            _leadsService = leadsService;
            SaveCommand = new DelegateCommand(OnSaveTapped);
        }

        private async void OnSaveTapped()
        {
            await _leadsService.AddUpdateLead(CurrentLead);
            RequestClose?.Invoke(new DialogParameters { { AppConstants.LeadKey, CurrentLead } });
        }

        private LeadContactInfo _currentLead;
        public LeadContactInfo CurrentLead { get => _currentLead; private set { SetProperty(ref _currentLead, value); } }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (!parameters.ContainsKey("Lead"))
            {
                PageDialogService.DisplayAlertAsync("Error", "Could not load a lead. Please try again.", "OK");
                RequestClose?.Invoke(null);
            }

            CurrentLead = parameters.GetValue<LeadContactInfo>(AppConstants.LeadKey);
            Title = $"Notes for {CurrentLead.FirstName}";
        }

        public DelegateCommand SaveCommand { get; }
    }
}
