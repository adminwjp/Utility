//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Utility.Configuration.Ef
//{
//     using Microsoft.EntityFrameworkCore;
//     using Microsoft.Extensions.Configuration;

//    public class EFConfigurationSource : IConfigurationSource
//    {
//        private readonly Action<DbContextOptionsBuilder> _optionsAction;

//        public EFConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
//        {
//            _optionsAction = optionsAction;
//        }

//        public IConfigurationProvider Build(IConfigurationBuilder builder)
//        {
//            return new EFConfigurationProvider(_optionsAction);
//        }
//    }
//}
