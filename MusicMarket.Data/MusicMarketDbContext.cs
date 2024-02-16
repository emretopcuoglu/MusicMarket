using Microsoft.EntityFrameworkCore;
using MusicMarket.Core.Model;
using MusicMarket.Data.Configurations;

namespace MusicMarket.Data
{
    public class MusicMarketDbContext : DbContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public MusicMarketDbContext(DbContextOptions<MusicMarketDbContext> options) : base(options)
        {
            /*
            add-migration InitialCreate
            update-database
            ardından her güncellemede "add-migration migrationAdi" yazılıp sonra "update-database" yapılmalı
            bir önceki migration'a dönmek için "remove-migration" kullanılır
             */
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new MusicConfiguration());

            builder
                .ApplyConfiguration(new ArtistConfiguration());

            // configleri okunabilirlik açısından farklı class'larda yazıldı
        }
    }
}
