using JasperFx.Core.Reflection;

namespace Wolverine.Runtime;

public partial class WolverineRuntime : IAsyncDisposable
{
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (!_hasStopped)
        {
            await StopAsync(CancellationToken.None);
        }

        Replies.Dispose();

        await Endpoints.DisposeAsync();

        await Options.Transports.As<IAsyncDisposable>().DisposeAsync();

        DurabilitySettings.Cancel();

        if (_durableScheduledJobs != null)
        {
            await _durableScheduledJobs.StopAsync(CancellationToken.None);
        }
        ScheduledJobs.Dispose();
    }
}