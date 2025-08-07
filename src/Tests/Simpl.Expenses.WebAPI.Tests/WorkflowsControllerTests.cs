using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using System.Collections.Generic;
using Core.WebApi;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class WorkflowsControllerTests : IntegrationTestBase
    {
        public WorkflowsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetWorkflowById_WhenWorkflowExists_ReturnsOk()
        {
            // Arrange
            var workflow = new Workflow
            {
                Name = "Test Workflow",
                Description = "Test Description"
            };
            var addedWorkflow = await AddAsync(workflow);
            //insert a step and relate it to the workflow
            var workflowStep = new WorkflowStep
            {
                WorkflowId = addedWorkflow.Id,
                StepNumber = 1,
                Name = "Step 1",
                ApproverRoleId = 1
            };
            await AddAsync(workflowStep);

            // Act
            var response = await _client.GetAsync($"/api/workflows/{addedWorkflow.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var workflowDto = await response.Content.ReadFromJsonAsync<WorkflowDto>();
            Assert.NotNull(workflowDto);
            Assert.Equal(workflow.Id, workflowDto.Id);
            Assert.NotNull(workflowDto.Steps);
            Assert.Single(workflowDto.Steps);
        }

        [Fact]
        public async Task GetAllWorkflows_And_ItsSteps_ReturnsOk()
        {
            // Arrange
            var workflow = new Workflow
            {
                Name = "Test Workflow",
                Description = "Test Description"
            };
            await AddAsync(workflow);
            var workflowStep = new WorkflowStep
            {
                WorkflowId = workflow.Id,
                StepNumber = 1,
                Name = "Step 1",
                ApproverRoleId = 1
            };
            await AddAsync(workflowStep);

            // Act
            var response = await _client.GetAsync("/api/workflows");
            var workflowsDto = await response.Content.ReadFromJsonAsync<List<WorkflowDto>>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(workflowsDto);
            Assert.NotNull(workflowsDto[0].Steps);
            Assert.Single(workflowsDto[0].Steps);
        }

        [Fact]
        public async Task CreateWorkflow_WithValidData_CreatesWorkflow()
        {
            // Arrange
            var createWorkflowDto = new CreateWorkflowDto { Name = "New Workflow", Description = "New Description" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/workflows", createWorkflowDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var workflowDto = await response.Content.ReadFromJsonAsync<WorkflowDto>();
            Assert.NotNull(workflowDto);
            Assert.Equal(createWorkflowDto.Name, workflowDto.Name);
        }

        [Fact]
        public async Task UpdateWorkflow_WithValidData_UpdatesWorkflow()
        {
            // Arrange
            var workflow = new Workflow { Name = "Original Workflow", Description = "Original Description" };
            await AddAsync(workflow);
            var updateWorkflowDto = new UpdateWorkflowDto { Name = "Updated Workflow", Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/workflows/{workflow.Id}", updateWorkflowDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteWorkflow_WhenWorkflowExists_DeletesWorkflow()
        {
            // Arrange
            var workflow = new Workflow { Name = "Delete Workflow", Description = "Delete Description" };
            await AddAsync(workflow);

            // Act
            var response = await _client.DeleteAsync($"/api/workflows/{workflow.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
