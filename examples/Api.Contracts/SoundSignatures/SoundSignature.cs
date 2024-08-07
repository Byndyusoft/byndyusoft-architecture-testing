namespace MusicalityLabs.Storage.Api.Contracts.SoundSignatures
{
    using System.ComponentModel.DataAnnotations;

    public class SoundSignature
    {
        [Required]
        public int? Id { get; set; }

        [Required] 
        public string Name { get; set; }
    }
}