using AN010.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

#region ���U Entity Framework �|�Ψ쪺�A�Ȩ� DI / IoC �e����
builder.Services.AddDbContext<SchoolContext>(options =>
{
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection"));
});
#endregion

var app = builder.Build();

app.MapGet("/", async (SchoolContext schoolContext) =>
{
    await schoolContext.Database.MigrateAsync();

    #region ���դj�q�s�W�O�����Ʈw��
    schoolContext.Students.RemoveRange(schoolContext.Students);
    await schoolContext.SaveChangesAsync();

    int maxRecords = 1000 * 10;
    Stopwatch stopwatch = Stopwatch.StartNew();
    stopwatch.Start();

    for (int i = 0; i < maxRecords; i++)
    {
        Student student = new Student()
        {
            Name = i.ToString(),
            DateOfBirth = DateTime.Now.AddDays(i),
            Weight = i,
            Height = i,
        };
        await schoolContext.Students.AddAsync(student);
        await schoolContext.SaveChangesAsync();
    }

    stopwatch.Stop();
    double elapse = stopwatch.Elapsed.TotalSeconds;
    #endregion

    return $"Hello World! Elapse is {elapse} seconds";
});

app.Run();
