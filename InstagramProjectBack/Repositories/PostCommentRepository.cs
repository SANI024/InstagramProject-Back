using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace InstagramProjectBack.Repositories
{
    public class PostCommentRepository : IPostCommentRepository
    {
        private readonly AppDbContext _context;

        public PostCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseDto<PostComment>> CreatePostCommentAsync(CreatePostCommentDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user == null)
            {
                return new BaseResponseDto<PostComment>
                {
                    Data = null,
                    Message = "User doesn't exist.",
                    Success = false
                };
            }

            var newPostComment = new PostComment
            {
                UserId = dto.UserId,
                Text = dto.Text,
                PostId = dto.PostId
            };

            _context.PostComments.Add(newPostComment);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<PostComment>
            {
                Data = newPostComment,
                Message = "Successfully created post comment.",
                Success = true
            };
        }

        public async Task<BaseResponseDto<List<PostComment>>> GetPostCommentsAsync()
        {
            var postComments = await _context.PostComments.ToListAsync();
            if (postComments.Count == 0)
            {
                return new BaseResponseDto<List<PostComment>>
                {
                    Data = null,
                    Message = "No post comments available.",
                    Success = false
                };
            }

            return new BaseResponseDto<List<PostComment>>
            {
                Data = postComments,
                Message = "Successfully retrieved post comments.",
                Success = true
            };
        }

        public async Task<BaseResponseDto<PostComment>> RemovePostCommentAsync(int userId, int postCommentId)
        {
            var postComment = await _context.PostComments
                .Include(pc => pc.Post)
                .FirstOrDefaultAsync(pc => pc.UserId == userId && pc.Id == postCommentId);

            if (postComment == null)
            {
                return new BaseResponseDto<PostComment>
                {
                    Success = false,
                    Data = null,
                    Message = "User can't delete post."
                };
            }

            _context.PostComments.Remove(postComment);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<PostComment>
            {
                Data = null,
                Message = "Successfully removed post.",
                Success = true
            };
        }

        public async Task<BaseResponseDto<PostComment>> UpdatePostCommentAsync(UpdatePostCommentDto dto)
        {
            var postComment = await _context.PostComments
                .FirstOrDefaultAsync(pc => pc.UserId == dto.UserId && pc.Id == dto.CommentId);

            if (postComment == null)
            {
                return new BaseResponseDto<PostComment>
                {
                    Success = false,
                    Message = "Cannot update the comment.",
                    Data = null
                };
            }

            postComment.Text = dto.Text;
            await _context.SaveChangesAsync();

            return new BaseResponseDto<PostComment>
            {
                Success = true,
                Message = "Comment updated.",
                Data = postComment
            };
        }
    }
}
