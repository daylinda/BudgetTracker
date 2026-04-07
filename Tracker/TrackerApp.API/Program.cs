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
builder.Services.AddSingleton<UserService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<FirebaseSettings>(
    builder.Configuration.GetSection("Firebase"));



var firebaseKeyPath = "Config/firebase-key.json";
if (File.Exists(firebaseKeyPath))
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(firebaseKeyPath),
        ProjectId = builder.Configuration["Firebase:ProjectId"]
    });
}
else
{
    Console.WriteLine("WARNING: firebase-key.json not found. Firebase Admin unavailable.");
}

builder.Services.AddHttpClient();





var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
