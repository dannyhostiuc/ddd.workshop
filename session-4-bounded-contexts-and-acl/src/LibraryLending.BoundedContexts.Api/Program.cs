using LibraryLending.BoundedContexts.Api;
using LibraryLending.BoundedContexts.Catalog;
using LibraryLending.BoundedContexts.Lending;

var builder = WebApplication.CreateBuilder(args);

// Catalog wiring (the "external" service) — kept inside the composition root.
builder.Services.AddSingleton<ICatalogClient, FakeCatalogClient>();

// The ACL is what Lending actually depends on.
builder.Services.AddSingleton<IExternalCatalog, CatalogAntiCorruptionLayer>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    lesson = "Session 4 — Bounded Contexts and ACL",
    endpoints = new[]
    {
        "GET /lending/items/{reference}    e.g. /lending/items/9780321125217",
    }
}));

// IMPORTANT: this handler is the Lending API. It accepts only Lending types
// (LoanableItem) and depends only on IExternalCatalog. There is no `using`
// for LibraryLending.BoundedContexts.Catalog in the handler — by design.
app.MapGet("/lending/items/{reference}", (string reference, IExternalCatalog catalog) =>
{
    var item = catalog.FindByReference(reference);
    return item is null ? Results.NotFound() : Results.Ok(item);
});

app.Run();
