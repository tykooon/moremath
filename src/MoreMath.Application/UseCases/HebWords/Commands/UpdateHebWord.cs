using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.HebWords.Commands;

public record UpdateHebWordCommand(
    int Id,
    string? Shoresh,
    string? Niqqud,
    string? NoNiqqud,
    string? ExtraForm,
    string? Spelling,
    int? StressLetter,
    string? Translation,
    string? Notes,
    string? ExtraInfo) : IRequest<ResultWrap>;

public class  UpdateHebWordHandler(IAppDbContext context):
    AbstractHandler<UpdateHebWordCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(UpdateHebWordCommand command, CancellationToken cancellationToken)
    {
        var hebWord = await _context.HebWords.FindAsync(command.Id);

        if (hebWord == null)
        {
            return ResultWrap.Failure(new Error("HebWord.NotFound", "Failed to update HebWord with given Id. HebWord wasn't found."));
        }

        hebWord.Shoresh = command.Shoresh ?? hebWord.Shoresh;
        hebWord.Niqqud = command.Niqqud ?? hebWord.Niqqud;
        hebWord.NoNiqqud = command.NoNiqqud ?? hebWord.NoNiqqud;
        hebWord.ExtraForm = command.ExtraForm ?? hebWord.ExtraForm;
        hebWord.Spelling = command.Spelling ?? hebWord.Spelling;
        hebWord.StressLetter = command.StressLetter ?? hebWord.StressLetter;
        hebWord.Translation = command.Translation ?? hebWord.Translation;
        hebWord.Notes = command.Notes ?? hebWord.Notes;
        hebWord.ExtraInfo = command.ExtraInfo ?? hebWord.ExtraInfo;

        _context.HebWords.Update(hebWord);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
