using FluentValidation;
using MusicMarket.Api.DTO;

namespace MusicMarket.Api.Validator
{
    public class SaveMusicResourceValidator : AbstractValidator<SaveMusicDTO>
    {
        public SaveMusicResourceValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(m => m.ArtistId)
                .NotEmpty()
                .WithMessage("'Artist Id' must not be 0.");

            // Generic olarak model classı verilip, ctor içerisinde kuralları oluşturulur.
            // WithMessage ile her kuralla ilgili tek tek uyulmadığı durumdan kaynaklı hata mesajları yazılabilir.
        }
    }
}
