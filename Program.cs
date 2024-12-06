using TestWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<SalesOrderService>();
builder.Services.AddSingleton<CustomerService>();

// Add session services
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout
  options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
  options.Cookie.IsEssential = true; // Mark the cookie as essential
});

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.MapRazorPages();

app.Run();
