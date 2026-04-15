using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

public class ActiviteReunionExterneBusinessService
    : GenericBusinessService<ActiviteReunionExterne, int>
{
    private readonly MoisClotureBusinessService _moisService;

    public ActiviteReunionExterneBusinessService(
        IUnitOfWork unitOfWork,
        MoisClotureBusinessService moisService)
        : base(unitOfWork)
    {
        _moisService = moisService;
    }

    public override Expression<Func<ActiviteReunionExterne, object>>[] GetDefaultLoadProperties()
    {
        Expression<Func<ActiviteReunionExterne, object>> loadActivite = x => x.Activite;
        Expression<Func<ActiviteReunionExterne, object>> loadAgencyW = x => x.Activite.AgenceWilaya;
        return new Expression<Func<ActiviteReunionExterne, object>>[] { loadActivite, loadAgencyW };
        
    }

    protected override void OnAdding(ActiviteReunionExterne entity)
    {
        AssertParentMoisOuvert(entity.ActiviteId);
        base.OnAdding(entity);
    }

    protected override void OnUpdating(ActiviteReunionExterne entity)
    {
        AssertParentMoisOuvert(entity.ActiviteId);
        base.OnUpdating(entity);
    }

    protected override void OnDeleting(ActiviteReunionExterne entity)
    {
        AssertParentMoisOuvert(entity.ActiviteId);
        base.OnDeleting(entity);
    }

    private void AssertParentMoisOuvert(int activiteId)
    {
        var activite = _unitOfWork.GetRepository<Activite, int>().GetById(activiteId);
        if (activite != null)
            _moisService.AssertMoisOuvert(activite.DateActivite.Year, activite.DateActivite.Month);
    }
}
