using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TrackerApp.API.Config;
using TrackerApp.API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<FirebaseSettings>(
    builder.Configuration.GetSection("Firebase"));



// Init Firebase Admin
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("Config/firebase-key.json"),
    ProjectId = builder.Configuration["Firebase:ProjectId"]

});

builder.Services.AddHttpClient();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
