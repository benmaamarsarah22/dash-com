using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

public class ActiviteForumBusinessService
    : GenericBusinessService<ActiviteForum, int>
{
    public ActiviteForumBusinessService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {



    }
 

    public override Expression<Func<ActiviteForum, object>>[] GetDefaultLoadProperties()
    {

        Expression<Func<ActiviteForum, object>> loadActivity = x => x.Activite;

        return new Expression<Func<ActiviteForum, object>>[] { loadActivity };
    }

}