using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleCommentsQuery(int id) : IRequest<ResultWrap<IEnumerable<CommentDto>>>;

public class GetArticleCommentsHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticleCommentsQuery, ResultWrap<IEnumerable<CommentDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<CommentDto>>> Handle(GetArticleCommentsQuery request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(request.id);

        if(article == null) 
        {
            ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var comments = await _unitOfWork.CommentRepo.GetFilteredAsync(c => c.ArticleId == article!.Id);

        return ResultWrap<IEnumerable<CommentDto>>.Success(comments.Select(a => a.ToDto()));
    }
}
