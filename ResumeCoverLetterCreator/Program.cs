using Microsoft.EntityFrameworkCore;
using ResumeCoverLetterCreator;
using ResumeCoverLetterCreator.DataAccess;
using ResumeCoverLetterCreator.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ResumeCreatorDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ResumeDb")));

builder.Services.AddScoped<IDocumentTemplateService>((sp) => new DocumentTemplateService(builder.Environment.WebRootPath+"\\templates"));
builder.Services.AddScoped<IDocumentProcessingService, DocumentProcessingService>();
builder.Services.AddScoped<ITextProcessingService, TextProcessingService>();

builder.Services.AddRazorPages();

var app = builder.Build();

//app.UseStaticFiles();
//app.MapGet("/", () => "Hello World!");
app.MapRazorPages();

SeedData.EnsureCreated(app.Services);

app.Run();
