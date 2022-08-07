using EAuction.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Workflow
{
    internal class WorkflowOrchestrator : IWorkflowOrchestrator
    {
        private IEnumerable<IWorkflow> workflows;

        private readonly IDictionary<string, IWorkflowData> workflowContext;

        public WorkflowOrchestrator()
        {
            this.workflowContext = new Dictionary<string, IWorkflowData>();
        }

        public async Task ExecuteWorkFlowAsync(IWorkflowData initialInput, IEnumerable<IWorkflow> workflows = null)
        {
            this.workflows = workflows;
            var tasks = new List<Task>();
            
            foreach (var workflow in this.workflows)
            {
                tasks.Add(Task.Run(() => workflow.RunAsync(initialInput))
                    .ContinueWith((task) => this.ExecuteWorkFlowAsync(task.Result, workflow.DependentWorkFlows)));
            }

            await Task.WhenAll(tasks);
        }
    }
}
