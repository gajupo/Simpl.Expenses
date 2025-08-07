using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class WorkflowStepsControllerTests : IntegrationTestBase
    {
        public WorkflowStepsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetWorkflowStepById_WhenWorkflowStepExists_ReturnsOk()
        {
            // Arrange
            var workflow = await AddAsync(new Workflow { Name = "Test Workflow", Description = "Test Description" });
            var workflowStep = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, StepNumber = 1, Name = "Step 1", ApproverRoleId = 1 });

            // Act
            var response = await _client.GetAsync($"/api/workflows/{workflow.Id}/steps/{workflowStep.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var workflowStepDto = await response.Content.ReadFromJsonAsync<WorkflowStepDto>();
            Assert.NotNull(workflowStepDto);
            Assert.Equal(workflowStep.Id, workflowStepDto.Id);
        }

        [Fact]
        public async Task CreateWorkflowStep_WithValidData_CreatesWorkflowStep()
        {
            // Arrange
            var workflow = await AddAsync(new Workflow { Name = "Test Workflow", Description = "Test Description" });
            var createWorkflowStepDto = new CreateWorkflowStepDto { WorkflowId = workflow.Id, StepNumber = 1, Name = "New Step", ApproverRoleId = 1 };

            // Act
            var response = await _client.PostAsJsonAsync($"/api/workflows/{workflow.Id}/steps", createWorkflowStepDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var workflowStepDto = await response.Content.ReadFromJsonAsync<WorkflowStepDto>();
            Assert.NotNull(workflowStepDto);
            Assert.Equal(createWorkflowStepDto.Name, workflowStepDto.Name);
        }

        [Fact]
        public async Task UpdateWorkflowStep_WithValidData_UpdatesWorkflowStep()
        {
            // Arrange
            var workflow = await AddAsync(new Workflow { Name = "Test Workflow", Description = "Test Description" });
            var workflowStep = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, StepNumber = 1, Name = "Original Step", ApproverRoleId = 1 });
            var updateWorkflowStepDto = new UpdateWorkflowStepDto { StepNumber = 2, Name = "Updated Step", ApproverRoleId = 1 };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/workflows/{workflow.Id}/steps/{workflowStep.Id}", updateWorkflowStepDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteWorkflowStep_WhenWorkflowStepExists_DeletesWorkflowStep()
        {
            // Arrange
            var workflow = await AddAsync(new Workflow { Name = "Test Workflow", Description = "Test Description" });
            var workflowStep = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, StepNumber = 1, Name = "Delete Step", ApproverRoleId = 1 });

            // Act
            var response = await _client.DeleteAsync($"/api/workflows/{workflow.Id}/steps/{workflowStep.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetCurrentStep_WhenStepExists_ReturnsOk()
        {
            // Arrange
            var workflow = await AddAsync(new Workflow { Name = "Test Workflow", Description = "Test Description" });
            var workflowStep = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, StepNumber = 1, Name = "Step 1", ApproverRoleId = 1 });

            // Act
            var response = await _client.GetAsync($"/api/workflows/{workflow.Id}/steps/current/{workflowStep.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var workflowStepDto = await response.Content.ReadFromJsonAsync<WorkflowStepDto>();
            Assert.NotNull(workflowStepDto);
            Assert.Equal(workflowStep.Id, workflowStepDto.Id);
        }

        [Fact]
        public async Task GetNextStep_WhenNextStepExists_ReturnsOk()
        {
            // Arrange
            var workflow = await AddAsync(new Workflow { Name = "Test Workflow", Description = "Test Description" });
            var workflowStep1 = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, StepNumber = 1, Name = "Step 1", ApproverRoleId = 1 });
            var workflowStep2 = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, StepNumber = 2, Name = "Step 2", ApproverRoleId = 1 });

            // Act
            var response = await _client.GetAsync($"/api/workflows/{workflow.Id}/steps/next/{workflowStep1.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var workflowStepDto = await response.Content.ReadFromJsonAsync<WorkflowStepDto>();
            Assert.NotNull(workflowStepDto);
            Assert.Equal(workflowStep2.Id, workflowStepDto.Id);
        }
    }
}
