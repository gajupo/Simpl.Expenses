# Roles & Permissions – **Drop‑in Implementation Guide** (Start from your existing `User`)

> Goal: Add **dynamic roles & permissions** on top of your current .NET Core Web API with **JWT**. This guide starts at the **User model** and moves outward. Keep it lean and production‑ready for an *expenses* app.

---

## 0) Assumptions

- You already have: `User` entity, `ApplicationDbContext`, JWT login (or easy to modify), SQL Server.
- We will **not** bring full ASP.NET Identity; only `PasswordHasher<T>` for hashing.
- NuGets ensured: `Microsoft.EntityFrameworkCore`, `…SqlServer`, `…Design`, `Microsoft.AspNetCore.Authentication.JwtBearer`, `Microsoft.IdentityModel.Tokens`, `Microsoft.AspNetCore.Identity`. Note: If they are indeed installed, you can skip this step.

---

## 1) Extend your **User** model (add nav properties)

Add only the relationships—don’t rename your existing columns.

```csharp
// Domain/Entities/User.cs (extend your existing class)
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public int? ReportsToId { get; set; }
    public User ReportsTo { get; set; }
    public bool IsActive { get; set; }

    // NEW
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
```

---

## 2) Add **Role**, **Permission** entities + join tables

Create new files in your Domain project.

```csharp
public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!; // e.g., "Expenses.Read"
    public string? Description { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}

public class UserRole
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
}

public class RolePermission
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;
}

public class UserPermission
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;
}
```

---

## 3) **DbContext**: DbSets + keys/indexes

```csharp
public DbSet<Role> Roles => Set<Role>();
public DbSet<Permission> Permissions => Set<Permission>();
public DbSet<UserRole> UserRoles => Set<UserRole>();
public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
public DbSet<UserPermission> UserPermissions => Set<UserPermission>();

protected override void OnModelCreating(ModelBuilder b)
{
    base.OnModelCreating(b);

    b.Entity<User>(e =>
    {
        e.HasIndex(x => x.Username).IsUnique();
        e.Property(x => x.Username).HasMaxLength(128).IsRequired();
        e.Property(x => x.Email).HasMaxLength(256).IsRequired();
    });

    b.Entity<Role>(e =>
    {
        e.HasIndex(x => x.Name).IsUnique();
        e.Property(x => x.Name).HasMaxLength(128).IsRequired();
    });

    b.Entity<Permission>(e =>
    {
        e.HasIndex(x => x.Name).IsUnique();
        e.Property(x => x.Name).HasMaxLength(128).IsRequired();
    });

    b.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });
    b.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
    b.Entity<UserPermission>().HasKey(x => new { x.UserId, x.PermissionId });
}
```
### Important: For entity relationships, follow the pattern we already have in out /Infrastructure/Configurations folder for each model.

Run migration:

```bash
# From solution root (adjust project names)
dotnet ef migrations add AddRolesPermissions --project src/Migrations

dotnet ef database update --project src/Infrastructure/Simpl.Expenses.Infrastructure.csproj --startup-project src/Core.WebApi/Core.WebApi.csproj
```

---

## 4) Permission catalog (constants used in policies)

Expenses is equal to Reports in our code.

```csharp
public static class AppPermissions
{
    // Expenses
    public const string Expenses_Read    = "Expenses.Read";
    public const string Expenses_Create  = "Expenses.Create";
    public const string Expenses_Update  = "Expenses.Update";
    public const string Expenses_Delete  = "Expenses.Delete";
    public const string Expenses_Approve = "Expenses.Approve";

    // Admin
    public const string Users_Manage       = "Users.Manage";
    public const string Roles_Manage       = "Roles.Manage";
    public const string Permissions_Manage = "Permissions.Manage";

    public static readonly string[] All =
    {
        Expenses_Read, Expenses_Create, Expenses_Update, Expenses_Delete, Expenses_Approve,
        Users_Manage, Roles_Manage, Permissions_Manage
    };
}
```

---

## 5) **JWT login**: add role & permission claims
Atention: Please create the contoller, services, di registrations for authentication it does no exist we need it for this implementation
We need this code to compute **effective permissions** and append claims.

```csharp
var user = await _db.Users
    .Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
    .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive, ct)
    ?? throw new UnauthorizedAccessException("Invalid user");

var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
if (verify == PasswordVerificationResult.Failed)
    throw new UnauthorizedAccessException("Invalid password");

var roleNames = user.UserRoles.Select(ur => ur.Role.Name).Distinct().ToArray();
var rolePerms = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Name));
one
var userPerms = user.UserPermissions.Select(up => up.Permission.Name);
var effectivePerms = rolePerms.Union(userPerms).Distinct().ToArray();

var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.Username)
};
claims.AddRange(roleNames.Select(r => new Claim(ClaimTypes.Role, r)));
claims.AddRange(effectivePerms.Select(p => new Claim("permission", p)));
// ... sign token as you already do
```

