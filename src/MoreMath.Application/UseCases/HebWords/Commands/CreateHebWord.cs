using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Extensions;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.HebWords.Commands;

public record CreateHebWordCommand(
     string Shoresh,
     string Niqqud,
     string NoNiqqud,
     string ExtraForm,
     string Spelling,
     int StressLetter,
     string Translation,
     string Notes,
     string ExtraInfo,
     string[] Tags) : IRequest<ResultWrap<int>>;



public class CreateHebWordHandler(IAppDbContext context):
    AbstractHandler<CreateHebWordCommand, ResultWrap<int>>(context)
{
    public override async Task<ResultWrap<int>> Handle(
        CreateHebWordCommand command, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags.Where(t => command.Tags.Contains(t.TagName)).ToListAsync(cancellationToken);


        HebWord hebWord = new()
        {
            Shoresh = command.Shoresh,
            Niqqud = command.Niqqud,
            NoNiqqud = command.NoNiqqud.RemoveNiqqud(),
            ExtraForm = command.ExtraForm,
            Spelling = command.Spelling,
            StressLetter = command.StressLetter,
            Translation = command.Translation,
            Notes = command.Notes,
            ExtraInfo = command.ExtraInfo,
            Tags = tags 
        };

        await _context.HebWords.AddAsync(hebWord, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return hebWord.Id == 0
            ? ResultWrap.Failure(new Error("HebWord.CreateError", "HebWord was not created"))
            : ResultWrap<int>.Success(hebWord.Id);
    }
}
