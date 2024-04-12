using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

namespace TaskManager.Controllers
{
    [SwaggerTag("comments")]
    [Route("api/comment/")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public CommentController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "create-comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Comment>> CreateComment([FromBody] CreateCommentScheme model)
        {

            var owner = await _context.Users
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            if (owner == null)
            {
                throw new Exception("Что то пошло очень не так");
            }

            var comment = new Comment()
            {
                Content = model.Text,
                CreatedAt = DateTime.UtcNow,
                Author = owner,
                ProductId = model.ProductId
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpGet(Name = "get-comments")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<Comment>>> GetComments([FromQuery] GetCommentsScheme model)
        {
            var comments = await _context.Comments
                .Skip(model.Start)
                .Take(model.End)
                .ToListAsync();

            return Ok(comments);
        }

        [HttpDelete("{id}", Name = "delete-comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Comment>> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return NotFound(new JsonResult("Комментарий не найден") { StatusCode = 404 });
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpPatch("{id}", Name = "update-comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Comment>> UpdateComment(Guid id, [FromBody] UpdateCommentSchema model)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
                return NotFound(new JsonResult("Коментарий не найден") { StatusCode = 401 });
            comment.Content = model.Text;

            _context.Update(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }
    }
}

