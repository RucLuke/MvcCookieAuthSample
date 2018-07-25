using System.Threading.Tasks;

namespace MyPipeline
{
    public delegate Task RequestDelegate(Context boundObject);
}
