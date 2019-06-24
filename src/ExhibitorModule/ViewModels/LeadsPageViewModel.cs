using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ShowNotes = new DelegateCommand<LeadItem>(OnNotesTapped);
        }

        private void OnNotesTapped(LeadItem lead)
        {
            _dialogService.ShowDialog(Dialogs.Notes, new DialogParameters {
                    { "Lead", lead },
                    { KnownDialogParameters.CloseOnBackgroundTapped, true }
                }, HandleAction);
        }

        void HandleAction(IDialogResult result)
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

        private ObservableRangeCollection<LeadItem> _leads = new ObservableRangeCollection<LeadItem>();
        public ObservableRangeCollection<LeadItem> Leads
        {
            get => _leads;
            set { SetProperty(ref _leads, value); }
        }

        public DelegateCommand LoadLeadsCommand { get; set; }
        public DelegateCommand<LeadItem> ShowNotes { get; set; }

        private async void OnLoadLeadsCommandsTapped()
        {
            var results = await RunTask(GetLeads());
            LoadLeads(results);
        }

        private void LoadLeads(List<LeadItem> leads)
        {
            if (!(leads?.Any() ?? false)) return;

            Leads.ReplaceRange(leads);
        }

        private Task<List<LeadItem>> GetLeads(string query = null)
        {
            if (string.IsNullOrWhiteSpace(query))
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
            _leadsService.GetAttendees();
        }
    }
}
