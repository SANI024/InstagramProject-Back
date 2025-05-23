using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public class PostCommentRepository : IPostCommentRepository
    {
        private readonly AppDbContext _context;
        public PostCommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public BaseResponseDto<PostComment> CreatePostComment(CreatePostCommentDto dto)
        {
            User UserExists = _context.Users.FirstOrDefault(u => u.Id == dto.UserId);
            if (UserExists == null)
            {
                return new BaseResponseDto<PostComment>
                {
                    Data = null,
                    Message = "User Doesn't Exists.",
                    Success = false,
                };
            }

            PostComment NewPostComment = new PostComment
            {
                UserId = dto.UserId,
                Text = dto.Text,
                PostId = dto.PostId
            };

            _context.PostComments.Add(NewPostComment);
            _context.SaveChanges();

            return new BaseResponseDto<PostComment>
            {
                Data = NewPostComment,
                Message = "Succesfully Created PostComment.",
                Success = true
            };
        }

        public BaseResponseDto<List<PostComment>> GetPostComments()
        {
            List<PostComment> postComments = _context.PostComments.ToList();
            if (postComments.Count == 0)
            {
                return new BaseResponseDto<List<PostComment>>
                {
                    Data = null,
                    Message = "no post comments avaiable.",
                    Success = false
                };
            }

            return new BaseResponseDto<List<PostComment>>
            {
                Data = postComments,
                Message = "Succesfully Created PostComment.",
                Success = true
            };
        }

        public BaseResponseDto<PostComment> RemovePostComment(int UserId, int PostCommentId)
        {
            User UserExists = _context.Users.FirstOrDefault(u => u.Id == UserId);
            if (UserExists == null)
            {
                return new BaseResponseDto<PostComment>
                {
                    Data = null,
                    Message = "User Doesn't Exists.",
                    Success = false,
                };
            }

            PostComment PostCommentExists = _context.PostComments.FirstOrDefault(pc => pc.Id == PostCommentId);
            if (PostCommentExists == null)
            {
                return new BaseResponseDto<PostComment>
                {
                    Data = null,
                    Message = "Post Doesn't Exists.",
                    Success = false,
                };
            }

            _context.PostComments.Remove(PostCommentExists);
            _context.SaveChanges();

            return new BaseResponseDto<PostComment>
            {
                Data = null,
                Message = "Succesfully removed post.",
                Success = true,
            };
        }

        public BaseResponseDto<PostComment> UpdatePostComment(UpdatePostCommentDto dto)
        {
            PostComment postComment = _context.PostComments.FirstOrDefault(pc => pc.UserId == dto.UserId && pc.PostId == dto.PostId);
            if (postComment == null)
            {
                return new BaseResponseDto<PostComment>
                {
                    Success = false,
                    Message = "Comment not found.",
                    Data = null
                };
            }
            postComment.Text = dto.Text;
            _context.SaveChanges();
            return new BaseResponseDto<PostComment>
            {
                Success = true,
                Message = "Comment updated.",
                Data = postComment
            };
        }
    }
}

