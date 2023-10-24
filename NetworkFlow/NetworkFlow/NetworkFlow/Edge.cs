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

public class Edge
{
    public required Node From { get; set; }
    public required Node To { get; set; }
    public required int Capacity { get; set; }
    public bool IsReverse { get; set; } = false;
    public int Flow { get; set; } = 0;
    public int ResidualCapacity => IsReverse ? Flow : Capacity - Flow;
    public string Content { get; set; }
}
