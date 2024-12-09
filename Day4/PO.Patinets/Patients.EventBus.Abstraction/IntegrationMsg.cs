using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patients.EventBus.Abstraction
{
    public  class IntegrationMsg
    {
        public string Type
        {
            get { 
                return this.GetType().Name;
            }
        }
    }
}
