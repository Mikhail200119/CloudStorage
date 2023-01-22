using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories;

public class ThumbnailInfoRepository : EfRepository<ThumbnailInfoDbModel>, IThumbnailInfoRepository
{
    public ThumbnailInfoRepository(DbContext context) : base(context)
    {
    }
}