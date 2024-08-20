namespace MusicalityLabs.Storage.Api.Clients
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Byndyusoft.ApiClient;
    using Contracts.SoundSignatures;
    using Microsoft.Extensions.Options;

    public class SoundSignaturesApiClient : BaseClient, ISoundSignaturesApi
    {
        private const string ApiRoutePrefix = "api/soundSignatures";

        public SoundSignaturesApiClient(HttpClient client, IOptions<StorageApiSettings> apiSettings)
            : base(client, apiSettings)
        {
        }

        public Task<SoundSignature> Save(SoundSignature soundSignature, CancellationToken cancellationToken)
            => PostAsync<SoundSignature>(ApiRoutePrefix, soundSignature, cancellationToken);
    }
}