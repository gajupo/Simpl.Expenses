# GEMINI Context: SimpleExpenses v3 (.NET Core)

This document provides the core context for the development of the SimpleExpenses application using the .NET Core stack. It outlines the project's vision, technology choices, features, and architectural principles to ensure consistent and high-quality development.

## 1. Project Vision & Goal

The primary goal is to build a modern, robust, and scalable expense management system from the ground up. The system will handle the entire lifecycle of an expense report, from submission and multi-step approval workflows to budget tracking and financial auditing. The application should be secure, maintainable, and provide a clean, intuitive user experience.

## 2. Technology Stack

- **Backend:** .NET 8 (or latest stable) Web API
- **Database:** Microsoft SQL Server
- **Data Access:** Entity Framework Core (EF Core)
- **Architecture:** Clean Architecture (Onion Architecture)
- **Authentication:** JWT-based token authentication
- **Frontend (Proposed):** Vue.js 3
- **Testing:** xUnit for unit and integration tests.

## 3. Core Features & Business Logic

The application will be built around the following key feature sets, derived from the refined database schema and analysis of the existing Node.js application:

#### User & Access Management
- Role-based access control (`Roles`: Admin, Finance, Manager, Employee).
- User profiles, including their department and reporting hierarchy (`reports_to_id`).
- Secure password management and user activation status.

#### Expense Reporting & ID Generation
- **Custom Report ID:** Reports will have a custom-generated ID in the format `YY-NNNNN` (e.g., `24-00123`), ensuring sequential, human-readable identifiers.
- Submission of different types of expense reports (`ReportTypes`):
  - **Reimbursement:** For employees seeking repayment.
  - **Purchase Order:** For pre-approved expenses.
  - **Advance Payment:** For requesting funds upfront.
- Ability to attach one or more files (receipts, invoices) to each report.
- Detailed information capture based on report type (e.g., `PurchaseOrderDetails`, `ReimbursementDetails`).

#### Approval Workflows & State Machine
- A flexible, multi-step workflow engine (`Workflows`, `WorkflowSteps`). The report's `status` will be the primary indicator of its position in the workflow.
- Workflows are assigned to projects (`WorkflowProjects`), allowing different approval chains.
- Approval steps are tied to `Roles`, not specific users, for flexibility.
- A complete `ApprovalLog` for auditing every action taken on a report.
- **Hierarchical Approval:** The system must route approvals up the departmental chain based on the `reportsTo` relationship.

#### Financial & Departmental Workflows
- **Treasury (`Tesoreria`):** A dedicated workflow for the Treasury department to mark reports as `PAGADO` and upload payment proofs.
- **Purchasing (`Compras`):** A specialized multi-step workflow for the Purchasing department, including validation, processing, and formal Purchase Order (OC) generation.
- **Accounts Payable:** A workflow to handle reports that have been fully approved and are ready for processing.

#### Automated Email Notifications
- **Critical Communication:** The system will send automated emails at every key stage of the workflow:
  - **Submission:** Notify the relevant manager when a new report is submitted.
  - **Approval/Denial:** Inform the user of the outcome of their report.
  - **Inter-department Handoff:** Notify the next group in the workflow (e.g., notify Treasury when a report is ready for payment).
  - **Supplier Communication:** Automatically email generated Purchase Orders to the relevant supplier.
  - **Payment Confirmation:** Notify the user once their expense has been paid.

#### File Management & Archiving
- **Secure Storage:** All file attachments will be stored securely on the filesystem, organized in folders by `report_id`.

#### Budget Management
- Define budgets for `CostCenters` and `AccountProjects` over specific time periods.
- A `BudgetConsumption` table to provide a real-time, transactional ledger of all expenses against their respective budgets.
- Prevents overspending and provides clear financial visibility.

#### Administration & Configuration
- Management of all lookup tables (e.g., `Suppliers`, `Categories`, `Plants`, `Incoterms`).
- Enable a flag at configuration level to toggle in maintenance mode, preventing login and report submissions.

## 4. Database Schema

The application will be built on the following well-normalized and scalable SQL Server schema.

