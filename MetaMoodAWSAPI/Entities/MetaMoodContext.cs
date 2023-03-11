using System;
using System.Collections.Generic;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=ls-249ad327ce44da9463f737ece7b6de0d2b258dc1.cjdpzq5pew4s.us-east-2.rds.amazonaws.com;port=3306;user=root;password=lsdmCS4243;database=meta_mood", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

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
