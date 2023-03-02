using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Authorization
{
    public class DictionaryValueAddInput
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long DictionaryTypeId { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public ComStatus Status { get; set; }
    }

    public class DictionaryValueUpdateInput : DictionaryValueAddInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required]
        public long Id { get; set; }
    }
}
