using MediatR;
using Paye.Application.DTOs;
using Paye.Application.Features.Documents.Commands;
using Paye.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Paye.Domain.Entities;
using Paye.Infrastructure.DependencyInjection;

namespace Paye.Infrastructure.Features.Documents.Commands
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, string>
    {
        private readonly PayeDbContext _db;
        private readonly IFileStorageService _fileStorage;
        public UploadDocumentCommandHandler(PayeDbContext db, IFileStorageService fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        public async Task<string> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            // Validate file type
            var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
            if (!allowedTypes.Contains(request.File.ContentType))
                throw new InvalidDataException("Invalid file type.");

            // TODO: Virus scanning logic here

            // Store file
            using var stream = request.File.OpenReadStream();
            var path = await _fileStorage.SaveAsync(stream, request.File.FileName, request.File.ContentType, cancellationToken);

            // Link to submission
            var submission = await _db.TaxReliefSubmissions.FindAsync(new object[] { request.SubmissionId }, cancellationToken);
            if (submission == null) throw new KeyNotFoundException("Submission not found");
            var doc = new SupportingDocument(request.File.FileName, request.File.ContentType, path, request.UploadedBy);
            submission.AddSupportingDocument(doc);
            _db.SupportingDocuments.Add(doc);
            await _db.SaveChangesAsync(cancellationToken);
            return path;
        }
    }
}
