﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/openiddict/openiddict-core for more information concerning
 * the license and the contributors participating to this project.
 */

using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using OpenIddict.Client.SystemNetHttp;
using Polly;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Exposes the necessary methods required to configure the OpenIddict client/System.Net.Http integration.
/// </summary>
public sealed class OpenIddictClientSystemNetHttpBuilder
{
    /// <summary>
    /// Initializes a new instance of <see cref="OpenIddictClientBuilder"/>.
    /// </summary>
    /// <param name="services">The services collection.</param>
    public OpenIddictClientSystemNetHttpBuilder(IServiceCollection services)
        => Services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>
    /// Gets the services collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IServiceCollection Services { get; }

    /// <summary>
    /// Amends the default OpenIddict client/System.Net.Http configuration.
    /// </summary>
    /// <param name="configuration">The delegate used to configure the OpenIddict options.</param>
    /// <remarks>This extension can be safely called multiple times.</remarks>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    public OpenIddictClientSystemNetHttpBuilder Configure(Action<OpenIddictClientSystemNetHttpOptions> configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        Services.Configure(configuration);

        return this;
    }

    /// <summary>
    /// Configures the <see cref="HttpClient"/> used by the OpenIddict client/System.Net.Http integration.
    /// </summary>
    /// <remarks>
    /// Note: customizations configured using this method apply to all providers.
    /// </remarks>
    /// <param name="configuration">The delegate used to configure the <see cref="HttpClient"/>.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public OpenIddictClientSystemNetHttpBuilder ConfigureHttpClient(Action<HttpClient> configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return Configure(options =>
        {
            if (options.HttpClientActions.TryGetValue(string.Empty, out var actions))
            {
                actions.Add(configuration);
            }

            else
            {
                options.HttpClientActions[string.Empty] = new(capacity: 1) { configuration };
            }
        });
    }

    /// <summary>
    /// Configures the <see cref="HttpClient"/> used by the OpenIddict client/System.Net.Http integration.
    /// </summary>
    /// <remarks>
    /// Note: customizations configured using this method only apply to the specified provider.
    /// </remarks>
    /// <param name="provider">The provider name, to which the customizations are applied.</param>
    /// <param name="configuration">The delegate used to configure the <see cref="HttpClient"/>.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public OpenIddictClientSystemNetHttpBuilder ConfigureHttpClient(string provider, Action<HttpClient> configuration)
    {
        if (string.IsNullOrEmpty(provider))
        {
            throw new ArgumentException(SR.FormatID0366(nameof(provider)), nameof(provider));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return Configure(options =>
        {
            if (options.HttpClientActions.TryGetValue(provider, out var actions))
            {
                actions.Add(configuration);
            }

            else
            {
                options.HttpClientActions[provider] = new(capacity: 1) { configuration };
            }
        });
    }

    /// <summary>
    /// Configures the <see cref="HttpClientHandler"/> used by the OpenIddict client/System.Net.Http integration.
    /// </summary>
    /// <remarks>
    /// Note: customizations configured using this method apply to all providers.
    /// </remarks>
    /// <param name="configuration">The delegate used to configure the <see cref="HttpClientHandler"/>.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public OpenIddictClientSystemNetHttpBuilder ConfigureHttpClientHandler(Action<HttpClientHandler> configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return Configure(options =>
        {
            if (options.HttpClientHandlerActions.TryGetValue(string.Empty, out var actions))
            {
                actions.Add(configuration);
            }

            else
            {
                options.HttpClientHandlerActions[string.Empty] = new(capacity: 1) { configuration };
            }
        });
    }

    /// <summary>
    /// Configures the <see cref="HttpClientHandler"/> used by the OpenIddict client/System.Net.Http integration.
    /// </summary>
    /// <remarks>
    /// Note: customizations configured using this method only apply to the specified provider.
    /// </remarks>
    /// <param name="provider">The provider name, to which the customizations are applied.</param>
    /// <param name="configuration">The delegate used to configure the <see cref="HttpClientHandler"/>.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public OpenIddictClientSystemNetHttpBuilder ConfigureHttpClientHandler(string provider, Action<HttpClientHandler> configuration)
    {
        if (string.IsNullOrEmpty(provider))
        {
            throw new ArgumentException(SR.FormatID0366(nameof(provider)), nameof(provider));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return Configure(options =>
        {
            if (options.HttpClientHandlerActions.TryGetValue(provider, out var actions))
            {
                actions.Add(configuration);
            }

            else
            {
                options.HttpClientHandlerActions[provider] = new(capacity: 1) { configuration };
            }
        });
    }

    /// <summary>
    /// Sets the contact address used in the "From" header that is attached
    /// to the backchannel HTTP requests sent to the authorization server.
    /// </summary>
    /// <param name="address">The mail address.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    public OpenIddictClientSystemNetHttpBuilder SetContactAddress(MailAddress address)
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address));
        }

        return Configure(options => options.ContactAddress = address);
    }

    /// <summary>
    /// Sets the contact address used in the "From" header that is attached
    /// to the backchannel HTTP requests sent to the authorization server.
    /// </summary>
    /// <param name="address">The mail address.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    public OpenIddictClientSystemNetHttpBuilder SetContactAddress(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new ArgumentException(SR.FormatID0366(nameof(address)), nameof(address));
        }

        return SetContactAddress(new MailAddress(address));
    }

    /// <summary>
    /// Replaces the default HTTP error policy used by the OpenIddict client services.
    /// </summary>
    /// <param name="policy">The HTTP Polly error policy.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    public OpenIddictClientSystemNetHttpBuilder SetHttpErrorPolicy(IAsyncPolicy<HttpResponseMessage> policy)
    {
        if (policy is null)
        {
            throw new ArgumentNullException(nameof(policy));
        }

        return Configure(options => options.HttpErrorPolicy = policy);
    }

    /// <summary>
    /// Sets the product information used in the "User-Agent" header that is attached
    /// to the backchannel HTTP requests sent to the authorization server.
    /// </summary>
    /// <param name="information">The product information.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    public OpenIddictClientSystemNetHttpBuilder SetProductInformation(ProductInfoHeaderValue information)
    {
        if (information is null)
        {
            throw new ArgumentNullException(nameof(information));
        }

        return Configure(options => options.ProductInformation = information);
    }

    /// <summary>
    /// Sets the product information used in the "User-Agent" header that is attached
    /// to the backchannel HTTP requests sent to the authorization server.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="version">The product version.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    public OpenIddictClientSystemNetHttpBuilder SetProductInformation(string name, string? version)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0345), nameof(name));
        }

        return SetProductInformation(new ProductInfoHeaderValue(name, version));
    }

    /// <summary>
    /// Sets the product information used in the user agent header that is attached
    /// to the backchannel HTTP requests sent to the authorization server based
    /// on the identity of the specified .NET assembly (name and version).
    /// </summary>
    /// <param name="assembly">The assembly from which the product information is created.</param>
    /// <returns>The <see cref="OpenIddictClientSystemNetHttpBuilder"/> instance.</returns>
    public OpenIddictClientSystemNetHttpBuilder SetProductInformation(Assembly assembly)
    {
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        return SetProductInformation(new ProductInfoHeaderValue(
            productName: assembly.GetName().Name!,
            productVersion: assembly.GetName().Version!.ToString()));
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString() => base.ToString();
}
