I will implement the authentication system improvements without hardcoding values and add external login support.

### **Plan**

1.  **Configure `appsettings.json`**: Add an `Authentication` section for Google and Microsoft provider configuration.
2.  **Infrastructure Updates**:
    *   Update `InfrastructureServiceRegistration.cs` to add `AddAuthentication().AddGoogle().AddMicrosoftAccount()` reading from configuration.
    *   **Crucial Step**: Also add `.AddJwtBearer()` configuration here, as it was missing from the registration, ensuring JWT validation works correctly.
3.  **Application Contract Update**:
    *   Modify `IAuthService` to include `ExternalLoginAsync` method.
4.  **Auth Service Implementation**:
    *   Update `AuthService.cs` to implement `ExternalLoginAsync`. This method will:
        *   Accept an external provider token/info.
        *   Validate/Find the user in `Identity`.
        *   Create the user if they don't exist.
        *   Generate and return the application's JWT.
5.  **NuGet Packages**: Add necessary packages (`Microsoft.AspNetCore.Authentication.Google`, `Microsoft.AspNetCore.Authentication.MicrosoftAccount`, `Microsoft.AspNetCore.Authentication.JwtBearer`).

### **Configuration Structure (No Hardcoding)**
The configuration will look like this in `appsettings.json`:
```json
"Authentication": {
  "Google": { "ClientId": "", "ClientSecret": "" },
  "Microsoft": { "ClientId": "", "ClientSecret": "" }
}
```
Users can override these via environment variables or User Secrets in production.

Shall I proceed with this plan?