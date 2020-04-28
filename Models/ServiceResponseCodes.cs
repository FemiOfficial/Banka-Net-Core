namespace banka_net_core.Models
{
    public enum ServiceResponseCodes
    {
        Success = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        UnAuthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        ServerError = 500,
    }
}
