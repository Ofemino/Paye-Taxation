using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paye.Infrastructure.DependencyInjection;
using MediatR;
using Paye.Application.Features.Documents.Commands;
using System.Security.Claims;
using Paye.Shared;

namespace Paye.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> Upload([FromForm] IFormFile file, [FromForm] Guid submissionId, [FromForm] Guid uploadedBy)
        {
            // Users can only upload documents for their own submissions, or admins can upload for any
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("FC&SP") || User.IsInRole("Auditor") || User.IsInRole("Tax Unit");
            
            // Note: Additional validation should be done in the command handler to verify ownership
            var command = new UploadDocumentCommand(file, submissionId, uploadedBy);
            var path = await _mediator.Send(command);
            return Ok(ApiResponse.Succeed(new { Path = path }));
        }
    }
}
