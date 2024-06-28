namespace MusicalityLabs.Storage.DataAccess.SoundSignatures
{
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;
    using Api.Contracts.SoundSignatures;
    using Byndyusoft.Data.Relational;

    public class SoundSignaturesRepository : DbSessionConsumer
    {
        public SoundSignaturesRepository(IDbSessionAccessor sessionAccessor) : base(sessionAccessor)
        {
        }

        public Task<SoundSignature> Save(SoundSignature soundSignature, CancellationToken cancellationToken)
            => DbSession.QuerySingleAsync<SoundSignature>(
                @"
insert into public.sound_signatures
(   
    id,
    name
)
values
(
    @Id
,   @Name
)
on conflict (id) do update
set name = excluded.name
returning *;
",
                soundSignature,
                cancellationToken: cancellationToken
            );
    }
}