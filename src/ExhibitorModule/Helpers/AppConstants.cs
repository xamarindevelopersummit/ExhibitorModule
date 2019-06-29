using System.Threading;
using System.Threading.Tasks;
using ExhibitorModule.Services.Abstractions;
using Shiny.Jobs;

namespace ExhibitorModule.Common
{
    public static class AppConstants
    {
        public const int DefaultGetRetryCount = 3;
        public const string ContentTypeJson = "application/json";
        public static string LeadKey = "lead_key";
    }

    public class ExhibitorSyncJob : IJob
    {
        ILeadsService LeadService { get; }

        public ExhibitorSyncJob(ILeadsService leadService)
        {
            LeadService = leadService;
        }

        public async Task<bool> Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            await LeadService.GetAttendees();
            await LeadService.GetLeads();
            return true;
        }
    }
}
