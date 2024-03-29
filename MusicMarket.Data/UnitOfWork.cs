﻿using MusicMarket.Core;
using MusicMarket.Core.Repositories;
using MusicMarket.Data.Configurations.Repositories;
using System.Threading.Tasks;

namespace MusicMarket.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MusicMarketDbContext _context;

        private MusicRepository _musicRepository;
        private ArtistRepository _artistRepository;

        public UnitOfWork(MusicMarketDbContext context)
        {
            _context = context;
        }

        public IMusicRepository Musics => _musicRepository = _musicRepository ?? new MusicRepository(_context);

        public IArtistRepository Artists => _artistRepository = _artistRepository ?? new ArtistRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
