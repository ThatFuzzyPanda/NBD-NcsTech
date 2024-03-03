namespace NBDProjectNcstech.Models
{
    internal interface IApproval
    {
        string CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        string ApprovedBy { get; set; }
        DateTime? ApprovedOn { get; set; }
        string RejectedBy { get; set; }
        DateTime? RejectedOn { get; set; }
    }
}
