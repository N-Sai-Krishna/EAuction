using EAuction.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Workflow
{
    public interface IWorkflowOrchestrator
    {
        Task ExecuteWorkFlowAsync(IWorkflowData initialInput, IEnumerable<IWorkflow> workflows = null);

    }
}
