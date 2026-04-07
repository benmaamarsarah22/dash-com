using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class ActiviteJourneeInfoBusinessService
    : GenericBusinessService<ActiviteJourneeInfo, int>
{
    public ActiviteJourneeInfoBusinessService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    

    
    }
    public override Expression<Func<ActiviteJourneeInfo, object>>[] GetDefaultLoadProperties()
    {
        Expression<Func<ActiviteJourneeInfo, object>> loadActivity = x => x.Activite;

        return new Expression<Func<ActiviteJourneeInfo, object>>[] { loadActivity };
    }
}