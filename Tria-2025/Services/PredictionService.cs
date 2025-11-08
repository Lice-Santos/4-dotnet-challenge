using Microsoft.Extensions.ML;
using SmartApi.Web.Models;
using Tria_2025.Models;
using Tria_2025.Repository;

namespace SmartApi.Web.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _pool;
        private const string ModelName = "DefaultModelName";

        public PredictionService(PredictionEnginePool<ModelInput, ModelOutput> pool)
        {
            _pool = pool;
        }

        public ModelOutput Predict(ModelInput input)
        {
            return _pool.Predict(ModelName, input);
        }
    }
}