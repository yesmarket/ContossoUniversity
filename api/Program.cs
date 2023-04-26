using ContosoUniversity;
using HotChocolate.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SchoolContext>();

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<SchoolContext>()
    .AddQueryType<Query>()
    .AddProjections();

var app = builder.Build();

InitializeDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGraphQL();
app.UsePlayground();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.Run();

static void InitializeDatabase(IApplicationBuilder app)
{
    using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

    var context = serviceScope.ServiceProvider.GetRequiredService<SchoolContext>();
    if (!context.Database.EnsureCreated()) return;

    var course = new Course { Credits = 10, Title = "Object Oriented Programming 1" };

    context.Enrollments.Add(new Enrollment
    {
        Course = course,
        Student = new Student { FirstMidName = "Rafael", LastName = "Foo", EnrollmentDate = DateTime.UtcNow }
    });
    context.Enrollments.Add(new Enrollment
    {
        Course = course,
        Student = new Student { FirstMidName = "Pascal", LastName = "Bar", EnrollmentDate = DateTime.UtcNow }
    });
    context.Enrollments.Add(new Enrollment
    {
        Course = course,
        Student = new Student { FirstMidName = "Michael", LastName = "Baz", EnrollmentDate = DateTime.UtcNow }
    });
    context.SaveChangesAsync();
}
