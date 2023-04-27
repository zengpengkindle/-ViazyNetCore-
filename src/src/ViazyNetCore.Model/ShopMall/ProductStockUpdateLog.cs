namespace ViazyNetCore.Model
{
    /// <summary>
    /// ��Ʒ�ڿ�������¼��ֻ��¼��̨�����޸���ֵ��
    /// </summary>
    [Table(Name = "ShopMall.ProductStockUpdateLog")]
    public partial class ProductStockUpdateLog : EntityBase<string>
    {
        /// <summary>
        /// ����¼���
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// �ڿ������ֵ
        /// </summary>
        public int OldInStock { get; set; }

        /// <summary>
        /// �ڿ�������ֵ
        /// </summary>
        public int NewInStock { get; set; }

        /// <summary>
        /// �ڿ���䶯��ֵ
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// �޸���
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// ��ע��Ϣ
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// ��¼����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
