using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExhibitorModule.Common;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using MvvmHelpers;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace ExhibitorModule.ViewModels
{
    class LeadsPageViewModel : ViewModelBase
    {
        private readonly ILeadsService _leadsService;
        private readonly IDialogService _dialogService;

        public LeadsPageViewModel(IBase @base, ILeadsService leadsService, IDialogService dialogService)
            : base(@base)
        {
            Title = Strings.Resources.LeadsPageTitle;
            _leadsService = leadsService;
            _dialogService = dialogService;
            LoadLeadsCommand = new DelegateCommand(OnLoadLeadsCommandsTapped);
            ShowNotes = new DelegateCommand<LeadContactInfo>(OnNotesTapped);
        }

        private void OnNotesTapped(LeadContactInfo lead)
        {
            _dialogService.ShowDialog(Dialogs.Notes, new DialogParameters {
                    { AppConstants.LeadKey, lead },
                    { KnownDialogParameters.CloseOnBackgroundTapped, true }
                }, OnNotesDialogClosed);
        }

        void OnNotesDialogClosed(IDialogResult result)
        {
            if (!result.Parameters.Any())
                return;

            foreach (var key in result.Parameters.Keys)
            {
                Console.WriteLine($"Key:{key}, Value:{result.Parameters.GetValue<object>(key).ToString()}");
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                Search(value);
            }
        }

        private ObservableRangeCollection<LeadContactInfo> _leads = new ObservableRangeCollection<LeadContactInfo>();
        public ObservableRangeCollection<LeadContactInfo> Leads
        {
            get => _leads;
            set { SetProperty(ref _leads, value); }
        }

        public DelegateCommand LoadLeadsCommand { get; set; }
        public DelegateCommand<LeadContactInfo> ShowNotes { get; set; }

        private async void OnLoadLeadsCommandsTapped()
        {
            var results = await RunTask(GetLeads());
            LoadLeads(results);
        }

        private void LoadLeads(List<LeadContactInfo> leads)
        {
            if (!(leads?.Any() ?? false)) return;

            Leads.ReplaceRange(leads);
        }

        private Task<List<LeadContactInfo>> GetLeads(string query = null)
        {
            if (!string.IsNullOrWhiteSpace(query))
                return _leadsService.LookupLead(query);

            return _leadsService.GetLeads();
        }

        private async void Search(string query)
        {
            var results = await RunTask(GetLeads(query));
            LoadLeads(results);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadLeadsCommand?.Execute();

            if(parameters.GetNavigationMode() != NavigationMode.Back)
                _leadsService.GetAttendees();
        }
    }
}
