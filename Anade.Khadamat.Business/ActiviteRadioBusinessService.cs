using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class ActiviteRadioBusinessService
        : GenericBusinessService<ActiviteRadio, int>
    {
        private readonly MoisClotureBusinessService _moisService;

        public ActiviteRadioBusinessService(
            IUnitOfWork unitOfWork,
            MoisClotureBusinessService moisService)
            : base(unitOfWork)
        {
            _moisService = moisService;
        }

        public override Expression<Func<ActiviteRadio, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<ActiviteRadio, object>> loadActivite = x => x.Activite;
            return new Expression<Func<ActiviteRadio, object>>[] { loadActivite };
        }

        protected override void OnAdding(ActiviteRadio entity)
        {
            AssertParentMoisOuvert(entity.ActiviteId);
            base.OnAdding(entity);
        }

        protected override void OnUpdating(ActiviteRadio entity)
        {
            AssertParentMoisOuvert(entity.ActiviteId);
            base.OnUpdating(entity);
        }

        protected override void OnDeleting(ActiviteRadio entity)
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

