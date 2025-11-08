using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartApi.Web.Models;
using SmartApi.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.Connection;
using Tria_2025.Models;
using Tria_2025.Repository;

namespace Tria_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPredictionService _predictionService;

        /// <summary>
        /// Construtor do ReviewsController.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        /// <param name="predictionService">Serviço para predição de sentimentos</param>
        public ReviewsController(AppDbContext context, IPredictionService predictionService)
        {
            _context = context;
            _predictionService = predictionService;
        }

        /// <summary>
        /// Retorna todas as reviews cadastradas.
        /// </summary>
        /// <returns>Lista de reviews</returns>
        /// <response code="200">Lista retornada com sucesso</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        /// <summary>
        /// Retorna uma review pelo seu ID.
        /// </summary>
        /// <param name="id">ID da review</param>
        /// <returns>A review correspondente ao ID informado</returns>
        /// <response code="200">Review encontrada</response>
        /// <response code="404">Review não encontrada</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        /// <summary>
        /// Cria uma nova review e realiza a predição de sentimento.
        /// </summary>
        /// <param name="reviewDto">Dados da review a ser criada</param>
        /// <returns>Review criada com predição de sentimento</returns>
        /// <response code="201">Review criada com sucesso</response>
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(CreateReviewDto reviewDto)
        {
            var modelInput = new ModelInput { Text = reviewDto.Text };

            // Realiza a predição de sentimento usando o serviço
            var prediction = _predictionService.Predict(modelInput);

            var review = new Review
            {
                Text = reviewDto.Text,
                PredictedSentiment = prediction.Prediction ? "Positivo" : "Negativo",
                SentimentScore = prediction.Score
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        /// <summary>
        /// Remove uma review pelo seu ID.
        /// </summary>
        /// <param name="id">ID da review a ser removida</param>
        /// <response code="204">Review removida com sucesso</response>
        /// <response code="404">Review não encontrada</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
