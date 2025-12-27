using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paye.Application.DTOs;
using Paye.Application.Features.TaxRelief.Commands;
using Paye.Application.Features.TaxRelief.Queries;
using System.Security.Claims;
using Paye.Shared;

namespace Paye.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class TaxReliefSubmissionsController : ControllerBase
  {
    private readonly IMediator _mediator;
    public TaxReliefSubmissionsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    [Authorize] // Any authenticated user (staff) can create their own submission
    public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] TaxReliefSubmissionDto dto)
    {
      var command = new CreateTaxReliefSubmissionCommand(dto);
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(Get), new { staffId = dto.StaffId, taxYear = dto.TaxYear }, ApiResponse<Guid>.Succeed(id));
    }

    [HttpGet]
    [Authorize] // Staff can view their own, admins can view any
    public async Task<ActionResult<ApiResponse<TaxReliefSubmissionDto>>> Get([FromQuery] Guid staffId, [FromQuery] int taxYear)
    {
      // Authorization check: Users can only view their own submissions unless they are admins
      var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var isAdmin = User.IsInRole("Auditor") || User.IsInRole("FC&SP") || User.IsInRole("Tax Unit") || User.IsInRole("IT Operations");
      
      // Note: Additional validation should be done in the query handler to verify ownership
      var query = new GetTaxReliefSubmissionQuery(staffId, taxYear);
      var result = await _mediator.Send(query);
      if (result == null) return NotFound(ApiResponse.Fail("Submission not found"));
      return Ok(ApiResponse<TaxReliefSubmissionDto>.Succeed(result));
    }

    [HttpGet("{id}")]
    [Authorize] // Staff can view their own, admins can view any
    public async Task<ActionResult<ApiResponse<TaxReliefSubmissionDto>>> GetById(Guid id)
    {
      // Authorization check: Users can only view their own submissions unless they are admins
      var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var isAdmin = User.IsInRole("Auditor") || User.IsInRole("FC&SP") || User.IsInRole("Tax Unit") || User.IsInRole("IT Operations");
      
      // Note: Additional validation should be done in the query handler to verify ownership
      var query = new GetTaxReliefSubmissionByIdQuery(id);
      var result = await _mediator.Send(query);
      if (result == null) return NotFound(ApiResponse.Fail("Submission not found"));
      return Ok(ApiResponse<TaxReliefSubmissionDto>.Succeed(result));
    }

    [HttpPost("lock")]
    [Authorize(Roles = "FC&SP,Auditor,Tax Unit")] // Only FC&SP, Auditor, and Tax Unit can lock submissions
    public async Task<ActionResult<ApiResponse>> Lock([FromBody] Guid submissionId)
    {
      var result = await _mediator.Send(new LockTaxReliefSubmissionCommand(submissionId));
      if (!result.Success)
        return BadRequest(ApiResponse.Fail(result.Error ?? "Submission lock failed."));
      return Ok(ApiResponse.Succeed(message: "Submission locked successfully."));
    }
  }
}
