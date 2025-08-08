# Roles & Permissions – Guidance-First Playbook (Service Layer + Policy Auth)

> **Purpose:** Integrate **dynamic roles & permissions** with **JWT + policy-based authorization** in a .NET Core Web API, *without prescribing exact code changes*. Use this as a checklist and guidance the LLM can follow by reading your existing codebase. **No seeding. Controllers do not use `DbContext`. AutoMapper already configured.**

---

## How to use this playbook
- Treat each section as a **review + implement** step. The LLM should **inspect current code** and infer the minimal diffs.
- Prefer **adapting existing types/services** over creating new ones.
- Keep domain relationships **as-is**; do not propose schema rewrites unless obviously missing.

---

## 0) Baseline checks (inputs)
Confirm (and record for the LLM):
- DATA_PROJECT (EF project), API_PROJECT (Web API), DB_CONTEXT (your DbContext type), NAMESPACE_ROOT, and the connection string key.
- Packages present: EF Core (+ SqlServer + Design), JwtBearer + IdentityModel, and PasswordHasher<T>. (Do **not** install if already present.)

**Acceptance:** The LLM can reference real project names/types in later steps.

---

## 1) Domain readiness (entities/navigation)
**Goal:** Ensure the domain can express roles and permissions dynamically.

**Review:**
- `User` has identifiers and status fields (`Id`, `Username`, `Email`, `PasswordHash`, `IsActive`).
- Navigations exist (or equivalent): `User ↔ Role` (direct or via `UserRole`) and `Role ↔ Permission` (via `RolePermission`). Optional `User ↔ Permission` overrides.

**Action (if missing):**
- Add only the **minimal** navs required to represent: Users ⇄ Roles ⇄ Permissions (+ optional User ⇄ Permissions overrides). Keep existing keys and naming conventions.

**Acceptance:** The LLM can traverse from a `User` to effective permissions through roles (and user overrides) without schema redesign.

---

## 2) EF Core configuration placement (no relationship changes)
**Goal:** Keep your current relationship modeling; only place any new configs consistently.

**Where to place updates:**
- `…/Persistence/Configurations/UserConfiguration.cs` – property constraints (username unique/max length, email max length) if not already present.
- `…/RoleConfiguration.cs` – role `Name` required/max length/unique (align with your sample).
- `…/PermissionConfiguration.cs` – permission `Name` required/max length/unique.
- If join tables are present/added (`UserRoles`, `RolePermissions`, `UserPermissions`), ensure they have composite keys and table names consistent with your conventions, in their own config files.
- `DbContext.OnModelCreating` should already call `ApplyConfigurationsFromAssembly(...)`.

**Acceptance:** Config files exist (or are updated) in the same folder/namespace pattern as your sample `RoleConfiguration`.

---

## 3) Permission catalog (policy names)
**Goal:** A single source of truth for permission strings used by policies and UI.

**Review/Action:** Ensure a static catalog exists (or extend the existing one) with at least:
- `Expenses.Read`, `Expenses.Create`, `Expenses.Update`, `Expenses.Delete`, `Expenses.Approve`
- `Users.Manage`, `Roles.Manage`, `Permissions.Manage`

Expose `All` as an enumerable for policy registration.

**Acceptance:** Permissions are referenced by constant strings across API and admin flows.

---

## 4) Service layer contracts
**Goal:** Controllers only call services. Services encapsulate data access and mapping.

**Review existing services/DTOs** and identify minimal additions:
- **Queries:** a method that, given `username`, returns an **auth projection** exposing: `Id`, `Username`, `Roles[]`, `Permissions[]` (plus `PasswordHash` only if your verification requires it here; otherwise verify from entity separately).
- **User admin:** operations to create a user, assign roles (`Guid[]` roleIds), and set direct permissions (`string[]` permissionNames).
- **Role admin:** operations to create a role and assign permission names to that role.

