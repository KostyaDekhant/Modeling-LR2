using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeling_LR2
{
    public class Detail
{
    public int Index { get; set; }
    public double Pi1 { get; set; }
    public double Pi2 { get; set; }
    public double Lambda { get; set; }

    public Detail(int index, double pi1, double pi2)
    {
        Index = index;
        Pi1 = pi1;
        Pi2 = pi2;
        Lambda = Pi2 - Pi1;
    }
}
}
