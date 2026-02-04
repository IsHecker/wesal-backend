namespace Wesal.Presentation.Authentication.Requests;

internal record struct EmailPasswordRequest(string Email, string Password);