**Action:** If any contract is missing, **add a small method** to the relevant service interface and implement it. Reuse existing DTOs. Only create new DTOs if the shape is missing (keep them minimal and aligned with current naming).

**Acceptance:** Controllers never use `DbContext`. The service layer exposes exactly what the API needs.

---

## 5) Authentication flow (JWT issuance)
**Goal:** On login, issue a JWT containing role and permission claims based on effective permissions.

**Review:**
- Password verification uses your existing hashing (e.g., `PasswordHasher<T>`).
- After verification, fetch the **auth projection** (Step 4) to obtain `Roles[]` and `Permissions[]`.
- Build claims:
  - `ClaimTypes.NameIdentifier` = user Id
  - `ClaimTypes.Name` = username
  - One `ClaimTypes.Role` per role
  - One `permission` claim per permission string
- Sign and return JWT using your configured issuer/audience/key and lifetime.

**Acceptance:** Successful login returns a token with role + permission claims. No controller uses `DbContext` directly.

---

## 6) Authorization setup (Program + appsettings)
**Goal:** Map each permission string to a policy.

**Review:**
- JwtBearer is configured and validates issuer, audience, signing key, lifetime.
- `RoleClaimType` is `ClaimTypes.Role`, `NameClaimType` is `ClaimTypes.NameIdentifier`.
- For each permission in the catalog, register a policy that **requires a `permission` claim** with the same string.
- `appsettings.json` contains your JWT Issuer/Audience/Key and connection string.

**Acceptance:** Policies are registered 1:1 with permission names; startup runs without errors.

---

## 7) Admin API surface (service-backed)
**Goal:** Minimal endpoints for dynamic assignment.

**Review:** Ensure there are endpoints (or add them) that:
- Create a role.
- Assign **permission names** to a role.
- Create a user.
- Assign **role IDs** to a user.
- Assign **permission names** directly to a user (optional overrides).

**Guidance:**
- Controllers call the **existing services** from Step 4. Keep responses simple (ids/OK).
- Authorization on these endpoints should use policies: `Roles.Manage`, `Users.Manage`, `Permissions.Manage`.

**Acceptance:** An administrator can fully manage users/roles/permissions without DB seeding.

---

## 8) Protect business endpoints (policy attributes)
**Goal:** Enforce permissions at the API boundary.

**Review/Action:** For expense workflows, annotate endpoints with policies matching the permission catalog, e.g.:
- GET list/details → `Authorize(Policy = "Expenses.Read")`
- POST create → `Authorize(Policy = "Expenses.Create")`
- POST approve → `Authorize(Policy = "Expenses.Approve")`

**Acceptance:** Requests missing the required `permission` claim get **403**.

---

## 9) Data changes & migrations
**Goal:** Only apply schema migration if the review in Step 1 indicates missing tables/constraints.

**Guidance:**
- If all tables/relations already exist, **no migration** needed.
- If new join tables or constraints were added, run EF migrations targeting DATA_PROJECT with API_PROJECT as startup. Keep naming and conventions consistent with the existing codebase.

**Acceptance:** Database matches the code; no runtime mapping errors.

---

## 10) Operational notes
- **Token staleness:** Role/permission changes affect **new tokens**. Choose a reasonable JWT lifetime (e.g., 15–60 min) and/or implement refresh.
---

## Appendix – Visual assignment model (for Admin UI/Docs)
```
                  | Expenses.Read | Expenses.Create | Expenses.Approve | Users.Manage | Roles.Manage
------------------+---------------+-----------------+------------------+--------------+--------------
Employee          |      ✓        |        ✓        |                  |              |              
Approver          |      ✓        |                 |        ✓         |              |              
Admin             |      ✓        |        ✓        |        ✓         |      ✓       |      ✓      

User Maria: Roles = [Approver], Overrides = [Expenses.Create]
Effective Permissions(Maria) = {Read, Approve, Create}
```

