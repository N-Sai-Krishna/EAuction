using EAuction.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Workflow
{
    public interface IWorkflow
    {
        Task<IWorkflowData> RunAsync(IWorkflowData workflowData);

        int Rank { get; }
        bool IsCompleted { get; }
        bool IsErrored { get; }
        string Key { get; }

        IEnumerable<IWorkflow> DependentWorkFlows { get; }

        IWorkflow SetKey(string key);
        IWorkflow SetDependentWorkflows(IEnumerable<IWorkflow> workflows);

        IWorkflow SetRank(int rank);

    }
}
