using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class ActiviteTelevisionBusinessService
        : GenericBusinessService<ActiviteTelevision, int>
    {
        private readonly MoisClotureBusinessService _moisService;

        public ActiviteTelevisionBusinessService(
            IUnitOfWork unitOfWork,
            MoisClotureBusinessService moisService)
            : base(unitOfWork)
        {
            _moisService = moisService;
        }

        public override Expression<Func<ActiviteTelevision, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<ActiviteTelevision, object>> loadActivite = x => x.Activite;
            Expression<Func<ActiviteTelevision, object>> loadAgencyW = x => x.Activite.AgenceWilaya;
            return new Expression<Func<ActiviteTelevision, object>>[] { loadActivite, loadAgencyW };
             
        }

        protected override void OnAdding(ActiviteTelevision entity)
        {
            AssertParentMoisOuvert(entity.ActiviteId);
            base.OnAdding(entity);
        }

        protected override void OnUpdating(ActiviteTelevision entity)
        {
            AssertParentMoisOuvert(entity.ActiviteId);
            base.OnUpdating(entity);
        }

        protected override void OnDeleting(ActiviteTelevision entity)
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
}

