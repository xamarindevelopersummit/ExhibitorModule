using System;
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
            Title = "Notes";
            _leadsService = leadsService;
            SaveCommand = new DelegateCommand(OnSaveTapped);
        }

        private async void OnSaveTapped()
        {
            await _leadsService.AddUpdateLead(CurrentLead);
            RequestClose?.Invoke(new DialogParameters { { "Lead", CurrentLead } });
        }

        private Lead _currentLead;
        public Lead CurrentLead { get => _currentLead; private set { SetProperty(ref _currentLead, value); } }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Lead"))
                CurrentLead = parameters.GetValue<Lead>("Lead");
            else
            {
                PageDialogService.DisplayAlertAsync("Error", "Could not load a lead. Please try again.", "OK");
                RequestClose?.Invoke(null);
            }
        }

        public DelegateCommand SaveCommand { get; }
    }
}
