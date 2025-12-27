using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace Paye.Application.Features.Documents.Commands
{
    public class UploadDocumentCommand : IRequest<string>
    {
        public IFormFile File { get; set; }
        public Guid SubmissionId { get; set; }
        public Guid UploadedBy { get; set; }
        public UploadDocumentCommand(IFormFile file, Guid submissionId, Guid uploadedBy)
        {
            File = file;
            SubmissionId = submissionId;
            UploadedBy = uploadedBy;
        }
    }
}
