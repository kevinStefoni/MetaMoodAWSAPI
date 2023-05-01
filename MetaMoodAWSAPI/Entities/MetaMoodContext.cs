using Microsoft.EntityFrameworkCore;

namespace MetaMoodAWSAPI.Entities;

public partial class MetaMoodContext : DbContext
{
    public MetaMoodContext()
    {
    }

    public MetaMoodContext(DbContextOptions<MetaMoodContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SpotifyTrack> SpotifyTracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<SpotifyTrack>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("PRIMARY");

            entity.ToTable("spotify_track");

            entity.Property(e => e.TrackId)
                .HasMaxLength(64)
                .HasColumnName("track_id");
            entity.Property(e => e.Acousticness).HasColumnName("acousticness");
            entity.Property(e => e.AlbumId)
                .HasColumnType("text")
                .HasColumnName("album_id");
            entity.Property(e => e.CoverImageUrl)
                .HasColumnType("text")
                .HasColumnName("cover_image_url");
            entity.Property(e => e.Danceability).HasColumnName("danceability");
            entity.Property(e => e.Energy).HasColumnName("energy");
            entity.Property(e => e.Instrumentalness).HasColumnName("instrumentalness");
            entity.Property(e => e.Liveness).HasColumnName("liveness");
            entity.Property(e => e.Loudness).HasColumnName("loudness");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Popularity).HasColumnName("popularity");
            entity.Property(e => e.PreviewUrl)
                .HasColumnType("text")
                .HasColumnName("preview_url");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("text")
                .HasColumnName("release_date");
            entity.Property(e => e.Speechiness).HasColumnName("speechiness");
            entity.Property(e => e.Tempo).HasColumnName("tempo");
            entity.Property(e => e.TrackHref)
                .HasColumnType("text")
                .HasColumnName("track_href");
            entity.Property(e => e.Valence).HasColumnName("valence");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
