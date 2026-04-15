using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class ActiviteJourneeInfoBusinessService
    : GenericBusinessService<ActiviteJourneeInfo, int>
{
    private readonly MoisClotureBusinessService _moisService;

    public ActiviteJourneeInfoBusinessService(
        IUnitOfWork unitOfWork,
        MoisClotureBusinessService moisService)
        : base(unitOfWork)
    {
        _moisService = moisService;
    }

    public override Expression<Func<ActiviteJourneeInfo, object>>[] GetDefaultLoadProperties()
    {
        Expression<Func<ActiviteJourneeInfo, object>> loadActivite = x => x.Activite;
        Expression<Func<ActiviteJourneeInfo, object>> loadAgencyW = x => x.Activite.AgenceWilaya;
        return new Expression<Func<ActiviteJourneeInfo, object>>[] { loadActivite, loadAgencyW };
    }

    protected override void OnAdding(ActiviteJourneeInfo entity)
    {
        AssertParentMoisOuvert(entity.ActiviteId);
        base.OnAdding(entity);
    }

    protected override void OnUpdating(ActiviteJourneeInfo entity)
    {
        AssertParentMoisOuvert(entity.ActiviteId);
        base.OnUpdating(entity);
    }

    protected override void OnDeleting(ActiviteJourneeInfo entity)
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
