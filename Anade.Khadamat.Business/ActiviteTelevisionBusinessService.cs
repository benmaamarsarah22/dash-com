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
    public class ActiviteTelevisionBusinessService
       : GenericBusinessService<ActiviteTelevision, int>
    {
        public ActiviteTelevisionBusinessService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        

        public override Expression<Func<ActiviteTelevision, object>>[] GetDefaultLoadProperties()
        {

            Expression<Func<ActiviteTelevision, object>> loadActivity = x => x.Activite;

            return new Expression<Func<ActiviteTelevision, object>>[] { loadActivity };
        }
    }
}
