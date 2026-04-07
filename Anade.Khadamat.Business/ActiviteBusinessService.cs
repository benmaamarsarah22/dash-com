using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;

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

    //   AJOUT ACTIVITE
    protected override void OnAdding(Activite entity)
    {
        //   Vérifier que le mois est OUVERT
        _moisService.VerifierMoisOuvert(entity.DateActivite);

        base.OnAdding(entity);
    }

    //   MODIFICATION
    protected override void OnUpdating(Activite entity)
    {
        if (_moisService.EstCloture(entity.DateActivite.Year, entity.DateActivite.Month))
        {
            throw new BusinessException("⛔ Impossible de modifier une activité dans un mois clôturé !");
        }

        base.OnUpdating(entity);
    }

    //  SUPPRESSION
    protected override void OnDeleting(Activite entity)
    {
        if (_moisService.EstCloture(entity.DateActivite.Year, entity.DateActivite.Month))
        {
            throw new BusinessException("⛔ Impossible de supprimer une activité dans un mois clôturé !");
        }

        base.OnDeleting(entity);
    }

    #region helper

    public bool EstMoisCloture(DateTime date)
    {
        return _moisService.EstCloture(date.Year, date.Month);
    }

    #endregion
}