public class Worker : BackgroundService
{
    public const string Quote = "Anyone who doesn't believe in miracles is not a realist.";
    public const string Author = $"{Quote} David Ben-Gurion";

    private readonly ILogger<Worker> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Worker"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // default Tuple deconstruction
        var (x, y) = default((int, bool));

        _logger.LogInformation("{x}, {y}, {message}", x, y, Author);

        var count = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", new Schedule(DateTimeOffset.Now));
            try
            {
                // lambda target typing to Delegate
                ExecuteLambda(() => count++);
                await Task.Delay(1000, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }

    private void ExecuteLambda(Delegate action)
    {
        var ticks = DateTimeOffset.Now.Ticks;
        var bytes = BitConverter.GetBytes(ticks);
        var id = Convert.ToBase64String(bytes).Replace("+", string.Empty).Replace("/", string.Empty).TrimEnd('=').ToLower();

        var result = action?.DynamicInvoke();
        _logger.LogInformation("Count: {counter}: {id}", result, id);
    }
}

/// <summary>
/// record struct.
/// </summary>
record struct Schedule(DateTimeOffset time);
