using System;
using ExhibitorModule.Common;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using ExhibitorModule.Helpers;
using ExhibitorModule.Views;
using ExhibitorModule.Services.Abstractions;
using System.Collections.ObjectModel;
using ExhibitorModule.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Services.Dialogs;

namespace ExhibitorModule.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ILeadsService _leadsService;
        private readonly IDialogService _dialogService;

        public MainPageViewModel(IBase @base, ILeadsService leadsService, IDialogService dialogService)
            : base(@base)
        {
            Title = Strings.Resources.MainPageTitle;
            _leadsService = leadsService;
            _dialogService = dialogService;
            LoadLeadsCommand = new DelegateCommand(OnLoadLeadsCommandsTapped);
            ShowNotes = new DelegateCommand<Lead>(OnNotesTapped);
        }

        private void OnNotesTapped(Lead lead)
        {
            _dialogService.ShowDialog(nameof(NotesDialog), new DialogParameters { { "Lead", lead } }, HandleAction);
        }

        void HandleAction(IDialogResult obj)
        {
            if (!obj.Parameters.Any())
                return;
            foreach (var key in obj.Parameters.Keys)
            {
                Console.WriteLine($"Key:{key}, Value:{obj.Parameters.GetValue<object>(key).ToString()}");
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

        private ObservableCollection<Lead> _leads = new ObservableCollection<Lead>();
        public ObservableCollection<Lead> Leads
        {
            get => _leads;
            set { SetProperty(ref _leads, value); }
        }

        public DelegateCommand LoadLeadsCommand { get; set; }
        public DelegateCommand<Lead> ShowNotes { get; set; }

        private async void OnLoadLeadsCommandsTapped()
        {
            Leads.Clear();
            var results = await RunTask(GetLeads());
            foreach (var lead in results)
            {
                Leads.Add(lead);
            }
        }

        private Task<List<Lead>> GetLeads(string query = null) => _leadsService.GetLeads(query);

        private async void Search(string query)
        {
            Leads.Clear();
            var results = await RunTask(GetLeads(query));
            foreach (var lead in results)
            {
                Leads.Add(lead);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            LoadLeadsCommand?.Execute();
        }
    }
}