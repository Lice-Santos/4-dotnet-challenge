using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tria_2025.Connection;
using Tria_2025.Models;
using Tria_2025.Services;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Tria_2025.Repository;
using Tria_2025.Controllers;

namespace Tria_2025.Tests
{
    public class ReviewsControllerTests
    {
        // Cria um DbContext em memória para os testes
        private AppDbContext SetupDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        // Mock do serviço de predição
        private Mock<IPredictionService> SetupPredictionServiceMock(bool predictedResult, float score)
        {
            var fakeModelOutput = new ModelOutput
            {
                Prediction = predictedResult,
                Score = score
            };

            var mockService = new Mock<IPredictionService>();
            mockService
                .Setup(s => s.Predict(It.IsAny<ModelInput>()))
                .Returns(fakeModelOutput);

            return mockService;
        }

        [Fact]
        public async Task PostReview_WithPositiveText_ShouldSaveAsPositive()
        {
            var dbContext = SetupDbContext();
            var mockService = SetupPredictionServiceMock(predictedResult: true, score: 0.95f);
            var controller = new ReviewsController(dbContext, mockService.Object);

            var inputDto = new CreateReviewDto { Text = "The motorcycle is in perfect condition!" };

            var actionResult = await controller.PostReview(inputDto);

            // Verifica se o resultado é 201 Created
            Assert.IsType<CreatedAtActionResult>(actionResult.Result);

            // Verifica se a review foi salva no banco em memória
            var savedReview = await dbContext.Reviews.FirstOrDefaultAsync();
            Assert.NotNull(savedReview);
            Assert.Equal("Positivo", savedReview.PredictedSentiment);
        }

        [Fact]
        public async Task PostReview_WithNegativeText_ShouldSaveAsNegative()
        {
            var dbContext = SetupDbContext();
            var mockService = SetupPredictionServiceMock(predictedResult: false, score: 0.1f);
            var controller = new ReviewsController(dbContext, mockService.Object);

            var inputDto = new CreateReviewDto { Text = "Engine is misfiring and releasing smoke." };

            var actionResult = await controller.PostReview(inputDto);

            Assert.IsType<CreatedAtActionResult>(actionResult.Result);

            var savedReview = await dbContext.Reviews.FirstOrDefaultAsync();
            Assert.NotNull(savedReview);
            Assert.Equal("Negativo", savedReview.PredictedSentiment);
        }

        [Fact]
        public async Task GetReview_WithNonExistentId_ShouldReturnNotFound()
        {
            var dbContext = SetupDbContext();
            var mockService = SetupPredictionServiceMock(true, 0.95f);
            var controller = new ReviewsController(dbContext, mockService.Object);

            var actionResult = await controller.GetReview(999); // ID que não existe

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetReviews_ShouldReturnAllReviews()
        {
            var dbContext = SetupDbContext();
            var mockService = SetupPredictionServiceMock(true, 0.95f);
            var controller = new ReviewsController(dbContext, mockService.Object);

            // Adiciona algumas reviews manualmente
            dbContext.Reviews.Add(new Review { Text = "Test 1", PredictedSentiment = "Positivo", SentimentScore = 0.9f });
            dbContext.Reviews.Add(new Review { Text = "Test 2", PredictedSentiment = "Negativo", SentimentScore = 0.2f });
            await dbContext.SaveChangesAsync();

            var result = await controller.GetReviews();

            var reviews = result.Value.ToList();
            Assert.Equal(2, reviews.Count);
        }
    }
}
