using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Business
{
    public class ActiviteRadioBusinessService
        : GenericBusinessService<ActiviteRadio, int>
    {
        public ActiviteRadioBusinessService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        

        public override Expression<Func<ActiviteRadio, object>>[] GetDefaultLoadProperties()
        {

            Expression<Func<ActiviteRadio, object>> loadActivity = x => x.Activite;

            return new Expression<Func<ActiviteRadio, object>>[] { loadActivity };
        }
    }
}
