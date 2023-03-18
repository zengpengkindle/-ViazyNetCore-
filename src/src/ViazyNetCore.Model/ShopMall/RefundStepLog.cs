
namespace ViazyNetCore.Model
{
    /// <summary>
    /// �˻������̼�¼
    /// </summary>
    [Table(Name = "ShopMall.RefundStepLog")]
    public partial class RefundStepLog : EntityBase<string>
    {
        /// <summary>
        /// ��Ӧ�˻���-�Ӷ������
        /// </summary>
        public string RefundTradeId { get; set; }

        /// <summary>
        /// ���̼���
        /// </summary>
        public int StepIndex { get; set; }

        /// <summary>
        /// �������̱��
        /// </summary>
        public string StepId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// ������ʾ
        /// </summary>
        public string Remind { get; set; }

        /// <summary>
        /// ����˵��
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// ������-���û�ʱΪmemberId �̼�ʱΪshopId��
        /// </summary>
        public string HandleUserId { get; set; }

        /// <summary>
        /// ����������-0�̼�1�û�
        /// </summary>
        public RefundTradeLogType HandleUserType { get; set; }

        /// <summary>
        /// ��һ�����̱��
        /// </summary>
        public string PreStepLogId { get; set; }

        /// <summary>
        /// ��һ�����̱��
        /// </summary>
        public string NextStepLogId { get; set; }

        /// <summary>
        /// ���µĴ������̼�¼���
        /// </summary>
        public string NewStepLogId { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? HandleTime { get; set; }

    }
}
