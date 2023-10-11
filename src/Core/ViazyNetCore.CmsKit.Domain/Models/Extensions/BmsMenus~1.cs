using System;
using System.Collections.Generic;
using System.Text;

namespace ViazyNetCore.CmsKit.Models
{
    public partial class BmsMenus : IEquatable<BmsMenus>
    {
        public bool Equals(BmsMenus other)
        {
            if (other is null)
                return false;

            return this.Id == other.Id && this.ProjectId == other.ProjectId;
        }

        public override bool Equals(object obj) => Equals(obj as BmsMenus);
        public override int GetHashCode() => (Id, Name).GetHashCode();
    }
}
