using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NStore.Core.InMemory;
using NStore.Sample.Domain.Room;
using NStore.Sample.Support;

namespace NStore.Sample.Projections
{
    public class RoomsOnSaleProjection : AbstractProjection
    {
        private readonly IReporter _reporter;
        private readonly INetworkSimulator _networkSimulator;

        public class RoomsOnSale
        {
            public string Id { get; set; }
            public string RoomNumber { get; set; }
        }
        private readonly IDictionary<string, RoomsOnSale> _all = new Dictionary<string, RoomsOnSale>();

        public RoomsOnSaleProjection(IReporter reporter, INetworkSimulator networkSimulator)
        {
            _reporter = reporter;
            _networkSimulator = networkSimulator;
        }

        public IEnumerable<RoomsOnSale> List => _all.Values.OrderBy(x=>x.Id);

        public async Task On(BookingsEnabled e)
        {
            _all.Add(e.Id, new RoomsOnSale { Id = e.Id });
            var elapsed = await _networkSimulator.WaitFast().ConfigureAwait(false);
            this._reporter.Report($"Room available {e.Id} took {elapsed}ms");
        }
    }
}