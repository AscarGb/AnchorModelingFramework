using Microsoft.EntityFrameworkCore;

namespace AnchorModeling.QueryExtensions
{
    public static class AnchorModelSetExtensions
    {
        public static AnchorModelSet<TEntity> GetAnchorModelSet<TEntity>(this DbContext context) =>
            new AnchorModelSet<TEntity>
            {
                AnchorSetDataList = AnchorEntitiesFinder.FindAnchorEntities(typeof(TEntity), context)
            };
    }
}