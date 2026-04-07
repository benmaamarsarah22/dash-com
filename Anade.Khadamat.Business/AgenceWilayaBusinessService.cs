using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class AgenceWilayaBusinessService : GenericBusinessService<AgenceWilaya, int>
    {
        public AgenceWilayaBusinessService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override Expression<Func<AgenceWilaya, object>>[] GetDefaultLoadProperties()
        {

           Expression<Func<AgenceWilaya, object>> loadActivities = x => x.Activities;
            return new Expression<Func<AgenceWilaya, object>>[] {loadActivities};
        }
    }
}
