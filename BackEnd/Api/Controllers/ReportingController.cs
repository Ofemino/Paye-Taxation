using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Paye.Application.Features.Reporting.Queries;
using Paye.Shared;
using Paye.Application.DTOs;

namespace Paye.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Auditor,FC&SP,Tax Unit,IT Operations")]
    public class ReportingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Auditor,FC&SP,Tax Unit,IT Operations")] // All admin roles can view reports
        public async Task<ActionResult<ApiResponse<List<TaxReliefSubmissionDto>>>> Get([FromQuery] string? staffId, [FromQuery] string? branch, [FromQuery] string? state, [FromQuery] int? taxYear)
        {
            var query = new GetSubmissionsReportQuery { StaffId = staffId, Branch = branch, State = state, TaxYear = taxYear };
            var results = await _mediator.Send(query);
            return Ok(ApiResponse<List<TaxReliefSubmissionDto>>.Succeed(results));
        }

        [HttpGet("export")]
        [Authorize(Roles = "Auditor,FC&SP,Tax Unit,IT Operations")] // All admin roles can export reports
        public async Task<IActionResult> Export([FromQuery] string? staffId, [FromQuery] string? branch, [FromQuery] string? state, [FromQuery] int? taxYear)
        {
            var query = new GetSubmissionsReportQuery { StaffId = staffId, Branch = branch, State = state, TaxYear = taxYear };
            var results = await _mediator.Send(query);

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("StaffId,TaxYear,OwnershipType,AnnualRent,DeductibleInterest,DocumentsCount");

            foreach (var item in results)
            {
                builder.AppendLine($"{item.StaffId},{item.TaxYear},{item.OwnershipType},{item.AnnualRent},{item.DeductibleInterest},{item.SupportingDocuments.Count}");
            }

            return File(System.Text.Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"submissions_report_{DateTime.Now:yyyyMMddHHmmss}.csv");
        }
    }
}
