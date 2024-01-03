var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(50);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
}
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    // Other session configurations if needed
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
