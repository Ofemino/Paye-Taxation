namespace Paye.Domain.Entities
{
    public class Staff
    {
        public Guid Id { get; private set; }
        public string StaffId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Branch { get; private set; }
        public string State { get; private set; }
        public bool IsActive { get; private set; }
        public IReadOnlyCollection<TaxReliefSubmission> Submissions => _submissions.AsReadOnly();
        private readonly List<TaxReliefSubmission> _submissions = new();

        public Staff(string staffId, string firstName, string lastName, string email, string branch, string state, bool isActive)
        {
            Id = Guid.NewGuid();
            StaffId = staffId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Branch = branch;
            State = state;
            IsActive = isActive;
        }

        // Enforce one submission per staff per tax year, and only if staff is active
        public void AddSubmission(TaxReliefSubmission submission)
        {
            if (!IsActive)
                throw new InvalidOperationException("Inactive staff cannot create submissions.");
            if (_submissions.Any(s => s.TaxYear == submission.TaxYear))
                throw new InvalidOperationException($"Submission for tax year {submission.TaxYear} already exists.");
            _submissions.Add(submission);
        }
    }
}
