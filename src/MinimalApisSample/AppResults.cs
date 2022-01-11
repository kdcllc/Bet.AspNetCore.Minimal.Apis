public static class AppResults
{
    public static IResult NotFound() => Results.StatusCode(404);

    public static IResult Ok() => Results.StatusCode(200);

    public static IResult Status(int statusCode) => Results.StatusCode(statusCode);

    public static OkResult<T> Ok<T>(T value) => new(value);

    public static CreatedAtRouteResult<T> CreatedAt<T>(T value, string endpointName, object values) => new(value, endpointName, values);

    public class OkResult<T> : IResult
    {
        private readonly T _value;

        public OkResult(T value)
        {
            _value = value;
        }

        public Task ExecuteAsync(HttpContext httpContext)
        {
            return httpContext.Response.WriteAsJsonAsync(_value);
        }
    }

    public class CreatedAtRouteResult<T> : IResult
    {
        private readonly T _value;
        private readonly string _endpointName;
        private readonly object _values;

        public CreatedAtRouteResult(T value, string endpointName, object values)
        {
            _value = value;
            _endpointName = endpointName;
            _values = values;
        }

        public Task ExecuteAsync(HttpContext httpContext)
        {
            var linkGenerator = httpContext.RequestServices.GetRequiredService<LinkGenerator>();

            httpContext.Response.Headers.Add("Location", linkGenerator.GetPathByName(_endpointName, _values));

            return httpContext.Response.WriteAsJsonAsync(_value);
        }
    }
}
