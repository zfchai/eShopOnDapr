// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Microsoft.eShopOnDapr.Services.Identity.API.Quickstart;

public class SecurityHeadersAttribute : ActionFilterAttribute
{
    private const string X_Content_Type_Options = "X-Content-Type-Options";
    private const string X_Frame_Options = "X-Frame-Options";
    private const string Content_Security_Policy = "Content-Security-Policy";
    private const string X_Content_Security_Policy = "X-Content-Security-Policy";
    private const string Referrer_Policy = "Referrer-Policy";
    private const string X_Permitted_Cross_Domain_Policies = "X-Permitted-Cross-Domain-Policies";

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var result = context.Result;
        if (result is ViewResult)
        {
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            if (!context.HttpContext.Response.Headers.ContainsKey(X_Content_Type_Options))
            {
                context.HttpContext.Response.Headers.TryAdd(X_Content_Type_Options, "nosniff");
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            if (!context.HttpContext.Response.Headers.ContainsKey(X_Frame_Options))
            {
                context.HttpContext.Response.Headers.TryAdd(X_Frame_Options, "SAMEORIGIN");
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
            // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
            //csp += "upgrade-insecure-requests;";
            // also an example if you need client images to be displayed from twitter
            // csp += "img-src 'self' https://pbs.twimg.com;";

            // once for standards compliant browsers
            if (!context.HttpContext.Response.Headers.ContainsKey(Content_Security_Policy))
            {
                context.HttpContext.Response.Headers.TryAdd(Content_Security_Policy, csp);
            }
            // and once again for IE
            if (!context.HttpContext.Response.Headers.ContainsKey(X_Content_Security_Policy))
            {
                context.HttpContext.Response.Headers.TryAdd(X_Content_Security_Policy, csp);
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            if (!context.HttpContext.Response.Headers.ContainsKey(Referrer_Policy))
            {
                context.HttpContext.Response.Headers.TryAdd(Referrer_Policy, "no-referrer");
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/X-Permitted-Cross-Domain-Policies
            if (!context.HttpContext.Response.Headers.ContainsKey(X_Permitted_Cross_Domain_Policies))
            {
                context.HttpContext.Response.Headers.TryAdd(X_Permitted_Cross_Domain_Policies, "none");
            }
        }
    }
}
