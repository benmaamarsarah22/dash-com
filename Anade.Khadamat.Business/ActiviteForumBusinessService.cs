using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

public class ActiviteForumBusinessService
    : GenericBusinessService<ActiviteForum, int>
{
    private readonly MoisClotureBusinessService _moisService;

    public ActiviteForumBusinessService(
        IUnitOfWork unitOfWork,
        MoisClotureBusinessService moisService)
        : base(unitOfWork)
    {
        _moisService = moisService;
    }

    public override Expression<Func<ActiviteForum, object>>[] GetDefaultLoadProperties()
    {
        Expression<Func<ActiviteForum, object>> loadActivite = x => x.Activite;
        Expression<Func<ActiviteForum, object>> loadAgencyW = x => x.Activite.AgenceWilaya;
        return new Expression<Func<ActiviteForum, object>>[] { loadActivite, loadAgencyW };
    }

    protected override void OnAdding(ActiviteForum entity)
    {
        AssertParentMoisOuvert(entity.ActiviteId);
        base.OnAdding(entity);
    }

    protected override void OnUpdating(ActiviteForum entity)
    {
        AssertParentMoisOuvert(entity.ActiviteId);
        base.OnUpdating(entity);
    }

    protected override void OnDeleting(ActiviteForum entity)
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
