using BookingCase.Services; // <-- ekle

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// RapidAPI için isimli HttpClient
builder.Services.AddHttpClient("booking", client =>
{
    var host = builder.Configuration["RapidApi:Host"] ?? "booking-com15.p.rapidapi.com";
    client.BaseAddress = new Uri($"https://{host}/");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", host);

    var key = builder.Configuration["RapidApi:Key"]; // secrets.json'dan gelir
    if (!string.IsNullOrWhiteSpace(key))
        client.DefaultRequestHeaders.Add("x-rapidapi-key", key);
});

// Servis kaydý
builder.Services.AddScoped<IBookingApiService, BookingApiService>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
