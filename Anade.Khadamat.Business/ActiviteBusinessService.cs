using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

public class ActiviteBusinessService : GenericBusinessService<Activite, int>
{
    private readonly MoisClotureBusinessService _moisService;

    public ActiviteBusinessService(
        IUnitOfWork unitOfWork,
        MoisClotureBusinessService moisService)
        : base(unitOfWork)
    {
        _moisService = moisService;
    }

    public override Expression<Func<Activite, object>>[] GetDefaultLoadProperties()
    {
        Expression<Func<Activite, object>> loadAgence = x => x.AgenceWilaya;
        Expression<Func<Activite, object>> loadType = x => x.TypeActivite;
        return new Expression<Func<Activite, object>>[] { loadAgence, loadType };
    }

    protected override void OnAdding(Activite entity)
    {
        _moisService.AssertMoisOuvert(entity.DateActivite.Year, entity.DateActivite.Month);
        base.OnAdding(entity);
    }

    protected override void OnUpdating(Activite entity)
    {
        _moisService.AssertMoisOuvert(entity.DateActivite.Year, entity.DateActivite.Month);
        base.OnUpdating(entity);
    }

    protected override void OnDeleting(Activite entity)
    {
        _moisService.AssertMoisOuvert(entity.DateActivite.Year, entity.DateActivite.Month);
        base.OnDeleting(entity);
    }

    /// <summary>Convenience helper for controllers to show a lock indicator in the UI.</summary>
    public bool EstMoisCloture(DateTime date) =>
        _moisService.EstCloture(date.Year, date.Month);
}
