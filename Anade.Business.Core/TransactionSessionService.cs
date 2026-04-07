using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public class TransactionSessionService<T>:ITransactionSessionService<T>
    {
        //private readonly ConcurrentDictionary<string, (T Data, DateTime Expiry)> _sessions;
        //private readonly ILogger<TransactionSessionService<T>> _logger;

        //public TransactionSessionService()
        //{
        //    _sessions = new ConcurrentDictionary<string, (T, DateTime)>();
        //}

        //public async Task<string> BeginSessionAsync(T initialData, TimeSpan timeout)
        //{
        //    var sessionId = Guid.NewGuid().ToString();
        //    var expiry = DateTime.UtcNow.Add(timeout);

        //    _sessions.TryAdd(sessionId, (initialData, expiry));

        //    // Start timeout tracking
        //    _ = Task.Run(async () =>
        //    {
        //        await Task.Delay(timeout);
        //        if (_sessions.TryGetValue(sessionId, out _))
        //        {
        //            await RollbackAsync(sessionId);
        //        }
        //    });

        //    return sessionId;
        //}

        //public Task CleanupExpiredSessionsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> CommitAsync(string sessionId, Action<T> finalUpdate)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task RollbackAsync(string sessionId)
        //{
        //    if (_sessions.TryRemove(sessionId, out var session))
        //    {
        //        // Implement your rollback logic here
        //        _logger.LogInformation($"Rolled back session {sessionId}");
        //        await OnRollback(session.Data);
        //    }
        //}

        //protected virtual Task OnRollback(T data)
        //{
        //    // Override this to implement specific rollback logic
        //    return Task.CompletedTask;
        //}
    }
}
