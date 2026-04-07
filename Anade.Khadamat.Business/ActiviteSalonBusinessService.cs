using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

public class ActiviteSalonBusinessService
    : GenericBusinessService<ActiviteSalon, int>
{
    public ActiviteSalonBusinessService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {



    }
    public override Expression<Func<ActiviteSalon, object>>[] GetDefaultLoadProperties()
    {

        Expression<Func<ActiviteSalon, object>> loadActivity = x => x.Activite;

        return new Expression<Func<ActiviteSalon, object>>[] { loadActivity };
    }

}