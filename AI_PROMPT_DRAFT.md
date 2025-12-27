# AI Development Prompt: PAYE Tax Relief System

## Project Overview
Develop a "PAYE Tax Relief" management system using **.NET 10** (Clean Architecture) and **Blazor WebAssembly**. The system allows staff members to submit tax relief claims (Rent or Mortgage Interest) and admins to generate reports.

## Architecture
- **Style**: Clean Architecture (Domain, Application, Infrastructure, API, Client).
- **Pattern**: CQRS with MediatR.
- **Database**: SQL Server with EF Core.
- **Frontend**: Blazor WebAssembly.

## Key Modules & Features

### 1. Authentication & Staff Management
- **Entity**: `Staff` (Id, StaffId, Name, Email, Branch, State, IsActive).
- **Rules**:
  - Staff must be Active to perform actions.
  - Staff are identified by unique `StaffId`.

### 2. Tax Relief Submission (Core Feature)
- **Entity**: `TaxReliefSubmission`.
- **Inputs**: TaxYear, OwnershipType (Tenant/Landlord).
- **Business Rules**:
  - **One Submission Per Year**: A staff member can only have one submission per tax year.
  - **Tenant Mode**: Must provide `RentRelief` details.
  - **Landlord Mode**: Must provide `MortgageInterestRelief` details; cannot have Rent Relief.
  - **Locking**: Submissions can be "Locked" (immutable) after finalization.
  - **Audit Trail**: All major changes must be recorded in `AuditEntry` (Who, What, When).
- **File Uploads**: Support `SupportingDocument` uploads linked to the submission.

### 3. Reporting
- **Features**: Generate reports of submissions.
- **Filters**:
  - Staff ID
  - Branch
  - State
  - Tax Year

### 4. API Endpoints
- `POST /api/auth/login`: Authenticate staff.
- `POST /api/tax-relief-submissions`: Create a new submission.
- `GET /api/tax-relief-submissions/{id}`: Get submission details.
- `POST /api/documents`: Upload supporting documents.
- `GET /api/reporting`: Get filtered submission reports.

## Technical Requirements
- Use **FluentValidation** for DTO validation.
- Use **MediatR** for decoupling Commands/Queries.
- Implement **Global Exception Handling**.
- Ensure **Audit Logging** is implemented in the Domain entities.
