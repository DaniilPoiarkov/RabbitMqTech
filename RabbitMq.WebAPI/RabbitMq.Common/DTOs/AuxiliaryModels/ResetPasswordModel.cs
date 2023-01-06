namespace RabbitMq.Common.DTOs.AuxiliaryModels
{
    public class ResetPasswordModel
    {
        public string Email { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
