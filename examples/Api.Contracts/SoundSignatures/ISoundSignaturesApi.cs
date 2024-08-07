namespace MusicalityLabs.Storage.Api.Contracts.SoundSignatures
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISoundSignaturesApi
    {
        Task<SoundSignature> Save(SoundSignature soundSignature, CancellationToken cancellationToken);
    }
}