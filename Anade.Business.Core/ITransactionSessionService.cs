using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public interface ITransactionSessionService<T>
    {
        //Task<string> BeginSessionAsync(T initialData, TimeSpan timeout);
        //Task<bool> CommitAsync(string sessionId, Action<T> finalUpdate);
        //Task RollbackAsync(string sessionId);
        //Task CleanupExpiredSessionsAsync();
    }
}
