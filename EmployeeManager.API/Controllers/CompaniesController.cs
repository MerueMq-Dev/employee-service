using EmployeeManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.UseCases.Company.Commands;
using EmployeeManager.Application.UseCases.Company.Queries;
using EmployeeManager.API.Models.Company;

namespace EmployeeManager.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController(IMediator _mediator) : ControllerBase
    {

        /// <summary>
        /// Получить список всех компаний
        /// GET: api/companies
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CompanyResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanies()
        {
            IEnumerable<CompanyDto> allCompanies = await _mediator.Send(new GetAllCompaniesQuery());

            IEnumerable<CompanyResponse> allCompaniesResponse = allCompanies
                .Select(c => new CompanyResponse(c.Id, c.Name, c.Address));

            var response = ApiResponse<IEnumerable<CompanyResponse>>.SuccessResponse(allCompaniesResponse);
            return Ok(response);
        }

        /// <summary>
        /// Получить компанию по ID
        /// GET: api/companies/{companyId}
        /// </summary>
        //[ProducesResponseType(typeof(ApiResponse<CompanyResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetCompany(int companyId, CancellationToken cancellationToken)
        {
            CompanyDto companyDto = await _mediator.Send(new GetCompanyByIdQuery(companyId), cancellationToken);

            ApiResponse<CompanyResponse> response = ApiResponse<CompanyResponse>.SuccessResponse(
                new CompanyResponse(companyDto.Id, companyDto.Name, companyDto.Address));
            
            return Ok(response);
        }

        /// <summary>
        /// Создать новую компанию
        /// POST: api/companies
        /// </summary>
        //[ProducesResponseType(typeof(ApiResponse<CreateCompanyResponse>), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRequest request,
            CancellationToken cancellationToken)
        {

            CreateCompanyCommand command = new CreateCompanyCommand
            {
                Name = request.Name,
                Address = request.Address
            };
            CompanyDto createdCompany = await _mediator.Send(command, cancellationToken);

            ApiResponse<CreateCompanyResponse> response = ApiResponse<CreateCompanyResponse>.SuccessResponse(
                new CreateCompanyResponse(
                    createdCompany.Id, 
                    createdCompany.Address,
                    createdCompany.Name),
                "Company created successfully");

            return Ok(response);
        }

        /// <summary>
        /// Обновить компанию
        /// PUT: api/companies/{companyId}
        ///// </summary>
        //[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPut("{companyId}")]
        public async Task<IActionResult> UpdateCompanyAsync(int companyId, [FromBody] UpdateCompanyRequest request)
        {
            CompanyDto updatedCompanyDto =  await _mediator.Send(
                new UpdateCompanyCommand(companyId, request.Name, request.Address));

            UpdateCompanyResponse updateCompanyResponse = new UpdateCompanyResponse(updatedCompanyDto.Id,
                updatedCompanyDto.Name, updatedCompanyDto.Address);
            ApiResponse<UpdateCompanyResponse> response = ApiResponse<UpdateCompanyResponse>
                .SuccessResponse(updateCompanyResponse, "Company updated successfully");
            return Ok(response);
        }

        /// <summary>
        /// Удалить компанию
        /// DELETE: api/companies/{companyId}
        /// </summary>
        //[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpDelete("{companyId}")]
        public async Task<IActionResult> DeleteCompany(int companyId)
        {
            CompanyDto deletedCompany = await _mediator.Send(new DeleteCompanyCommand(companyId));

            ApiResponse<DeleteComapanyResponse> response = ApiResponse<DeleteComapanyResponse>.SuccessResponse(
                new DeleteComapanyResponse(
                    deletedCompany.Id,
                    deletedCompany.Name,
                    deletedCompany.Address),
                "Company deleted successfully");

            return Ok(response);
        }
    }
}