---

## 6) **Program.cs** – Authentication & Authorization

```csharp
// AuthN: We already have this config in place, just check if it needs some changes to align with the new claims

// AuthZ: one policy per permission
builder.Services.AddAuthorization(options =>
{
    foreach (var p in AppPermissions.All)
        options.AddPolicy(p, policy => policy.RequireClaim("permission", p));
});
```

---

## 7) Protect your endpoints with **policies**

```csharp
[Authorize(Policy = AppPermissions.Expenses_Read)]
[HttpGet] public IActionResult GetExpenses() => Ok();

[Authorize(Policy = AppPermissions.Expenses_Create)]
[HttpPost] public IActionResult CreateExpense(CreateExpenseDto dto) => Ok();

[Authorize(Policy = AppPermissions.Expenses_Approve)]
[HttpPost("{id:guid}/approve")] public IActionResult Approve(Guid id) => Ok();
```

---

## 8) Minimal **Admin** actions (dynamic assignment)

Use your existing admin controllers or add these endpoints.

```csharp
// Map permissions to a role
[HttpPost("roles/{roleId:guid}/permissions")]
[Authorize(Policy = AppPermissions.Roles_Manage)]
public async Task<IActionResult> SetRolePermissions(Guid roleId, [FromBody] string[] permissionNames)
{
    var role = await _db.Roles.Include(r => r.RolePermissions).FirstOrDefaultAsync(r => r.Id == roleId);
    if (role is null) return NotFound();
    var perms = await _db.Permissions.Where(p => permissionNames.Contains(p.Name)).ToListAsync();
    role.RolePermissions = perms.Select(p => new RolePermission { RoleId = roleId, PermissionId = p.Id }).ToList();
    await _db.SaveChangesAsync();
    return Ok();
}

// Assign roles to user
[HttpPost("users/{userId:guid}/roles")]
[Authorize(Policy = AppPermissions.Users_Manage)]
public async Task<IActionResult> SetUserRoles(Guid userId, [FromBody] Guid[] roleIds)
{
    var user = await _db.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
    if (user is null) return NotFound();
    user.UserRoles = roleIds.Select(id => new UserRole { UserId = userId, RoleId = id }).ToList();
    await _db.SaveChangesAsync();
    return Ok();
}

// Optional: direct user overrides
[HttpPost("users/{userId:guid}/permissions")]
[Authorize(Policy = AppPermissions.Users_Manage)]
public async Task<IActionResult> SetUserPermissions(Guid userId, [FromBody] string[] permissionNames)
{
    var user = await _db.Users.Include(u => u.UserPermissions).FirstOrDefaultAsync(u => u.Id == userId);
    if (user is null) return NotFound();
    var perms = await _db.Permissions.Where(p => permissionNames.Contains(p.Name)).ToListAsync();
    user.UserPermissions = perms.Select(p => new UserPermission { UserId = userId, PermissionId = p.Id }).ToList();
    await _db.SaveChangesAsync();
    return Ok();
}
```

---

## 9) Quick **test flow**

1. Login as admin → get JWT.
2. Create role “Approver”.
3. `POST /api/admin/roles/{roleId}/permissions` with `["Expenses.Read","Expenses.Approve"]`.
4. Assign role to target user → `POST /api/admin/users/{userId}/roles`.
5. Login as that user → call `POST /api/expenses/{id}/approve` → **200 OK**.

---

## 10) Troubleshooting

- **403 but should allow?** Confirm JWT contains `permission` claims and policy string matches exactly.
- **Changes not applied?** New permissions/roles apply on **next token**. Use a short token lifetime (15–60 min) or add refresh.
- **Login fails:** Be sure `PasswordHasher<T>` verification matches how you stored `PasswordHash`.

---

## 11) Visual assignment (simple matrix for the admin UI)

```
                  | Expenses.Read | Expenses.Create | Expenses.Approve | Users.Manage | Roles.Manage
------------------+---------------+-----------------+------------------+--------------+--------------
Employee          |      ✓        |        ✓        |                  |              |             
Approver          |      ✓        |                 |        ✓         |              |             
Admin             |      ✓        |        ✓        |        ✓         |      ✓       |      ✓      

User Maria: Roles = [Approver], Overrides = [Expenses.Create]
Effective Permissions(Maria) = {Read, Approve, Create}
```

> Back these checkboxes with the admin endpoints above.

---

### Definition of Done

- Admin can create roles, set role permissions, assign roles/overrides to users.
- Policies protect endpoints; missing permission returns **403**.
- JWT includes `role` and `permission` claims; seeded `admin` can do everything.

### Additional Notes
- Create controllers, DTOs, and services as needed for this implementation always following the existing patterns of current codebase.
- Ensure all new code is covered by unit tests, especially for the authorization logic.

