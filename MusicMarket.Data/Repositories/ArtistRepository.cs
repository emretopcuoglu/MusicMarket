using Microsoft.EntityFrameworkCore;
using MusicMarket.Core.Model;
using MusicMarket.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicMarket.Data.Configurations.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(MusicMarketDbContext context) : base(context)
        {
            
        }
        private MusicMarketDbContext MusicMarketDbContext
        {
            get { return _context as MusicMarketDbContext; }
        }
        public async Task<IEnumerable<Artist>> GetAllWithMusicAsync()
        {
            return await MusicMarketDbContext.Artists
                .Include(a => a.Musics)
                .ToListAsync();
        }

        public Task<Artist> GetWithMusicByIdAsync(int id)
        {
            return MusicMarketDbContext.Artists
                .Include(a => a.Musics)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}
