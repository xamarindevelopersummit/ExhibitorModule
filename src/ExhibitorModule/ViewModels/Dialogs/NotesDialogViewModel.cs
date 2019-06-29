using System;
using ExhibitorModule.Common;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using Prism.Services.Dialogs;

namespace ExhibitorModule.ViewModels
{
    public class NotesDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ILeadsService _leadsService;
        private readonly IPageDialogService _pageDialogService;

        public NotesDialogViewModel(ILeadsService leadsService, IPageDialogService pageDialogService)
        {
            _leadsService = leadsService;
            _pageDialogService = pageDialogService;
            SaveCommand = new DelegateCommand(OnSaveTapped);
        }

        private async void OnSaveTapped()
        {
            await _leadsService.AddUpdateLead(CurrentLead);
            RequestClose?.Invoke(new DialogParameters { { AppConstants.LeadKey, CurrentLead } });
        }

        private string _title;
        public string Title { get => _title; set { SetProperty(ref _title, value); } }

        private LeadContactInfo _currentLead;

        public LeadContactInfo CurrentLead { get => _currentLead; private set { SetProperty(ref _currentLead, value); } }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set
            {
                SetProperty(ref _notes, value);
                CurrentLead.Notes = value;
            }
        }
        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (!parameters.ContainsKey(AppConstants.LeadKey))
            {
                _pageDialogService.DisplayAlertAsync("Error", "Could not load a lead. Please try again.", "OK");
                RequestClose?.Invoke(null);
            }

            CurrentLead = parameters.GetValue<LeadContactInfo>(AppConstants.LeadKey);
            Title = $"Notes for {CurrentLead.FirstName}";
            Notes = CurrentLead.Notes;
        }

        public DelegateCommand SaveCommand { get; }
    }
}
