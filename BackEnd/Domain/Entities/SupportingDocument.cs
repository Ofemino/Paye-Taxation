using System;

namespace Paye.Domain.Entities
{
    public class SupportingDocument
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string FileType { get; private set; }
        public string StoragePath { get; private set; }
        public DateTime UploadedAt { get; private set; }
        public Guid UploadedBy { get; private set; }

        public SupportingDocument(string fileName, string fileType, string storagePath, Guid uploadedBy)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            FileType = fileType;
            StoragePath = storagePath;
            UploadedAt = DateTime.UtcNow;
            UploadedBy = uploadedBy;
        }
    }
}
