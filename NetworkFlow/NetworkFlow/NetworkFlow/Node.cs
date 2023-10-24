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

public class Node
{
    public required int Id { get; set; }
    public required List<Edge> Edges { get; set; }
}
