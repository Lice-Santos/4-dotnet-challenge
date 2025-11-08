
using SmartApi.Web.Models;
using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public interface IPredictionService
    {
        ModelOutput Predict(ModelInput input);
    }
}