# Flo.Repositories
Simple .NET Repository pattern with authentication and validation

## Why?

Repository is a common pattern in CRUD-style applications.  Flo.Repositories acts as a simple customisable pattern
template for you to easily create either simple or sophisticated repositories in a structured way.

Flo.Repositories will be most useful in a CRUD-heavy application where the majorty of business logic is 
data and validation related, but can also be used as a lighter repository layer in applications with more
business logic.

## Common problems with Repo patterns

- Many GetXXX functions and returning more data than is needed
  - Repositories tend not to offer IQueryable style interfaces, and each type of data request needs its
  own particular method to return data, ending up with lots and lots of (often similar looking) methods or returning more
  data than is required and letting the front-end sort it out
- Where to handle validation: 
  - Only in front-end with business and repository trusting data is valid?
  - In business layer before accessing repo?
- How to communicate validation results back to the front end
  - Lots of bespoke logic and mapping?  Generic errors?
- How to handle authorisation: 
  - Only in front-end with business and repository layers trusting that access is authorised?  What if another front-end is subsequently written or somebody accesses an API directly?

Flo.Repositories helps solve these by:

- A **safe IQueryable style interface** (`IRepoQuery`) that allows `.Where` and `.Select` methods, avoiding many 
  custom methods, but also limiting query results to the data the client is authorised to have access to
- Allowing for **custom validators** for each repository, so each entity can be validated in as simple or as complex 
  a manner as required
- A **Result** return object that lets the front end know about validation errors and can be easily mapped to the view model.  Can be serialised if required
- Allowing for **custom authorisers** for each repository, so requests can be screened against user credentials

### Basic Usage

Create an entity by inheriting from `Entity<TKeyType>`:

```csharp
public class Person : Entity<Guid>
{
    public string Name {get; set;}
}
```

Create a repo by inheriting from `RepositoryBase`:

```csharp
public class PersonRepo : RepositoryBase<Person, Guid>
{
    public PersonRepo(
        ICrud<Person, Guid> crud, // provided by data impelmentation
        IValidator<Person, Guid>, 
        IAuthoriser<Person, Guid>) 
        : base(crud, validator, authoriser) { }
}
```

Add your data implementation, your repos and your validators to the services collection:

```csharp
services.AddMongoDBRepo<
    Person, // your entity
    Guid,  // it's keytype
    PersonRepo, // your repository (above)
    DataAnnotationValidator, // a basic or your bespoke validatator
    AllowAllAuthoriser // a basic or your bespoke authoriser
    >();
```

And inject and use your repo:

```csharp
await repo.AddAsync(new Person { Name = "Brian" });
Person p = await repo.GetAsync().Where(p => p.Name = "Brian").FirstOrDefaultAsync();
p.Name = "Dave";
await repo.UpdateAsync(p);
await repo.DeleteAsync(p.Id);
```

### Validation

Add custom validation, basic or as complex as you want.  Implement `IValidator` and the `ValidateAsync(Entity)` method which
gets called each time an Add or Update call is made.

```csharp
public class PersonValidator : IValidator<Person, Guid>
{
  public async Task<Result> ValidateAsync(Person p)
  {
    Result r = Result.Ok(); // everything starts off ok!
    
    // make sure everyone is Brian
    if (p.Name != "Brian") { r.combine(Result.Fail(nameof(p.Name), "All must be Brian")); }
    
    // ... plus any other validation logic you can imagine - e.g. inject other repos, run queries, etc.

    return r;
  }
}

Result r = await repo.AddAsync(new Person { Name = "Dave" }); // returns Result.Suceeded = False, Result.Errors = { { "Name", "All must be Brian" } }

```

A basic DataAnnotations validator is included, and available alone as a base to add custom validation on top:

```csharp
public class PersonValidator : DataAnnotationsValidator<Person, Guid>
{
  public async Task<Result> ValidateAsync(Person p)
  {
    Result r = base.ValidateAsync(p); // call base first for data annotations validation...
    
    //... then add extra valdation as before
    
    // make sure everyone is Brian
    if (p.Name != "Brian") { r.combine(Result.Fail(nameof(p.Name), "All must be Brian")); }
    
    // ... plus any other validation logic you can imagine - e.g. inject other repos, run queries, etc.

    return r;
  }
}
```

### Flexible Result object to return validation errors

Add or Update functions return `Result` which has a `Result.Suceeded` boolean sucess value, and a `Result.Errors` list of key / value error messages for each field.  This can be directly returned to the UI (Json serialised on the way if needed) and easily mapped to front end validation handling for pretty handling of server side validation errors.

### Authorisation

You can also add authorisation logic to the repo:

```csharp
public class PersonAuthoriser : AuthoriserBase<Person, Guid>
{
  IRepository<User, String> userRepo;
  
  public PersonAuthoriser(IRepository<User, string> userRepo) // inject a userRepo for access to userdata, but could also inject ASP.NET identity classes or anything else
  {
    this.userRepo = userRepo;
  }
  
  public override async Task EnsureCanAdd(Person p)
  {
     string userId = authenticationService.GetCurrentUserId(); // IAuthenticationService impelmented by your authentication framework to provide a hook to user identity
     
     bool admin = await userRepo.GetAsync().Where(u => u.Id == userId && u.IsAdmin == true).AnyAsync();
     
     if (!admin) throw new NotAuthorisedException();
  }
  
  // ... EnsureCanDelete(), EnsureCanUpdate(), EnsureCanRead(), EnsureCanRead(id)... extend as needed
}

await repo.AddAsync(new Person { Name = "Dave" }); // gives NotAuthorisedException if current user is not admin
```

### Safe Query object that you can pass to clients

Passing IQueryable to clients can be dangerous as it can provide wider access than desired, but is very efficient.

Writing all query logic into the repository requires many, many query functions that are often repetitive.

Instead, an IQueryable from the underlying dataprovier is wrapped in an IRepoQuery object which gives authorised access whilst still 
providing useful IQueryable features (.Where, .Select, .Any, .FirstOrDefault, .ToList)

```csharp
await repo.GetAsync().Where(p => p.Name == "Brian").ToListAsync();
```

Control which users can access which types of data in as granular fashion as you wish by adding custom GetXXX to your Repos and custom EnsureXXX to your authorisers:

In the example below, we want to make sure that a user has access to a particular department's data, and limit the 
results only to this data.

```csharp
public class PersonRepo : RepositoryBase<Person, Guid>
{
  public async Task<IRepoQuery<Person, Guid>> GetByDepartmentAsync(int depNumber)
  {
    await authoriser.EnsureHasAccessToDepartment(depNumber); // custom EnsureXXX method on PersonAuthoriser with authorisation logic
    
    return await crud.GetAsync().Where(p => p.Department == depNumber);  // where clause limits IRepoQuery results to specific authorised department
  }
}

await repo.GetByDepartmentAsync(10).ToListAsync(); // returns NotAuthorisedException if current user does not have access to department 10, by whatever logic you implement
```

### More to come!

