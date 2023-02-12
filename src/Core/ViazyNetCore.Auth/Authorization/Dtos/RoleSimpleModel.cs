using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个简单的角色模型。
    /// </summary>
    public class RoleSimpleModel
    {
        /// <summary>
        /// 设置或获取一个值，表示编号。
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        [MaxLength(50), Required]
        public string Name { get; set; }
    }
}
