using EmployeeManager.API.Models;
using EmployeeManager.API.Models.EmployeeWithDetails;
using EmployeeManager.API.Models.Employee;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Employee.Commands;
using EmployeeManager.Application.UseCases.EmployeeWithDetails.Command;
using EmployeeManager.Application.UseCases.EmployeeWithDetails.Commands;
using EmployeeManager.Application.UseCases.EmployeeWithDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.Controllers
{
    [Route("api/detailed-employees")]
    [ApiController]
    public class DetailedEmployeesController(IMediator _mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateEmpolyeeAsync(
            [FromBody] CreateEmployeeWithDetailsRequest request,
            CancellationToken cancellationToken)
        {
            CreateEmployeeWithDetailsCommand command = new()
            {
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                CompanyId = request.CompanyId,
                Department = new DepartmentDetailsDto(request.Department.Name, request.Department.Phone),
                Passport = new PassportDetailsDto(request.Passport.Type, request.Passport.Number)
            };

            EmployeeWithDetailsDto createdEmployee = await _mediator.Send(command, cancellationToken);

            ApiResponse<CreateEmployeeWithDetailsResponse> response = ApiResponse<CreateEmployeeWithDetailsResponse>.SuccessResponse(
                 new CreateEmployeeWithDetailsResponse(createdEmployee.Id),
                "Employee created successfully");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            IEnumerable<EmployeeWithDetailsDto> allEmployees = await _mediator.Send(new GetAllEmployeesWithDetailsQuery());

            IEnumerable<EmployeeWithDetailsResponse> allEmployeesResponse = allEmployees
                .Select(c => new EmployeeWithDetailsResponse(c.Id, c.Name, c.Surname, c.Phone, c.CompanyId,
                new DepartmentDetail(c.Department.Name, c.Department.Phone),
                new PassportDetail(c.Passport.Type, c.Passport.Number)));

            var response = ApiResponse<IEnumerable<EmployeeWithDetailsResponse>>.SuccessResponse(allEmployeesResponse);
            return Ok(response);
        }

        [HttpGet("company/{companyId:int}")]
        public async Task<IActionResult> GetEmployeesByCompanyAsync(int companyId)
        {
            IEnumerable<EmployeeWithDetailsDto> allEmployees = await _mediator.Send(new GetEmployeeWithDetailsByComapnyIdQuery(companyId));

            IEnumerable<EmployeeWithDetailsResponse> allEmployeesResponse = allEmployees
                .Select(c => new EmployeeWithDetailsResponse(c.Id, c.Name, c.Surname, c.Phone, c.CompanyId,
                new DepartmentDetail(c.Department.Name, c.Department.Phone),
                new PassportDetail(c.Passport.Type, c.Passport.Number)));

            var response = ApiResponse<IEnumerable<EmployeeWithDetailsResponse>>.SuccessResponse(allEmployeesResponse);
            return Ok(response);
        }

        [HttpGet("department/{departmentId:int}")]
        public async Task<IActionResult> GetEmployeesByDepartmentAsync(int departmentId)
        {
            IEnumerable<EmployeeWithDetailsDto> allEmployees = await _mediator.Send(new GetEmployeeWithDetailsByDepartmentIdQuery(departmentId));

            IEnumerable<EmployeeWithDetailsResponse> allEmployeesResponse = allEmployees
                .Select(c => new EmployeeWithDetailsResponse(c.Id, c.Name, c.Surname, c.Phone, c.CompanyId,
                new DepartmentDetail(c.Department.Name, c.Department.Phone),
                new PassportDetail(c.Passport.Type, c.Passport.Number)));

            var response = ApiResponse<IEnumerable<EmployeeWithDetailsResponse>>.SuccessResponse(allEmployeesResponse);
            return Ok(response);
        }


        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken)
        {
            EmployeeWithDetailsDto employeeWithDetailsDto = await _mediator.Send(new GetEmployeeWithDetailsByIdQuery(employeeId), cancellationToken);

            ApiResponse<EmployeeWithDetailsResponse> response = ApiResponse<EmployeeWithDetailsResponse>.SuccessResponse(
                new EmployeeWithDetailsResponse(employeeWithDetailsDto.Id, employeeWithDetailsDto.Name,
                employeeWithDetailsDto.Surname, employeeWithDetailsDto.Phone, employeeWithDetailsDto.CompanyId,
                new DepartmentDetail(employeeWithDetailsDto.Department.Name, employeeWithDetailsDto.Department.Phone),
                new PassportDetail(employeeWithDetailsDto.Passport.Type, employeeWithDetailsDto.Passport.Number)));

            return Ok(response);
        }


        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeAsync(int employeeId, 
            [FromBody] UpdateDetailedEmployeeRequest request,
        CancellationToken cancellationToken)
        {
            UpdateEmployeeWithDetailsCommand command = new()
            {
                Id = employeeId,
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                Department = request.Department != null
                    ? new DepartmentDetailsDto(request.Department.Name, request.Department.Phone)
                    : null,
                Passport = request.Passport != null
                    ? new PassportDetailsDto(request.Passport.Type, request.Passport.Number)
                    : null
            };

            EmployeeWithDetailsDto updatedEmployee = await _mediator.Send(command, cancellationToken);

            EmployeeWithDetailsResponse responseData = new EmployeeWithDetailsResponse(
            Id: updatedEmployee.Id,
            Name: updatedEmployee.Name,
            Surname: updatedEmployee.Surname,
            Phone: updatedEmployee.Phone,
            CompanyId: updatedEmployee.CompanyId,
            Department: new DepartmentDetail(updatedEmployee.Department.Name, updatedEmployee.Department.Phone),
            Passport: updatedEmployee.Passport != null
                ? new PassportDetail(updatedEmployee.Passport.Type, updatedEmployee.Passport.Number)
                : null
            );

            ApiResponse<EmployeeWithDetailsResponse> response = ApiResponse<EmployeeWithDetailsResponse>.SuccessResponse(
                responseData,
               "Employee updated successfully");

            return Ok(response);
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int employeeId)
        {
            EmployeeDto deletedEmployee = await _mediator.Send(new DeleteEmployeeCommand(employeeId));

            ApiResponse<DeleteEmployeeWithDetailsResponse> response = ApiResponse<DeleteEmployeeWithDetailsResponse>.SuccessResponse(
                new DeleteEmployeeWithDetailsResponse(
                    deletedEmployee.Id),
                "Employee deleted successfully");

            return Ok(response);
        }
    }
}
