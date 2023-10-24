using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkFlow;

public interface INetworkFlowable
{
    public IEnumerable<Edge>? FindAugmentingPath(Graph graph);
    public Graph CreateResidualNetwork();
    public Graph FordFulkerson();
}
