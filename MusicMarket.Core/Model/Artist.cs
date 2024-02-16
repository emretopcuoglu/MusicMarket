using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MusicMarket.Core.Model
{
    public class Artist
    {
        public Artist()
        {
            Musics = new Collection<Music>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Music> Musics { get; set; }
    }
}
