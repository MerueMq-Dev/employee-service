using EmployeeManager.API.Models;
using EmployeeManager.API.Models.Employee;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Employee.Commands;
using EmployeeManager.Application.Employee.Queries;
using EmployeeManager.Application.UseCases.Employee.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController(IMediator _mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateEmpolyeeAsync(
            [FromBody] CreateEmployeeRequest request,
            CancellationToken cancellationToken)
        {

            CreateEmployeeCommand command = new()
            {
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                DepartmentId = request.DepartmentId,
                PassportId = request.PassportId,
            };

            EmployeeDto createdEmployee = await _mediator.Send(command, cancellationToken);

            ApiResponse<CreateEmployeeResponse> response = ApiResponse<CreateEmployeeResponse>.SuccessResponse(
                new CreateEmployeeResponse(
                    createdEmployee.Id,
                    createdEmployee.Name,
                    createdEmployee.Surname,
                    createdEmployee.Phone,
                    createdEmployee.DepartmentId,
                    createdEmployee.PassportId),
                "Employee created successfully");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            IEnumerable<EmployeeDto> allEmployees = await _mediator.Send(new GetAllEmployeesQuery());

            IEnumerable<EmployeeResponse> allEmployeesResponse = allEmployees
                .Select(c => new EmployeeResponse(c.Id, c.Name, c.Surname, c.Phone, c.DepartmentId, c.PassportId));

            var response = ApiResponse<IEnumerable<EmployeeResponse>>.SuccessResponse(allEmployeesResponse);
            return Ok(response);
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken)
        {
            EmployeeDto employeeDto = await _mediator.Send(new GetEmployeeByIdQuery(employeeId), cancellationToken);

            ApiResponse<EmployeeResponse> response = ApiResponse<EmployeeResponse>.SuccessResponse(
                new EmployeeResponse(employeeDto.Id, employeeDto.Name,
                employeeDto.Surname,
                employeeDto.Phone, employeeDto.DepartmentId, employeeDto.PassportId));

            return Ok(response);
        }


        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateDeparmentAsync(int employeeId, [FromBody] UpdateEmployeeRequest request)
        {
            EmployeeDto updatedEmployeeDto = await _mediator.Send(
                new UpdateEmployeeCommand(employeeId, request.Name,request.Surname, request.Phone, 
                request.DepartmentId, request.PassportId));

            UpdateEmployeeResponse updateEmployeeIdResponse = new UpdateEmployeeResponse(
                updatedEmployeeDto.Id, updatedEmployeeDto.Name, updatedEmployeeDto.Surname,
                updatedEmployeeDto.Phone, updatedEmployeeDto.DepartmentId, updatedEmployeeDto.PassportId);

            ApiResponse<UpdateEmployeeResponse> response = ApiResponse<UpdateEmployeeResponse>
                .SuccessResponse(updateEmployeeIdResponse, "Employee updated successfully");

            return Ok(response);
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int employeeId)
        {
            EmployeeDto deletedEmployee = await _mediator.Send(new DeleteEmployeeCommand(employeeId));

            ApiResponse<DeleteEmployeeResponse> response = ApiResponse<DeleteEmployeeResponse>.SuccessResponse(
                new DeleteEmployeeResponse(
                    deletedEmployee.Id,
                    deletedEmployee.Name,
                    deletedEmployee.Surname,
                    deletedEmployee.Phone,
                    deletedEmployee.DepartmentId,
                    deletedEmployee.PassportId
                    ),
                "Employee deleted successfully");

            return Ok(response);
        }
    }
}
