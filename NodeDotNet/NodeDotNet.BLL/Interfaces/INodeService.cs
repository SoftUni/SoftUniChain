using NodeDotNet.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeDotNet.BLL.Interfaces
{
    public interface INodeService
    {
        NodeInfoVM GetInfo();
    }
}
