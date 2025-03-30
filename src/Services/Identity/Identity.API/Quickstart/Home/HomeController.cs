// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Microsoft.eShopOnDapr.Services.Identity.API.Quickstart.Home;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController(
    ILogger<HomeController> logger,
    IServiceProvider sp
    ) : Controller
{
    private readonly IIdentityServerInteractionService _interaction = sp.GetRequiredService<IIdentityServerInteractionService>();   
    private readonly IWebHostEnvironment _environment = sp.GetRequiredService<IWebHostEnvironment>();

    public IActionResult Index()
    {
        if (_environment.IsDevelopment())
        {
            // only show in development
            return View();
        }

        logger.LogInformation("Homepage is disabled in production. Returning 404.");
        return NotFound();
    }

    /// <summary>
    /// Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        // retrieve error details from identityserver
        var message = await _interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            vm.Error = message;

            if (!_environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }

        return View("Error", vm);
    }
}