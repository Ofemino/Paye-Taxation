I have examined the backend and client code. Here is the status:

### **Status Report**
1.  **Backend (API & Logic)**:
    *   **Authentication**: ✅ Implemented (Controllers & Service).
    *   **Tax Relief Submission**: ✅ Implemented (Enforces "One per year" & "Tenant/Landlord" rules).
    *   **Reporting**: ⚠️ **Bug Found**. The `GetSubmissionsReportQueryHandler` **ignores** the `Branch` and `State` filters. It currently only filters by `StaffId` and `TaxYear`.
2.  **Frontend (Blazor Client)**:
    *   ❌ **Completely Empty**. The pages `Login.razor`, `StaffSubmission.razor`, and `Reporting.razor` are just placeholders.
    *   ❌ `ApiClient.cs` is empty and needs methods to call the API.

### **Implementation Plan**
I will fix the backend bug and implement the missing frontend features.

#### **Phase 1: Backend Fixes**
*   **Fix Reporting Query**: Update `GetSubmissionsReportQueryHandler.cs` to join `TaxReliefSubmissions` with `Staffs` so we can filter by `Branch` and `State`.

#### **Phase 2: Client Service Implementation**
*   **Update `ApiClient.cs`**: Add methods for:
    *   `LoginAsync(LoginDto)`
    *   `SubmitTaxReliefAsync(TaxReliefSubmissionDto)`
    *   `GetReportsAsync(filter)`
    *   `UploadDocumentAsync(file)`

#### **Phase 3: Frontend Pages**
*   **Implement `Login.razor`**: Create a login form with JWT storage.
*   **Implement `StaffSubmission.razor`**: Create a multi-step form:
    *   Select Tax Year.
    *   Select Type (Tenant vs Landlord).
    *   Conditional inputs (Rent vs Mortgage).
    *   File Upload.
*   **Implement `Reporting.razor`**: Create a dashboard with filters (Branch, State, Year) and a results table.

Shall I proceed with this plan?