using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization
{
    public partial class BmsOwnerPermission : IEquatable<BmsOwnerPermission>
    {
        public bool Equals(BmsOwnerPermission other)
        {
            if (other is null)
                return false;

            return this.Id == other.Id && this.OwnerId == other.OwnerId && this.OwnerType == other.OwnerType;
        }

        public override bool Equals(object obj) => Equals(obj as BmsOwnerPermission);
        public override int GetHashCode() => (Id, PermissionItemKey).GetHashCode();
    }
}
