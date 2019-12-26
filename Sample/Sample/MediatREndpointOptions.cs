using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample
{
    public class MediatREndpointOptions
    {
        public IEnumerable<Type> HandlerTypes { get; set; }
    }
}
