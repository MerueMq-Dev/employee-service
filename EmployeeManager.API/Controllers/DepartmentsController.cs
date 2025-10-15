using EmployeeManager.API.Models;
using EmployeeManager.API.Models.Department;
using EmployeeManager.Application.Company.Queries;
using EmployeeManager.Application.Department.Commands;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.UseCases.Department.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController(IMediator _mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateDepartmentAsync(
            [FromBody] CreateDepartmentRequest request,
            CancellationToken cancellationToken)
        {

            CreateDepartmentCommand command = new ()
            {
                Name = request.Name,
                Phone = request.Phone,
                CompanyId = request.CompanyId
            };

            DepartmentDto createdDepartment = await _mediator.Send(command, cancellationToken);

            ApiResponse<CreateDepartmentResponse> response = ApiResponse<CreateDepartmentResponse>.SuccessResponse(
                new CreateDepartmentResponse(
                    createdDepartment.Id,
                    createdDepartment.Name,
                    createdDepartment.Phone,
                    createdDepartment.CompanyId),
                "Department created successfully");

            return Ok(response);
        }
       
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            IEnumerable<DepartmentDto> allDepartments = await _mediator.Send(new GetAllDepartmenstQuery());

            IEnumerable<DepartmentResponse> allDepartmentsResponse = allDepartments
                .Select(c => new DepartmentResponse(c.Id, c.Name, c.Phone, c.CompanyId));

            var response = ApiResponse<IEnumerable<DepartmentResponse>>.SuccessResponse(allDepartmentsResponse);
            return Ok(response);
        }
        
        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetDepartmentAsync(int departmentId, CancellationToken cancellationToken)
        {
            DepartmentDto departmentDto = await _mediator.Send(new GetDepartmentByIdQuery(departmentId), cancellationToken);

            ApiResponse<DepartmentResponse> response = ApiResponse<DepartmentResponse>.SuccessResponse(
                new DepartmentResponse(departmentDto.Id, departmentDto.Name,
                departmentDto.Phone, departmentDto.CompanyId));

            return Ok(response);
        }

        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDeparmentAsync(int departmentId, [FromBody] UpdateDepartmentRequest request)
        {
            DepartmentDto updatedDepartmentDto = await _mediator.Send(
                new UpdateDepartmentCommand(departmentId, request.Name, request.Phone, request.CompanyId));

            UpdateDepartmentResponse updateDepartmentIdResponse = new UpdateDepartmentResponse(
                updatedDepartmentDto.Id,
                updatedDepartmentDto.Name, updatedDepartmentDto.Phone, updatedDepartmentDto.CompanyId);
        
            ApiResponse<UpdateDepartmentResponse> response = ApiResponse<UpdateDepartmentResponse>
                .SuccessResponse(updateDepartmentIdResponse, "Department updated successfully");

            return Ok(response);
        }

        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> DeleteDepartmentAsync(int departmentId)
        {
            DepartmentDto deletedDepartment = await _mediator.Send(new DeleteDepartmentCommand(departmentId));

            ApiResponse<DeleteDepartmentResponse> response = ApiResponse<DeleteDepartmentResponse>.SuccessResponse(
                new DeleteDepartmentResponse(
                    deletedDepartment.Id,
                    deletedDepartment.Name,
                    deletedDepartment.Phone,
                    deletedDepartment.CompanyId
                    ),
                "Department deleted successfully");

            return Ok(response);
        }
    }
}
