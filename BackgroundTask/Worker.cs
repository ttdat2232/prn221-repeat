
namespace BackgroundTask
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private const int UPDATE_TIME_INTERVAL = 5;
        private HttpClient httpClient;
        private int taskRunCount = 0;
        private const string END_POINT = @"https://localhost:7225/api/ClubActivity/Status";

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            httpClient = httpClientFactory.CreateClient();
            logger.LogInformation(httpClient.ToString());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await httpClient.GetAsync(END_POINT, stoppingToken).ContinueWith(t =>
                {
                    if (t.Result.StatusCode == System.Net.HttpStatusCode.OK)
                        logger.LogInformation($"Background task has finished: {++taskRunCount}");
                });
                await Task.Delay(TimeSpan.FromMinutes(UPDATE_TIME_INTERVAL), stoppingToken);
            } while (!stoppingToken.IsCancellationRequested);
        }
    }
}