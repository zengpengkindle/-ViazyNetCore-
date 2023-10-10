using FreeSql.DataAnnotations;

namespace ViazyNetCore.IdentityService4.FreeSql.Entities
{
    public abstract class UserClaim : Entity<long>
    {
        [Column(StringLength = 200, IsNullable = false)]
        public string Type { get; set; }
    }
}
