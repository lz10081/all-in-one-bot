using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAIO
{

    /// <summary>
    /// Use this interface to interact with the task manager.
    /// </summary>
    public interface ITask
    {

        void Run();

        void Callback();

    }
}
