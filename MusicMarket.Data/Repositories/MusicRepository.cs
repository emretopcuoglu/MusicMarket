using Microsoft.EntityFrameworkCore;
using MusicMarket.Core.Model;
using MusicMarket.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicMarket.Data.Configurations.Repositories
{
    public class MusicRepository : Repository<Music>, IMusicRepository
    {
        public MusicRepository(MusicMarketDbContext context) : base(context)
        {
        
        }
        private MusicMarketDbContext MusicMarketDbContext
        {
            get { return _context as MusicMarketDbContext; }
        }

        public async Task<IEnumerable<Music>> GetAllWithArtistAsync()
        {
            return await MusicMarketDbContext.Musics
                .Include(m => m.Artist)
                .ToListAsync();
        }

        public async Task<Music> GetWithArtistByIdAsync(int id)
        {
            return await MusicMarketDbContext.Musics
                .Include(m => m.Artist)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Music>> GetAllWithArtistByArtistIdAsync(int artistId)
        {
            return await MusicMarketDbContext.Musics
                .Include(m => m.Artist)
                .Where(m => m.ArtistId == artistId)
                .ToListAsync();
        }
    }
}
