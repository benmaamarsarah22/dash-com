using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

public class ActiviteReunionExterneBusinessService
    : GenericBusinessService<ActiviteReunionExterne, int>
{
    public ActiviteReunionExterneBusinessService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {



    }

  

    public override Expression<Func<ActiviteReunionExterne, object>>[] GetDefaultLoadProperties()
    {

        Expression<Func<ActiviteReunionExterne, object>> loadActivity = x => x.Activite;

        return new Expression<Func<ActiviteReunionExterne, object>>[] { loadActivity };
    }

}