using EmployeeManager.API.Models;
using EmployeeManager.API.Models.Passport;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Passport.Commands;
using EmployeeManager.Application.UseCases.Passport.Commands;
using EmployeeManager.Application.UseCases.Passport.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.Controllers
{
    [Route("api/passports")]
    [ApiController]
    public class PassportsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePassportAsync(
            [FromBody] CreatePassportRequest request,
            CancellationToken cancellationToken)
        {

            CreatePassportCommand command = new()
            {
                Type = request.Type,
                Number = request.Number
            };

            PassportDto createdPassport = await _mediator.Send(command, cancellationToken);

            ApiResponse<CreatePassportResponse> response = ApiResponse<CreatePassportResponse>.SuccessResponse(
                new CreatePassportResponse(
                    createdPassport.Id,
                    createdPassport.Type,
                    createdPassport.Number
                    ),
                "Passport created successfully");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPassportsAsync()
        {
            IEnumerable<PassportDto> allPassports = await _mediator.Send(new GetAllPassportsQuery());

            IEnumerable<PassportResponse> allPassportsResponse = allPassports
                .Select(p => new PassportResponse(p.Id, p.Type, p.Number));

            var response = ApiResponse<IEnumerable<PassportResponse>>.SuccessResponse(allPassportsResponse);
            return Ok(response);
        }

        [HttpGet("{passportId}")]
        public async Task<IActionResult> GetPassportAsync(int passportId, CancellationToken cancellationToken)
        {
            PassportDto passportDto = await _mediator.Send(new GetPassportByIdQuery(passportId), cancellationToken);

            ApiResponse<PassportResponse> response = ApiResponse<PassportResponse>.SuccessResponse(
                new PassportResponse(passportDto.Id, passportDto.Type, passportDto.Number ));

            return Ok(response);
        }


        [HttpPut("{passportId}")]
        public async Task<IActionResult> UpdateDeparmentAsync(int passportId, [FromBody] UpdatePassportRequest request)
        {
            PassportDto updatedPassportDto = await _mediator.Send(
                new UpdatePassportCommand(passportId, request.Type, request.Number));

            UpdatePassportResponse updatePassportIdResponse = new UpdatePassportResponse(
                updatedPassportDto.Id, updatedPassportDto.Type, updatedPassportDto.Number);

            ApiResponse<UpdatePassportResponse> response = ApiResponse<UpdatePassportResponse>
                .SuccessResponse(updatePassportIdResponse, "Passport updated successfully");

            return Ok(response);
        }

        [HttpDelete("{passportId}")]
        public async Task<IActionResult> DeletePassportAsync(int passportId)
        {
            PassportDto deletedPassport = await _mediator.Send(new DeletePassportCommand(passportId));

            ApiResponse<DeletePassportResponse> response = ApiResponse<DeletePassportResponse>.SuccessResponse(
                new DeletePassportResponse(
                    deletedPassport.Id,
                    deletedPassport.Type,
                    deletedPassport.Number
                    ),
                "Passport deleted successfully");

            return Ok(response);
        }
    }
}
