using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class ActivitePresseBusinessService
        : GenericBusinessService<ActivitePresse, int>
    {
        private readonly MoisClotureBusinessService _moisService;

        public ActivitePresseBusinessService(
            IUnitOfWork unitOfWork,
            MoisClotureBusinessService moisService)
            : base(unitOfWork)
        {
            _moisService = moisService;
        }

        public override Expression<Func<ActivitePresse, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<ActivitePresse, object>> loadActivite = x => x.Activite;
            return new Expression<Func<ActivitePresse, object>>[] { loadActivite };
        }

        protected override void OnAdding(ActivitePresse entity)
        {
            AssertParentMoisOuvert(entity.ActiviteId);
            base.OnAdding(entity);
        }

        protected override void OnUpdating(ActivitePresse entity)
        {
            AssertParentMoisOuvert(entity.ActiviteId);
            base.OnUpdating(entity);
        }

        protected override void OnDeleting(ActivitePresse entity)
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

