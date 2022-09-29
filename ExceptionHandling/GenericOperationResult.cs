using GenericProvisioningLib;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SparkDotNet.ExceptionHandling
{
    public struct TicketInformation
    {
        internal string RequestBody { get; set; }

        internal string RequestMethod { get; set; }

        internal string RequestUrl { get; set; }

        internal string TrackingId { get; set; }

        internal string ResponseBody { get; set; }

        internal string Token { get; set; }

        public override string ToString() => $"TrackingId:{TrackingId}\nMethod:{RequestMethod}\nRequestURL:{RequestUrl}\nRequestBody:{RequestBody}\nResponseBody:{ResponseBody}\n{Token}";
    }

    

    internal class FixSizeQueue<T>
    {
        public FixSizeQueue(int limit = 10)
        {
            Limit = limit;
        }

        private object @lock = new object();
        private ConcurrentQueue<T> q = new ConcurrentQueue<T>();

        public int Limit { get; set; }

        public void Enqueue(T obj)
        {
            q.Enqueue(obj);
            lock (@lock)
            {
                while (q.Count > Limit && q.TryDequeue(out _)) ;
            }
        }

        public T GetTicketInformation(int entryIndex) => q.ElementAt<T>(entryIndex);

        public T GetLastTicketInformation() => q.Last();

        public List<T> GetAllTicketInformation() => q.ToList();
    }

    public class TicketInformations
    {
        private readonly FixSizeQueue<TicketInformation> ticketInformations = new FixSizeQueue<TicketInformation>(10);

        public async Task FillRequestParameter(HttpResponseMessage response)
        {
            TicketInformation ti = new TicketInformation();
            ti.RequestUrl = response.RequestMessage.RequestUri.AbsoluteUri;
            ti.RequestMethod = response.RequestMessage.Method.Method;
            if(response.RequestMessage.Method != HttpMethod.Get)
            {
                ti.RequestBody = response.RequestMessage.Content != null ? await response.RequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false) : "Empty";
            }
            ti.TrackingId = response.Headers.GetValues(nameof(TicketInformation.TrackingId)).FirstOrDefault();
            ti.ResponseBody = response.Content != null ? await response.Content.ReadAsStringAsync().ConfigureAwait(false) : "Empty";
            ti.Token = response.RequestMessage.Headers.GetValues("Authorization")?.FirstOrDefault();
            ticketInformations.Enqueue(ti);
        }

        public async Task<TicketInformation> GetTicketInformation(int entryIndex) => await Task.Run(() => ticketInformations.GetTicketInformation(entryIndex)).ConfigureAwait(false);

        public async Task<List<TicketInformation>> GetAllTicketInformation() => await Task.Run(() => ticketInformations.GetAllTicketInformation()).ConfigureAwait(false);

        public async Task<TicketInformation> GetLastTicketInformation() => await Task.Run(() => ticketInformations.GetLastTicketInformation()).ConfigureAwait(false);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(ticketInformations.Limit);
            foreach (var item in ticketInformations.GetAllTicketInformation())
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }

        public async Task DumpToConsole() => await Task.Run(() => ticketInformations.GetAllTicketInformation().ForEach(t => System.Console.WriteLine($"{t}\n"))).ConfigureAwait(false);
    }
}