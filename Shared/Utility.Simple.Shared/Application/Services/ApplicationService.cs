using Utility.Domain.Services;

namespace Utility.Application.Services
{
    /// <summary>
    /// application service interface implement
    /// </summary>
    public  class ApplicationService:DomainService,IApplicationService
    {
        /// <summary>
        /// no param application service  constractor 
        /// </summary>
        public ApplicationService() :base(){ }
    }
}