```sql
-- 1. Core User & Organization Tables
CREATE TABLE Roles (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Departments (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(100) NOT NULL,
    cost_center_id INT FOREIGN KEY REFERENCES CostCenters(id) NULL
);

CREATE TABLE Users (
    id INT PRIMARY KEY IDENTITY,
    username NVARCHAR(50) UNIQUE NOT NULL,
    email NVARCHAR(100) UNIQUE NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    role_id INT FOREIGN KEY REFERENCES Roles(id),
    department_id INT FOREIGN KEY REFERENCES Departments(id),
    reports_to_id INT FOREIGN KEY REFERENCES Users(id),
    is_active BIT NOT NULL DEFAULT 1
);

CREATE TABLE UserPlants (
    user_id INT FOREIGN KEY REFERENCES Users(id),
    plant_id INT FOREIGN KEY REFERENCES Plants(id),
    PRIMARY KEY (user_id, plant_id)
);

-- 2. Workflow Engine Tables
CREATE TABLE Workflows (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(100) UNIQUE NOT NULL,
    description NVARCHAR(255)
);

CREATE TABLE WorkflowSteps (
    id INT PRIMARY KEY IDENTITY,
    workflow_id INT FOREIGN KEY REFERENCES Workflows(id),
    step_number INT NOT NULL,
    name NVARCHAR(100) NOT NULL,
    approver_role_id INT FOREIGN KEY REFERENCES Roles(id)
);

CREATE TABLE WorkflowProjects (
    workflow_id INT FOREIGN KEY REFERENCES Workflows(id),
    project_id INT FOREIGN KEY REFERENCES AccountProjects(id),
    PRIMARY KEY (workflow_id, project_id)
);

-- 3. Supporting Lookup & Budgeting Tables
CREATE TABLE ReportTypes (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Plants (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(100) NOT NULL
);

CREATE TABLE Categories (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(100) NOT NULL,
    icon NVARCHAR(10)
);

CREATE TABLE Suppliers (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    rfc NVARCHAR(20) UNIQUE NULL,
    address NVARCHAR(255) NULL,
    email NVARCHAR(100),
    is_active BIT NOT NULL DEFAULT 1
);

CREATE TABLE CostCenters (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    code NVARCHAR(50),
    is_active BIT NOT NULL DEFAULT 1
);

CREATE TABLE AccountProjects (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    code NVARCHAR(50),
    is_active BIT NOT NULL DEFAULT 1
);

CREATE TABLE Budgets (
    id INT PRIMARY KEY IDENTITY,
    cost_center_id INT FOREIGN KEY REFERENCES CostCenters(id) NULL,
    account_project_id INT FOREIGN KEY REFERENCES AccountProjects(id) NULL,
    amount DECIMAL(18, 2) NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    CONSTRAINT UQ_Budget_Period UNIQUE (cost_center_id, account_project_id, start_date, end_date)
);

CREATE TABLE UsoCFDI (
    id INT PRIMARY KEY IDENTITY,
    clave NVARCHAR(10) NOT NULL,
    description NVARCHAR(255) NOT NULL
);

CREATE TABLE Incoterms (
    id INT PRIMARY KEY IDENTITY,
    clave NVARCHAR(10) NOT NULL,
    description NVARCHAR(255) NOT NULL
);

CREATE TABLE Settings (
    [key] NVARCHAR(50) PRIMARY KEY,
    value NVARCHAR(255) NOT NULL
);

-- 4. Report & Transactional Tables
CREATE TABLE Reports (
    id INT PRIMARY KEY IDENTITY,
    report_number NVARCHAR(20) UNIQUE NOT NULL,
    name NVARCHAR(255) NOT NULL,
    amount DECIMAL(18, 2) NOT NULL,
    currency NVARCHAR(3) NOT NULL DEFAULT 'MXN',
    user_id INT FOREIGN KEY REFERENCES Users(id),
    report_type_id INT FOREIGN KEY REFERENCES ReportTypes(id),
    plant_id INT FOREIGN KEY REFERENCES Plants(id),
    category_id INT FOREIGN KEY REFERENCES Categories(id),
    supplier_id INT FOREIGN KEY REFERENCES Suppliers(id) NULL,
    bank_name NVARCHAR(100) NULL,
    account_number NVARCHAR(50) NULL,
    clabe NVARCHAR(18) NULL,
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE ReportState (
    report_id INT PRIMARY KEY FOREIGN KEY REFERENCES Reports(id),
    workflow_id INT FOREIGN KEY REFERENCES Workflows(id),
    current_step_id INT FOREIGN KEY REFERENCES WorkflowSteps(id),
    status NVARCHAR(20) NOT NULL,
    updated_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE ApprovalLog (
    id INT PRIMARY KEY IDENTITY,
    report_id INT FOREIGN KEY REFERENCES Reports(id),
    user_id INT FOREIGN KEY REFERENCES Users(id),
    action NVARCHAR(20) NOT NULL,
    comment NVARCHAR(MAX),
    log_date DATETIME DEFAULT GETDATE()
);

CREATE TABLE ReportAttachments (
    id INT PRIMARY KEY IDENTITY,
    report_id INT FOREIGN KEY REFERENCES Reports(id) NOT NULL,
    uploaded_by_user_id INT FOREIGN KEY REFERENCES Users(id),
    file_name NVARCHAR(255) NOT NULL,
    file_path NVARCHAR(500) NOT NULL,
    file_size_kb INT,
    mime_type NVARCHAR(100),
    uploaded_at DATETIME DEFAULT GETDATE()
);

-- 5. Report Detail Tables
CREATE TABLE ReimbursementDetails (
    report_id INT PRIMARY KEY FOREIGN KEY REFERENCES Reports(id),
    employee_name NVARCHAR(255) NOT NULL,
    employee_number NVARCHAR(50)
);

CREATE TABLE PurchaseOrderDetails (
    report_id INT PRIMARY KEY FOREIGN KEY REFERENCES Reports(id),
    cost_center_id INT FOREIGN KEY REFERENCES CostCenters(id),
    account_project_id INT FOREIGN KEY REFERENCES AccountProjects(id),
    uso_cfdi_id INT FOREIGN KEY REFERENCES UsoCFDI(id),
    incoterm_id INT FOREIGN KEY REFERENCES Incoterms(id)
);

CREATE TABLE AdvancePaymentDetails (
    report_id INT PRIMARY KEY FOREIGN KEY REFERENCES Reports(id),
    order_number NVARCHAR(100)
);

-- 6. Budget Consumption Tracking
CREATE TABLE BudgetConsumption (
    id INT PRIMARY KEY IDENTITY,
    cost_center_id INT FOREIGN KEY REFERENCES CostCenters(id) NULL,
    account_project_id INT FOREIGN KEY REFERENCES AccountProjects(id) NULL,
    report_id INT FOREIGN KEY REFERENCES Reports(id),
    amount DECIMAL(18, 2) NOT NULL,
    consumption_date DATETIME DEFAULT GETDATE(),
    UNIQUE(cost_center_id, account_project_id, report_id)
);

-- 7. Secondary Indexes for Performance
CREATE INDEX IX_Reports_user_id ON Reports(user_id);
CREATE INDEX IX_ReportState_status ON ReportState(status);
CREATE INDEX IX_ApprovalLog_report_id ON ApprovalLog(report_id);
CREATE INDEX IX_Suppliers_rfc ON Suppliers(rfc);
CREATE INDEX IX_Users_email ON Users(email);
```

