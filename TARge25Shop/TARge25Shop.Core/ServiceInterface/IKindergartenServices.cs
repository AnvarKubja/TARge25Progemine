using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<Kindergarten> Create(KindergartenDto dto);
        Task<Kindergarten> Update(KindergartenDto dto);
        Task<Kindergarten> DetailAsync(KindergartenDto dto);
        Task<Kindergarten> Delete(KindergartenDto dto);
    }
}
