using System.Net.Http.Json;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication.Models;

namespace Bookify.Infrastructure.Authentication;

internal sealed class AuthenticationService(HttpClient httpClient) : IAuthenticationService
{
    private const string PASSWORD_CREDENTIAL_TYPE = "password";

    public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials =
        [
            new CredentialsRepresentationModel
            {
                Value = password,
                Temporary = false,
                Type = PASSWORD_CREDENTIAL_TYPE
            }
        ];

        var response = await httpClient.PostAsJsonAsync(
            "users",
            userRepresentationModel,
            cancellationToken
        );

        return ExtractIdentityIdFromLocationHandler(response);
    }

    private string ExtractIdentityIdFromLocationHandler(HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        var locationHandler = httpResponseMessage.Headers.Location?.PathAndQuery;
        if (locationHandler is null)
        {
            throw new InvalidOperationException("Location handler can't be null.");
        }

        var userSegmentValueIndex = locationHandler.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase
        );

        var userIdentityId = locationHandler.Substring(
            userSegmentValueIndex + usersSegmentName.Length
        );

        return userIdentityId;
    }
}