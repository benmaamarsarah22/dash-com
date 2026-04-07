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
    public class ActivitePresseBusinessService
    : GenericBusinessService<ActivitePresse, int>
    {
        public ActivitePresseBusinessService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

       

        public override Expression<Func<ActivitePresse, object>>[] GetDefaultLoadProperties()
        {

            Expression<Func<ActivitePresse, object>> loadActivity = x => x.Activite;

            return new Expression<Func<ActivitePresse, object>>[] { loadActivity };
        }
    }
}
