using MusicMarket.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace MusicMarket.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IMusicRepository Musics { get; }
        IArtistRepository Artists { get; }
        Task<int> CommitAsync();
    }
}