## 5. Architectural Principles

- **Separation of Concerns:** Strictly follow Clean Architecture principles, separating the code into distinct layers: Domain, Application, Infrastructure, and Presentation (API).
- **Dependency Injection (DI):** Use .NET's built-in DI container to manage dependencies, promoting loose coupling and testability.
- **Unit and component testing:** Write unit tests for all business logic in the `Core` layer and integration tests for API endpoints. Use xUnit as the testing framework.
- **API Design:** Follow RESTful principles for API design. Use meaningful HTTP verbs (GET, POST, PUT, DELETE) and status codes. Use structured error responses.
- **Versioning:** Implement API versioning to allow for future changes without breaking existing clients.
- **Logging & Monitoring:** Integrate structured logging (e.g., Serilog).
- **Error Handling:** Implement global exception handling middleware to catch and log errors. Return standardized error responses.
- **Configuration Management:** Use `appsettings.json` for configuration, with environment-specific overrides.
- **Database Migrations:** Use EF Core migrations to manage database schema changes. Ensure migrations are applied in a controlled manner.
- **Security First:** All endpoints must be secure by default. Implement proper authorization and validation. Sanitize all inputs to prevent injection attacks.
- **Code Style:** Adhere to the standard C# coding conventions and .NET API design guidelines.
- **Documentation:** Consider using Swagger/OpenAPI for API documentation.
- **Frontend Integration:** The frontend will be developed using Vue.js 3, communicating with the backend via RESTful APIs. Ensure CORS is configured correctly to allow frontend access.
- **File Uploads configuration:** Allow file uploads for report attachments and make the api support multipart/form-data requests. Store files in a secure location, and maintain a reference in the database. Also allow upload files bigger than 4MB.
- **Continuous Integration/Deployment (CI/CD):** Set up a CI/CD pipeline to automate builds, tests, and deployments. Use tools like GitHub Actions.
- **Send Email Notifications:** Integrate an email service (e.g., SMTP) to send notifications for report submissions, approvals, and rejections.

## 6. Development Roadmap (Initial)

1.  **Project Scaffolding:** Set up the .NET solution with the layer projects (`Core`, `Application`, `Infrastructure`, `WebAPI`).
2.  **EF Core Implementation:** Create the `DbContext` and entity configurations to match the schema. Generate the initial migration.
3.  **Core API Endpoints:** Build out the initial CRUD endpoints for core entities (Users, Roles, Departments, etc.) with proper validation and error handling.
4.  **Authentication & Authorization:** Implement JWT generation and validation middleware. Secure endpoints with role-based authorization attributes. API will auth against active directory but generate JWT tokens for frontend use.
5.  **Frontend Scaffolding:** Initialize the chosen frontend framework and set up API communication. 

## 7. AI Development & Verification Protocol

To ensure quality, consistency, and correctness, the AI model (Gemini) must adhere to the following protocol for every feature implementation or modification request:

1.  **Understand the Goal:** Before writing any code, I will first confirm my understanding of the requested change, referencing the existing code and the principles in this document to clarify any ambiguity.

2.  **Identify Affected Components:** I will identify all parts of the application (API endpoints, services, database, etc.) that will be affected by the change.

3.  **Write Unit Tests First (TDD):** For any new or modified business logic within the Application or Domain layers, I will start by writing xUnit tests that define and verify the expected behavior. The tests must initially fail and then pass once the implementation is complete.

4.  **Implement the Feature/Change:** I will write the implementation code, strictly following the Clean Architecture principles, .NET coding standards, and the logic outlined in this document.

5.  **Verify with Integration Tests:** After the unit tests pass, I will write or update integration tests to verify the feature works correctly from end-to-end. This includes:
    *   Testing the API endpoint with various inputs (valid, invalid, edge cases).
    *   Confirming the correct data is being saved to and retrieved from the database.
    *   Ensuring that authentication and authorization rules are being enforced.

6.  **Code Review Simulation:** I will perform a final review of all my changes, checking for adherence to coding standards, security best practices, and the architectural principles of the project before presenting the solution.

7.  **Confirmation:** I will present the completed code and the results of the passing tests. I will not consider a task complete until all verification steps have been successfully passed.

## 8. Development Roadmap (Initial)

1.  **Project Scaffolding:** Set up the .NET solution with the layer projects (`Core`, `Application`, `Infrastructure`, `WebAPI`).
2.  **EF Core Implementation:** Create the `DbContext` and entity configurations to match the schema. Generate the initial migration.
3.  **Core API Endpoints:** Build out the initial CRUD endpoints for core entities like `Users`, `Reports`, and `Suppliers`.
4.  **Authentication & Authorization:** Implement JWT generation and validation middleware. Secure endpoints with role-based authorization attributes.
5.  **Frontend Scaffolding:** Initialize the chosen frontend framework and set up API communication. 
